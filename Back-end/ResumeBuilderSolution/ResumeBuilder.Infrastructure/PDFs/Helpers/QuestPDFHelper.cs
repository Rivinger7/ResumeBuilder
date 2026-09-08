using QuestPDF.Fluent;

namespace ResumeBuilder.Infrastructure.PDFs.Helpers;

public static class QuestPDFHelper
{
    public static TextSpanDescriptor ApplyFontWeight(this TextSpanDescriptor text, int weight)
    {
        // CSS Value does not fit to QuestPDF value so need to plus 100 value into QuestPDF
        return weight switch
        {
            <= 300 => text.Light(),
            400 => text.NormalWeight(),
            500 => text.SemiBold(),
            600 => text.Bold(),
            700 => text.ExtraBold(),
            >= 800 => text.Black(),
            _ => text.NormalWeight()
        };
    }
}

