import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.scss'
})
export class LogInComponent {
  constructor(private router: Router){ }

  login(){
    sessionStorage.setItem('SessionLogged', 'true');
    sessionStorage.setItem('IdAutenticador', '1');
    this.router.navigate(["/home"]);
  }
}
