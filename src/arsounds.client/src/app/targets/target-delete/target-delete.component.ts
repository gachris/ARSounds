import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationService } from '../../../services/targets/notification.service';
import { TargetService } from '../../../services/targets/target.service';
import { Alert } from '../../notification/notification.component';

@Component({
  selector: 'app-target-delete',
  standalone: false,
  templateUrl: './target-delete.component.html',
  styles: []
})

export class TargetDeleteComponent {
  targetId: string;
  submitted = false;
  deleted: boolean = false;

  constructor(
    private notificationService: NotificationService,
    private service: TargetService,
    public modal: NgbActiveModal) {
  }

  ngOnInit() {

  }

  onSubmit() {
    if (!this.submitted) {
      this.submitted = true;
      this.service.delete(this.targetId).subscribe(item => {
        this.deleted = true;
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
