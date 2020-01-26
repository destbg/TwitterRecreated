import { ComponentPortal } from '@angular/cdk/portal';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-close-dialog',
  templateUrl: './close-dialog.component.html',
})
export class CloseDialogComponent implements OnInit {
  @Input() portal: any;
  API_URL = environment.API_URL.replace('api/', '');

  componentPortal: ComponentPortal<any>;

  constructor(private readonly dialogRef: MatDialogRef<CloseDialogComponent>) {}

  ngOnInit(): void {
    this.componentPortal = new ComponentPortal(this.portal);
    this.portal = undefined;
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
