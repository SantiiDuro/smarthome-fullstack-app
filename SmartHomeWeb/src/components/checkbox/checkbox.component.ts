import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-checkbox',
  standalone: true,
  imports: [],
  templateUrl: './checkbox.component.html',
  styles: ``
})
export class CheckboxComponent {
  @Input() label: string | null = null;
  @Input() isChecked = false;
  @Output() isCheckedChange = new EventEmitter<boolean>();

  toggleCheckbox(event: any) {
    this.isChecked = event.target.checked;
    this.isCheckedChange.emit(event.target.checked);
  }
}
