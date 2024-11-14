using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Translator8.Scaffold;

public partial class Word
{
    [Key]
    public string English { get; set; } = null!;

    public string Chinese { get; set; } = null!;
}
