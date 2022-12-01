import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  //Parce que le template est un child du component, on mets cela pour y acceder avec le nom du form 
  @ViewChild('editForm') editForm: NgForm | undefined;
  // Permet d'avoir acces au browser event et de notifier (voulez vous quitter) un member qui aurait fait des changements au form 
  // de changer de page ex: aller sur google.com
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }

  member: Member | undefined;
  user: User | null = null;

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
   }

  ngOnInit(): void {
    this.loadMember();
  }

  //va chercher les detail du membre via l'API
  loadMember() {
    if(!this.user) return;
    // appel la fonction getMember() de members.service.ts
    this.memberService.getMember(this.user.username).subscribe({
      next: member => this.member = member
    });
  }

  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.toastr.success('Profile updated successfully');
        // une fois les changements submit on reset le form avec les nouvelles donnés updaté
        this.editForm?.reset(this.member);
      }
    })
  }
}
