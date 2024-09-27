using Azure.Messaging;
using CHAT_APP_CLIENT.Extensions;
using CHAT_APP_CLIENT.Services;
using SERVER_SIDE.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CHAT_APP_CLIENT.View_Models
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private readonly ApiServiceMembers _apiServiceMembers; // API service to get the data from the ASP.NET CORE WEB API back-end
        private readonly ApiServiceMessages _apiServiceMessages;
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly SignalRService _signalRService;

        // Commands for different button actions
        public ICommand SendMessageCommand { get; private set; } // Send message command
        public ICommand JoinChatCommand { get; private set; } // Join chat command
        public ICommand LeaveChatCommand { get; private set; } // Left the chat command
        public ICommand ClearAllMembersCommand { get; private set; } // Clear all members command
        public ICommand ClearChatCommand { get; private set; } // Clear the caht command

        // strings for write and display the text in the UI
        private string _textBoxMessage = string.Empty;
        private string _textBoxJoinOrLeaveChat = string.Empty;
        private string _displayText = string.Empty;
        private string _deleteText = string.Empty;
        private string _errorMessage = string.Empty;
        private string _memberLogin = string.Empty;
        private string _chatLog = string.Empty; 

        // collections of strings for collect all the messages and users that are in the chat
        private ObservableCollection<string> _linesMessage; // Collection that will contain all the messages in the chat
        private ObservableCollection<string> _linesJoinChat; // Collection that will contain all the names of the users who joined the chat

        private ObservableCollection<Member> _chatMembers = new ObservableCollection<Member>(); // collecation for collect all the memeber that availiable in the chat
        private ObservableCollection<Message> _chatMessages = new ObservableCollection<Message>(); // collecation for collect all the messages that availiable in the chat

        private const int maxCharsPerLine = 30; // define the maximum letters for a memeber name in the chat

        public ViewModelBase()
        {
            _signalRService = new SignalRService();
            SendMessageCommand = new Commands(async () => await SendMessageAsync());

            // Subscribe to incoming messages from the server
            _signalRService.OnMessageReceived((user, message) =>
            {
                // Update the chat log when a new message is received
                ChatLog += $"{user}: {message._content}\n";
            });

            // Start the SignalR connection
            StartSignalRConnectionAsync();

            // Initialize commands with their corresponding methods
            SendMessageCommand = new Commands(OnClick_SendMessage); // Initial send message command
            JoinChatCommand = new Commands(OnClick_JoinNewMemberInChat); // Initial join caht command
            LeaveChatCommand = new Commands(OnClick_LeaveChat); // Initial left the chat command
            ClearAllMembersCommand = new Commands(OnClick_ClearMembers); // Initial clear memebers command
            ClearChatCommand = new Commands(OnClick_ClearMessagesChat); // Initial clear chat command

            // Initial collections
            _linesMessage = new ObservableCollection<string>(); // Initial the collection that include all the msessages in the chat
            _linesJoinChat = new ObservableCollection<string>(); // Initial the collection that include all the users who joined the chat
           
            _chatMembers = new ObservableCollection<Member>(); // Initialize the ObservableCollection for chat members
            _chatMessages = new ObservableCollection<Message>(); // Initialize the ObservableCollection for chat messages

            _apiServiceMembers = new ApiServiceMembers();
            _apiServiceMessages = new ApiServiceMessages();
            InitializeCommands();

            LoadMembers(); // Load members when the ViewModel is instantiated
            LoadMessages(); // Load messages when the ViewModel is instantiated
        }

        // Start SignalR connection asynchronously
        private async Task StartSignalRConnectionAsync()
        {
            await _signalRService.StartAsync();
        }

        // Send message method that will send a Message object to the server
        public async Task SendMessageAsync()
        {
            if (!string.IsNullOrEmpty(MemberLogin) && !string.IsNullOrEmpty(TextBoxMessage))
            {
                // Create a new message object
                var message = new Message
                {
                    _content = TextBoxMessage,
                    _sender = MemberLogin
                };

                // Send the message to the server using the SignalR service
                await _signalRService.SendMessageAsync(MemberLogin, message);

                // Clear the message box after sending
                TextBoxMessage = string.Empty;
            }
        }

        #region API operation
        /// <summary>
        /// Display all the memebrs that alreadt exist in the caht (in the data base)
        /// </summary>
        private async void LoadMembers()
        {
            try
            {
                var members = await _apiServiceMembers.GetAllMembersAsync();
                if (members != null)
                {
                    foreach (var member in members)
                    {
                        ChatMembers.Add(member); // Add each member to the ObservableCollection
                    }

                    // Update DisplayTextJoinChat with the names of all members
                    DisplayTextJoinChat = string.Join("\n", ChatMembers.Select(m => m._name));
                }
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = "Failed to load members: " + ex.Message;
            }
        }

        public async void LoadMessages()
        {
            try
            {
                var messages = await _apiServiceMessages.GetAllMessages();
                if (messages != null)
                {
                    foreach (var message in messages)
                    {
                        ChatMessages.Add(message);
                    }

                    DisplayTextMessage = string.Join("\n", ChatMessages.Select(ms => ms._content));
                }
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = "Failed to load members: " + ex.Message;
            }
        }

        // Initialize commands here
        private void InitializeCommands()
        {
            SendMessageCommand = new Commands(OnClick_SendMessage);
            JoinChatCommand = new Commands(OnClick_JoinNewMemberInChat);
            LeaveChatCommand = new Commands(OnClick_LeaveChat);
            ClearAllMembersCommand = new Commands(OnClick_ClearMembers);
            ClearChatCommand = new Commands(OnClick_ClearMessagesChat);
        }
        #endregion

        // Method to notify property changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties
        public ObservableCollection<Member> ChatMembers
        {
            get => _chatMembers;
            set
            {
                _chatMembers = value;
                OnPropertyChanged(nameof(ChatMembers));
            }
        }

        public ObservableCollection<Message> ChatMessages
        {
            get { return _chatMessages; }
            set
            {
                _chatMessages = value;
                OnPropertyChanged(nameof(ChatMessages));
            }
        }

        // Property for the first TextBox text
        public string TextBoxMessage
        {
            get { return _textBoxMessage; }
            set
            {
                if (_textBoxMessage != value)
                {
                    _textBoxMessage = value;
                    OnPropertyChanged(nameof(TextBoxMessage));
                }
            }
        }

        // Property for the second TextBox text
        public string TextBoxJoinOrLeaveChat
        {
            get { return _textBoxJoinOrLeaveChat; }
            set
            {
                if (_textBoxJoinOrLeaveChat != value)
                {
                    if (value.Length > maxCharsPerLine)
                    {
                        // Set an error message if the input exceeds the max characters
                        ErrorMessage = $"Input cannot exceed {maxCharsPerLine} characters.";
                    }
                    else
                    {
                        ErrorMessage = string.Empty; // Clear error message
                        _textBoxJoinOrLeaveChat = value;
                        OnPropertyChanged(nameof(TextBoxJoinOrLeaveChat));
                    }
                }
            }
        }

        public string DeleteTextJoinChat
        {
            get { return _deleteText; }
            set
            {
                if (_deleteText != value)
                {
                    _deleteText = value;
                    OnPropertyChanged(nameof(DeleteTextJoinChat));
                }
            }
        }

        public string DisplayTextMessage
        {
            get { return _displayText; }
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged(nameof(DisplayTextMessage));
                }
            }
        }

        public string DisplayTextJoinChat
        {
            get { return _displayText; }
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged(nameof(DisplayTextJoinChat));
                }
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public string MemberLogin
        {
            get { return _memberLogin; }
            set
            {
                if (_memberLogin != value)
                {
                    _memberLogin = value;
                    OnPropertyChanged(nameof(MemberLogin));
                }
            }
        }

        public string ChatLog
        {
            get { return _chatLog; }
            set
            {
                if (_chatLog != value)
                {
                    _chatLog = value;
                    OnPropertyChanged(nameof(ChatLog));
                }
            }
        }
        #endregion

        private async void MemebrConnection(Member currentMember)
        {
            if (!string.IsNullOrEmpty(MemberLogin))
            {
                currentMember._isLogin = true;
            }
        }

        // Method that handles sending a message
        private async void OnClick_SendMessage()
        {
            if (!string.IsNullOrEmpty(TextBoxMessage))
            {
                var newMessage = new Message
                {
                    _content = TextBoxMessage,
                    _id = 0,
                    _sender = ""
                };
                TextBoxMessage = string.Empty;

                try
                {
                    var response = await _apiServiceMessages.AddMessageToChatAsync(newMessage);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var addMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(responseBody);

                        if (addMessage != null)
                        {
                            _chatMessages.Add(addMessage);

                            // Update the display text
                            DisplayTextMessage = string.Join("\n", _chatMessages.Select(m => m._content));
                            OnPropertyChanged(nameof(DisplayTextMessage));
                        }
                        else
                        {
                            ErrorMessage = "Failed to parse the message details from the response.";
                        }
                    }
                    else
                    {
                        ErrorMessage = $"Failed to add message: {response.ReasonPhrase}";
                    }
                }
                catch (HttpRequestException ex)
                {
                    ErrorMessage = "Failed to add message: " + ex.Message;
                }
            }
            else
            {
                ErrorMessage = $"Input cannot exceed {maxCharsPerLine} characters.";
            }
        }

        private async void OnClick_JoinNewMemberInChat()
        {
            if (!string.IsNullOrEmpty(TextBoxJoinOrLeaveChat))
            {
                var newMember = new Member
                {
                    _name = TextBoxJoinOrLeaveChat,
                    _id = 0,
                    _gender = "Female/Male",
                    _age = 0,
                    _isManager = false,
                    _isLogin = false
                };
                TextBoxJoinOrLeaveChat = string.Empty;

                try
                {
                    var response = await _apiServiceMembers.AddMemberToChatAsync(newMember);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var addedMember = Newtonsoft.Json.JsonConvert.DeserializeObject<Member>(responseBody);

                        if (addedMember != null)
                        {
                            _chatMembers.Add(addedMember);

                            // Update the display text
                            DisplayTextJoinChat = string.Join("\n", _chatMembers.Select(m => m._name));
                            OnPropertyChanged(nameof(DisplayTextJoinChat));
                        }
                        else
                        {
                            ErrorMessage = "Failed to parse the member details from the response.";
                        }
                    }
                    else
                    {
                        ErrorMessage = $"Failed to add member: {response.ReasonPhrase}";
                    }
                }
                catch (HttpRequestException ex)
                {
                    ErrorMessage = "Failed to add member: " + ex.Message;
                }
            }
            else
            {
                ErrorMessage = $"Input cannot exceed {maxCharsPerLine} characters.";
            }
        }

        private async void OnClick_LeaveChat()
        {
            if (!string.IsNullOrEmpty(TextBoxJoinOrLeaveChat))
            {
                var memberName = TextBoxJoinOrLeaveChat;
                var member = _chatMembers.FirstOrDefault(m => m._name == memberName);

                if (member != null)
                {
                    try
                    {
                        bool success = await _apiServiceMembers.DeleteMemberFromChatAsync(member._id);

                        if (success)
                        {
                            // Remove only the deleted member from the ObservableCollection
                            _chatMembers.Remove(member);

                            // Clear TextBoxJoinOrLeaveChat after deletion
                            TextBoxJoinOrLeaveChat = string.Empty;

                            // Update DisplayTextJoinChat with the remaining members
                            DisplayTextJoinChat = string.Join("\n", _chatMembers.Select(m => m._name));
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = "Failed to delete member: " + ex.Message;
                    }
                } else
                {
                    MessageBox.Show($"{TextBoxJoinOrLeaveChat} is not currently in the chat.");
                }
            } else
            {
                MessageBox.Show("No name provided. Enter a member name to leave the chat.");
            }
        }

        // Clear all members in the chat
        private async void OnClick_ClearMembers()
        {
            try
            {
                await _apiServiceMembers.DeleteAllMembersAsync();

                _chatMembers.Clear(); // Clear the list of members
                _linesJoinChat.Clear(); // Clear any other related collections
                DisplayTextJoinChat = string.Empty; // Clear the chat display text
                ErrorMessage = string.Empty; // Clear any error messages

                OnPropertyChanged(nameof(DisplayTextJoinChat));
                OnPropertyChanged(nameof(_chatMembers)); // Notify UI that members list has changed
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while clearing members: {ex.Message}";
                OnPropertyChanged(nameof(ErrorMessage)); // Notify UI to update the error message
            }
        }

        // Clear all messages from the chat
        private async void OnClick_ClearMessagesChat()
        {
            try
            {
                await _apiServiceMessages.DeleteAllMessagesAsync();
                _chatMessages.Clear();
                _linesMessage.Clear();
                DisplayTextMessage = string.Empty;
                ErrorMessage = string.Empty;
                OnPropertyChanged(nameof(DisplayTextMessage));
                OnPropertyChanged(nameof(_chatMessages));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while clearing messages: {ex.Message}";
                OnPropertyChanged(nameof(ErrorMessage)); // Notify UI to update the error message
            }
        }
    }
}