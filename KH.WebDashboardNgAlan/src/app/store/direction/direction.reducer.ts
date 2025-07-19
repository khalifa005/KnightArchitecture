import { createAction, createReducer, on } from "@ngrx/store";
import { switchArDirection } from "./direction.action";


const initialState = "ar-SA";
export const directionReducer = createReducer(
    initialState, 
    on(switchArDirection, (state)=> {
        console.log(state)
        return state == "ar-SA" ? "en-US" : "ar-SA"} ))