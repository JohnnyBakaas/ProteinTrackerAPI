namespace ProteinTrackerAPI.Model
{
    [Serializable]
    public class User
    {
        public string UserName { get; set; }
        public int Id { get; set; }
        private string Password { get; set; }
        public int KcalDelta { get; set; }
        public string Gender { get; set; }
        public List<Food> Meals { get; set; }
        public List<Weight> Weights { get; set; }


        public User(string userName, int id, string password, int delta, string gender)
        {
            UserName = userName;
            Id = id;
            Password = password;
            KcalDelta = delta;
            Gender = gender;
            Meals = new List<Food>();
            Weights = new List<Weight>();
        }

        public string GetUserName() { return UserName; }
        public void DeBug()
        {
            Console.WriteLine(UserName);
            Console.WriteLine(KcalDelta);
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
