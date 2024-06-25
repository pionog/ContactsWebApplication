using Microsoft.EntityFrameworkCore;

namespace ContactsWebApplication.Models
{
    public class AccountDetailContext : DbContext
    {
        public AccountDetailContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AccountDetail> AccountDetails { get; set; }
    }
}
