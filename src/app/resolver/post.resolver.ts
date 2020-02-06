import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { IPost } from '../model/post.model';
import { PostService } from '../service/post.service';

@Injectable({ providedIn: 'root' })
export class PostResolver implements Resolve<IPost> {
  constructor(
    private readonly postService: PostService,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<IPost> {
    if (isPlatformBrowser(this.platformId)) {
      return this.postService.getPost(+route.paramMap.get('id'));
    }
    return of(undefined);
  }
}

@Injectable({ providedIn: 'root' })
export class UserPostsResolver implements Resolve<IPost[]> {
  constructor(
    private readonly postService: PostService,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<IPost[]> {
    if (isPlatformBrowser(this.platformId)) {
      return this.postService.getUserPosts(route.paramMap.get('username'));
    }
    return of([]);
  }
}

@Injectable({ providedIn: 'root' })
export class UserLikedPostsResolver implements Resolve<IPost[]> {
  constructor(
    private readonly postService: PostService,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {}

  resolve(): Observable<IPost[]> {
    if (isPlatformBrowser(this.platformId)) {
      return this.postService.getUserLikedPosts(
        window.location.pathname.split('/')[1],
      );
    }
    return of([]);
  }
}

@Injectable({ providedIn: 'root' })
export class UserMultimediaPostsResolver implements Resolve<IPost[]> {
  constructor(
    private readonly postService: PostService,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {}

  resolve(): Observable<IPost[]> {
    if (isPlatformBrowser(this.platformId)) {
      return this.postService.getUserMultimediaPosts(
        window.location.pathname.split('/')[1],
      );
    }
    return of([]);
  }
}

@Injectable({ providedIn: 'root' })
export class PostRepliesResolver implements Resolve<IPost[]> {
  constructor(
    private readonly postService: PostService,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<IPost[]> {
    if (isPlatformBrowser(this.platformId)) {
      return this.postService.getPostReplies(+route.paramMap.get('id'));
    }
    return of([]);
  }
}

@Injectable({ providedIn: 'root' })
export class PostSearchResolver implements Resolve<IPost[]> {
  constructor(
    private readonly postService: PostService,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<IPost[]> {
    if (isPlatformBrowser(this.platformId)) {
      const q = route.paramMap.get('q').trim();
      if (q !== '') {
        return this.postService.getSearchPosts(q);
      }
    }
    return of([]);
  }
}

@Injectable({ providedIn: 'root' })
export class PostImageSearchResolver implements Resolve<IPost> {
  constructor(private readonly postService: PostService) {}

  resolve(): Observable<any> {
    return this.postService.getImageSearchPosts(
      window.location.pathname.split('/')[2],
    );
  }
}

@Injectable({ providedIn: 'root' })
export class PostVideoSearchResolver implements Resolve<IPost[]> {
  constructor(
    private readonly postService: PostService,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {}

  resolve(): Observable<IPost[]> {
    if (isPlatformBrowser(this.platformId)) {
      return this.postService.getVideoSearchPosts(
        window.location.pathname.split('/')[2],
      );
    }
    return of([]);
  }
}
