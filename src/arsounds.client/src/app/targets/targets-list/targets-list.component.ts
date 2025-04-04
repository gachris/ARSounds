import { Component, Input, Output, EventEmitter } from '@angular/core';
import { TargetModel } from '../../../lib/target.models';

@Component({
  selector: 'app-targets-list',
  standalone: false,
  templateUrl: './targets-list.component.html',
  styleUrl: './targets-list.component.css'
})
export class TargetsListComponent {
  @Output() scrollingFinished = new EventEmitter<void>();
  @Input() targets: Array<TargetModel> = [];

  constructor() { }

  onScrollingFinished() {
    this.scrollingFinished.emit();
  }
}
