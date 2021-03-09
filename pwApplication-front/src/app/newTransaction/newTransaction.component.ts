import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import {Observable} from 'rxjs';
import { startWith, debounceTime, distinctUntilChanged, switchMap, map } from 'rxjs/operators';
import {FormControl} from '@angular/forms';
import {Router} from "@angular/router";

export interface User {
  userId: string;
  displayName: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './newTransaction.component.html',
  styleUrls: ['./newTransaction.component.css']
})
export class NewTransactionComponent implements OnInit {
  errorMessage?: string;
  receiverUserId?: string;
  isSending = false;
  isSent = false;
  form: any = {
    receiverUserId: null,
    amount: null
  };

  newTransactionControl = new FormControl();
  filteredOptions: Observable<User[]>;

  constructor(private userService: UserService, private router: Router) {
    this.filteredOptions = this.newTransactionControl.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap(val => {
        return this.filter(val || '')
      })
    )
  }

  filter(val: string): Observable<User[]> {
    return this.userService.getUserByName(val)
      .pipe(
        map(response => JSON.parse(response).users)
      )
  }

  ngOnInit(): void { }

  onReceiverUserSelected(option: User){
    this.receiverUserId = option.userId;
  }

  onSubmit(): void {
    this.isSending = true;
    const { amount } = this.form;

    if (this.receiverUserId != null) {
      this.userService.commitTransaction(this.receiverUserId, amount).subscribe(
        data => {
          if(confirm("Payment completed. Do you want to create another one?")){
            return window.location.reload();
          }
          else{
            return this.router.navigate(['transactions'])
          }
        },
        err => {
          this.errorMessage = JSON.parse(err.error).message;
          this.isSending = false;
          this.isSent = true;
        }
      );
    }
  }
}
