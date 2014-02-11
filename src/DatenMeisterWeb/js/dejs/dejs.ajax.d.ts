/// <reference path="../jquery/jquery.d.ts" />
export declare class AjaxSettings {
    public type: string;
    public data: any;
    public prefix: string;
    public contentType: string;
}
export declare function reportError(serverResponse: string): void;
export declare function loadWebContent(data: any): void;
export interface PerformRequestSettings {
    url?: string;
    method?: string;
    data?: any;
    contentType?: string;
    success?: (data: any) => void;
    fail?: (data: any) => void;
    prefix?: string;
}
export declare function performRequest(data: PerformRequestSettings): void;
export declare function showFormFailure(prefix: string, failFunction: (data: any) => void): void;
