import { Component } from '@angular/core';
import { AuthserviceService } from './Services/authservice.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'OrderManagementUI';
  isLoggedIn!:boolean;
  constructor(private authService :AuthserviceService){ }
  checkLoggedInUser(){this.isLoggedIn= this.authService.isLoggedIn();}
  logout(){ this.authService.logout(); }
}
