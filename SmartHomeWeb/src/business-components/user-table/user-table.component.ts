import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { TableComponent } from '../../components/table/table.component';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';
import { UserService } from '../../backend/services/user/user.service';
import UserBasicInfoModel from '../../backend/services/user/models/UserBasicInfoModel';

@Component({
  selector: 'app-user-table',
  standalone: true,
  imports: [TableComponent, CommonModule, FormComponent, FormInputComponent, FormButtonComponent],
  providers: [DatePipe],
  templateUrl: './user-table.component.html',
  styles: ``
})
export class UserTableComponent {
  users: UserBasicInfoModel[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;

  readonly formField: any = {
    rol: {
      name: "rol"
    },
    nombreCompleto: {
      name: "nombreCompleto"
    },
  };

  readonly filterForm = new FormGroup({
    [this.formField.rol.name]: new FormControl("", []),
    [this.formField.nombreCompleto.name]: new FormControl("", []),
  });

  constructor(
    private readonly _userService: UserService,
    private datePipe: DatePipe
  ) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    const rol = this.filterForm.get('rol')?.value;
    const nombreCompleto = this.filterForm.get('nombreCompleto')?.value;

    this._userService.getUsers(this.currentPage, this.pageSize, rol, nombreCompleto).subscribe({
      next: (response) => {
        this.users = response.usuarios.map(user => ({
          ...user,
          fechaCreacion: this.datePipe.transform(user.fechaCreacion, 'dd/MM/yyyy')
        }));
        this.totalPages = response.cantidadPaginas;
      },
      error: (error) => {
        console.error('Error loading companies', error);
      }
    });
  }

  onFilterSubmit() {
    this.currentPage = 1;
    this.loadUsers();
  }

  goToPreviousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadUsers();
    }
  }

  goToNextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadUsers();
    }
  }
}
