import { createAction, props } from "@ngrx/store";

export const switchArDirection = createAction("switchArDirection", props<{lang:string}>());