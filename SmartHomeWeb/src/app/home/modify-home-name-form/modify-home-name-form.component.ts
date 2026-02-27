import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HomeService } from '../../../backend/services/home/home.service';
import ModifyNameModel from '../../../backend/services/home/models/ModifyNameModel';

@Component({
  selector: 'app-modify-home-name-form',
  templateUrl: './modify-home-name-form.component.html',
  styles: ``
})
export class ModifyHomeNameFormComponent {
  selectedHome: string | null = null;

  readonly formField: any = {
    alias: {
      name: "alias",
      required: "Alias es requerido",
    },
  };

  readonly modifyHomeNameForm = new FormGroup({
    [this.formField.alias.name]: new FormControl("", [
      Validators.required,
    ]),
  });

  modifyStatus: {
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
    this.modifyStatus = { loading: true };

    const formValue = this.modifyHomeNameForm.value;
    const model: ModifyNameModel = {
      alias: formValue[this.formField.alias.name] || '',
    };

    this._homeService.modifyName(this.selectedHome || '', model).subscribe({
      next: (response) => {
        this._router.navigate(['/home', 'myHomes']);
      },
      error: (error) => {
        this.modifyStatus = { error };
      }
    });
  }
}
