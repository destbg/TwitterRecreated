import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PostNumberComponent } from './post-number.component';

describe('PostNumberComponent', () => {
  let component: PostNumberComponent;
  let fixture: ComponentFixture<PostNumberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PostNumberComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PostNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
