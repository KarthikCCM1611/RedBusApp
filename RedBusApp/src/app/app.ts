import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { UpperCasePipe } from '@angular/common';
import { AuthService } from './services/auth-service';

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterOutlet, UpperCasePipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('RedBusApp');
  currentTheme = signal<string>('Dark');
  authSrc = inject(AuthService);
  router = inject(Router);
  userEmail = signal<string>("");
  ngOnInit(): void {

  }
  logout() {
    this.authSrc.logout();
    this.router.navigateByUrl("/login")
  }

  toggleTheme() {
    let theme = this.currentTheme() === 'Dark' ? 'Light' : 'Dark';
    this.currentTheme.set(theme);
    document.documentElement.setAttribute('data-theme', theme.toLowerCase());
  }
}
