using Microsoft.AspNetCore.Mvc;
using TransportationArea.DBProviderService.Data;
using TransportationArea.DBProviderService;
using TransportationArea.Models;

namespace TransportationArea.Controllers
{
    /// <summary>
    /// Контроллер для работы с Зонами
    /// </summary>
    public class AreaController : Controller
    {
        Services _services;

        public AreaController(Services services)
        {
            _services = services;
        }

      
        public IActionResult CreateArea(string Name, double Price)
        {
            if (!string.IsNullOrEmpty(Name) && Price > 0)
            {
                Area area = new Area()
                {
                    Name = Name,
                    Price = Price
                };
                _services.AddArea(area);
                return RedirectToAction("AreasDirectory");

            }
            return View();
        }

        
        public IActionResult AddGridOfArea(int areaId)
        {
            GridAreaModel model = new GridAreaModel()
            {
                areas = _services.GetAreas(),
                areaSelect=areaId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddGridOfArea(GridAreaModel? model)
        {
            if (model.area1.Id != null && model.area2.Id!=null && model.area1.Id!=model.area2.Id) { 
                _services.AddGridOfArea(model.area1.Id,model.area2.Id);
            }
            return RedirectToAction("AreasDirectory");
        }

        public IActionResult DeleteGridOfArea(int gridId)
        {
            if (gridId!=null && gridId>0)
            {
                _services.DeleteGridOfArea(gridId);
            }
            return RedirectToAction("AreasDirectory");
        }

        public async Task<IActionResult> AreasDirectory()
        {
            GridAreasModel model = new GridAreasModel()
            {
                areas=_services.GetAreas(),
                gridAreas = await _services.GetGridAreas(),
                loadingPoints= await _services.GetLoadingPointsWithoutArea()
            };
            
            return View(model);
        }

        public IActionResult ChangeArea(int Id)
        {
            var model=_services.GetArea(Id);
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeArea(Area model)
        {
            if (model.Id != null && model.Name != null && model.Price != null)
            _services.ChangeArea(model);
            return RedirectToAction("AreasDirectory");
        }


    }
}
