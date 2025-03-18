using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ListView_Euroopa_riigid;

public partial class List_Page : ContentPage
{
    //public List<Telefon> telefons { get; set;}
    //public ObservableCollection<Telefon> telefons {  get; set; }
    public ObservableCollection<Ruhm<string, Telefon>> telefonideruhmades {  get; set; }
    Label lbl_list;
	ListView list;
	Button lisa, kustuta, btn_valifoto, btn_tableview;
    EntryCell ec_nimetus, ec_tootja, ec_hind;
    ImageCell ic;
    private string lisafoto;
    TableView tableview;

    public List_Page()
	{
        // Поля ввода
        ec_nimetus = new EntryCell
        {
            Label = "Nimetus:",
            Placeholder = "Sisesta telefoni nimi"
        };
        ec_tootja = new EntryCell
        {
            Label = "Tootja:",
            Placeholder = "Sisesta tootja"
        };
        ec_hind = new EntryCell
        {
            Label = "Hind:",
            Placeholder = "Sisesta hind", 
            Keyboard = Keyboard.Numeric
        };
        ic = new ImageCell
        {
            ImageSource = ImageSource.FromFile("pilt.png"),
        };
        btn_valifoto = new Button
        {
            Text = "Valige foto",
            BackgroundColor = Color.FromArgb("#FFE4E1"),
            TextColor = Color.FromArgb("#FF0000"),
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            BorderWidth = 2,
            BorderColor = Color.FromArgb("#CD5C5C"),
            HorizontalOptions = LayoutOptions.Center
        };
        btn_valifoto.Clicked += Btn_valifoto_Clicked;
        btn_tableview = new Button
        {
            Text = "Näita",
            BackgroundColor = Color.FromArgb("#FFE4E1"),
            TextColor = Color.FromArgb("#FF0000"),
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            BorderWidth = 2,
            BorderColor = Color.FromArgb("#CD5C5C"),
            HorizontalOptions = LayoutOptions.Center
        };
        btn_tableview.Clicked += Btn_tableview_Clicked;


        lisa = new Button { Text = "Lisa felefon" };
        lisa.Clicked += Lisa_Clicked;
        kustuta = new Button { Text = "Kustuta telefn" };
        kustuta.Clicked += Kustuta_Clicked;
        var telefonid = new List<Telefon>
		{
			new Telefon {Nimetus = "Samsung Galaxy S22 Ultra", Tootja="Samsung", Hind=1349, Pilt="samsung.jpg"},
            new Telefon {Nimetus = "Xiaomi Mi 11 Lite 5G NE", Tootja="Xiaomi", Hind=399, Pilt="xiaomine.jpg"},
            new Telefon {Nimetus = "Xiaomi Mi 11 Lite 5G", Tootja="Xiaomi", Hind=339, Pilt = "xiaomi.jpg"},
            new Telefon {Nimetus = "iPhone 13", Tootja="Apple", Hind=1179, Pilt = "iphone.jpg"},
            new Telefon {Nimetus = "iPhone 12", Tootja="Apple", Hind=1179, Pilt = "iphone12.jpg"}
        };
		var ruhmad = telefonid.GroupBy(p => p.Tootja).Select(g => new Ruhm<string, Telefon>(g.Key, g));
		telefonideruhmades = new ObservableCollection<Ruhm<string, Telefon>>(ruhmad);
		lbl_list = new Label
		{
			Text = "Telefonide loetelu",
			HorizontalOptions = LayoutOptions.Center,
			FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
		};
		list = new ListView
		{ 
			SeparatorColor = Color.FromArgb("#FFA500"),
			Header = "Telefonid rühmades",
			Footer = DateTime.Now.ToString("T"),

			HasUnevenRows = true,
			ItemsSource = telefonideruhmades,
			IsGroupingEnabled = true,
			GroupHeaderTemplate = new DataTemplate(()=>
			{
				Label tootja = new Label();
				tootja.SetBinding(Label.TextProperty, "Nimetus");
				return new ViewCell
				{
					View = new StackLayout
					{
						Padding = new Thickness(0, 5),
						Orientation = StackOrientation.Vertical,
						Children = { tootja }
					}
				};
			}),
			ItemTemplate = new DataTemplate(() =>
			{
				Image img = new Image { WidthRequest = 50, HeightRequest = 50 };
				img.SetBinding(Image.SourceProperty, "Pilt");
                Label nimetus = new Label { FontSize = 20 };
				nimetus.SetBinding(Label.TextProperty, "Nimetus");
				Label hind = new Label();
				hind.SetBinding(Label.TextProperty, "Hind");
				return new ViewCell
				{
					View = new StackLayout
					{
						Padding = new Thickness(0, 5),
						Orientation = StackOrientation.Vertical,
						Children = {img, nimetus, hind }
                    }
                };
			})
        };
        tableview = new TableView
        {
            Intent = TableIntent.Form, 
            Root = new TableRoot("Andmete sisestamine")
            {
                new TableSection("Nimetus:")
                {
                    ec_nimetus
                },
                new TableSection("Tootja:")
                {
                    ec_tootja
                },
                new TableSection("Hind:")
                {
                    ec_hind
                },
                new TableSection("Pilt:")
                {
                    ec_hind
                },
            },
            IsVisible = false
        };
        list.ItemTapped += List_ItemTapped;
        //list.ItemSelected += List_ItemSelected;
		this.Content = new StackLayout { Children = { lbl_list, list, lisa, kustuta} };
	}

    private void Btn_tableview_Clicked(object? sender, EventArgs e)
    {
        tableview.IsVisible = !tableview.IsVisible;
        btn_tableview.Text = tableview.IsVisible ? "Peida" : "Näita";
    }

    private void Lisa_Clicked(object? sender, EventArgs e)
    {
        var uusTelefon = new Telefon { Nimetus = "Uus Telefon", Tootja = "Uus Tootja", Hind = 200 };

        // Найти группу
        var grupp = telefonideruhmades.FirstOrDefault(g => g.Nimetus == uusTelefon.Tootja);
        if (grupp != null)
        {
            grupp.Add(uusTelefon);
        }
        else
        {
            telefonideruhmades.Add(new Ruhm<string, Telefon>(uusTelefon.Tootja, new List<Telefon> { uusTelefon }));
        }
    }

    private void Kustuta_Clicked(object? sender, EventArgs e)
    {
        Telefon phone = list.SelectedItem as Telefon;
        if (phone != null)
        {
            var grupp = telefonideruhmades.FirstOrDefault(g => g.Nimetus == phone.Tootja);
            if (grupp != null)
            {
                grupp.Remove(phone);
                if (grupp.Count == 0)
                    telefonideruhmades.Remove(grupp);
            }
            list.SelectedItem = null;
        }
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
    private async void Btn_valifoto_Clicked(object sender, EventArgs e)
    {
        FileResult foto = await MediaPicker.Default.PickPhotoAsync();
        await SalvestaFoto(foto);
    }
    // Метод для сохранения фото в локальное хранилище
    private async Task SalvestaFoto(FileResult foto)
    {
        if (foto != null)
        {
            // Сохраняем путь к файлу в переменную photoPath
            lisafoto = Path.Combine(FileSystem.CacheDirectory, foto.FileName);

            using Stream sourceStream = await foto.OpenReadAsync();
            using FileStream localFileStream = File.OpenWrite(lisafoto);
            await sourceStream.CopyToAsync(localFileStream);

            ic.ImageSource = ImageSource.FromFile(lisafoto);

            await Shell.Current.DisplayAlert("Edu", "Foto on edukalt salvestatud", "Ok");
        }
    }
}