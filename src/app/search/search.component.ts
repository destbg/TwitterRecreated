import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  ActivatedRoute,
  NavigationEnd,
  ParamMap,
  Router,
  RouterEvent,
} from '@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
})
export class SearchComponent implements OnInit, AfterViewInit {
  private currentRoute: string;

  @ViewChild('search', { static: false })
  private readonly search: ElementRef<HTMLInputElement>;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
  ) {
    this.currentRoute = '/';
  }

  ngOnInit(): void {
    this.router.events.subscribe((routerEvent: RouterEvent) => {
      if (routerEvent instanceof NavigationEnd) {
        const split = routerEvent.url.split('/');
        if (split.length === 4) {
          this.currentRoute = '/' + split[3];
        } else {
          this.currentRoute = '/';
        }
      }
    });
  }

  ngAfterViewInit(): void {
    this.route.paramMap.subscribe((paramMap: ParamMap) => {
      this.search.nativeElement.value = decodeURIComponent(
        paramMap.get('q'),
      ).trim();
    });
  }

  searchInput(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      this.router.navigate([
        '/search/' +
          encodeURIComponent(this.search.nativeElement.value) +
          this.currentRoute,
      ]);
    }
  }
}
