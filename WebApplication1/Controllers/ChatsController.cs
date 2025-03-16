using Microsoft.AspNetCore.Mvc;
using SERVER_SIDE.DBContext;
using SERVER_SIDE.Models;
using SERVER_SIDE.Services;

namespace SERVER_SIDE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly ChatsService _chatsService;

        public ChatsController(DataBaseContext dataBaseContext, ChatsService chatsService)
        {
            _dataBaseContext = dataBaseContext;
            _chatsService = chatsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats()
        {
            var allChats = await _chatsService.GetChats(); // Await the asynchronous service method
            return Ok(allChats); // Return the resolved result
        }

        [HttpPost("addChat")]
        public async Task<IActionResult> AddNewChat([FromBody] Chat chat)
        {
            if (chat == null)
            {
                return BadRequest("Chat cannot be null")    ;
            }

            var newChat = await _chatsService.AddChat(chat);
            return CreatedAtAction(nameof(GetAllChats), new { Id = newChat._id }, newChat);
        }
    }
}