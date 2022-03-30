namespace Movies.Client.Models
{
    public class UserInfoViewModel
    {
        public Dictionary<string, string> UserDictionary { get; private set; } = null;

        public UserInfoViewModel(Dictionary<string, string> userDictionary)
        {
            this.UserDictionary = userDictionary;
        }
    }
}
