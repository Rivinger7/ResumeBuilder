import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-entry-editor-actions',
  templateUrl: './entry-editor-actions.html',
})
export class EntryEditorActions {
  readonly disabled = input(false); // Todo: Add UI for this
  readonly cancel = output<void>();
  readonly save = output<void>();
}
