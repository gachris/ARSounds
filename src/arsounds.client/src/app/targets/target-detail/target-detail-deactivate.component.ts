import { AfterViewInit, Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TargetService } from '../../../lib/target.service';
import { TargetResponse, Target } from '../../../lib/target.models';
import { Alert } from '../../notification/notification.component';
import { NotificationService } from '../../../lib/notification.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-target-detail-deactivate',
  standalone: false,
  templateUrl: './target-detail-deactivate.component.html',
  styleUrl: './target-detail-deactivate.component.css'
})
export class TargetDetailDeactivateComponent implements AfterViewInit {
  @ViewChild('audioElement', { static: false }) audioElementRef: ElementRef;
  @Input() target: Target = new Target();
  @Output() targetChanged = new EventEmitter();
  audioElement: HTMLAudioElement;
  playing = false;
  target$: Observable<TargetResponse>;

  constructor(
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private service: TargetService
  ) { }

  ngAfterViewInit(): void {
    this.audioElement = this.audioElementRef.nativeElement;
    this.audioElement.src = this.target.audio;
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
    this.service.deactivate(id).subscribe(_ => {
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
    });
  }
}
