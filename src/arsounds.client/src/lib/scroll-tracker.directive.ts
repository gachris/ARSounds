import { Directive, Output, EventEmitter, HostListener } from '@angular/core';

@Directive({
  selector: '[scrollTracker]',
  standalone: false
})
export class ScrollTrackerDirective {
  @Output() scrollingFinished = new EventEmitter<void>();

  emitted = false;

  @HostListener("window:scroll", [])
  onScroll(): void {
    if ((window.innerHeight + window.scrollY + 13) >= document.body.offsetHeight && !this.emitted) {
      this.emitted = true;
      this.scrollingFinished.emit();
    } else if ((window.innerHeight + window.scrollY) < document.body.offsetHeight) {
      this.emitted = false;
    }
  }
}
