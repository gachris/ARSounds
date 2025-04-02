import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TargetService } from '../../../services/targets/target.service';
import { UpdateTargetRequest } from '../target.models';
import { Observable } from 'rxjs';
import { TargetResponse } from '../target.responses';

@Component({
  selector: 'app-target-edit',
  standalone: false,
  templateUrl: './target-edit.component.html',
})

export class TargetEditComponent implements OnInit {
  targetId: string = null;
  target = new UpdateTargetRequest();
  submitted = false;
  target$: Observable<TargetResponse>;

  constructor(private service: TargetService, public modal: NgbActiveModal) { }

  ngOnInit() {
  }

  onSubmit() {
    if (!this.submitted) {
      this.submitted = true;
      this.service.edit(this.targetId, this.target).subscribe(item => {
        this.target$ = this.service.get(this.targetId);
        this.target$.subscribe(item => {
          this.modal.close(item.response.result);
        });
      });
    }
  }
}
