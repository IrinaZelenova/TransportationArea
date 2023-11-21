using TransportationArea.DBProviderService.Data;

namespace TransportationArea.Models
{
    public class OrderModel
    {

        public Order? Order { get; set; }
        public List<LoadingPoints>? LoadingPoint { get; set; }   
        public List<List<Area>> Routes { get; set; }
        public List<Car>? freeCar { get; set; }
    }

    public class OrderCardModel
    {
        public Order? Order { get; set; }
        public List<OrderStatus>? OrderStatus { get; set; }
       public List<CarRoute> CarRoutes { get; set; }
    }


    public class OrdersModel
    {
      // public List<Order>? OrdersRepository;
        public List<OrderStatus>? OrdersRepository;
    }

   

}
