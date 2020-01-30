import { IPost } from './post.model';

export interface IBookmark {
  id: number;
  createdOn: Date;
  post: IPost;
}
