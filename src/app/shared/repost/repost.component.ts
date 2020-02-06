import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { IPostShort } from 'src/app/model/post.model';
import { IUser } from 'src/app/model/user.model';

@Component({
  selector: 'app-repost',
  templateUrl: './repost.component.html',
  styleUrls: ['./repost.component.scss'],
})
export class RepostComponent {
  @Input() repost: IPostShort;
  @Input() user: IUser;

  constructor(private readonly router: Router) {}

  navigateToRepost(): void {
    this.router.navigate(['/status/' + this.repost.id]);
  }
}
