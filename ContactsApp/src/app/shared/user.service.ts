import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private fb:FormBuilder, private http:HttpClient) { }

  //definiowanie formularza
  formModel = this.fb.group(
    {
      Name: [''],
      Surname: [''],
      Email: ['', [Validators.required, Validators.email]],
      Passwords: this.fb.group({
        Password: ['', [Validators.required, Validators.minLength(6)]],
        ConfirmPassword: ['', Validators.required]
      },{validator: this.comparePasswords}),
      Category: [''],
      SubCategory: [''],
      PhoneNumber: [''],
      BirthDate: ['']
    }
  )

  //funkcja sprawdzajaca hasla w formularzu
  comparePasswords(fb: FormGroup) {
    let confirmPswrdCtrl = fb.get('ConfirmPassword');
    if (confirmPswrdCtrl?.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors) {
      if (fb.get('Password')?.value != confirmPswrdCtrl?.value)
        confirmPswrdCtrl?.setErrors({ passwordMismatch: true });
      else
        confirmPswrdCtrl?.setErrors(null);
    }
  }

  //przepisywanie danych z formularza do wiadomosci wysylanej metoda POST
  register() {
    var body = {
      accountID: 0,
      name: this.formModel.value.Name,
      surname: this.formModel.value.Surname,
      email: this.formModel.value.Email,
      password: this.formModel.value.Passwords.Password,
      category: this.formModel.value.Category,
      subCategory: this.formModel.value.SubCategory,
      phoneNumber: this.formModel.value.PhoneNumber,
      birthDate: this.formModel.value.BirthDate,
    };
    return this.http.post(environment.apiBaseUrl + '/AccountDetail/Registration', body);
  }

  //ustawianie tokena w localstorage po wyslaniu wiadomosci metoda POST
  login(formData: any) {
    return this.http.post(environment.apiBaseUrl + '/AccountDetail/Login', formData).pipe(
      tap((res: any) => {
        localStorage.setItem('token', res.token);
      })
    );
  }

  //uzyskiwanie danych o koncie
  getAccountDetail(): Observable<any> {
    return this.http.get(environment.apiBaseUrl + '/UserProfile/GetAccountDetail', {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`
      }
    });
  }
}
