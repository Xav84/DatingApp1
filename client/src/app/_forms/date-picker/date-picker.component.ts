import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.css']
})
export class DatePickerComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() maxDate: Date | undefined;
  // le fait de mettre Partial permet de rendre tout les autre parametre de config optionel
  bsConfig: Partial<BsDatepickerConfig> | undefined;

  // @Self permet de ne pas reustiliser un control qui serait en memoire 
  // et d en utiliser un nouveau a chaque input
  constructor(@Self() public ngControl: NgControl) {
    // initialise la valeur de valueAccessor en TextInputComponent
    this.ngControl.valueAccessor = this;
    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: 'DD MMMM YYYY'
    }
   }

  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }
  
  ngOnInit(): void {
  }

  // permet de caster ngControl.control en FormControl
  get control(): FormControl {
    return this.ngControl.control as FormControl  }
}
