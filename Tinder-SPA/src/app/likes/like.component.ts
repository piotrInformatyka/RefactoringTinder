import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { Pagination, PaginationResult } from '../_models/pagination';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-like',
  templateUrl: './like.component.html',
  styleUrls: ['./like.component.css']
})
export class LikeComponent implements OnInit {

  users: User[];
  pagination: Pagination;
  likesParam: string;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data.users.result;
      this.pagination = data.users.pagination;
    });
    this.likesParam = 'UserLikes';
  }

  loadUsers(){
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.likesParam)
      .subscribe((res: PaginationResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    }, error => {this.alertify.error(error); });
   }
   pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }
}
