using System.Threading.Tasks;
using System.Windows;

using Autofac;

using Translator.Kernel;

namespace Translator
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public IContainer DI { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var repo = new WordRepo();
            var dictionary = new WordDictionary();

            Task.Run(repo.getWords)
                .ContinueWith(tsk =>
                {
                    foreach (var word in tsk.Result)
                    {
                        dictionary.Dictionary.Add(word.English, word.Chinese);
                    }
                });

            var builder = new ContainerBuilder();
            builder.RegisterInstance(repo).As<WordRepo>();
            builder.RegisterInstance(dictionary).As<WordDictionary>();

            builder.RegisterType<TranslatorViewModel>();
            builder.RegisterType<TranslatorWindow>();

            //builder.RegisterType<WordViewModel>();
            builder.RegisterType<WordWindow>();

            //IContainer container = builder.Build();
            this.DI = builder.Build();

            using (var scope = this.DI.BeginLifetimeScope())
            {
                var window = scope.Resolve<TranslatorWindow>();
                window.Show();
            }
        }
    }
}
