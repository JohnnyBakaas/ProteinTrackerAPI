namespace ProteinTrackerAPI.Model
{
    public class DB
    {
        public static List<User> Users;
        public static List<Food> Foods;
        public static List<Weight> Weights;
        private static bool _used = false;

        public DB()
        {
            if (!_used)
            {
                _used = true;
                Users = new();
                Foods = new();
                Weights = new();

                Users.Add(new User("john_smith", 1001, "password123", "admin", "male"));
                Users.Add(new User("jane_doe", 1002, "testpassword", "user", "female"));
                Users.Add(new User("bob_johnson", 1003, "ilovecoding", "user", "male"));
            }
        }

        public static void DataDump()
        {
            Console.WriteLine("_____________");
            Console.WriteLine("Users");
            foreach (var user in Users)
            {
                Console.WriteLine("------------");
                user.DeBug();
            }
            Console.WriteLine("_____________");
        }

        public static void UpdateUser(User inputUser)
        {
            string userName = inputUser.GetUserName();
            bool newUser = true;
            for (int i = 0; i < Users.Count; i++)
            {
                if (userName == Users[i].GetUserName())
                {
                    Users[i] = inputUser;
                    newUser = false;
                    break;
                }
            }
            if (newUser)
            {
                Users.Add(inputUser);
            }
        }

        public static void AddFood(Food newFood)
        {
            Foods.Add(newFood);
        }

        public static void AddWeight(Weight newWeight)
        {
            Weights.Add(newWeight);
        }
    }
}
