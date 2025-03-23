using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ListView_Euroopa_riigid;

public partial class EuroopaRiigid_Page : ContentPage
{
    public ObservableCollection<Euroopa> EuroopaRiigid { get; set; }

    Label lbl_list;
    ListView list;
    Button lisa, btn_valifoto, uuenda, kustuta, btn_tableview;
    EntryCell ec_nimetus, ec_pealinn, ec_rahvastiku_suurus, ec_info, ec_keel;
    Image ic;
    TableView tableview;
    private string lisafoto;
    private Euroopa andmeid;

    public EuroopaRiigid_Page()
    {
        lbl_list = new Label
        {
            Text = "Euroopa Riigid",
            HorizontalOptions = LayoutOptions.Center,
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
        };

        ec_nimetus = new EntryCell 
        { 
            Placeholder = "Sisesta riigi nimi" 
        };
        ec_pealinn = new EntryCell 
        { 
            Placeholder = "Sisesta riigi pealinn" 
        };
        ec_rahvastiku_suurus = new EntryCell 
        { 
            Placeholder = "Sisesta riigi rahvastik", 
            Keyboard = Keyboard.Numeric 
        };
        ec_info = new EntryCell 
        { 
            Placeholder = "Sisesta riigi info" 
        };
        ec_keel = new EntryCell 
        { 
            Placeholder = "Sisesta riigi keel" 
        };

        ic = new Image { WidthRequest = 60, HeightRequest = 60, Source = "pilt.png" };

        btn_valifoto = new Button
        {
            Text = "Vali lipp",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            WidthRequest = 200
        };
        btn_valifoto.Clicked += Btn_valifoto_Clicked;

        btn_tableview = new Button
        {
            Text = "Näita",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            WidthRequest = 160
        };
        btn_tableview.Clicked += Btn_tableview_Clicked;

        lisa = new Button 
        { 
            Text = "Lisa riik",  
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            WidthRequest = 200
        };
        lisa.Clicked += Lisa_Clicked;
        uuenda = new Button 
        { 
            Text = "Uuenda",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            WidthRequest = 160
        }; 
        uuenda.Clicked += Uuenda_Clicked;
        kustuta = new Button 
        { 
            Text = "Kustuta",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black
        };
        kustuta.Clicked += Kustuta_Clicked;

        EuroopaRiigid = new ObservableCollection<Euroopa>
        {
           new Euroopa {Nimetus = "Eesti", Pealinn="Tallinn", Rahvastiku_suurus=1369285, Lipp="eesti.jpg",
               Info="Eesti on parlamentaarne vabariik, kus rahvas teostab oma võimu Riigikogu valimiste kaudu. " +
                "Territoorium on jagatud 15 maakonnaks, 79 kohalikuks omavalitsuseks, millel on oma volikogu, " +
                "eelarve ja maksustamisõigus. Kohalikud omavalitsused võivad teha koostööd teistega, " +
                "et esindada tõhusalt huve erinevatel valitsemistasanditel.", Keel="eesti keel"},
            new Euroopa {Nimetus = "Soome", Pealinn="Helsinki", Rahvastiku_suurus=5681803, Lipp="soome.jpg", Info="", Keel=""},
            new Euroopa {Nimetus = "Läti", Pealinn="Riia", Rahvastiku_suurus=1369285, Lipp="lati.jpg", Info="", Keel=""},
            new Euroopa {Nimetus = "Leedu", Pealinn="Vilnius", Rahvastiku_suurus=169285, Lipp="leedu.jpg", Info="", Keel=""},
            new Euroopa {Nimetus = "Rootsi", Pealinn="Stockholm", Rahvastiku_suurus=1369285, Lipp="rootsi.jpg", Info="", Keel=""}
        };
        list = new ListView
        {
            SeparatorColor = Colors.DarkViolet,
            BackgroundColor = Colors.Cornsilk,
            Header = "Euroopa Riigid",
            Footer = DateTime.Now.ToString("T"),
            HasUnevenRows = true,
            ItemsSource = EuroopaRiigid,
            IsGroupingEnabled = false,
            ItemTemplate = new DataTemplate(() =>
            {
                Image img = new Image { WidthRequest = 50, HeightRequest = 50 };
                img.SetBinding(Image.SourceProperty, "Lipp");

                Label nimetus = new Label { FontSize = 18, FontAttributes = FontAttributes.Bold };
                nimetus.SetBinding(Label.TextProperty, "Nimetus");

                Label pealinn = new Label { FontSize = 16, FontAttributes = FontAttributes.Bold};
                pealinn.SetBinding(Label.TextProperty, new Binding("Pealinn", stringFormat: "Pealinn: {0}"));

                Label keel = new Label { FontSize = 16 };
                keel.SetBinding(Label.TextProperty, new Binding("Keel", stringFormat: "Keel: {0}"));

                Label rahvastik = new Label { FontSize = 16 };
                rahvastik.SetBinding(Label.TextProperty, new Binding("Rahvastiku_suurus", stringFormat: "Rahvastik: {0}"));

                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(10),
                        Children =
                        {
                            img,
                            new StackLayout
                            {
                                Orientation = StackOrientation.Vertical,
                                Padding = new Thickness(10, 0),
                                Children = { nimetus, pealinn, keel, rahvastik }
                            }
                        }
                    }
                };
            })
        };

        list.ItemTapped += List_ItemTapped;
        tableview = new TableView
        {
            Intent = TableIntent.Form, 
            Root = new TableRoot("Andmete sisestamine")
            {
                new TableSection("Nimetus:")
                {
                    ec_nimetus
                },
                new TableSection("Pealinn:")
                {
                    ec_pealinn
                },
                new TableSection("Rahvastik:")
                {
                    ec_rahvastiku_suurus
                },
                new TableSection("Info:")
                {
                    ec_info
                },
                new TableSection("Keel:")
                {
                    ec_keel
                },
                new TableSection("Pilt:")
                {
                    new ViewCell { View = new Image { Source = ic.Source, WidthRequest = 60, HeightRequest = 60 } }
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
                tableview,
                new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { btn_valifoto, btn_tableview }
                },
                new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { lisa, uuenda }
                },
                kustuta
            }
        };
    }

    private void Btn_tableview_Clicked(object? sender, EventArgs e)
    {
        tableview.IsVisible = !tableview.IsVisible;
        btn_tableview.Text = tableview.IsVisible ? "Peida" : "Näita";
    }
    private void Lisa_Clicked(object? sender, EventArgs e)
    {
        string nimetus = ec_nimetus.Text?.Trim();
        string pealinn = ec_pealinn.Text?.Trim();
        string info = ec_info.Text?.Trim();
        string keel = ec_keel.Text?.Trim();

        if (!int.TryParse(ec_rahvastiku_suurus.Text, out int rahvastik))
        {
            Shell.Current.DisplayAlert("Viga", "Rahvastiku suurus peab olema arv!", "OK");
            return;
        }

        if (string.IsNullOrEmpty(nimetus) || string.IsNullOrEmpty(pealinn) || string.IsNullOrEmpty(info) || string.IsNullOrEmpty(keel))
        {
            Shell.Current.DisplayAlert("Viga", "Palun sisestage kõik andmed!", "OK");
            return;
        }

        string lipp = string.IsNullOrEmpty(lisafoto) ? "pilt.png" : lisafoto;
        Euroopa uusRiik = new Euroopa
        {
            Nimetus = nimetus,
            Pealinn = pealinn,
            Rahvastiku_suurus = rahvastik,
            Lipp = lipp,
            Info = info,
            Keel = keel
        };

        EuroopaRiigid.Add(uusRiik);
    }

    private async void List_ItemTapped(object? sender, ItemTappedEventArgs e)
    {
        if (e.Item is Euroopa selectedRiik)
        {
            await DisplayAlert("Euroopa riigid", $"{selectedRiik.Nimetus} - {selectedRiik.Info}", "OK");

            andmeid = selectedRiik; 

            ec_nimetus.Text = andmeid.Nimetus;
            ec_pealinn.Text = andmeid.Pealinn;
            ec_rahvastiku_suurus.Text = andmeid.Rahvastiku_suurus.ToString();
            ec_info.Text = andmeid.Info;
            ec_keel.Text = andmeid.Keel;
            ic.Source = ImageSource.FromFile(andmeid.Lipp);
        }
    }


    private async void Btn_valifoto_Clicked(object sender, EventArgs e)
    {
        FileResult foto = await MediaPicker.Default.PickPhotoAsync();
        if (foto != null)
        {
            // Получаем путь и сохраняем в переменной lisafoto
            lisafoto = Path.Combine(FileSystem.CacheDirectory, foto.FileName);

            // Открываем поток и сохраняем фото в директории
            using Stream sourceStream = await foto.OpenReadAsync();
            using FileStream localFileStream = File.OpenWrite(lisafoto);
            await sourceStream.CopyToAsync(localFileStream);

            // Обновляем иконку с изображением
            ic.Source = ImageSource.FromFile(lisafoto);
        }
    }


    private void Kustuta_Clicked(object? sender, EventArgs e)
    {
        Euroopa euroopa = list.SelectedItem as Euroopa;
        if (euroopa != null)
        {
            EuroopaRiigid.Remove(euroopa);
            list.SelectedItem = null;
        }
    }

    private async void Uuenda_Clicked(object sender, EventArgs e)
    {
        if (andmeid == null)
        {
            Shell.Current.DisplayAlert("Viga", "Palun vali riik, mida uuendada!", "OK");
            return;
        }

        string nimetus = ec_nimetus.Text?.Trim();
        string pealinn = ec_pealinn.Text?.Trim();
        string info = ec_info.Text?.Trim();
        string keel = ec_keel.Text?.Trim();

        if (!int.TryParse(ec_rahvastiku_suurus.Text, out int rahvastik))
        {
            Shell.Current.DisplayAlert("Viga", "Rahvastiku suurus peab olema arv!", "OK");
            return;
        }

        if (string.IsNullOrEmpty(nimetus) || string.IsNullOrEmpty(pealinn) || string.IsNullOrEmpty(info) || string.IsNullOrEmpty(keel))
        {
            Shell.Current.DisplayAlert("Viga", "Palun sisestage kõik andmed!", "OK");
            return;
        }

        // Обновление данных
        andmeid.Nimetus = nimetus;
        andmeid.Pealinn = pealinn;
        andmeid.Rahvastiku_suurus = rahvastik;
        andmeid.Info = info;
        andmeid.Keel = keel;

        // Обновление фотографии, если выбрана новая
        if (!string.IsNullOrEmpty(lisafoto))
        {
            andmeid.Lipp = lisafoto;
        }

        // Обновление объекта в коллекции
        int index = EuroopaRiigid.IndexOf(andmeid);
        if (index != -1)
        {
            EuroopaRiigid[index] = new Euroopa
            {
                Nimetus = andmeid.Nimetus,
                Pealinn = andmeid.Pealinn,
                Rahvastiku_suurus = andmeid.Rahvastiku_suurus,
                Info = andmeid.Info,
                Keel = andmeid.Keel,
                Lipp = andmeid.Lipp
            };
        }

        // Обновляем источник данных в ListView
        list.ItemsSource = null;
        list.ItemsSource = EuroopaRiigid;

        // Обновляем картинку в интерфейсе
        if (!string.IsNullOrEmpty(andmeid.Lipp))
        {
            ic.Source = ImageSource.FromFile(andmeid.Lipp); // Обновляем источник изображения
        }

        // Снимаем выбор с элемента в ListView
        list.SelectedItem = null;
    }
}