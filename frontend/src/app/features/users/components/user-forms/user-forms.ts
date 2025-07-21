// user-dialog.component.ts
import { Component, Inject, Input, OnInit, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { User } from '../../models/users.model';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { UserService } from '../../services/users';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogService } from '../../../../shared/dialogs/services/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-user-forms',
  standalone: true,
  templateUrl: './user-forms.html',
  styleUrls: ['./user-forms.scss'],
  imports: [
    CommonModule,
    MatDialogModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatIconModule
  ],
})
export class UserForms implements OnInit {
  form!: FormGroup;
  dialogRef: any;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router, // for navigation after save
    private userService: UserService,
    private dialogService: DialogService,
    private snackBar: MatSnackBar
  ) {} 

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.form = this.fb.group({
      firstName: ['', [
        Validators.required,
        Validators.maxLength(50),
        Validators.pattern(/^[\p{L} \-'\.]+$/u)
      ]],
      lastName: ['', [
        Validators.required,
        Validators.maxLength(50),
        Validators.pattern(/^[\p{L} \-'\.]+$/u)
      ]],
      email: ['', [
        Validators.required,
        Validators.email
      ]],
      phoneNumber: ['', [
        Validators.pattern(/^\+?\d{7,15}$/)
      ]],
      zipCode: ['', [
        Validators.pattern(/^[A-Za-z0-9\- ]{3,10}$/)
      ]]
    });
  }

  isSaving = signal(false);

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.getRawValue();

    this.isSaving.set(true);
    this.userService.addUser(this.form.getRawValue()).pipe(
      finalize(() => this.isSaving.set(false))
    ).subscribe({
      next: () => {
        this.dialogService.alert(
          'Add Success', 
          'User added successfully.',
          'success'
        );
        this.form.reset();
        this.form.markAsPristine();
        this.form.markAsUntouched();
      },
      error: (err) => {
        console.error('Failed to add user', err);
        this.snackBar.open(
          err.error?.message || 'Failed to add user. Please try again.', 
          'Close', 
          { duration: 3000 }
        );
      }
    });
  }
}
