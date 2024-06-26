import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private fb:FormBuilder, private http:HttpClient) { }

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

  comparePasswords(fb: FormGroup) {
    let confirmPswrdCtrl = fb.get('ConfirmPassword');
    if (confirmPswrdCtrl?.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors) {
      if (fb.get('Password')?.value != confirmPswrdCtrl?.value)
        confirmPswrdCtrl?.setErrors({ passwordMismatch: true });
      else
        confirmPswrdCtrl?.setErrors(null);
    }
  }

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

  login(formData: any) {
    return this.http.post(environment.apiBaseUrl + '/AccountDetail/Login', formData);
  }

  getUserProfile() {
    return this.http.get(environment.apiBaseUrl + '/UserProfile');
  }
}
