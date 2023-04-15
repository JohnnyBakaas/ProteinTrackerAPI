namespace ProteinTrackerAPI.Model
{
    public class SesionToken
    {
        private int _userId { get; set; }
        public string TokenString { get; set; }

        private static List<SesionToken> _tokenList { get; set; }
        private static bool _first = true;

        public SesionToken(int userId)
        {
            if (_first)
            {
                _tokenList = new List<SesionToken>();
                _first = false;
            }

            _userId = userId;
            TokenString = GenerateToken();
            _tokenList.Add(this);
        }

        private string GenerateToken()
        {
            string result = string.Empty;
            var rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < 256; i++)
            {
                result += chars[rnd.Next(chars.Length)];
            }
            if (_tokenList.Any(T => T.TokenString == result))
            {
                return GenerateToken();
            }
            return result;
        }

        public static int TokenStringToId(string tokenString)
        {
            try
            {
                return _tokenList.First(T => T.TokenString == tokenString)._userId;
            }
            catch
            {
                return -1;
            }
        }

    }
}
