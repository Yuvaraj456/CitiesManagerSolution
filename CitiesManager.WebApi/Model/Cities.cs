using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebApi.Model
{
    public class Cities
    {
        [Key]
        public Guid CityId { get; set; }

        [Required(ErrorMessage ="City Name is Required")]
        public string? CityName { get; set; }
    }
}
