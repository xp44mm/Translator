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
using System.Windows.Shapes;

namespace Translator8
{
    using FSharp.Idioms;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using Translator8.Kernel;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    /// <summary>
    /// TranslatorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TranslatorWindow : Window
    {
        public TranslatorWindow()
        {
            InitializeComponent();
            this.DataContext = new TranslatorViewModel();
        }

        public TranslatorViewModel GetTranslatorViewModel()
        {
            return (TranslatorViewModel)this.DataContext;
        }

        private string getText()
        {
            if (Clipboard.ContainsText(TextDataFormat.UnicodeText))
            {
                try
                {
                    return Clipboard.GetText(TextDataFormat.UnicodeText);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return "Clipboard does not contain text!";
            }

        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            var text = this.getText();
            this.GetTranslatorViewModel().Sentances = Kernel.SentanceViewModel.from(text);
            if (!this.lstSentances.Items.IsEmpty)
                this.lstSentances.SelectedIndex = 0;

        }


        private void translateSelectedSetance(bool force)
        {
            if (this.lstSentances.SelectedItem is null) return;
            SentanceViewModel sentance = (SentanceViewModel) this.lstSentances.SelectedItem;

            if (! sentance.Updated || force)
            {
                var olds = Translation.keepSelection(sentance.PhraseItems);
                App app = (App) Application.Current;
                sentance.PhraseItems = Translation.Update(app.Repository.Words, sentance.Tokens, olds);

            }


        }

        private void btnUpdateDict_Click(object sender, RoutedEventArgs e)
        {
            WordWindow dlg = new WordWindow();
            dlg.Owner = this;

            string[] phrase = this.sentanceControl.SelectedPhrase();

            if (phrase.Length > 0)
            {

                dlg.tbEnglish.Text = phrase[0];
            }

            if (phrase.Length > 1)
            {
                dlg.tbChinese.Text = phrase[1];
            }

            var dlgres = dlg.ShowDialog();

            if(dlgres.HasValue && dlgres.Value)
            {
                var vm = this.GetTranslatorViewModel();

                foreach(var  sent in vm.Sentances)
                {
                    sent.Updated = false;
                }

                translateSelectedSetance(false);

            }

        }

        private void lstSentances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            translateSelectedSetance(false);

        }
    }
}
