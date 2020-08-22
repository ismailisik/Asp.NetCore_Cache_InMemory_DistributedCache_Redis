using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDistributedCacheRedis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.IO;

namespace IDistributedCacheRedis.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public IActionResult Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

            //Basit bir string cache oluşturma....
            //_distributedCache.SetString("Adı Soyadı", "Harun IŞIK",options);

            //Complex type bir veriyi ilk önce serilize etmemiz gerekmektedir (Json yada binary gibi)
            var ProductList = new List<Product>() 
            {
                new Product{Id=1,ProductName="Test Product1",UnitInStock=40,UnitPrice=4.45},
                new Product{Id=2,ProductName="Test Product2",UnitInStock=30,UnitPrice=5.45},
                new Product{Id=3,ProductName="Test Product3",UnitInStock=50,UnitPrice=6.45},
                new Product{Id=4,ProductName="Test Product4",UnitInStock=60,UnitPrice=7.45},
                new Product{Id=5,ProductName="Test Product5",UnitInStock=70,UnitPrice=8.45}
            };

            //Json serilize
            var ProductJson = JsonConvert.SerializeObject(ProductList);
            //_distributedCache.SetString("Product:1", ProductJson);

            //Binary serilize
            Byte[] byteProducts = Encoding.UTF8.GetBytes(ProductJson);
            _distributedCache.Set("Product:1", byteProducts);

            return View();
        }

        public IActionResult GetCache()
        {
            //var name = _distributedCache.GetString("Adı Soyadı");
            //ViewBag.AdSoyad = name;

            //JSON

            //var ProductListJson = _distributedCache.GetString("Product:1");
            //if (ProductListJson != null)
            //{
            //    //Json deserilize
            //    var ProductList = JsonConvert.DeserializeObject<List<Product>>(ProductListJson);
            //    ViewBag.Products = ProductList;
            //}


            //BINARAY

            var byteProduct = _distributedCache.Get("Product:1");

            if (byteProduct!=null)
            {
                var ProductString = Encoding.UTF8.GetString(byteProduct);
                var ProductList = JsonConvert.DeserializeObject<List<Product>>(ProductString);
                ViewBag.Products = ProductList;
            }
           
            return View();
        }

        public IActionResult RemoveCache()
        {
            _distributedCache.Remove("Product:1");
            return View();
        }

        public IActionResult CacheImageFile()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/soyut.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("images", imageByte);

            return View();
        }

        public IActionResult CacheImagesShow()
        {
            byte[] imageByte = _distributedCache.Get("images");

            return File(imageByte, "image/jpg");
        }
       
    }
}