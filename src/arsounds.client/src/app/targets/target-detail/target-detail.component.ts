import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { TargetService } from '../../../lib/target.service';
import { TargetResponse, Target } from '../../../lib/target.models';

import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-target-detail',
  standalone: false,
  templateUrl: './target-detail.component.html',
  styleUrl: './target-detail.component.css'
})
export class TargetDetailComponent implements OnInit {
  target$: Observable<TargetResponse>;
  target: Target = null;

  constructor(
    private route: ActivatedRoute,
    private service: TargetService,
    private targetUpdateService: TargetUpdateService
  ) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get("id");
    this.target$ = this.service.get(id);
    this.target$.subscribe(item => {
      this.target = item.response.result;
    });

    // Subscribe to updates so that the detail view gets refreshed
    this.targetUpdateService.targetUpdate$.subscribe(updatedTarget => {
      if (updatedTarget) {
        this.target = updatedTarget;
      }
    });
  }

  targetChangedCallback(target: any) {
    this.target = target;
  }
}

@Injectable({ providedIn: 'root' })
export class TargetUpdateService {
  private targetUpdateSubject = new BehaviorSubject<Target>(null);
  targetUpdate$ = this.targetUpdateSubject.asObservable();

  updateTarget(target: Target) {
    this.targetUpdateSubject.next(target);
  }
}
