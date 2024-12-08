using PapayagramsClient.PapayagramsService;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace PapayagramsClient
{
    public static class CurrentPlayer
    {
        public static PlayerDC Player { get; set; }

        public static ApplicationSettingsDC Configuration { get; set; }

        public static bool IsGuest = false;
    }

    public static class ImagesService
    {
        private const int NUMBER_OF_IMAGES = 3;

        public static BitmapImage GetImageFromId(int id)
        {
            if (id < 1 || id > NUMBER_OF_IMAGES)
            {
                return null;
            }

            string imagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\PlayerImages\\image" + id + ".jpg";
            return new BitmapImage(new Uri(imagePath));
        }
    }
}
