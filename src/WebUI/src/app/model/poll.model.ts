export interface IPoll {
  option: string;
  votes: number;
}

export class PollVote {
  option: string;
  postId: number;

  constructor(option: string, postId: number) {
    this.option = option;
    this.postId = postId;
  }
}

export interface IPollVoted {
  option: string;
  postId: number;
}
