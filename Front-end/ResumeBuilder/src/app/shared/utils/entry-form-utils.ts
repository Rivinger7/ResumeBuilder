// Shared helpers used by every per-section entry editor when diffing form values
// against the original entry and mapping <input type="month"> to/from ISO dates.

export function toMonthInput(isoDate: string | null | undefined): string {
  if (!isoDate) return '';
  return isoDate.slice(0, 7);
}

export function fromMonthInput(monthValue: string): string | null {
  if (!monthValue) return null;
  return `${monthValue}-01`;
}

// '' is treated as empty -> send null to BE instead of an empty string.
export function normalizeEmpty(value: unknown): unknown {
  return value === '' ? null : value;
}

// '' and null/undefined are treated as equivalent — avoids false positives when the
// original field is null but the form patches it to '' via `?? ''`.
export function isEqualFormValue(a: unknown, b: unknown): boolean {
  const normalize = (v: unknown) => (v === '' || v == null ? null : v);
  return normalize(a) === normalize(b);
}

/** True if any of the given fields differ from `original` (always true when original is null — create mode). */
export function hasFieldChanges<TForm extends object>(
  current: TForm,
  original: TForm | null,
  fields: (keyof TForm)[],
): boolean {
  if (original === null) return true;
  return fields.some((field) => !isEqualFormValue(current[field], original[field]));
}

/**
 * Diffs `current` against `original` and returns only changed fields.
 * In create mode (original null) every mapped field is treated as changed.
 */
export function buildPartialPayload<TForm extends object, TRequest extends object>(
  current: TForm,
  original: TForm | null,
  fieldMap: { [K in keyof TRequest]?: keyof TForm },
  transform?: Partial<Record<keyof TForm, (raw: TForm[keyof TForm]) => unknown>>,
): Partial<TRequest> {
  const isCreate = original === null;
  const payload: Partial<TRequest> = {};

  for (const backendKey of Object.keys(fieldMap) as (keyof TRequest)[]) {
    const formField = fieldMap[backendKey];
    if (!formField) continue;

    const currentValue = current[formField];
    const originalValue = original?.[formField];
    const changed = isCreate || !isEqualFormValue(currentValue, originalValue);

    if (!changed) continue;

    const apply = transform?.[formField];
    payload[backendKey] = (
      apply ? apply(currentValue) : normalizeEmpty(currentValue)
    ) as TRequest[keyof TRequest];
  }

  return payload;
}
