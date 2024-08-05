using Miljokaz.Models;
using SQLite;

namespace Miljokaz.Data
{
	public class DataRepository
	{
		public string _dbPath;
		private SQLiteConnection conn;

		public DataRepository(string dbPath)
		{
			_dbPath = dbPath;
		}

		public void Init()
		{
			conn = new SQLiteConnection(_dbPath);
			conn.CreateTable<CategoryModel>();
			conn.CreateTable<ItemModel>();
		}
		public List<CategoryModel> GetCategoryPickerList()
		{
			Init();
			return conn.Table<CategoryModel>().Where(c => c.ColorStatus == 0).ToList();
		}

		public List<ItemModel> GetAllItems()
		{
			Init();
			return conn.Table<ItemModel>().ToList();
		}
		public List<ItemModel> GetDataRange(DateTime startDate, DateTime endDate)
		{
			Init();
			var query = conn.Table<ItemModel>().Where(data => data.dateTime >= startDate && data.dateTime <= endDate);
			return query.ToList();
		}
		public List<CategoryModel> GetAllCategories() 
		{
			Init();
			return conn.Table<CategoryModel>().Where(category => category.ColorStatus == 1).ToList();
		}

		public void AddItem(ItemModel itemModel)
		{
			conn = new SQLiteConnection(_dbPath);
			conn.Insert(itemModel);
		}

	
		public void DeleteItem(int itemId) 
		{
			using (var conn = new SQLiteConnection(_dbPath))
			{
				conn.Delete<ItemModel>(itemId);
			}
		}

		public int ColorCount() //za provjeru treba li kreirati tablice ako je count 0
		{
			conn = new SQLiteConnection(_dbPath);
			return conn.ExecuteScalar<int>("SELECT COUNT(*) FROM ColorsList");
		}

		public void AddColors() // insert boja u tablicu CategoryModel
		{
			{
				Init();

				int count = conn.Table<CategoryModel>().Count();
				if (count == 0)
				{
					foreach (var color in MyColors.ColorList)
					{
						var categoryModel = new CategoryModel
						{
							ColorName = color.ColorName,
							ColorCode = color.ColorHexCode,
							ColorStatus = color.Status
						};
						conn.Insert(categoryModel);
					}
				}
			}
		}

		public void AddCategoryName(int colorId, string categoryName) //markiranje da je boja iskorištena + NAZIV KATEGORIJE 
		{
			conn = new SQLiteConnection(_dbPath);

			var colorItem = conn.Table<CategoryModel>().FirstOrDefault(c => c.Id == colorId);

			if (colorItem != null)
			{
				colorItem.ColorStatus = 1;
				colorItem.CategoryName = categoryName;

				conn.Update(colorItem);
			}
		}

		public DateTime GetOldestDate()
		{
			conn = new SQLiteConnection(_dbPath);

			var oldestDate = conn.Table<ItemModel>().Min(x => x.dateTime);
			return oldestDate;
		}

		public void UpdateItem(int selectedID, ItemModel updatedModel)
		{
			ItemModel existingModel = conn.Table<ItemModel>().FirstOrDefault(model => model.ItemId == selectedID);

			if (existingModel != null)
			{
				existingModel.ItemCategory = updatedModel.ItemCategory;
				existingModel.dateTime = updatedModel.dateTime;
				existingModel.Amount = updatedModel.Amount;
				existingModel.ItemCategoryHexClor = updatedModel.ItemCategoryHexClor;
				existingModel.ItemDescription = updatedModel.ItemDescription;


				conn.Update(existingModel);
			}
		}
	}
}
