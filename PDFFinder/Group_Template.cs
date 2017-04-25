namespace PDFFinder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Group_Template
    {
        [Key]
        [StringLength(12)]
        public string group_name { get; set; }

        [Required]
        [StringLength(12)]
        public string printer_name { get; set; }

        public bool? duplex { get; set; }

        [StringLength(2)]
        public string paper_format { get; set; }
    }
}
