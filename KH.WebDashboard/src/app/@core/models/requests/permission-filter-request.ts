import { CommonLookupFiltersRequest } from "../base/request/common-lookup-filters-request";

export interface PermissionFilterRequest extends CommonLookupFiltersRequest {
    rolesIds?: number[];
}



