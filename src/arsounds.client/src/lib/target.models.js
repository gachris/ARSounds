"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
exports.TargetBrowserQuery = exports.BrowserQuery = exports.PaginationFilter = exports.TargetActivateModel = exports.TargetEditModel = exports.TargetBindingCreateModel = exports.TargetCreateModel = exports.TargetModel = void 0;
var TargetModel = /** @class */ (function () {
    function TargetModel() {
    }
    return TargetModel;
}());
exports.TargetModel = TargetModel;
var TargetCreateModel = /** @class */ (function () {
    function TargetCreateModel() {
    }
    return TargetCreateModel;
}());
exports.TargetCreateModel = TargetCreateModel;
var TargetBindingCreateModel = /** @class */ (function () {
    function TargetBindingCreateModel() {
    }
    return TargetBindingCreateModel;
}());
exports.TargetBindingCreateModel = TargetBindingCreateModel;
var TargetEditModel = /** @class */ (function () {
    function TargetEditModel() {
    }
    return TargetEditModel;
}());
exports.TargetEditModel = TargetEditModel;
var TargetActivateModel = /** @class */ (function () {
    function TargetActivateModel() {
    }
    return TargetActivateModel;
}());
exports.TargetActivateModel = TargetActivateModel;
var PaginationFilter = /** @class */ (function () {
    function PaginationFilter() {
    }
    return PaginationFilter;
}());
exports.PaginationFilter = PaginationFilter;
var BrowserQuery = /** @class */ (function (_super) {
    __extends(BrowserQuery, _super);
    function BrowserQuery() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return BrowserQuery;
}(PaginationFilter));
exports.BrowserQuery = BrowserQuery;
var TargetBrowserQuery = /** @class */ (function (_super) {
    __extends(TargetBrowserQuery, _super);
    function TargetBrowserQuery() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return TargetBrowserQuery;
}(BrowserQuery));
exports.TargetBrowserQuery = TargetBrowserQuery;
//# sourceMappingURL=target.models.js.map