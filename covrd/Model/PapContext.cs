using Microsoft.EntityFrameworkCore;

namespace covrd.Model
{
    public class PapContext : DbContext
    {
        public PapContext(DbContextOptions<PapContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Paper> Papers { get; set; }
        public DbSet<Metadata> Metadatas { get; set; }
        public DbSet<Abstract> Abstract { get; set; }
        public DbSet<Span> Span { get; set; }
        public DbSet<MetadataAuthor> MetadataAuthor { get; set; }
        public DbSet<Affiliation> Affiliation { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<BibEntries> BibEntries { get; set; }
        public DbSet<Bibref0> Bibref0 { get; set; }
        //public DbSet<OtherIds> OtherIds { get; set; }
        public DbSet<RefEntries> RefEntries { get; set; }
        public DbSet<Ref0> Ref0 { get; set; }
        public DbSet<MetadataOverview> MetadataOverviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ExecutedSearch> ExecutedSearchs { get; set; }
    }
}
