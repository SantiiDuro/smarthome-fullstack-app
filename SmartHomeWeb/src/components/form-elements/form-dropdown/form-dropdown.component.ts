import { Component, EventEmitter, Input, Output } from '@angular/core';
import DropdownOption from '../../dropdown/models/DropdownOption';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DropdownComponent } from '../../dropdown/dropdown.component';
import { NgIf, NgStyle } from '@angular/common';

@Component({
  selector: 'app-form-dropdown',
  standalone: true,
  imports: [DropdownComponent, NgIf, NgStyle],
  templateUrl: './form-dropdown.component.html',
  styles: ``
})
export class FormDropdownComponent {
  @Input() label: string = '';
  @Input() placeholder: string = 'Selecciona una opción';
  @Input() emptyMessage: string = 'No hay opciones disponibles';
  @Input({ required: true }) value: string | null = null;
  @Input() options: Array<DropdownOption> = [];
  @Input({ required: true }) name!: string;
  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) formField!: any;

  @Output() valueChange = new EventEmitter<string | null>();

  get error() {
    const control = this.form.get(this.name)!;

    if (!control.errors || !control.touched) {
      return null;
    }

    const errorKey = Object.keys(control.errors)[0];

    return this.formField[this.name][errorKey];
  }

  public onValueChange(newValue: string | null) {
    this.valueChange.emit(newValue);
  }
}
