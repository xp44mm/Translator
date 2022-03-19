using System;
using System.Windows.Controls;
using System.Windows.Input;

using Translator.Kernel;

namespace Translator
{
    /// <summary>
    /// SentanceUserControl.xaml 的交互逻辑
    /// </summary>
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

        //public PhraseItem GetCurrentPhraseItem()
        //{
        //    return (PhraseItem)this.lstPhrases.SelectedItem;
        //}

        public String[] SelectedPhrase()
        {
            //var pc = this.GetCurrentPhraseItem();
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

        //public WordViewModel GetWordViewModel()
        //{
        //    using (var scope = ((App)Application.Current).DI.BeginLifetimeScope())
        //    {
        //        var vm = scope.Resolve<WordViewModel>();

        //        if (this.lstPhrases.SelectedItem != null)
        //        {
        //            var pc = (PhraseItem)this.lstPhrases.SelectedItem;
        //            vm.English = pc.Phrase.Text;
        //            vm.Chinese = String.Join("\r\n", pc.ChineseCandidates);
        //        }
        //        return vm;
        //    }
        //}
    }
}
