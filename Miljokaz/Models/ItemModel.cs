using SQLite;

namespace Miljokaz.Models
{
	public class ItemModel
	{
		[PrimaryKey, AutoIncrement, Column("Id")]
		public int ItemId { get; set; }
		public string ItemDescription { get; set; }
		public string ItemCategory { get; set; }
		public string ItemCategoryHexClor { get; set; }
		public DateTime dateTime { get; set; }
		public float Amount { get; set; }


	}
}
