using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.Steelv.Lib.RedisCOM
{
    public class RedisProperty
    {
        public RedisProperty(string RedisDB)
        {
            _connectionString = SetConnectionString(RedisDB);
        }

        #region 常量

        /// <summary>
        /// 全局缓存配置服务器名称
        /// </summary>
        public const string SERVICE_NAME = "redisDB";

        #endregion

        #region 字段
        private string _connectionString;
        public string ConnectionString { get { return _connectionString; } }
        private static string _serverName;

        #endregion

        /// <summary>
        /// 服务器类型名称
        /// </summary>
        

        #region 配置信息
        private string _abortConnect;
        //当为true时，当没有可用的服务器时则不会创建一个连接
        public string AbortConnect { get { return _abortConnect; } }


        private string _allowAdmin;
        //当为true时 ，可以使用一些被认为危险的命令
        public string AllowAdmin { get { return _allowAdmin; } }


        private string _channelPrefix;
        //所有pub/sub渠道的前缀
        public string ChannelPrefix { get { return _channelPrefix; } }


        private string _connectRetry;
        //重试连接的次数
        public string ConnectRetry { get { return _connectRetry; } }

        //超时时间
        private string _connectTimeout;
        public string ConnectTimeout { get { return _abortConnect; } }

        //Broadcast channel name for communicating configuration changes
        private string _configChannel;
        public string ConfigChannel { get { return _configChannel; } }

        //保存x秒的活动连接
        private string _keepAlive;
        public string KeepAlive { get { return _keepAlive; } }

        //默认0到-1
        private string _defaultDatabase;
        public string DefaultDatabase { get { return _defaultDatabase; } }

        //ClientName
        private string _name;
        public string Name { get { return _name; } }


        private string _password;
        //password
        public string PassWord { get { return _password; } }


        private string _ssl;
        //使用sll加密
        public string Ssl { get { return _ssl; } }


        private string _sslHost;
        //强制服务器使用特定的ssl标识
        public string SslHost { get { return _sslHost; } }


        private string _syncTimeout;
        //异步超时时间
        public string SyncTimeout { get { return _syncTimeout; } }

        private string _writeBuffer;
        //输出缓存区的大小
        public string WriteBuffer { get { return _writeBuffer; } }

        #endregion


        /// <summary>
        /// 获取配置信息
        /// </summary>
        protected void GetSettings(string ConnKey)
        {
            _connectionString = SetConnectionString(ConnKey);
            foreach (string strData in _connectionString.Split(';'))
            {
                string[] strArgs = strData.Split('=');
                if (strArgs.Length < 2) continue;
                string strKey = strArgs[0];
                string strValue = strArgs[1];

                switch (strKey.ToLower())
                {
                    case "abortConnect":
                        _abortConnect = strValue;
                        break;
                    case "connectRetry":
                        _connectRetry = strValue;
                        break;
                    case "connectTimeout":
                        _connectTimeout = strValue;
                        break;
                    case "keepAlive":
                        _keepAlive = strValue;
                        break;
                    case "name":
                        _name = strValue;
                        break;
                    case "password":
                        _password = strValue;
                        break;

                    case "defaultDatabase":
                        _defaultDatabase = strValue;
                        break;

                    case "writeBuffer":
                        _writeBuffer = strValue;
                        break;
                }
            }
        }



        protected string SetConnectionString(string connKey)
        {
            var settings = System.Configuration.ConfigurationManager.AppSettings[connKey];
            if (string.IsNullOrEmpty(settings))
            {
                throw new Exception("未添加Redis服务器配置节点");
            }

            return settings;
        }
    }
}
