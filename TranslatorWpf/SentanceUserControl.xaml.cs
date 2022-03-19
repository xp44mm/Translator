using System;
using System.Windows.Controls;
using System.Windows.Input;
using Translator.Kernel;

namespace TranslatorWpf
{
    public partial class SentanceUserControl : UserControl
    {
        public SentanceUserControl()
        {
            this.InitializeComponent();
        }

        private void Item_MouseRightButtonUp(Object sender, MouseButtonEventArgs e)
        {
            var vm = (SentanceViewModel)this.DataContext;
            dynamic src = e.Source;
            vm.Transla += src.Text;
        }


        public String[] SelectedPhrase()
        {
            if (this.lstPhrases.SelectedItem is PhraseItem pc)
            {
                var en = pc.Phrase.Text;
                var zh = String.Join("\r\n", pc.ChineseCandidates);
                return new[] { en, zh };
            }
            else
            {
                return null;
            }
        }

    }
}
