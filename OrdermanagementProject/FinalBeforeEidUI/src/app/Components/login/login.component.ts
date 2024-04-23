import { Component,  OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginResult } from '../../Models/common-model';
import { Router } from '@angular/router';
import { AuthserviceService } from '../../Services/authservice.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  frm!: FormGroup;
  status: LoginResult = { success: false, message: '', token: '' };
  constructor(private fb:FormBuilder,private router:Router,public authservice:AuthserviceService){}
  onPost() {
    this.status = { success: false, message: 'Wait...', token: '' };
    const { Email, Password } = this.frm.value; 
    
    this.authservice.login({ Email, Password }).subscribe({
      next: (res) => {
        this.status = res;
        if (res.success) {
          this.router.navigate(['/Dashboard']);
  
        }
      },
      error: (err) => {
        console.log(err);
        this.status = { success: false, message: 'Some error on the server side', token: '' };
      }
    });
  }
  ngOnInit(): void {
    this.frm = this.fb.group({
      Email: ['', Validators.required],
      Password: ['', Validators.required]
    });

    if (this.authservice.isLoggedIn()) {
      this.router.navigate(['/Dashboard']);
    }
  }
  
}
