import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IPost } from '../model/post.model';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  constructor(private readonly http: HttpClient) {}

  public getPosts(skip?: Date): Observable<IPost[]> {
    return this.skipPosts(`post`, skip);
  }

  public sendPost(
    content: string,
    replyId: number,
    files?: FileList,
    poll?: [string[], string],
    gif?: string,
  ): void {
    if (files) {
      const formData = new FormData();
      for (let i = 0; i < files.length; i++) {
        formData.append('files', files.item(i));
      }
      if (replyId) {
        formData.append('replyId', replyId.toString());
      }
      formData.append('content', content);
      this.sendPostData(Boolean(replyId), formData);
    } else if (poll) {
      this.sendPostData(Boolean(replyId), {
        content,
        replyId,
        poll: poll[0],
        pollEnd: poll[1],
      });
    } else {
      this.sendPostData(Boolean(replyId), { content, replyId, gif });
    }
  }

  public sendRepost(content: string, repostId: number): void {
    this.http
      .post(`${environment.API_URL}repost/create`, {
        content,
        repostId,
      })
      .subscribe((data: any) => data);
  }

  public getPost(id: number): Observable<IPost> {
    return this.http.get<IPost>(`${environment.API_URL}post/${id}`);
  }

  public getUserPosts(username: string, skip?: Date): Observable<IPost[]> {
    return this.skipPosts(`user/post/${username}`, skip);
  }

  public getUserLikedPosts(username: string, skip?: Date): Observable<IPost[]> {
    return this.skipPosts(`like/${username}`, skip);
  }

  public getUserMultimediaPosts(
    username: string,
    skip?: Date,
  ): Observable<IPost[]> {
    return this.skipPosts(`multimedia/${username}`, skip);
  }

  public getPostReplies(id: number, skip?: Date): Observable<IPost[]> {
    return this.skipPosts(`reply/${id}`, skip);
  }

  public getSearchPosts(q: string, skip?: Date): Observable<IPost[]> {
    return this.skipPosts(`search/post/${q}`, skip);
  }

  public getImageSearchPosts(q: string, skip?: Date): Observable<IPost[]> {
    return this.skipPosts(`search/image/${q}`, skip);
  }

  public getVideoSearchPosts(q: string, skip?: Date): Observable<IPost[]> {
    return this.skipPosts(`search/video/${q}`, skip);
  }

  public deletePost(id: number): void {
    this.http
      .delete(`${environment.API_URL}post/${id}`)
      .subscribe((data: any) => data);
  }

  private sendPostData(isReply: boolean, sendData: any): void {
    this.http
      .post(
        !isReply ? `${environment.API_URL}post` : `${environment.API_URL}reply`,
        sendData,
      )
      .subscribe((data: any) => data);
  }

  private skipPosts(url: string, skip?: Date): Observable<IPost[]> {
    if (skip) {
      return this.http.get<IPost[]>(environment.API_URL + url + '/' + skip);
    } else {
      return this.http.get<IPost[]>(environment.API_URL + url);
    }
  }
}
