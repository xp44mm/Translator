using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows;
using System.Reactive;

//using Autofac;

using Translator.Kernel;
using System.Reactive.Linq;
using System.Threading;

namespace TranslatorWpf
{

    public partial class App : Application
    {
        //public IContainer DI { get; private set; }
        public WordRepo repo { get; private set; } = new WordRepo();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            TranslatorWindow window;
            window = new TranslatorWindow(new TranslatorViewModel());

            Task.Run(repo.getWords)
                .ContinueWith(tsk =>
                {
                    foreach (var word in tsk.Result)
                    {
                        Singleton.Words.Add(word.English, word.Chinese);
                    }
                })
                .ToObservable()
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(new ReadyGo(window));

            window.Show();
        }


    }
}
