using Microsoft.EntityFrameworkCore;
using SERVER_SIDE.DBContext;
using SERVER_SIDE.Models;

namespace SERVER_SIDE.Services
{
    public class MembersService
    {
        private readonly DataBaseContext _dataBaseContext;

        public MembersService(DataBaseContext dataBaseContext)
        {
            this._dataBaseContext = dataBaseContext;
        }

        // GET action
        public async Task<List<Member>> GetAllMembers()
        {
            var allMembers = await _dataBaseContext.memberEntity.ToListAsync();
            return allMembers;
        }

        // POST  action
        public async Task<Member> AddMemberToChat(Member member)
        {
            await _dataBaseContext.memberEntity.AddAsync(member);
            await _dataBaseContext.SaveChangesAsync();
            return member;
        } 

        // DELETE action
        public async Task<Member> DeleteMeberFromChat(int id)
        {
            var member = await _dataBaseContext.memberEntity.FirstOrDefaultAsync(db =>db._id == id);
            if (member != null)
            {
                _dataBaseContext.memberEntity.Remove(member);
                await _dataBaseContext.SaveChangesAsync();
            } else
            {
                Console.WriteLine("Error occur");
            }
            
            if (member == null)
            {
                Console.WriteLine("The memebr data is null"); // Print on the console that the member variable is null - empty fron data
                return member;
            } else
            {
                return member;

            }
        }

        // DELETE ALL members action
        public async Task DeleteAllMembers()
        {
            var allMembers = await _dataBaseContext.memberEntity.ToListAsync();
            if (allMembers.Any())
            {
                _dataBaseContext.memberEntity.RemoveRange(allMembers);
                await _dataBaseContext.SaveChangesAsync();
            }
        }
    }
}