// message-dialog.component.ts
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef } from '@angular/material/dialog';

@Component({
  templateUrl: './message-dialog.html',
  styleUrls: ['./message-dialog.scss'],
  imports: [
    MatDialogActions,
    MatDialogClose,
    MatDialogContent
  ]
})
export class MessageDialog {
  constructor(
    public dialogRef: MatDialogRef<MessageDialog>,
    @Inject(MAT_DIALOG_DATA) public data: { 
      title: string; 
      message: string;
      type: 'success' | 'error' | 'info'
    }
  ) {}
}