using TransportationArea.DBProviderService;
using TransportationArea.DBProviderService.Data;

namespace TransportationArea.SettlementCenter
{
    public class StatusServices
    {
        /// <summary>
        /// Контроллер для работы со статусами
        /// </summary>
        Services _services;
        public StatusServices(Services services) {
            _services = services;
        }
        public void AddOrderStatus(Order order, OrderStatusName statusName,DateTime dateStart,DateTime dateEnd) {
            OrderStatus orderStatus = new OrderStatus()
            {
                Order = order,
                Status = statusName,
                Active = true,
                Start =dateStart,
                End = dateEnd
            };
            _services.AddOrderStatus(orderStatus);

        }

    }
}
