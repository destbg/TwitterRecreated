import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import * as moment from 'moment';
import { IAlbum, Lightbox } from 'ngx-lightbox';
import { interval, Subscription } from 'rxjs';
import { IPoll } from 'src/app/model/poll.model';
import { IPost } from 'src/app/model/post.model';
import { VoteService } from 'src/app/service/vote.service';
import videojs from 'video.js';

@Component({
  selector: 'app-post-pinned',
  templateUrl: './post-pinned.component.html',
})
export class PostPinnedComponent implements OnInit, AfterViewInit, OnDestroy {
  private timer: Subscription;
  @Input() post: IPost;
  videoLink: SafeResourceUrl;
  optionId: number;
  pollEnded: boolean;
  pollEndDate: string;
  totalVotes: number;
  images: IAlbum[];

  @ViewChild('vid', { static: false })
  private readonly videoElement: ElementRef<HTMLVideoElement>;

  constructor(
    private readonly sanitizer: DomSanitizer,
    private readonly voteService: VoteService,
    private readonly lightbox: Lightbox,
  ) {}

  ngOnInit(): void {
    if (!this.post) {
      return;
    }
    if (this.post.video && !this.post.video.includes('.')) {
      this.videoLink = this.sanitizer.bypassSecurityTrustResourceUrl(
        'https://www.youtube.com/embed/' + this.post.video,
      );
    }
    if (!this.post.poll || this.post.poll.length !== 0) {
      this.totalVotes = this.calcTotalVotes();
      this.pollEnded = moment.utc(this.post.pollEnd).diff(moment.utc()) < 0;
      if (this.pollEnded) {
        this.pollEndDate = this.getPollEndedDate();
        if (
          Math.floor(
            moment.duration(moment.utc().diff(this.post.pollEnd)).asDays(),
          ) === 0
        ) {
          this.timer = interval(60 * 1000).subscribe(() => {
            this.pollEndDate = this.getPollEndedDate();
          });
        }
      } else {
        this.pollEndDate = this.getPollEndDate();
        if (
          Math.floor(
            moment
              .duration(moment.utc(this.post.pollEnd).diff(moment.utc()))
              .asDays(),
          ) === 0
        ) {
          this.timer = interval(60 * 1000).subscribe(() => {
            this.pollEndDate = this.getPollEndDate();
          });
        }
      }
    }
    if (
      this.post.images &&
      (this.post.images.length > 1 ||
        this.post.images.some((image: string) => !image.startsWith('http')))
    ) {
      this.images = this.post.images.map(
        (image: string, index: number) =>
          ({
            src: image,
            caption: `image ${index + 1}`,
            thumb: image,
          } as IAlbum),
      );
    }
  }

  ngAfterViewInit(): void {
    if (this.post && this.post.video && this.post.video.includes('.')) {
      videojs(this.videoElement.nativeElement, {
        controls: true,
        autoplay: true,
        muted: true,
        preload: 'auto',
        src: this.post.video,
        techOrder: ['html5'],
        width: 500,
      });
    }
  }

  ngOnDestroy(): void {
    if (this.timer) {
      this.timer.unsubscribe();
    }
  }

  selectOption(optionId: number): void {
    this.optionId = optionId;
  }

  voteOnOption(): void {
    if (!this.optionId) {
      alert('You need to select an option before you can vote');
      return;
    }
    this.post.hasVoted = true;
    this.voteService.voteOnPost(this.optionId);
  }

  calcPercentage(index: number, progressBar: HTMLDivElement): number {
    this.totalVotes = this.calcTotalVotes();
    const result = Math.round(
      (this.post.poll[index].votes / this.totalVotes) * 100,
    );
    progressBar.style.width = result + '%';
    return isNaN(result) ? 0 : result;
  }

  openImage(index: number): void {
    this.lightbox.open(this.images, index);
  }

  private calcTotalVotes(): number {
    return this.post.poll
      .map((f: IPoll) => f.votes)
      .reduce((a: number, b: number) => a + b);
  }

  private getPollEndDate(): string {
    const dateTime = moment.utc(this.post.pollEnd).diff(moment.utc());
    if (dateTime < 0) {
      this.pollEnded = true;
      this.pollEndDate = this.getPollEndedDate();
      this.timer.unsubscribe();
      this.timer = interval(60 * 1000).subscribe(() => {
        this.pollEndDate = this.getPollEndedDate();
      });
    }
    const date = moment.duration(dateTime);
    const days = Math.round(date.asDays());
    if (days > 0) {
      return days + ' days';
    }
    const hours = Math.round(date.asHours());
    if (hours > 0) {
      return hours + ' hours';
    }
    const minutes = Math.round(date.asMinutes());
    if (minutes > 0) {
      return minutes + ' minutes';
    }
    return Math.round(date.asSeconds()) + ' seconds';
  }

  private getPollEndedDate(): string {
    const date = moment.duration(
      moment.utc().diff(moment.utc(this.post.pollEnd)),
    );
    const days = Math.round(date.asDays());
    if (days > 0) {
      return days + ' days';
    }
    const hours = Math.round(date.asHours());
    if (hours > 0) {
      return hours + ' hours';
    }
    const minutes = Math.round(date.asMinutes());
    if (minutes > 0) {
      return minutes + ' minutes';
    }
    return Math.round(date.asSeconds()) + ' seconds';
  }
}
