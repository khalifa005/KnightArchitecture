export class PagedResponse<t> {
  items?: t[];
  currentPage?: number | null;
  totalPages?: number | null;
  pageSize?: number | null;
  totalCount?: number | null;
}

