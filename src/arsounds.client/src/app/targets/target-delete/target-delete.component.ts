import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Alert } from '../../notification/notification.component';
import { NotificationService } from '../../../lib/notification.service';
import { TargetService } from '../../../lib/target.service';

@Component({
  selector: 'app-target-delete',
  standalone: false,
  templateUrl: './target-delete.component.html',
  styleUrl: './target-delete.component.css'
})
export class TargetDeleteComponent {
  targetId: string;
  submitted = false;

  constructor(
    private notificationService: NotificationService,
    private service: TargetService,
    public modal: NgbActiveModal
  ) { }

  onSubmit() {
    if (!this.submitted) {
      this.submitted = true;
      this.service.delete(this.targetId).subscribe(_ => {
        this.modal.close(true);
        var alert: Alert = {
          type: 'success',
          message: "Target deleted successful!"
        }
        this.notificationService.notify(alert);
      });
    }
  }
}
