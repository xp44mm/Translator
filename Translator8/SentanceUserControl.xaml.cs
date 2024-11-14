using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Translator8.Kernel;

namespace Translator8
{
    /// <summary>
    /// SentanceUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class SentanceUserControl : UserControl
    {
        public SentanceUserControl()
        {
            InitializeComponent();
        }

        private SentanceViewModel GetSentanceViewModel()
        {
            return (SentanceViewModel)this.DataContext;
        }


        private string getText(Object src)
        {
            var tp = src.GetType();
            if (tp == typeof(TextBlock))
                return ((TextBlock)src).Text;
            else if (tp == typeof(ComboBox))
                return ((ComboBox)src).Text;
            else return tp.Name;
        }

        private void Item_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            string t = getText(sender);
            SentanceViewModel sentance = this.GetSentanceViewModel();
            sentance.Transla = sentance.Transla + t;

        }

        public string[] SelectedPhrase()
        {
            var pc = (PhraseItem)this.lstPhrases.SelectedItem;
            if (pc != null)
            {
                var en = pc.Phrase.Text;
                var zh = String.Join("\r\n", pc.ChineseCandidates);
                return [en, zh];
            }
            else
            {
                return [];
            }
        }


    }
}
