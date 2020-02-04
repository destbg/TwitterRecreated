import { Injectable, EventEmitter } from '@angular/core';
import { SocketService } from '../service/socket.service';
import * as SimplePeer from 'simple-peer';
import { AuthService } from '../service/auth.service';
import { ISelfUser } from '../model/auth.model';

@Injectable({
  providedIn: 'root',
})
export class PeerStorage {
  private username: string;
  private myVideo: HTMLVideoElement;
  private otherVideo: HTMLVideoElement;
  private stream: MediaStream;
  private peer: SimplePeer.Instance;

  public requestingCall: EventEmitter<number>;
  public isRequestingCall: boolean;

  constructor(
    private readonly socketService: SocketService,
    private readonly authService: AuthService,
  ) {
    this.requestingCall = new EventEmitter<number>();
    this.authService.currentUser.subscribe((user: ISelfUser) => {
      if (user) {
        this.username = user.username;
      }
    });
    this.socketService.hubConnection.on('startCall', () => {
      if (!this.isRequestingCall) {
        return;
      }
      try {
        this.myVideo.srcObject = this.stream;
      } catch {
        this.myVideo.src = URL.createObjectURL(this.stream);
      }
      this.myVideo.play();
      this.myVideo.volume = 0;
    });
    this.socketService.hubConnection.on('acceptRequest', (data: string) => {
      if (this.peer) {
        this.peer.signal(JSON.parse(data));
      }
    });
  }

  public initPeer(
    initiator: boolean,
    stream: MediaStream,
    myVideo: HTMLVideoElement,
    otherVideo: HTMLVideoElement,
    chatId: number,
    sentData?: string,
  ): void {
    this.stream = stream;
    this.myVideo = myVideo;
    this.otherVideo = otherVideo;
    const peer = new SimplePeer({
      initiator,
      stream,
      trickle: false,
    });
    peer.on('close', () => {
      this.otherVideo.pause();
      peer.destroy();
    });
    peer.on('signal', (data: any) => {
      if (initiator) {
        this.socketService.requestCall({
          data: JSON.stringify(data),
          id: chatId,
        });
      } else {
        this.socketService.respondToCall({
          accept: true,
          data: JSON.stringify(data),
          username: this.username,
        });
      }
    });
    peer.on('stream', (receivedStream: any) => {
      this.isRequestingCall = false;
      try {
        this.otherVideo.srcObject = receivedStream;
      } catch {
        this.otherVideo.src = URL.createObjectURL(receivedStream);
      }
      this.otherVideo.play();
      try {
        this.myVideo.srcObject = this.stream;
      } catch {
        this.myVideo.src = URL.createObjectURL(this.stream);
      }
      this.myVideo.volume = 0;
      this.myVideo.play();
    });
    if (!initiator) {
      peer.signal(JSON.parse(sentData));
    }
    this.peer = peer;
  }

  public closeCall(): void {
    this.peer.destroy();
    this.peer = undefined;
    this.myVideo.pause();
    this.myVideo.src = undefined;
    this.myVideo = undefined;
    this.otherVideo.pause();
    this.otherVideo.src = undefined;
    this.otherVideo = undefined;
  }
}
