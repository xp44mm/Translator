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
using Translator8.Kernel;

namespace Translator8
{
    /// <summary>
    /// WordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WordWindow : Window
    {
        public WordWindow()
        {
            InitializeComponent();
        }

        private void btnNeat_Click(object sender, RoutedEventArgs e)
        {
            this.tbEnglish.Text = WordOperators.neatEnglish(this.tbEnglish.Text);
            this.tbChinese.Text = WordOperators.neatChinese(this.tbChinese.Text);

        }

        private void btnUpdateDatabase_Click(object sender, RoutedEventArgs e)
        {
            string eng = WordOperators.neatEnglish(this.tbEnglish.Text);
            string chn = WordOperators.neatChinese(this.tbChinese.Text);
            App app = (App)Application.Current;

            if (String.IsNullOrEmpty(chn))
                app.Repository.Words.delete(eng);
            else
                app.Repository.Words.update(eng, chn);

            //修改永久数据库
            this.DialogResult = app.Repository.updateDatabase(eng, chn);
            this.Close();

        }

        public void InitialText(string[] ls)
        {
            this.tbEnglish.Text = ls[0];
            this.tbChinese.Text = ls[1];
        }

    }
}
