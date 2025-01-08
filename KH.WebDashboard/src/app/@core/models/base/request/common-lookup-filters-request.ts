
import { CustomPagingParameters } from "../custom-paging-parameters";

export interface CommonLookupFiltersRequest extends CustomPagingParameters  {
  id?: number;
  nameEn?: string;
  nameAr?: string;
  description?: string;
  model?: string;
  modelId?: number;
  isDeleted?: boolean;
  IgnoreCache?: boolean | null ;
  isSent?: boolean;
  requestFromCustomerPortal?: boolean;

  
}
