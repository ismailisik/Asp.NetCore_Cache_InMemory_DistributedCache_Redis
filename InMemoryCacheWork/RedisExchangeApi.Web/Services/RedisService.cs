using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeApi.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;

        //Redis ile haberleşicek main classımız...(Connect methoduna bakz.)
        private ConnectionMultiplexer _redis;

        //Redis ile gelen 15 tane db den seçim yapmamıza yarayacak interface...(GetDb methoduna bakz.)
        public IDatabase db { get; set; }
        public RedisService(IConfiguration configuration)
        {
            _redisHost = configuration["Redis:Host"]; //appSettings bakmz.
            _redisPort = configuration["Redis:Port"]; //appSettings bakmz.
        }


        //Uygulamam ayağa kalktığında benim bu methodu çalıştırmam lazım ki redise bağlanabileyim. (Startup bakz.)
        public void Connect()
        {
            var ConfigString = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect(ConfigString);
        }

        public IDatabase GetDb(int db)
        {
            //GetDatabase ile redis sunucumdaki hangi db ile çalışacağımı belirliyebilirim.
            return _redis.GetDatabase(db);
        }
    }
}
