import { KeyValueList } from "./KeyValueList";


export interface CustomPagingParameters {
    pageIndex?: number | null; //current page
    pageSize?: number | null; // items per page
    sort?: string | null; //right now it contains sortKey and order we can split them based on the our need- IdAsc - IdDesc
    sortOrder?: string | null; //right now it contains sortKey and order we can split them based on the our need- IdAsc - IdDesc
    sortKey?: string | null; //right now it contains sortKey and order we can split them based on the our need- IdAsc - IdDesc
    // Order?: string | null;
    search?: string | null; //global search if needed
    isDeleted?: boolean | null; //global filter for active items 
    dynamicFilters?: KeyValueList | null; //global filter for any table columns will contains columnDbName and value for it's filter 
}

