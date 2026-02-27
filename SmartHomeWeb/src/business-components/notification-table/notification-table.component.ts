import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import NotificationBasicInfoModel from '../../backend/services/notification/models/NotificationBasicInfoModel';
import { NotificationService } from '../../backend/services/notification/notification.service';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { TableComponent } from '../../components/table/table.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';
import { BoolDropdownComponent } from '../bool-dropdown/bool-dropdown.component';
import { ButtonComponent } from '../../components/button/button.component';

@Component({
  selector: 'app-notification-table',
  standalone: true,
  imports: [FormComponent, FormInputComponent, TableComponent, FormButtonComponent, BoolDropdownComponent, ButtonComponent],
  templateUrl: './notification-table.component.html',
  styles: ``
})
export class NotificationTableComponent {
  selectedBool: string | null = null;
  notifications: Array<NotificationBasicInfoModel> = [];

  readonly formField: any = {
    tipoDispositivo: {
      name: "tipoDispositivo"
    },
    fechaDeCreacion: {
      name: "fechaDeCreacion"
    },
    leida: {
      name: "leida"
    }
  };

  readonly filterForm = new FormGroup({
    [this.formField.tipoDispositivo.name]: new FormControl("", []),
    [this.formField.fechaDeCreacion.name]: new FormControl("", []),   
    [this.formField.leida.name]: new FormControl("", [])
  });

  constructor(
    private readonly _notificationService: NotificationService,
  ) {}

  ngOnInit() {
    this.loadNotifications();
  }

  loadNotifications() {
    const tipoDispositivo = this.filterForm.get('tipoDispositivo')?.value;
    const fechaDeCreacion = this.filterForm.get('fechaDeCreacion')?.value;
    const leida = this.filterForm.get('leida')?.value;

    this._notificationService.getNotifications(tipoDispositivo, fechaDeCreacion, leida).subscribe({
      next: (response) => {
        this.notifications = response.map(notification => ({
          ...notification,
          fueLeida: notification.fueLeida ? 'Sí' : 'No'
        }));
      },
      error: (error) => {
        console.error('Error loading notifications', error);
      }
    });
  }

  onFilterSubmit() {
    this.loadNotifications();
  }

  markAsRead(){
    const tipoDispositivo = this.filterForm.get('tipoDispositivo')?.value;
    const fechaDeCreacion = this.filterForm.get('fechaDeCreacion')?.value;
    const leida = this.filterForm.get('leida')?.value;

    this._notificationService.markAsRead(tipoDispositivo, fechaDeCreacion, leida).subscribe({
      next: (response) => {
        this.loadNotifications();
      },
      error: (error) => {
        console.error('Error loading notifications', error);
      }
    });
  }
}
