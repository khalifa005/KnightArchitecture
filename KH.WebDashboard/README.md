#  Dashboard

This project wish to be an alternative to the official and now older Nebular Starter Kit.
It has been designed to works with the last angular version (18), the last nebular packages (14) and customized with few essential (for me) packages:

-  ng-bootstrap
-  ngx-translate
-  secure-web-storage
-  @ngrx/store

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

---

## Core Services and Structure
### Application Core Services
1. oauthService: Validates the token (Oauth2).  
2. user.service: Manages user data, login, and logout.  
3. Router: Handles navigation (e.g., this.router.navigateByUrl("/unauthorized")).  
4. ActivatedRoute: Deals with route parameters.  

### Project Folder Structure
- auth: Contains user services, models, auth-callback-component, and unauthorized-component.  
- config: Contains application constants and integration URLs.  
- core: Manages configurations (e.g., API Gateway), interceptors, and requests.  
- dashboard: Contains dashboard components for numbers and tasks.  
- layout: Contains header, footer, sidebar, and main components.  
- shared: Shared components used across the application.  

---

## Syntax
Generate component:  
ng g c shop --flat --skip-tests  

Generate service:  
ng g s shop --flat --skip-tests  

Generate module:  
ng g m [name] --module app  

Lint and fix warnings:  
ng lint --fix  

Update Angular CLI and Core:  
ng update @angular/cli @angular/core  

---

## Questions
- How is the main component in the main folder being read?  
- What is the role of `app-routing.module`?  
- Can we use an injected service in a class constructor before the constructor runs?  

---

## Topics
- Structural Directives: Manipulate the DOM (e.g., *ngIf, *ngFor).  
- Best Practices for ngFor: Use trackBy to maintain the DOM efficiently.  
- Data Binding: Transfer data between parent and child using ViewChild or Input properties.  
- Content Projection: Render fragments using `<ng-content>`.  
- ViewChild: Available after the view is rendered; used to access child components.  

---

## Errors and Fixes
- Fixing `ps1 cannot be loaded because running scripts is disabled`:  
  https://www.c-sharpcorner.com/article/how-to-fix-ps1-can-not-be-loaded-because-running-scripts-is-disabled-on-this-sys/  

- Resolving version mismatch:  
  npm install --save @angular/cli@13.0.3  
  ng update @angular/cli@13.0.3 @angular/core@13.0.3  

- Update Angular:  
  ng update @angular/cli @angular/core  

---

## Notes
- Use `npm i webpack webpack-cli --save-dev` to install Webpack.  
- Use `npm i typescript` to add TypeScript support.  


# Angular Incremental Build Issues and Solutions

## Table of Contents
1. Common Issues
2. Solutions
3. References and Links

---

## Common Issues

### Incremental Build Issues
Angular's development server uses incremental builds to speed up recompilation. However, sometimes the incremental build does not detect all changes, leading to false errors.

### TypeScript Compiler Cache
The TypeScript compiler may cache previous states, causing stale errors that resolve upon restarting the server.

---

## Solutions

### Restarting the Server
Stopping the application and running `ng serve` again often resolves these issues.

### Clear Angular Cache
Run `ng cache clean` to clear Angular's build cache.

### Use Full Rebuild Mode
Angular CLI allows you to enforce a full rebuild for every change:
- Run `ng serve --aot` to enable Ahead-of-Time (AOT) compilation, ensuring a clean build.

### Delete Temporary Files
Remove the `dist/` folder and temporary TypeScript files to ensure a clean build:
1. Run `rm -rf dist/ node_modules/`
2. Reinstall dependencies: `npm install`
3. Start the server: `ng serve`

### Clear npm Cache
1. Remove `node_modules`: `rm -rf node_modules`
2. Clear npm cache: `npm cache clean --force`
3. Reinstall dependencies: `npm install`

---

## References and Links

### ChatGPT Windows
- Incremental Build Issues: https://chatgpt.com/g/g-U80yiDX6F-angular/c/673ae6db-8bcc-800a-b011-54a84e610aef
- Translation Example: https://chatgpt.com/c/675035e4-b0ac-800a-90ca-bc0ac5d2818b
- Input Examples: https://chatgpt.com/g/g-U80yiDX6F-angular/c/674ef6d1-3ed4-800a-b034-d7c1af68dd94
- dynamic table table usagew https://chatgpt.com/g/g-3s6SJ5V7S-askthecode-git-companion/c/675adf47-be08-800a-815a-b924e915f431
### Production Configuration
- https://chatgpt.com/c/6757dd6b-1780-800a-99e5-665be9ab771f
- https://chatgpt.com/c/6757e6da-3088-800a-8b0c-8494ee5ffab9

### Commit Examples
1. Changed the app production configuration and used `custom-webpack` to track large modules:  
   https://chatgpt.com/c/6757e6da-3088-800a-8b0c-8494ee5ffab9  
2. Updated TypeScript configuration files.



## sometimes angular show errors like this one and once i stop the app and ng serve again the error is gone
Incremental Build Issues: Angular's development server uses incremental builds to speed up recompilation. However, sometimes the incremental build does not detect all changes, leading to false errors.
TypeScript Compiler Cache: The TypeScript compiler may cache previous states, leading to stale errors that resolve upon restarting the server.

ng cache clean

6. Use Full Rebuild Mode
Angular CLI allows you to enforce a full rebuild for every change, which can avoid incremental build errors during development:

bash
Copy code
ng serve --aot

4. Delete Temporary Files
Delete the dist/ folder and temporary TypeScript files to ensure a clean build:

bash
Copy code
rm -rf dist/ node_modules/
npm install
ng serve


rm -rf node_modules
npm cache clean --force
npm install


gpt window
https://chatgpt.com/g/g-U80yiDX6F-angular/c/673ae6db-8bcc-800a-b011-54a84e610aef

trasnlatoin
https://chatgpt.com/c/675035e4-b0ac-800a-90ca-bc0ac5d2818b

input examples 
https://chatgpt.com/g/g-U80yiDX6F-angular/c/674ef6d1-3ed4-800a-b034-d7c1af68dd94 

## production configuratoin
https://chatgpt.com/c/6757dd6b-1780-800a-99e5-665be9ab771f
https://chatgpt.com/c/6757e6da-3088-800a-8b0c-8494ee5ffab9 

## commit name after changing production configuration
changed the app production configuration and used custom-webpack package to track large modules
https://chatgpt.com/c/6757e6da-3088-800a-8b0c-8494ee5ffab9
also 
updated ts configuration files commit
