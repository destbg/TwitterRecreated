import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule, MatProgressSpinnerModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { NgxEmojiPickerModule } from 'ngx-emoji-picker';
import { PostContentComponent } from '../post-content/post-content.component';
import { TimeDirective } from '../post/time.directive';
import { RepostComponent } from '../repost/repost.component';
import { PollComponent } from './poll/poll.component';
import { PostFormComponent } from './post-form.component';
import { PostTextareaDirective } from './post-textarea.directive';

describe('PostFormComponent', () => {
  let component: PostFormComponent;
  let fixture: ComponentFixture<PostFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        PostFormComponent,
        PostTextareaDirective,
        PollComponent,
        RepostComponent,
        TimeDirective,
        PostContentComponent,
      ],
      imports: [
        HttpClientModule,
        MatDialogModule,
        NgxEmojiPickerModule,
        MatProgressSpinnerModule,
        RouterModule.forRoot([]),
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
