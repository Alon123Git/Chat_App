namespace SERVER_SIDE.Models.DTOModels
{
    public class ResponseModel
    {
        public string _token { get; set; }
        public string _userName { get; set; }
        public string _role { get; set; }

        public ResponseModel(string token, string userName, string role)
        {
            this._token = token;
            this._userName = userName;
            this._role = role;
        }

        public ResponseModel() { }
    }
}