import { IPoll } from './poll.model';
import { IUserShort } from './user.model';

export interface IPost {
  id: number;
  content: string;
  user: IUserShort;
  postedOn: Date;
  images?: string[];
  video?: string;
  isLiked: boolean;
  likes: number;
  comments: number;
  reposts: number;
  poll?: IPoll[];
  hasVoted?: boolean;
  pollEnd?: Date;
  reply?: IPostReply;
  reposters?: string[];
  likers?: string[];
  repost?: IPostShort;
  bookmarkId?: number;
}

export class PostContent {
  constructor(public content: string, public syntaxType: number) {}
}

export interface IPostLike {
  postId: number;
  isLike: boolean;
  username: string;
}

export interface IPostReply {
  username: string;
  postId: number;
  content: string;
}

export interface IPostShort {
  id: number;
  content: string;
  user: IUserShort;
  postedOn: Date;
}
