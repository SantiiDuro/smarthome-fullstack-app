import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from '../guards/auth.guard';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { noAuthGuard } from '../guards/no-auth.guard';

const routes: Routes = [
  {
    path: '',
    canActivate: [noAuthGuard],
    loadChildren: () =>
      import('./authentication/authentication.module').then((m) => m.AuthenticationModule)
  },
  {
    path: 'home',
    canActivate: [authGuard],
    loadChildren: () => import('./home/home.module').then((m) => m.HomeModule),
  },
  {
    path: '**',
    component: PageNotFoundComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { } 
 