namespace PDFFinder
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model_PDFFinder : DbContext
    {
        public Model_PDFFinder()
            : base("name=Model_PDFFinder")
        {
        }

        public virtual DbSet<Group_Template> Group_Template { get; set; }
        public virtual DbSet<Report_Template> Report_Template { get; set; }
        public virtual DbSet<Statistic> Statistics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
