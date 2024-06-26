import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/user.service';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styles: ``
})
export class LoginComponent implements OnInit{
  formModel = {
    Email: '',
    Password: ''
  }
  
  constructor(private service: UserService, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem('token') != null)
      this.router.navigateByUrl('/home'); //jesli jest token w localstorage, to 
  }

  onSubmit(form: NgForm) {
    this.service.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token); //zapisanie tokena w localstorage
        this.router.navigateByUrl('/home'); //przejscie do podstrony home
      },
      err => {
        console.log(err);
      }
    );
  }

}
