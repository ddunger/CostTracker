using Miljokaz.Models;
using Miljokaz.Views;
using Miljokaz.Data;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;


namespace Miljokaz.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
	

		#region LISTE

		private List<ItemModel> userItems;
		public List<ItemModel> UserItems
		{
			get { return userItems; }
			set
			{
				userItems = value;
				OnPropertyChanged(nameof(UserItems));
			}
		}

		private List<CategoryModel> userCategories;
		public List<CategoryModel> UserCategories
		{
			get { return userCategories; }
			set
			{
				userCategories = value;
				OnPropertyChanged(nameof(UserCategories));
			}
		}

		private DisplayModel _selectedItem;
		public DisplayModel SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				if (_selectedItem != value)
				{
					_selectedItem = value;
					OnPropertyChanged(nameof(SelectedItem));
					Debug.WriteLine("---------------------List item selected");
					SelectedEditItem();
				}
			}
		}

		private List<ChartEntry> chartData;

		public List<ChartEntry> ChartData
		{
			get { return chartData; }
			set
			{
				if (chartData != value)
				{
					chartData = value;
					OnPropertyChanged(nameof(ChartData));
				}
			}
		}

		private List<CategoryModel> categoryPickerList;

		public List<CategoryModel> CategoryPickerList
		{
			get { return categoryPickerList; }
			set
			{
				if (categoryPickerList != value)
				{
					categoryPickerList = value;
					OnPropertyChanged(nameof(CategoryPickerList));
				}
			}
		}

		private ObservableCollection<DisplayModel> displayData;

		public ObservableCollection<DisplayModel> DisplayData
		{
			get { return displayData; }
			set
			{
				if (displayData != value)
				{
					displayData = value;
					OnPropertyChanged(nameof(DisplayData));
				}
			}
		}
		

		public ObservableCollection<DisplayModel> DisplayAvailableColors { get; set; }


		#endregion

		#region Properties

		public DateTime currentDate { get; set; }

		private string categoryNameEntry;

		public string CategoryNameEntry
		{
			get { return categoryNameEntry; }
			set
			{
				if (categoryNameEntry != value)
				{
					categoryNameEntry = value;
					OnPropertyChanged(nameof(CategoryNameEntry));
				}
			}
		}
		private string itemDescriptionEntry;

		public string ItemDescriptionEntry
		{
			get { return itemDescriptionEntry; }
			set
			{
				if (itemDescriptionEntry != value)
				{
					itemDescriptionEntry = value;
					OnPropertyChanged(nameof(ItemDescriptionEntry));
				}
			}
		}
		private float itemAmountEntry;

		public float ItemAmountEntry
		{
			get { return itemAmountEntry; }
			set
			{
				if (itemAmountEntry != value)
				{
					itemAmountEntry = value;
					OnPropertyChanged(nameof(ItemAmountEntry));
				}
			}
		}

		private CategoryModel selectedCategory;

		public CategoryModel SelectedCategory
		{
			get { return selectedCategory; }
			set
			{
				if (selectedCategory != value)
				{
					selectedCategory = value;
					OnPropertyChanged(nameof(SelectedCategory));

				}
			}
		}

		private DisplayModel selectedColorItem;

		public DisplayModel SelectedColorItem
		{
			get { return selectedColorItem; }
			set
			{
				if (selectedColorItem != value)
				{
					selectedColorItem = value;
					OnPropertyChanged(nameof(SelectedColorItem));

					if (selectedColorItem != null)
					{
						SelectedCategoryColor = Color.FromArgb(selectedColorItem.ColorCode);
						Debug.WriteLine("SelectedCategoryColor " + SelectedCategoryColor);
					}
				}
			}
		}


		private Color selectedCategoryColor;

		public Color SelectedCategoryColor
		{
			get { return selectedCategoryColor; }
			set
			{
				if (selectedCategoryColor != value)
				{
					selectedCategoryColor = value;
					OnPropertyChanged(nameof(SelectedCategoryColor));
				}
			}
		}

		private PieChart chart;

		public PieChart Chart
		{
			get { return chart; }
			set
			{
				if (chart != value)
				{
					chart = value;
					OnPropertyChanged(nameof(Chart));
				}
			}
		}


		private DateTime _selectedStartDate;
		public DateTime SelectedStartDate
		{
			get => _selectedStartDate;
			set
			{
				if (_selectedStartDate != value)
				{
					_selectedStartDate = value;
					OnPropertyChanged(nameof(SelectedStartDate));
					UpdateChart();

				}
			}
		}

		private DateTime _selectedEndDate;
		public DateTime SelectedEndDate
		{
			get => _selectedEndDate;
			set
			{
				if (_selectedEndDate != value)
				{
					_selectedEndDate = value;
					OnPropertyChanged(nameof(SelectedEndDate));
					UpdateChart();
				}
			}
		}


		private int _itemId;
		public int ItemId
		{
			get { return _itemId; }
			set
			{
				if (_itemId != value)
				{
					_itemId = value;
					OnPropertyChanged(nameof(ItemId));


				}
			}
		}
		public DateTime EditDate { get; set; }
		public float EditAmount { get; set; }
		#endregion


		#region Gumbi

		public ICommand SelectDateRange { get; set; }
		public ICommand SelectAllItems { get; set; }
		public ICommand SelectNewItem { get; set; }
		public ICommand SaveNewCategoryCommand { get; set; }
		public ICommand SaveNewItemCommand { get; set; }
		public ICommand SaveEditedTypeCommand { get; set; }
		public ICommand CancelEditItem { get; set; }
		public ICommand DeleteEditItem { get; set; }
		public ICommand BackButton { get; set; }


		#endregion



		public MainPageViewModel()
		{
			SelectAllItems = new Command(SelectedAllItemsAsync);
			SelectNewItem = new Command(SelectedNewItem);
			SaveNewCategoryCommand = new Command(InsertNewCategory);
			SaveNewItemCommand = new Command(InsertNewItem);
			SaveEditedTypeCommand = new Command(SaveEditedType);
			CancelEditItem = new Command(CancelEdit);
			DeleteEditItem = new Command(DeleteEdit);
			BackButton = new Command(GoBack);

			try
			{
				UserItems = new List<ItemModel>();
				UserCategories = new List<CategoryModel>();
				CategoryPickerList = new List<CategoryModel>();
				DisplayData = new ObservableCollection<DisplayModel>();

				App.DataRepository.Init();
				UserItems = App.DataRepository.GetAllItems();
				UserCategories = App.DataRepository.GetAllCategories();
				InitialiseColors();
				ConvertToDisplayData();
				DrawChart();
				SelectedEndDate = DateTime.Now;
				SelectedStartDate = App.DataRepository.GetOldestDate();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}



		#region Metode

		private void InitialiseColors()
		{
			try
			{
				
				App.DataRepository.AddColors();
				
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}


		private void DrawChart()
		{
			try
			{
				var localEntries = new Dictionary<(SKColor Color, string Label), float>();

				foreach (var itemModel in UserItems)
				{
					SKColor color = SKColor.Parse(itemModel.ItemCategoryHexClor);
					string label = $"{itemModel.ItemCategory}";

					var key = (color, label);
					if (localEntries.ContainsKey(key))
					{
						localEntries[key] += itemModel.Amount;
					}
					else
					{
						localEntries[key] = itemModel.Amount;
					}
				}

				var aggregatedEntries = localEntries.Select(entry => new ChartEntry(entry.Value)
				{
					Color = entry.Key.Color,
					ValueLabel = $"{entry.Value.ToString("F2")} €",
					Label = entry.Key.Label
				}).ToList();

				Chart = new PieChart { Entries = aggregatedEntries, LabelMode = LabelMode.RightOnly, GraphPosition = GraphPosition.AutoFill };
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}


		private void UpdateChart()
		{
			UserItems.Clear();
			UserItems = App.DataRepository.GetDataRange(SelectedStartDate, SelectedEndDate);
			DrawChart();
			ConvertToDisplayData();
		}


		public void EditItemById()
		{
			ItemModel itemModel = UserItems.FirstOrDefault(t => t.ItemId == ItemId);
			if (itemModel != null)
			{
				EditDate = itemModel.dateTime;
				EditAmount = itemModel.Amount;
			}

		}
		
	

		public void InsertNewItem()
		{
			try
			{
				if (SelectedCategory.CategoryName.ToString() == null)
				{

					Application.Current.MainPage.DisplayAlert("", "Please select a category!", "OK");

					return;
				}


				else if (SelectedCategory.ColorCode == null)
				{
					Application.Current.MainPage.DisplayAlert("", "Please select a color!", "OK");

					return;

				}
				else if (ItemAmountEntry == 0)
				{ 
					Application.Current.MainPage.DisplayAlert("", "Please add amount!", "OK");
					return;
				}

				else if (ItemAmountEntry != 0) { }
				{
					string category = SelectedCategory.CategoryName.ToString();
					string HexColor = SelectedCategory.ColorCode;
					currentDate = DateTime.Now;
					App.DataRepository.AddItem(new ItemModel { ItemCategory = category, dateTime = currentDate, Amount = ItemAmountEntry, ItemCategoryHexClor = HexColor, ItemDescription = ItemDescriptionEntry }); 

					UserItems.Clear();
					UserItems = App.DataRepository.GetAllItems();
					DisplayData.Clear();
					ConvertToDisplayData();
					Application.Current.MainPage.DisplayAlert("", "New expenditure added!", "OK");
					DrawChart();
				}
				

			}
			catch (Exception ex)
			{

				Debug.WriteLine(ex.ToString());
			}


		}
		public void ConvertToDisplayData()
		{
			try
			{
				
				DisplayData.Clear();


				var orderedUserItems = UserItems.OrderByDescending(item => item.dateTime);

				foreach (var itemModel in orderedUserItems)
				{
					string amountToString = itemModel.Amount.ToString();
					DisplayData.Add(new DisplayModel
					{
						Id = itemModel.ItemId,
						Category = itemModel.ItemCategory,
						dateTimeString = itemModel.dateTime.ToString("dd/MM/yy"),
						AmountString = string.Concat(amountToString + " €"),
						DisplayColor = Color.Parse(itemModel.ItemCategoryHexClor),
						Description = itemModel.ItemDescription,
						CreatedDate = itemModel.dateTime
					});
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}



		public async void InsertNewCategory() //pretumbati da se sprema u već kreiranu tablicu kao update
		{
			try
			{
				if (string.IsNullOrEmpty(CategoryNameEntry))
				{
					Application.Current.MainPage.DisplayAlert("", "Please enter category name!", "OK");
					return;

				}
				else if (SelectedColorItem != null)
				{
					Application.Current.MainPage.DisplayAlert("", "Please select color!", "OK");
					return;
				}
				else 
				{

					SKColor selectedColor = SKColor.Parse(selectedColorItem.ColorCode);
					int idToBeUpdated = selectedColorItem.Id;
					string newName = CategoryNameEntry;
					App.DataRepository.AddCategoryName(idToBeUpdated, newName);
					UserCategories.Clear();
					UserCategories = App.DataRepository.GetAllCategories();
					Application.Current.MainPage.DisplayAlert("", "New category added!", "OK");
					SelectedColorItem = null;

				}
			}
			catch (Exception ex)
			{

				Debug.WriteLine(ex.ToString());
			}
		}

		
		private async void SelectedEditItem() // samo select odabranog modela, bez kategorije?
		{
			if (SelectedItem != null)
			{
				ItemId = SelectedItem.Id;
				EditItemById();

				if (UserItems != null)
				{
					ItemModel matchingItemModel = UserItems.FirstOrDefault(itemModel => itemModel.ItemId == ItemId);

					//if (matchingItemModel != null)
					//{
					//	ItemModel matchingTypeModel = UserCategories.FirstOrDefault(typeModel => typeModel.Type == matchingDataModel.Type);

					//	if (matchingTypeModel != null)
					//	{
					//		SelectedCategory = matchingTypeModel;
					//	}
					//}
				}

				await Shell.Current.GoToAsync("//EditItem");
			}
			else
			{
				Debug.WriteLine("SelectedItem is null");
			}
		}
		public async void SaveEditedType() // isto presložiti
		{
			//try
			//{
			//	string typeString = SelectedCategory.Type.ToString();

			//	string HexColor = SelectedCategory.typeHexColor;
			//	string newType = SelectedCategory.Type;
			//	if (EditAmount != 0)
			//	{
			//		DataModel updatedModel = new DataModel
			//		{
			//			Id = ItemId,
			//			Type = newType,
			//			dateTime = EditDate,
			//			Amount = EditAmount,
			//			dataHexColor = HexColor
			//		};

			//		App.DataRepository.UpdateItem(ItemId, updatedModel);

			//		UpdateChart();

			//		await Shell.Current.GoToAsync("//AllItems");
			//		Application.Current.MainPage.DisplayAlert("", "Item successfully edited!", "OK");
			//	}
			//	else
			//	{
			//		Application.Current.MainPage.DisplayAlert("", "Please add amount!", "OK");

			//	}
			//}
			//catch (Exception ex)
			//{
			//	Application.Current.MainPage.DisplayAlert("", "Please select type!", "OK");

			//	Debug.WriteLine(ex.ToString());
			//}
		}
		#endregion


		#region Navigacija

		public async void SelectedNewItem()
		{
			await Shell.Current.GoToAsync("//NewItem");
			CategoryPickerList.Clear();
			CategoryPickerList = App.DataRepository.GetCategoryPickerList();

			DisplayAvailableColors = new ObservableCollection<DisplayModel>();

			foreach (var item in CategoryPickerList) 
			{
				var displayModel = new DisplayModel()
				{
					Id = item.Id,
					DisplayColor = Color.FromRgba(item.ColorCode),
					ColorName = item.ColorName,
					ColorCode = item.ColorCode,
				};

				DisplayAvailableColors.Add(displayModel);
			}


		}

		private async void SelectedAllItemsAsync()
		{
			await Shell.Current.GoToAsync("//AllItems");
			ConvertToDisplayData();

		}
		public async void CancelEdit()
		{
			await Shell.Current.GoToAsync("//AllItems");
		}

		public async void DeleteEdit()
		{
			App.DataRepository.Delete(ItemId);
			UpdateChart();
			await Shell.Current.GoToAsync("//AllItems");
			Application.Current.MainPage.DisplayAlert("", "Item successfully deleted!", "OK");

		}


		public async void GoBack()
		{
			await Shell.Current.GoToAsync("//MainPage");
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

	}
}
