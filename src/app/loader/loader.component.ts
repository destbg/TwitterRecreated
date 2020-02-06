import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import {
  NavigationCancel,
  NavigationEnd,
  NavigationError,
  NavigationStart,
  Router,
  RouterEvent,
} from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { PostStorage } from '../storage/post.storage';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss'],
})
export class LoaderComponent implements OnInit {
  loading: boolean;
  hasInitialized: boolean;
  isBrowser: boolean;

  constructor(
    public postStorage: PostStorage,
    private readonly router: Router,
    @Inject(PLATFORM_ID) platformId: object,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.router.events.subscribe((routerEvent: RouterEvent) => {
      if (routerEvent instanceof NavigationStart) {
        this.loading = true;
      }

      if (
        routerEvent instanceof NavigationEnd ||
        routerEvent instanceof NavigationCancel ||
        routerEvent instanceof NavigationError
      ) {
        this.loading = false;
        if (this.isBrowser) {
          (window as any).scrollTop = 0;
          // window.scrollTo(0, 0);
        }
      }
    });
  }
}
