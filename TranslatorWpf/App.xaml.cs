using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using System.Reactive.Concurrency;

using Translator.Kernel;
using Microsoft.EntityFrameworkCore;
using Translator.ef;

namespace TranslatorWpf
{

    public partial class App : Application
    {
        public WordRepo repo { get; private set; } = new WordRepo();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            TranslatorWindow window = new TranslatorWindow(new TranslatorViewModel());

            Observable.FromAsync(async () =>
            {
                using (var db = new TranslateContext())
                {
                    var words = await db.Words.AsNoTracking().ToArrayAsync();
                    foreach (var word in words)
                    {
                        Singleton.Words.Add(word.English, word.Chinese);
                    }
                };
                return Unit.Default;
            })
                .SubscribeOn(System.Reactive.Concurrency.TaskPoolScheduler.Default)
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(Observer.Create((Unit x) => window.btnPaste.IsEnabled = true));

            window.Show();
        }


    }
}
