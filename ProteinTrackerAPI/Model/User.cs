using System.Text.Json;

namespace ProteinTrackerAPI.Model
{
    public class User
    {
        private int _id;
        private string _name;
        private string _gender;
        private string _userName;
        private string _password;
        private int _weight;

        public List<User> MakeUserData()
        {
            List<User> users = new List<User>();
            users.Add(new User()
            {
                _id = _id,
                _name = _name,
                _gender = _gender,
                _userName = _userName,
                _password = _password,
                _weight = _weight
            });
            string json = JsonSerializer.Serialize(users);
        }

    }
}
