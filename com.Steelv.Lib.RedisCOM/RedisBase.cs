using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.Steelv.Lib.RedisCOM
{
    public abstract class RedisBase
    {

        public static string ConnectionString;
        public RedisBase(string RedisDB)
        {
            if (string.IsNullOrWhiteSpace(ConnectionString) && !string.IsNullOrWhiteSpace(RedisDB))
            {
                ConnectionString = new RedisProperty(RedisDB).ConnectionString;
            }
        }

    }
}
