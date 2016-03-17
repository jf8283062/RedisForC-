/* 
 * 
 * 文 件 名：RedisFactory.cs
 * 建立时间：2015.12.31
 * 创 建 人：JiaoFeng(Tristan Jiao)
 * Email   : 1006200300@qq.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.Steelv.Lib.RedisCOM
{
    public class RedisFactory
    {        
        public static RedisDB CreateRDB()
        {
            return new RedisDB("redisDB");;
        }
    }
}
