import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TargetDeleteComponent } from './target-delete.component';

@Component({
  selector: 'app-target-delete-modal-container',
  standalone: false,
  template: ''
})

export class TargetDeleteModalContainerComponent implements OnInit, OnDestroy {
  destroy = new Subject<void>();
  modalRef: NgbModalRef = null;

  constructor(
    private ngbModalService: NgbModal,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.destroy)).subscribe(params => {
      this.modalRef = this.ngbModalService.open(TargetDeleteComponent);
      this.modalRef.componentInstance.targetId = params['id'];
      this.modalRef.result.then(_ => {
        this.router.navigateByUrl('/targets');
      }, _ => {
        this.router.navigateByUrl('/targets/' + params['id']);
      });
    });
  }

  ngOnDestroy() {
    this.destroy.next();
  }
}
