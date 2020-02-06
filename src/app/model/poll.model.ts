export interface IPoll {
  id: number;
  option: string;
  votes: number;
}

export class PollVote {
  optionId: number;
  postId: number;

  constructor(optionId: number, postId: number) {
    this.optionId = optionId;
    this.postId = postId;
  }
}

export interface IPollVoted {
  optionId: number;
  postId: number;
}
