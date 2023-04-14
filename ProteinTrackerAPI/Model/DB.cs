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

                Users.Add(new User("john_smith", 1, "password123", "admin", "male"));
                Users.Add(new User("jane_doe", 2, "testpassword", "user", "female"));
                Users.Add(new User("bob_johnson", 3, "ilovecoding", "user", "male"));

                Foods.Add(new Food
                {
                    Name = "Apple",
                    Id = 1,
                    Kcal = 95,
                    Protein = 1,
                    ConsumptionDateTime = new DateTime(2023, 4, 14, 12, 0, 0),
                    UserId = 1
                });

                Foods.Add(new Food
                {
                    Name = "Banana",
                    Id = 2,
                    Kcal = 105,
                    Protein = 1,
                    ConsumptionDateTime = new DateTime(2023, 4, 14, 12, 30, 0),
                    UserId = 1
                });

                Foods.Add(new Food
                {
                    Name = "Grilled Chicken",
                    Id = 3,
                    Kcal = 180,
                    Protein = 26,
                    ConsumptionDateTime = new DateTime(2023, 4, 14, 19, 0, 0),
                    UserId = 1
                });

                Foods.Add(new Food
                {
                    Name = "Salmon",
                    Id = 4,
                    Kcal = 207,
                    Protein = 23,
                    ConsumptionDateTime = new DateTime(2023, 4, 14, 20, 0, 0),
                    UserId = 2
                });

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
            Users.ForEach(x => x.GetMeals());
        }
    }
}
