import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatToolbar } from '@angular/material/toolbar';
import { MatSidenav, MatSidenavContainer, MatSidenavContent } from '@angular/material/sidenav';
import { MatListItem, MatNavList } from '@angular/material/list';
import { UserList } from './features/users/components/user-list/user-list';
import { UserForms } from './features/users/components/user-forms/user-forms';
import { UserSearch } from './features/users/components/user-search/user-search';

@Component({
  selector: 'app-root',
  imports: [ 
    CommonModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatIcon,
    MatToolbar,
    MatSidenav,
    MatSidenavContent,
    MatSidenavContainer,
    MatNavList,
    MatListItem,
    UserList,
    UserForms,
    UserSearch
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  standalone: true,
})
export class App {
  sidebarOpen = signal(true);
  currentView = signal('Get All User');
  
  navItems = [
    { name: 'Get All User', icon: 'people', view: 'get all user' },
    { name: 'Search User', icon: 'search', view: 'search user' },
    { name: 'Add User', icon: 'person_add', view: 'add user' },
  ];

  toggleSidebar() {
    this.sidebarOpen.update(open => !open);
  }

  navigateTo(view: string) {
    this.currentView.set(view);
  }
}
