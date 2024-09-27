using CHAT_APP_CLIENT.Services;

namespace CHAT_APP_CLIENT.View_Models
{
    public class ViewModel_MemberLogin
    {
        private readonly SignalRService _signalRService;
        private string _userName;
        private string _message;
        private string _chatLog;

        public ViewModel_MemberLogin(SignalRService signalRService, string userName, string message, string chatLog)
        {
            _signalRService = signalRService;
            _userName = userName;
            _message = message;
            _chatLog = chatLog;
        }


    }
}