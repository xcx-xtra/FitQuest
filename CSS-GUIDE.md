# FitQuest CSS Design System Guide

This guide provides comprehensive documentation for the FitQuest CSS design system, which replaces traditional CSS frameworks with a modern CSS variable-based approach.

## Overview

The FitQuest CSS design system is built on CSS custom properties (variables) and follows a component-based architecture. This approach provides:

- **Better Performance**: No external CSS framework to download
- **Easy Theming**: Change CSS variables to create new themes
- **Maintainability**: Centralized design tokens and consistent patterns
- **Modern CSS**: Uses latest CSS features like Grid, Flexbox, and custom properties
- **Zero Dependencies**: No external CSS frameworks required

## Architecture

### File Structure

```
wwwroot/css/
├── variables.css      # CSS design system variables
├── base.css          # Reset, typography, and foundational elements
├── components.css    # Reusable UI components
├── layout.css        # Grid systems and layout utilities
├── utilities.css     # Minimal utility classes
└── app.css          # Main application styles (imports all others)
```

### Design Tokens

All design decisions are encoded as CSS custom properties in `variables.css`:

#### Color System

```css
:root {
  /* Primary Colors */
  --color-primary: #1b6ec2;
  --color-primary-hover: #1861ac;
  --color-primary-focus: #0d47a1;

  /* Secondary Colors */
  --color-secondary: #6c757d;
  --color-secondary-hover: #5a6268;

  /* Semantic Colors */
  --color-success: #26b050;
  --color-success-hover: #1e8e3e;
  --color-danger: #dc3545;
  --color-danger-hover: #c82333;
  --color-warning: #ffc107;
  --color-warning-hover: #e0a800;
  --color-info: #17a2b8;
  --color-info-hover: #138496;

  /* Neutral Colors */
  --color-white: #ffffff;
  --color-light: #f8f9fa;
  --color-gray-100: #f1f3f4;
  --color-gray-200: #e9ecef;
  --color-gray-300: #dee2e6;
  --color-gray-400: #ced4da;
  --color-gray-500: #adb5bd;
  --color-gray-600: #6c757d;
  --color-gray-700: #495057;
  --color-gray-800: #343a40;
  --color-gray-900: #212529;
  --color-dark: #343a40;
  --color-black: #000000;
}
```

#### Typography Scale

```css
:root {
  /* Font Families */
  --font-family-primary: "Inter", -apple-system, BlinkMacSystemFont, "Segoe UI",
    sans-serif;
  --font-family-mono: "SF Mono", Monaco, "Cascadia Code", "Roboto Mono",
    Consolas, "Courier New", monospace;

  /* Font Sizes */
  --font-size-xs: 0.75rem; /* 12px */
  --font-size-sm: 0.875rem; /* 14px */
  --font-size-base: 1rem; /* 16px */
  --font-size-lg: 1.125rem; /* 18px */
  --font-size-xl: 1.25rem; /* 20px */
  --font-size-2xl: 1.5rem; /* 24px */
  --font-size-3xl: 1.875rem; /* 30px */
  --font-size-4xl: 2.25rem; /* 36px */

  /* Font Weights */
  --font-weight-light: 300;
  --font-weight-normal: 400;
  --font-weight-medium: 500;
  --font-weight-semibold: 600;
  --font-weight-bold: 700;

  /* Line Heights */
  --line-height-tight: 1.25;
  --line-height-normal: 1.5;
  --line-height-relaxed: 1.75;
}
```

#### Spacing System

```css
:root {
  /* Spacing Scale (based on 0.25rem = 4px) */
  --spacing-0: 0;
  --spacing-1: 0.25rem; /* 4px */
  --spacing-2: 0.5rem; /* 8px */
  --spacing-3: 0.75rem; /* 12px */
  --spacing-4: 1rem; /* 16px */
  --spacing-5: 1.25rem; /* 20px */
  --spacing-6: 1.5rem; /* 24px */
  --spacing-8: 2rem; /* 32px */
  --spacing-10: 2.5rem; /* 40px */
  --spacing-12: 3rem; /* 48px */
  --spacing-16: 4rem; /* 64px */
  --spacing-20: 5rem; /* 80px */

  /* Semantic Spacing */
  --spacing-xs: var(--spacing-1);
  --spacing-sm: var(--spacing-2);
  --spacing-md: var(--spacing-4);
  --spacing-lg: var(--spacing-6);
  --spacing-xl: var(--spacing-8);
  --spacing-2xl: var(--spacing-12);
}
```

#### Design Elements

```css
:root {
  /* Border Radius */
  --radius-none: 0;
  --radius-sm: 0.25rem;
  --radius-md: 0.375rem;
  --radius-lg: 0.5rem;
  --radius-xl: 0.75rem;
  --radius-2xl: 1rem;
  --radius-full: 9999px;

  /* Shadows */
  --shadow-sm: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
  --shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
  --shadow-lg: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
  --shadow-xl: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
  --shadow-inner: inset 0 2px 4px 0 rgba(0, 0, 0, 0.06);

  /* Transitions */
  --transition-fast: 150ms ease-in-out;
  --transition-normal: 250ms ease-in-out;
  --transition-slow: 350ms ease-in-out;

  /* Z-Index Scale */
  --z-dropdown: 1000;
  --z-sticky: 1020;
  --z-fixed: 1030;
  --z-modal-backdrop: 1040;
  --z-modal: 1050;
  --z-popover: 1060;
  --z-tooltip: 1070;
}
```

## Component System

### Buttons

The button component system provides consistent styling across all interactive elements:

#### Base Button Classes

```css
.btn {
  /* Base button styles using design tokens */
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: var(--spacing-2) var(--spacing-4);
  border: 1px solid transparent;
  border-radius: var(--radius-md);
  font-size: var(--font-size-sm);
  font-weight: var(--font-weight-medium);
  line-height: var(--line-height-tight);
  text-decoration: none;
  cursor: pointer;
  transition: var(--transition-fast);
  user-select: none;
}
```

#### Button Variants

```html
<!-- Primary Actions -->
<button class="btn btn-primary">Save Changes</button>
<button class="btn btn-secondary">Cancel</button>

<!-- Semantic Actions -->
<button class="btn btn-success">Confirm</button>
<button class="btn btn-danger">Delete</button>
<button class="btn btn-warning">Warning</button>
<button class="btn btn-info">Info</button>

<!-- Outline Variants -->
<button class="btn btn-outline-primary">Secondary Action</button>
<button class="btn btn-outline-danger">Destructive Action</button>

<!-- Sizes -->
<button class="btn btn-primary btn-sm">Small Button</button>
<button class="btn btn-primary">Default Button</button>
<button class="btn btn-primary btn-lg">Large Button</button>

<!-- Link Style -->
<button class="btn btn-link">Link Button</button>
```

### Form Components

#### Form Groups and Labels

```html
<div class="form-group">
  <label class="form-label" for="goal-name">Goal Name</label>
  <input
    type="text"
    id="goal-name"
    class="form-control"
    placeholder="Enter your goal"
  />
  <div class="form-text">Choose a descriptive name for your fitness goal</div>
</div>
```

#### Input Variants

```html
<!-- Standard Input -->
<input type="text" class="form-control" placeholder="Standard input" />

<!-- Size Variants -->
<input
  type="text"
  class="form-control form-control-sm"
  placeholder="Small input"
/>
<input
  type="text"
  class="form-control form-control-lg"
  placeholder="Large input"
/>

<!-- States -->
<input type="text" class="form-control is-valid" placeholder="Valid input" />
<input
  type="text"
  class="form-control is-invalid"
  placeholder="Invalid input"
/>

<!-- Textarea -->
<textarea
  class="form-control"
  rows="3"
  placeholder="Enter description"
></textarea>

<!-- Select -->
<select class="form-control">
  <option>Choose option...</option>
  <option value="1">Option 1</option>
  <option value="2">Option 2</option>
</select>
```

#### Checkboxes and Radios

```html
<!-- Checkbox -->
<div class="form-check">
  <input type="checkbox" id="check1" class="form-check-input" />
  <label class="form-check-label" for="check1"> Enable notifications </label>
</div>

<!-- Radio Buttons -->
<div class="form-check">
  <input type="radio" id="radio1" name="options" class="form-check-input" />
  <label class="form-check-label" for="radio1"> Option 1 </label>
</div>
<div class="form-check">
  <input type="radio" id="radio2" name="options" class="form-check-input" />
  <label class="form-check-label" for="radio2"> Option 2 </label>
</div>
```

### Card Components

#### Basic Card Structure

```html
<div class="card">
  <div class="card-header">
    <h3 class="card-title">Card Title</h3>
    <p class="card-subtitle">Optional subtitle</p>
  </div>
  <div class="card-body">
    <p class="card-text">
      Card content goes here. You can include any HTML content.
    </p>
    <button class="btn btn-primary">Action Button</button>
  </div>
  <div class="card-footer">
    <small class="text-muted">Last updated 3 mins ago</small>
  </div>
</div>
```

#### Card Variants

```html
<!-- Themed Cards -->
<div class="card card-primary">...</div>
<div class="card card-success">...</div>
<div class="card card-danger">...</div>

<!-- Card with Image -->
<div class="card">
  <img src="image.jpg" class="card-img-top" alt="Card image" />
  <div class="card-body">
    <h5 class="card-title">Card with Image</h5>
    <p class="card-text">Some quick example text.</p>
  </div>
</div>
```

### Layout System

#### Grid System

The layout system is based on CSS Grid and Flexbox:

```html
<!-- Basic Row/Column Layout -->
<div class="row">
  <div class="col">
    <div class="card">Column 1</div>
  </div>
  <div class="col">
    <div class="card">Column 2</div>
  </div>
  <div class="col">
    <div class="card">Column 3</div>
  </div>
</div>

<!-- Responsive Columns -->
<div class="row">
  <div class="col-12 col-md-8">
    <div class="card">Main Content</div>
  </div>
  <div class="col-12 col-md-4">
    <div class="card">Sidebar</div>
  </div>
</div>

<!-- Fixed Width Columns -->
<div class="row">
  <div class="col-3">Quarter width</div>
  <div class="col-6">Half width</div>
  <div class="col-3">Quarter width</div>
</div>
```

#### Container System

```html
<!-- Fluid Container (full width) -->
<div class="container-fluid">
  <div class="row">...</div>
</div>

<!-- Fixed Container (max-width responsive) -->
<div class="container">
  <div class="row">...</div>
</div>
```

## Utility Classes

### Spacing Utilities

```html
<!-- Margin -->
<div class="m-0">No margin</div>
<div class="m-1">Small margin</div>
<div class="m-4">Default margin</div>
<div class="mx-2">Horizontal margin</div>
<div class="my-3">Vertical margin</div>
<div class="mt-4">Top margin</div>

<!-- Padding -->
<div class="p-2">Small padding</div>
<div class="p-4">Default padding</div>
<div class="px-3">Horizontal padding</div>
<div class="py-2">Vertical padding</div>
```

### Text Utilities

```html
<!-- Alignment -->
<p class="text-left">Left aligned text</p>
<p class="text-center">Center aligned text</p>
<p class="text-right">Right aligned text</p>

<!-- Colors -->
<p class="text-primary">Primary text</p>
<p class="text-success">Success text</p>
<p class="text-danger">Danger text</p>
<p class="text-muted">Muted text</p>

<!-- Sizes -->
<p class="text-sm">Small text</p>
<p class="text-lg">Large text</p>

<!-- Weight -->
<p class="fw-light">Light weight</p>
<p class="fw-normal">Normal weight</p>
<p class="fw-bold">Bold weight</p>
```

### Display Utilities

```html
<!-- Display -->
<div class="d-none">Hidden</div>
<div class="d-block">Block</div>
<div class="d-flex">Flex container</div>
<div class="d-grid">Grid container</div>

<!-- Responsive Display -->
<div class="d-none d-md-block">Hidden on mobile, visible on tablet+</div>
<div class="d-block d-md-none">Visible on mobile, hidden on tablet+</div>
```

## Responsive Design

### Breakpoint System

```css
/* Mobile First Approach */
:root {
  --breakpoint-sm: 576px; /* Small devices (landscape phones) */
  --breakpoint-md: 768px; /* Medium devices (tablets) */
  --breakpoint-lg: 992px; /* Large devices (desktops) */
  --breakpoint-xl: 1200px; /* Extra large devices (large desktops) */
  --breakpoint-xxl: 1400px; /* Extra extra large devices */
}
```

### Responsive Utilities

```html
<!-- Responsive Text Alignment -->
<p class="text-center text-md-left">Center on mobile, left on tablet+</p>

<!-- Responsive Margins/Padding -->
<div class="p-2 p-md-4">Small padding on mobile, larger on tablet+</div>

<!-- Responsive Columns -->
<div class="row">
  <div class="col-12 col-sm-6 col-lg-4">Responsive column</div>
</div>
```

## Theming and Customization

### Creating Custom Themes

To create a custom theme, override CSS variables:

```css
/* Dark Theme Example */
:root {
  --color-primary: #6366f1;
  --color-background: #1f2937;
  --color-surface: #374151;
  --color-text: #f9fafb;
  --color-text-muted: #d1d5db;
}

/* Custom Brand Theme */
:root {
  --color-primary: #your-brand-color;
  --color-secondary: #your-secondary-color;
  --font-family-primary: "Your Brand Font", sans-serif;
  --radius-md: 0.75rem; /* More rounded corners */
}
```

### Component Customization

Override component styles while maintaining the design system:

```css
/* Custom Card Variant */
.card-custom {
  background: linear-gradient(
    135deg,
    var(--color-primary),
    var(--color-secondary)
  );
  color: var(--color-white);
  border: none;
}

/* Custom Button Variant */
.btn-custom {
  background-color: var(--color-warning);
  border-color: var(--color-warning);
  color: var(--color-dark);
}

.btn-custom:hover {
  background-color: var(--color-warning-hover);
  border-color: var(--color-warning-hover);
}
```

## Best Practices

### Naming Conventions

1. **Use semantic class names**: `.btn-primary` instead of `.btn-blue`
2. **Follow BEM methodology**: `.card__header`, `.card__title`
3. **Use consistent prefixes**: All utilities start with their type (`.text-`, `.m-`, `.p-`)

### Performance Optimization

1. **Minimize CSS specificity**: Use single class names when possible
2. **Leverage CSS variables**: Allows browser optimization and easy theming
3. **Avoid !important**: Design the system to avoid specificity wars
4. **Use efficient selectors**: Prefer classes over complex selectors

### Maintenance Guidelines

1. **Keep design tokens in sync**: Always update variables.css first
2. **Document component variants**: Add comments for complex components
3. **Test across browsers**: Ensure CSS variable support
4. **Maintain consistency**: Follow established patterns for new components

### Accessibility

1. **Color contrast**: Ensure sufficient contrast ratios for all color combinations
2. **Focus states**: Provide clear focus indicators for interactive elements
3. **Motion**: Respect `prefers-reduced-motion` media query
4. **Typography**: Use relative units and readable line heights

```css
/* Accessibility Examples */
@media (prefers-reduced-motion: reduce) {
  * {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}

/* High contrast mode support */
@media (prefers-contrast: high) {
  :root {
    --color-primary: #0000ff;
    --color-danger: #ff0000;
    --color-success: #008000;
  }
}
```

## Migration Guide

### From Bootstrap

1. **Replace utility classes**:

   - `mb-3` → `mb-3` (spacing utilities are similar)
   - `text-primary` → `text-primary` (semantic colors maintained)
   - `btn btn-primary` → `btn btn-primary` (button classes similar)

2. **Update grid system**:

   - Bootstrap classes mostly work the same
   - Some advanced features may need custom CSS

3. **Convert components**:
   - Cards: Structure remains similar
   - Forms: Form classes are compatible
   - Navigation: May need custom styling

### From Tailwind CSS

1. **Replace utility-first approach**:

   - `bg-blue-500` → Use `.btn-primary` or custom component class
   - `text-lg font-bold` → `.text-lg fw-bold`
   - `p-4 m-2` → `.p-4 m-2`

2. **Create component classes**:

   - Combine Tailwind utilities into semantic component classes
   - Use CSS variables for consistency

3. **Responsive prefixes**:
   - `md:text-left` → `.text-md-left`
   - `lg:p-8` → `.p-lg-8`

## Examples and Patterns

### Common UI Patterns

#### Dashboard Card

```html
<div class="card">
  <div class="card-header d-flex justify-content-between align-items-center">
    <h4 class="card-title mb-0">Weekly Progress</h4>
    <span class="badge bg-success">+12%</span>
  </div>
  <div class="card-body">
    <div class="row">
      <div class="col-6">
        <div class="text-center">
          <h2 class="text-primary mb-1">85%</h2>
          <p class="text-muted mb-0">Goal Progress</p>
        </div>
      </div>
      <div class="col-6">
        <div class="text-center">
          <h2 class="text-success mb-1">7</h2>
          <p class="text-muted mb-0">Days Active</p>
        </div>
      </div>
    </div>
  </div>
</div>
```

#### Form with Validation

```html
<form class="needs-validation" novalidate>
  <div class="form-group mb-3">
    <label class="form-label" for="goalName">Goal Name</label>
    <input type="text" id="goalName" class="form-control" required />
    <div class="invalid-feedback">Please provide a goal name.</div>
    <div class="valid-feedback">Looks good!</div>
  </div>

  <div class="form-group mb-3">
    <label class="form-label" for="targetValue">Target Value</label>
    <div class="input-group">
      <input type="number" id="targetValue" class="form-control" required />
      <span class="input-group-text">steps</span>
    </div>
  </div>

  <div class="d-grid gap-2 d-md-flex justify-content-md-end">
    <button type="button" class="btn btn-secondary">Cancel</button>
    <button type="submit" class="btn btn-primary">Save Goal</button>
  </div>
</form>
```

#### Navigation Component

```html
<nav class="navbar">
  <div class="container">
    <a class="navbar-brand" href="/">
      <img src="/logo.svg" alt="FitQuest" height="32" />
    </a>

    <ul class="navbar-nav">
      <li class="nav-item">
        <a class="nav-link" href="/dashboard">Dashboard</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="/goals">Goals</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="/leaderboard">Leaderboard</a>
      </li>
    </ul>

    <div class="navbar-actions">
      <button class="btn btn-outline-primary">Profile</button>
    </div>
  </div>
</nav>
```

This design system provides a solid foundation for building consistent, maintainable, and performant user interfaces in the FitQuest application.
