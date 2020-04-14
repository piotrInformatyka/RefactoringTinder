import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { LikeComponent } from './likes/like.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { UserDetailComponent } from './users/user-detail/user-detail.component';
import { UserDetailResolver } from './_resolvers/user-datail.resolver';
import { UserListResolver } from './_resolvers/user-list.resolver';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { UserEditResolver } from './_resolvers/user-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {path: '',  runGuardsAndResolvers:
        'always',
         canActivate: [AuthGuard],
        children: [
            {path: 'uzytkownik/edycja', component: UserEditComponent, resolve: {user: UserEditResolver},
                canDeactivate: [PreventUnsavedChanges]},
            {path: 'uzytkownicy', component: UserListComponent, resolve: {users: UserListResolver}},
            {path: 'uzytkownicy/:id', component: UserDetailComponent, resolve: {user: UserDetailResolver}},
            {path: 'polubienia', component: LikeComponent},
            {path: 'wiadomosci', component: MessagesComponent},

        ]
    },
    {path: '**', redirectTo: '', pathMatch: 'full'},
];
