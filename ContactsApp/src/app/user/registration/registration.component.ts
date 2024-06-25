import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserService } from '../../shared/user.service';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [RouterOutlet, ReactiveFormsModule, HttpClientModule],
  templateUrl: './registration.component.html',
  styles: ``
})
export class RegistrationComponent {
  constructor(public service: UserService){}

  onSubmit() {
    this.service.register().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
        } else {
          res.errors.forEach((element: { code: any; }) => {
            switch (element.code) {
              case 'DuplicateUserName':
                break;

              default:
                break;
            }
          });
        }
      },
      err => {
        console.log(err);
      }
    );
  }
}
