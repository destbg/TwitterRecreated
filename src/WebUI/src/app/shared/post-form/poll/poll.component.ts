import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'app-poll',
  templateUrl: './poll.component.html',
})
export class PollComponent implements OnInit {
  submitted: boolean;
  pollsList: number[];
  polls: string[];
  days: number;
  hours: number;
  minutes: number;

  @Input() submittedEvent: EventEmitter<any>;
  @Output() closeEvent = new EventEmitter<any>();

  constructor() {
    this.days = 0;
    this.hours = 0;
    this.minutes = 0;
  }

  ngOnInit(): void {
    this.pollsList = [0, 0];
    this.polls = ['', ''];
    if (this.submittedEvent) {
      this.submittedEvent.subscribe(() => (this.submitted = true));
    }
  }

  addOption(): void {
    if (this.polls.length > 9) {
      return;
    }
    this.pollsList.push(0);
    this.polls.push('');
  }

  removeOption(index: number): void {
    this.pollsList.shift();
    this.polls = this.polls.filter((f: string) => f !== this.polls[index]);
  }

  increaseCount(element: ElementRef, length: number): void {
    element.nativeElement.innerText = length;
  }

  sendPoll(): [string[], string] {
    if (
      this.days + this.hours + this.minutes < 0 ||
      this.polls.some((f: string) => f.trim() === '')
    ) {
      return;
    }
    const date = moment.utc();
    date.add(this.days, 'days');
    date.add(this.hours, 'hours');
    date.add(this.minutes, 'minutes');
    return [this.polls, date.toJSON()];
  }

  textChange(event: any, index: number, textChar: any): void {
    this.polls[index] = event.target.value;
    textChar.innerText = event.target.value.length;
  }

  selectChange(event: any, type: number): void {
    switch (type) {
      case 1:
        this.days = event.target.value;
        break;
      case 2:
        this.hours = event.target.value;
        break;
      case 3:
        this.minutes = event.target.value;
        break;
    }
  }
}
