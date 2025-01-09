/**
 * Clean Architecture Web API V1
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { UserListResponse } from './userListResponse';


export interface UserListResponsePagedList { 
    currentPage?: number;
    totalPages?: number;
    pageSize?: number;
    totalCount?: number;
    items?: Array<UserListResponse> | null;
}

