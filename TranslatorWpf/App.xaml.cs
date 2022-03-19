using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Translator.Kernel;

namespace TranslatorWpf
{

    public partial class App : Application
    {
        public WordRepo repo { get; private set; } = new WordRepo();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var tsk = Task.Run(repo.getWords);

            var window = new TranslatorWindow(new TranslatorViewModel());
                tsk
                .ToObservable()
                .Select(words =>
                {
                    foreach (var word in words)
                    {
                        Singleton.Words.Add(word.English, word.Chinese);
                    }
                    return Unit.Default;
                })
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(new ReadyGo(window));

            window.Show();
        }


    }
}
