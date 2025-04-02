import { PagedResponse, ResponseDoc, ResponseMessageGeneric } from "../../core/api.response";
import { TargetModel } from "./target.models";

export class BrowserTargetResponse extends PagedResponse<TargetModel[]>{
  constructor(obj: any) {
    super(obj);
    this.response = obj as ResponseDoc<TargetModel[]>;
  }
}

export class TargetResponse extends ResponseMessageGeneric<TargetModel>{
  constructor(obj: any) {
    super(obj);
    this.response = obj as ResponseDoc<TargetModel>;
  }
}
