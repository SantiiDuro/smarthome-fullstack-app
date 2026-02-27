import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-navbar-button',
  standalone: true,
  imports: [],
  templateUrl: './navbar-button.component.html',
  styles: ``
})
export class NavbarButtonComponent {
  @Input({ required: true }) title!: string;
  @Output() onClick = new EventEmitter<void>();

  handleClick() {
    this.onClick.emit();
  }
}
