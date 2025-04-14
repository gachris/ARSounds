export class Target {
  id: any;
  name: string;
  audio: string
  isActive: boolean;
  image?: string;
  openVisiontId?: string;
  isTrackable?: boolean;
  color?: string;
  rate?: number;
  created: Date;
  updated: Date;
}

export class BrowserQuery {
  page: number;
  size: number;
}

export class TargetBrowserQuery extends BrowserQuery {
  name: string;
}

export class CreateTargetRequest {
  name: string;
  audio: string;
}

export class UpdateTargetRequest {
  name: string;
  isTrackable: boolean;
  color: string;
}

export class ActivateTargetRequest {
  image: string;
  color: string;
}

export class Error {
  resultCode: string;
  message: any;
}

export class ResponseMessage {
  transactionId: any;
  statusCode: any;
  errors: Error[];
}

export class ResponseDoc<TResult> {
  result: TResult;
}

export class ResponseMessageGeneric<TResult> extends ResponseMessage {
  response: ResponseDoc<TResult>;
  constructor(obj: any) {
    super();
    this.response = obj as ResponseDoc<TResult>;
  }
}

export class PagedResponse<TResult> extends ResponseMessageGeneric<TResult> {
  page: number;
  size: number;
  firstPage: any
  lastPage: any
  totalPages: number;
  totalRecords: number;
  nextPage: any
  previousPage: any
}

export class BrowserTargetResponse extends PagedResponse<Target[]> {
  constructor(obj: any) {
    super(obj);
    this.response = obj as ResponseDoc<Target[]>;
  }
}

export class TargetResponse extends ResponseMessageGeneric<Target> {
  constructor(obj: any) {
    super(obj);
    this.response = obj as ResponseDoc<Target>;
  }
}

export class TargetBindingCreateModel {
  name: string;
  file: File | null;
}
