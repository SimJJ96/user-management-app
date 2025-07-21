import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { UserService } from '../../services/users';
import { Page } from '../../models/page.model';
import { User } from '../../models/users.model';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatCard, MatCardContent } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { UserOperationsService } from '../../services/users-operations';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatCard,
    MatCardContent,
    MatIcon
  ],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.scss']
})
export class UserList implements OnInit {
  displayedColumns: string[] = [
    'firstName',
    'lastName',
    'email',
    'phoneNumber',
    'zipCode',
    'operations'
  ];

  users = signal<User[]>([]);
  dataSource = computed(() => new MatTableDataSource(this.users()));
  
  // Pagination
  pageNumber = signal(1);
  pageSize = signal(5);
  totalRecords = signal(0);
  totalPages = signal(0);
  pageIndex = computed(() => this.pageNumber() - 1);
  private pageSizeChanged = signal(false); 

  constructor(
    private userService: UserService, 
    private snackBar: MatSnackBar,
    private userOperations: UserOperationsService
  ) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    }).subscribe({
        next: (res: Page<User>) => {
          this.users.set(res.data);
          this.totalRecords.set(res.totalRecords);
          this.totalPages.set(res.totalPages);
        },
        error: (err) => {
            console.error('Get all users failed:', err);
            this.snackBar.open('Failed to get all users. Please try again.', 'Close', {
                duration: 3000,
            });
        }
      });
  }

  onPageChange(event: PageEvent) {    
    this.pageNumber.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.loadUsers();
  }

  viewUser(user: User): void {
    this.userOperations.viewUser(user)
  }

  editUser(user: User): void {
    this.userOperations.editUser(user, () => {
      this.loadUsers();
    });
  }

  deleteUser(user: User): void {
    this.userOperations.deleteUser(user, () => {
      this.loadUsers();
    });
  }
}