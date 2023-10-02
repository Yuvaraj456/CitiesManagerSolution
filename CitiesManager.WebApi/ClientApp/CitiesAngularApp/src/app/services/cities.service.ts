import { Injectable } from '@angular/core';
import { City } from '../models/city'
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

const API_Base_Url: string = "https://localhost:7263/api/";
@Injectable({
  providedIn: 'root'
})


export class CitiesService {

  //cities: City[] =[];

  constructor(private httpClient:HttpClient)
  {
    
   
  }

  public getCities(): Observable<City[]> {

    let Headers = new HttpHeaders();
    Headers = Headers.append("Authorization", "Bearer mytoken");
    return this.httpClient.get<City[]>(`${API_Base_Url}v1/cities`, { headers: Headers });
  }

  public postCities(city:City): Observable<City> {

    let Headers = new HttpHeaders();
    Headers = Headers.append("Authorization", "Bearer mytoken");
    return this.httpClient.post<City>(`${API_Base_Url}v1/cities`,city, { headers: Headers });
  }

  public putCities(city: City): Observable<string> {
    let Headers = new HttpHeaders();
    Headers = Headers.append("Authorization", "Bearer mytoken");
    return this.httpClient.put<string>(`${API_Base_Url}v1/cities/${city.cityId}`, city, { headers: Headers });
  }

  public deleteCities(cityId:string | null): Observable<string> {
    let Headers = new HttpHeaders();
    Headers = Headers.append("Authorization", "Bearer mytoken");
    return this.httpClient.delete<string>(`${API_Base_Url}v1/cities/${cityId}`, { headers: Headers });
  }

}
