import { Component, Input, Output, EventEmitter } from '@angular/core';
import { TargetModel } from '../target.models';

@Component({
  selector: 'app-targets-list',
  templateUrl: './targets-list.component.html'
})

export class TargetsListComponent {
  @Output() scrollingFinished = new EventEmitter<void>();
  @Input() targets: Array<TargetModel> = [];

  constructor() { }

  onScrollingFinished() {
    this.scrollingFinished.emit();
  }
}
