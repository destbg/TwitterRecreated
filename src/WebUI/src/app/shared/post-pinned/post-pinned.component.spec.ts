import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { LightboxModule } from 'ngx-lightbox';
import { PostContentComponent } from '../post-content/post-content.component';
import { TimeDirective } from '../post/time.directive';
import { RepostComponent } from '../repost/repost.component';
import { PostPinnedComponent } from './post-pinned.component';

describe('PostPinnedComponent', () => {
  let component: PostPinnedComponent;
  let fixture: ComponentFixture<PostPinnedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        PostPinnedComponent,
        RepostComponent,
        PostContentComponent,
        TimeDirective,
      ],
      imports: [RouterModule.forRoot([]), HttpClientModule, LightboxModule],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostPinnedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
