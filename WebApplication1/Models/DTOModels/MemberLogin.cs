using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SERVER_SIDE.Models.DTOModels
{
    public class MemberLogin
    {
        public string _name { get; set; }
        public string _passwordHash { get; set; }
        public string _token { get; set; }
        public string _role { get; set; }

        public MemberLogin(string name, string password, string passwordHash, string token, string role)
        {
            this._name = name;
            this._passwordHash = passwordHash;
            this._token = token;
            this._role = role;
        }

        public MemberLogin() { }
    }
}