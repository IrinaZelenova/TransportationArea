using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TransportationArea.DBProviderService.Data
{
    /// <summary>
    /// Структура БД
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        [Display(Name = "Город погрузки")]
        [Required(ErrorMessage = "Не указан город погрузки")]
        [StringLength(255)]
        public LoadingPoints SendSity { get; set; }

        [Display(Name = "Адрес погрузки")]
        [Required(ErrorMessage = "Не указан адрес погрузки")]
        [StringLength(255)]
        public string SendAddress { get; set; }
        [Display(Name = "Город получателя")]
        [Required(ErrorMessage = "Не указан город получателя")]
        [StringLength(255)]
        public LoadingPoints ReceivedSity { get; set; }

        [Display(Name = "Адрес доставки")]
        [Required(ErrorMessage = "Не указан адрес доставки")]
        [StringLength(100)]
        public string ReceivedAddress { get; set; }

        [Display(Name = "Наименование груза")]
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [Display(Name = "Вес")]
        [Required(ErrorMessage = "Не указан вес")]
        public double Mass { get; set; }

       
        [Display(Name = "Дата дата забора груза")]
        [Required(ErrorMessage = "Не указана дата")]
        [DataType(DataType.Date)]
        public DateTime LoadingDate { get; set; }
               
        public double Price { get; set; }

        public OrderRoute? orderRoute { get; set; }
     }

   
    public class OrderRoute
    {       
        public int Id { get; set; }        
        public List<Area> route { get; set; }
    }

       public enum OrderStatusName
        {
            [Display(Name = "Новый")]
            //[Description("Новый")]
            New = 0,
            [Description("Назначены автомобили и маршрут")]
            AssignCarsAndRoute = 1,
            [Description("В пути")]
            OnTheWay = 2,
            [Description("Доставлен")]
            Finished = 3,
            [Description("Приостановлен")]
            Stopped =200 ,
            [Description("Отменен")]
            Canceled = 400          
           
         }
    /// <summary>
    /// Статус этапа на конкретную дату и история изменения
    /// </summary>
    public class OrderStatus
    {
        public int Id { get; set; }

        public Order Order { get; set; }
        public OrderStatusName Status { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Active { get; set; }

        //TODO add сервис получения из Description
        public string GetStatusName()
        {
            switch (Status)
            {
                case OrderStatusName.New: return "Новый";
                case OrderStatusName.AssignCarsAndRoute: return "Назначены автомобили и маршрут";
                case OrderStatusName.OnTheWay: return "В пути";
                case OrderStatusName.Finished: return "Доставлен(Завершен)";
                case OrderStatusName.Stopped: return "Приостановлен";
                case OrderStatusName.Canceled: return "Отменен";              
            }
            return "Статус не определен";
        }

        public string? GetNextCommand()
        {
            switch (Status)
            {
                case OrderStatusName.New: return "Назначить маршрут и автомобиль";
                case OrderStatusName.AssignCarsAndRoute: return "Согласовать ";
                case OrderStatusName.OnTheWay: return "Завершить доставку";
                case OrderStatusName.Finished: return null;
                case OrderStatusName.Stopped: return "Запустить";
                case OrderStatusName.Canceled: return null;
            }
            return "Статус не определен";
        }
    }


public class LoadingPoints
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Area Area { get; set; }
    }

    public class Area
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Область не указана")]
        public string Name { get; set; }

        [Display(Name = "Цена перехода по зоне")]
        [Required(ErrorMessage = "Цена не указана")]
        public double Price { get; set; }
    }

    /// <summary>
    /// Определяем соседство зон
    /// </summary>
    public class GridOfArea
    {
        public int Id { get; set; }

        public Area Area1 { get; set; }

        public Area Area2 { get; set; }
    }

    
    public class Car
    {
        public int Id { get; set; }

        
        [Display(Name = "Номер автомобиля")]
        [Required(ErrorMessage = "Необходимо заполнить")]        
        public string Number{ get; set; }

      
        [Display(Name = "Грузоподъемность")]
        [Required(ErrorMessage = "Необходимо заполнить")]
        public double Capacity { get; set; }
    }

    public class CarRoute
    {
        public int Id { get; set; }

        public Car Car { get; set; }

        public Order Order { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public double Mass { get; set; }

        public LoadingPoints SendSity { get; set; }
        public LoadingPoints ReceivedSity { get; set; }

        public CarRouteStatusName Status { get; set; }

    }

    public enum CarRouteStatusName
    {
        [Description("Новый")]
        New = 0,
        [Description("В пути")]
        OnTheWay = 1,
        [Description("Доставлен")]
        Finished = 2,
        [Description("Приостановлен")]
        Stopped = 200,
        [Description("Отменен")]
        Canceled = 400

    }

}
