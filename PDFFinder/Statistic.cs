namespace PDFFinder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Statistic
    {
        [Key]
        [StringLength(12)]
        public string group_name { get; set; }

           
            
         
        public int processed_files_count { get; set; }
    }
}
