using Microsoft.EntityFrameworkCore;
using SERVER_SIDE.DBContext;
using SERVER_SIDE.Models;

namespace SERVER_SIDE.Services
{
    public class ChatsService
    {
        private readonly DataBaseContext _dataBaseContext; // Data context dependency injection

        public ChatsService(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        // GET action
        public async Task<List<Chat>> GetChats()
        {
            var allChats = await _dataBaseContext.chatEntity.ToListAsync();
            return allChats;
        }
        
        // POST action
        public async Task<Chat> AddChat(Chat chat)
        {
            await _dataBaseContext.chatEntity.AddAsync(chat);
            await _dataBaseContext.SaveChangesAsync();
            return chat;
        }
    }
}