import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../environments/environment';
import { AuthService } from './auth.service';
import {
  ResponseMessageGeneric,
  BrowserTargetResponse,
  TargetResponse,
  ActivateTargetRequest,
  TargetBrowserQuery,
  CreateTargetRequest,
  UpdateTargetRequest
} from './target.models';

@Injectable({
  providedIn: 'root'
})
export class TargetService {
  constructor(private http: HttpClient, private authService: AuthService) { }

  getAll(query: TargetBrowserQuery) {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    let url = environment.api_base_uri + "/api/targets/?page=" + query.page + "&size=" + query.size;
    if (query.name != null && query.name.trim() !== '') {
      url += "&name=" + encodeURIComponent(query.name);
    }
    return this.http.get<BrowserTargetResponse>(url, { headers: headers });
  }

  get(id: string) {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<TargetResponse>(environment.api_base_uri + "/api/targets/" + id, { headers: headers });
  }

  create(model: CreateTargetRequest) {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    return this.http.post<TargetResponse>(environment.api_base_uri + "/api/targets/", model, { headers: headers });
  }

  edit(id: string, model: UpdateTargetRequest) {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    return this.http.post<TargetResponse>(environment.api_base_uri + "/api/targets/" + id, model, { headers: headers });
  }

  activate(id: string, model: ActivateTargetRequest) {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    return this.http.post<TargetResponse>(environment.api_base_uri + "/api/targets/" + id + "/activate", model, { headers: headers });
  }

  deactivate(id: string) {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.post<TargetResponse>(environment.api_base_uri + "/api/targets/" + id + "/deactivate", null, { headers: headers });
  }

  delete(id: string) {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.delete<ResponseMessageGeneric<boolean>>(environment.api_base_uri + "/api/targets/" + id, { headers: headers });
  }
}
