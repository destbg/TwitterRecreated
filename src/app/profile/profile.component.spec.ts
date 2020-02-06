import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';
import { RightSideComponent } from '../shared/right-side/right-side.component';
import { ProfileComponent } from './profile.component';

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ProfileComponent, NavMenuComponent, RightSideComponent],
      imports: [
        HttpClientModule,
        RouterModule,
        RouterModule.forRoot([]),
        MatDialogModule,
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
