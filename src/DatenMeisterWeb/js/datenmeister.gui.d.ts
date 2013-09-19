/// <reference path="lib/jquery.d.ts" />
import d = require("datenmeister.objects");
import t = require("datenmeister.datatable");
export declare function showExtents(domElement: JQuery): void;
export declare function showObjectsByUri(uri: string, domElement: JQuery): void;
export declare function showObjects(data: d.JsonExtentData, domElement: JQuery, options?: t.TableOptions): t.DataTable;
