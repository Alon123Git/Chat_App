using System.ComponentModel.DataAnnotations;

namespace SERVER_SIDE.Models
{
    
    public class Message
    {
        [Key]
        public int _id {  get; set; }
        public string _content { get; set; }
        public string _sender { get; set; } // Who send the message

        public Message(int id, string content)
        {
            this._id = id;
            this._content = content;
            this._sender = content;
        }

        public Message() { }
    }
}