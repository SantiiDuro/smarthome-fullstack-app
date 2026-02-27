import { Component, Input } from '@angular/core';
import ListItem from './models/ListItem';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-list',
  standalone: true,
  imports: [NgIf, NgFor],
  templateUrl: './list.component.html',
  styles: ``
})
export class ListComponent {
  @Input() items: Array<ListItem> = [];
  @Input() emptyMessage = 'No hay elementos para mostrar';
  @Input() title?: string;
}
