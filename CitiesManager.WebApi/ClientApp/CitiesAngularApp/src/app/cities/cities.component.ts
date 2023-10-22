import { Component } from '@angular/core';
import { FormGroup,FormControl, Validators, FormArray } from '@angular/forms';
import { City } from '../models/city';
import { CitiesService } from '../services/cities.service';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css']
})

export class CitiesComponent {

  cities: City[] = [];
  postCityForm: FormGroup;
  isPostCityFormSubmitted: boolean = false;

  putCityForm: FormGroup;
  editCityId: string | null = null;

  constructor(private citiesService: CitiesService,private accountService:AccountService) {

    this.postCityForm = new FormGroup({
      cityName: new FormControl(null, [Validators.required])
      });

    this.putCityForm = new FormGroup({
      cities: new FormArray([])
    });

  }
  get putCityFormArray():FormArray {
    return this.putCityForm.get("cities") as FormArray;
  }

  loadCities() {
    this.citiesService.getCities().
      subscribe({
        next: (response: City[]) => {
          this.cities = response;
          this.cities.forEach(city => {
            this.putCityFormArray.push(new FormGroup({
              cityId: new FormControl(city.cityId, [Validators.required]),
              cityName: new FormControl({ value: city.cityName, disabled: true }, [Validators.required]),
            }));
          });
        },
        error: (error: any) => { console.log(error) },
        complete: () => { }
      });
  }
  ngOnInit() {
    this.loadCities();

  }


  get postCity_CityNameControl():any {
    return this.postCityForm.controls['cityName'];
  }

  public postCitySubmitted() {
    //To Do: add logic here

    this.isPostCityFormSubmitted = true;
    console.log(this.postCityForm.value)
    this.citiesService.postCities(this.postCityForm.value).subscribe({
      next: (response: City) => {
        console.log(response)
        //this.loadCities();
        this.putCityFormArray.push(new FormGroup({
          cityId:new FormControl(response.cityId, [Validators.required]),
          cityName:new FormControl({value: response.cityName, disabled:true}, [Validators.required])
        }))
        this.cities.push(new City(response.cityId, response.cityName));

        this.postCityForm.reset();
        this.isPostCityFormSubmitted = false;
      },
      error: (error:any) => {
        console.log(error);
      },
      complete: () => {

      }
    });


    
  }

  //Executes when the clicks on 'Edit' button the for the particular city 
  editClicked(city: City): void {
    this.editCityId = city.cityId;
  }

  //executes when the clicks on 'Update' button after editing
  updateClicked(i:number):void {
    //To Do: add logic here
    this.citiesService.putCities(this.putCityFormArray.controls[i].value).subscribe({
      next: (response: string) => {
        console.log(response);

        this.editCityId = null;

        this.putCityFormArray.controls[i].reset(this.putCityFormArray.controls[i].value);
      },

      error: (error: any) => {
        console.log(error);
      },

      complete: () => {},
    });
  }

  deleteClicked(city:City, i:number):void{
  if(confirm(`Are you sure to delete this city: ${city.cityName}?`))
  {
    this.citiesService.deleteCities(city.cityId).subscribe({
      next:(response:string)=>{
        console.log(response);
        this.putCityFormArray.removeAt(i);
        this.cities.splice(i,1);
      },
      error:(error:any)=>{
        console.log(error)
      },
      complete:()=>{}
    });
  }


 }
 
 //execute when 'refresh' button is clicked
 refreshClicked():void{

  this.accountService.postGenerateNewToken().subscribe({
    next:(response:any)=>{
      localStorage["token"] = response.token;
      localStorage["refreshToken"] = response.refreshToken;
      this.accountService.currentUserName = response.email;
      this.loadCities();
    },
    error:(err:any)=>{console.log(err)},
    complete:()=>{}
  });

 }

}
