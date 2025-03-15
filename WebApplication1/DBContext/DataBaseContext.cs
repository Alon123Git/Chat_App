using Microsoft.EntityFrameworkCore;
using SERVER_SIDE.Models;

namespace SERVER_SIDE.DBContext
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        public DbSet<Member> memberEntity { get; set; } // Members table
        public DbSet<Message> messageEntity { get; set; } // Messages table
        public DbSet<Chat> chatEntity { get; set; } // Chats table
    }
}