using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public bool _isRegistered { get; set; }
        public string _password { get; set; }
        public string _passwordHash { get; set; }
        public string _role { get; set; }

        public Member(int id, string name, string gender, int age, bool isManager, bool isLogin,
            string password, string role, string passwordHash, bool isRegistered)
        {
            this._id = id;
            this._name = name;
            this._gender = gender;
            this._age = age;
            this._isManager = isManager;
            this._isLogin = isLogin;
            this._password = password;
            this._role = role;
            this._passwordHash = passwordHash;
            this._isRegistered = isRegistered;
        }

        public Member() { }
    }
}