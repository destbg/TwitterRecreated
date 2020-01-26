import { Component, HostListener, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HistoryStorage } from 'src/app/storage/history.storage';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-top',
  templateUrl: './top.component.html',
})
export class TopComponent implements OnInit {
  @Input() text: string;
  @Input() showBack: boolean;
  phoneSized: boolean;
  showMessage: boolean;
  API_URL = environment.API_URL.replace('api/', '');

  constructor(
    private readonly router: Router,
    public historyStorage: HistoryStorage,
  ) {
    this.showBack = true;
    this.showMessage = true;
  }

  ngOnInit(): void {
    if (window) {
      const width = window.innerWidth;
      if (width < 768) {
        this.phoneSized = true;
      } else {
        this.phoneSized = false;
      }
      this.checkURL();
    }
  }

  @HostListener('window:resize', ['$event.target'])
  onResize(target: Window): void {
    const width = target.innerWidth;
    if (width < 768) {
      this.phoneSized = true;
    } else {
      this.phoneSized = false;
    }
    this.checkURL();
  }

  private checkURL(): void {
    if (
      !(
        this.router.url === '/message' ||
        this.router.url.startsWith('/home') ||
        this.router.url.startsWith('/search') ||
        this.router.url.startsWith('/notification')
      ) &&
      this.phoneSized
    ) {
      this.showMessage = true;
    } else {
      this.showMessage = false;
    }
  }
}
