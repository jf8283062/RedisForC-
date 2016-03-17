using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using com.Steelv.Lib.RedisCOM;
namespace RedisText
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConnectionMultiplexer redis = Helper.redis;
            //IDatabase redisdb = Helper.redis.GetDatabase();
            //var a = new A() { Name = "jiaofeng", Age = 12 };
            //var by = SerializeObject(a);
            //redisdb.StringSet("1","dasfdsfdsf");
            //var x = redisdb.StringGet("1");
            //RedisValue p = RedisValue.Null;
            //redisdb.StringSet("sa", by);
            //var come = redisdb.StringGet("sa");
            //redis.Dispose();
            //var de = DeserializeObject(come);
            //A xxx = de as A;
            //Console.WriteLine(xxx.Name);
            string arrkey = "arrkeys";

            RedisFactory.CreateRDB().Delete(arrkey);
            //  RedisFactory.CreateRDB().get(arrkey);
            //List<A> arrA = new List<A>();
            //for (int i = 0; i < 100;i++ )
            //{
            //    arrA.Add(new A() { Name = i.ToString(), Age = i });

            //}
            //RedisFactory.CreateRDB().AddListArray<A>(arrkey, arrA.ToArray());
            //Console.WriteLine("==========存入Rides==========" );


             RedisFactory.CreateRDB().SetValue<A>("key_A", new A() { Age = 27, Name = "jiaofeng" });
             A x = RedisFactory.CreateRDB().GetValue<A>("key_A");

            Console.WriteLine("-------------" + x.Name + "-------------");
            Console.WriteLine("-------------" + x.Age + "-------------");







            Console.WriteLine("==========Rides取数组第一个 成功==========");

            //var arrData = RedisFactory.CreateRDB().GetSet<A>(arrkey);
            //foreach (var item in arrData)
            //{
            //    Console.WriteLine("-------------" + item.Name + "-------------");
            //}
            //Console.WriteLine("==========Rides取数组全部 成功==========");

            Dictionary<int, A> dic = new Dictionary<int, A>();
             for (int i = 0; i < 10;i++ )
            {
                dic.Add(i,new A() { Name = i.ToString(), Age = i });

            }
            string dicKey = "dickey";
            RedisFactory.CreateRDB().Delete(dicKey);
            RedisFactory.CreateRDB().SetHash<int, A>(dicKey, dic);
            //RedisFactory.CreateRDB().AddHash<string, A>(dicKey, "AA", new A() { Age = 00, Name = "BB" });


            //RedisFactory.CreateRDB().GetHashAll<A>(dicKey, "AA", new A() { Age = 00, Name = "BB" });

            var modela = RedisFactory.CreateRDB().GetHashValue<int, A>(dicKey, 1);
            Console.WriteLine("-------------" + modela.Name + "-------------");


            var dicData = RedisFactory.CreateRDB().GetHashAll<A>(dicKey);

            foreach (var item in dicData)
            {
                Console.WriteLine("----key:" + item.Key + "----  value:"+item.Value.Name+"---------");

            }
            Console.WriteLine("从rides取出数据");
            
            
            Console.ReadKey();


        }

        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
                return null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            byte[] bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);
            ms.Close();
            return bytes;
        }

        /// <summary>  
        /// 把字节数组反序列化成对象  
        /// </summary>  
        public static object DeserializeObject(byte[] bytes)
        {
            object obj = null;
            if (bytes == null)
                return obj;
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            obj = formatter.Deserialize(ms);
            ms.Close();
            return obj;
        }  
    }



    [Serializable]
    public class A {
        public string Name { set; get; }
        public int Age { set; get; }
    }



}
