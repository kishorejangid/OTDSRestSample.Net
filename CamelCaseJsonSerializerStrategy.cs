using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTDSRestSample
{
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
