using BotAdminPanel.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BotAdminPanel.Models
{
    public class BotContext : DbContext
    {
        public BotContext(DbContextOptions<BotContext> option):base(option)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
