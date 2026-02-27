import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CheckboxComponent } from '../../components/checkbox/checkbox.component';

@Component({
  selector: 'app-home-permissions-checkbox',
  standalone: true,
  imports: [CheckboxComponent],
  templateUrl: './home-permissions-checkbox.component.html',
  styles: ``
})
export class HomePermissionsCheckboxComponent {
  @Input() label: string | null = null;

  @Input() permisoAsociarDispositivos = false;
  @Input() permisoListarDispositivos = false;
  @Input() permisoNotificaciones = false;
  @Input() perimsoAdministrarCuartos = false;
  @Input() permisoModificarNombreDispositivos = false;

  @Output() optionsChange = new EventEmitter<{ 
    permisoAsociarDispositivos: boolean;
    permisoListarDispositivos: boolean;
    permisoNotificaciones: boolean;
    perimsoAdministrarCuartos: boolean;
    permisoModificarNombreDispositivos: boolean;
  }>();

  checkOptions() {
    this.optionsChange.emit({
      permisoAsociarDispositivos: this.permisoAsociarDispositivos,
      permisoListarDispositivos: this.permisoListarDispositivos,
      permisoNotificaciones: this.permisoNotificaciones,
      perimsoAdministrarCuartos: this.perimsoAdministrarCuartos,
      permisoModificarNombreDispositivos: this.permisoModificarNombreDispositivos
    });
  }
}
