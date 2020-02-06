import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { PostContentComponent } from '../post-content/post-content.component';
import { TimeDirective } from '../post/time.directive';
import { RepostComponent } from './repost.component';

describe('RepostComponent', () => {
  let component: RepostComponent;
  let fixture: ComponentFixture<RepostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RepostComponent, PostContentComponent, TimeDirective],
      imports: [RouterModule.forRoot([])],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RepostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
