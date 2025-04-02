import { Component, OnDestroy, OnInit } from '@angular/core';
import { ModalDismissReasons, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TargetDeleteComponent } from './target-delete.component';

@Component({
  selector: 'app-target-delete-modal-container',
  template: ''
})

export class TargetDeleteModalContainerComponent implements OnInit, OnDestroy {
  destroy = new Subject<void>();
  modalRef: NgbModalRef = null;
  closeResult = null;

  constructor(private ngbModalService: NgbModal, private router: Router, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.destroy)).subscribe(params => {
      this.modalRef = this.ngbModalService.open(TargetDeleteComponent);
      this.modalRef.componentInstance.targetId = params.id;
      this.modalRef.result.then(result => {
        this.closeResult = `Closed with: ${result}`;
        if (result === true) {
          this.router.navigateByUrl('targets');
        }
        else {
          this.router.navigateByUrl('/target/' + params.id);
        }
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
