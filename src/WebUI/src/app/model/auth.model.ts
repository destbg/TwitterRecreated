export interface IAuth {
  user: ISelfUser;
  token: string;
}

export interface ISelfUser {
  username: string;
  email: string;
  id: string;
  image: string;
}
