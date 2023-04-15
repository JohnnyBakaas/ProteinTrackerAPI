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

                Users.Add(new User("test", 1, "test", "admin", "male"));
                Users.Add(new User("jane_doe", 2, "testpassword", "user", "female"));
                Users.Add(new User("bob_johnson", 3, "ilovecoding", "user", "male"));

                Foods.Add(new Food("Apple", 95, 1, new DateTime(2023, 4, 14, 12, 0, 0), 1));
                Foods.Add(new Food("Banana", 105, 1, new DateTime(2023, 4, 14, 12, 30, 0), 1));
                Foods.Add(new Food("Grilled Chicken", 180, 26, new DateTime(2023, 4, 14, 19, 0, 0), 1));
                Foods.Add(new Food("Salmon", 207, 23, new DateTime(2023, 4, 14, 20, 0, 0), 2));

                Weights.Add(new Weight(170, "Starting weight", 1, new DateTime(2023, 4, 10, 10, 0, 0)));
                Weights.Add(new Weight(168, "Lost 2 pounds", 1, new DateTime(2023, 4, 11, 10, 0, 0)));
                Weights.Add(new Weight(166, "Lost another 2 pounds", 1, new DateTime(2023, 4, 12, 10, 0, 0)));
                Weights.Add(new Weight(165, "Lost 1 pound", 1, new DateTime(2023, 4, 13, 10, 0, 0)));
                Weights.Add(new Weight(164, "Lost 1 more pound", 1, new DateTime(2023, 4, 14, 10, 0, 0)));



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
            bool newUser = true;
            for (int i = 0; i < Users.Count; i++)
            {
                if (inputUser.Id == Users[i].Id)
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

        public static void ConectData()
        {
            Users.ForEach(x => { x.GetMeals(); x.GetWeights(); });
        }
    }
}
