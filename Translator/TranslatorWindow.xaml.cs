using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Autofac;

using Translator.Kernel;

namespace Translator
{
    /// <summary>
    /// TranslatorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TranslatorWindow : Window
    {
        public TranslatorWindow(TranslatorViewModel vm)
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }

        //粘贴文本
        private void Paste_Click(Object sender, RoutedEventArgs e)
        {
            var text = this.GetTextFromClipboard();
            var essay = new Translator.Kernel.Essay(text);

            var vm = (TranslatorViewModel)this.DataContext;
            vm.Sentances = // TranslatorViewModel.getSentances(text);
                essay.Sentances
                .Select(sent => new SentanceViewModel(sent.Tokens, sent.Text))
                .ToArray();

            if (this.lstSentances.Items.Count > 0)
            {
                this.lstSentances.SelectedIndex = 0;
            }
        }

        //句子切换当前
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
            using (var scope = ((App)Application.Current).DI.BeginLifetimeScope())
            {
                var dlg = scope.Resolve<WordWindow>();
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
        }

        private void divide_Click(Object sender, RoutedEventArgs e)
        {
            if (this.sentanceControl.lstPhrases.SelectedItem is PhraseItem phraseItem)
            {
                var dlg = new TokensWindow
                {
                    DataContext = phraseItem.Phrase.Tokens,
                    Owner = this
                };

                if (dlg.ShowDialog().Value)
                {
                    this.translateSelectedSetance(force:true);
                }
            }
        }

        private void translateSelectedSetance(bool force = false)
        {
            var sentance = (SentanceViewModel)this.lstSentances.SelectedItem;
            if (sentance != null && (!sentance.Updated || force))
            {
                using (var scope = ((App)Application.Current).DI.BeginLifetimeScope())
                {
                    var dictionary = scope.Resolve<WordDictionary>();

                    var olds = Translation.keepSelection(sentance.PhraseItems);
                    sentance.PhraseItems = Translation.Update(dictionary.Dictionary, sentance.Tokens, olds);
                }
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

        private void Button_Click(Object sender, RoutedEventArgs e)
        {
            using (var scope = ((App)Application.Current).DI.BeginLifetimeScope())
            {
                var dictionary = scope.Resolve<WordDictionary>();
                MessageBox.Show($"{dictionary.Dictionary.Count}");
            }
        }
    }
}
