import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-authentication-page',
  templateUrl: './authentication-page.component.html',
  styles: ``
})
export class AuthenticationPageComponent {
  constructor(private readonly _router: Router) {}

  goToLogin() {
    this._router.navigate(["/login"]);
  }

  goToRegister() {
    this._router.navigate(['/register']);
  }
}
