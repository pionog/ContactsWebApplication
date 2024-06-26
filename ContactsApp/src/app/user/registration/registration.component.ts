import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserService } from '../../shared/user.service';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { catchError, of, tap } from 'rxjs';

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
    this.service.register().pipe(
      tap((res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
        }
        else {
          res.errors?.forEach((element: { code: any; }) => {
            switch (element.code) {
                case 'DuplicateEmail':
                  console.log('Duplicate email');
                  break;

              default:
                console.log('Other error');
                break;
            }
          });
        }
      }),
      catchError(err => {
        console.log(err);
        return of(null);
      })
    ).subscribe();
  }
}
