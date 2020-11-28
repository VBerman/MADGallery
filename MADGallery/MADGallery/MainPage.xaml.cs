using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Diagnostics;

namespace MADGallery
{
    public partial class MainPage : ContentPage
    {


        public bool VisibleCheckBox
        {
            get { return (bool)GetValue(VisibleCheckBoxProperty); }
            set { SetValue(VisibleCheckBoxProperty, value); }
        }

        public static readonly BindableProperty VisibleCheckBoxProperty =
            BindableProperty.Create("VisibleCheckBox", typeof(bool), typeof(MainPage));

        public int lastUpdate;



        public int countPage { get; set; } = 1;
        public ObservableCollection<ViewListItem> currentPage { get; set; } = new ObservableCollection<ViewListItem>();
        public WebClient client = new WebClient();

        public void LoadPageFrowRes()
        {
            try {
                var local = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filename = Path.Combine(local, "checkedItems.json");
                var json = File.ReadAllText(filename);
                currentPage = JsonConvert.DeserializeObject<ObservableCollection<ViewListItem>>(json);
                if (currentPage.Count == 0) { AddNewItem(); AddNewItem(); AddNewItem(); }
            }
            catch
            {
                if (currentPage.Count == 0) { AddNewItem(); AddNewItem(); AddNewItem(); }
            }
        }

        public MainPage()
        {
            client.Headers.Add("Authorization", "563492ad6f91700001000001970891f562ee45e2b6d6dbc2e7aa018e");
            try
            {
                LoadPageFrowRes();
            }
            catch {  }



            this.BindingContext = this;
            

            InitializeComponent();
            CollectionViewItem.ItemsSource = currentPage;

            Appearing += MainPage_Appearing;



        }

        private void MainPage_Appearing(object sender, EventArgs e)
        {
            currentPage.Clear();
            LoadPageFrowRes();
            CollectionViewItem.ItemsSource = currentPage;
        }

        private void AddNewItem()
        {
            var item = new ViewListItem(JsonConvert.DeserializeObject<Result>(client.DownloadString(@"https://api.pexels.com/v1/search?query=nature&per_page=0&page=" + countPage.ToString())));
            item.srcBool = new SrcBool();
            if (currentPage.FirstOrDefault(p => p.id == item.id) == null)
            {
                currentPage.Add(item);
            }
            countPage++;
        }

        private void CollectionViewItem_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (e.LastVisibleItemIndex == currentPage.Count() - 1)
            {
                AddNewItem();
            }

        }

        private void OnTapped(object sender, EventArgs e)
        {
            VisibleCheckBox = !VisibleCheckBox;
            //PropertyChanged(nameof(useCheckBox));
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var json = JsonConvert.SerializeObject(currentPage);
            var local = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(local, "checkedItems.json");
            File.WriteAllText(filename, json);

        }

        private async void ImageTap(object sender, EventArgs e)
        {
            //OpenForm()
            try
            {

                var uri = ((sender as Image)?.Source as UriImageSource).Uri.OriginalString;
                //var a = (sender as Image)?.BindingContext;
                await Navigation.PushAsync(new Page1(uri, ((sender as Image)?.BindingContext as ViewListItem).id.ToString()), true);
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
    }



    public class ViewListItem
    {
        public ViewListItem() { }

        public ViewListItem(Result result)
        {
            id = result.photos[0].id;
            src = result.photos[0].src;
        }
        public int id { get; set; }
        public Src src { get; set; }
        public SrcBool srcBool { get; set; }

    }
}
