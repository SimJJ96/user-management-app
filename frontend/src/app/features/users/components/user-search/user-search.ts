import { Component, computed, signal } from '@angular/core';
import { map, Observable, of, startWith } from 'rxjs';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { Page } from '../../models/page.model'
import { User } from '../../models/users.model'
import { UserService } from '../../services/users';
import { UserOperationsService } from '../../services/users-operations';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatOption } from '@angular/material/autocomplete';
import { MatFormField, MatLabel, MatSelect, MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-user-search',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule
  ],
  templateUrl: './user-search.html',
  styleUrls: ['./user-search.scss'],
})
export class UserSearch {
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

  searchTerm: string = '';
  sortBy = signal('');

  sortOptions = [
    { value: '', label: 'Default (No sorting)' },
    { value: 'firstName', label: 'First Name' },
    { value: 'lastName', label: 'Last Name' },
    { value: 'email', label: 'Email' }
  ];

  constructor(
    private userService: UserService,
    private userOperations: UserOperationsService
  ) {
    this.search();
  }


  search(): void {
    this.pageNumber.set(1);
    this.pageSizeChanged.set(false);
    this.loadUsers();
  }

  pageSearch(): void {
    this.pageSizeChanged.set(false);
    this.loadUsers();
  }

  private loadUsers(): void {
    this.userService.searchUsers({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize(),
      search: this.searchTerm,
      sortBy: this.sortBy()
    }).subscribe({
      next: (page: Page<User>) => {
        this.users.set(page.data);
        this.totalRecords.set(page.totalRecords);
        this.totalPages.set(page.totalPages);
      },
      error: () => this.users.set([]) // Clear on error
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber.set(event.pageIndex + 1);

    if (event.pageSize !== this.pageSize()) {
      this.pageSize.set(event.pageSize);
      this.pageSizeChanged.set(true);
    } 
    this.pageSearch();
  }

  // User operations
  viewUser(user: User): void {
    this.userOperations.viewUser(user);
  }

  editUser(user: User): void {
    this.userOperations.editUser(user, () => {
      this.search(); // Refresh after edit
    });
  }

  deleteUser(user: User): void {
    this.userOperations.deleteUser(user, () => {
      this.search(); // Refresh after delete
    });
  }
}