import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { TagSelectComponent } from '@delon/abc/tag-select';
import { _HttpClient } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';

@Component({
  selector: 'app-list-articles',
  templateUrl: './articles.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [...SHARED_IMPORTS, TagSelectComponent]
})
export class ProListArticlesComponent implements OnInit {
  private readonly http = inject(_HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);

  q = {
    ps: 5,
    categories: [],
    owners: ['zxx'],
    user: '',
    rate: ''
  };

  list: any[] = [];
  loading = false;

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

  owners = [
    {
      id: 'wzj',
      name: 'list.myself'
    },
    {
      id: 'wjh',
      name: 'list.wu_jiahao'
    },
    {
      id: 'zxx',
      name: 'list.zhou_xingxing'
    },
    {
      id: 'zly',
      name: 'list.zhao_liying'
    },
    {
      id: 'ym',
      name: 'list.yao_ming'
    }
  ];

  changeCategory(status: boolean, idx: number): void {
    if (idx === 0) {
      this.categories.map(i => (i.value = status));
    } else {
      this.categories[idx].value = status;
    }
  }

  setOwner(): void {
    this.q.owners = [`wzj`];
    // TODO: wait nz-dropdown OnPush mode
    setTimeout(() => this.cdr.detectChanges());
  }

  ngOnInit(): void {
    this.getData();
  }

  getData(more = false): void {
    this.loading = true;
    this.http.get('/api/list', { count: this.q.ps }).subscribe(res => {
      this.list = more ? this.list.concat(res) : res;
      this.loading = false;
      this.cdr.detectChanges();
    });
  }
}
