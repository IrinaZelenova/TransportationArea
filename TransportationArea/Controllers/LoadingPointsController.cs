using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using TransportationArea.DBProviderService;
using TransportationArea.DBProviderService.Data;
using TransportationArea.Models;

namespace TransportationArea.Controllers
{
   /// <summary>
   /// Контроллер для Работы с Городами
   /// </summary>
    public class LoadingPointsController : Controller
    {
        Services _services;

        public  LoadingPointsController(Services services)
        {
            _services = services;
        }
        public IActionResult CreateCity()
        {
            AreasModel model = new AreasModel();
            model.areas = _services.GetAreas();
            return View(model);
        }


        [HttpPost]
        public IActionResult CreateCity(string Name, int IdArea)
        {
            _services.AddLoadingPoints(Name, IdArea);
            return RedirectToAction("LoadingPointsDirectory");
        }


        public async Task<IActionResult> LoadingPointsDirectory()
        {

            LoadingPointsModel model = new LoadingPointsModel()
            {
                LoadingPointsRepository = await _services.GetLoadingPoints()
            };
            return View(model);
        }

        public IActionResult ChangeLoadingPoint(int Id)
        {
            var LoadingPoint = _services.GetLoadingPoints(Id);
            LoadingPointModel model = new LoadingPointModel()
            {
                Id=LoadingPoint.Id,
                Name=LoadingPoint.Name,
                Area=LoadingPoint.Area,
                areas = _services.GetAreas()
            };
           return View(model);
        }

        [HttpPost]
        public IActionResult ChangeLoadingPoint(LoadingPointModel model)
        {
            
            if (model.Id!=null && model.Name!=null && model.Area.Id!=null)
            _services.ChangeLoadingPoints(model.Id, model.Name, model.Area.Id);
            return RedirectToAction("LoadingPointsDirectory");
        }

    }
}
