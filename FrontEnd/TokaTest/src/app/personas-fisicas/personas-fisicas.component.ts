import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-personas-fisicas',
  templateUrl: './personas-fisicas.component.html',
  styleUrl: './personas-fisicas.component.scss'
})
export class PersonasFisicasComponent {

  constructor(private router: Router){}

  ngOnInit(){
    if(sessionStorage.getItem('SessionLogged') == null){
      this.router.navigate(["/LogIn"])
    }
  }
}
