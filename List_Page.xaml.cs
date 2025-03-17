using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ListView_Euroopa_riigid;

public partial class List_Page : ContentPage
{
	public ObservableCollection<Telefon> telefons {  get; set; }
	Label lbl_list;
	ListView list;
	Button lisa, kustuta;
    public List_Page()
	{
        lisa = new Button { Text = "Lisa felefon" };
        kustuta = new Button { Text = "Kustuta telefn" };
        telefons = new ObservableCollection<Telefon>
		{
			new Telefon {Nimetus = "Samsung Galaxy S22 Ultra", Tootja="Samsung", Hind=1349, Pilt=""},
            new Telefon {Nimetus = "Xiaomi Mi 11 Lite 5G NE", Tootja="Xiaomi", Hind=399, Pilt=""},
            new Telefon {Nimetus = "Xiaomi Mi 11 Lite 5G", Tootja="Xiaomi", Hind=339, Pilt = ""},
            new Telefon {Nimetus = "iPhone 13", Tootja="Apple", Hind=1179, Pilt = ""},
        };
		lbl_list = new Label
		{
			Text = "Telefonide loetelu",
			HorizontalOptions = LayoutOptions.Center,
			FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
		};
		list = new ListView
		{ 
			HasUnevenRows = true,
			ItemsSource = telefons,
			ItemTemplate = new DataTemplate(()=>
			{
				ImageCell imageCell = new ImageCell { TextColor = Color.FromArgb("#FF0000"), DetailColor = Color.FromArgb("#2E8B57") };
				imageCell.SetBinding(ImageCell.TextProperty, "Nimetus");
				Binding companyBinding = new Binding { Path="Tootja", StringFormat = "Tore telefon firmait {0}"};
				imageCell.SetBinding(ImageCell.DetailProperty, companyBinding);
				imageCell.SetBinding(ImageCell.ImageSourceProperty, "Pilt");
				return imageCell;
			})
		};
        list.ItemTapped += List_ItemTapped;
        //list.ItemSelected += List_ItemSelected;
		this.Content = new StackLayout { Children = { lbl_list, list} };
	}

    private async void List_ItemTapped(object? sender, ItemTappedEventArgs e)
    {
		Telefon selectedPhone = e.Item as Telefon;
		if (selectedPhone != null)
			await DisplayAlert("Valitud mudel", $"{selectedPhone.Tootja} - {selectedPhone.Nimetus}", "OK");
    }

    private void List_ItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null) 
			lbl_list.Text = e.SelectedItem.ToString();
    }
}