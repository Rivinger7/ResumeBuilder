const HTML_TAG_PATTERN = /<[a-z][\s\S]*>/i;

/**
 * ngx-editor stores/emits description fields as an HTML string. Entries saved before the
 * rich-text editor existed hold plain text with literal '\n' line breaks, which HTML
 * ignores — convert those to <br> so legacy content still displays multi-line instead of
 * collapsing into a single run-on line, both inside the editor and in the live preview.
 */
export function toEditorHtml(value: string): string {
  if (!value || HTML_TAG_PATTERN.test(value)) {
    return value;
  }

  const escaped = value.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
  return escaped.replace(/\r?\n/g, '<br>');
}
