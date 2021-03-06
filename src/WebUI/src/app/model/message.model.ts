import { IUserShort } from './user.model';

export interface IMessage {
  id: number;
  message: string;
  user: IUserShort;
  chatId: number;
  createdAt: Date;
  updatedAt?: Date;
}

export class PreviewMessage {
  constructor(public message: string, public image: string) {}
}

export interface IChatUser {
  username: string;
  displayName: string;
  image: string;
  moderator: boolean;
  selfColor: string;
  othersColor: string;
  isOnline: boolean;
  messageReadId?: number;
}

export interface IChat {
  id: number;
  name: string;
  image: string;
  users: IChatUser[];
  lastMessage: string;
  newMessage: boolean;
  isGroup: boolean;
}

export interface ICallRequest {
  id: number;
  user: IUserShort;
  data: string;
}

export interface IRequestCall {
  id: number;
  data: string;
}

export interface ICallResponse {
  username: string;
  data?: string;
  accept: boolean;
}

export interface IUserTyping {
  username: string;
  chatId: number;
}

export interface IMessageRead {
  user: IUserShort;
  chatId: number;
  messageId: number;
}

export interface IUserInChat {
  user: IUserShort;
  chatId: number;
}
