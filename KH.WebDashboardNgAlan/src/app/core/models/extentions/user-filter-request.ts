import { LookupFiltersRequest } from './custom-paging-request';

export interface UserFilterRequest extends LookupFiltersRequest {
   username?: string;
   email?: string;
   phone_number?: string;
   national_id?: string;
   branch?: string;
   name_ar?: string;
   name_en?: string;
   role?: string;
   is_active?: boolean;
   created_by?: string;
   created_at?: string;
} 