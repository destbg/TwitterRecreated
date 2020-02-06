import { Injectable } from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { IFormattedTag, ITag } from '../model/tag.model';
import { TagService } from '../service/tag.service';
import { formatNumber } from '../util/functions';
import { FollowService } from '../service/follow.service';
import { IUserShort } from '../model/user.model';
import { AuthService } from '../service/auth.service';
import { ISelfUser } from '../model/auth.model';

@Injectable({
  providedIn: 'root',
})
export class RightSideStorage {
  private timer: Subscription;
  private blurTimer: Subscription;

  private tagsArray: IFormattedTag[];
  get tags(): IFormattedTag[] {
    if (this.tagsArray.length === 0) {
      return undefined;
    }
    return this.tagsArray;
  }
  private usersArray: IUserShort[];
  get users(): IUserShort[] {
    if (this.usersArray.length === 0) {
      return undefined;
    }
    return this.usersArray;
  }

  constructor(
    private readonly tagService: TagService,
    private readonly followService: FollowService,
    private readonly authService: AuthService,
  ) {
    this.usersArray = [];
    this.tagsArray = [];
    this.followService.follow.subscribe(() => {
      this.getSuggestions();
    });
    this.followService.unFollow.subscribe(() => {
      this.getSuggestions();
    });
    this.authService.currentUser.subscribe((user: ISelfUser) => {
      if (!user || !this.followService) {
        if (this.timer) {
          this.timer.unsubscribe();
        }
        return;
      }
      this.getTags();
      this.getSuggestions();
      if (document.hasFocus()) {
        this.timer = interval(60 * 1000).subscribe(() => {
          this.getTags();
          this.getSuggestions();
        });
      }
    });
    if (typeof window !== 'undefined') {
      window.onblur = () => {
        this.blurTimer = interval(10 * 1000).subscribe(() => {
          if (this.timer) {
            this.timer.unsubscribe();
          }
          this.blurTimer.unsubscribe();
        });
      };
      window.onfocus = () => {
        if (this.blurTimer) {
          if (!this.blurTimer.closed) {
            this.blurTimer.unsubscribe();
          } else {
            this.timer = interval(60 * 1000).subscribe(() => {
              this.getTags();
              this.getSuggestions();
            });
          }
        }
      };
    }
  }

  private getTags(): void {
    this.tagService.getTopTags().subscribe((tagsArray: ITag[]) => {
      this.tagsArray = tagsArray.map((tag: ITag) => ({
        tag: tag.tag,
        posts: formatNumber(tag.posts, true),
      }));
    });
  }

  private getSuggestions(): void {
    this.followService
      .getSuggestedUsers()
      .subscribe((usersArray: IUserShort[]) => (this.usersArray = usersArray));
  }
}
