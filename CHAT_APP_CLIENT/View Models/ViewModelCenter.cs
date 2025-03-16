using CHAT_APP_CLIENT.Extensions;
using CHAT_APP_CLIENT.Services;
using Newtonsoft.Json;
using SERVER_SIDE.Models;
using SERVER_SIDE.Models.DTOModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CHAT_APP_CLIENT.View_Models
{
    public class ViewModelCenter : INotifyPropertyChanged
    {
        private string _textBoxMessage = string.Empty;
        private string _textBoxJoinOrLeaveChat = string.Empty;
        private string _textBoxPassword = string.Empty;
        private string _displayText = string.Empty;
        private string _deleteText = string.Empty;
        private string _memberLogin = string.Empty;
        private string _chatLog = string.Empty;
        private string _txtMemberName = string.Empty;
        private string _txtJoinConectedOrDissconnectedMemberChat = string.Empty;
        private string _jwtToken = string.Empty;
        private bool _isMaleButtonEnabled = false;
        private bool _isFemaleButtonEnabled = false;
        private string _selectedGender = string.Empty;
        private int _selectedAge = 0;


        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly ApiServiceChats _apiServiceChats;
        private readonly ApiServiceMembers _apiServiceMembers; // API service to get the data from the ASP.NET CORE WEB API back-end
        private readonly ApiServiceAuth _apiServiceAuth;
        private readonly SignalRService _signalRService;

        private ObservableCollection<Member> _chatMembers = new ObservableCollection<Member>(); // collecation for collect all the memeber that availiable in the chat

        public ICommand createChatCommand { get; private set; }
        public ICommand registerCommand { get; private set; }
        public ICommand loginCommand { get; private set; }
        
        private string _textBoxChat = string.Empty;
        private string _displayChatText = string.Empty;
        private string _textBoxCreateNewChat = string.Empty;
        private string _errorMessage = string.Empty;

        private ObservableCollection<Chat> _chatsList = new ObservableCollection<Chat>();

        private const int maxCharsPerLine = 100;

        public ViewModelCenter()
        {
            _signalRService = new SignalRService();

            _ = StartSignalRConnectionAsync(); // Start SignalR connection

            _signalRService = new SignalRService();
            _apiServiceChats = new ApiServiceChats();
            _apiServiceMembers = new ApiServiceMembers();
            _apiServiceAuth = new ApiServiceAuth();

            InitializeCommands();

            _chatsList = new ObservableCollection<Chat>();
            
            _ = FetchChatsAsync(); // Fetch chats from the server on startup

            LoadMembers();
        }

        private void InitializeCommands()
        {
            createChatCommand = new Commands(OnClick_CreateNewChat);
            registerCommand = new Commands(OnClick_Register);
            loginCommand = new Commands(OnClick_Login);
        }

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
                    DisplayTextJoinChat = string.Join("\n", ChatMembers.Select(m => $"{m._name}  ({m._gender} {m._age})")); // Update DisplayTextJoinChat with the names of all members
                }
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = "Failed to load members: " + ex.Message;
            }
        }

        public ObservableCollection<Member> ChatMembers
        {
            get { return _chatMembers; }
            set
            {
                _chatMembers = value;
                OnPropertyChanged(nameof(ChatMembers));
            }
        }

        private async Task FetchChatsAsync()
        {
            try
            {
                var chats = await _apiServiceChats.GetAllChatsAsync();
                if (chats != null && chats.Count != 0)
                {
                    foreach (var chat in chats)
                    {
                        _chatsList.Add(chat);
                        ButtonList.Add(chat._name); // Add chat name to ButtonList
                    }

                    UpdateDisplayChatText();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to fetch chats: {ex.Message}";
            }
        }

        // Method to notify property changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task StartSignalRConnectionAsync()
        {
            await _signalRService.StartAsync();
        }

        #region Properties
        public ObservableCollection<Chat> ChatsList
        {
            get { return _chatsList; }
            set
            {
                _chatsList = value;
                OnPropertyChanged(nameof(ChatsList));
                UpdateDisplayChatText();
            }
        }

        private void UpdateDisplayChatText()
        {
            DisplayChatText = string.Join("\n", _chatsList.Select(c => $"{c._name}"));
        }

        public string DisplayChatText
        {
            get { return _displayChatText; }
            set
            {
                if (_displayChatText != value)
                {
                    _displayChatText = value;
                    OnPropertyChanged(nameof(DisplayChatText));
                }
            }
        }

        public string TextBoxJoinOrLeaveChat
        {
            get { return _textBoxJoinOrLeaveChat; }
            set
            {
                if (_textBoxJoinOrLeaveChat != value)
                {
                    if (value.Length > maxCharsPerLine)
                    {
                        ErrorMessage = $"Input cannot exceed {maxCharsPerLine} characters."; // Set an error message if the input exceeds the max characters
                    } else
                    {
                        ErrorMessage = string.Empty; // Clear error message
                        _textBoxJoinOrLeaveChat = value;
                        OnPropertyChanged(nameof(TextBoxJoinOrLeaveChat));

                        // Update button states
                        IsMaleButtonEnabled = !string.IsNullOrWhiteSpace(_textBoxJoinOrLeaveChat);
                        IsFemaleButtonEnabled = !string.IsNullOrWhiteSpace(_textBoxJoinOrLeaveChat);
                    }
                }
            }
        }

        public string TextBoxPassword
        {
            get { return _textBoxPassword; }
            set
            {
                if (_textBoxPassword != value)
                {
                    if (value.Length > maxCharsPerLine)
                    {
                        ErrorMessage = $"Input cannot exceed {maxCharsPerLine} characters."; // Set an error message if the input exceeds the max characters
                    } else
                    {
                        ErrorMessage = string.Empty; // Clear error message
                        _textBoxPassword = value;
                        OnPropertyChanged(nameof(TextBoxPassword));
                    }
                }
            }
        }

        public bool IsMaleButtonEnabled
        {
            get { return _isMaleButtonEnabled; }
            set
            {
                if (_isMaleButtonEnabled != value)
                {
                    _isMaleButtonEnabled = value;
                    OnPropertyChanged(nameof(IsMaleButtonEnabled));
                }
            }
        }

        public bool IsFemaleButtonEnabled
        {
            get { return _isFemaleButtonEnabled; }
            set
            {
                if (_isFemaleButtonEnabled != value)
                {
                    _isFemaleButtonEnabled = value;
                    OnPropertyChanged(nameof(IsFemaleButtonEnabled));
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

        public string TextBoxCreateNewChat
        {
            get { return _textBoxCreateNewChat; }
            set
            {
                if (_textBoxCreateNewChat != value)
                {
                    _textBoxCreateNewChat = value;
                    OnPropertyChanged(nameof(TextBoxCreateNewChat));
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
        #endregion

        private ObservableCollection<string> _buttonList = new ObservableCollection<string>();
        public ObservableCollection<string> ButtonList
        {
            get { return _buttonList; }
            set
            {
                _buttonList = value;
                OnPropertyChanged(nameof(ButtonList));
            }
        }

        public async void OnClick_CreateNewChat()
        {
            var newChat = new Chat
            {
                _name = TextBoxCreateNewChat,
                _id = 0,
                _memberBelong = new Member
                {
                    _name = "John Doe",
                    _gender = "Male",
                    _age = 25,
                    _isManager = false,
                    _isLogin = true,
                    _isRegistered = false,
                    _passwordHash = ""
                }
            };
            TextBoxCreateNewChat = string.Empty;

            try
            {
                var response = await _apiServiceChats.AddChatAsync(newChat);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var addedChat = Newtonsoft.Json.JsonConvert.DeserializeObject<Chat>(responseBody);
                    if (addedChat != null)
                    {
                        _chatsList.Add(addedChat);

                        ButtonList.Add(addedChat._name); // Add the new chat name to the button list

                        DisplayChatText = string.Empty; // Update the UI
                        OnPropertyChanged(nameof(DisplayChatText));
                    }
                    else
                    {
                        ErrorMessage = "Failed to parse the chat details from the response";
                    }
                }
                else
                {
                    ErrorMessage = $"Failed to add chat: {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = "Failed to add chat: " + ex.Message;
            }
        }

        private async void OnClick_Register()
        {
                var newMember = new Member
                {
                    _name = TextBoxJoinOrLeaveChat,
                    _id = 0,
                    _gender = _selectedGender,
                    _age = _selectedAge,
                    _isManager = false,
                    _isLogin = false,
                    _isRegistered = true,
                    _password = "",
                    _passwordHash = !string.IsNullOrEmpty(TextBoxPassword) ? BCrypt.Net.BCrypt.HashPassword(TextBoxPassword) : "",
                    _role = ""
                };
                TextBoxJoinOrLeaveChat = string.Empty; TextBoxPassword = string.Empty;

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
                            DisplayTextJoinChat = string.Join("\n", _chatMembers.Select(m => $"{m._name}  ({m._gender} {m._age})"));
                            OnPropertyChanged(nameof(DisplayTextJoinChat));
                        } else
                        {
                            ErrorMessage = "Failed to parse the member details from the response.";
                        }
                    } else
                    {
                        ErrorMessage = $"Failed to add member: {response.ReasonPhrase}";
                    }
                } catch (HttpRequestException ex)
                {
                    ErrorMessage = "Failed to add member: " + ex.Message;
                }
        }

        private async void OnClick_Login()
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(TextBoxJoinOrLeaveChat) || string.IsNullOrWhiteSpace(TextBoxPassword))
            {
                ErrorMessage = "Please enter both username and password.";
                return;
            }

            try
            {
                var loginRequest = new MemberLogin // Create a MemberLogin DTO
                {
                    _name = TextBoxJoinOrLeaveChat,
                    _passwordHash = TextBoxPassword
                };

                string token = await _apiServiceAuth.LoginAsync(loginRequest); // Call the login service with the DTO and get the token

                if (!string.IsNullOrEmpty(token))
                {
                    _jwtToken = token; // Store the JWT token
                    
                    var claims = DecodeJwtToken(_jwtToken); // Decode the JWT token to extract claims

                    // Check if required claims are present
                    if (claims.TryGetValue("name", out string? username) && claims.ContainsKey("role"))
                    {
                        var role = claims["role"];

                        DisplayTextJoinChat = $"Logged in as {username} (Role: {role})"; // Update UI or perform actions based on claims

                        // Clear the input fields
                        TextBoxJoinOrLeaveChat = string.Empty;
                        TextBoxPassword = string.Empty;

                        Console.WriteLine($"Login successful: {username} (Role: {role})");
                    } else
                    {
                        // Claims are missing, handle accordingly
                        ErrorMessage = "Login successful, but required claims are missing in the token.";
                        Console.WriteLine("Decoded claims: " + string.Join(", ", claims));
                    }
                } else
                {
                    ErrorMessage = "Login failed: No token received."; // No token received, login failed
                }
            } catch (Exception ex)
            {
                // Handle exceptions
                ErrorMessage = "Login failed: " + ex.Message;
                Console.WriteLine(ex.ToString());
            }
        }

        private Dictionary<string, string> DecodeJwtToken(string token)
        {
            var claims = new Dictionary<string, string>();
            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var parts = token.Split('.');

                    if (parts.Length == 3)
                    {
                        var payload = parts[1];

                        var jsonBytes = _apiServiceAuth.Base64UrlDecode(payload); // Base64-URL decode the payload

                        var jsonString = Encoding.UTF8.GetString(jsonBytes); // Convert the bytes to a string

                        // Deserialize the JSON to a dictionary
                        claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString)
                                 ?? new Dictionary<string, string>();
                    } else
                    {
                        Console.WriteLine("Invalid JWT token: Incorrect number of parts.");
                    }
                } else
                {
                    Console.WriteLine("Empty or null JWT token provided.");
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Error decoding JWT token: {ex.Message}");
            }
            return claims;
        }
    }
}