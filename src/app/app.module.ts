import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import {
  MatDialogModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
} from '@angular/material';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { ClipboardModule } from 'ngx-clipboard';
import { DeviceDetectorModule } from 'ngx-device-detector';
import { NgxEmojiPickerModule } from 'ngx-emoji-picker';
import { AlertComponent } from './alert/alert.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CallComponent } from './call/call.component';
import { LoaderComponent } from './loader/loader.component';
import { MainComponent } from './main/main.component';
import { AddUserDialogComponent } from './message/add-user-dialog/add-user-dialog.component';
import { MessageComponent } from './message/message.component';
import { UsersDialogComponent } from './message/users-dialog/users-dialog.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { OverlayComponent } from './overlay/overlay.component';
import { EditProfileDialogComponent } from './profile/edit-profile-dialog/edit-profile-dialog.component';
import { ProfileComponent } from './profile/profile.component';
import { ChatResolver } from './resolver/message.resolver';
import { PostSearchResolver } from './resolver/post.resolver';
import { UserResolver } from './resolver/user.resolver';
import { SearchOverlayComponent } from './search-overlay/search-overlay.component';
import { SearchComponent } from './search/search.component';
import { AuthService } from './service/auth.service';
import { GifDialogComponent } from './shared/post-form/gif-dialog/gif-dialog.component';
import { ReplyDialogComponent } from './shared/reply-dialog/reply-dialog.component';
import { SharedModule } from './shared/shared.module';
import { ErrorInterceptor } from './util/error.interceptor';
import { JwtInterceptor } from './util/jwt.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    ProfileComponent,
    MainComponent,
    NavMenuComponent,
    AlertComponent,
    MessageComponent,
    SearchComponent,
    UsersDialogComponent,
    GifDialogComponent,
    OverlayComponent,
    LoaderComponent,
    ReplyDialogComponent,
    EditProfileDialogComponent,
    CallComponent,
    SearchOverlayComponent,
    AddUserDialogComponent,
  ],
  entryComponents: [
    UsersDialogComponent,
    GifDialogComponent,
    ReplyDialogComponent,
    EditProfileDialogComponent,
    AddUserDialogComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule,
    MatDialogModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    ClipboardModule,
    ReactiveFormsModule,
    NgxEmojiPickerModule.forRoot(),
    DeviceDetectorModule.forRoot(),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    AuthService,
    UserResolver,
    ChatResolver,
    PostSearchResolver,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
