using Swashbuckle.AspNetCore.Annotations;

namespace EfficyRnD.Models
{
    public class Counter
    {
		[SwaggerSchema(ReadOnly = true)]
		public int Id { get; set; }
		public int UserId { get; set; }
		public int TeamId { get; set; }
		public string? Name { get; set; }
		public int Value { get; set; } = 0;

		public void Increment() { Value++; }
	}
}