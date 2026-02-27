import { NgFor, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-table',
  standalone: true,
  imports: [NgIf, NgFor],
  templateUrl: './table.component.html',
  styles: ``
})
export class TableComponent {
  @Input() items: any[] = [];
  @Input() emptyMessage = 'No hay elementos disponibles para mostrar';
  @Input() title?: string;
  @Input() tableHeaders: string[] = [];
  @Input() tableColumns: string[] = [];
}
