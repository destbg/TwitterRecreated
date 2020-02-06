import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatProgressSpinnerModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { CallComponent } from './call.component';

describe('CallComponent', () => {
  let component: CallComponent;
  let fixture: ComponentFixture<CallComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CallComponent],
      imports: [
        MatProgressSpinnerModule,
        HttpClientModule,
        RouterModule.forRoot([]),
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CallComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
