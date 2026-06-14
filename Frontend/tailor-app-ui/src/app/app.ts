import { Component, OnInit, HostListener } from '@angular/core';
import { Router, RouterOutlet, RouterLink, RouterLinkActive, NavigationEnd } from '@angular/router';
import { AuthService } from './services/auth.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {

  sidebarOpen = true;
  isMobile    = false;
  profileOpen = false;

  constructor(
    public authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.checkScreenSize();
    this.router.events.pipe(
      filter(e => e instanceof NavigationEnd)
    ).subscribe(() => {
      if (this.isMobile) this.sidebarOpen = false;
      this.profileOpen = false;
    });
  }

  @HostListener('window:resize')
  onResize(): void { this.checkScreenSize(); }

  checkScreenSize(): void {
    this.isMobile   = window.innerWidth <= 768;
    this.sidebarOpen = !this.isMobile;
  }

  toggleSidebar(): void { this.sidebarOpen = !this.sidebarOpen; }

  onNavClick(): void {
    if (this.isMobile) this.sidebarOpen = false;
  }

  toggleProfile(): void { this.profileOpen = !this.profileOpen; }
  closeProfile(): void  { this.profileOpen = false; }

  get initials(): string {
    const user = this.authService.getCurrentUser();
    if (!user) return '?';
    return (user.firstName?.[0] ?? '') + (user.lastName?.[0] ?? '');
  }

  logout(): void {
    this.authService.logout();
    this.profileOpen = false;
    this.router.navigate(['/login']);
  }
}