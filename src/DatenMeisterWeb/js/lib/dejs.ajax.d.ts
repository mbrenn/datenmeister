/// <reference path="jquery.d.ts" />
export declare class AjaxSettings {
    public type: string;
    public data: any;
    public prefix: string;
    public contentType: string;
}
export declare function reportError(serverResponse: string): void;
export declare function loadWebContent(data: any): void;
export declare function performRequest(data: any): void;
export declare function showFormFailure(prefix: string, message: string): void;
