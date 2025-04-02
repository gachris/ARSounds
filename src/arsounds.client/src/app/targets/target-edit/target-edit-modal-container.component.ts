import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { ModalDismissReasons, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TargetEditComponent } from './target-edit.component';
import { TargetDetailComponent } from '../target-detail/target-detail.component';

@Component({
  selector: 'app-target-edit-modal-container',
  template: ''
})

export class TargetEditModalContainerComponent implements OnInit, OnDestroy {
  destroy = new Subject<void>();
  modalRef: NgbModalRef = null;
  closeResult = null;

  constructor(@Inject(TargetDetailComponent) private targetDetailComponent: TargetDetailComponent, private ngbModalService: NgbModal, private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.destroy)).subscribe(params => {
      this.modalRef = this.ngbModalService.open(TargetEditComponent);
      this.modalRef.componentInstance.targetId = params.id;
      this.modalRef.componentInstance.target.description = this.targetDetailComponent.target.description;
      this.modalRef.componentInstance.target.hex_color = this.targetDetailComponent.target.hex_color;
      this.modalRef.componentInstance.target.is_trackable = this.targetDetailComponent.target.is_trackable;
      this.modalRef.result.then(result => {
        if (result instanceof Object) {
          this.targetDetailComponent.target = result;
        }
        this.closeResult = `Closed with: ${result}`;
        this.router.navigateByUrl('/target/' + params.id);
      }, reason => {
        this.router.navigateByUrl('/target/' + params.id);
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      });
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  ngOnDestroy() {
    this.destroy.next();
  }
}
