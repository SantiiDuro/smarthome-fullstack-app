import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormDropdownComponent } from '../../components/form-elements/form-dropdown/form-dropdown.component';
import DropdownOption from '../../components/dropdown/models/DropdownOption';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-sensor-type-dropdown',
  standalone: true,
  imports: [FormDropdownComponent],
  templateUrl: './sensor-type-dropdown.component.html',
  styles: ``
})
export class SensorTypeDropdownComponent {
  @Input({ required: true }) name!: string;
  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) formField!: any;
  @Input({ required: true }) value: string | null = null;
  @Output() valueChange = new EventEmitter<string | null>();

  types: Array<DropdownOption> = [
    { value: 'movimiento', label: 'Movimiento' },
    { value: 'ventana', label: 'Ventana' },
  ]

  public onChange(event: any): void {
    const newValue = event.target.value === 'null' ? null : event.target.value;
    this.valueChange.emit(newValue);
  
    this.form.get(this.name)?.setValue(newValue);
  }
  
  public onValueChange(newValue: string | null) {
    this.valueChange.emit(newValue);
    this.form.get(this.name)?.setValue(newValue);
  }
}
