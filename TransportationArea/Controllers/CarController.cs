using Microsoft.AspNetCore.Mvc;
using TransportationArea.DBProviderService.Data;
using TransportationArea.DBProviderService;

namespace TransportationArea.Controllers
{

    public class CarController : Controller
    {
        Services _services;

        public CarController(Services services)
        {
            _services = services;
        }
        public IActionResult CreateCar(Car car)
        {
            if (!string.IsNullOrEmpty(car.Number) && car.Capacity > 0)
            {
                _services.AddCar(car);
                return RedirectToAction("CarsDirectory");

            }
            return View();
        }

      
        public IActionResult CarsDirectory(){           
            return View(_services.GetCars());
         }

        public IActionResult EditCar(int Id)
        {
            return View(_services.GetCar(Id));
        }

        [HttpPost]
        public IActionResult EditCar(Car model)
        {
            if (model.Id!=null && model.Capacity>0 && model.Number != null){
                _services.EditCar(model);
               
            }
            return RedirectToAction("CarsDirectory");
        }
    }
}
