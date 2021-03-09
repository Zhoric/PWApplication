import {Component, NgModule, OnInit} from '@angular/core';
import { UserService } from '../_services/user.service';
import {Transaction} from "../entities/Transaction"

@Component({
  selector: 'app-home',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})

export class TransactionsComponent implements OnInit {
  content?: string;
  transactions?: Transaction[];

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getTransactions().subscribe(
      data => {
        this.transactions = JSON.parse(data);
        this.content = data;
      },
      err => {
        this.content = JSON.parse(err.error).message;
      }
    );
  }
}
