import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { IPost } from 'src/app/model/post.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-reply-dialog',
  templateUrl: './reply-dialog.component.html',
})
export class ReplyDialogComponent {
  post: IPost;
  API_URL = environment.API_URL.replace('api/', '');

  constructor(private readonly dialogRef: MatDialogRef<ReplyDialogComponent>) {}

  closeDialog(): void {
    this.dialogRef.close();
  }
}
