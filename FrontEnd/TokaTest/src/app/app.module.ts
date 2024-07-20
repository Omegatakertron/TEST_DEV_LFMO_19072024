import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NombreDelComponenteComponent } from './nombre-del-componente/nombre-del-componente.component';
import { PersonaFisicaComponent } from './persona-fisica/persona-fisica.component';
import { LoginComponent } from './login/login.component';
import { ReportesComponent } from './reportes/reportes.component';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    NombreDelComponenteComponent,
    PersonaFisicaComponent,
    LoginComponent,
    ReportesComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
