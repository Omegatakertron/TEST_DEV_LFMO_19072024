import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.component.html',
  styleUrl: './reportes.component.scss'
})
export class ReportesComponent {
  constructor(private router: Router){

  }

  ngOnInit(){
    if(sessionStorage.getItem('SessionLogged') == null){
      this.router.navigate(["/Reportes"])
    }
  }
}
