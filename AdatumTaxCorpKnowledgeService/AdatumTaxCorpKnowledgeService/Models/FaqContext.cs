namespace AdatumTaxCorpKnowledgeService.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FaqContext : DbContext
    {
        public FaqContext()
            : base("name=FaqContext")
        {
        }

        public virtual DbSet<Faq> Faqs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
