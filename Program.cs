using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serialization.Json;

namespace OTDSRestSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Since OTCS Rest API doesn't support token generation, get it from Content Server Rest API
            //If you need to handle authentication in OTDS, please use OAuth            
            String token = GetToken();
            Console.WriteLine(token);

            OTDSRestClient client = new OTDSRestClient(token);
            //client.CreateUser();
            //client.CreateGroup();
            
            client.AddUserToGroups("kisho", new List<string> {
                "cn=Business Administrators,ou=groups,ou=Root,ou=Content Server Members,ou=IdentityProviders,dc=identity,dc=opentext,dc=net",
                "cn=Developers,ou=Root,ou=Content Server Members,ou=IdentityProviders,dc=identity,dc=opentext,dc=net"
            });
            Console.ReadLine();
        }

        public static string GetToken()
        {
            var client = new RestClient(new OTCSConfig().GetUri());
            var request = new RestRequest("/api/v1/auth", Method.POST);
            request.AddParameter("userName", "Admin");
            request.AddParameter("password", "livelink");
            IRestResponse<OTDSTicket> response = client.Execute<OTDSTicket>(request);
            return response.Data.Ticket;
        }
    }

    public class OTDSTicket
    {
        public string Ticket { get; set; }
    }

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

    public class OTDSRestClient
    {
        private string _token = null;
        private Config _config = new OTDSConfig();
        private RestClient _restClient;
        public OTDSRestClient(string token)
        {
            _token = token;
            _restClient = new RestClient(_config.GetUri());
            SimpleJson.CurrentJsonSerializerStrategy = new CamelCaseJsonSerializerStrategy();            
            
            //Need work here to get the actual token or perform OAuth
            _token = "*OTDSSSO*AXhBQlNFaGg1NnNqbTFFRlZ6aUpoVU43MXNhSGhQZWdBUVV5dXphVTh5bV9RMmhNbF9hNE5Ua2dEd05VWDlLcXRaRHltU2ZJYlo5Vk9iNEx4VUhvQ3RYdnJWcWJyTzJWYl9GclhXM0phSF9xRUtSWjRiSmN3NFVFbTM4bVZQTE1kMGh2Sk5oZmFyRkxyNVV0WWRuOEFYbS1HWE94VHU0Y1BHRjQtSWdKTS1TdjZTT3ZldWdDVnpaWmZjVTZwZUFISVpSTEx3QkdGZFZqTW5vZ2J5LVdNVENMWnduVjU2VFJLczJaMEpDMHRoNW1iN3hxOU00NFVMUUZvUlA5WC1VRktIRmNfZkx2ci16b1VhdDA5WXc1WTk3VGpMNEJ4MU1qakJaSlhmVDFTLXZING9WY0FXNHRURlR3dDNieVBmSGJCY2lwOHJtMUpiRFVEVUh0clBCNVpCRlVNVWJVUGs3WF9paTgzV0U4MWc0bURFcWlUUXg3MDUyLW9SAE4ASgAUjawDj6OZD-Tf2lJ0vBoT098djlEAEGas47CjYcrluF_D0RjrfsoAIAgDQkCpCtBHknp-JlPd3lquRsTwUqv5dy2lbgGx43NGAAA*";

        }

        public void CreateUser()
        {            
            var request = new RestRequest("/users", Method.POST);
            request.AddHeader("OTDSTicket", _token);
            var createUserRequest = new UserInfo
            {
                UserPartitionID = "Content Server Members",
                Name = "Sample User",
                Location = "ou=Root,ou=Content Server Members,ou=IdentityProviders,dc=identity,dc=opentext,dc=net",                
                Values = new List<Value> {
                    new Value{
                        Name = "mail",
                        Values = new List<string> {
                            "kishore@dignaj.com"
                        }
                    },
                    new Value{
                        Name = "oTCompany",
                        Values = new List<string>{
                            "dignaj.com"
                        }
                    },
                    new Value{
                        Name = "userPassword",
                        Values = new List<string>{
                            "Livelink@123"
                        }
                    }
                    //Similarly add all other properties like Department
                }
            };
            request.AddJsonBody(createUserRequest);
            Console.WriteLine("Request:");
            IRestResponse response = _restClient.Execute(request);
            IDeserializer deserializer = new JsonDeserializer();
            if (response.IsSuccessful)
            {
                UserInfo userInfo = deserializer.Deserialize<UserInfo>(response);
            }
            else
            {
                ErrorResponse error = deserializer.Deserialize<ErrorResponse>(response);
            }
            Console.WriteLine("Response:");
            Console.WriteLine(response.Content);
        }

        //Return type should be generic
        public UserInfo CreateGroup()
        {            
            var request = new RestRequest("/groups", Method.POST);
            request.AddHeader("OTDSTicket", _token);
            var createGroupRequest = new UserInfo
            {
                UserPartitionID = "Content Server Members",
                Name = "Raja Group",
                Location = "ou=Root,ou=Content Server Members,ou=IdentityProviders,dc=identity,dc=opentext,dc=net",
                Values = new List<Value> {
                    new Value{
                        Name = "displayName",
                        Values = new List<string> {
                            "Group Display Name"
                        }
                    }                                        
                }
            };
            request.AddJsonBody(createGroupRequest);            
            IRestResponse response = _restClient.Execute(request);
            IDeserializer deserializer = new JsonDeserializer();
            if (response.IsSuccessful)
            {
                UserInfo userInfo = deserializer.Deserialize<UserInfo>(response);
                return userInfo;
            }
            else
            {
                ErrorResponse error = deserializer.Deserialize<ErrorResponse>(response);
            }
            Console.WriteLine("Response:");
            Console.WriteLine(response.Content);
            return null;
        }

        public void AddUserToGroups(string user, List<string> groups) {            
            //In future, create a custom attribute for Serializing something like [FieldName="string_list"]            
            var request = new RestRequest("/users/{user_id}/memberof", Method.POST);
            request.AddHeader("OTDSTicket", _token);
            request.AddUrlSegment("user_id", user);            
            request.AddJsonBody(new {
                StringList = groups
            });
            IRestResponse response = _restClient.Execute(request);            
            Console.WriteLine("Response:");
            Console.WriteLine(response.Content);            
        }
    }

    public class ErrorResponse
    {
        public int Status { get; set; }
        public string Error { get; set; }
        public string ErrorDetails { get; set; }
    }

    /// <summary>
    /// Represent the User JSON returned from Rest API, need to implement all other properties     
    /// </summary>
    public class UserInfo
    {
        public string UserPartitionID { get; set; }
        public string Name { get; set; }

        public string Location { get; set; }

        public List<Value> Values { get; set; }
    }

    public class Value
    {
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }

    public class CamelCaseJsonSerializerStrategy : PocoJsonSerializerStrategy
    {
        protected override string MapClrMemberNameToJsonFieldName(string clrPropertyName)
        {            
            if (clrPropertyName.Count() >= 2)
            {
                return char.ToLower(clrPropertyName[0]).ToString() + clrPropertyName.Substring(1);
            }
            else
            {
                return clrPropertyName.ToLower();
            }
        }
    }
}
