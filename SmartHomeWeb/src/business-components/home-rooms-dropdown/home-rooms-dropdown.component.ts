import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormDropdownComponent } from '../../components/form-elements/form-dropdown/form-dropdown.component';
import { CommonModule, NgIf } from '@angular/common';
import HomeRoomStatus from './models/home-room.status';
import { Subscription } from 'rxjs';
import { HomeService } from '../../backend/services/home/home.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-home-rooms-dropdown',
  standalone: true,
  imports: [FormDropdownComponent, CommonModule, NgIf],
  templateUrl: './home-rooms-dropdown.component.html',
  styles: ``
})
export class HomeRoomsDropdownComponent {
  @Input({ required: true }) name!: string;
  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) formField!: any;
  @Input({ required: true }) value: string | null = null;
  @Output() valueChange = new EventEmitter<string | null>();

  selectedHome: string | null = null;
  
  status: HomeRoomStatus = {
    loading: true,
    rooms: [],
  }

  private _homeRoomsGetAllSubscription: Subscription | null = null;

  constructor(
    private readonly _homeService: HomeService,
    private readonly _router: Router,
    private readonly _route: ActivatedRoute
  ) {}

  ngOnDestroy(): void {
    this._homeRoomsGetAllSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this._route.params.subscribe((params) => {
      this.selectedHome = params['homeId'] || null;

      if (!this.selectedHome) {
        this._router.navigate(['/home', 'myHomes']);
      }

      this._homeRoomsGetAllSubscription = this._homeService
      .getHomeRooms(this.selectedHome || "")
      .subscribe({
        next: (rooms) => {
          this.status = {
            rooms: rooms.map((room) => ({
              value: room.id,
              label: `Nombre: ${room.nombre}`,
            })),
          };
        },
        error: (error) => {
          this.status = {
            rooms: [],
            error,
          };
        },
      });
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
