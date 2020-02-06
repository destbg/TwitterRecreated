import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { DeviceDetectorModule } from 'ngx-device-detector';
import { NgxEmojiPickerModule } from 'ngx-emoji-picker';
import { ChatMessagesComponent } from './chat-messages.component';

describe('ChatMessagesComponent', () => {
  let component: ChatMessagesComponent;
  let fixture: ComponentFixture<ChatMessagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ChatMessagesComponent],
      imports: [
        RouterModule,
        HttpClientModule,
        RouterModule.forRoot([]),
        NgxEmojiPickerModule,
        DeviceDetectorModule.forRoot(),
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatMessagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
