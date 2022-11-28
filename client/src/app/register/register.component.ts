import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  /* @Input() lets a parent component update data in the child component. Allo tu vas recevoir quelque chose de ton parent
  Conversely, @Output() lets the child send data to a parent component.*/
  //@Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {}

  constructor(private acountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.acountService.register(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: error => console.log(error)
    })
  }

  //Envoi au parent un boolean false
  cancel() {
    this.cancelRegister.emit(false);
  }
}