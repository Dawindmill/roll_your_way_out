using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InGameUI : Page
    {
        private MainPage parent;
        public LabGame game;
        public InGameUI(MainPage parent,LabGame game)
        {
            InitializeComponent();
            this.parent = parent;
            this.game = game;
        }

       
        private void GoBack(object sender, RoutedEventArgs e)
        {
            parent.Children.Add(parent.mainMenu);
            parent.Children.Remove(this);
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
            game.reCreate();
        }

        private void seedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (Int32.TryParse(seedTextBox.Text, out num))
            {
                parent.game.mazeSeed = num;
            }
            else
            {
                seedTextBox.Text = "123";

            }
        }

        private void pathHintButton_Click(object sender, RoutedEventArgs e)
        {
            if (pathHintButton.Content.Equals("Path Hint"))
            {
                game.mazeLandscape.showPath();
                pathHintButton.Content = "Off Hint";
            }
            else
            {
                game.mazeLandscape.closePath();
                pathHintButton.Content = "Path Hint";
            }
        }

        private void hidePathHintButton_Click(object sender, RoutedEventArgs e)
        {
            game.mazeLandscape.closePath();

        }


    }
}
