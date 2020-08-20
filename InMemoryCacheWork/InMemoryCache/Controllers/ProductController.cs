﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCache.Controllers
{
    public class ProductController : Controller
    {
        //En basit haliyle IMemoryCache interface mi controllerıma injecte ediyorum.
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;

        }

        public IActionResult Index()
        {
            //Aşağıda oluşturduğum basit bir cache'in süresini belirlemek istiyorsam AbsuluteExpiration kullanırım.Bu benim cache'mi belirlediğim süre dahilinde memory'de tutar ardından siler.

            //MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            //options.AbsoluteExpiration = DateTime.Now.AddSeconds(10); //şimdiki zamandan 10 sn sonrası 

            //Peki ben datamın cache'de kalma süresini her istek aldığında 5 sn arttırmak eğer belirli bir süre istek almaz ise silinmesini ister isem o zamnda SlindingExpiration kullanırım.

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.SlidingExpiration = TimeSpan.FromSeconds(5); //İstek geldikçe 5 sn arttır.
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(30); //Herhalükarda 30 sn sonra sil.

            //Önemli Not: Eğer bizim cache mize sürekli istek gelirse SlindingExpiration kullandığımızda cache deki datamız eski kalabilir. Bu yüzden hem SlindingExpiration hemde AbsoluteExpiration ikisini beraber tanımlamak mantıklı olandır. Datamız aralıklıda olsa güncelleniyorsa.

            //Önemli Not: Eğer beim memory dolarsa ne olur cache'lerim önem sıralarına göre silinmeye başlarlar.Bu sebepten dolayı ben cache'lerime priority vermelim.Ben her cache'me NeverRemove verirsemde memorym doluca exception alırm.
            options.Priority = CacheItemPriority.NeverRemove; // 4 seçenek sorar (low,hight,normal,NeverRemove)

            //Set ile string tipinde bir veri set ediyorum
            _memoryCache.Set<string>("time", DateTime.Now.ToString(),options); //oluşturduğum options u 3 parametre olarak veriyorum


            //******InMemoryCache ile ilgili kontroller********//

            //1.Eğer ben ilk olarak bir key değerine ait cache varmı yok mu kontrol etmek istiyorsam ve yoksa cache oluşturmak istyiyorsam
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //{
            //    //Yoksa burada time keyime ait datamı set edebilirim.
            //}

            //2.Yukarıdakinin alternatifi(TryGetValue: Bu method bana verdiğim key ile ilgili bir data var mı kontrol eder. Boolien döner. Eğer var ise ben onu 2.parametrede out olarak verdiğim değişkenime o keye ait cache te olan değerimi atayabilirim).
            //if (!_memoryCache.TryGetValue("time",out string timeNow))
            //{
            //    //Burada time key'ine ait cache'im yok ise atabilirim.Var ise timeNow değişkenim ile bu değeri kullanabilirim.
            //}

            //3.Yukarıdakilerin bir diğer alternatifi(GetOrCreate: Bu method eğer istediğim key değerine ait value varsa getiri yoksa oluşturur.)
           //var sameData= _memoryCache.GetOrCreate("time", entry =>
           // {
           //     return DateTime.Now.ToString(); //Bakıcak time keyine ait cache varmı yopsa oluşturup valuesine benim return ettiğm değeri atıyacak.
           // });

            return View();
        }

        public IActionResult ShowTime()
        {
            //Get ile string tipinde seet ettiğim datamı yine aynı tipte geri çekiyorum.
            //ViewBag.time = _memoryCache.Get<string>("time");

            _memoryCache.TryGetValue("time", out string time);

            ViewBag.time = time;

            return View();
        }
    }
}