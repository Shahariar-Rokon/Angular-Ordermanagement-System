import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { ItemComponent } from './Components/item/item.component';
import { combineLatest } from 'rxjs';
import { DailyMenuComponent } from './Components/daily-menu/daily-menu.component';
import { canActivate } from './Services/auth.guard';
import { ReqviewComponent } from './Components/reqview/reqview.component';
import { SaleHeaderComponent } from './Components/sale-header/sale-header.component';
import { SaleheaderEditComponent } from './Components/saleheader-edit/saleheader-edit.component';
import { RecipeComponent } from './Components/recipe/recipe.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { RecipeEditComponent } from './Components/recipe-edit/recipe-edit.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {path: 'Dashboard', component: DashboardComponent, canActivate: [canActivate]},
  { path: 'Item', component: ItemComponent, canActivate: [canActivate] },
  {
    path: 'DailyMenu',
    component: DailyMenuComponent,
    canActivate: [canActivate],
  },
  {
    path: 'Requisition',
    component: ReqviewComponent,
    canActivate: [canActivate],
  },
  {
    path: 'SaleHeader', component: SaleHeaderComponent,canActivate: [canActivate],
  },
  {
    path: 'SaleHeader/edit/:id', component: SaleheaderEditComponent,canActivate: [canActivate],
  },
  {
    path: 'Recipe', component: RecipeComponent,canActivate: [canActivate],
  },
  {
    path: 'Recipe/edit/:id', component: RecipeEditComponent,canActivate: [canActivate],
  },
  { path: 'Account/login', component: LoginComponent },
  { path: 'Account/register', component: RegisterComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
