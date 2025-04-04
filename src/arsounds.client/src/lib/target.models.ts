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

export class PagedResponse<TResult> extends ResponseMessageGeneric<TResult> {
  page: number;
  size: number;
  first_page: any
  last_page: any
  total_pages: number;
  total_records: number;
  next_page: any
  previous_page: any
}

export class BrowserTargetResponse extends PagedResponse<TargetModel[]> {
  constructor(obj: any) {
    super(obj);
    this.response = obj as ResponseDoc<TargetModel[]>;
  }
}

export class TargetResponse extends ResponseMessageGeneric<TargetModel> {
  constructor(obj: any) {
    super(obj);
    this.response = obj as ResponseDoc<TargetModel>;
  }
}

export class TargetModel {
  id: any;
  description: string;
  title: string;
  audio_type: string;
  audio_base64: string
  jpeg_base64: string;
  image_base64: string;
  is_active: boolean;
  is_trackable: boolean;
  hex_color: string;
  rate?: number;
  created: Date;
  updated: Date;
}

export class TargetBindingCreateModel {
  description: string;
  file: File | null;
}

export class CreateTargetRequest {
  description: string;
  filename: string;
  audio_base64: string;
  audio_type: string;
}

export class UpdateTargetRequest {
  description: string;
  is_trackable: boolean;
  hex_color: string;
}

export class TargetActivateRequest {
  png_base64: string;
  hex_color: string;
}

export class PaginationFilter {
  page: number;
  size: number;
}

export class BrowserQuery extends PaginationFilter {
  search_text: string;
}

export class TargetBrowserQuery extends BrowserQuery {
  description: string;
  created: Date;
}
