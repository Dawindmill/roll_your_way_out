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
            seedTextBox.Text = ""+game.mazeSeed;
            sldDimension.Value = game.mazeDimension;
            sldGravityFactor.Value = game.gravityFactor;

        }

       
        private void GoBack(object sender, RoutedEventArgs e)
        {
            parent.Children.Add(parent.mainMenu);
            parent.Children.Remove(this);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            game.gravityFactor = (float)sldGravityFactor.Value;
            game.mazeDimension = (int)sldDimension.Value;
            game.reCreate();
            pauseButton.Content = "Pause";
            game.resumed = true;
            
        }

        private void SeedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (Int32.TryParse(seedTextBox.Text, out num)&&num<Int32.MaxValue-1&&num>0)
            {
                parent.game.mazeSeed = num;
            }
            else
            {
                seedTextBox.Text = ""+game.mazeSeed;

            }
        }

        private void PathHintButton_Click(object sender, RoutedEventArgs e)
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

        private void HidePathHintButton_Click(object sender, RoutedEventArgs e)
        {
            game.mazeLandscape.closePath();

        }

        private void ChangeDimension(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            //if (game != null) { parent.game.difficulty = (float)e.NewValue; }
            //if (game != null) { parent.game.mazeDimension = (int)e.NewValue; }
        }

        private void ChangeGravityFactor(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            //if (game != null) { parent.game.difficulty = (float)e.NewValue; }
            if (game != null) { game.gravityFactor = (int)e.NewValue; }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (pauseButton.Content.Equals("Pause"))
            { 
                pauseButton.Content = "Resume";
                game.resumed = false;
            }
            else
            {
                pauseButton.Content = "Pause";
                game.resumed = true;
            } 
        }


    }
}
