# Project Instructions

## Tech Stack

- Angular 21 (standalone components)
- Angular CDK 21
- Angular Material 21
- TailwindCSS (+ tailwindcss-primeui plugin)
- lucide-angular (icons)
- @ngx-env/builder (.env config)

## Component Philosophy

Components should:

- focus on presentation first, but may contain logic as needed
- remain visually clean
- stay readable and maintainable

## Preferred Structure

Follow the [Angular Tips folder structure](https://ngtips.com/general/folder-structure): group by domain/feature, not by technical type.

```
src/app/
  core/           ← non-business, global, instantiated once
    auth/
    layout/
      nav-bar/
      page-layout/
    interceptors/
  features/       ← business features grouped by domain
    dashboard/
    blog/
      feed/
      post/
        comment/
      blog.routes.ts
  shared/         ← reusable, no business logic
    components/
    directives/
    models/
    pipes/
    services/
```

Rules:

- Colocate files belonging to the same feature in the same folder (component + template + styles + store + http client together)
- `core` must never import from `features`
- `shared` must never import from `core` or `features`
- Give each multi-route domain a dedicated `<feature>.routes.ts`
- Rename folders to avoid redundant path segments (`blog/post/blog-post.ts`, not `blog/blog-post/blog-post.ts`)

## Angular Guidelines

- Prefer standalone components
- Use signals where appropriate for local component state
- Keep templates clean; avoid deeply nested conditional logic in HTML
- Services for business logic, API integration, and stores are expected and encouraged when the feature needs them
- Avoid premature abstraction — don't create a service/store until there's an actual need for shared state or reused logic

## Simplicity Rules

Prioritize:

- visual quality
- readability
- maintainability

Avoid:

- unnecessary abstractions
- overengineered architecture for simple features
- premature optimization

When building with mock/placeholder data during UI-first development, keep it as simple static arrays/constants — don't simulate full backend behavior unless explicitly asked.

Prefer:

- simpler code
- cleaner templates
- better spacing
- stronger visual polish

## Styling

- Use TailwindCSS utility classes as the default and primary styling approach for all components
- Do NOT create a component `.css`/`.scss` file by default — inline Tailwind classes in the template first
- Only create a component stylesheet when Tailwind genuinely cannot express it (e.g. complex keyframe animations, `::ng-deep` overrides needed for deep Material internals, or dynamic values that can't be utility classes) — and keep it minimal, scoped to just that exception
- Use Angular CDK primitives (Overlay, A11y, Drag&Drop) for custom interactive behavior instead of reinventing positioning/focus logic
- Use Angular Material components where a ready-made accessible component fits, styled/themed to match the design
- Use lucide-angular for icons
