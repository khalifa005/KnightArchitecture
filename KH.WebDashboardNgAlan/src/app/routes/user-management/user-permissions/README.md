# User Permissions Component

## Overview
The `UserPermissionsComponent` is a dedicated component for debugging and displaying user access control list (ACL) information. This component was extracted from the `ListUserProfilesComponent` to provide a centralized location for ACL debugging and testing.

## Features

### Current User Information Display
- Shows current user details including name, role, department, and branch
- Displays active permissions in a readable format
- Provides real-time ACL status information

### ACL Testing Interface
- **Title Visibility Tests**: Tests various ACL conditions for UI element visibility
- **Action Test Buttons**: Demonstrates ACL-based button visibility
- **Input Field Testing**: Shows conditional input field visibility based on permissions
- **Access Denied Templates**: Displays appropriate messages when permissions are insufficient

### Permission Validation
- Tests role-based access control (RBAC)
- Validates permission combinations
- Shows detailed permission status for debugging

## Usage

### Navigation
Access the component via the route: `/user-management/user-permissions`

### Integration
This component can be used as a standalone debugging tool or integrated into other components for ACL testing purposes.

## Dependencies
- `@delon/acl` - ACL service for permission checking
- `@delon/theme` - Settings service for user information
- `ng-zorro-antd` - UI components

## Styling
The component uses custom LESS styles with:
- Responsive grid layout for visibility tests
- Color-coded sections for different types of information
- Smooth animations and hover effects
- Mobile-friendly design

## Development
When adding new ACL tests or permission checks, add them to this component to maintain a centralized debugging interface. 