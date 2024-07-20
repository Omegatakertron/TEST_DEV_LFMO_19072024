import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

  constructor(private router: Router){

  }

  ngOnInit(){
    if(sessionStorage.getItem('SessionLogged') != null){
      this.router.navigate(["/home"])
    }

  }
}
