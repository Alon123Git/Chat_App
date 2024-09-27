using Microsoft.EntityFrameworkCore;
using SERVER_SIDE.Models;

namespace SERVER_SIDE.DBContext
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        public DbSet<Member> memberEntity { get; set; }
        public DbSet<Message> messageEntity { get; set; }
    }
}