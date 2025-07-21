import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { UserService } from '../../services/users';
import { User } from '../../models/users.model';
import { MatError, MatFormField, MatHint, MatInput, MatLabel } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-user-dialog',
  templateUrl: './user-dialog.html',
  styleUrls: ['./user-dialog.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatFormField,
    MatInput,
    MatLabel,
    MatError,
    MatHint,
    MatButton,
  ]
})
export class UserDialog {
  form!: FormGroup;
  isEditMode: boolean;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<UserDialog>,
    private userService: UserService,
    @Inject(MAT_DIALOG_DATA) public data: { user: User, mode: 'view' | 'edit' }
  ) {
    this.isEditMode = data.mode === 'edit';
    this.createForm(data.user);
  }

  private createForm(user: User): void {
    this.form = this.fb.group({
      id: [user.id],
      firstName: [
        user.firstName, [
            Validators.required, 
            Validators.maxLength(50), 
            Validators.pattern(/^[\p{L} \-'\.]+$/u)
        ]
      ],
      lastName: [
        user.lastName, [
            Validators.required, 
            Validators.maxLength(50),
            Validators.pattern(/^[\p{L} \-'\.]+$/u)
        ]
      ],
      email: [
        user.email, [
            Validators.required, 
            Validators.email
        ]
      ],
      phoneNumber: [
        user.phoneNumber, [
            Validators.pattern(/^\+?\d{7,15}$/)
        ]
      ],
      zipCode: [
        user.zipCode, [
            Validators.pattern(/^[A-Za-z0-9\- ]{3,10}$/)
        ]
      ]
    });

    if (!this.isEditMode) {
      this.form.disable();
    }
  }

  onSave(): void {
    if (this.form.valid) {
      const formValue = this.form.getRawValue();
      this.userService.editUser(formValue).subscribe(() => {
        // if success show a Update success popup?
      })
      this.dialogRef.close(this.form.value);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}