using Microsoft.EntityFrameworkCore;

namespace ContactsWebApplication.Models
{
    //klasa umozliwiajaca komunikacje z baza danych
    public class AccountDetailContext : DbContext
    {
        public AccountDetailContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AccountDetail> AccountDetails { get; set; }
    }
}
