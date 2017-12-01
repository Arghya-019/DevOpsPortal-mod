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

    public interface ICustomerRepositoryMoq
    {
        bool AddCustomer(Models.CustomerDetails customerdetails);
        List<Models.CustomerDetails> GetCustomers();

    }

    public class CustomerRepositoryMoq : ICustomerRepositoryMoq
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
        public bool AddCustomer(Models.CustomerDetails customerdetails)
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
        public List<Models.CustomerDetails> GetCustomers()
        {
            connection();
            List<Models.CustomerDetails> CustomerList = new List<Models.CustomerDetails>();
            SqlCommand com = new SqlCommand("GetCustomerDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            //Bind CustomerDetails generic list using LINQ 
            CustomerList = (from DataRow dr in dt.Rows
                            select new Models.CustomerDetails()
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

    }

    public class CustomerService
    {
        private ICustomerRepositoryMoq _repository;

        public CustomerService(ICustomerRepositoryMoq repository)
        {
            _repository = repository;
        }

        public bool AddCustomer(Models.CustomerDetails customerdetails)
        {
            return _repository.AddCustomer(customerdetails);
        }

        public List<Models.CustomerDetails> GetCustomers()
        {
            return _repository.GetCustomers();
        }
    }
}