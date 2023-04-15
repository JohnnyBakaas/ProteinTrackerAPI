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
        public List<Food> Meals { get; set; }
        public List<Weight> Weights { get; set; }


        public User(string userName, int id, string password, string mode, string gender)
        {
            UserName = userName;
            Id = id;
            Password = password;
            Mode = mode;
            Gender = gender;
            Meals = new List<Food>();
            Weights = new List<Weight>();
        }

        public string GetUserName() { return UserName; }
        public void DeBug()
        {
            Console.WriteLine(UserName);
            Console.WriteLine(Mode);
        }


        public void GetMeals()
        {
            Meals = DB.Foods.Where(obj => obj.UserId == this.Id).ToList();
        }

        public void GetWeights()
        {
            Weights = DB.Weights.Where(obj => obj.UserId == this.Id).ToList();
        }

        public bool ValidateUsernameAndPasword(string userName, string password)
        {
            if (UserName == userName && Password == password) return true;
            return false;
        }

    }
}
