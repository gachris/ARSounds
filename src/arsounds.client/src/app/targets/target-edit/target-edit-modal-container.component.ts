import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TargetEditComponent } from './target-edit.component';
import { TargetService } from '../../../lib/target.service';
import { Target } from '../../../lib/target.models';
import { TargetUpdateService } from '../target-detail/target-detail.component';

@Component({
  selector: 'app-target-edit-modal-container',
  standalone: false,
  template: ''
})
export class TargetEditModalContainerComponent implements OnInit, OnDestroy {
  destroy = new Subject<void>();
  modalRef: NgbModalRef = null;
  closeResult: any = null;

  constructor(
    private ngbModalService: NgbModal,
    private router: Router,
    private route: ActivatedRoute,
    private targetService: TargetService,
    private targetUpdateService: TargetUpdateService
  ) { }

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.destroy)).subscribe(params => {
      const targetId = params['id'];
      this.targetService.get(targetId).subscribe(response => {
        const targetData: Target = response.response.result;

        this.modalRef = this.ngbModalService.open(TargetEditComponent);
        this.modalRef.componentInstance.targetId = targetId;
        this.modalRef.componentInstance.target = { ...targetData };

        this.modalRef.result.then(result => {
          this.targetUpdateService.updateTarget(result);
          this.router.navigateByUrl('/targets/' + targetId);
        }, _ => {
          this.router.navigateByUrl('/targets/' + targetId);
        });
      });
    });
  }

  ngOnDestroy() {
    this.destroy.next();
  }
}
