using PapayagramsClient.PapayagramsService;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace PapayagramsClient.Menu
{
    public partial class Leaderboards : Page
    {
        public Leaderboards(LeaderboardStatsDC[] stats)
        {
            InitializeComponent();
            SetTitleGrid();

            foreach (LeaderboardStatsDC userStats in stats)
            {
                Grid userPanel = new Grid { Margin = new Thickness(10), HorizontalAlignment = HorizontalAlignment.Center, Background = Brushes.Beige, Width = 800 };

                for (int i = 0; i < 4; i++)
                {
                    userPanel.ColumnDefinitions.Add(new ColumnDefinition());
                }

                TextBlock usernameLabel = new TextBlock { Text = userStats.PlayerUsername };
                Grid.SetColumn(usernameLabel, 0);
                userPanel.Children.Add(usernameLabel);

                TextBlock TotalGamesLabel = new TextBlock { Text = userStats.TotalGames.ToString() };
                Grid.SetColumn(TotalGamesLabel, 1);
                userPanel.Children.Add(TotalGamesLabel);

                TextBlock GamesWonLabel = new TextBlock { Text = userStats.GamesWon.ToString() };
                Grid.SetColumn(GamesWonLabel, 2);
                userPanel.Children.Add(GamesWonLabel);

                TextBlock GamesLostLabel = new TextBlock { Text = userStats.GamesLost.ToString() };
                Grid.SetColumn(GamesLostLabel, 3);
                userPanel.Children.Add(GamesLostLabel);

                StatsPanel.Children.Add(userPanel);
            }
        }

        private void SetTitleGrid()
        {
            Grid titlesPanel = new Grid { Margin = new Thickness(10), HorizontalAlignment = HorizontalAlignment.Center, Background = Brushes.Beige, Width = 800 };

            for (int i = 0; i < 4; i++)
            {
                titlesPanel.ColumnDefinitions.Add(new ColumnDefinition());
            }

            TextBlock usernameLabel = new TextBlock { Text = Properties.Resources.globalUsername };
            Grid.SetColumn(usernameLabel, 0);
            titlesPanel.Children.Add(usernameLabel);

            TextBlock TotalGamesLabel = new TextBlock { Text = Properties.Resources.profilePlayedGames };
            Grid.SetColumn(TotalGamesLabel, 1);
            titlesPanel.Children.Add(TotalGamesLabel);

            TextBlock GamesWonLabel = new TextBlock { Text = Properties.Resources.profileGamesWon };
            Grid.SetColumn(GamesWonLabel, 2);
            titlesPanel.Children.Add(GamesWonLabel);

            TextBlock GamesLostLabel = new TextBlock { Text = Properties.Resources.statsGamesLost };
            Grid.SetColumn(GamesLostLabel, 3);
            titlesPanel.Children.Add(GamesLostLabel);

            StatsPanel.Children.Add(titlesPanel);
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }
    }
}
