using PapayagramsClient.PapayagramsService;
using PapayagramsClient.WPFControls;
using System;
using System.Windows.Controls;

namespace PapayagramsClient.Menu
{
    public partial class Achievements : Page
    {
        private AchievementDC[] _achievements;
        public Achievements(AchievementDC[] achievements)
        {
            _achievements = achievements;
            InitializeComponent();
            AddAchievementProgress();
            AddAchievementList();
        }

        private void AddAchievementList()
        {
            foreach (AchievementDC achievement in _achievements)
            {
                string achievementDescription = (string)FindResource("addFriend");
                Console.WriteLine(achievementDescription);
                WPFAchievementView achievementView = new WPFAchievementView(achievementDescription, achievement.IsAchieved);
                AchievementListPanel.Children.Add(achievementView);
            }
        }

        private void AddAchievementProgress()
        {
            int total = _achievements.Length;
            int totalAchieved = 0;

            foreach (AchievementDC achievement in _achievements)
            {
                if (achievement.IsAchieved)
                {
                    totalAchieved++;
                }
            }

            ProgressLabel.Content = $"{totalAchieved} / {total}";

            if (total < 1)
            {
                return;
            }

            AchievementsProgress.Value = totalAchieved / total;
        }

        private void ReturnToMainMenu(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
