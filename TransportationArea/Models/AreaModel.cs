using TransportationArea.DBProviderService.Data;

namespace TransportationArea.Models
{
    public class AreaModel
    {
    }
    public class CreateAreaModel
    {
        public List<Order>? OrdersRepository;
    }

    public class AreasModel
    {

        public string? Name { get; set; }
        public int IdArea { get; set; }

        public List<Area>? areas { get; set; }

    }

    public class GridAreasModel
    {
        public List<Area>? areas { get; set; }

        public List<LoadingPoints>? loadingPoints { get; set; }
        public List<GridOfArea>? gridAreas { get; set; }
    }

    public class GridAreaModel
    {
        public List<Area>? areas { get; set; }
        public Area area1 { get; set; }

        public Area area2 { get; set; }

        public int? areaSelect {  get; set; }
    }
}
