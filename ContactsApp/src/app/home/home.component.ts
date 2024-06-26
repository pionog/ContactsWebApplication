import { Component, OnInit } from '@angular/core';
import { UserService } from '../shared/user.service';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styles: ``
})
export class HomeComponent implements OnInit {
  accountDetails: any;
  constructor(private service: UserService, private router: Router) { }

  ngOnInit(): void {
    this.service.getAccountDetail().pipe(
      tap(
        (res) => this.accountDetails = res,
        (err) => console.log(err)
      )
    ).subscribe();
  }
  

  onLogout() {
    localStorage.removeItem('token');
    this.router.navigateByUrl('/user/login');
  }
}
