using System;
using System.Collections.Generic;
using System.Text;
using Translator.Kernel;

namespace TranslatorWpf
{
    static class Singleton
    {
        // 数据库源的内存表示
        public readonly static WordDictionary Words = new WordDictionary();
    }
}
