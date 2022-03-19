using System.Windows;

using Autofac;

using Translator.Kernel;

namespace Translator
{
    /// <summary>
    /// WordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WordWindow : Window
    {
        private WordRepo Repo { get; set; }

        public WordWindow(WordRepo repo)
        {
            this.InitializeComponent();
            this.Repo = repo;
        }

        public void InitialText(System.String[] arr)
        {
            if (arr != null)
            {
                this.tbEnglish.Text = arr[0];
                this.tbChinese.Text = arr[1];
            }
        }

        private void UpdateDatabase_Click(System.Object sender, RoutedEventArgs e)
        {
            var eng = WordOperators.neatEnglish(this.tbEnglish.Text);
            var chn = WordOperators.neatChinese(this.tbChinese.Text);

            //todo:
            using (var scope = ((App)Application.Current).DI.BeginLifetimeScope())
            {
                var dictionary = scope.Resolve<WordDictionary>();
                if (string.IsNullOrEmpty(chn))
                {
                    dictionary.delete(eng);
                }
                else
                {
                    dictionary.update(eng, chn);
                }
            }

            this.DialogResult = WordOperators.updateDatabase(this.Repo, eng, chn);
            this.Close();
        }

        private void Neat_Click(System.Object sender, RoutedEventArgs e)
        {
            this.tbEnglish.Text = WordOperators.neatEnglish(this.tbEnglish.Text);
            this.tbChinese.Text = WordOperators.neatChinese(this.tbChinese.Text);
        }
    }
}
