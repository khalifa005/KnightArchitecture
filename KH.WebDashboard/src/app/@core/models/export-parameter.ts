import { Type } from '@angular/core';
import { LookupResponse } from './base/response/lookup.model';
import { KeyValueList } from './base/KeyValueList';

  export class ExportParameter {
    module: string;
    moduleType: string;
    query: string;
    selectedColumnsToExportObj?: LookupResponse[];
    selectedColumnsToExport?: string[];
    fromDate?: Date | null;
    toDate?: Date | null;
    dynamicFilters?: KeyValueList | null;
    
    constructor() {
      this.query = '';
      this.moduleType = '';
      this.module = '';
      this.selectedColumnsToExportObj = [];
      this.selectedColumnsToExport = [];
    }
  }
