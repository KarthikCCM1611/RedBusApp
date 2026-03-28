import { Component, inject, signal, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { User } from '../../models/class/User';
import { Router, RouterLink } from "@angular/router";
import { Toast } from '../../shared/reusableComponents/toast/toast';
import { ToastType } from '../../models/toast.model.ts';
import { AuthService } from '../../services/auth-service';
import { IAuthResponse } from '../../models/interface/AuthResponse';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  imports: [FormsModule, RouterLink, Toast],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  registerObj = new User();

  @ViewChild('toast') toast!: Toast;
  toastType = signal<ToastType>('info');
  delay = signal<number>(2000);

  authSrc = inject(AuthService);
  router = inject(Router);

  onRegister(form: NgForm): void {
    // Template-driven form validation guard
    if (form.invalid || this.registerObj.password !== this.registerObj.confirmPassword) {
      this.toastType.set('danger');
      this.toast.show('Form is invalid');
      return;
    }
    this.authSrc.register(this.registerObj).subscribe({
      next: (res: IAuthResponse) => {
          this.toastType.set('success');
          this.authSrc.setToken(res.accessToken)
          this.router.navigateByUrl("/home");
      },
      error: (error: HttpErrorResponse) => {
        this.toastType.set('danger');
        this.toast.show(error.error.message);
      }
    })
  }
}
