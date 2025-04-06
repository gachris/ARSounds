import { Component, ElementRef, Input, ViewChild, ChangeDetectorRef, Renderer2, EventEmitter, Output, AfterViewInit } from '@angular/core';
import { TargetResponse, Target, ActivateTargetRequest } from '../../../lib/target.models';
import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { TargetService } from '../../../lib/target.service';
import { NotificationService } from '../../../lib/notification.service';
import { Alert } from '../../notification/notification.component';
import WaveSurfer, * as wavesurferjs from 'wavesurfer.js';

@Component({
  selector: 'app-target-detail-activate',
  standalone: false,
  templateUrl: './target-detail-activate.component.html',
  styleUrl: './target-detail-activate.component.css'
})
export class TargetDetailActivateComponent implements AfterViewInit {
  @Input() target: Target;
  @Output() targetChanged = new EventEmitter();
  @ViewChild('audioElement', { static: false }) audioElementRef: ElementRef;
  @ViewChild('barWidthInput', { static: false }) barWidthInputRef: ElementRef;
  @ViewChild('heightInput', { static: false }) heightInputRef: ElementRef;
  @ViewChild('widthInput', { static: false }) widthInputRef: ElementRef;
  @ViewChild('wavesurfer', { static: false }) wavesurferRef: ElementRef;
  audioElement: HTMLAudioElement;
  wavesurfer: WaveSurfer = null;
  progress = 0;
  playing = false;
  target$: Observable<TargetResponse>;

  constructor(
    private notificationService: NotificationService,
    private ref: ChangeDetectorRef,
    private route: ActivatedRoute,
    private service: TargetService,
    private renderer: Renderer2) {
  }

  ngAfterViewInit(): void {
    this.wavesurfer = wavesurferjs.default.create({
      container: this.wavesurferRef.nativeElement,
      waveColor: '#000000',
      progressColor: '#000',
      cursorColor: '#000',
      backend: 'MediaElement',
      mediaControls: false,
      barWidth: 0,
      fillParent: false,
      hideScrollbar: true,
      cursorWidth: 1,
      barRadius: 1
    });

    this.wavesurfer.load(this.target.audio);

    this.wavesurfer.on('play', () => {
      this.playing = true;
    });
    this.wavesurfer.on('pause', () => {
      this.playing = false;
    });
    this.wavesurfer.on('finish', () => {
      this.playing = false;
      this.wavesurfer.seekTo(0);
      this.progress = 0;
      this.ref.markForCheck();
    });
    this.wavesurfer.on('audioprocess', () => {
      this.progress = this.wavesurfer.getCurrentTime() / this.wavesurfer.getDuration() * 100;
      this.ref.markForCheck();
    });
    this.wavesurfer.on('ready', () => {
      this.SetDefaultBarWidth();
      this.SetDefaultsHeight();
      this.SetDefaultWidth();

      this.audioElement = this.audioElementRef.nativeElement;
      this.audioElement.src = this.target.audio;
      this.audioElement.setAttribute('class', 'm-audio');
    });
  }

  playPause() {
    if (this.wavesurfer) {
      this.wavesurfer.playPause();
    }
  }

  async activate() {
    if (this.wavesurfer) {
      var id = this.route.snapshot.paramMap.get("id");
      var model = new ActivateTargetRequest();
      model.image = (await this.wavesurfer.exportImage("image/png", 1, "dataURL"))[0];
      model.color = this.wavesurfer.options.waveColor;
      this.service.activate(id, model).subscribe(_ => {
        this.target$ = this.service.get(id);
        this.target$.subscribe(item => {
          this.target = item.response.result;
          this.targetChanged.emit(this.target);
          var alert: Alert = {
            type: 'success',
            message: "Target activated successful!"
          }
          this.notificationService.notify(alert);
        });
      });
    }
  }

  SetDefaultBarWidth() {
    this.barWidthInputRef.nativeElement.max = 10;
    this.barWidthInputRef.nativeElement.min = 0;
    this.barWidthInputRef.nativeElement.value = 0;
    this.barWidthInputRef.nativeElement.addEventListener('input', this.UpdateBarWidth.bind(this));
  }

  UpdateBarWidth() {
    this.wavesurfer.setOptions({ barWidth: parseInt(this.barWidthInputRef.nativeElement.value) });
  }

  SetDefaultsHeight() {
    this.heightInputRef.nativeElement.max = 600;
    this.heightInputRef.nativeElement.min = 300;
    this.heightInputRef.nativeElement.value = 600;
    this.heightInputRef.nativeElement.addEventListener('input', this.UpdateHeight.bind(this));
    this.wavesurfer.setOptions({ height: 600 });
  }

  UpdateHeight() {
    this.wavesurfer.setOptions({ height: parseFloat(this.heightInputRef.nativeElement.value) });
  }

  SetDefaultWidth() {
    var audioDuration = this.wavesurfer.getDuration();
    var offsetWidth = this.wavesurferRef.nativeElement.parentNode.offsetWidth;
    let max = offsetWidth / audioDuration as number;
    let min = 300 / audioDuration;
    this.widthInputRef.nativeElement.max = max;
    this.widthInputRef.nativeElement.min = min;
    this.widthInputRef.nativeElement.value = max;
    this.widthInputRef.nativeElement.addEventListener('input', this.UpdateWidth.bind(this));
    this.wavesurfer.setOptions({ minPxPerSec: max });
  }

  UpdateWidth() {
    var audioDuration = this.wavesurfer.getDuration();
    var offsetWidth = this.wavesurferRef.nativeElement.parentNode.offsetWidth;
    let max = offsetWidth / audioDuration as number;
    let min = 300 / audioDuration;
    this.widthInputRef.nativeElement.max = max;
    this.widthInputRef.nativeElement.min = min;
    this.wavesurfer.setOptions({ minPxPerSec: parseFloat(this.widthInputRef.nativeElement.value) });
  }

  onResize() {
    var audioDuration = this.wavesurfer.getDuration();
    var offsetWidth = this.wavesurferRef.nativeElement.parentNode.offsetWidth;
    let max = offsetWidth / audioDuration as number;
    let min = 300 / audioDuration;
    this.widthInputRef.nativeElement.max = max;
    this.widthInputRef.nativeElement.min = min;
    this.widthInputRef.nativeElement.value = max;
    this.wavesurfer.setOptions({ minPxPerSec: max });
  }
}
