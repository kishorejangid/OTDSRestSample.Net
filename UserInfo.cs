using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTDSRestSample
{
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
}
