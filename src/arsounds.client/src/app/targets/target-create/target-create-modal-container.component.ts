import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TargetCreateComponent } from './target-create.component';

@Component({
  selector: 'app-target-create-modal-container',
  standalone: false,
  template: ''
})
export class TargetCreateModalContainerComponent implements OnInit, OnDestroy {
  destroy = new Subject<void>();
  modalRef: NgbModalRef = null;

  constructor(
    private ngbModalService: NgbModal,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.destroy)).subscribe(_ => {
      this.modalRef = this.ngbModalService.open(TargetCreateComponent);
      this.modalRef.result.then(result => {
        this.router.navigateByUrl('/targets/' + result);
      }, _ => {
        this.router.navigateByUrl('/targets');
      });
    });
  }

  ngOnDestroy() {
    this.destroy.next();
  }
}
