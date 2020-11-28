using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Webkit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Graphics.Bitmap;
using Xamarin.Essentials;


namespace MADGallery
{
    //preferences
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Page1 : ContentPage
    {
        public ImageSource Source { get; set; }
        public Byte[] Bytes { get; set; }
        public String IdName { get; set; }
        
        public Page1(string PictureRef, string idName)
        {
            IdName = idName;

            InitializeComponent();

            var client = new WebClient();
            Bytes = client.DownloadData(PictureRef);
            Source = ImageSource.FromStream(() => new MemoryStream(Bytes));
            OnPropertyChanged(nameof(Source));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var matrix = new Matrix();
            matrix.PostRotate(90);
            var original = await BitmapFactory.DecodeByteArrayAsync(Bytes, 0, Bytes.Length);

            var rotated = Bitmap.CreateBitmap(original, 0, 0, original.Width, original.Height, matrix, true);

            using (var ms = new MemoryStream())
            {
                await rotated.CompressAsync(CompressFormat.Jpeg, 100, ms);

                Bytes = ms.ToArray();

                Source = ImageSource.FromStream(() => new MemoryStream(Bytes));
                OnPropertyChanged(nameof(Source));
            }

            //}


        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            
            var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            
            var statusS = await Permissions.RequestAsync<Permissions.StorageRead>();
            
            var fileName = System.IO.Path.Combine(path, IdName + ".png");

            
            File.WriteAllBytes(fileName, Bytes);
        }
    }
}