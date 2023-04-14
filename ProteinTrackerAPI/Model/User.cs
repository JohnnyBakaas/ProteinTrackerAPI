namespace ProteinTrackerAPI.Model
{
    [Serializable]
    public class User
    {
        public string UserName { get; set; }
        public int Id { get; set; }
        public string Password { get; set; }
        public string Mode { get; set; }
        public string Gender { get; set; }

        public User(string userName, int id, string password, string mode, string gender)
        {
            UserName = userName;
            Id = id;
            Password = password;
            Mode = mode;
            Gender = gender;
        }

        public string GetUserName() { return UserName; }
        public void DeBug()
        {
            Console.WriteLine(UserName);
            Console.WriteLine(Mode);
        }
    }
}
