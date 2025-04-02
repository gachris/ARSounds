import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TargetService } from '../../../services/targets/target.service';
import { TargetBrowserQuery, TargetModel } from '../target.models';
import { Alert } from '../../notification/notification.component';
import { TargetBindingCreateModel, CreateTargetRequest } from '../target.models';
import { Observable } from 'rxjs';
import { TargetResponse } from '../target.responses';
import { NotificationService } from '../../../services/targets/notification.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-targets',
  standalone: false,
  templateUrl: './targets.component.html'
})

export class TargetsComponent {
  private targetsSubject = new BehaviorSubject<Array<TargetModel>>([]);
  targets$ = this.targetsSubject.asObservable();
  targets: Array<TargetModel> = [];
  pageSize: number = 9;
  currentPageNumber: number = 1;
  totalPage: number = 1;
  getnext: boolean;
  processing: boolean;
  target = new TargetBindingCreateModel();
  submitted = false;
  target$: Observable<TargetResponse>;

  constructor(private dataService: TargetService,
    private service: TargetService,
    private notificationService: NotificationService,
    private router: Router) {
    this.loadMore();
  }

  public onScrollingFinished() {
    if (this.getnext) {
      console.log('load more');
      this.loadMore();
    }
  }

  loadMore(): void {
    this.getAll();
    this.targetsSubject.next(this.targets);
  }

  getAll() {
    if (!this.processing) {
      this.processing = true;
      const query = new TargetBrowserQuery();
      query.page = this.currentPageNumber;
      query.size = this.pageSize;
      this.dataService.getAll(query).subscribe(response => {
        var array = response.response.result;
        this.totalPage = response.total_pages;
        this.getnext = this.totalPage >= this.currentPageNumber;
        if (this.getnext) {
          this.currentPageNumber = this.currentPageNumber + 1;
          this.targets.push(...array);
        }
        this.processing = false;
      });
    }
  }

  onSubmit() {
    this.submitted = true;
    const model = new CreateTargetRequest();
    model.audio_type = this.target.file.type;
    model.filename = this.target.file.name;
    model.description = this.target.description;
    let reader = new FileReader();
    reader.onload = () => {
      model.audio_base64 = reader.result.toString();
      this.service.create(model).subscribe(item => {
        this.target$ = this.service.get(item.response.result);
        this.target$.subscribe(item => {
          this.router.navigateByUrl('/target/' + item.response.result.id);
          var alert: Alert = {
            type: 'success',
            message: "Target created successful!"
          }
          this.notificationService.notify(alert);
        });
      });
    }
    reader.readAsDataURL(this.target.file);
  }
}
