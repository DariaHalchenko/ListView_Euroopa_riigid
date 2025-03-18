namespace ListView_Euroopa_riigid;

public partial class StartPage : ContentPage
{
    public List<ContentPage> lehed = new List<ContentPage>() {new List_Page() };
    public List<string> tekstid = new List<string> { "Tee lahti List_Page" };

    ScrollView sv;
    VerticalStackLayout vsl;

    public StartPage()
    {
        Title = "Avaleht";
        vsl = new VerticalStackLayout { BackgroundColor = Color.FromArgb("#D8BFD8") };

        for (int i = 0; i < tekstid.Count; i++)
        {
            Button nupp = new Button
            {
                Text = tekstid[i],
                BackgroundColor = Color.FromArgb("#DDA0DD"),
                TextColor = Color.FromArgb("#DA70D6"),
                BorderWidth = 10,
                ZIndex = i,
                FontFamily = "Luckyfield 400",
                FontSize = 25
            };
            vsl.Add(nupp);
            nupp.Clicked += Lehte_avamine;
        }


        sv = new ScrollView { Content = vsl };
        Content = sv;
    }

    private async void Lehte_avamine(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        await Navigation.PushAsync(lehed[btn.ZIndex]);
    }

    private async void Tagasi_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}