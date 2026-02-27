import { NgIf } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';
import ListItem from '../../components/list/models/ListItem';
import { ListComponent } from '../../components/list/list.component';

@Component({
  selector: 'app-secondary-images',
  standalone: true,
  imports: [NgIf, FormComponent, FormInputComponent, FormButtonComponent, ListComponent],
  templateUrl: './secondary-images.component.html',
  styles: ``
})
export class SecondaryImagesComponent {
  public secondaryPhotos: string[] = [];
  public items: ListItem[] = [];

  @Output() secondaryPhotosChange = new EventEmitter<string[]>();

  readonly formField: any = {
    url: {
      name: 'url',
      required: 'URL de la foto requerida',
    }
  }

  public photoForm = new FormGroup({
    [this.formField.url.name]: new FormControl('', [
      Validators.required
    ])
  });

  addPhoto() {
    const photoUrl = this.photoForm.get(this.formField.url.name)?.value;
    if (photoUrl) {
      this.secondaryPhotos.push(photoUrl);
      this.items = this.secondaryPhotos.map(photo => ({ label: photo }));
      this.photoForm.reset();
      this.secondaryPhotosChange.emit(this.secondaryPhotos);
    }
  }
}
