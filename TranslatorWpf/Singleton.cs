using System;
using System.Collections.Generic;
using System.Text;
using Translator.Kernel;

namespace TranslatorWpf
{
    static class Singleton
    {
        public readonly static WordDictionary Words = new WordDictionary();
    }
}
