import {
  AfterViewInit,
  Directive,
  ElementRef,
  HostListener,
} from '@angular/core';
import { IndexOfNonRepeat } from 'src/app/util/functions';
import { Subscription, interval } from 'rxjs';
import { OverlayService } from 'src/app/service/overlay.service';

@Directive({
  selector: '[appPostTextarea]',
})
export class PostTextareaDirective implements AfterViewInit {
  private readonly postInput: HTMLTextAreaElement;
  private readonly imposterDiv: HTMLDivElement;
  private term: string;
  private timer: Subscription;

  constructor(
    element: ElementRef<HTMLTextAreaElement>,
    private readonly overlayService: OverlayService,
  ) {
    this.postInput = element.nativeElement;
    this.imposterDiv = document.createElement('div');
    this.imposterDiv.classList.add('post-input');
    let attributeName: string;
    for (let i = 0; i < this.postInput.attributes.length; i++) {
      const attribute = this.postInput.attributes.item(i);
      if (attribute.name.startsWith('_ngcontent')) {
        attributeName = attribute.name;
        break;
      }
    }
    if (attributeName) {
      this.imposterDiv.setAttribute(attributeName, '');
    } else {
      console.error(
        'attribute name has been changed in post textarea directive',
      );
    }
  }

  ngAfterViewInit(): void {
    this.postInput.parentElement.insertBefore(
      this.imposterDiv,
      // Insert it after the first element which should be the post input
      this.postInput.parentElement.childNodes.item(1),
    );
    this.postInput.addEventListener('input', () => {
      this.textChange();
    });
    this.postInput.addEventListener('focus', () => {
      this.checkForTag();
      if (!this.timer || this.timer.closed) {
        this.timer = interval(3 * 1000).subscribe(() => {
          this.checkForTag();
        });
      }
    });
    this.postInput.addEventListener('blur', () => {
      if (this.timer) {
        this.timer.unsubscribe();
      }
    });
    this.adjustTextArea();
  }

  @HostListener('window:resize')
  onResize(): void {
    // Only apply adjustment if element width has changed.
    if (this.postInput.clientWidth === this.postInput.clientWidth) {
      return;
    }
    this.adjustTextArea();
  }

  private textChange(): void {
    const text = this.postInput.value;
    this.imposterDiv.innerHTML = '';
    if (text.length !== 0) {
      let content = text.slice(0, 250);
      const textSpan = document.createElement('span');
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
          textSpan.appendChild(document.createTextNode(cut));
        }
        cut = content.substring(index.start, index.end);
        let node: Node;
        if (syntaxType === 1) {
          node = document.createTextNode(cut);
        } else {
          const tagAnchor = document.createElement('a');
          tagAnchor.setAttribute('href', '#');
          tagAnchor.innerText = cut;
          node = tagAnchor;
        }
        textSpan.appendChild(node);
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
        textSpan.appendChild(document.createTextNode(content));
      }
      this.imposterDiv.appendChild(textSpan);
    }
    if (text.length >= 250) {
      const span = document.createElement('span');
      span.innerText = text.slice(250);
      span.style.backgroundColor = 'rgba(250, 0, 0, .5)';
      span.classList.add('warn-text');
      this.imposterDiv.appendChild(span);
    }
    this.adjustTextArea();
  }

  private adjustTextArea(): void {
    this.postInput.style.height = 'auto';
    this.postInput.style.height = this.postInput.scrollHeight + 'px';
    this.imposterDiv.style.height = this.postInput.scrollHeight + 'px';
    this.imposterDiv.style.width = this.postInput.clientWidth + 'px';
  }

  private checkForTag(): void {
    const caret = this.postInput.selectionStart;
    if (caret === this.postInput.selectionEnd) {
      const el = this.imposterDiv;
      if (el.childNodes.length > 0) {
        const elem = el.childNodes.item(0);
        let pos = caret;
        let childElem: Node;
        for (let i = 0; i < elem.childNodes.length; i++) {
          const child = elem.childNodes.item(i);
          if (child.textContent.length < pos) {
            pos -= child.textContent.length;
          } else {
            childElem = child;
            break;
          }
        }
        if (childElem && childElem instanceof HTMLAnchorElement) {
          const type = childElem.textContent.slice(0, 1);
          const term = childElem.textContent.slice(1, pos);
          if (this.term !== term) {
            this.term = term;
            this.overlayService.followDiv = childElem;
            if (type === '#') {
              this.overlayService.event.emit('hashtagOverlay', term);
            } else {
              this.overlayService.event.emit('atOverlay', term);
            }
          }
        } else {
          this.overlayService.event.emit('closeOverlay');
        }
      }
    }
  }
}
