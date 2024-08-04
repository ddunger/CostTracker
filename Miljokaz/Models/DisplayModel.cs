namespace Miljokaz.Models
{
	public class DisplayModel
	{
		public int Id { get; set; }
		public string? Category { get; set; }
		public string? dateTimeString { get; set; }
		public string? AmountString { get; set; }
		public string? ColorCode { get; set; }
		public DateTime? CreatedDate { get; set; }
		public Color DisplayColor { get; set; }
		public string? Description { get; set; }
		public string? ColorName {get; set;	}
	}
}