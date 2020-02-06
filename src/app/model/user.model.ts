export interface IUser {
  username: string;
  displayName: string;
  image: string;
  thumbnail: string;
  description: string;
  following: number;
  followers: number;
  followed: boolean;
}

export interface IUserShort {
  username: string;
  displayName: string;
  image: string;
}

export interface IUserFollow extends IUserShort {
  followed: boolean;
}
