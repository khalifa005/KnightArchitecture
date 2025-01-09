/**
 * Clean Architecture Web API V1
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { UserRoleResponse } from './userRoleResponse';


export interface UserListResponse { 
    Id?: number | null;
    CreatedDate?: string;
    CreatedById?: number | null;
    UpdatedDate?: string | null;
    UpdatedById?: number | null;
    IsDeleted?: boolean;
    DeletedDate?: string | null;
    DeletedById?: number | null;
    FirstName?: string | null;
    LastName?: string | null;
    Email?: string | null;
    Username?: string | null;
    BirthDate?: string | null;
    MobileNumber?: string | null;
    DepartmentNames?: Array<string> | null;
    UserRoles?: Array<UserRoleResponse> | null;
}

