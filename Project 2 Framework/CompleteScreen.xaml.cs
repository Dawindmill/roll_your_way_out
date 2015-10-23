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
        public CompleteScreen(MainPage parent,LabGame game)
        {
            this.InitializeComponent();
            this.game = game;
            this.parent = parent;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            game.reCreate();
            parent.Children.Remove(this);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            parent.Children.Remove(this);
        }
    }
}
