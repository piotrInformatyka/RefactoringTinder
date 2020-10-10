import { Component, OnInit } from '@angular/core';
import { Pagination, PaginationResult } from '../_models/pagination';
import { Message } from '../_models/message';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: Message[];
  pagination: Pagination;
  messageContainer = 'Nieprzeczytane';
  flagaOutbox = false;
  id: number;


  constructor(private userService: UserService, private authService: AuthService, private route: ActivatedRoute,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data =>{
      this.messages = data.messages.result;
      this.pagination = data.messages.pagination;
      // tslint:disable-next-line: radix
      this.id = parseInt(this.authService.decodedToken.nameid);
    });
  }
  loadMessages(){
    this.userService.getMessages(this.authService.decodedToken.nameid, this.pagination.currentPage,
                                this.pagination.itemsPerPage, this.messageContainer)
        .subscribe((res: PaginationResult<Message[]>) => {
          this.messages = res.result;
          this.pagination = res.pagination;
          if (res.result[0].recipientId === this.id) {
            this.flagaOutbox = false;
          }
          if (res.result[0].senderId === this.id){
            this.flagaOutbox = true;
          }

        }, error => {
          this.alertify.error(error);
        });
  }
  deleteMessage(id: number){
    this.alertify.confirm('Czy na pewno chcesz usunąć tę wiadomość?', () => {
      this.userService.deleteMessage(id, this.authService.decodedToken.nameid).subscribe(() => {
        this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
        this.alertify.success('Wiadomosć została usunięta');
      }, error => {
        this.alertify.error(error);
      });
    });
  }

  pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }
}
