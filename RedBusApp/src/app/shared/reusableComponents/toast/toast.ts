import { AfterViewInit, Component, ElementRef, Input, signal, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { ToastType } from '../../../models/toast.model.ts.js';


// Bootstrap types (comes from bootstrap.bundle.js at runtime)
declare const bootstrap: any;

@Component({
  selector: 'app-toast',
  imports: [],
  templateUrl: './toast.html',
  styleUrl: './toast.css',
})
export class Toast implements AfterViewInit {

  @Input() type: ToastType = 'info';
  @Input() autohide = true;
  @Input() delay = 4000; // ms

  @ViewChild('toastEl', { static: true }) toastEl!: ElementRef<HTMLDivElement>;

  message = signal<string>('');
  private instance: any;

  ngAfterViewInit() {
    if (!this.toastEl?.nativeElement) return;

    // Create bootstrap Toast instance once
    this.instance = new bootstrap.Toast(this.toastEl.nativeElement, {
      autohide: this.autohide,
      delay: this.delay
    });

    // When toast is hidden, clear message
    this.toastEl.nativeElement.addEventListener('hidden.bs.toast', () => {
      this.message.set('');
    });
  }

  /** **Ccall from parent to show a toast with a message */
  show(msg: string) {
    this.message.set(msg);
    
    // Reconfigure dynamic options if inputs changed at runtime
    // (bootstrap doesn't update options, so we recreate instance when needed)
    if (this.instance) {
      this.instance.hide();
    }
    this.instance = new bootstrap.Toast(this.toastEl.nativeElement, {
      autohide: this.autohide,
      delay: this.delay
    });
    this.instance.show();
  }

  /** Optional: manual close from close button */
  close() {
    if (this.instance) {
      this.instance.hide();
    }
  }

  get classes(): string {
    switch (this.type) {
      case 'success': return 'text-bg-success';
      case 'warning': return 'text-bg-warning';
      case 'danger': return 'text-bg-danger';
      default: return 'text-bg-info';
    }
  }

}

