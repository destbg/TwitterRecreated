import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { PostContentComponent } from '../post-content/post-content.component';
import { PostNumberComponent } from '../post-number/post-number.component';
import { PostPinnedComponent } from '../post-pinned/post-pinned.component';
import { RepostComponent } from '../repost/repost.component';
import { PostComponent } from './post.component';
import { TimeDirective } from './time.directive';

describe('PostComponent', () => {
  let component: PostComponent;
  let fixture: ComponentFixture<PostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        PostComponent,
        PostContentComponent,
        PostPinnedComponent,
        PostNumberComponent,
        TimeDirective,
        RepostComponent,
      ],
      imports: [
        HttpClientModule,
        RouterModule,
        RouterModule.forRoot([]),
        MatDialogModule,
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
