/// <reference path="../jquery/jquery.d.ts" />

export class String {
    encodeHtml(text: string) {
        return $("<div></div>").text(text).html();
    }

    nl2br(text: string) {
        return text.replace(/\n/g, "<br />");
    }
}
