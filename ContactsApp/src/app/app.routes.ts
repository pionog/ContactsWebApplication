import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';


//klasa umozliwiajaca przekierowywanie uzytkownika na odpowiednie podstrony
export const routes: Routes = [
    {
        path:'',
        redirectTo:'/user/login',
        pathMatch:'full'
    },

    {
        path:'user',
        component: UserComponent,
        children: [
            {
                path:'registration',
                component: RegistrationComponent
            },
            { 
                path: 'login', 
                component: LoginComponent
            }
        ]
    },
    {
        path:'home',
        component: HomeComponent
    }
];
