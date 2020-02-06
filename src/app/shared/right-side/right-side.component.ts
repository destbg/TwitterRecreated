import {
  Component,
  ElementRef,
  HostListener,
  Input,
  ViewChild,
} from '@angular/core';
import { Router } from '@angular/router';
import { FollowService } from 'src/app/service/follow.service';
import { RightSideStorage } from 'src/app/storage/right-side.storage';

@Component({
  selector: 'app-right-side',
  templateUrl: './right-side.component.html',
})
export class RightSideComponent {
  private position: number;
  @Input() hasSearch: boolean;

  @ViewChild('rightSide', { static: false })
  private readonly rightSide: ElementRef<HTMLDivElement>;
  @ViewChild('search', { static: false })
  private readonly search: ElementRef<HTMLInputElement>;

  constructor(
    private readonly router: Router,
    private readonly followService: FollowService,
    public rightSideStorage: RightSideStorage,
  ) {
    this.hasSearch = true;
  }

  @HostListener('window:scroll')
  onScroll(): void {
    if (!this.rightSide) {
      return;
    }
    if (document.documentElement.scrollTop > this.position) {
      this.rightSide.nativeElement.scrollTop =
        this.rightSide.nativeElement.scrollTop + 10;
    } else {
      this.rightSide.nativeElement.scrollTop =
        this.rightSide.nativeElement.scrollTop - 10;
    }
    this.position = document.documentElement.scrollTop;
  }

  searchInput(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      this.router.navigate([
        '/search/' + encodeURIComponent(this.search.nativeElement.value),
      ]);
    }
  }

  followUser(username: string): void {
    this.followService.followUser(username, true);
  }

  encodeContentToUrl(index: number): string {
    return encodeURIComponent('#' + this.rightSideStorage.tags[index].tag);
  }
}
