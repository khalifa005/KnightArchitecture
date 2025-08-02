# Global Table Styles Guide

This guide explains how to use the global table styles defined in `src/styles/table-styles.less` for consistent data table styling across the application.

## Quick Start

To use the global table styles in any component:

1. **Import the styles** (already done globally in `src/styles/index.less`)
2. **Use the container class**: `data-table-container`
3. **Apply button classes**: `primary-action-btn`, `success-action-btn`, `danger-action-btn`, etc.
4. **Use status classes**: `status-tag`, `role-tag`, `status-active`, etc.

## Basic Table Structure

```html
<div class="data-table-container">
  <!-- Search Bar -->
  <div class="search-bar">
    <nz-input-group>
      <input nz-input class="search-input" placeholder="Search..." />
    </nz-input-group>
    <button nz-button class="search-btn">Search</button>
    <button nz-button class="add-btn">Add New</button>
  </div>

  <!-- Table -->
  <nz-table [nzData]="data">
    <thead>
      <tr>
        <th>ID</th>
        <th>Name</th>
        <th>Status</th>
        <th>Actions</th>
      </tr>
    </thead>
    <tbody>
      <tr class="clickable-row">
        <td class="id-cell">{{ item.id }}</td>
        <td class="name-cell">{{ item.name }}</td>
        <td>
          <nz-tag class="status-tag" [nzColor]="item.active ? 'green' : 'red'">
            {{ item.active ? 'Active' : 'Inactive' }}
          </nz-tag>
        </td>
        <td>
          <button nz-button class="edit-btn">Edit</button>
          <button nz-button class="delete-btn">Delete</button>
        </td>
      </tr>
    </tbody>
  </nz-table>
</div>
```

## Available CSS Classes

### Container Classes

- `.data-table-container` - Main container for data tables
- `.search-bar` - Container for search functionality
- `.table-actions` - Container for table action buttons

### Button Classes

#### Action Buttons
- `.primary-action-btn` - Blue gradient (Edit, View, etc.)
- `.success-action-btn` - Green gradient (Add, Activate, etc.)
- `.danger-action-btn` - Red gradient (Delete, Deactivate, etc.)
- `.warning-action-btn` - Orange gradient (Warning actions)

#### Specific Button Classes
- `.search-btn` - Search button styling
- `.add-btn` - Add new item button
- `.edit-btn` - Edit button
- `.delete-btn` - Delete button
- `.activate-btn` - Activate button
- `.deactivate-btn` - Deactivate button
- `.view-btn` - View button

### Status Classes

#### Tags
- `.status-tag` - General status tag styling
- `.role-tag` - Role tag styling

#### Text Status
- `.status-active` - Active status text (green)
- `.status-inactive` - Inactive status text (red)
- `.status-pending` - Pending status text (orange)
- `.status-suspended` - Suspended status text (purple)

### Table Cell Classes

#### Data Cells
- `.user-avatar` - User avatar styling
- `.username-text` - Username text styling
- `.email-cell` - Email cell with ellipsis
- `.branch-cell` - Branch cell with ellipsis
- `.name-cell` - Name cell with ellipsis
- `.phone-cell` - Phone cell with ellipsis
- `.id-cell` - ID cell with blue color

#### Row Classes
- `.clickable-row` - Makes table rows clickable with hover effects
- `.table-row` - Basic table row styling

### Pagination Classes

- `.pagination-info` - Pagination information text
- `.total-count-display` - Total count display with blue background

### Expanded Content Classes

- `.expanded-content` - Container for expandable row content
- `.detail-item` - Individual detail item in expanded content
- `.action-buttons` - Container for action buttons in expanded content
- `.action-btn` - Individual action button in expanded content

### Utility Classes

- `.text-center` - Center text alignment
- `.w-100` - 100% width
- `.flex-center` - Flexbox center alignment
- `.flex-between` - Flexbox space-between alignment
- `.flex-end` - Flexbox end alignment

### Animation Classes

- `.fade-in` - Fade in animation
- `.slide-in` - Slide in animation

## Examples

### Complete User Table Example

```html
<div class="data-table-container">
  <!-- Search and Actions -->
  <div class="search-bar">
    <div nz-row [nzGutter]="16">
      <div nz-col [nzSpan]="16">
        <nz-input-group>
          <input nz-input class="search-input" placeholder="Search users..." />
        </nz-input-group>
      </div>
      <div nz-col [nzSpan]="8" class="table-actions">
        <button nz-button class="search-btn">
          <span nz-icon nzType="search"></span>
          Search
        </button>
        <button nz-button class="add-btn">
          <span nz-icon nzType="plus"></span>
          Add User
        </button>
      </div>
    </div>
  </div>

  <!-- User Table -->
  <nz-table [nzData]="users">
    <thead>
      <tr>
        <th>ID</th>
        <th>User</th>
        <th>Email</th>
        <th>Role</th>
        <th>Status</th>
        <th>Actions</th>
      </tr>
    </thead>
    <tbody>
      <tr class="clickable-row" *ngFor="let user of users">
        <td class="id-cell">{{ user.id }}</td>
        <td>
          <div nz-flex nzAlign="center" [nzGap]="8">
            <nz-avatar [nzSrc]="user.avatar" class="user-avatar"></nz-avatar>
            <span class="username-text">{{ user.username }}</span>
          </div>
        </td>
        <td class="email-cell">{{ user.email }}</td>
        <td>
          <nz-tag class="role-tag" [nzColor]="getRoleColor(user.role)">
            {{ user.role }}
          </nz-tag>
        </td>
        <td>
          <nz-tag class="status-tag" [nzColor]="user.active ? 'green' : 'red'">
            {{ user.active ? 'Active' : 'Inactive' }}
          </nz-tag>
        </td>
        <td>
          <button nz-button class="edit-btn">
            <span nz-icon nzType="edit"></span>
            Edit
          </button>
          <button nz-button class="delete-btn">
            <span nz-icon nzType="delete"></span>
            Delete
          </button>
        </td>
      </tr>
    </tbody>
  </nz-table>
</div>
```

### Expanded Row Example

```html
<tr class="clickable-row">
  <!-- Regular table cells -->
</tr>
<tr *ngIf="item.expanded">
  <td colspan="6">
    <div class="expanded-content">
      <div nz-row [nzGutter]="[16, 16]">
        <div nz-col [nzSpan]="8">
          <nz-card nzTitle="Details">
            <div class="detail-item">
              <strong>Name:</strong>
              <span>{{ item.name }}</span>
            </div>
            <div class="detail-item">
              <strong>Email:</strong>
              <span>{{ item.email }}</span>
            </div>
          </nz-card>
        </div>
        <div nz-col [nzSpan]="8">
          <nz-card nzTitle="Actions">
            <div class="action-buttons">
              <button nz-button class="action-btn edit-btn">Edit</button>
              <button nz-button class="action-btn delete-btn">Delete</button>
            </div>
          </nz-card>
        </div>
      </div>
    </div>
  </td>
</tr>
```

## Customization

### Overriding Global Styles

If you need to customize styles for a specific component, you can override the global styles in your component's CSS file:

```less
// In your component's .less file
.data-table-container {
  // Override specific styles for this component
  .search-btn {
    background: linear-gradient(135deg, #722ed1, #531dab); // Purple gradient
  }
  
  .add-btn {
    background: linear-gradient(135deg, #13c2c2, #08979c); // Teal gradient
  }
}
```

### Adding New Button Variants

To add new button variants, extend the global styles:

```less
// In your component's .less file
.info-action-btn {
  @extend .action-button;
  background: linear-gradient(135deg, #13c2c2, #08979c);
  
  &:hover {
    background: linear-gradient(135deg, #36cfc9, #13c2c2);
    box-shadow: 0 4px 8px rgba(19, 194, 194, 0.3);
  }
}
```

## Best Practices

1. **Always use the container class**: Wrap your table in `.data-table-container`
2. **Use semantic button classes**: Choose the appropriate button class for the action
3. **Apply status classes consistently**: Use the same status styling across all tables
4. **Keep component-specific styles minimal**: Only override when necessary
5. **Use utility classes**: Leverage the provided utility classes for common layouts

## Migration from Component-Specific Styles

When migrating from component-specific styles to global styles:

1. Replace component container classes with `.data-table-container`
2. Update button classes to use the global variants
3. Apply status and cell classes consistently
4. Remove duplicate styles from component CSS files
5. Test the appearance and functionality

## Browser Support

The global table styles use modern CSS features including:
- CSS Grid and Flexbox
- CSS Custom Properties (variables)
- Modern CSS animations
- CSS gradients

These features are supported in all modern browsers (Chrome 60+, Firefox 55+, Safari 12+, Edge 79+). 