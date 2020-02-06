import { EventEmitter, Injectable } from '@angular/core';
import * as SimplePeer from 'simple-peer';
import { ICallRequest } from '../model/message.model';
import { SocketService } from '../service/socket.service';

@Injectable({
  providedIn: 'root',
})
export class PeerStorage {
  private myVideo: HTMLVideoElement;
  private otherVideo: HTMLVideoElement;
  private stream: MediaStream;
  private peer: SimplePeer.Instance;

  public requestingCall: EventEmitter<number>;
  public isRequestingCall: boolean;

  constructor(private readonly socketService: SocketService) {
    this.requestingCall = new EventEmitter<number>();
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
    callRequest?: ICallRequest,
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
          username: callRequest.user.username,
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
      this.otherVideo.parentElement.style.widows =
        this.otherVideo.clientWidth + 'px';
      try {
        this.myVideo.srcObject = this.stream;
      } catch {
        this.myVideo.src = URL.createObjectURL(this.stream);
      }
      this.myVideo.volume = 0;
      this.myVideo.play();
    });
    if (!initiator) {
      peer.signal(JSON.parse(callRequest.data));
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
