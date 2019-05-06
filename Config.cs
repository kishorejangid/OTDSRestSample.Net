using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTDSRestSample
{
    public abstract class Config
    {
        public Config()
        {
            _protocol = Uri.UriSchemeHttp;
            _host = "localhost";
            _port = 80;
        }

        protected string _host;
        public string Host
        {
            get
            {
                return _host;
            }
        }

        protected string _protocol;
        public string Protocol
        {
            get
            {
                return _protocol;
            }
        }

        public string _path;
        public string Path
        {
            get
            {
                return _path;
            }
        }

        protected int _port;
        public int Port { get { return _port; } }

        public Uri GetUri()
        {
            var builder = new UriBuilder
            {
                Host = Host,
                Port = Port,
                Scheme = Protocol,
                Path = Path
            };
            return builder.Uri;
        }
    }

    public class OTCSConfig : Config
    {
        public OTCSConfig()
        {
            _host = "kksvr";
            _path = "/otcs167/cs.exe";
        }
    }

    public class OTDSConfig : Config
    {
        public OTDSConfig()
        {
            _host = "kksvr";
            _port = 8080;
            _path = "/otdsws/rest";
        }
    }
}
