import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Target } from '../../../lib/target.models';

@Component({
  selector: 'app-targets-list',
  standalone: false,
  templateUrl: './targets-list.component.html',
  styleUrl: './targets-list.component.css'
})
export class TargetsListComponent {
  @Output() scrollingFinished = new EventEmitter<void>();
  @Input() targets: Array<Target> = [];

  constructor() { }

  onScrollingFinished() {
    this.scrollingFinished.emit();
  }
}
