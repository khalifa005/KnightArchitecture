import { BasicTrackerResponse } from "./basic-trackerresponse.model";

export class LookupResponse extends BasicTrackerResponse {
  id: number;
  key?: string;
  nameEn: string;
  nameAr: string;
  columnDbName?: string;
  description?: string;
  parentId?: number;

  constructor(id: number = 0, nameAr: string = '', columnDbName: string = '', nameEn: string = '') {
    super();
    this.id = id;
    this.nameAr = nameAr;
    this.nameEn = nameEn;
    this.columnDbName = columnDbName;
  }
}


