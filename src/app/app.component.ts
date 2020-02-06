import { AfterViewInit, Component, OnInit } from '@angular/core';
import { DeviceDetectorService } from 'ngx-device-detector';
import { AuthService } from './service/auth.service';
import { SocketService } from './service/socket.service';
import { ISelfUser } from './model/auth.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit, AfterViewInit {
  title = 'AngularTwitter';

  constructor(
    private readonly deviceDetector: DeviceDetectorService,
    private readonly authService: AuthService,
    private readonly socketService: SocketService,
  ) {}

  ngOnInit(): void {
    this.authService.currentUser.subscribe(async (user: ISelfUser) => {
      if (!user) {
        await this.socketService.hubConnection.stop();
      } else {
        await this.socketService.connectToServer();
      }
    });
  }

  ngAfterViewInit(): void {
    if (
      typeof document != 'undefined' &&
      typeof document.body != 'undefined' &&
      this.deviceDetector.isMobile()
    ) {
      document.body.style.overflowX = 'hidden';
    }
  }
}
