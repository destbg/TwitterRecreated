import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatDialogModule,
} from '@angular/material';
import { RouterModule } from '@angular/router';
import { DeviceDetectorModule } from 'ngx-device-detector';
import { AlertComponent } from './alert/alert.component';
import { AppComponent } from './app.component';
import { CallComponent } from './call/call.component';
import { LoaderComponent } from './loader/loader.component';
import { OverlayComponent } from './overlay/overlay.component';
import { SearchOverlayComponent } from './search-overlay/search-overlay.component';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        AlertComponent,
        OverlayComponent,
        LoaderComponent,
        CallComponent,
        SearchOverlayComponent,
      ],
      imports: [
        RouterModule,
        RouterModule.forRoot([]),
        MatProgressBarModule,
        MatProgressSpinnerModule,
        DeviceDetectorModule.forRoot(),
        HttpClientModule,
        MatDialogModule,
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
