using PapayagramsClient.PapayagramsService;

namespace PapayagramsClient
{
    public static class CurrentPlayer
    {
        public static PlayerDC Player { get; set; }

        public static ApplicationSettingsDC Configuration { get; set; }

        public static bool IsGuest = false;
    }
}
