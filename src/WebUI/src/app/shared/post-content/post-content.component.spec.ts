import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { PostContentComponent } from './post-content.component';

describe('PostContentComponent', () => {
  let component: PostContentComponent;
  let fixture: ComponentFixture<PostContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PostContentComponent],
      imports: [RouterModule, RouterModule.forRoot([])],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
