import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormDropdownComponent } from '../../components/form-elements/form-dropdown/form-dropdown.component';
import { CommonModule, NgIf } from '@angular/common';
import { FormGroup } from '@angular/forms';
import HomesStatus from './models/homes.status';
import { Subscription } from 'rxjs';
import { HomeService } from '../../backend/services/home/home.service';
import { DropdownComponent } from '../../components/dropdown/dropdown.component';

@Component({
  selector: 'app-user-homes-dropdown',
  standalone: true,
  imports: [DropdownComponent, CommonModule, NgIf],
  templateUrl: './user-homes-dropdown.component.html',
  styles: ``
})
export class UserHomesDropdownComponent {
  @Input({ required: true }) name!: string;
  @Input({ required: true }) value: string | null = null;
  @Output() valueChange = new EventEmitter<string | null>();

  status: HomesStatus = {
    loading: true,
    homes: [],
  };

  private _homesGetAllFromUserSubscription: Subscription | null = null;

  constructor(private readonly _homeService: HomeService) {}

  ngOnDestroy(): void {
    this._homesGetAllFromUserSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this._homesGetAllFromUserSubscription = this._homeService
      .getHomesFromUser()
      .subscribe({
        next: (homes) => {
          this.status = {
            homes: homes.map((home) => ({
              value: home.id,
              label: `Calle: ${home.calle}, Número: ${home.numPuerta}` + 
              (home.alias ? `, Alias: ${home.alias}` : ""),
            })),
          };
        },
        error: (error) => {
          this.status = {
            homes: [],
            error,
          };
        },
      });
  }

  public onChange(event: any): void {
    const newValue = event.target.value === 'null' ? null : event.target.value;
    this.valueChange.emit(newValue);
  }
  
  public onValueChange(newValue: string | null) {
    this.valueChange.emit(newValue);
  }
}
