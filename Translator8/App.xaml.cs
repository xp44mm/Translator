using System.Configuration;
using System.Data;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

using System.Reactive.Linq;
using System.Runtime.Intrinsics.X86;

namespace Translator8
{
    using FSharp.Idioms;
    using System.Xml.Linq;
    using Translator8.Kernel;
    using Translator8.Scaffold;


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // 数据库源的内存表示
        private readonly WordDictionary _words = new WordDictionary();
        public WordDictionary Words
        {
            get { return _words; }
            //set { unsaveDate = value; }
        }

        // 操作数据库的方法
        private readonly WordRepo _repo = new WordRepo();
        public WordRepo Repo
        {
            get { return _repo; }
            //set { unsaveDate = value; }
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

            var startup = new TranslatorWindow();
            AppOps.getWordsStream()
                .SubscribeOn(System.Reactive.Concurrency.TaskPoolScheduler.Default)
                .ObserveOn(SynchronizationContext.Current ?? throw new ArgumentNullException())
                .Synchronize()
                .Do(word => this.Words.Add(word.English, word.Chinese))
            .Subscribe(
                value => { },
                err => { },
                () => { startup.btnPaste.IsEnabled = true; }
                );
            startup.Show();

        }

    }

}
