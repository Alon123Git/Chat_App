using System.ComponentModel.DataAnnotations;

namespace SERVER_SIDE.Models
{
    public class Chat
    {
        [Key]
        public int _id { get; set; }
        public string _name { get; set; }
        public Member _memberBelong {  get; set; }

        public Chat(int id, string name, Member memberBelong)
        {
            this._id = id;
            this._name = name;
            this._memberBelong = memberBelong;
        }

        public Chat() { }
    }
}