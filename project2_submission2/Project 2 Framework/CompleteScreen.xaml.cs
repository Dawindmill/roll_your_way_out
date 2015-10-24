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
    public sealed partial class CompleteScreen : Page
    {
        LabGame game;
        MainPage parent;
        int dimensionIncrease = 5;
        public CompleteScreen(MainPage parent,LabGame game,Double gameTimeSeconds)
        {
            this.InitializeComponent();
            this.game = game;
            this.parent = parent;
            timeUsedTextBlock.Text = timeUsedTextBlock.Text + " " + ((int)(gameTimeSeconds/60))+
                "m "+((int)(gameTimeSeconds%60))+"s";
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {

            game.mazeSeed = game.random.Next(1, Int32.MaxValue - 1);
            if (game.mazeDimension + dimensionIncrease <= game.mazeMaxDimension)
            {
                game.mazeDimension = game.mazeDimension + dimensionIncrease;
            }

            

            game.completeScreen = null;

            parent.Children.Remove(this);

            game.reCreate();

            parent.Children.Remove(parent.inGameUI);

            parent.inGameUI = new InGameUI(parent, game);

            parent.Children.Add(parent.inGameUI);

            
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(this);
        }
    }
}
