import { Injectable } from '@angular/core';
import { EventEmitter } from 'events';

@Injectable({
  providedIn: 'root',
})
export class OverlayService {
  public event: EventEmitter;
  public followDiv: HTMLElement;

  constructor() {
    this.event = new EventEmitter();
  }
}
