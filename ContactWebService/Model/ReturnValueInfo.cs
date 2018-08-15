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
    public class ReturnValueInfo<T>
    {
        [DataMember]
        public T ReturnValue { get; set; }

        private ErrorInfo _errorInfo = null;

        [DataMember]
        public ErrorInfo Result
        {
            get
            {
                if (_errorInfo == null)
                    _errorInfo = new ErrorInfo();
                return _errorInfo;
            }
            set
            {
                if (value == null)
                    _errorInfo = new ErrorInfo();
                else
                    _errorInfo = value;
            }
        }
    }

    [DataContract]
    public class ErrorInfo
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public ResultStatus Status { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

    }

    public enum ResultStatus
    {
        FAIL = 0,
        SUCCESS = 1,
        DUPLICATE = 2,
        NORECORDS = 3
    }
}