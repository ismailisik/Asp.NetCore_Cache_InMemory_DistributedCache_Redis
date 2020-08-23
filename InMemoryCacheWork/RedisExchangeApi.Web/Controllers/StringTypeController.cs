using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = redisService.GetDb(0);
        }  
        public IActionResult Index()
        {
            db.StringSet("Adı Soyadı", "İsmail Işık");
            db.StringSet("Ziyaretci", 10);
            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("Adı Soyadı");
            var result = db.StringGet("Ziyaretci");

            //Eğer redis üzerinde asyn bir işlem yapıcaksanız async-await kullanmak istemiyorsanız action da Result kullanılmalıdır.
            var guncelZiyaretci= db.StringIncrementAsync("Ziyaretci", 10).Result; //Bu ifade bana Ziyaretci nin value değerini 1 arttırıp güncel veriyi bana geri döner.

            db.StringDecrementAsync("Ziyaretci", 1).Wait(); //Action'umda async yapmadan rediste bir yadı asyc olarak atabilmek için Wait methodunu kullandım.
            
            //Not: Redis te veri tipleri ile ilgili birçok method bulunmaktadır.Bunların Hepsi burada denenmemiştir dökümantasyonundan araştırılabilir.

            if (value.HasValue)
            {
                ViewBag.AdSoyad = value.ToString();
                ViewBag.Ziyaretci = guncelZiyaretci;
            }
            return View();
        }
    }
}