using TransportationArea.DBProviderService.Data;

namespace TransportationArea.Models
{
    public class LoadingPointsModel
    {
        public List<LoadingPoints>? LoadingPointsRepository;
    }

    public class LoadingPointModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Area Area { get; set; }
        public List<Area>? areas { get; set; }
    }
}
