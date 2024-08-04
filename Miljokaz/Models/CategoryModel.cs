using SQLite;


namespace Miljokaz.Models
{
	public class CategoryModel
	{
		[PrimaryKey, AutoIncrement, Column("Id")]
		public int Id { get; set; }
		public string ColorName { get; set; }
		public string ColorCode { get; set; }
		public int ColorStatus { get; set; }
		public string? CategoryName { get; set; }

	}
}
