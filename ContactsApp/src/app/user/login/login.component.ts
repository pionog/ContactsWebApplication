import { Component } from '@angular/core';
import { UserService } from '../../shared/user.service';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styles: ``
})
export class LoginComponent {
  formModel = {
    Email: '',
    Password: ''
  }
  
  constructor(private service: UserService) { }

  onSubmit(form: NgForm) {
    this.service.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
      },
      err => {
        console.log(err);
      }
    );
  }

}
