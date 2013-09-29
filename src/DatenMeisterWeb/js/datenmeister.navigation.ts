/// <reference path="lib/backbone.d.ts" />

/*
 * Stores the last navigation points
 */
var lastNavigations = new Array<string>();

export function to(navigationUrl: string): void {
    lastNavigations.push(navigationUrl);
    Backbone.history.navigate(navigationUrl, { trigger: true });
}

/* 
  Just adds a point to navigation history without moving to the given url 
*/
export function add(navigationUrl: string): void {
    lastNavigations.push(navigationUrl);
}

export function back() :void {
    if (lastNavigations.length <= 1) {
        // If nothing gets found, return to 'login'
        to("login");
        return;
    }

    var currentRoute = lastNavigations.pop();
    var lastRoute = lastNavigations.pop();
    to(lastRoute);
}