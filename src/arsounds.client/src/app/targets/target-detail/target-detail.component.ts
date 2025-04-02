import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { TargetService } from '../../../services/targets/target.service';
import { TargetModel } from '../target.models';
import { TargetResponse } from '../target.responses';

@Component({
  selector: 'app-target-detail',
  standalone: false,
  templateUrl: './target-detail.component.html',
})

export class TargetDetailComponent implements OnInit {
  target$: Observable<TargetResponse>;
  target: TargetModel = null;

  constructor(private route: ActivatedRoute, private service: TargetService) {
  }

  ngOnInit() {
    var id = this.route.snapshot.paramMap.get("id");
    this.target$ = this.service.get(id);
    this.target$.subscribe(item => {
      this.target = item.response.result;
    });
  }

  targetChangedCallback(target: any) {
    this.target = target;
  }
}
