# Role Management Module

This module provides a complete role management system with server-side pagination, sorting, and filtering capabilities.

## Features

### Mock API Endpoints
- `GET /api/roles` - List roles with pagination, sorting, and filtering
- `GET /api/roles/:id` - Get role by ID
- `POST /api/roles` - Create new role
- `PUT /api/roles/:id` - Update role
- `DELETE /api/roles/:id` - Delete role

### Role Data Structure
Each role contains the following fields:
- `id` - Unique identifier
- `name_en` - English name
- `name_ar` - Arabic name
- `created_by` - User who created the role
- `edited_by` - User who last edited the role
- `created_at` - Creation timestamp
- `updated_at` - Last update timestamp

### Table Features
- **Server-side pagination** - Configurable page sizes (10, 20, 50, 100)
- **Server-side sorting** - Sort by any column
- **Server-side filtering** - Filter by name (English/Arabic), created by, edited by
- **Search functionality** - Search across multiple fields
- **Bulk operations** - Select multiple roles for deletion
- **Individual actions** - Edit and delete individual roles

### Components
- `ListRoleComponent` - Main list view with search and table
- `RoleDetailsComponent` - Add/Edit role form (placeholder)
- `RoleHistoryComponent` - Role history view (placeholder)

## Usage

Navigate to `/a-role-demo/list-roles` to access the role management interface.

### Search and Filter
- Use the search form to filter roles by name, creator, or editor
- Click column headers to sort by that column
- Use the filter icons in column headers for quick filtering

### Actions
- Click "Edit" to modify a role (placeholder functionality)
- Click "Delete" to remove a role
- Select multiple roles and use "Delete Selected" for bulk operations
- Click "Add New Role" to create a new role (placeholder functionality)

## Mock Data

The system includes 110 mock roles with realistic data including:
- 10 predefined roles (Administrator, Doctor, Nurse, etc.)
- 100 generated roles with various types and creators
- Bilingual names (English and Arabic)
- Realistic timestamps and user information

## Technical Implementation

### Dependencies
- `@delon/abc/st` - Table component with server-side features
- `@delon/mock` - Mock API system
- `ng-zorro-antd` - UI components

### Key Files
- `_mock/_roles.ts` - Mock API implementation
- `list-role/list-role.component.ts` - Main component logic
- `list-role/list-role.component.html` - Template with search and table
- `list-role/list-role.component.less` - Styling

### Server-side Features
- Pagination parameters: `pi` (page index), `ps` (page size)
- Sorting: `sort` parameter with field and direction
- Filtering: Individual filter parameters for each field
- Response format: `{ list: [], total: number, pi: number, ps: number }` 