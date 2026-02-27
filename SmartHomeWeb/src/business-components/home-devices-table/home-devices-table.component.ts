import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { HomeService } from '../../backend/services/home/home.service';
import HomeDeviceBasicInfoModel from '../../backend/services/home/models/HomeDeviceBasicInfoModel';
import { ActivatedRoute, Router } from '@angular/router';
import { TableComponent } from '../../components/table/table.component';
import { CommonModule } from '@angular/common';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';

@Component({
  selector: 'app-home-devices-table',
  standalone: true,
  imports: [TableComponent, CommonModule, FormComponent, FormInputComponent, FormButtonComponent],
  templateUrl: './home-devices-table.component.html',
  styles: ``
})
export class HomeDevicesTableComponent {
  selectedHome: string | null = null;
  devices: Array<HomeDeviceBasicInfoModel> = [];

  readonly formField: any = {
    nombreCuarto: {
      name: 'nombreCuarto',
    }
  }

  readonly filterForm = new FormGroup({
    [this.formField.nombreCuarto.name]: new FormControl('', [])
  });

  constructor(
    private readonly _homeService: HomeService,
    private readonly _route: ActivatedRoute,
    private readonly _router: Router,
  ) { }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      this.selectedHome = params["homeId"];
      if(!this.selectedHome) {
        this._router.navigate(["/home", "myHomes"]);
      }
      this.loadHomeDevices();
    });
  }

  loadHomeDevices() {
    const nombreCuarto = this.filterForm.get("nombreCuarto")?.value;

    this._homeService.getHomeDevices(nombreCuarto, this.selectedHome || "").subscribe({
      next: (response) => {
        this.devices = response;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  onFilterSubmit() {
    this.loadHomeDevices();
  }
}
