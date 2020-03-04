using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models
{
    public class TalkModel
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(4000, MinimumLength = 20)]
        public string Abstract { get; set; }
        [Required]
        [Range(100, 300)]
        public int Level { get; set; }

        [Required]
        public SpeakerModel Speaker { get; set; }
    }
}
