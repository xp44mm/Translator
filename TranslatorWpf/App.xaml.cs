using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

//using Autofac;

using Translator.Kernel;

namespace TranslatorWpf
{
    public partial class App : Application
    {
        //public IContainer DI { get; private set; }
        public WordRepo repo { get; private set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.repo = new WordRepo();

            Task.Run(repo.getWords)
                .ContinueWith(tsk =>
                {
                    foreach (var word in tsk.Result)
                    {
                        Singleton.Words.Add(word.English, word.Chinese);
                    }
                });

            //var builder = new ContainerBuilder();

            //builder.RegisterInstance(repo).As<WordRepo>();
            //builder.RegisterType<TranslatorViewModel>();
            //builder.RegisterType<TranslatorWindow>();
            //builder.RegisterType<WordWindow>();

            //this.DI = builder.Build();

            //using (var scope = this.DI.BeginLifetimeScope())
            //{
            var window = new TranslatorWindow(new TranslatorViewModel());
            window.btnPaste.IsEnabled = true;
            window.Show();
            //}
        }


    }
}
