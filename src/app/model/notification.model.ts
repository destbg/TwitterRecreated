import { IPostShort } from './post.model';
import { IUserShort } from './user.model';

export interface INotification {
  id: number;
  type: NotificationType;
  user: IUserShort;
  post: IPostShort;
}

export enum NotificationType {
  repost,
  reply,
  like,
  post,
  follow,
  unFollow,
}
