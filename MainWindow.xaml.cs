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

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();

        }

        bool lostGame = false;
        private void Timer_Tick(object sender, EventArgs e)
        {
            string userName = usernameTextBox.Text;
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again, " + userName + "?";
            }
            if (timeTextBlock.Text == "10.0s")
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - You lose, " + userName + "!";
                lostGame = true;
                SetUpGame();
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "😺","😺",
                "🙈","🙈",
                "🐶","🐶",
                "🐯","🐯",
                "🦊","🦊",
                "🐮","🐮",
                "🐷","🐷",
                "🐴","🐴",
            };

            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock" && textBlock.Name != "usernameTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }
            
            
            
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (usernameTextBlock.Text !="" && lostGame != true)
            {
                TextBlock textBlock = sender as TextBlock;
                if (findingMatch == false)
                {
                    textBlock.Visibility = Visibility.Hidden;
                    lastTextBlockClicked = textBlock;
                    findingMatch = true;
                }
                else if (textBlock.Text == lastTextBlockClicked.Text)
                {
                    matchesFound++;
                    textBlock.Visibility = Visibility.Hidden;
                    findingMatch = false;
                }
                else
                {
                    lastTextBlockClicked.Visibility = Visibility.Visible;
                    findingMatch = false;
                }
            }
        }

        private void timeTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }

        private void usernameTextBox_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            string userName = usernameTextBox.Text;
            if (e.Key == Key.Enter)
            {
                lostGame = false;
                usernameTextBlock.Text = userName;
                SetUpGame();
                timer.Start();
                tenthsOfSecondsElapsed = 0;
                matchesFound = 0;
            }
        }
    }
}
