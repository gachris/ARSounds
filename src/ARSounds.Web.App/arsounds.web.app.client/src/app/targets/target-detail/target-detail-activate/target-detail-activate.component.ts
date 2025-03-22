import { Component, ElementRef, Input, ViewChild, ChangeDetectorRef, Renderer2, EventEmitter, Output, OnInit, AfterViewInit } from '@angular/core';
import { TargetModel, TargetActivateRequest } from '../../target.models';
import { Observable } from 'rxjs';
import { TargetResponse } from '../../target.responses';
import { ActivatedRoute } from '@angular/router';
import { TargetService } from '../../../../services/targets/target.service';
import { NotificationService } from '../../../../services/targets/notification.service';
import { Alert } from '../../../notification/notification.component';
import * as wavesurferjs from 'wavesurfer.js';

@Component({
  selector: 'app-target-detail-activate',
  templateUrl: './target-detail-activate.component.html',
})

export class TargetDetailActivateComponent implements AfterViewInit {
  @Input() target: TargetModel;
  @Output() targetChanged = new EventEmitter();
  @ViewChild('barWidthInput', { static: false }) barWidthInputRef: ElementRef;
  @ViewChild('heightInput', { static: false }) heightInputRef: ElementRef;
  @ViewChild('widthInput', { static: false }) widthInputRef: ElementRef;
  @ViewChild('waveformMedia', { static: false }) waveformMediaRef: ElementRef;
  @ViewChild('wavesurfer', { static: false }) wavesurferRef: ElementRef;
  audioElement: HTMLAudioElement;
  wavesurfer: any = null;
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
      mediaControls: true,
      barWidth: 0,
      fillParent: false,
      hideScrollbar: true,
      cursorWidth: 1,
      barRadius: 1
    });

    this.wavesurfer.load(this.target.audio_base64);

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

      this.audioElement = this.wavesurferRef.nativeElement.querySelector("audio");
      this.renderer.removeChild(this.wavesurferRef.nativeElement, this.audioElement);
      this.renderer.appendChild(this.waveformMediaRef.nativeElement, this.audioElement);
      this.audioElement.setAttribute('class', 'm-audio');
    });
  }

  playPause() {
    if (this.wavesurfer && this.wavesurfer.isReady) {
      this.wavesurfer.playPause();
    }
  }

  activate() {
    if (this.wavesurfer && this.wavesurfer.isReady) {
      var id = this.route.snapshot.paramMap.get("id");
      var model = new TargetActivateRequest();
      model.image_base64 = this.wavesurfer.exportImage();
      model.hex_color = this.wavesurfer.params.waveColor;
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
    this.wavesurfer.params.barWidth = parseInt(this.barWidthInputRef.nativeElement.value);
    this.wavesurfer.drawBuffer();
  }

  SetDefaultsHeight() {
    this.heightInputRef.nativeElement.max = 600;
    this.heightInputRef.nativeElement.min = 300;
    this.heightInputRef.nativeElement.value = 600;
    this.heightInputRef.nativeElement.addEventListener('input', this.UpdateHeight.bind(this));
    this.wavesurfer.params.height = 600;
    this.wavesurfer.drawer.setHeight(600);
    this.wavesurfer.drawBuffer();
  }

  UpdateHeight() {
    //this.wavesurfer.drawer.setHeight(parseFloat(this.heightInput.nativeElement.value));
    this.wavesurfer.params.height = parseFloat(this.heightInputRef.nativeElement.value);
    this.wavesurfer.drawBuffer();
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

    this.wavesurfer.params.minPxPerSec = max;
  }

  UpdateWidth() {
    var audioDuration = this.wavesurfer.getDuration();
    var offsetWidth = this.wavesurferRef.nativeElement.parentNode.offsetWidth;
    let max = offsetWidth / audioDuration as number;
    let min = 300 / audioDuration;
    this.widthInputRef.nativeElement.max = max;
    this.widthInputRef.nativeElement.min = min;

    this.wavesurfer.params.minPxPerSec = parseFloat(this.widthInputRef.nativeElement.value);
    this.wavesurfer.drawBuffer();
  }

  onResize(event) {
    var audioDuration = this.wavesurfer.getDuration();
    var offsetWidth = this.wavesurferRef.nativeElement.parentNode.offsetWidth;
    let max = offsetWidth / audioDuration as number;
    let min = 300 / audioDuration;
    this.widthInputRef.nativeElement.max = max;
    this.widthInputRef.nativeElement.min = min;
    this.widthInputRef.nativeElement.value = max;

    this.wavesurfer.params.minPxPerSec = max;
    this.wavesurfer.drawBuffer();
  }
}


//function RenderMasterWaveform(containerId, audio) {
//  if (containerId != null && containerId != undefined && containerId != '' &&
//    audio != null && audio != undefined) {
//    var wavesurfer = WaveSurfer.create({
//      container: '#' + containerId,
//      waveColor: '#000',
//      width: 110,
//      progressColor: '#000',
//      cursorWidth: 0,
//      hideScrollbar: true,
//      responsive: true,
//      height: 220,
//      pixelRatio: 1,
//      minPxPerSec: 1
//    });

//    wavesurfer.load(audio);

//    return wavesurfer;
//  }
//  else {
//    return null;
//  }
//}


//if (timelineId != null && timelineId != undefined && timelineId != '') {
//  wavesurfer.addPlugin(
//    WaveSurfer.timeline.create({
//      container: '#' + timelineId,
//      primaryColor: "#fd11ff",
//      secondaryColor: "#000",
//      primaryFontColor: "#fd11ff",
//      secondaryFontColor: "#000"
//    })
//  ).initPlugin('timeline');
//}

//wavesurfer.addPlugin(
//  WaveSurfer.regions.create({
//    //dragSelection: {
//    //    slop: 5
//    //}
//  })
//).initPlugin('regions');

////wavesurfer.addRegion({
////    start: 0,
////    end: 10,
////    loop: false,
////    drag: false,
////    color: 'hsla(400, 100%, 30%, 0.5)'
////});

////wavesurfer.addRegion({
////    start: 24,
////    end: 27,
////    loop: false,
////    drag: true,
////    color: 'hsla(200, 50%, 70%, 0.4)'
////});

////wavesurfer.addPlugin(
////    WaveSurfer.cursor.create({
////        showTime: true,
////        opacity: 1,
////        height:600,
////        customShowTimeStyle: {
////            'background-color': '#000',
////            color: '#fff',
////            padding: '2px',
////            'font-size': '10px'
////        }
////    })
////).initPlugin('cursor');
