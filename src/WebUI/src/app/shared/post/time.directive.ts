import { Directive, ElementRef, Input, OnDestroy, OnInit } from '@angular/core';
import * as moment from 'moment';
import { interval, Subscription } from 'rxjs';

@Directive({
  selector: '[appTime]',
})
export class TimeDirective implements OnInit, OnDestroy {
  private postedDate: moment.Moment;
  private timer: Subscription;

  @Input() date: Date;

  constructor(private readonly element: ElementRef<HTMLElement>) {}

  ngOnInit(): void {
    this.postedDate = moment(this.date);
    this.element.nativeElement.innerText = this.getPostedOn();
    if (
      Math.floor(moment.duration(moment().diff(this.postedDate)).asDays()) === 0
    ) {
      this.timer = interval(60 * 1000).subscribe(() => {
        this.element.nativeElement.innerText = this.getPostedOn();
      });
    }
  }

  ngOnDestroy(): void {
    if (this.timer) {
      this.timer.unsubscribe();
    }
  }

  private getPostedOn(): string {
    const date = moment.duration(moment().diff(this.postedDate));
    const days = Math.round(date.asDays());
    if (days > 0) {
      return days + ' days';
    }
    const hours = Math.round(date.asHours());
    if (hours > 0) {
      return hours + ' hours';
    }
    const minutes = Math.round(date.asMinutes());
    if (minutes > 0) {
      return minutes + ' minutes';
    }
    return Math.round(date.asSeconds()) + ' seconds';
  }
}
