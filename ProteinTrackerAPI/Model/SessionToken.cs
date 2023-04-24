namespace ProteinTrackerAPI.Model
{
    public class SessionToken
    {
        private int _userId { get; set; }
        public string TokenString { get; set; }

        private static List<SessionToken> _tokenList { get; set; } = new List<SessionToken>();


        public SessionToken(int userId)
        {
            _userId = userId;
            TokenString = GenerateToken();
            _tokenList.Add(this);
        }

        private string GenerateToken()
        {
            string result;
            var rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            do
            {
                result = string.Empty;
                for (int i = 0; i < 256; i++)
                {
                    result += chars[rnd.Next(chars.Length)];
                }
            } while (_tokenList.Any(T => T.TokenString == result));

            return result;
        }

        public static int TokenStringToId(string tokenString)
        {
            try
            {
                var foundToken = _tokenList.FirstOrDefault(T => T.TokenString == tokenString);
                if (foundToken != null)
                {
                    return foundToken._userId;
                }
                else
                {

                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in TokenStringToId: " + ex.Message);
                return -1;
            }
        }

        public static void DebugList()
        {
            _tokenList.ForEach(e => Console.WriteLine(e.TokenString));
        }

    }
}
