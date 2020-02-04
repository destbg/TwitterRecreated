import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Data } from '@angular/router';
import { IUserFollow } from 'src/app/model/user.model';
import { FollowService } from 'src/app/service/follow.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
})
export class UsersComponent implements OnInit {
  users: IUserFollow[];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly followService: FollowService,
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((routerData: Data) => {
      this.users = routerData.users;
    });
  }

  followUser(index: number): void {
    this.followService.followUser(
      this.users[index].username,
      !this.users[index].followed,
    );
    this.users[index].followed = !this.users[index].followed;
  }

  buttonCss(index: number): string {
    return this.users[index].followed ? 'danger' : 'info';
  }
}
