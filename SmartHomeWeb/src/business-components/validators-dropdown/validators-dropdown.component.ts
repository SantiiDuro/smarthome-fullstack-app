import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import ValidatorStatus from './models/validators.status';
import { Subscription } from 'rxjs';
import { CompanyService } from '../../backend/services/company/company.service';
import { FormDropdownComponent } from '../../components/form-elements/form-dropdown/form-dropdown.component';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-validators-dropdown',
  standalone: true,
  imports: [FormDropdownComponent, CommonModule, NgIf],
  templateUrl: './validators-dropdown.component.html',
  styles: ``
})
export class ValidatorsDropdownComponent implements OnInit, OnDestroy {
  @Input({ required: true }) name!: string;
  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) formField!: any;
  @Input({ required: true }) value: string | null = null;
  @Output() valueChange = new EventEmitter<string | null>();

  status: ValidatorStatus = {
    loading: true,
    validators: [],
  };

  private _validatorGetAllSubscription: Subscription | null = null;

  constructor(private readonly _companyService: CompanyService) {}

  ngOnDestroy(): void {
    this._validatorGetAllSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this._validatorGetAllSubscription = this._companyService
      .getValidators()
      .subscribe({
        next: (validators) => {
          this.status = {
            validators: validators.map((validator) => ({
              value: validator.validador,
              label: validator.validador,
            })),
          };
        },
        error: (error) => {
          this.status = {
            validators: [],
            error,
          };
        },
      });
  }

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
