export interface Error {
  result_code: string;
  message: any;
}

export interface IResponseMessage {
  transaction_id: any;
  status_code: any;
  errors: Error[];
}

export interface IResponseMessageGeneric<TResult> extends IResponseMessage {
  response: ResponseDoc<TResult>;
}

export class ResponseMessage implements IResponseMessage {
  transaction_id: any;
  status_code: any;
  errors: Error[];
}

export class ResponseDoc<TResult> {
  result: TResult;
}

export class ResponseMessageGeneric<TResult> extends ResponseMessage implements IResponseMessageGeneric<TResult> {
  response: ResponseDoc<TResult>;
  constructor(obj: any) {
    super();
    this.response = obj as ResponseDoc<TResult>;
  }
}

export class PagedResponse<TResult> extends ResponseMessageGeneric<TResult>
{
  page: number;
  size: number;
  first_page: any
  last_page: any
  total_pages: number;
  total_records: number;
  next_page: any
  previous_page: any
}
