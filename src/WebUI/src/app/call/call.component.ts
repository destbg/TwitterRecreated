import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ISelfUser } from '../model/auth.model';
import { ICallRequest } from '../model/message.model';
import { AuthService } from '../service/auth.service';
import { SocketService } from '../service/socket.service';
import { MessageStorage } from '../storage/message.storage';
import { PeerStorage } from '../storage/peer.storage';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss'],
})
export class CallComponent implements OnInit {
  private username: string;
  private chatId: number;
  callRequest: ICallRequest;
  hasRequest: boolean;
  isInCall: boolean;
  notOnline: boolean;

  @ViewChild('myVideo', { static: false })
  private readonly myVideo: ElementRef<HTMLVideoElement>;
  @ViewChild('otherVideo', { static: false })
  private readonly otherVideo: ElementRef<HTMLVideoElement>;

  constructor(
    private readonly authService: AuthService,
    private readonly socketService: SocketService,
    public peerStorage: PeerStorage,
    public messageStorage: MessageStorage,
  ) {
    this.hasRequest = false;
  }

  ngOnInit(): void {
    this.authService.currentUser.subscribe((user: ISelfUser) => {
      if (user) {
        this.username = user.username;
      }
    });
    this.socketService.hubConnection.on(
      'callRequest',
      (callRequest: ICallRequest) => {
        this.hasRequest = true;
        this.callRequest = callRequest;
      },
    );
    this.socketService.hubConnection.on('notOnline', () => {
      this.notOnline = true;
      this.requestDenied();
    });
    this.socketService.hubConnection.on('requestDenied', () => {
      this.requestDenied();
    });
    this.peerStorage.requestingCall.subscribe(async (id: number) => {
      this.chatId = id;
      this.peerStorage.isRequestingCall = true;
      this.isInCall = true;
      await navigator.mediaDevices
        .getUserMedia({ video: true, audio: true })
        .then((stream: MediaStream) => {
          this.peerStorage.initPeer(
            true,
            stream,
            this.myVideo.nativeElement,
            this.otherVideo.nativeElement,
            this.chatId,
          );
        })
        .catch(() => {
          this.requestDenied();
        });
    });
  }

  async acceptRequest(): Promise<void> {
    await navigator.mediaDevices
      .getUserMedia({ video: true, audio: true })
      .then(async (stream: MediaStream) => {
        this.isInCall = true;
        await new Promise((resolve: any) => setTimeout(resolve, 1));
        this.peerStorage.initPeer(
          false,
          stream,
          this.myVideo.nativeElement,
          this.otherVideo.nativeElement,
          this.chatId,
          this.callRequest.data,
        );
        this.hasRequest = false;
      })
      .catch(() => {
        this.hasRequest = false;
        this.requestDenied();
      });
  }

  denyRequest(): void {
    this.socketService.respondToCall({
      accept: false,
      username: this.username,
    });
    this.requestDenied();
  }

  closeCall(): void {
    this.peerStorage.closeCall();
  }

  private requestDenied(): void {
    this.hasRequest = false;
    if (this.peerStorage.isRequestingCall) {
      setTimeout(() => {
        this.peerStorage.isRequestingCall = false;
      }, 3 * 1000);
    }
  }
}
