import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { RightSideComponent } from './right-side.component';
import { HttpClientModule } from '@angular/common/http';

describe('RightSideComponent', () => {
  let component: RightSideComponent;
  let fixture: ComponentFixture<RightSideComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RightSideComponent],
      imports: [RouterModule, RouterModule.forRoot([]), HttpClientModule],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RightSideComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
