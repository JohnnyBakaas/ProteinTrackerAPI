using MySqlConnector;
using System.Globalization;

namespace ProteinTrackerAPI.Model
{
    public static class DB
    {
        public static List<User> Users = new List<User>();
        public static List<Food> Foods = new List<Food>();
        public static List<Weight> Weights = new List<Weight>();

        public static MySqlConnection connection;

        public static void ConectDBToMySQL()
        {
            string connectionString = "server=localhost;port=3306;user=root;password=;database=protein_app";
            connection = new MySqlConnection(connectionString);
            connection.Open();

            // TESTER VIDRE UNDER
            string query = "SELECT * FROM users";

            using MySqlCommand command = new MySqlCommand(query, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader["UserName"]} - {reader["KcalDelta"]}");
            }
        }

        public static User LoginUser(string userName, string pasword)
        {
            string query = $"SELECT * FROM `users` WHERE `UserName` LIKE '{userName}' AND `Pasword` LIKE '{pasword}'";

            using MySqlCommand command = new MySqlCommand(query, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            User foundUser = null;

            while (reader.Read())
            {
                Console.WriteLine($"{reader["UserName"]} - {reader["KcalDelta"]} - It's me you are looking fore");
                foundUser = new User(
                        reader["UserName"].ToString(),
                        int.Parse(reader["Id"].ToString()),
                        reader["Pasword"].ToString(),
                        reader["KcalDelta"].ToString() == "" ?
                            0
                            :
                            int.Parse(reader["KcalDelta"].ToString()),
                        reader["Gender"].ToString()
                        );
            };

            return foundUser;
        }

        public static User GetUserFromToken(string token)
        {
            User foundUser = null;

            string query = $"SELECT * FROM `users` WHERE `Id` LIKE '{SessionToken.TokenStringToId(token)}'";
            using MySqlCommand command = new MySqlCommand(query, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"{reader["UserName"]} - {reader["KcalDelta"]} - It's me you are looking fore");
                foundUser = new User
                    (
                        reader["UserName"].ToString(),
                        int.Parse(reader["Id"].ToString()),
                        reader["Pasword"].ToString(),
                        reader["KcalDelta"].ToString() == "" ?
                            0
                            :
                            int.Parse(reader["KcalDelta"].ToString()),
                        reader["Gender"].ToString()
                    );
            };
            reader.DisposeAsync();

            if (foundUser != null)
            {
                ConectFoodsToUser(foundUser);
                ConectWeightsToUser(foundUser);
            }

            return foundUser;
        }

        private static void ConectFoodsToUser(User user)
        {
            string foodQuery = $"SELECT * FROM `food` WHERE `UserId` = {user.Id}";
            using MySqlCommand foodCommand = new MySqlCommand(foodQuery, connection);
            using MySqlDataReader foodReader = foodCommand.ExecuteReader();
            while (foodReader.Read())
            {
                user.Meals.Add(new Food
                    (
                        foodReader["Name"].ToString(),
                        int.Parse(foodReader["Kcal"].ToString()),
                        int.Parse(foodReader["Protein"].ToString()),
                        DateTime.ParseExact(foodReader["ConsumptionDateTime"].ToString(), "dd.MM.yyyy HH.mm.ss", CultureInfo.InvariantCulture),
                        int.Parse(foodReader["UserId"].ToString())
                    ));
            }
            foodReader.Dispose();
        }

        private static void ConectWeightsToUser(User user)
        {
            string weightQuery = $"SELECT * FROM `weight` WHERE `UserId` = {user.Id}";
            using MySqlCommand weightCommand = new MySqlCommand(weightQuery, connection);
            using MySqlDataReader weightReader = weightCommand.ExecuteReader();
            while (weightReader.Read())
            {
                user.Weights.Add(new Weight
                    (
                        Convert.ToDecimal(weightReader["MeshuredWeight"].ToString()),
                        weightReader["Coment"].ToString(),
                        int.Parse(weightReader["UserId"].ToString()),
                        DateTime.ParseExact(weightReader["WeightDateTime"].ToString(), "dd.MM.yyyy HH.mm.ss", CultureInfo.InvariantCulture)
                    ));
            }
            weightReader.Dispose();
        }

        public static void AddFoodToSQL(Food food)
        {
            string query = @$"INSERT INTO `food` (`Id`, `Name`, `Kcal`, `Protein`, `ConsumptionDateTime`, `UserId`) VALUES 
        (NULL, '{food.Name}', '{food.Kcal}', '{food.Protein}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', '{food.UserId}')";

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public static void AddWeightToSQL(string weight, string coment, string tokenFromClient)
        {
            string query = $@"INSERT INTO `weight` (`Id`, `UserId`, `MeshuredWeight`, `Coment`, `WeightDateTime`) VALUES 
        (NULL, '{SessionToken.TokenStringToId(tokenFromClient)}', '{weight}', '{coment}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public static void UpdateKcalDeltaInSQL(int delta, string tokenFromClient)
        {
            if (SessionToken.TokenStringToId(tokenFromClient) != -1)
            {
                string query = $"UPDATE `users` SET `KcalDelta` = '{delta}' WHERE `users`.`Id` = {SessionToken.TokenStringToId(tokenFromClient)}";
                using MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        // Gammel kode↓

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

        public static void UpdateKcalDelta(int id, int delta)
        {
            try
            {
                DB.Users.First(e => e.Id == id).KcalDelta = delta;

            }
            catch { Console.WriteLine("UpdateKcalDelta has faild"); }
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

            string query = "SELECT * FROM users";

            using MySqlCommand command = new MySqlCommand(query, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Users.Add(
                    new User(
                        reader["UserName"].ToString(),
                        int.Parse(reader["Id"].ToString()),
                        reader["Pasword"].ToString(),
                        reader["KcalDelta"].ToString() == "" ?
                            0
                            :
                            int.Parse(reader["KcalDelta"].ToString()),
                        reader["Gender"].ToString()
                        )
                    );
                Console.WriteLine($"{reader["UserName"]} - {reader["KcalDelta"]}");
            }


            Users.ForEach(x => { x.ConectMeals(); x.ConectWeights(); });
        }
    }
}
