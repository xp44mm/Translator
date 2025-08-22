using System.ComponentModel.DataAnnotations;

namespace Translator8.Scaffold;

public partial class Word
{
    [Key]
    [Required]
    public string English { get; set; } = string.Empty;

    [Required]
    public string Chinese { get; set; } = string.Empty;
}
