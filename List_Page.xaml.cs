using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ListView_Euroopa_riigid;

public partial class List_Page : ContentPage
{
    public ObservableCollection<Ruhm<string, Telefon>> telefonideruhmades { get; set; }
    Label lbl_list;
    ListView list;
    Button lisa, kustuta, btn_valifoto, btn_tableview, btn_lisa_andmed;
    EntryCell ec_nimetus, ec_tootja, ec_hind;
    ImageCell ic;
    private string lisafoto;
    TableView tableview;

    public List_Page()
    {
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
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center
        };
        btn_valifoto.Clicked += Btn_valifoto_Clicked;
        btn_tableview = new Button
        {
            Text = "Näita",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
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
            GroupHeaderTemplate = new DataTemplate(() =>
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
                        Children = { img, nimetus, hind }
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
                    new ViewCell { View = new Image { Source = ic.ImageSource, WidthRequest = 100, HeightRequest = 100 } }
                }
            },
            IsVisible = false
        };

        list.ItemTapped += List_ItemTapped;
        this.Content = new StackLayout
        {
            Padding = new Thickness(10),
            Children =
            {
                lbl_list,  
                list,  
                btn_valifoto, 
                btn_tableview, 
                tableview, 
                lisa,  
                kustuta  
            }
        };

    }

    private void Lisa_Clicked(object sender, EventArgs e)
    {
        string nimetus = ec_nimetus.Text?.Trim();
        string tootja = ec_tootja.Text?.Trim();
        string hindText = ec_hind.Text?.Trim();

        if (string.IsNullOrEmpty(nimetus) || string.IsNullOrEmpty(tootja) || string.IsNullOrEmpty(hindText))
        {
            Shell.Current.DisplayAlert("Viga", "Palun sisestage kõik andmed!", "OK");
            return;
        }

        if (!decimal.TryParse(hindText, out decimal hind))
        {
            Shell.Current.DisplayAlert("Viga", "Hind peab olema number!", "OK");
            return;
        }

        string telefonPilt = string.IsNullOrEmpty(lisafoto) ? "pilt.png" : lisafoto;

        var uusTelefon = new Telefon
        {
            Nimetus = nimetus,
            Tootja = tootja,
            Hind = (int)hind,
            Pilt = telefonPilt
        };

        var grupp = telefonideruhmades.FirstOrDefault(g => g.Nimetus == tootja);
        if (grupp != null)
        {
            grupp.Add(uusTelefon);
        }
        else
        {
            telefonideruhmades.Add(new Ruhm<string, Telefon>(tootja, new List<Telefon> { uusTelefon }));
        }

        Shell.Current.DisplayAlert("Edu", "Telefon edukalt lisatud!", "OK");

        ec_nimetus.Text = string.Empty;
        ec_tootja.Text = string.Empty;
        ec_hind.Text = string.Empty;
        lisafoto = null;
        ic.ImageSource = "pilt.png";
    }

    private void Btn_tableview_Clicked(object? sender, EventArgs e)
    {
        tableview.IsVisible = !tableview.IsVisible;
        btn_tableview.Text = tableview.IsVisible ? "Peida" : "Näita";
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

    private async void Btn_valifoto_Clicked(object sender, EventArgs e)
    {
        FileResult foto = await MediaPicker.Default.PickPhotoAsync();
        if (foto != null)
        {
            await SalvestaFoto(foto);
        }
    }

    private async Task SalvestaFoto(FileResult foto)
    {
        if (foto != null)
        {
            lisafoto = Path.Combine(FileSystem.CacheDirectory, foto.FileName);

            using Stream sourceStream = await foto.OpenReadAsync();
            using FileStream localFileStream = File.OpenWrite(lisafoto);
            await sourceStream.CopyToAsync(localFileStream);

            ic.ImageSource = ImageSource.FromFile(lisafoto);

            await Shell.Current.DisplayAlert("Edu", "Foto on edukalt salvestatud", "OK");
        }
    }
}
