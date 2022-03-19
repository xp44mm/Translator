using System;

namespace Translator.ef
{
    //word的主键English不区分大小写相等。则Word相等。
    public partial class Word : IComparable
    {
        public int CompareTo(object obj)
        {
            return obj is Word y ?
                StringComparer.OrdinalIgnoreCase.Compare(this.English, y.English) : 1;
        }

        public override bool Equals(object obj)
        {
            return obj is Word y ?
                StringComparer.OrdinalIgnoreCase.Equals(this.English, y.English) : false;
        }

        public override int GetHashCode()
        {
            return this.English.ToLower().GetHashCode();
        }
    }
}
