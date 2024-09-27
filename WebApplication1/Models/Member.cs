using System.ComponentModel.DataAnnotations;

namespace SERVER_SIDE.Models
{
    public class Member
    {
        [Key]
        public int _id {  get; set; }
        public string _name { get; set; }
        public string _gender { get; set; }
        public int _age { get; set; }
        public bool _isManager { get; set; }
        public bool _isLogin  { get; set; }

        public Member(int id, string name, string gender, int age, bool isManager, bool isLogin)
        {
            this._id = id;
            this._name = name;
            this._gender = gender;
            this._age = age;
            this._isManager = isManager;
            this._isLogin = isLogin;
        }

        public Member() { }
    }
}