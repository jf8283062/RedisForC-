using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.Steelv.Lib.RedisCOM
{
    public interface IRedisDB
    {
         string GetValue(string key);

         int Database { get; }

         T HashGet<T>(string key, string hashField, CommandFlags flags = CommandFlags.None);
    }
}
