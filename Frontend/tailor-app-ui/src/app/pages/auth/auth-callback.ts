import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AuthResponse } from '../../models/auth';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  template: `
    <div style="display:flex;align-items:center;justify-content:center;height:100vh;background:#0F0F0F;color:#888;font-family:Poppins,sans-serif;gap:12px;">
      <div style="width:20px;height:20px;border:2px solid #2A2A2A;border-top-color:#F5C518;border-radius:50%;animation:spin 0.7s linear infinite"></div>
      Signing you in...
      <style>@keyframes spin{to{transform:rotate(360deg)}}</style>
    </div>
  `
})
export class AuthCallback implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Token is in the URL fragment (#) — not sent to servers, not in access logs
    const fragment = this.route.snapshot.fragment ?? '';
    const params = new URLSearchParams(fragment);
    const token = params.get('token');

    if (!token) {
      this.router.navigate(['/login'], { queryParams: { error: 'no_token' } });
      return;
    }

    const authData: AuthResponse = {
      token,
      email:     params.get('email')     ?? '',
      firstName: params.get('firstName') ?? '',
      lastName:  params.get('lastName')  ?? '',
      role:      params.get('role')      ?? 'Staff',
      shopId:    params.get('shopId') ? Number(params.get('shopId')) : 1,
      expiry:    params.get('expiry')    ?? ''
    };

    this.authService.storeAuthData(authData);
    this.router.navigate(['/dashboard']);
  }
}
