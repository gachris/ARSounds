import { AfterViewInit, Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { TargetService } from '../../../../services/targets/target.service';
import { TargetModel } from '../../target.models';
import { ResponseMessage } from '../../../../core/api.response';
import { Alert } from '../../../notification/notification.component';
import { NotificationService } from '../../../../services/targets/notification.service';
import { Observable } from 'rxjs';
import { TargetResponse } from '../../target.responses';

@Component({
  selector: 'app-target-detail-deactivate',
  templateUrl: './target-detail-deactivate.component.html',
})

export class TargetDetailDeactivateComponent implements AfterViewInit {
  @ViewChild('audioElement', { static: false }) audioElementRef: ElementRef;
  @Input() target: TargetModel = new TargetModel();
  @Output() targetChanged = new EventEmitter();
  audioElement: HTMLAudioElement;
  playing = false;
  target$: Observable<TargetResponse>;

  constructor(
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private service: TargetService) {
  }

  ngAfterViewInit(): void {
    this.audioElement = this.audioElementRef.nativeElement;
    this.audioElement.src = this.target.audio_base64;
    this.audioElement.setAttribute('class', 'm-audio');
  }

  playPause() {
    if (!this.playing) {
      this.playing = true;
      this.audioElement.play();
    }
    else {
      this.playing = false;
      this.audioElement.pause();
    }
  }

  deactivate() {
    var id = this.route.snapshot.paramMap.get("id");
    this.service.deactivate(id).subscribe(item => {
      this.target$ = this.service.get(id);
      this.target$.subscribe(item => {
        this.target = item.response.result;
        this.targetChanged.emit(this.target);
        var alert: Alert = {
          type: 'success',
          message: "Target deactivated successful!"
        }
        this.notificationService.notify(alert);
      });
    }, (httpErrorResponse: HttpErrorResponse) => {
      var errorResponseMessage: ResponseMessage = httpErrorResponse.error;
      var alert: Alert = {
        type: 'danger',
        message: errorResponseMessage.status_code
      }
      this.notificationService.notify(alert);
    });
  }

}
