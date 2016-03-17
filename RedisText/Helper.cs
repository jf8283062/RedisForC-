using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisText
{
    public class Helper
    {
        private static string constr = System.Configuration.ConfigurationManager.AppSettings["redis"];

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(constr);
        });

        public static ConnectionMultiplexer redis
        {
            get
            {
                return lazyConnection.Value;
            }
        }


    }
}
