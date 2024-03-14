using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Translator.ef
{
    [Table("Word")]
    public partial class Word
    {
        [Key]
        [StringLength(200)]
        public string English { get; set; }

        [Required]
        [StringLength(600)]
        public string Chinese { get; set; }
    }
}
