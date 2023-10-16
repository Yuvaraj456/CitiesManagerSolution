import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterUser } from '../models/register-user';
import { Observable } from 'rxjs';
import { LoginUser } from '../models/login-user';

const API_BASE_URL:string = "https://localhost:7263/api/v1/Account";

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  public currentUserName:string|null=null;

  constructor(private httpClient:HttpClient) { 

  }

public postRegister(registerUser:RegisterUser):Observable<any>
  {

    return this.httpClient.post<any>(`${API_BASE_URL}/PostRegister`,registerUser )
  }

  public postLogin(loginUser:LoginUser):Observable<any>
  {

    return this.httpClient.post<any>(`${API_BASE_URL}/PostLogin`,loginUser )
  }

  public getLogOut():Observable<string>
  {
  
    return this.httpClient.get<string>(`${API_BASE_URL}/GetLogOut`)
  }

}
