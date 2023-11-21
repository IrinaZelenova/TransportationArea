using TransportationArea.DBProviderService.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TransportationArea.DBProviderService
{
    /// <summary>
    /// Сервис запросов к БД
    /// </summary>
    public class Services
    {
        private readonly ApplicationContext _datebase;
        public Services(ApplicationContext datebase)
        {
            _datebase = datebase;
        }

        public Order AddOrder(Order order)
        {
            _datebase.Orders.Add(order);
            _datebase.SaveChanges();
            return order;
        }

        public void AddOrderStatus(OrderStatus orderStatus)
        {
            _datebase.OrdersStatus.Add(orderStatus);
            _datebase.SaveChanges();
        }
        public void AddArea(Area area)
        {
            _datebase.Areas.Add(area);
            _datebase.SaveChanges();
        }

        public void AddCar(Car car)
        {
            _datebase.Cars.Add(car);
            _datebase.SaveChanges();
        }

        public void AddLoadingPoints(string Name,int IdArea)
        {
            LoadingPoints loadingPoints = new LoadingPoints()
            {
                Name = Name,
                Area =GetArea(IdArea)
            };
            _datebase.LoadingPoints.Add(loadingPoints);
            _datebase.SaveChanges();
        }

        public void AddOrderRoute(OrderRoute orderRoute,int IdOrder, List<int> FreeCars)
        {
            var order = GetOrder(IdOrder);

            var cars = CreateFreeCarList(FreeCars, order.Mass);
            if (cars.Sum(x => x.Capacity) < order.Mass) return;


            _datebase.OrderRoutes.Add(orderRoute);
            order.orderRoute =orderRoute;
            order.Price = orderRoute.route.Sum(x => x.Price);

            var activeStatus = GetOrderStatus(IdOrder).Result.Where(x => x.Active).First();
            activeStatus.End = DateTime.Now;
            activeStatus.Active = false;

            OrderStatus orderStatus = new OrderStatus()
            {
                Order = order,
                Status = OrderStatusName.AssignCarsAndRoute,
                Start = DateTime.Now,
                Active = true
            };
            var freeMass = order.Mass;
            foreach (var car in cars) {

                CarRoute carRoute = new CarRoute()
                {
                    Car = car,
                    Order = order,
                    Start = order.LoadingDate,
                    SendSity=order.SendSity,
                    ReceivedSity=order.ReceivedSity,
                    Status=CarRouteStatusName.New,
                    Mass = MassСalculation(car.Capacity, freeMass)
                };
                freeMass= freeMass-carRoute.Mass;
                _datebase.CarRoutes.Add(carRoute);
            }
            _datebase.OrdersStatus.Add(orderStatus);
            _datebase.SaveChanges();
        }

        double MassСalculation(double carMass,double orderMass)
        {
            if (orderMass - carMass >= 0) return carMass;
            else return orderMass;
        }

        public void AddNewOrderStatusName(int IdOrder,OrderStatusName orderStatusName)
        {
            var order = GetOrder(IdOrder);

            var activeStatus = GetOrderStatus(IdOrder).Result.Where(x => x.Active).First();
            activeStatus.End = DateTime.Now;
            activeStatus.Active = false;

            OrderStatus orderStatus = new OrderStatus()
            {
                Order = order,
                Status = orderStatusName,
                Start = DateTime.Now,
                Active = true
            };
            if (orderStatusName == OrderStatusName.Finished)
            {
                orderStatus.End = DateTime.Now;
            }
           
            _datebase.OrdersStatus.Add(orderStatus);
            _datebase.SaveChanges();
        }

        public void AddNewCarRouteStatusName(int IdOrder, CarRouteStatusName carStatus) 
        {
            var cars = GetCarRoutes(IdOrder).Result.Where(x => x.Status < CarRouteStatusName.Finished).ToList();
            foreach(var car in cars)
            {
                car.Status = carStatus;
            }
            _datebase.SaveChanges();
        }
        public List<Car> CreateFreeCarList(List<int> carId,double orderMass)
        {
            List<Car> cars = new List<Car>();
            foreach(var id in carId)
            {
                var car = GetCar(id);
                if (orderMass - car.Capacity > 0)
                {
                    cars.Add(car);
                }
                else
                {
                    cars.Add(car);
                    break;
                }

                
            }
            return cars;
        }


        public async void ChangeLoadingPoints(int Id,string Name, int IdArea)
        {
            var loadingPoints =await GetLoadingPoints(Id);
            loadingPoints.Name = Name;
            loadingPoints.Area = GetArea(IdArea);
            _datebase.SaveChanges();
        }

        public List<Area> GetAreas()
        {
            return _datebase.Areas
                            .OrderBy(x => x.Id)
                            .ToList();
        }

        
        public async Task<List<GridOfArea>> GetGridAreas()
        {
            return await _datebase.GridsOfArea
                        .Include(x => x.Area1)
                        .Include(x => x.Area2)
                        .OrderBy(x => x.Area1)
                        .ToListAsync();
        }
        public Area GetArea(int IdArea)
        {
            return  _datebase.Areas.Where(x => x.Id == IdArea).First();
        }

        public async Task<Area>? GetAreaByPoint(int IdLoadingPoint)
        {
            return await _datebase.LoadingPoints
                   .Include(x => x.Area)
                   .Where(x => x.Id == IdLoadingPoint)
                   .Select(x => x.Area)
                   .FirstAsync();
                
        }

        public async Task<List<LoadingPoints>> GetLoadingPoints()
        {
            return await _datebase.LoadingPoints.
                         Include(x=>x.Area)
                         .OrderBy(x => x.Id)
                         .ToListAsync();
        }

        public async Task<LoadingPoints> GetLoadingPoints(int Id)
        {
            return await _datebase.LoadingPoints
                         .Include(x => x.Area)
                         .Where(x => x.Id == Id)
                         .FirstAsync();
        }

        public async Task<List<LoadingPoints>> GetLoadingPointsWithoutArea()
        {
            return await _datebase.LoadingPoints
                         .ToListAsync();
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _datebase.Orders
                         .Include(x=>x.SendSity)
                         .Include(x=>x.ReceivedSity)
                         .OrderBy(x=>x.Id)
                         .ToListAsync();
                         
        }

        public async Task<List<OrderStatus>> GetOrderStatus(int IdOrder)
        {
            return await _datebase.OrdersStatus
                          .Where(x => x.Order.Id == IdOrder)
                          .ToListAsync();

        }

        public async Task<Area> GetOrderAreaSend(int Id)
        {
            return await _datebase.Orders
                         .Include(x => x.SendSity).ThenInclude(x=>x.Area)
                         .Where(x => x.Id == Id)
                         .Select(x=>x.SendSity.Area)
                         .FirstAsync();
        }

        public async Task<Area> GetOrderAreaReceived(int Id)
        {
            return await _datebase.Orders
                         .Include(x => x.ReceivedSity).ThenInclude(x => x.Area)
                         .Where(x => x.Id == Id)
                         .Select(x => x.ReceivedSity.Area)
                         .FirstAsync();
        }

        public async Task<Order> GeAllOrder(int IdOrder)
        {
            return await _datebase.Orders
                         .Include(x => x.SendSity)
                         .Include(x => x.ReceivedSity)
                         .Include(x => x.orderRoute).ThenInclude(x => x.route)
                          .Where(x => x.Id == IdOrder)
                          .FirstAsync();

        }

        public Order GetOrder(int IdOrder)
        {
            return _datebase.Orders
                          .Include(x => x.SendSity)
                          .Include(x => x.ReceivedSity)
                          .Where(x => x.Id == IdOrder)
                          .First();

        }
        public async Task<List<OrderStatus>> GetOrdersStatus()
        {
            return await _datebase.OrdersStatus
                          .Include(x=>x.Order)
                          .ThenInclude(x => x.SendSity)
                          .Include(x => x.Order)
                          .ThenInclude(x => x.ReceivedSity)
                          .Where(x=>x.Active)
                          .OrderBy(x => x.Order.Id)
                          .ToListAsync();

        }

        public void AddGridOfArea(int area1Id, int area2Id)
        {
            int result=_datebase.GridsOfArea.Where(x=>(x.Area1.Id==area1Id && x.Area2.Id==area2Id) || (x.Area1.Id == area2Id && x.Area2.Id == area1Id)).Count();
            if (result == 0)
            {
                GridOfArea grid = new GridOfArea()
                {
                    Area1 = GetArea(area1Id),
                    Area2 = GetArea(area2Id)
                };
                _datebase.GridsOfArea.Add(grid);
                _datebase.SaveChanges();
            }
        }

        public void DeleteGridOfArea(int gridId)
        {
                _datebase.GridsOfArea.Where(x => x.Id == gridId).ExecuteDelete();
                _datebase.SaveChanges();
          
        }

        public void ChangeArea(Area area)
        {
           _datebase.Areas.Update(area);
            _datebase.SaveChanges();
        }

        public List<Car> GetCars()
        {
            return _datebase.Cars
                  .OrderBy(x => x.Id)
                   .ToList();   
        }

        public Car? GetCar(int Id)
        {
            return _datebase.Cars
                   .Where(x=>x.Id==Id).FirstOrDefault();
        }
        public void EditCar(Car car)
        {
            _datebase.Cars.Update(car);
            _datebase.SaveChanges();
        }

        public List<Area> SeachGridOfArea(Area area)
        {
            return _datebase.GridsOfArea                        
                        .Include(x => x.Area2)
                        .Where(x => x.Area1 == area)
                        .Select(x => x.Area2)
                        .ToList()
                        .Union(
                                _datebase.GridsOfArea
                               .Include(x => x.Area1)
                               .Where(x => x.Area2 == area)
                                .Select(x => x.Area1)
                                .ToList()
                              )
                        .Distinct()
                        .ToList();
            
            
        }

        public async Task<List<CarRoute>> GetCarRoutes(int IdOrder)
        {
            return await _datebase.CarRoutes
                            .Include(x => x.Order)
                            .Include(x => x.Car)
                            .Include(x => x.SendSity)
                            .Include(x => x.ReceivedSity)
                            .Where(x => x.Order.Id == IdOrder)
                             .ToListAsync();
        }

        public List<Area> CreateRoute(List<int> areaId)
        {
            List<Area> routes = new List<Area>();
            foreach(int id in areaId)
            {
                routes.Add(GetArea(id));
            }
            return routes;
        }

        public List<Car> GetFreeCars(int IdOrder)
        {
            Order order = GetOrder(IdOrder);
           var BusyCars=_datebase.CarRoutes
                         .Include(x=>x.Car)
                         .Where(x=>x.Status< CarRouteStatusName.Finished || x.End>order.LoadingDate || (x.End != order.LoadingDate && x.ReceivedSity!=order.ReceivedSity))
                         .Select(x=>x.Car).Distinct().ToList();
            if (BusyCars.Count == 0) return _datebase.Cars.ToList();
             return _datebase.Cars.Except(BusyCars).ToList();   

        }

    }
}
