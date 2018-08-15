using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SignUpService.Model;
using SignUpService.Business;
using System.ServiceModel.Activation;
 

namespace SignUpService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ContactService : IContact 
    {
        /// <summary>
        /// Get Customer Detail  
        /// </summary>
        /// <param name="practInfo"></param>
        /// <returns></returns>
        public ReturnValueInfo<IList<CustomerInfo>> GetContactList(CustomerInfo custInfo)
        {
          
            ReturnValueInfo<IList<CustomerInfo>> returnValue = new ReturnValueInfo<IList<CustomerInfo>>();
            try
            {
                if (custInfo != null)
                {
                    SignUpPracticeBusiness practBusiness = new SignUpPracticeBusiness();
                    returnValue.ReturnValue = practBusiness.GetContactList(custInfo);
                    returnValue.Result.Status = ResultStatus.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                returnValue.Result.StackTrace = ex.StackTrace;
                returnValue.Result.Status = ResultStatus.FAIL;
                returnValue.Result.Type = ex.GetType().ToString();
                returnValue.Result.Description = ex.Message;
            }
            return returnValue;
        }

        /// <summary>
        /// Set Customer Detail  
        /// </summary>
        /// <param name="practInfo"></param>
        /// <returns></returns>
        public ReturnValueInfo<int> AddUpdateContactDetails(CustomerInfo custInfo)
        {
            ReturnValueInfo<int> returnValue = new ReturnValueInfo<int>();
            try
            {
                if (custInfo != null)
                {
                    SignUpPracticeBusiness practBusiness = new SignUpPracticeBusiness();
                    returnValue.ReturnValue = practBusiness.AddUpdateContactDetails(custInfo);
                    returnValue.Result.Status = ResultStatus.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                returnValue.Result.StackTrace = ex.StackTrace;
                returnValue.Result.Status = ResultStatus.FAIL;
                returnValue.Result.Type = ex.GetType().ToString();
                returnValue.Result.Description = ex.Message;
            }
            return returnValue;
        }
 
    }
}
