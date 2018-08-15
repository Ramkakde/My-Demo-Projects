using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SignUpService.Model;
using System.Data;


namespace SignUpService.DataAccess
{
    public class CustomerDA
    {
        // DB transactions.
        /// <summary>
        /// Create  Customer Detail in database tranSaction
        /// </summary>
        /// <param name="practInfo"></param>
        /// <returns></returns>
        private int SetContactDetailDBTranSaction(CustomerInfo practInfo)
        {
            DataTable returnValue = new DataTable(); 
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Usp_AddUpdateContactDetails";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(practInfo.ContactDetailUID == "" ? new SqlParameter("@ContactDetailUID", DBNull.Value) : new SqlParameter("@ContactDetailUID", practInfo.ContactDetailUID));
            cmd.Parameters.Add(practInfo.First == "" ? new SqlParameter("@First", DBNull.Value) : new SqlParameter("@First", practInfo.First));
            cmd.Parameters.Add(practInfo.Middle == "" ? new SqlParameter("@Middle", DBNull.Value) : new SqlParameter("@Middle", practInfo.Middle));
            cmd.Parameters.Add(practInfo.Last == "" ? new SqlParameter("@Last", DBNull.Value) : new SqlParameter("@Last", practInfo.Last));
            cmd.Parameters.Add(practInfo.Email == "" ? new SqlParameter("@Email", DBNull.Value) : new SqlParameter("@Email", practInfo.Email));
            cmd.Parameters.Add(practInfo.PhoneNo == "" ? new SqlParameter("@PhoneNo", DBNull.Value) : new SqlParameter("@PhoneNo", practInfo.PhoneNo));
            cmd.Parameters.Add(practInfo.InactiveStatus == false ? new SqlParameter("@InactiveStatus", DBNull.Value) : new SqlParameter("@InactiveStatus", practInfo.InactiveStatus));
            DBHelper db = new DBHelper("myConnectionString");
            return db.ExecuteNonQuery(cmd);
             
        }

        public DataTable GetCustomerListDBTranSaction(CustomerInfo practInfo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Usp_GetContactDetails";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(practInfo.ContactDetailUID == "" ? new SqlParameter("@ContactDetailUID", DBNull.Value) : new SqlParameter("@ContactDetailUID", practInfo.ContactDetailUID));
            cmd.Parameters.Add(practInfo.First == "" ? new SqlParameter("@First", DBNull.Value) : new SqlParameter("@First", practInfo.First));
            cmd.Parameters.Add(practInfo.Middle == "" ? new SqlParameter("@Middle", DBNull.Value) : new SqlParameter("@Middle", practInfo.Middle));
            cmd.Parameters.Add(practInfo.Last == "" ? new SqlParameter("@Last", DBNull.Value) : new SqlParameter("@Last", practInfo.Last));
            cmd.Parameters.Add(practInfo.Email == "" ? new SqlParameter("@Email", DBNull.Value) : new SqlParameter("@Email", practInfo.Email));
            cmd.Parameters.Add(practInfo.PhoneNo == "" ? new SqlParameter("@PhoneNo", DBNull.Value) : new SqlParameter("@PhoneNo", practInfo.PhoneNo));
            cmd.Parameters.Add(practInfo.InactiveStatus == false ? new SqlParameter("@InactiveStatus", DBNull.Value) : new SqlParameter("@InactiveStatus", practInfo.InactiveStatus));
            DBHelper db = new DBHelper("myConnectionString");
            return db.GetDataTable(cmd);
        }

        public IList<CustomerInfo> GetContactList(CustomerInfo practInfo)
        {
            IList<CustomerInfo> returnValue = new List<CustomerInfo>();
            using (DataTable CustDataTable = GetCustomerListDBTranSaction(practInfo))
            {
                if (CustDataTable.Rows.Count > 0)
                {
                    foreach (DataRow rowitem in CustDataTable.Rows)
                    {
                        CustomerInfo tempCust = new CustomerInfo();
                        tempCust.ContactDetailUID = rowitem["ContactDetailUID"].ToString(); 
                        tempCust.First = rowitem["First"].ToString(); 
                        tempCust.Middle = rowitem["Middle"].ToString();
                        tempCust.Last = rowitem["Last"].ToString();
                        tempCust.Email = rowitem["Email"].ToString();
                        tempCust.PhoneNo = rowitem["PhoneNo"].ToString();
                        tempCust.InactiveStatus = Convert.ToBoolean(rowitem["InactiveStatus"].ToString());
                        returnValue.Add(tempCust);
                    }

                }
            }
            return returnValue;
        }

        public int AddUpdateContactDetails(CustomerInfo custInfo)
        {
             return SetContactDetailDBTranSaction(custInfo);
             
        }
        
 
    } 
}
