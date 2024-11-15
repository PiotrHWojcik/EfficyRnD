using Swashbuckle.AspNetCore.Annotations;

namespace EfficyRnD.Models
{
    public class Team
    {
		[SwaggerSchema(ReadOnly = true)]
		public int Id { get; set; }
		public string? Name { get; set; }
    }
}