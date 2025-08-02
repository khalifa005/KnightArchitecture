import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewChild, inject } from '@angular/core';
import { STChange, STColumn, STComponent, STData } from '@delon/abc/st';
import { _HttpClient } from '@delon/theme';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { AddRoleComponent } from '../add-role/add-role.component';

@Component({
  selector: 'app-list-role',
  imports: SHARED_IMPORTS,
  templateUrl: './list-role.component.html',
  styleUrl: './list-role.component.less',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ListRoleComponent implements OnInit {
  private readonly http = inject(_HttpClient);
  private readonly msg = inject(NzMessageService);
  private readonly modalSrv = inject(NzModalService);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);

  @ViewChild('st', { static: true })
  st!: STComponent;

  // Search parameters
  q: {
    pi: number;
    ps: number;
    name_en: string;
    name_ar: string;
    created_by: string;
    edited_by: string;
  } = {
      pi: 1,
      ps: 10,
      name_en: '',
      name_ar: '',
      created_by: '',
      edited_by: ''
    };

  // Table data
  data: any[] = [];
  loading = false;
  total = 0;

  // Table columns configuration
  columns: STColumn[] = [
    { title: '', index: 'key', type: 'checkbox' },
    {
      title: 'ID',
      index: 'id',
      width: 80,
      sort: {
        compare: (a, b) => a['id'] - b['id']
      }
    },
    {
      title: 'Name (English)',
      index: 'name_en',
      sort: {
        compare: (a, b) => a['name_en'].localeCompare(b['name_en'])
      },
      filter: {
        type: 'keyword',
        fn: (filter, record) => {
          const filterValue = typeof filter === 'string' ? filter : filter?.value || '';
          return record['name_en'].toLowerCase().includes(filterValue.toLowerCase());
        }
      }
    },
    {
      title: 'Name (Arabic)',
      index: 'name_ar',
      sort: {
        compare: (a, b) => a['name_ar'].localeCompare(b['name_ar'])
      },
      filter: {
        type: 'keyword',
        fn: (filter, record) => {
          const filterValue = typeof filter === 'string' ? filter : filter?.value || '';
          return record['name_ar'].includes(filterValue);
        }
      }
    },
    {
      title: 'Created By',
      index: 'created_by',
      sort: {
        compare: (a, b) => a['created_by'].localeCompare(b['created_by'])
      },
      filter: {
        type: 'keyword',
        fn: (filter, record) => {
          const filterValue = typeof filter === 'string' ? filter : filter?.value || '';
          return record['created_by'].toLowerCase().includes(filterValue.toLowerCase());
        }
      }
    },
    {
      title: 'Edited By',
      index: 'edited_by',
      sort: {
        compare: (a, b) => a['edited_by'].localeCompare(b['edited_by'])
      },
      filter: {
        type: 'keyword',
        fn: (filter, record) => {
          const filterValue = typeof filter === 'string' ? filter : filter?.value || '';
          return record['edited_by'].toLowerCase().includes(filterValue.toLowerCase());
        }
      }
    },
    {
      title: 'Created At',
      index: 'created_at',
      type: 'date',
      sort: {
        compare: (a, b) => new Date(a['created_at']).getTime() - new Date(b['created_at']).getTime()
      }
    },
    {
      title: 'Updated At',
      index: 'updated_at',
      type: 'date',
      sort: {
        compare: (a, b) => new Date(a['updated_at']).getTime() - new Date(b['updated_at']).getTime()
      }
    },
    {
      title: 'Actions',
      width: 200,
      buttons: [
        {
          text: 'Edit',
          type: 'link',
          click: (item: any) => this.editRole(item)
        },
        {
          text: 'Delete',
          type: 'del',
          click: (item: any) => this.deleteRole(item)
        }
      ]
    }
  ];

  selectedRows: STData[] = [];
  expandForm = false;

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;

    // Build query parameters
    const params: any = {
      pi: this.q.pi,
      ps: this.q.ps
    };

    if (this.q.name_en) params.name_en = this.q.name_en;
    if (this.q.name_ar) params.name_ar = this.q.name_ar;
    if (this.q.created_by) params.created_by = this.q.created_by;
    if (this.q.edited_by) params.edited_by = this.q.edited_by;

    this.http
      .get('/api/roles', params)
      .subscribe({
        next: (res: any) => {
          this.data = res.list || [];
          this.total = res.total || 0;
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (error) => {
          this.msg.error('Failed to load roles');
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
  }

  stChange(e: STChange): void {
    switch (e.type) {
      case 'checkbox':
        this.selectedRows = e.checkbox!;
        this.cdr.detectChanges();
        break;
      case 'filter':
        this.loadData();
        break;
      case 'sort':
        this.loadData();
        break;
      case 'pi':
        this.q.pi = e.pi!;
        this.loadData();
        break;
      case 'ps':
        this.q.ps = e.ps!;
        this.q.pi = 1; // Reset to first page when changing page size
        this.loadData();
        break;
    }
  }

  onSearch(): void {
    this.q.pi = 1; // Reset to first page when searching
    this.loadData();
  }

  onReset(): void {
    this.q = {
      pi: 1,
      ps: 10,
      name_en: '',
      name_ar: '',
      created_by: '',
      edited_by: ''
    };
    this.loadData();
  }

  editRole(role: any): void {
    this.msg.info(`Edit role: ${role['name_en']}`);
    // TODO: Implement edit functionality
  }

  deleteRole(role: any): void {
    this.modalSrv.confirm({
      nzTitle: 'Confirm Delete',
      nzContent: `Are you sure you want to delete the role "${role['name_en']}"?`,
      nzOkText: 'Yes',
      nzCancelText: 'No',
      nzOnOk: () => {
        this.http.delete(`/api/roles/${role['id']}`).subscribe({
          next: (res: any) => {
            this.msg.success('Role deleted successfully');
            this.loadData();
          },
          error: (error) => {
            this.msg.error('Failed to delete role');
          }
        });
      }
    });
  }

  removeSelected(): void {
    if (this.selectedRows.length === 0) {
      this.msg.warning('Please select roles to delete');
      return;
    }

    this.modalSrv.confirm({
      nzTitle: 'Confirm Delete',
      nzContent: `Are you sure you want to delete ${this.selectedRows.length} selected roles?`,
      nzOkText: 'Yes',
      nzCancelText: 'No',
      nzOnOk: () => {
        const deletePromises = this.selectedRows.map(row =>
          this.http.delete(`/api/roles/${row['id']}`).toPromise()
        );

        Promise.all(deletePromises)
          .then(() => {
            this.msg.success('Selected roles deleted successfully');
            this.selectedRows = [];
            this.loadData();
          })
          .catch(() => {
            this.msg.error('Failed to delete some roles');
          });
      }
    });
  }

  addNewRole(): void {
    const modal = this.modalSrv.create({
      nzContent: AddRoleComponent,
      nzWidth: 800,
      nzFooter: null,
      nzClosable: true,
      nzMaskClosable: false,
      nzBodyStyle: { padding: '24px' }
    });

    modal.afterClose.subscribe((result) => {
      if (result) {
        // Role was created successfully, refresh the data
        this.loadData();
      }
    });
  }
}
