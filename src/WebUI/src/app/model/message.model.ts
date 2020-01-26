import { IUserShort } from './user.model';

export interface IMessage {
  id: number;
  message: string;
  user: IUserShort;
  chatId: number;
  createdAt: Date;
  updatedAt?: Date;
  users?: IUserShort[];
}

export class PreviewMessage {
  constructor(public message: string, public image: string) {}
}

export interface IChatUserOptions {
  selfColor: string;
  othersColor: string;
}

export interface IChatUser {
  username: string;
  displayName: string;
  image: string;
  moderator: boolean;
}

export interface IChat {
  id: number;
  name: string;
  image: string;
  users: IChatUser[];
  userOptions: IChatUserOptions;
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
  username: string;
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
}

export interface IUserInChat {
  user: IUserShort;
  chatId: number;
}
