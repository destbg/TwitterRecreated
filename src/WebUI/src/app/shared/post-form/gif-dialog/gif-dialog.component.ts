import { Component, OnInit, QueryList } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { IGif, IGifCategory } from 'src/app/model/gif.model';
import { GifService } from 'src/app/service/gif.service';

@Component({
  selector: 'app-gif',
  templateUrl: './gif-dialog.component.html',
})
export class GifDialogComponent implements OnInit {
  gifCategories: QueryList<IGifCategory>;
  gifs: QueryList<IGif>;
  showSearch: boolean;

  constructor(
    private readonly gifService: GifService,
    private readonly dialogRef: MatDialogRef<GifDialogComponent>,
  ) {}

  ngOnInit(): void {
    this.showSearch = true;
    this.gifService
      .getCategories()
      .subscribe(
        (gifCategories: QueryList<IGifCategory>) =>
          (this.gifCategories = gifCategories),
      );
  }

  goToCategory(category: IGifCategory): void {
    this.showSearch = false;
    this.gifCategories = undefined;
    this.gifService
      .getCategoryGifs(category.tag)
      .subscribe((gifs: QueryList<IGif>) => (this.gifs = gifs));
  }

  selectGif(gif: IGif): void {
    this.dialogRef.close(gif.gif);
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
