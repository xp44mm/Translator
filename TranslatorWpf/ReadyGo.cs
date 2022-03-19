using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorWpf
{
    public class ReadyGo : IObserver<Unit>
    {
        TranslatorWindow window;
        public ReadyGo(TranslatorWindow window)
        {
            this.window = window;
        }

        public virtual void OnCompleted()
        {
        }

        public virtual void OnError(Exception e)
        {
        }

        public void OnNext(Unit value)
        {
            this.window.btnPaste.IsEnabled = true;
        }
        // 取消订阅
    }
}
