export interface CustomFilterRequest {
   search?: string | null; //global search if needed
   isDeleted?: boolean | null; //global filter for active items 
}

export interface CustomPagingRequest {
   pageIndex?: number | null; //current page
   pageSize?: number | null; // items per page
   // sorting?: CustomSortingRequest | null; //right now it contains sortKey and order we can split them based on the our need- IdAsc - IdDesc
   // filters?: CustomFilterRequest | null; //global filter for any table columns will contains columnDbName and value for it's filter 
}

export interface CustomSortingRequest {
   sortOrder?: string | null; //right now it contains sortKey and order we can split them based on the our need- IdAsc - IdDesc
   sortKey?: string | null; //right now it contains sortKey and order we can split them based on the our need- IdAsc - IdDesc
}

export interface LookupFiltersRequest extends CustomPagingRequest, CustomFilterRequest, CustomSortingRequest {
   id?: number | null;
   nameEn?: string;
   nameAr?: string;
   description?: string;
   ignoreCache?: boolean | null;
}


