using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebApi.Model
{
    public class Cities
    {
        [Key]
        public Guid CityId { get; set; }


        public string? CityName { get; set; }
    }
}
