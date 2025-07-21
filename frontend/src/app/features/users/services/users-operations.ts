import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from './users';
import { User } from '../models/users.model';
import { UserDialog } from '../components/user-dialog/user-dialog';
import { DialogService } from '../../../shared/dialogs/services/dialog';

@Injectable({
  providedIn: 'root'
})
export class UserOperationsService {
  constructor(
    private dialog: MatDialog,
    private userService: UserService,
    private dialogService: DialogService,
    private snackBar: MatSnackBar
  ) {}

  viewUser(user: User): void {
    this.userService.viewUser(user.id).subscribe({
      next: (fullUser) => {
        this.dialog.open(UserDialog, {
          data: { mode: 'view', user: fullUser }
        });
      },
      error: (err) => this.handleError(err, 'Failed to load user details')
    });
  }

  editUser(user: User, onSuccess?: () => void): void {
    const dialogRef = this.dialog.open(UserDialog, {
      data: { mode: 'edit', user: user }
    });

    dialogRef.afterClosed().subscribe((result: User | undefined) => {
      if (result) {
        this.userService.editUser(result).subscribe({
          next: (updatedUser) => {
            this.dialogService.alert(
              'Edit Success', 
              'User edited successfully.',
              'success'
            );
            onSuccess?.();
          },
          error: (err) => this.handleError(err, 'Failed to update user')
        });
      }
    });
  }

  deleteUser(user: User, onSuccess?: () => void): void {
    const dialogRef = this.dialogService.confirm(
      'Confirm Delete User',
      'Are you sure you want to delete this user?'
    );

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.userService.deleteUser(user.id).subscribe({
          next: () => {
            this.dialogService.alert(
              'Delete Success',
              'User deleted successfully.',
              'success'
            );
            onSuccess?.();
          },
          error: (err) => this.handleError(err, 'Failed to delete user')
        });
      }
    });
  }

  private handleError(error: any, message: string): void {
    console.error(message, error);
    this.snackBar.open(`${message}. Please try again.`, 'Close', {
      duration: 3000
    });
  }
}