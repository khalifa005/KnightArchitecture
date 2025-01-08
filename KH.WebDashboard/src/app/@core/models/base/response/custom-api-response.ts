export class ApiResponse<t> {
  data?: t;
  errors?: string[] | null;
  statusCode?: number;
  errorMessageAr?: string | null;
  errorMessage?: string | null;
}


  
