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
    }
}
