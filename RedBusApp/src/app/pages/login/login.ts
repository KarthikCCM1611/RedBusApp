import { Component, inject, signal, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from "@angular/router";
import { Login as LoginClass } from '../../models/class/Login';
import { Toast } from '../../shared/reusableComponents/toast/toast';
import { ToastType } from '../../models/toast.model.ts';
import { AuthService } from '../../services/auth-service';
import { IAuthResponse } from '../../models/interface/AuthResponse';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-login',
  imports: [RouterLink, ReactiveFormsModule, Toast],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  loginForm: FormGroup;
  authSrc = inject(AuthService);
  router = inject(Router);
  submitting = signal<boolean>(false);

  @ViewChild('toast') toast!: Toast;
  // title = signal<string>("");
  toastType = signal<ToastType>('info');
  delay = signal<number>(2000);

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],
      password: [
        '',
        [
          Validators.required,
          // Validators.minLength(6)
        ]
      ]
    });
  }
  onLogin() {
    if (this.loginForm.valid) {
      this.submitting.set(true);
      const data: LoginClass = this.loginForm.value;
      this.authSrc.login(data).subscribe({
        next: (res: IAuthResponse) => {
          this.submitting.set(false);
          this.toastType.set('success');
          this.toast.show('Login Success');
          this.authSrc.setToken(res.accessToken);
          debugger;
          this.authSrc.email();
          this.router.navigateByUrl("/home");
        },
        error: (error: HttpErrorResponse) => {
          this.submitting.set(false);
          this.toastType.set('danger');
          this.toast.show(error.message);
        }
      })
    }
  }

  // Convenience getters for template
  get email() { return this.loginForm.get('email'); }
  get password() { return this.loginForm.get('password'); }

}
