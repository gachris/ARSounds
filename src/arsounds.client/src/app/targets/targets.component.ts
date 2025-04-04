import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TargetService } from '../../lib/target.service';
import { TargetBrowserQuery, TargetModel } from '../../lib/target.models';

@Component({
  selector: 'app-targets',
  standalone: false,
  templateUrl: './targets.component.html',
  styleUrl: './targets.component.css'
})

export class TargetsComponent implements OnInit {
  private targetsSubject = new BehaviorSubject<Array<TargetModel>>([]);
  targets$ = this.targetsSubject.asObservable();
  targets: Array<TargetModel> = [];
  pageSize: number = 9;
  currentPageNumber: number = 1;
  totalPage: number = 1;
  getnext: boolean;
  processing: boolean;
  searchText: string = '';

  constructor(private service: TargetService) { }

  ngOnInit() {
    this.loadMore();
  }

  public onScrollingFinished() {
    if (this.getnext) {
      this.loadMore();
    }
  }

  loadMore(): void {
    this.getAll();
  }

  getAll() {
    if (!this.processing) {
      this.processing = true;
      const query = new TargetBrowserQuery();
      query.description = this.searchText;
      query.page = this.currentPageNumber;
      query.size = this.pageSize;
      this.service.getAll(query).subscribe(response => {
        const array = response.response.result;
        this.totalPage = response.total_pages;
        this.getnext = this.totalPage >= this.currentPageNumber;
        if (this.getnext) {
          this.currentPageNumber++;
          this.targets.push(...array);
        }
        this.targetsSubject.next(this.targets);
        this.processing = false;
      });
    }
  }

  search() {
    this.currentPageNumber = 1;
    this.targets = [];
    this.targetsSubject.next(this.targets);
    this.loadMore();
  }
}
