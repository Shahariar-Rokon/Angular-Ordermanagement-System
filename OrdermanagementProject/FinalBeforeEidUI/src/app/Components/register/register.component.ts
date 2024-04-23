import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthserviceService } from '../../Services/authservice.service';
import { GeneralResponse } from '../../Models/common-model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  form!: FormGroup;
  status: GeneralResponse = { success: false, message: ' ' }
  constructor(private fb: FormBuilder, private router: Router, public authservice: AuthserviceService) { }
  onRegister() {
    this.status = { success: false, message: 'Registering...' };
    const { Name, Email, Password, confirmPassword } = this.form.value;
    this.authservice.register({ Name, Email, Password, confirmPassword }).subscribe({
      next: (res) => {
      console.log(res);
        this.status = res;
        if (res.success) {
          this.router.navigate(['/Account/login']);
        }
      },
      error: (err) => {
        console.log(err);
        this.status = { success: false, message: 'Some error on the server side' };
      }
    });
  }
  ngOnInit(): void {
    this.form = this.fb.group({
      Name: ['', Validators.required],
      Email: ['', Validators.required],
      Password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    })
  }

}

