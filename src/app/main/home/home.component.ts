import { Component, OnInit, HostListener, AfterViewInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { PostStorage } from 'src/app/storage/post.storage';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit, AfterViewInit {
  username: string;

  constructor(
    private readonly authService: AuthService,
    private readonly title: Title,
    public postStorage: PostStorage,
  ) {}

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      return;
    }

    if (typeof document != 'undefined') {
      document.body.addEventListener('touchmove', () => {
        this.onTouchScroll();
      });
    }

    this.title.setTitle('Home | AngularTwitter');

    this.username = this.authService.currentUserValue.username;
  }

  ngAfterViewInit(): void {
    document.documentElement.scrollTop = this.postStorage.scrollPos;
  }

  onTouchScroll(): void {
    const pos = document.body.scrollTop;
    this.postStorage.scrollPos = pos;
    this.postStorage.scrolled();
  }

  @HostListener('window:scroll')
  onScroll(): void {
    const pos = document.documentElement.scrollTop;
    this.postStorage.scrollPos = pos;
    this.postStorage.scrolled();
  }
}
