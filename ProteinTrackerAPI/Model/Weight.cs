namespace ProteinTrackerAPI.Model
{
    public class Weight
    {
        public int MeshuredWeight { get; set; }
        public int Id { get; set; }
        public string Coment { get; set; }
        public int UserId { get; set; }
        public DateTime WeightDateTime { get; set; }

        private static int _lastId = 0;

        public Weight(int waight, string coment, int userId, DateTime weightDateTime)
        {
            _lastId++;
            Coment = coment;
            Id = _lastId;
            UserId = userId;
            MeshuredWeight = waight;
            WeightDateTime = weightDateTime;

        }
    }
}
