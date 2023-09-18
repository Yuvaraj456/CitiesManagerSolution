export class City {
  cityId: string | null; //number or null
  cityName: string | null; //string or null

  constructor(cityId: string | null = null, cityName: string | null = null) {
    this.cityId = cityId;
    this.cityName = cityName;
  }

}
