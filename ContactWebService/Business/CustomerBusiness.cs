using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SignUpService.Model;
using SignUpService.DataAccess;
using System.Net.Mail;


namespace SignUpService.Business
{
    public class SignUpPracticeBusiness
    {

        public IList<CustomerInfo> GetContactList(CustomerInfo sInfo)
        {
             return new CustomerDA().GetContactList(sInfo);
        }

        public int AddUpdateContactDetails(CustomerInfo custInfo)
        {
            return new CustomerDA().AddUpdateContactDetails(custInfo);
            
        }
    }
}
