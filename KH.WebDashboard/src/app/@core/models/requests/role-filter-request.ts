import { CommonLookupFiltersRequest } from "../base/request/common-lookup-filters-request";

export interface RoleFilterRequest extends CommonLookupFiltersRequest {
    reportToRoleId?: number;
}



