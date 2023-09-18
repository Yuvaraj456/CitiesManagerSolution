using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.Entities
{
    public class Cities
    {
        [Key]
        public Guid CityId { get; set; }

        [Required(ErrorMessage ="City Name is Required")]
        public string? CityName { get; set; }
    }
}
