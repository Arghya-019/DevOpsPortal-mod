using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DevOpsPortal.Models;

namespace DevOpsPortal.Repository
{
    public class CustomerRepository
    {
        private SqlConnection con;
        #region To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            con = new SqlConnection(constr);

        }
        #endregion

        #region Add customer details
        public bool AddCustomer(CustomerDetails customerdetails)
        {
            connection();
            SqlCommand com = new SqlCommand("AddCustomerDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", customerdetails.Name);
            com.Parameters.AddWithValue("@Address", customerdetails.Address);
            com.Parameters.AddWithValue("@ContactNumber", customerdetails.ContactNumber);
            com.Parameters.AddWithValue("@AlternateContactNumber", customerdetails.AlternateContactNumber);
            com.Parameters.AddWithValue("@Specialty", customerdetails.Specialty);
            com.Parameters.AddWithValue("@QualificationSummary", customerdetails.QualificationSummary);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        #endregion

        #region View customer details 
        public List<CustomerDetails> GetCustomers()
        {
            connection();
            List<CustomerDetails> CustomerList = new List<CustomerDetails>();
            SqlCommand com = new SqlCommand("GetCustomerDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind CustomerDetails generic list using LINQ 
            CustomerList = (from DataRow dr in dt.Rows
                       select new CustomerDetails()
                       {
                           CustomerId = Convert.ToInt32(dr["CustomerId"]),
                           Name = Convert.ToString(dr["Name"]),
                           Address = Convert.ToString(dr["Address"]),
                           ContactNumber = Convert.ToString(dr["ContactNumber"]),
                           AlternateContactNumber = Convert.ToString(dr["AlternateContactNumber"]),
                           Specialty = Convert.ToString(dr["Specialty"]),
                           QualificationSummary = Convert.ToString(dr["QualificationSummary"])
                       }).ToList();

            return CustomerList;

        }
        #endregion

        #region Update customer details
        public bool UpdateCustomer(CustomerDetails customerdetails)
        {
            connection();
            SqlCommand com = new SqlCommand("UpdateCustomerDetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CustomerId", customerdetails.CustomerId);
            com.Parameters.AddWithValue("@Name", customerdetails.Name);
            com.Parameters.AddWithValue("@Address", customerdetails.Address);
            com.Parameters.AddWithValue("@ContactNumber", customerdetails.ContactNumber);
            com.Parameters.AddWithValue("@AlternateContactNumber", customerdetails.AlternateContactNumber);
            com.Parameters.AddWithValue("@Specialty", customerdetails.Specialty);
            com.Parameters.AddWithValue("@QualificationSummary", customerdetails.QualificationSummary);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        #endregion

        #region Delete Employee details
        public bool DeleteCustomer(int CustomerId)
        {

            connection();
            SqlCommand com = new SqlCommand("DeleteCustomerDetails", con);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CustomerId", CustomerId);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        #endregion

        #region Customer registration details
        public bool UserRegistration(LoginDetails LoginDetails)
        {
            connection();
            SqlCommand com = new SqlCommand("UserRegistrations", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Email", LoginDetails.Email);
            com.Parameters.AddWithValue("@Password", LoginDetails.Password);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        #endregion

        #region View customer details
        public LoginDetails GetUser(string email)
        {
            connection();
            LoginDetails user = new LoginDetails();
            SqlCommand com = new SqlCommand("GetLoginDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Email", email);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind CustomerDetails generic list using LINQ
            if (dt.Rows.Count > 0)
            {
                user = (from DataRow dr in dt.Rows
                        select new LoginDetails()
                          {
                              Email = Convert.ToString(dr["Email"]),
                              Password = Convert.ToString(dr["Password"])
                          }).ToList()[0];
            }

            return user;

        }
        #endregion
    }
}