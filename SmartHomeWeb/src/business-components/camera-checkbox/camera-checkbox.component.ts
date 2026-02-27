import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CheckboxComponent } from '../../components/checkbox/checkbox.component';

@Component({
  selector: 'app-camera-checkbox',
  standalone: true,
  imports: [CheckboxComponent],
  templateUrl: './camera-checkbox.component.html',
  styles: ``
})
export class CameraCheckboxComponent {
  @Input() detectaMovimiento = false;
  @Input() detectaPersona = false;
  @Input() usoExterior = false;
  @Input() usoInterior = false;

  @Output() optionsChange = new EventEmitter<{ 
    detectaMovimiento: boolean; detectaPersona: boolean; usoExterior: boolean; usoInterior: boolean;
  }>();

  checkOptions() {
    this.optionsChange.emit({
      detectaMovimiento: this.detectaMovimiento,
      detectaPersona: this.detectaPersona,
      usoExterior: this.usoExterior,
      usoInterior: this.usoInterior
    });
  }
}
