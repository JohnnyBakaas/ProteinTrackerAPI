namespace ProteinTrackerAPI.Model
{
    public class Food
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Kcal { get; set; }
        public int Protein { get; set; }
        public DateTime ConsumptionDateTime { get; set; }
        public int UserId { get; set; }
        public string TokenFromClient { get; set; }

        private static int _lastId = 0;

        public Food(string name, int kcal, int protein, DateTime consumptionDateTime, int userId)
        {
            Name = name;
            Id = _lastId;
            Kcal = kcal;
            Protein = protein;
            ConsumptionDateTime = consumptionDateTime;
            UserId = userId;
            _lastId++;
        }
    }
}
