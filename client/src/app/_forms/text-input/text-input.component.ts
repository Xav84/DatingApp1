import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() type = 'text';

  // @Self permet de ne pas reustiliser un control qui serait en memoire 
  // et d en utiliser un nouveau a chaque input
  constructor(@Self() public ngControl: NgControl) {
    // initialise la valeur de valueAccessor en TextInputComponent
    this.ngControl.valueAccessor = this;
   }

  writeValue(obj: any): void {
  }

  registerOnChange(fn: any): void {
  }

  registerOnTouched(fn: any): void {
  }

  // permet de caster ngControl.control en FormControl
  get control(): FormControl {
    return this.ngControl.control as FormControl
  }
}
