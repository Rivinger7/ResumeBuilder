import { Component, OnDestroy, OnInit, forwardRef, input, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Editor } from '@tiptap/core';
import StarterKit from '@tiptap/starter-kit';
import TextAlign from '@tiptap/extension-text-align';
import Placeholder from '@tiptap/extension-placeholder';
import { TiptapEditorDirective } from 'ngx-tiptap';
import { LucideIconModule } from '../../utils/lucide-icon-module';

type TextAlignValue = 'left' | 'center' | 'right' | 'justify';

// Rich-text input with a formatting toolbar, usable via formControlName like any other
// input. Wraps a tiptap Editor and implements ControlValueAccessor itself rather than
// relying on TiptapEditorDirective's own accessor, so the toolbar can live alongside it
// as a single reusable field instead of being copy-pasted into every entry editor.
@Component({
  selector: 'app-rich-text-field',
  imports: [TiptapEditorDirective, LucideIconModule],
  templateUrl: './rich-text-field.html',
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => RichTextField), multi: true },
  ],
})
export class RichTextField implements ControlValueAccessor, OnInit, OnDestroy {
  readonly label = input.required<string>();

  editor!: Editor;
  readonly activeFormats = signal<ReadonlySet<string>>(new Set());

  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};
  private pendingValue = '';
  private ready = false;

  ngOnInit(): void {
    this.editor = new Editor({
      // Underline/Link ship as part of StarterKit — configuring them here instead of
      // adding separate extension instances avoids duplicate marks in the schema, which
      // otherwise corrupts click/selection mark resolution for unrelated marks too.
      extensions: [
        StarterKit.configure({
          link: { openOnClick: false, autolink: true },
        }),
        TextAlign.configure({ types: ['paragraph', 'heading'] }),
        Placeholder.configure({ placeholder: 'Enter your description...' }),
      ],
      content: this.pendingValue,
    });
    this.ready = true;
    this.editor.on('selectionUpdate', () => this.updateActiveFormats());
    this.editor.on('transaction', () => this.updateActiveFormats());
    this.editor.on('update', () => this.onChange(this.editor.getHTML()));
    this.editor.on('blur', () => this.onTouched());
  }

  ngOnDestroy(): void {
    this.editor?.destroy();
  }

  writeValue(value: string | null): void {
    this.pendingValue = value ?? '';
    if (this.ready && this.editor.getHTML() !== this.pendingValue) {
      this.editor.chain().setContent(this.pendingValue, { emitUpdate: false }).run();
    }
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.editor?.setEditable(!isDisabled);
  }

  private updateActiveFormats(): void {
    const formats = new Set<string>();
    for (const mark of ['bold', 'italic', 'underline', 'link'] as const) {
      if (this.editor.isActive(mark)) formats.add(mark);
    }
    if (this.editor.isActive('orderedList')) formats.add('orderedList');
    if (this.editor.isActive('bulletList')) formats.add('bulletList');
    for (const align of ['left', 'center', 'right', 'justify'] as const) {
      if (this.editor.isActive({ textAlign: align })) formats.add(`align-${align}`);
    }
    this.activeFormats.set(formats);
  }

  toggleBold(): void {
    this.editor.chain().focus().toggleBold().run();
  }

  toggleItalic(): void {
    this.editor.chain().focus().toggleItalic().run();
  }

  toggleUnderline(): void {
    this.editor.chain().focus().toggleUnderline().run();
  }

  toggleOrderedList(): void {
    this.editor.chain().focus().toggleOrderedList().run();
  }

  toggleBulletList(): void {
    this.editor.chain().focus().toggleBulletList().run();
  }

  setTextAlign(align: TextAlignValue): void {
    this.editor.chain().focus().setTextAlign(align).run();
  }

  toggleLink(): void {
    if (this.editor.isActive('link')) {
      this.editor.chain().focus().extendMarkRange('link').unsetLink().run();
      return;
    }

    const url = window.prompt('URL');
    if (!url) return;
    this.editor.chain().focus().extendMarkRange('link').setLink({ href: url }).run();
  }

  clearFormatting(): void {
    this.editor.chain().focus().unsetAllMarks().clearNodes().run();
  }
}
