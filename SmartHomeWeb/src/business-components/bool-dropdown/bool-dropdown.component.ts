import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import DropdownOption from '../../components/dropdown/models/DropdownOption';
import { FormDropdownComponent } from '../../components/form-elements/form-dropdown/form-dropdown.component';

@Component({
  selector: 'app-bool-dropdown',
  standalone: true,
  imports: [FormDropdownComponent],
  templateUrl: './bool-dropdown.component.html',
  styles: ``
})
export class BoolDropdownComponent {
  @Input({ required: true }) name!: string;
  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) formField!: any;
  @Input({ required: true }) value: string | null = null;
  @Output() valueChange = new EventEmitter<string | null>();

  bools: Array<DropdownOption> = [
    { value: 'true', label: 'Si' },
    { value: 'false', label: 'No' },
  ]

  public onChange(event: any): void {
    const newValue = event.target.value === 'null' ? null : event.target.value;
    this.valueChange.emit(newValue);
  
    this.form.get(this.name)?.setValue(newValue);
  }
  
  public onValueChange(newValue: string | null) {
    const finalValue = newValue === null ? '' : newValue;
    this.valueChange.emit(newValue);
    this.form.get(this.name)?.setValue(finalValue);
  }
}
