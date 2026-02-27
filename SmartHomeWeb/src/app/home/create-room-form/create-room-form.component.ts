import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HomeService } from '../../../backend/services/home/home.service';
import CreateRoomModel from '../../../backend/services/home/models/CreateRoomModel';

@Component({
  selector: 'app-create-room-form',
  templateUrl: './create-room-form.component.html',
  styles: ``
})
export class CreateRoomFormComponent {
  selectedHome: string | null = null;

  readonly formField: any = {
    nombre: {
      name: "nombre",
      required: "Nombre es requerido",
    },
  };

  readonly createRoomForm = new FormGroup({
    [this.formField.nombre.name]: new FormControl("", [
      Validators.required,
    ]),
  });

  createStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _route: ActivatedRoute,
    private readonly _homeService: HomeService,
  ) { }

  ngOnInit(): void {
    this._route.params.subscribe((params) => {
      this.selectedHome = params['homeId'] || null;

      if (!this.selectedHome) {
        this._router.navigate(['/home', 'myHomes']);
      }
    });
  }

  public onSubmit(){
    this.createStatus = { loading: true };

    const formValue = this.createRoomForm.value;
    const model: CreateRoomModel = {
      nombre: formValue[this.formField.nombre.name] || '',
    };

    this._homeService.createRoom(this.selectedHome || '', model).subscribe({
      next: (response) => {
        this._router.navigate(['/home', 'myHomes']);
      },
      error: (error) => {
        this.createStatus = { error };
      }
    });
  }
}
