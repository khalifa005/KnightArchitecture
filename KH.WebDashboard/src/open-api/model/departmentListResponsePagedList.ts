/**
 * Clean Architecture Web API V1
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { DepartmentListResponse } from './departmentListResponse';


export interface DepartmentListResponsePagedList { 
    currentPage?: number;
    totalPages?: number;
    pageSize?: number;
    totalCount?: number;
    items?: Array<DepartmentListResponse> | null;
}

