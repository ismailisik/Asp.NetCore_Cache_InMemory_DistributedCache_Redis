using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private readonly string setName = "HashName";
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = redisService.GetDb(2);
        }
        //Redis ile ilgili 2 tane daha veriş tipi vardır(Sorted List ve Hash).Bu proje kapsamında redis entegrasyonu ve bu 3 tip incelemesi yapılmıştır.Detaylar için Redisin dökümanları incelenebilir.

        //Önemli Not: Set'in list typendan en büyük farkı Setin verileri unique dir ve rastgele kayıt atar. List başına yada sonuna atar ve unique şartı aramaz.
        public IActionResult Index()
        {
            List<string> users = new List<string>();
            if (db.KeyExists(setName))
            {
                db.SetMembers(setName).ToList().ForEach(x=> {
                    users.Add(x.ToString());
                });
            }
            return View(users);
        }

        public IActionResult Add(string name)
        {
            db.SetAdd(setName, name);
            db.KeyExpire(setName, DateTime.Now.AddSeconds(30)); //Expire Time bu şekilde verilebilir.
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string name)
        {
            db.SetRemoveAsync(setName, name).Wait();
            return RedirectToAction("Index");
        }
      
    }
}