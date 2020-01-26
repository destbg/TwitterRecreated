import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { ColorSketchModule } from 'ngx-color/sketch';
import { ChatColorComponent } from './chat-color.component';

describe('ChatColorComponent', () => {
  let component: ChatColorComponent;
  let fixture: ComponentFixture<ChatColorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ChatColorComponent],
      imports: [ColorSketchModule, RouterModule.forRoot([]), HttpClientModule],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatColorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
