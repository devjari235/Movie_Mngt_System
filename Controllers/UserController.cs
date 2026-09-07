using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Movie_Mngt_System.Models;

namespace Movie_Mngt_System.Controllers
{
    public class UserController : Controller
    {

        public int Get_User_id()
        {
            int id = 0;
            if (Session["Email"] != null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("Get_User", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmd.Parameters.AddWithValue("@Email_id", Session["Email"]);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = Convert.ToInt32(reader["User_id"]);
                }

            }
            return id;
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit()
        {
            User use = new User();
            if (Session["Email"] != null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("Get_User", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                cmd.Parameters.AddWithValue("@Email_id", Session["Email"]);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    use.user_id = Convert.ToInt32(reader["User_id"]);
                    use.user_name = reader["User_name"].ToString();
                    use.email_id = reader["Email_id"].ToString();
                    use.user_password = reader["User_password"].ToString();
                    use.city = reader["City"].ToString();
                    use.phone_number = reader["PhoneNo"].ToString();
                }
                connection.Close();
            }
            return View(use);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(User use)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand("Update_User", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    cmd.Parameters.AddWithValue("@User_id", Get_User_id());
                    cmd.Parameters.AddWithValue("@User_name", use.user_name);
                    cmd.Parameters.AddWithValue("@Email_id", use.email_id);
                    cmd.Parameters.AddWithValue("@User_password", use.user_password);
                    cmd.Parameters.AddWithValue("@City", use.city);
                    cmd.Parameters.AddWithValue("@PhoneNo", use.phone_number);
                    cmd.ExecuteNonQuery();
                    ViewBag.Message = "Update Successfully!";
                    connection.Close();
                }
                return View(use);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex + " Error to Update profile";
                return View(use);
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
