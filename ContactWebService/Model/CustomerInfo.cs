using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;
using System.ServiceModel.Web;

namespace SignUpService.Model
{
    [DataContract]
    public class CustomerInfo
    {
        [DataMember]
        public string ContactDetailUID { get; set; }
        [DataMember]
        public string First { get; set; }
        [DataMember]
        public string Middle { get; set; }
        [DataMember]
        public string Last { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string PhoneNo { get; set; }
        [DataMember]
        public bool InactiveStatus { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public IList<CustomerInfo> CustomerInfoList { get; set; }

    }
}
