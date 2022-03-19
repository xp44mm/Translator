﻿using System.Windows;
using System.Windows.Data;

using Translator.Kernel;

namespace TranslatorWpf
{
    public partial class TokensWindow : Window
    {
        public TokensWindow()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void SpaceSwhich_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchSpaceDisplay();
        }

        private void SwitchSpaceDisplay()
        {
            var view = (ListCollectionView)
                CollectionViewSource.GetDefaultView(this.lstTokens.ItemsSource);
            if (view == null)
            {
                return;
            }
            else
            {
                var showSpace = this.btnShowSpace.IsChecked.Value;
                view.Filter = (object item) => showSpace || ((Token)item).Lexeme != " ";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.SwitchSpaceDisplay();
        }
        // end class
    }
}
