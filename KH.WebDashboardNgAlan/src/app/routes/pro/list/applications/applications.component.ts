import { DecimalPipe } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { TagSelectComponent } from '@delon/abc/tag-select';
import { _HttpClient } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';

interface ProListApplicationListItem {
  title: string;
  avatar: string;
  activeUser: string | number;
  newUser: number;
}

@Component({
  selector: 'app-list-applications',
  templateUrl: './applications.component.html',
  styleUrls: ['./applications.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [...SHARED_IMPORTS, TagSelectComponent, DecimalPipe]
})
export class ProListApplicationsComponent implements OnInit {
  private readonly http = inject(_HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);

  q = {
    ps: 8,
    user: null,
    rate: null,
    categories: [],
    owners: ['zxx']
  };

  list: ProListApplicationListItem[] = [];

  loading = true;

  // region: cateogry
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
  // endregion

  ngOnInit(): void {
    this.getData();
  }

  getData(): void {
    this.loading = true;
    this.http.get('/api/list', { count: this.q.ps }).subscribe(res => {
      this.list = res.map((item: ProListApplicationListItem) => {
        item.activeUser = this.formatWan(item.activeUser as number);
        return item;
      });
      this.loading = false;
      this.cdr.detectChanges();
    });
  }

  private formatWan(val: number): string | number {
    const v = val * 1;
    if (!v || isNaN(v)) {
      return '';
    }

    let result: number | string = val;
    if (val > 10000) {
      result = Math.floor(val / 10000);
      result = `${result}`;
    }
    return result;
  }
}
