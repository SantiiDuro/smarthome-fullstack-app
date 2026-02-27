import { Component } from '@angular/core';
import { TableComponent } from '../../components/table/table.component';
import { CommonModule } from '@angular/common';
import MemberBasicInfoModel from '../../backend/services/home/models/MemberBasicInfoModel';
import { HomeService } from '../../backend/services/home/home.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-members-table',
  standalone: true,
  imports: [TableComponent, CommonModule],
  templateUrl: './members-table.component.html',
  styles: ``
})
export class MembersTableComponent {
  selectedHome: string | null = null;
  members: Array<MemberBasicInfoModel> = [];

  constructor(
    private readonly _homeService: HomeService,
    private readonly _route: ActivatedRoute,
    private readonly _router: Router
  ){}

  ngOnInit(){
    this._route.params.subscribe((params) => {
        this.selectedHome = params['homeId'];
        if(!this.selectedHome){
          this._router.navigate(['/home', 'myHomes']);
        }
        this.loadMembers();
    });
  }

  loadMembers(){
    this._homeService.getMembers(this.selectedHome || "" ).subscribe({
      next: (response) => {
        this.members = response;
      },
      error: (error) => {
        console.error(error);
      }
    });
  }
}
