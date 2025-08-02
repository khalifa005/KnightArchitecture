import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { TagSelectComponent } from '@delon/abc/tag-select';
import { _HttpClient } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-list-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [...SHARED_IMPORTS, TagSelectComponent]
})
export class ProListProjectsComponent implements OnInit {
  private readonly http = inject(_HttpClient);
  readonly msg = inject(NzMessageService);
  private readonly cdr = inject(ChangeDetectorRef);

  q = {
    ps: 8,
    categories: [],
    owners: ['zxx'],
    user: null,
    rate: null
  };
  list: any[] = [];
  loading = true;

  categories = [
    { id: 0, text: 'list.all', value: false },
    { id: 1, text: 'list.category_one', value: false },
    { id: 2, text: 'list.category_two', value: false },
    { id: 3, text: 'list.category_three', value: false },
    { id: 4, text: 'list.category_four', value: false },
    { id: 5, text: 'list.category_five', value: false },
    { id: 6, text: 'list.category_six', value: false },
    { id: 7, text: 'list.category_seven', value: false },
    { id: 8, text: 'list.category_eight', value: false },
    { id: 9, text: 'list.category_nine', value: false },
    { id: 10, text: 'list.category_ten', value: false },
    { id: 11, text: 'list.category_eleven', value: false },
    { id: 12, text: 'list.category_twelve', value: false }
  ];

  changeCategory(status: boolean, idx: number): void {
    if (idx === 0) {
      this.categories.map(i => (i.value = status));
    } else {
      this.categories[idx].value = status;
    }
    this.getData();
  }

  ngOnInit(): void {
    this.getData();
  }

  getData(): void {
    this.loading = true;
    this.http.get('/api/list', { count: this.q.ps }).subscribe(res => {
      this.list = this.list.concat(res);
      this.loading = false;
      this.cdr.detectChanges();
    });
  }
}
