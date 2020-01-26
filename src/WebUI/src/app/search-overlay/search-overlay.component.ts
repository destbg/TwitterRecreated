import {
  Component,
  ElementRef,
  HostListener,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ITag } from '../model/tag.model';
import { IUserFollow } from '../model/user.model';
import { OverlayService } from '../service/overlay.service';
import { TagService } from '../service/tag.service';
import { UserService } from '../service/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-search-overlay',
  templateUrl: './search-overlay.component.html',
  styleUrls: ['./search-overlay.component.scss'],
})
export class SearchOverlayComponent implements OnInit {
  hashtagOverlay: boolean;
  atOverlay: boolean;
  hashtags: ITag[];
  users: IUserFollow[];
  API_URL = environment.API_URL.replace('api/', '');

  @ViewChild('list', { static: false })
  private readonly listDiv: ElementRef<HTMLDivElement>;

  constructor(
    private readonly overlayService: OverlayService,
    private readonly userService: UserService,
    private readonly tagService: TagService,
  ) {}

  ngOnInit(): void {
    this.overlayService.event.on('hashtagOverlay', (term: string) => {
      this.hashtagOverlay = true;
      this.tagService
        .searchTags(term)
        .subscribe((tags: ITag[]) => (this.hashtags = tags));
      this.onResize();
    });
    this.overlayService.event.on('atOverlay', (term: string) => {
      this.atOverlay = true;
      this.userService
        .getUserSearch(term)
        .subscribe((users: IUserFollow[]) => (this.users = users));
      this.onResize();
    });
    this.overlayService.event.on('closeOverlay', () => this.closeOverlay());
  }

  @HostListener('window:resize')
  onResize(): void {
    if (this.overlayService.followDiv && this.listDiv) {
      const position = this.overlayService.followDiv.getBoundingClientRect();
      this.listDiv.nativeElement.style.left =
        position.left - this.listDiv.nativeElement.clientWidth + 'px';
      this.listDiv.nativeElement.style.top =
        document.documentElement.scrollTop + position.top + 'px';
    }
  }

  private closeOverlay(): void {
    this.hashtagOverlay = false;
    this.atOverlay = false;
    this.users = undefined;
    this.hashtags = undefined;
    this.overlayService.followDiv = undefined;
  }
}
