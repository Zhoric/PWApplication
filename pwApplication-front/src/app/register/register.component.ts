import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form: any = {
    username: null,
    email: null,
    password: null
  };
  isSuccessful = false;
  isSignUpFailed = false;
  errorMessage = '';

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    const { username, email, password, password2 } = this.form;

    this.authService.register(username, email, password, password2).subscribe(
      data => {
        console.log(data);
        this.isSuccessful = true;
        this.isSignUpFailed = false;
      },
      err => {
        let errors = err.error.errors;
        console.log(this.errorMessage);
        if(typeof errors === "string"){
          this.errorMessage = errors;
        }
        else this.errorMessage = (!isEmpty(errors.DisplayName) ?  "Name error: " + errors.DisplayName + "\n": "")
          + (!isEmpty(errors.Email) ?  "Email error: " + errors.Email + "\n": "")
          + (!isEmpty(errors.Password) ?  "Password error: " + errors.Password[0] + "\n": "");
        this.isSignUpFailed = true;
      }
    );
  }
}

function isEmpty(str: string) {
  return (!str || 0 === str.length);
}
