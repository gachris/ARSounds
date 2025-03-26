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
