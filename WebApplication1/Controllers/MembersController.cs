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
        private readonly DataBaseContext _dataBaseContext; // Data context dependency injection
        private readonly MembersService _memberService; // Service dependency injection
        private readonly IConfiguration _configuration; // Configuration dependency injection

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

        [HttpPut("updateLoginFieldForConnectedMemebr/{id}")]
        public async Task<IActionResult> UpdateLoginFieldForConnectedMember(int id, [FromBody] Member member)
        {
            if (id != member._id)
            {
                return BadRequest("Member ID mismatch");
            }

            var updatedMember = await _memberService.UpdateMemberLoginFieldConnectedMember(member);

            if (updatedMember != null)
            {
                return Ok(updatedMember); // Return the updated member
            }
            return NotFound(); // Return 404 if the member is not found
        }

        [HttpPut("updateLoginFieldForDisconnectedonnectedMemebr/{id}")]
        public async Task<IActionResult> UpdateLoginFieldForDisconnectedMember(int id, [FromBody] Member member)
        {
            if (id != member._id)
            {
                return BadRequest("Member ID mismatch");
            }

            var updatedMember = await _memberService.UpdateMemberLoginFieldDisconnectedMember(member);

            if (updatedMember != null)
            {
                return Ok(updatedMember); // Return the updated member
            }

            return NotFound(); // Return 404 if the member is not found
        }

        // Reset all fields login of all memebrs to false
        [HttpPut("resetAllLogins")]
        public async Task<IActionResult> ResetAllMembersLogin()
        {
            var allMembers = await _memberService.GetAllMembers();

            if (allMembers == null)
            {
                return NotFound("No members found.");
            }

            foreach (var member in allMembers)
            {
                member._isLogin = false; // Reset login status to false
                await _memberService.resetAllLoginFields(member); // Update each member in the database
            }

            return Ok("All members login status has been reset.");
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