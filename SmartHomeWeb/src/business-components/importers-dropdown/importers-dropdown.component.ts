import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormDropdownComponent } from '../../components/form-elements/form-dropdown/form-dropdown.component';
import { CommonModule, NgIf } from '@angular/common';
import { FormGroup } from '@angular/forms';
import ImporterStatus from './models/importers.status';
import { Subscription } from 'rxjs';
import { DeviceService } from '../../backend/services/device/device.service';

@Component({
  selector: 'app-importers-dropdown',
  standalone: true,
  imports: [FormDropdownComponent, CommonModule, NgIf],
  templateUrl: './importers-dropdown.component.html',
  styles: ``
})
export class ImportersDropdownComponent {
  @Input({ required: true }) name!: string;
  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) formField!: any;
  @Input({ required: true }) value: string | null = null;
  @Output() valueChange = new EventEmitter<string | null>();

  status: ImporterStatus = {
    loading: true,
    importers: [],
  };

  private _importerGetAllSubscription: Subscription | null = null;

  constructor(private readonly _deviceService: DeviceService) {}

  ngOnDestroy(): void {
    this._importerGetAllSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this._importerGetAllSubscription = this._deviceService
      .getImporters()
      .subscribe({
        next: (importers) => {
          this.status = {
            importers: importers.map((importer) => ({
              value: importer.importador,
              label: importer.importador,
            })),
          };
        },
        error: (error) => {
          this.status = {
            importers: [],
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
