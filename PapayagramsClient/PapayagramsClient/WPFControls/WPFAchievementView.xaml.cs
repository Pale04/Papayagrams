using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PapayagramsClient.WPFControls
{
    public partial class WPFAchievementView : UserControl
    {
        public WPFAchievementView(string achievementDescription, bool unlocked)
        {
            InitializeComponent();
            AchievementDescriptionLabel.Content = achievementDescription;

            Visibility doneImageVisible;
            if (unlocked)
            {
                doneImageVisible = Visibility.Visible;
            }
            else
            {
                doneImageVisible = Visibility.Hidden;
            }

            AchivementDoneImage.Visibility = doneImageVisible;
        }
    }
}
