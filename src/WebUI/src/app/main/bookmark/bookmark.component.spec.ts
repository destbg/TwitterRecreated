import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { PostContentComponent } from 'src/app/shared/post-content/post-content.component';
import { PostListComponent } from 'src/app/shared/post-list/post-list.component';
import { PostNumberComponent } from 'src/app/shared/post-number/post-number.component';
import { PostPinnedComponent } from 'src/app/shared/post-pinned/post-pinned.component';
import { PostComponent } from 'src/app/shared/post/post.component';
import { TimeDirective } from 'src/app/shared/post/time.directive';
import { RepostComponent } from 'src/app/shared/repost/repost.component';
import { RightSideComponent } from 'src/app/shared/right-side/right-side.component';
import { TopComponent } from 'src/app/shared/top/top.component';
import { BookmarkComponent } from './bookmark.component';

describe('BookmarkComponent', () => {
  let component: BookmarkComponent;
  let fixture: ComponentFixture<BookmarkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        BookmarkComponent,
        PostListComponent,
        PostComponent,
        PostContentComponent,
        PostPinnedComponent,
        PostNumberComponent,
        TimeDirective,
        TopComponent,
        RightSideComponent,
        RepostComponent,
      ],
      imports: [RouterModule.forRoot([]), HttpClientModule],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookmarkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
