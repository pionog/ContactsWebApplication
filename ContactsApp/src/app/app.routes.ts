import { Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';

export const routes: Routes = [
    {
        path:'',
        redirectTo:'/user/registration',
        pathMatch:'full'
    },

    {
        path:'user',
        component: UserComponent,
        children: [
            {
                path:'registration',
                component: RegistrationComponent
            }
        ]
    }
];
