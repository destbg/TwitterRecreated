import { Component, Input, OnInit } from '@angular/core';
import { PostContent } from 'src/app/model/post.model';
import { IndexOfNonRepeat } from 'src/app/util/functions';

@Component({
  selector: 'app-post-content',
  templateUrl: './post-content.component.html',
})
export class PostContentComponent implements OnInit {
  contentList: PostContent[];

  @Input() content: string;

  ngOnInit(): void {
    if (!this.content) {
      return;
    }
    this.contentList = new Array<PostContent>();
    let content = this.content;
    let index: { start: number; end: number };
    let syntaxType: number;
    let hashtag = IndexOfNonRepeat(content, '#', /^[a-zA-Z0-9]+$/);
    let userTag = IndexOfNonRepeat(content, '@', /^[a-zA-Z0-9]+$/);
    if (userTag && hashtag && hashtag.start > userTag.start) {
      index = userTag;
      syntaxType = 3;
    } else if (hashtag) {
      index = hashtag;
      syntaxType = 2;
    } else {
      index = userTag;
      syntaxType = 3;
    }
    while (index) {
      let cut = content.substring(0, index.start);
      if (cut !== '') {
        this.contentList.push(new PostContent(cut, 1));
      }
      cut = content.substring(index.start, index.end);
      this.contentList.push(new PostContent(cut, syntaxType));
      content = content.substring(index.end);
      hashtag = IndexOfNonRepeat(content, '#', /^[a-zA-Z0-9]+$/);
      userTag = IndexOfNonRepeat(content, '@', /^[a-zA-Z0-9]+$/);
      if (userTag && hashtag && hashtag.start < userTag.start) {
        index = userTag;
        syntaxType = 3;
      } else {
        index = hashtag;
        syntaxType = 2;
      }
    }
    if (content !== '') {
      this.contentList.push(new PostContent(content, 1));
    }
  }

  encodeContentToUrl(index: number): string {
    return encodeURIComponent(this.contentList[index].content);
  }
}
