using Microsoft.AspNetCore.Mvc;
using TransportationArea.DBProviderService.Data;
using TransportationArea.DBProviderService;
using System;
using TransportationArea.Models;
using Microsoft.AspNetCore.Authorization;
using TransportationArea.SettlementCenter;

namespace TransportationArea.Controllers
{
    /// <summary>
    /// Контроллер для работы с заказами
    /// </summary>
    public class OrderController : Controller
    {
        Services _services;
        OptimalRoute _optimalRoute;
        StatusServices _statusServices;
        public OrderController(Services services,OptimalRoute optimalRoute, StatusServices statusServices)
        {
            _services = services;
            _optimalRoute = optimalRoute;
           _statusServices = statusServices;
        }
        public IActionResult Index()
        {
            return View();
        }

     
        public async Task<IActionResult> OrdersDirectory()
        {           
            OrdersModel model = new OrdersModel()
            {
                OrdersRepository = await _services.GetOrdersStatus()
            };
            return View(model);
        }

        
       
        public async Task<IActionResult> CreateOrder()
        {
            OrderModel model = new OrderModel()
            {
                LoadingPoint = await _services.GetLoadingPoints()
            };
            return View(model);
                       
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderModel model)
        {
            model.Order.SendSity = await _services.GetLoadingPoints(model.Order.SendSity.Id);
            model.Order.ReceivedSity = await _services.GetLoadingPoints(model.Order.ReceivedSity.Id);
            var newOrder = _services.AddOrder(model.Order); 
            _statusServices.AddOrderStatus(newOrder,OrderStatusName.New,DateTime.Now,DateTime.Now.AddDays(1));
            return RedirectToAction("OrdersDirectory");            
        }



        public async Task<IActionResult> CreateOrderRoutes(int IdOrder)
        {

            OrderModel model = new OrderModel()
            {
                Routes = _optimalRoute.SearchAllRoute( await _services.GetOrderAreaSend(IdOrder), await _services.GetOrderAreaReceived(IdOrder)),
                Order=await _services.GeAllOrder(IdOrder),
                freeCar=_optimalRoute.SearchCars(IdOrder)
            };
            return View(model);
        }


       
        public IActionResult SaveRouteOrder(int IdOrder,List<int> Route,List<int> Cars)
        {
          
            OrderRoute orderRoute = new OrderRoute()
            {
                route = _services.CreateRoute(Route)
            };
                       
            _services.AddOrderRoute(orderRoute, IdOrder,Cars);
            return RedirectToAction("OrderCard", new {Id=IdOrder});
        }

        public async Task<IActionResult> OrderCard(int Id)
        {
            OrderCardModel model = new OrderCardModel()
            {
                Order=await _services.GeAllOrder(Id),
                OrderStatus = await _services.GetOrderStatus(Id),
                CarRoutes=await _services.GetCarRoutes(Id)

            };
            return View(model);
        }

        
        public IActionResult Distributor(int orderId, OrderStatusName Status)
        {
            switch (Status)
            {
                case OrderStatusName.New: return RedirectToAction("CreateOrderRoutes", new { IdOrder=orderId });
                case OrderStatusName.AssignCarsAndRoute: return RedirectToAction("AcceptOnTheWay", new { IdOrder = orderId });
                case OrderStatusName.OnTheWay: return RedirectToAction("AcceptFinished", new { IdOrder = orderId });

            }
            return RedirectToAction("OrdersDirectory");
        }

        public IActionResult AcceptOnTheWay(int IdOrder)
        {
            _services.AddNewOrderStatusName(IdOrder,OrderStatusName.OnTheWay);
            return RedirectToAction("OrderCard", new { Id = IdOrder });
        }

        public IActionResult AcceptFinished(int IdOrder)
        {
            _services.AddNewOrderStatusName(IdOrder, OrderStatusName.Finished);
            _services.AddNewCarRouteStatusName(IdOrder, CarRouteStatusName.Finished);
            return RedirectToAction("OrderCard", new { Id = IdOrder });
        }

    }
}
