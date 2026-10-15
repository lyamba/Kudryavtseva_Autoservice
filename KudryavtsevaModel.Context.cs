namespace Kudryavtseva_Autoservice
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class КудрявцеваАвтосервисEntities : DbContext
    {
        public КудрявцеваАвтосервисEntities()
            : base("name=КудрявцеваАвтосервисEntities")
        {
        }

        private static КудрявцеваАвтосервисEntities _context;

        public static КудрявцеваАвтосервисEntities GetContext()
        {
            if (_context == null)
                _context = new КудрявцеваАвтосервисEntities();

            return _context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientService> ClientService { get; set; }
        public virtual DbSet<DocumentByService> DocumentByService { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductPhoto> ProductPhoto { get; set; }
        public virtual DbSet<ProductSale> ProductSale { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<ServicePhoto> ServicePhoto { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
    }
}
