using PapayagramsClient.PapayagramsService;
using System;
using System.Windows.Controls;

namespace PapayagramsClient.Menu
{
    public partial class Achievements : Page
    {
        public Achievements(AchievementDC[] achievements)
        {
            InitializeComponent();
            AddAchievementProgress();
            AddAchievementList();
        }

        private void AddAchievementList()
        {
            throw new NotImplementedException();
        }

        private void AddAchievementProgress()
        {
            throw new NotImplementedException();
        }

        private void ReturnToMainMenu(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
