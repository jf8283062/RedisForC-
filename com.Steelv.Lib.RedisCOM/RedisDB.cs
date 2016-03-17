using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.Steelv.Lib.RedisCOM
{
    public class RedisDB :RedisBase
    {
    
        public static Lazy<ConnectionMultiplexer> lazyConnection ;
        public RedisDB(string RedisDB)
            : base(RedisDB)
        {
            
            lazyConnection= new Lazy<ConnectionMultiplexer>(() =>
            {
            return ConnectionMultiplexer.Connect(ConnectionString);
            });
        }

        public ConnectionMultiplexer GetDBConnection()
        {
            return lazyConnection.Value;
        }

        /// <summary>
        /// 数据库的数字标识
        /// </summary>
        /// <returns></returns>
        public int DataBase()
        {
            var con = lazyConnection.Value;
            var x = con.GetDatabase().Database;
            con.Dispose();
            return x;
        }

        /// <summary>
        /// 查找Key是否有对应的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool KeyExists(string key, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                return con.GetDatabase().KeyExists(key, flags);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Delete(string key, CommandFlags flags = CommandFlags.None)
        {
            
            using (var con = lazyConnection.Value)
            {
                return con.GetDatabase().KeyDelete(key, flags);
            }
        }

        public IEnumerable<RedisKey> GetAllKeys()
        {
            IEnumerable<RedisKey> keys = null;
            using (var con = lazyConnection.Value)
            {
                  keys = con.GetServer(ConnectionString).Keys();
            }
            return keys;

        }

        #region string
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public RedisValue GetValue(string key, CommandFlags flags = CommandFlags.None)
        {
            using(var con = lazyConnection.Value)
            {
                return con.GetDatabase().StringGet(key, flags);
            }

        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T GetValue<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                byte[] data = con.GetDatabase().StringGet(key, flags);
                return Tools.DeserializeObject<T>(data);
            }
        }


        public T[] GetValue<T>(string[] keys, CommandFlags flags = CommandFlags.None)
        {
            RedisKey[] keysr = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length ; i++)
            {
                keysr[i] = keys[i] ;
            }
            T[]  x= new T[]{};
            using (var con = lazyConnection.Value)
            {
                
                RedisValue[] date = con.GetDatabase().StringGet(keysr, flags);
                for (int i = 0; i < date.Length; i++)
			    {
                   x[i] = Tools.DeserializeObject<T>(date[i]);

			    }
            }
            return x;
        }
     
        /// <summary>
        /// 存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool SetValue<T>(string key, T value,TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                var bValue = Tools.SerializeObject(value);
                return con.GetDatabase().StringSet(key, bValue, expiry, when, flags);
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool SetValue(string key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                return con.GetDatabase().StringSet(key, value, expiry, when, flags);
            }
        }


        public bool KeyRename(string oldKey, string newKey, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                return con.GetDatabase().KeyRename(oldKey,newKey,when,flags);
            }
        }
        #endregion 

        #region arry
        /// <summary>
        /// 向rides存放有序数组
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>存入个数</returns>
        public long AddArray(string key, params string[] values)
        {
            using (var con = lazyConnection.Value)
            {
                RedisValue[] x = new RedisValue[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    x[i] = values[i];
                }
                return con.GetDatabase().ListRightPush(key, x);
            }
        }

        /// <summary>
        /// 向rides存放有序数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public long AddArray<T>(string key, params T[] values)
        {
            using (var con = lazyConnection.Value)
            {
                RedisValue[] x = new RedisValue[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    x[i] = Tools.SerializeObject(values[i]);
                }
                return con.GetDatabase().ListRightPush(key, x);
            }
        }


        /// <summary>
        /// 获取存入的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T[] GetArray<T>(string key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            RedisValue[] data = null;
            using (var con = lazyConnection.Value)
            {
                data = con.GetDatabase().ListRange(key,start,stop,flags);
            }
            T[] rData = new T[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                rData[i] = Tools.DeserializeObject<T>(data[i]);
            }
            return rData;
        }
        /// <summary>
        /// 获取存入的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public string[] GetArray(string key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            RedisValue[] data = null;
            using (var con = lazyConnection.Value)
            {
                data = con.GetDatabase().ListRange(key, start, stop, flags);
            }
            string[] rData = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                rData[i] = data[i];
            }
            return rData;
        }
        /// <summary>
        /// 返回存储在键中的列表中的元素。该指数是零，所以0意味着第一个元素，1的第二元素等。负指数可以用来指定元素开始在列表的尾部。在这里，1意味着最后的元素，2是倒数第二等。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetArryByIndex(string key,int index,CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                return con.GetDatabase().ListGetByIndex(key, index, flags);
            }
        }

        /// <summary>
        /// 返回存储在键中的列表中的元素。该指数是零，所以0意味着第一个元素，1的第二元素等。负指数可以用来指定元素开始在列表的尾部。在这里，1意味着最后的元素，2是倒数第二等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T GetArryByIndex<T>(string key, int index, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                return Tools.DeserializeObject<T>(con.GetDatabase().ListGetByIndex(key, index, flags));
            }
        }
        /// <summary>
        /// 返回存储在键中的列表中的元素。该指数是零，所以0意味着第一个元素，1的第二元素等。负指数可以用来指定元素开始在列表的尾部。在这里，1意味着最后的元素，2是倒数第二等。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long ArrayRemove<T>(string key,T value ,long count = 0, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                var x = Tools.SerializeObject(value);
                return con.GetDatabase().ListRemove(key:key,value:x,count: count,flags:flags);
            }
        }

        #endregion

        #region hash
        /// <summary>
        /// 存放hash形式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <param name="flags"></param>
        public void SetHash<V,T>(string key,Dictionary<V,T> dic, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                HashEntry[] hashFields = new HashEntry[dic.Count];
                int i = 0;
                foreach (var item in dic)
                {
                    var skey =Tools.SerializeObject(item.Key);
                    var value = Tools.SerializeObject(item.Value);
                    hashFields[i] = new HashEntry(skey, value);
                    i++;
                }
                 con.GetDatabase().HashSet(key, hashFields, flags);
            }
        }

        /// <summary>
        /// 存放hash形式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool AddHash(string key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
               return con.GetDatabase().HashSet(key, hashField, value,when,flags);
            }
        }

        /// <summary>
        /// 存放hash形式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool AddHash<V, T>(string key, V hashField, T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            using (var con = lazyConnection.Value)
            {
                var hf = Tools.SerializeObject(hashField);
                var v = Tools.SerializeObject(value);
                return con.GetDatabase().HashSet(key, hf, v, when, flags);
            }
        }

        /// <summary>
        /// 获取rides,key对应的hash中hashField对应的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T GetHashValue<V, T>(string key, V hashField, CommandFlags flags = CommandFlags.None)
        {
            var con = lazyConnection.Value;
            var hkey = Tools.SerializeObject(hashField);
            byte[] data = con.GetDatabase().HashGet(key, hkey, flags);
            con.Dispose();
            var model = Tools.DeserializeObject(data);
            if (model is T)
                return (T)model;
            else
                return default(T);
        }
        /// <summary>
        /// 获取hash所有的value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Dictionary<string, V> GetHashAll<V>(string key, CommandFlags flags = CommandFlags.None)
        {
            HashEntry[] data = null;
            
            using (var con = lazyConnection.Value)
            {
                data = con.GetDatabase().HashGetAll(key, flags);
            }
            Dictionary<string, V> rdata = new Dictionary<string, V>();
            foreach (var itme in data)
			{
                rdata.Add(itme.Name.ToString(), Tools.DeserializeObject<V>(itme.Value));
			}
            return rdata;        
        }


        /// <summary>
        /// 获取hash所有的value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Dictionary<string, V> GetHashAll<V>(string key, string[] HashKey, CommandFlags flags = CommandFlags.None)
        {
            RedisValue[] data = null;
            RedisValue[] rv = new RedisValue[HashKey.Length];
            for (int i = 0; i < HashKey.Length; i++)
            {
                rv[i] = HashKey[i];
            }
            using (var con = lazyConnection.Value)
            {
                // RedisValue[] HashGet(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None);
                data = con.GetDatabase().HashGet(key, rv);
            }
            Dictionary<string, V> rdata = new Dictionary<string, V>();
            for(int i = 0; i < HashKey.Length; i++)
            {
                rdata.Add(HashKey[i], Tools.DeserializeObject < V > (data[i]));
            }
            return rdata;
        }

        #endregion

        #region set
        /// <summary>
        /// 存放SET数据类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public long AddSet(string key, params string[] values)
        {
            using (var con = lazyConnection.Value)
            {
                RedisValue[] x = new RedisValue[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    x[i] = values[i];
                }
                return con.GetDatabase().SetAdd(key, x);
            }
        }
        /// <summary>
        /// 存放SET数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public long AddSet<T>(string key, params T[] values)
        {
            using (var con = lazyConnection.Value)
            {
                RedisValue[] x = new RedisValue[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    x[i] = Tools.SerializeObject(values[i]);
                }
                return con.GetDatabase().SetAdd(key, x);
            }
        }


        public T[] GetSet<T>(string key,CommandFlags flags = CommandFlags.None)
        {
            RedisValue[] data = null;
            using (var con = lazyConnection.Value)
            {
                data = con.GetDatabase().SetMembers(key,flags);
            }
            T[] rData = new T[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                rData[i] = Tools.DeserializeObject<T>(data[i]);
            }
            return rData;
        }

        public string[] GetSet(string key, CommandFlags flags = CommandFlags.None)
        {
            RedisValue[] data = null;
            using (var con = lazyConnection.Value)
            {
                data = con.GetDatabase().SetMembers(key, flags);
            }
            string[] rData = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                rData[i] = data[i];
            }
            return rData;
        }
#endregion
    }
}
