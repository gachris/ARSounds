import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationService } from '../../../lib/notification.service';
import { Alert } from '../../notification/notification.component';
import { TargetService } from '../../../lib/target.service';
import { Observable } from 'rxjs';
import {
  TargetResponse,
  TargetBindingCreateModel,
  CreateTargetRequest
} from '../../../lib/target.models';

@Component({
  selector: 'app-target-create',
  standalone: false,
  templateUrl: './target-create.component.html',
  styleUrl: './target-create.component.css'
})
export class TargetCreateComponent {
  submitted = false;
  targetId: string = null;
  target = new TargetBindingCreateModel();
  target$: Observable<TargetResponse>;

  constructor(
    private notificationService: NotificationService,
    private service: TargetService,
    public modal: NgbActiveModal
  ) { }

  onSubmit() {
    if (!this.submitted) {
      this.submitted = true;

      const model = new CreateTargetRequest();
      model.audio_type = this.target.file.type;
      model.filename = this.target.file.name;
      model.description = this.target.description;
      let reader = new FileReader();
      reader.onload = () => {
        model.audio_base64 = reader.result.toString();
        this.service.create(model).subscribe(item => {
          this.modal.close(item.response.result);
          var alert: Alert = {
            type: 'success',
            message: "Target created successful!"
          }
          this.notificationService.notify(alert);
        });
      }
      reader.readAsDataURL(this.target.file);
    }
  }
}
