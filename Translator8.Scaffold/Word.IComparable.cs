namespace Translator8.Scaffold;

using System;

//word的主键English不区分大小写相等。
public partial class Word : IComparable, IComparable<Word>
{
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is Word that)
            return StringComparer.OrdinalIgnoreCase.Compare(
                this.English ?? string.Empty,
                that.English ?? string.Empty);
        throw new ArgumentException("Object must be of type Word", nameof(obj));
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is Word that)
            return StringComparer.OrdinalIgnoreCase.Equals(
                this.English ?? string.Empty,
                that.English ?? string.Empty);
        return false;
    }

    public override int GetHashCode()
    {
        return this.English?.ToLowerInvariant().GetHashCode() ?? 0;
    }

    public int CompareTo(Word? that)
    {
        if (that is null) return 1;
        return StringComparer.OrdinalIgnoreCase.Compare(
            this.English ?? string.Empty,
            that.English ?? string.Empty);
    }
}
