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
    public sealed partial class Option : Page
    {
        private MainPage parent;
        public readonly LabGame game;
        public Option(MainPage parent,LabGame game)
        {
            InitializeComponent();
            this.parent = parent;
            this.game = game;
            sldDimension.Value = game.mazeDimension;
            sldGravityFactor.Value = game.gravityFactor;
            seedTextBox.Text = ""+game.mazeSeed;
        }

       
        private void GoBack(object sender, RoutedEventArgs e)
        {
            //parent.game.reCreate();
           
            parent.Children.Add(parent.mainMenu);
            parent.Children.Remove(this);
        }

        private void ChangeDimension(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            //if (game != null) { parent.game.difficulty = (float)e.NewValue; }
            if (game != null) { parent.game.mazeDimension = (int)e.NewValue; }
        }

        private void ChangeGravityFactor(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            //if (game != null) { parent.game.difficulty = (float)e.NewValue; }
            if (game != null) { parent.game.gravityFactor = (float)e.NewValue; }
        }

        private void seedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num;
            if (Int32.TryParse(seedTextBox.Text,out num))
            {
                parent.game.mazeSeed = num;
            }
            else
            {
                seedTextBox.Text = "123";

            }
        }

        private void mazeDimensionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
    }
}
