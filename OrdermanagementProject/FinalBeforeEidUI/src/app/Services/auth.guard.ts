import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthserviceService } from "./authservice.service";

export const canActivate: (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree = 
(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const authService = inject(AuthserviceService);
  const router = inject(Router);
  if (authService.isLoggedIn()) {
    return true;
  } else {
    router.navigate(['/Account/login']);
    return false;
  }
};