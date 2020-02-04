import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material';
import { IUser } from 'src/app/model/user.model';
import { AuthService } from 'src/app/service/auth.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-edit-profile-dialog',
  templateUrl: './edit-profile-dialog.component.html',
})
export class EditProfileDialogComponent implements OnInit, AfterViewInit {
  private userImageFile: File;
  private thumbnailFile: File;
  editProfileDialog: FormGroup;
  username: string;
  userImg: string;
  thumbnailImg: string;
  loading: boolean;
  @Input() user: IUser;

  @ViewChild('thumbnail', { static: false })
  private readonly thumbnail: ElementRef<HTMLDivElement>;
  @ViewChild('thumbnailimg', { static: false })
  private readonly thumbnailImage: ElementRef<HTMLImageElement>;

  constructor(
    private readonly dialogRef: MatDialogRef<EditProfileDialogComponent>,
    private readonly authService: AuthService,
    private readonly formBuilder: FormBuilder,
    private readonly userService: UserService,
  ) {}

  ngOnInit(): void {
    this.thumbnailImg = this.user.thumbnail;
    this.userImg = this.user.image;
    this.username = this.authService.currentUserValue.username;
    this.editProfileDialog = this.formBuilder.group({
      name: [
        this.user.displayName,
        [
          Validators.minLength(6),
          Validators.maxLength(50),
          Validators.pattern(/^[a-zA-Z0-9 ]*$/),
        ],
      ],
      bio: [this.user.description || '', [Validators.maxLength(250)]],
    });
  }

  ngAfterViewInit(): void {
    this.setThumbnailSize();
  }

  @HostListener('window:resize')
  onResize(): void {
    this.setThumbnailSize();
  }

  get f(): any {
    return this.editProfileDialog.controls;
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    this.loading = true;
    const controls = this.editProfileDialog.controls;
    const user = {
      displayName:
        !controls.name.invalid && this.user.displayName !== controls.name.value
          ? controls.name.value
          : undefined,
      description:
        !controls.bio.invalid && this.user.description !== controls.bio.value
          ? controls.bio.value
          : undefined,
    };
    this.userService
      .changeUserProfile(
        user.displayName,
        user.description,
        this.userImageFile ? this.userImageFile : undefined,
        this.thumbnailFile ? this.thumbnailFile : undefined,
      )
      .subscribe((data: { image: string; thumbnail: string }) => {
        if (data.image) {
          this.authService.changeImage(data.image);
        }
        this.dialogRef.close({
          displayName: user.displayName,
          description: user.description,
          image: data.image,
          thumbnail: data.thumbnail,
        });
      });
  }

  handleFileInput(files: FileList, thumbnail: boolean): void {
    if (files.length === 0) {
      return;
    }
    const file = files.item(0);
    if (!file.type.startsWith('image')) {
      return;
    }
    const reader = new FileReader();
    reader.onload = (event: any) => {
      if (thumbnail) {
        this.thumbnailImg = event.target.result;
        this.thumbnailFile = file;
      } else {
        this.userImg = event.target.result;
        this.userImageFile = file;
      }
    };
    reader.readAsDataURL(file);
  }

  private setThumbnailSize(): void {
    if (!this.thumbnail) {
      return;
    }
    this.thumbnail.nativeElement.style.height =
      this.thumbnail.nativeElement.clientWidth / 3 + 'px';
    this.thumbnailImage.nativeElement.style.height = this.thumbnail.nativeElement.style.height;
    this.thumbnailImage.nativeElement.style.width =
      this.thumbnail.nativeElement.clientWidth + 'px';
  }
}
