import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any ={}
  
  constructor(public accountService: AccountService, private router: Router, 
    private toastr: ToastrService) { }

  ngOnInit(): void {
    
  }

  // on subscribe ici mais pas besoin de unsubscribe 
  // car c est une requete http et ca se fait automatiquement
  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => this.router.navigateByUrl('/members')
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/')
  }
}
