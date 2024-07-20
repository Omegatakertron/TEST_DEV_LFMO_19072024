import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogInComponent } from './log-in/log-in.component';
import { PersonasFisicasComponent } from './personas-fisicas/personas-fisicas.component';
import { ReportesComponent } from './reportes/reportes.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {path: 'home', component: HomeComponent},
  {path: 'LogIn', component: LogInComponent, pathMatch: "full"},
  {path: 'Personas Fisicas', component: PersonasFisicasComponent},
  {path: 'Reportes', component: ReportesComponent},
  {path: '**', pathMatch:'full' , redirectTo:'LogIn'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
