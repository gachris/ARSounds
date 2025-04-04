import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationService } from '../../../lib/notification.service';
import { Alert } from '../../notification/notification.component';
import { TargetService } from '../../../lib/target.service';
import { UpdateTargetRequest } from '../../../lib/target.models';

@Component({
  selector: 'app-target-edit',
  standalone: false,
  templateUrl: './target-edit.component.html',
  styleUrl: './target-edit.component.css'
})
export class TargetEditComponent {
  targetId: string = null;
  submitted = false;
  target = new UpdateTargetRequest();

  constructor(
    private notificationService: NotificationService,
    private service: TargetService,
    public modal: NgbActiveModal
  ) { }

  onSubmit() {
    if (!this.submitted) {
      this.submitted = true;
      this.service.edit(this.targetId, this.target).subscribe(item => {
        this.service.get(this.targetId).
          subscribe(item => {
            this.modal.close(item.response.result);
            var alert: Alert = {
              type: 'success',
              message: "Target updated successful!"
            }
            this.notificationService.notify(alert);
          });
      });
    }
  }
}
