// dialog.service.ts
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialog } from '../confirmation/confirmation-dialog';
import { MessageDialog } from '../message/message-dialog';

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  constructor(private dialog: MatDialog) {}

  confirm(title: string, message: string) {
    return this.dialog.open(ConfirmationDialog, {
      width: '400px',
      data: { title, message }
    });
  }

  alert(title: string, message: string, type: 'success' | 'error' | 'info' = 'info') {
    return this.dialog.open(MessageDialog, {
      width: '400px',
      data: { title, message, type }
    });
  }
}