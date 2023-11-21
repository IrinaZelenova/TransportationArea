using Microsoft.AspNetCore.Routing;
using System.Collections;
using System.Collections.Generic;
using TransportationArea.DBProviderService;
using TransportationArea.DBProviderService.Data;

namespace TransportationArea.SettlementCenter
{
    /// <summary>
    /// Класс вспомогательных математических расчетов
    /// </summary>
    public class OptimalRoute
    {
        Services _services;
        public OptimalRoute(Services services) {
            _services = services;
        }
        /// <summary>
        /// Поиск всех возможных маршрутов по зонам
        /// </summary>
        /// <param name="areaStart"></param>
        /// <param name="areaFinish"></param>
        /// <returns></returns>
        public List<List<Area>>? SearchAllRoute(Area areaStart,Area areaFinish)
        {
            List<List<Area>> routes= new List<List <Area>> ();
            List<Area> route= new List<Area>() { areaStart};          
            SearchRoute(areaStart, areaFinish, route, routes);
            return routes;
        }

        void SearchRoute(Area areaStart, Area areaFinish, List<Area> route, List<List<Area>> routes)
        {
           if (areaStart == areaFinish) {
                List<Area> routeNew= new List<Area>(route);
                routes.Add(routeNew);             
                return;
              }

            List<Area>? areasNext;
            if (route.Count >= 2) {
                areasNext = _services.SeachGridOfArea(route[route.Count - 1]).Where(x => !route.Contains(x)).ToList();
            }
            else 
             {
                areasNext = _services.SeachGridOfArea(route[route.Count - 1]);
             }
            
            if (areasNext.Count == 0) {
                route.RemoveRange(route.Count - 1, 1);
                return;
            }

            foreach (var area in areasNext)
            {
                route.Add(area);
                SearchRoute(area, areaFinish, route, routes);
                route.Remove(area);
            }

        }

        /// <summary>
        /// Поиск свободных автомобилей для постановки на заказ
        /// </summary>
        /// <param name="IdOrder"></param>
        /// <returns></returns>
        public List<Car> SearchCars(int IdOrder)
        {
            if (_services.GetOrderStatus(IdOrder).Result.Where(x => x.Active).Select(x => x.Status).First() != OrderStatusName.New)
            {
                return null;
            }

            return _services.GetFreeCars(IdOrder);
           
        }
    }
}
