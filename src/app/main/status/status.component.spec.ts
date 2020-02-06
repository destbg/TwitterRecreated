import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material';
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
import { StatusComponent } from './status.component';

describe('StatusComponent', () => {
  let component: StatusComponent;
  let fixture: ComponentFixture<StatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        StatusComponent,
        RightSideComponent,
        PostContentComponent,
        PostPinnedComponent,
        PostNumberComponent,
        TimeDirective,
        PostListComponent,
        TopComponent,
        RepostComponent,
        PostComponent,
      ],
      imports: [
        RouterModule,
        HttpClientModule,
        RouterModule.forRoot([]),
        MatDialogModule,
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
