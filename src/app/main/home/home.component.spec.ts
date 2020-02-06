import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule, MatProgressSpinnerModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { NgxEmojiPickerModule } from 'ngx-emoji-picker';
import { PostContentComponent } from 'src/app/shared/post-content/post-content.component';
import { PollComponent } from 'src/app/shared/post-form/poll/poll.component';
import { PostFormComponent } from 'src/app/shared/post-form/post-form.component';
import { PostNumberComponent } from 'src/app/shared/post-number/post-number.component';
import { PostPinnedComponent } from 'src/app/shared/post-pinned/post-pinned.component';
import { PostComponent } from 'src/app/shared/post/post.component';
import { TimeDirective } from 'src/app/shared/post/time.directive';
import { RepostComponent } from 'src/app/shared/repost/repost.component';
import { RightSideComponent } from 'src/app/shared/right-side/right-side.component';
import { TopComponent } from 'src/app/shared/top/top.component';
import { HomeComponent } from './home.component';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        HomeComponent,
        RightSideComponent,
        PostFormComponent,
        PostComponent,
        PostContentComponent,
        PostPinnedComponent,
        PostNumberComponent,
        TimeDirective,
        PollComponent,
        TopComponent,
        RepostComponent,
      ],
      imports: [
        RouterModule,
        HttpClientModule,
        RouterModule.forRoot([]),
        MatDialogModule,
        MatProgressSpinnerModule,
        NgxEmojiPickerModule,
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
