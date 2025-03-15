using CHAT_APP_CLIENT.Extensions;
using CHAT_APP_CLIENT.Services;
using SERVER_SIDE.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace CHAT_APP_CLIENT.View_Models
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly ApiServiceMembers _apiServiceMembers; // API service to get the data from the ASP.NET CORE WEB API back-end
        private readonly ApiServiceMessages _apiServiceMessages;
        private readonly SignalRService _signalRService;
        
        // Commands for different button actions
        public ICommand sendMessageCommand { get; private set; } // Send message command
        public ICommand joinChatCommand { get; private set; } // Join chat command
        public ICommand leaveChatCommand { get; private set; } // Left the chat command
        public ICommand clearAllMembersCommand { get; private set; } // Clear all members command
        public ICommand clearChatCommand { get; private set; } // Clear the caht command
        public ICommand connectMemberCommand { get; private set; } // Join connected memebr to the chat command
        public ICommand disconnectMemberCommand { get; private set; } // Join connected memebr to the chat command
        public ICommand genderMaleMemberCommand { get; private set; } // Memeber gender male command
        public ICommand genderFemaleMemebrCommand { get; private set; } // Member gender female command
        public ICommand agesCommand { get; private set; } // Member ages list command
        public ICommand selectedAgeCommand { get; private set; } // Member selected age command
        public ICommand navigateCommand { get; private set; } // Navigate command

        // strings for write and display the text in the UI
        private string _textBoxMessage = string.Empty;
        private string _textBoxJoinOrLeaveChat = string.Empty;
        private string _displayText = string.Empty;
        private string _deleteText = string.Empty;
        private string _errorMessage = string.Empty;
        private string _memberLogin = string.Empty;
        private string _chatLog = string.Empty;
        private string _txtMemberName = string.Empty;
        private string _txtJoinConectedOrDissconnectedMemberChat = string.Empty;
        private bool _isMaleButtonEnabled = false;
        private bool _isFemaleButtonEnabled = false;
        private string _selectedGender = string.Empty;
        private int _selectedAge = 0;

        // collections of strings for collect all the messages and users that are in the chat
        private ObservableCollection<string> _linesMessage; // Collection that will contain all the messages in the chat
        private ObservableCollection<string> _linesJoinChat; // Collection that will contain all the names of the users who joined the chat
        private ObservableCollection<int> _agesList;

        private ObservableCollection<Member> _chatMembers = new ObservableCollection<Member>(); // collecation for collect all the memeber that availiable in the chat
        private ObservableCollection<Member> _connectedMembers = new ObservableCollection<Member>();
        private ObservableCollection<Message> _chatMessages = new ObservableCollection<Message>(); // collecation for collect all the messages that availiable in the chat

        private const int maxCharsPerLine = 100; // define the maximum letters for a memeber name in the chat

        public ViewModelBase()
        {
            IsMaleButtonEnabled = false; // Set the genders buttons to be not ebaled
            IsFemaleButtonEnabled = false; // Set the genders buttons to be not ebaled

            _signalRService = new SignalRService();
            sendMessageCommand = new Commands(async () => await SendMessageAsync());

            // Subscribe to incoming messages from the server
            _signalRService.OnMessageReceived((user, message) =>
            {
                ChatLog += $"{user}: {message._content}\n"; // Update the chat log when a new message is received
            });

            _ = StartSignalRConnectionAsync(); // Start the SignalR connection. _ - to avoid warning

            // Initial collections
            _linesMessage = new ObservableCollection<string>(); // Initial the collection that include all the msessages in the chat
            _linesJoinChat = new ObservableCollection<string>(); // Initial the collection that include all the users who joined the chat
            _agesList = new ObservableCollection<int>();

            _chatMembers = new ObservableCollection<Member>(); // Initialize the ObservableCollection for chat members
            _connectedMembers = new ObservableCollection<Member>();
            _chatMessages = new ObservableCollection<Message>(); // Initialize the ObservableCollection for chat messages

            _apiServiceMembers = new ApiServiceMembers();
            _apiServiceMessages = new ApiServiceMessages();
            InitializeCommands(); // Initialize commands with their corresponding methods
            InitialAgesList(); // Initial the ages list

            LoadMembers(); // Load members when the ViewModel is instantiated
            LoadMessages(); // Load messages when the ViewModel is instantiated
        }

        private void InitialAgesList()
        {
            AgesList = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
            11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
            30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48,
            49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67,
            68, 89, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
            87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100};
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
                
                await _signalRService.SendMessageAsync(MemberLogin, message); // Send the message to the server using the SignalR service

                TextBoxMessage = string.Empty; // Clear the message box after sending
            }
        }

        #region API operation
        // Display all the memebrs that alreadt exist in the caht (in the data base)
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
            } catch (HttpRequestException ex)
            {
                ErrorMessage = "Failed to load members: " + ex.Message;
            }
        }

        // Display all the messages that alreadt exist in the caht (in the data base)

        public async void LoadMessages()
        {
            try
            {
                var messages = await _apiServiceMessages.GetAllMessages();
                if (messages != null)
                {
                    ChatMessages.Clear(); // Clear existing messages before adding new ones
                    foreach (var message in messages)
                    {
                        ChatMessages.Add(message);
                    }

                    // Ensure DisplayTextMessage is updated after loading messages
                    DisplayTextMessage = string.Join("\n", ChatMessages.Select(m => $"[{m._sender}]:\t{m._content}"));
                }
            } catch (HttpRequestException ex)
            {
                ErrorMessage = "Failed to load messages: " + ex.Message;
            }
        }

        // Initialize commands here
        private void InitializeCommands()
        {
            sendMessageCommand = new Commands(OnClick_SendMessage);
            joinChatCommand = new Commands(OnClick_JoinNewMemberInChat);
            leaveChatCommand = new Commands(OnClick_LeaveChat);
            clearAllMembersCommand = new Commands(OnClick_ClearMembers);
            clearChatCommand = new Commands(OnClick_ClearMessagesChat);
            connectMemberCommand = new Commands(OnClick_JoinConnectedMemberChat);
            disconnectMemberCommand = new Commands(OnClick_DisconnectMemberChat);
            genderMaleMemberCommand = new Commands(OnClick_MemebrGenderMale);
            genderFemaleMemebrCommand = new Commands(OnClick_MemebrGenderFemale);
            selectedAgeCommand = new Commands(OnSelected_SelectedMemberAge);
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
            get { return _chatMembers; }
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

        public ObservableCollection<int> AgesList
        {
            get { return _agesList; }
            set
            {
                _agesList = value;
                OnPropertyChanged(nameof(AgesList));
            }
        }

        public int SelectedMemberAge
        {
            get { return _selectedAge; }
            set
            {
                if (_selectedAge != value)
                {
                    _selectedAge = value;
                    OnPropertyChanged(nameof(SelectedMemberAge));
                    // You can add additional logic here if necessary
                }
            }
        }

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

        public string TextBoxMemberName
        {
            get { return _txtMemberName; }
            set
            {
                if (_txtMemberName != value)
                {
                    _txtMemberName = value;
                    OnPropertyChanged(nameof(TextBoxMemberName));
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

        public string TextBoxJoinConnectedOrDissconnectedMemberChat
        {
            get { return _txtJoinConectedOrDissconnectedMemberChat; }
            set
            {
                if (_txtJoinConectedOrDissconnectedMemberChat != value)
                {
                    if (value.Length > maxCharsPerLine)
                    { 
                        ErrorMessage = $"Input cannot exceed {maxCharsPerLine} characters."; // Set an error message if the input exceeds the max characters
                    } else
                    {
                        ErrorMessage = string.Empty; // Clear error message
                        _txtJoinConectedOrDissconnectedMemberChat = value;

                        OnPropertyChanged(nameof(TextBoxJoinConnectedOrDissconnectedMemberChat)); // Notify property change for TextBoxJoinConnectedOrDissconnectedMemberChat
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

        public string DisplayTextConnectedMember
        {
            get { return _displayText; }
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged(nameof(DisplayTextConnectedMember));
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

        private void MemebrConnection(Member currentMember) // Not made async to prevent warning
        {
            if (!string.IsNullOrEmpty(MemberLogin))
            {
                currentMember._isLogin = true;
            }
        }

        // Method that handles sending a message
        private async void OnClick_SendMessage()
        {
            var memberName = TextBoxMemberName;
            var member = _chatMembers.FirstOrDefault(m => m._name == memberName); // Find member

            if (string.IsNullOrWhiteSpace(TextBoxMemberName) && string.IsNullOrEmpty(TextBoxMessage))
            {
                MessageBox.Show("Enter the member name at the top of the page, and enter a message to the chat at the bottom of the page.");
            } else if (string.IsNullOrWhiteSpace(TextBoxMemberName))
            {
                MessageBox.Show("Enter the member name at the top of the page.");
            } else if (string.IsNullOrEmpty(TextBoxMessage))
            {
                MessageBox.Show("Enter a message to the chat at the bottom of the page.");
            } else if (member == null)
            {
                MessageBox.Show("The member is not in the chat.");
            } else if (!member._isLogin && member != null)
            {
                MessageBox.Show("The member is in the chat but not connected to the chat. Connect.");
            } else
            {
                if (!string.IsNullOrEmpty(TextBoxMessage))
                {
                    var newMessage = new Message
                    {
                        _content = TextBoxMessage, // Use the string here
                        _id = 0,
                        _sender = TextBoxMemberName // Assign the member name as the sender
                    };
                    TextBoxMessage = string.Empty; // Clear the message box, but keep the member name

                    try
                    {
                        var response = await _apiServiceMessages.AddMessageToChatAsync(newMessage);
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();
                            var addMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(responseBody);

                            if (addMessage != null)
                            {
                                ChatMessages.Add(addMessage); // Add the new message to the collection
                                DisplayTextMessage = string.Join("\n", ChatMessages.Select
                                    (m => $"[{m._sender}]:\t{m._content}")); // Proper concatenation

                                OnPropertyChanged(nameof(DisplayTextMessage));
                            } else
                            {
                                ErrorMessage = "Failed to parse the message details from the response.";
                            }
                        } else
                        {
                            ErrorMessage = $"Failed to add message: {response.ReasonPhrase}";
                        }
                    } catch (HttpRequestException ex)
                    {
                        ErrorMessage = "Failed to add message: " + ex.Message;
                    }
                } else
                {
                    ErrorMessage = $"Input cannot be empty. Please enter a message.";
                }
            }
        }

        private async void OnClick_MemebrGenderMale()
        {
            IsMaleButtonEnabled = false;
            IsFemaleButtonEnabled = true;
            if (IsMaleButtonEnabled == false)
            {
                _selectedGender = "Male";
            }
        }

        private async void OnClick_MemebrGenderFemale()
        {
            IsFemaleButtonEnabled = false;
            IsMaleButtonEnabled = true;
            if (IsFemaleButtonEnabled == false)
            {
                _selectedGender = "Female";
            }
        }

        private bool CanClick_MaleButton()
        {
            return !IsMaleButtonEnabled;  // Can execute if the male button is enabled
        }

        private bool CanClick_FemaleButton()
        {
            return !IsFemaleButtonEnabled;  // Can execute if the female button is enabled
        }

        public async void OnSelected_SelectedMemberAge()
        {
            var ages = AgesList;
            foreach (var age in ages)
            {
                if (SelectedMemberAge == age)
                {
                    _selectedAge = age;
                    break;
                }
            }
        }

        private async void OnClick_JoinNewMemberInChat()
        {
            if (_selectedGender == "" && _selectedAge == 0)
            {
                MessageBox.Show("No gender and age selected");
            } else if (_selectedGender == "")
            {
                MessageBox.Show("No gender was selected");
            } else if (_selectedAge == 0)
            {
                MessageBox.Show("No age was selected");
            } else if (!string.IsNullOrEmpty(TextBoxJoinOrLeaveChat))
            {
                var newMember = new Member
                {
                    _name = TextBoxJoinOrLeaveChat,
                    _id = 0,
                    _gender = _selectedGender,
                    _age = _selectedAge,
                    _isManager = false,
                    _isLogin = false,
                    _isRegistered = false,
                    _passwordHash = "rnd password",
                    _role = ""
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
            } else
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
                            _chatMembers.Remove(member); // Remove only the deleted member from the ObservableCollection

                            TextBoxJoinOrLeaveChat = string.Empty; // Clear TextBoxJoinOrLeaveChat after deletion

                            DisplayTextJoinChat = string.Join("\n", _chatMembers.Select(m => m._name)); // Update DisplayTextJoinChat with the remaining members
                        }
                    } catch (Exception ex)
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

        private async void OnClick_JoinConnectedMemberChat()
        {
            if (!string.IsNullOrEmpty(TextBoxJoinConnectedOrDissconnectedMemberChat))
            {
                var memberName = TextBoxJoinConnectedOrDissconnectedMemberChat;
                var member = _chatMembers.FirstOrDefault(m => m._name == memberName); // Find member

                if (member != null)
                {
                    try
                    {
                        member._isLogin = true; // Set member login status to true

                        var updatedMember = await _apiServiceMembers.UpdateLoginFieldToTrue(member._id, member); // Call the service method to update the login field in the database

                        if (updatedMember != null)
                        {
                            _connectedMembers.Add(member); // Update local list of connected members

                            // Clear the text box and update display text
                            TextBoxJoinConnectedOrDissconnectedMemberChat = string.Empty;
                            DisplayTextConnectedMember = string.Join("\n", _connectedMembers.Select(m => $"{m._name}" +
                            $"  ({m._gender} {m._age})"));
                            OnPropertyChanged(nameof(DisplayTextConnectedMember));
                        } else
                        {
                            ErrorMessage = "Failed to update login status in the database.";
                        }
                    } catch (Exception ex)
                    {
                        ErrorMessage = "Failed to add member: " + ex.Message;
                    }
                } else
                {
                    MessageBox.Show($"{TextBoxJoinConnectedOrDissconnectedMemberChat} is not currently connected to the chat.");
                }
            } else
            {
                MessageBox.Show("No member name provided. Enter a member name to connect to the chat.");
            }
        }

        private async void OnClick_DisconnectMemberChat()
        {
            if (!string.IsNullOrEmpty(TextBoxJoinConnectedOrDissconnectedMemberChat))
            {
                var memberName = TextBoxJoinConnectedOrDissconnectedMemberChat;
                var member = _chatMembers.FirstOrDefault(m => m._name == memberName); // Find member

                if (member != null)
                {
                    try
                    {
                        member._isLogin = false; // Set member login status to false (disconnecting the member)

                        var updatedMember = await _apiServiceMembers.UpdateLoginFieldToFalse(member._id, member); // Call the service method to update the login field in the database

                        if (updatedMember != null)
                        {
                            
                            _connectedMembers.Remove(member); // Remove member from the conection members list

                            TextBoxJoinConnectedOrDissconnectedMemberChat = string.Empty; // Clear the text box and update display text
                            DisplayTextConnectedMember = string.Join("\n", _connectedMembers.Select(m => m._name));
                            OnPropertyChanged(nameof(DisplayTextConnectedMember));
                        } else
                        {
                            ErrorMessage = "Failed to update login status in the database.";
                        }
                    } catch (Exception ex)
                    {
                        ErrorMessage = "Failed to disconnect member: " + ex.Message;
                    }
                } else
                {
                    MessageBox.Show($"{TextBoxJoinConnectedOrDissconnectedMemberChat} is not currently connected to the chat.");
                }
            } else
            {
                MessageBox.Show("No member name provided. Enter a member name to disconnect from the chat.");
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
            } catch (Exception ex)
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
            } catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while clearing messages: {ex.Message}";
                OnPropertyChanged(nameof(ErrorMessage)); // Notify UI to update the error message
            }
        }
    }
}