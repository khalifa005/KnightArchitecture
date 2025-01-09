#  Dashboard


## Node Version server

This project is tested to work with Node 22.10.0 64bit in Windows enviroment, but maybe can works also with other settings.


# Angular and Development Shortcuts and Concepts

## Table of Contents
1. Shortcuts
2. Commands
3. Concepts
4. Core Services and Structure
5. Syntax
6. Questions
7. Topics
8. Version Information
9. Errors and Fixes

---

## Shortcuts
ng g c --flat --skip-tests=true component-name  
ng g c --skip-tests=true component-name  
ng g s --flat service-name  

## auto generate the .net APIs and models
npm run generate-client-sdk 
https://openapi-ts.dev/introduction
npx openapi-typescript http://localhost:5000/swagger/v1/swagger.json -o src/open-api/schema.d.ts

---

## Commands
To install node modules:  
npm install --legacy-peer-deps --save radium  

To build with source map:  
npm run build -- --configuration production --sourceMap=true  

Run source map explorer:  
source-map-explorer main.df1dc9d3cb8a3ac8.js  

Run Angular app:  
ng serve -o  
ng serve -o --port 4300  

Uninstall modules:  
npm uninstall <name of the module>  
npm uninstall <name of the module> --save  
npm uninstall <name of the module> --save-dev  

---

## Concepts
1. RxJS: A library for reactive programming using Observables to compose asynchronous or callback-based code.  
2. ActivatedRouteSnapshot: Contains information about a route associated with a component loaded in an outlet at a particular moment.  
3. ActivatedRoute: Contains information about a route associated with a component loaded in an outlet.  
4. CanActivate Interface: Checks if the current user has permission to activate the requested route.  
5. Angular Directives: Extend HTML with new syntax. Directives can be used in elements or attributes. Examples:  
   - []: Receives something.  
   - (): Performs something.  
6. npm install: Adds dependencies to `package.json`.  
7. webpack: Configures how the application is built.  
8. Environment Variables: Add variables to the environment folder for later use.  
9. String Interpolation: {{var-name}}.  
10. Data Binding:  
    - One-way: From source to target view (event binding).  
    - Two-way: Use [(target)]="expression" (e.g., [(ngModel)]="value").  
11. Bootstrap [MainComponent]: The main component acts as a worker for managing other workers.  
12. Track Elements: Use trackBy with index number and object to prevent DOM loss.  
13. Dependency Injection: Declare services in the `providers` array in `app.module` or add `provideIn: root` in the service file.  
14. router-outlet: Determines which component is visible.  

