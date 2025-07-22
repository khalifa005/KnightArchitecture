import { Actions, createEffect, ofType } from "@ngrx/effects";
import { switchArDirection } from "./direction.action";
import { tap } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable()
export class DirectionEffects {

    savelanguage = createEffect(() => this.actions.pipe(
        //all actions will pass here and we interested in this one switchArDirection
        ofType(switchArDirection),
        tap((action) => {
            localStorage.setItem("lang", action.lang);
        }))
        , { dispatch: false } //without this line it will go into infinate loop because it will keep changing the value listing over and over 
    );
    constructor(private actions: Actions) {

    }

}