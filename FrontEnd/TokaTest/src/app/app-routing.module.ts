import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { PersonaFisicaComponent } from './persona-fisica/persona-fisica.component';
import { ReportesComponent } from './reportes/reportes.component';

const routes: Routes = [
  {path: 'Home', component: HomeComponent},
  {path: 'LogIn', component: LoginComponent, pathMatch: "full"},
  {path: 'Personas Fisicas', component: PersonaFisicaComponent},
  {path: 'Reportes', component: ReportesComponent},
  {path: '**', pathMatch: 'full', redirectTo: 'LogIn'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
