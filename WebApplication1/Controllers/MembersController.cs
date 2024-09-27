using Microsoft.AspNetCore.Mvc;
using SERVER_SIDE.DBContext;
using SERVER_SIDE.Models;
using SERVER_SIDE.Services;
namespace SERVER_SIDE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly MembersService _memberService;
        private readonly IConfiguration _configuration;

        public MembersController(DataBaseContext dataBaseContext, MembersService memberService, IConfiguration configuration)
        {
            _dataBaseContext = dataBaseContext;
            _memberService = memberService;
            _configuration = configuration;
        }

        // GET action
        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            var allMembers = await _memberService.GetAllMembers();
            return Ok(allMembers);
        }

        // POST action
        [HttpPost("addMember")]
        public async Task<IActionResult> AddMemberToChat([FromBody] Member member)
        {
            if (member == null)
            {
                return BadRequest("Member cannot be null");
            }

            var newMember = await _memberService.AddMemberToChat(member);
            return CreatedAtAction(nameof(GetAllMembers), new { id = newMember._id }, newMember);
        }

        // DELETE action
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberFromChat(int id)
        {
            var member = await _memberService.DeleteMeberFromChat(id);

            if (member == null)
            {
                return NotFound($"Member with ID {id} not found.");
            }

            return NoContent(); // Successfully deleted, no content to return
        }

        // Delete ALL MEMBERS action
        [HttpDelete("allMembers")]
        public async Task<IActionResult> DeleteAllMembers(MembersService _memberService)
        {
            await _memberService.DeleteAllMembers();
            return NoContent();
        }
    }
}