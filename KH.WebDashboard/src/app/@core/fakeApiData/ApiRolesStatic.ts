import { LookupResponse } from "../models/base/response/lookup.model";
//log all selected permisisons and selected role id 
//if user clicks on the label of the permission select it  
//get roles from api 
//get all permissions from api then filter by the selected roles 
//update the role permissoin by api put  
//do the translation for all erlated permission page
//add the users table and roles table and department table with related actoin delete add updated 

export const StaticRoles: LookupResponse[] = [
    new LookupResponse(1, 'مستخدم', 'user', 'User'),
    new LookupResponse(2, 'مدير', 'admin', 'Admin'),
    new LookupResponse(3, 'ضيف', 'guest', 'Guest'),
    new LookupResponse(4, 'مشرف', 'supervisor', 'Supervisor'),
    new LookupResponse(5, 'موظف', 'employee', 'Employee'),
];
