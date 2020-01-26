import {
  Component,
  ElementRef,
  HostListener,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material';
import { NavigationEnd, Router, RouterEvent } from '@angular/router';
import { ISelfUser } from '../model/auth.model';
import { AuthService } from '../service/auth.service';
import { CloseDialogComponent } from '../shared/close-dialog/close-dialog.component';
import { PostFormComponent } from '../shared/post-form/post-form.component';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent implements OnInit {
  private position: number;
  user: ISelfUser;
  phoneSized: boolean;
  showBottom: boolean;
  showPhoneMenu: boolean;
  API_URL = environment.API_URL.replace('api/', '');

  @Input() showPost: boolean;

  @ViewChild('leftSide', { static: false })
  private readonly leftSide: ElementRef<HTMLDivElement>;
  @ViewChild('phoneMenu', { static: false })
  private readonly phoneMenu: ElementRef<HTMLDivElement>;

  constructor(
    private readonly authService: AuthService,
    private readonly dialog: MatDialog,
    private readonly router: Router,
  ) {
    this.showPost = true;
  }

  ngOnInit(): void {
    if (this.authService.currentUserValue && window) {
      this.authService.currentUser.subscribe(
        (user: ISelfUser) => (this.user = user),
      );
      const width = window.innerWidth;
      if (width < 768) {
        this.phoneSized = true;
        this.checkURL(this.router.url);
      } else {
        this.phoneSized = false;
      }
      window.onresize = () => {
        this.onResize();
      };
      this.router.events.subscribe((routerEvent: RouterEvent) => {
        if (routerEvent instanceof NavigationEnd) {
          this.checkURL(routerEvent.urlAfterRedirects);
        }
      });
    }
  }

  onResize(): void {
    const width = window.innerWidth;
    if (width < 768) {
      this.phoneSized = true;
    } else {
      this.phoneSized = false;
    }
    this.checkURL(this.router.url);
  }

  @HostListener('window:scroll')
  onScroll(): void {
    if (!this.leftSide) {
      return;
    }
    const scroll = document.documentElement.scrollTop;
    if (scroll > this.position) {
      this.leftSide.nativeElement.scrollTop =
        this.leftSide.nativeElement.scrollTop + 20;
    } else {
      this.leftSide.nativeElement.scrollTop =
        this.leftSide.nativeElement.scrollTop - 20;
    }
    this.position = scroll;
  }

  openPostDialog(): void {
    const dialogRef = this.dialog.open(CloseDialogComponent, {
      width: '50%',
      minWidth: '500px',
    });
    dialogRef.componentInstance.portal = PostFormComponent;
  }

  async expandPhoneMenu(): Promise<void> {
    this.showPhoneMenu = true;
    await new Promise((resolve: any) => setTimeout(resolve, 100));
    this.phoneMenu.nativeElement.classList.add('expanded');
  }

  collapsePhoneMenu(): void {
    this.phoneMenu.nativeElement.classList.remove('expanded');
    setTimeout(() => {
      this.phoneMenu.nativeElement.classList.add('expanded');
      this.showPhoneMenu = false;
    }, 200);
  }

  navigateToURL(url: string): void {
    this.router.navigate([url]);
  }

  private checkURL(url: string): void {
    if (
      (url === '/message' ||
        url.startsWith('/home') ||
        url.startsWith('/search') ||
        url.startsWith('/notification')) &&
      this.phoneSized
    ) {
      this.showBottom = true;
    } else {
      this.showBottom = false;
    }
  }
}
