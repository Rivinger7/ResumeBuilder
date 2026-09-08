using AngleSharp.Html.Parser;
using QuestPDF.Fluent;
using HtmlElement = AngleSharp.Dom.IElement;
using HtmlNode = AngleSharp.Dom.INode;
using HtmlText = AngleSharp.Dom.IText;
using HtmlDocument = AngleSharp.Dom.IDocument;

namespace ResumeBuilder.Infrastructure.PDFs;

/// <summary>
/// Renders the HTML produced by the FE's rich-text editor (Tiptap — bold, italic,
/// underline, ordered/bullet lists, links, paragraph alignment) into a QuestPDF column,
/// instead of flattening it to plain text. Scoped to exactly what the editor's toolbar can
/// produce — not a general-purpose HTML-to-PDF renderer.
/// </summary>
internal static class HtmlRichTextRenderer
{
    private static readonly HtmlParser Parser = new();

    public static void Render(ColumnDescriptor column, string? html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return;
        }

        HtmlDocument document = Parser.ParseDocument($"<body>{html}</body>");
        foreach (HtmlNode node in document.Body!.ChildNodes)
        {
            RenderBlock(column, node);
        }
    }

    private static void RenderBlock(ColumnDescriptor column, HtmlNode node)
    {
        if (node is HtmlElement element)
        {
            switch (element.TagName)
            {
                case "UL":
                    RenderList(column, element, ordered: false);
                    return;
                case "OL":
                    RenderList(column, element, ordered: true);
                    return;
                default:
                    RenderParagraph(column, element);
                    return;
            }
        }

        if (node is HtmlText text && !string.IsNullOrWhiteSpace(text.Data))
        {
            // Stray text not wrapped in a block element — render as its own line.
            column.Item().Text(text.Data.Trim());
        }
    }

    private static void RenderParagraph(ColumnDescriptor column, HtmlElement element)
    {
        // Skips empty paragraphs, e.g. the trailing "<p><br></p>" ProseMirror leaves behind.
        if (string.IsNullOrWhiteSpace(element.TextContent))
        {
            return;
        }

        string? alignment = GetTextAlign(element);

        column.Item().Text(text =>
        {
            switch (alignment)
            {
                case "center":
                    text.AlignCenter();
                    break;
                case "right":
                    text.AlignRight();
                    break;
                case "justify":
                    text.Justify();
                    break;
            }

            RenderInline(text, element, bold: false, italic: false, underline: false, link: null);
        });
    }

    private static void RenderList(ColumnDescriptor column, HtmlElement listElement, bool ordered)
    {
        int index = 1;
        foreach (HtmlElement item in listElement.Children.Where(c => c.TagName == "LI"))
        {
            string prefix = ordered ? $"{index}.  " : "•  ";
            column.Item().Text(text =>
            {
                text.Span(prefix);
                RenderInline(text, item, bold: false, italic: false, underline: false, link: null);
            });
            index++;
        }
    }

    private static void RenderInline(TextDescriptor text, HtmlNode node, bool bold, bool italic, bool underline, string? link)
    {
        foreach (HtmlNode child in node.ChildNodes)
        {
            if (child is HtmlText textNode)
            {
                if (string.IsNullOrEmpty(textNode.Data))
                {
                    continue;
                }

                TextSpanDescriptor span = link is null ? text.Span(textNode.Data) : text.Hyperlink(textNode.Data, link);
                if (bold)
                {
                    span.Bold();
                }
                if (italic)
                {
                    span.Italic();
                }
                if (underline)
                {
                    span.Underline();
                }

                continue;
            }

            if (child is not HtmlElement element)
            {
                continue;
            }

            switch (element.TagName)
            {
                case "STRONG":
                case "B":
                    RenderInline(text, element, true, italic, underline, link);
                    break;
                case "EM":
                case "I":
                    RenderInline(text, element, bold, true, underline, link);
                    break;
                case "U":
                    RenderInline(text, element, bold, italic, true, link);
                    break;
                case "A":
                    RenderInline(text, element, bold, italic, underline, element.GetAttribute("href") ?? link);
                    break;
                case "BR":
                    text.EmptyLine();
                    break;
                default:
                    RenderInline(text, element, bold, italic, underline, link);
                    break;
            }
        }
    }

    private static string? GetTextAlign(HtmlElement element)
    {
        string style = element.GetAttribute("style") ?? string.Empty;
        int index = style.IndexOf("text-align", StringComparison.OrdinalIgnoreCase);
        if (index < 0)
        {
            return null;
        }

        int colon = style.IndexOf(':', index);
        if (colon < 0)
        {
            return null;
        }

        int semicolon = style.IndexOf(';', colon);
        string value = (semicolon < 0 ? style[(colon + 1)..] : style[(colon + 1)..semicolon]).Trim().ToLowerInvariant();
        return value is "center" or "right" or "justify" ? value : null;
    }
}
