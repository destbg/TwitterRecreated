import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { PostContentComponent } from 'src/app/shared/post-content/post-content.component';
import { PostListComponent } from 'src/app/shared/post-list/post-list.component';
import { PostNumberComponent } from 'src/app/shared/post-number/post-number.component';
import { PostPinnedComponent } from 'src/app/shared/post-pinned/post-pinned.component';
import { PostComponent } from 'src/app/shared/post/post.component';
import { TimeDirective } from 'src/app/shared/post/time.directive';
import { LikedProfileComponent } from './liked-profile.component';
import { RepostComponent } from 'src/app/shared/repost/repost.component';

describe('LikedProfileComponent', () => {
  let component: LikedProfileComponent;
  let fixture: ComponentFixture<LikedProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        LikedProfileComponent,
        PostComponent,
        PostListComponent,
        PostNumberComponent,
        PostContentComponent,
        PostPinnedComponent,
        TimeDirective,
        RepostComponent,
      ],
      imports: [HttpClientModule, RouterModule, RouterModule.forRoot([])],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LikedProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
