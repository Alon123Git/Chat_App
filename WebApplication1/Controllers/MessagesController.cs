using Microsoft.AspNetCore.Mvc;
using SERVER_SIDE.DBContext;
using SERVER_SIDE.Models;
using SERVER_SIDE.Services;

namespace SERVER_SIDE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : Controller
    {
        private readonly DataBaseContext _dataBaseContext; // Data context dependency injection
        private readonly MessagesService _messgeService; // Service dependency injection

        public MessagesController(DataBaseContext dataBaseContext, MessagesService memberService)
        {
            this._dataBaseContext = dataBaseContext;
            this._messgeService = memberService;
        }

        // GET action
        [HttpGet]
        public async Task<IActionResult> GetAllMesages()
        {
            var allMembers = await _messgeService.GetAllMessages();
            return Ok(allMembers);
        }

        // POST action
        [HttpPost("addMessage")]
        public async Task<IActionResult> AddMessageToChat(Message message)
        {
            Message newMessage = await _messgeService.AddMessageToChat(message);
            return Ok(newMessage);
        }

        // DELETE action
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageFromChat(int id)
        {
            var deletedMessage = await _messgeService.DeleteMessageFromChat(id);
            return Ok(deletedMessage);
        }

        [HttpDelete("allMessages")]
        public async Task<IActionResult> DeleteAllMessages(MessagesService _messageService)
        {
            await _messageService.DeleteAllMessages();
            return NoContent();
        }
    }
}