import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shake, shakeX, tada } from 'ng-animate';
import { lastValueFrom, timer } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    animations: [
      trigger('ng_shake', [
        transition(':increment', useAnimation(shake, {
          params: { timing: 2}
        }))
      ]),
      trigger('ng_bounce', [
        transition(':increment', useAnimation(bounce, {
          params: { timing: 4}
        }))
      ]),
      trigger('ng_tada', [
        transition(':increment', useAnimation(tada, {
          params: { timing: 3}
        }))
      ])
    ]
})
export class AppComponent {
  title = 'ngAnimations';

  rotate : boolean = false;
  ng_shake : number = 0;
  ng_bounce : number = 0;
  ng_tada : number = 0;
  keepPlayingAnimation : boolean = false;

  constructor() {
  }

  faireTourner() {
    this.rotate = true;
    setTimeout(() => {this.rotate = false;}, 2000);
  }

  async waitFor(delayInSeconds:number) {
  await lastValueFrom(timer(delayInSeconds * 1000));
  }

  async animerUneFois() {
    this.ng_shake++;
    await this.waitFor(2);
    this.ng_bounce++;
    await this.waitFor(3);
    this.ng_tada++;
    await this.waitFor(3);
  }

  startStopAnimationEnBoucle() {
    if (this.keepPlayingAnimation){
      this.keepPlayingAnimation = false;
    }
    else {
      this.keepPlayingAnimation = true;
      this.animerEnBoucle();
    }
  }

  async animerEnBoucle(){
    while (this.keepPlayingAnimation) {
      await this.animerUneFois()
    }
  }

}
