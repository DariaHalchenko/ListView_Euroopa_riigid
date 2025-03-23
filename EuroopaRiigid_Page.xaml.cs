using System.Collections.ObjectModel;

namespace ListView_Euroopa_riigid;

public partial class EuroopaRiigid_Page : ContentPage
{
    public ObservableCollection<Euroopa> EuroopaRiigid { get; set; }

    Label lbl_list, lbl_nimetus, lbl_pealinn, lbl_rahvastiku_suurus, lbl_info, lbl_keel;
    ListView list;
    Button lisa, btn_valifoto, uuenda, kustuta;
    Entry e_nimetus, e_pealinn, e_rahvastiku_suurus, e_info, e_keel;
    Image ic;
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

        lbl_nimetus = new Label { Text = "Riigi nimi: ", FontSize = 20 };
        e_nimetus = new Entry { Placeholder = "Sisesta riigi nimi" };

        lbl_pealinn = new Label { Text = "Pealinn: ", FontSize = 20 };
        e_pealinn = new Entry { Placeholder = "Sisesta riigi pealinn" };

        lbl_rahvastiku_suurus = new Label { Text = "Rahvastik: ", FontSize = 20 };
        e_rahvastiku_suurus = new Entry { Placeholder = "Sisesta riigi rahvastik", Keyboard = Keyboard.Numeric };

        lbl_info = new Label { Text = "Info: ", FontSize = 20 };
        e_info = new Entry { Placeholder = "Sisesta riigi info" };

        lbl_keel = new Label { Text = "Keel: ", FontSize = 20 };
        e_keel = new Entry { Placeholder = "Sisesta riigi keel" };

        ic = new Image { WidthRequest = 100, HeightRequest = 60, Source = "default_flag.png" };

        btn_valifoto = new Button
        {
            Text = "Vali lipp",
            BackgroundColor = Colors.LightGreen,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center
        };
        btn_valifoto.Clicked += Btn_valifoto_Clicked;

        lisa = new Button { Text = "Lisa riik" };
        lisa.Clicked += Lisa_Clicked;
        uuenda = new Button { Text = "Uuenda" }; 
        uuenda.Clicked += Uuenda_Clicked;
        kustuta = new Button { Text = "Kustuta telefn" };
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
            SeparatorColor = Colors.Azure,
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

        this.Content = new VerticalStackLayout
        {
            Children =
            {
                new VerticalStackLayout
                {
                    Children = { list }, 
                    VerticalOptions = LayoutOptions.FillAndExpand 
                },
                new VerticalStackLayout
                {
                    Children = { lbl_list, lbl_nimetus, e_nimetus, lbl_pealinn, e_pealinn, 
                                lbl_rahvastiku_suurus, e_rahvastiku_suurus, lbl_info, e_info, 
                                lbl_keel, e_keel, btn_valifoto, ic, lisa, kustuta, uuenda },
                    Padding = 10,
                    VerticalOptions = LayoutOptions.End 
                }
            }
        };
    }

    private void Lisa_Clicked(object? sender, EventArgs e)
    {
        string nimetus = e_nimetus.Text?.Trim();
        string pealinn = e_pealinn.Text?.Trim();
        string info = e_info.Text?.Trim();
        string keel = e_keel.Text?.Trim();

        if (!int.TryParse(e_rahvastiku_suurus.Text, out int rahvastik))
        {
            Shell.Current.DisplayAlert("Viga", "Rahvastiku suurus peab olema arv!", "OK");
            return;
        }

        if (string.IsNullOrEmpty(nimetus) || string.IsNullOrEmpty(pealinn) || string.IsNullOrEmpty(info) || string.IsNullOrEmpty(keel))
        {
            Shell.Current.DisplayAlert("Viga", "Palun sisestage kõik andmed!", "OK");
            return;
        }

        string lipp = string.IsNullOrEmpty(lisafoto) ? "default_flag.png" : lisafoto;

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

            e_nimetus.Text = andmeid.Nimetus;
            e_pealinn.Text = andmeid.Pealinn;
            e_rahvastiku_suurus.Text = andmeid.Rahvastiku_suurus.ToString();
            e_info.Text = andmeid.Info;
            e_keel.Text = andmeid.Keel;
            ic.Source = ImageSource.FromFile(andmeid.Lipp);
        }
    }


    private async void Btn_valifoto_Clicked(object sender, EventArgs e)
    {
        FileResult foto = await MediaPicker.Default.PickPhotoAsync();
        if (foto != null)
        {
            lisafoto = Path.Combine(FileSystem.CacheDirectory, foto.FileName);

            using Stream sourceStream = await foto.OpenReadAsync();
            using FileStream localFileStream = File.OpenWrite(lisafoto);
            await sourceStream.CopyToAsync(localFileStream);

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

    private void Uuenda_Clicked(object sender, EventArgs e)
    {
        if (andmeid == null)
        {
            Shell.Current.DisplayAlert("Viga", "Palun vali riik, mida uuendada!", "OK");
            return;
        }

        string nimetus = e_nimetus.Text?.Trim();
        string pealinn = e_pealinn.Text?.Trim();
        string info = e_info.Text?.Trim();
        string keel = e_keel.Text?.Trim();

        if (!int.TryParse(e_rahvastiku_suurus.Text, out int rahvastik))
        {
            Shell.Current.DisplayAlert("Viga", "Rahvastiku suurus peab olema arv!", "OK");
            return;
        }

        if (string.IsNullOrEmpty(nimetus) || string.IsNullOrEmpty(pealinn) || string.IsNullOrEmpty(info) || string.IsNullOrEmpty(keel))
        {
            Shell.Current.DisplayAlert("Viga", "Palun sisestage kõik andmed!", "OK");
            return;
        }

        andmeid.Nimetus = nimetus;
        andmeid.Pealinn = pealinn;
        andmeid.Rahvastiku_suurus = rahvastik;
        andmeid.Info = info;
        andmeid.Keel = keel;

        if (!string.IsNullOrEmpty(lisafoto))
        {
            andmeid.Lipp = lisafoto;
        }

        list.SelectedItem = null;
    }
}