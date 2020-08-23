using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private readonly string listName = "Users";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> UserList = new List<string>();
            if (db.KeyExists(listName)) //Verdiğim key'e ait veri var mı? 
            {
                db.ListRange(listName).ToList().ForEach(x=> {
                    UserList.Add(x.ToString());
                });
            }
            return View(UserList);
        }
        public IActionResult Add(string name)
        {
            db.ListLeftPushAsync(listName, name).Wait();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(string name)
        {
            db.ListRemoveAsync(listName,name).Wait();
            return RedirectToAction("Index");
        }
        public IActionResult DeleteFirst()
        {
            db.ListLeftPopAsync(listName);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteLast()
        {
            db.ListRightPopAsync(listName);
            return RedirectToAction("Index");
        }

    }
}