﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Translator.Kernel;

namespace TranslatorWpf
{
    public partial class TranslatorWindow
    {
        public TranslatorWindow(TranslatorViewModel vm)
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }

        private void Paste_Click(Object sender, RoutedEventArgs e)
        {
            var text = this.GetTextFromClipboard();
            var essay = new Translator.Kernel.Essay(text);

            var vm = (TranslatorViewModel)this.DataContext;
            vm.Sentances =
                essay.Sentances
                .Select(sent => new SentanceViewModel(sent.Tokens, sent.Text))
                .ToArray();

            if (this.lstSentances.Items.Count > 0)
            {
                this.lstSentances.SelectedIndex = 0;
            }
        }

        private void lstSentances_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.translateSelectedSetance();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"selection changed:{ex.Message}");
            }
        }

        private void modifyDict_Click(Object sender, RoutedEventArgs e)
        {
            var dlg = new WordWindow(((App)Application.Current).repo);
            dlg.Owner = this;
            dlg.InitialText(this.sentanceControl.SelectedPhrase());

            if (dlg.ShowDialog().Value)
            {
                var vm = (TranslatorViewModel)this.DataContext;
                foreach (var sent in vm.Sentances)
                {
                    sent.Updated = false;
                }
                this.translateSelectedSetance();
            }
        }

        private void translateSelectedSetance(bool force = false)
        {
            var sentance = (SentanceViewModel)this.lstSentances.SelectedItem;
            if (sentance != null && (!sentance.Updated || force))
            {
                var dictionary = Singleton.Words;
                var olds = Translation.keepSelection(sentance.PhraseItems);
                sentance.PhraseItems = Translation.Update(dictionary, sentance.Tokens, olds);
            }
        }

        private String GetTextFromClipboard()
        {
            if (!Clipboard.ContainsText())
            {
                return "剪贴板没有包含文本！";
            }
            else
            {
                try
                {
                    return Clipboard.GetText();
                }
                catch
                {
                    return "剪切板格式异常!";
                }
            }
        }


    }
}
