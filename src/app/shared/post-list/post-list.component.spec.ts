import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { PostContentComponent } from '../post-content/post-content.component';
import { PostNumberComponent } from '../post-number/post-number.component';
import { PostPinnedComponent } from '../post-pinned/post-pinned.component';
import { PostComponent } from '../post/post.component';
import { TimeDirective } from '../post/time.directive';
import { RepostComponent } from '../repost/repost.component';
import { PostListComponent } from './post-list.component';

describe('PostListComponent', () => {
  let component: PostListComponent;
  let fixture: ComponentFixture<PostListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        PostListComponent,
        PostComponent,
        PostContentComponent,
        PostPinnedComponent,
        PostNumberComponent,
        TimeDirective,
        RepostComponent,
      ],
      imports: [RouterModule.forRoot([]), HttpClientModule],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
