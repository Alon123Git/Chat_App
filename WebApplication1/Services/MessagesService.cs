using Microsoft.EntityFrameworkCore;
using SERVER_SIDE.DBContext;
using SERVER_SIDE.Models;
namespace SERVER_SIDE.Services
{
    public class MessagesService
    {
        private readonly DataBaseContext _dataBaseContext;

        public MessagesService(DataBaseContext dataBaseContext)
        {
            this._dataBaseContext = dataBaseContext;
        }

        // GET action
        public async Task<List<Message>> GetAllMessages()
        {
            var allMessages = await _dataBaseContext.messageEntity.ToListAsync();
            return allMessages;
        }

        // POST  action
        public async Task<Message> AddMessageToChat(Message message)
        {
            await _dataBaseContext.messageEntity.AddAsync(message);
            await _dataBaseContext.SaveChangesAsync();
            return message;
        }

        // DELETE action
        public async Task<Message> DeleteMessageFromChat(int id)
        {
            var message = await _dataBaseContext.messageEntity.FirstOrDefaultAsync(db => db._id == id);
            if (message != null)
            {
                _dataBaseContext.messageEntity.Remove(message);
                await _dataBaseContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Error occur");
            }
            return message;
        }

        // DELETE ALL MESSAGES action
        public async Task DeleteAllMessages()
        {
            var allMessages = await _dataBaseContext.messageEntity.ToListAsync();
            if (allMessages.Any())
            {
                _dataBaseContext.messageEntity.RemoveRange(allMessages);
                await _dataBaseContext.SaveChangesAsync();
            }
        }
    }
}