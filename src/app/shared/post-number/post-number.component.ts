import {
  Component,
  Input,
  OnChanges,
  SimpleChanges,
  ElementRef,
  ViewChild,
  AfterViewInit,
  OnInit,
} from '@angular/core';
import { formatNumber } from 'src/app/util/functions';

@Component({
  selector: 'app-post-number',
  templateUrl: './post-number.component.html',
  styleUrls: ['./post-number.component.scss'],
})
export class PostNumberComponent implements OnInit, OnChanges {
  @Input() number: number;

  public oldFormat: string;

  @ViewChild('cube', { static: false })
  private readonly cube: ElementRef<HTMLDivElement>;

  ngOnInit(): void {
    if (!this.number) {
      this.number = 0;
    }
    this.oldFormat = formatNumber(this.number);
  }

  ngOnChanges(changes: SimpleChanges): void {
    const current = formatNumber(changes.number.currentValue);
    if (current !== this.oldFormat) {
      this.changeNumber(
        current,
        changes.number.currentValue > changes.number.previousValue,
      );
    }
  }

  private changeNumber(format: string, bigger: boolean): void {
    if (!this.cube) {
      return;
    }
    let side = 'show-';
    const node = document.createElement('div');
    if (bigger) {
      node.classList.add('cube-face', 'cube-bottom');
      side = side + 'bottom';
    } else {
      node.classList.add('cube-face', 'cube-top');
      side = side + 'top';
    }
    node.innerText = format;
    this.cube.nativeElement.appendChild(node);
    this.cube.nativeElement.classList.add('cube-transform', side);
    setTimeout(() => {
      this.cube.nativeElement.classList.remove('cube-transform', side);
      this.cube.nativeElement.removeChild(node);
      this.cube.nativeElement.classList.add('show-front');
      this.oldFormat = format;
    }, 200);
  }
}
