import { Injectable } from '@angular/core';
import { Router, NavigationEnd, RouterEvent } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class HistoryStorage {
  private urlHistory: string[];

  constructor(private readonly router: Router) {
    this.urlHistory = ['/'];
    this.router.events.subscribe((routerEvent: RouterEvent) => {
      if (routerEvent instanceof NavigationEnd) {
        if (routerEvent.urlAfterRedirects === '/home') {
          this.urlHistory = ['/'];
        }
        this.urlHistory = this.urlHistory.filter(
          (url: string) => url !== routerEvent.url,
        );
        this.urlHistory.push(routerEvent.urlAfterRedirects);
      }
    });
  }

  public isEmpty(): boolean {
    return this.urlHistory.length <= 1;
  }

  public navigateBack(): void {
    this.urlHistory.pop();
    const lastUrl = this.urlHistory.pop();
    if (this.urlHistory.length === 0) {
      this.urlHistory.push('/');
    }
    this.router.navigate([lastUrl]);
  }
}
