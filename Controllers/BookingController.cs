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
    public class BookingController : Controller
    {
        public List<SelectListItem> Bind_Movie(int cat_id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            List<SelectListItem> list = new List<SelectListItem>();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Bind_Movie", connection);
            cmd.Parameters.AddWithValue("@Cat_id", cat_id);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SelectListItem { Value = reader["Movie_id"].ToString(), Text = reader["Movie_name"].ToString() + "\t\t|\t\tPrice : " + reader["Rate"].ToString() });
            }
            ViewBag.MovieList = list;
            return list;
        }

        public int Calculate_Price(int movie_id, int no_of_tickets)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Get_Rate", connection);
            cmd.Parameters.AddWithValue("@Movie_id", movie_id);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int price = 0;
            if (reader.Read())
            {
                price = Convert.ToInt32(reader["Rate"]);
            }
            int total_price = price * no_of_tickets;
            ViewBag.TotalPrice = total_price;
            return total_price;
        }
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


        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        // GET: Booking/Details/5
        public ActionResult Details()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Get_Booking", connection);
            cmd.CommandType = CommandType.StoredProcedure;
           List<Booking>booklist = new List<Booking>();
            connection.Open();
            cmd.Parameters.AddWithValue("@user_id",Get_User_id());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Booking booking = new Booking();
                booking.cat_name = reader["Cat_type"].ToString();
                booking.movie_name = reader["Movie_name"].ToString() ;
                booking.no_of_ticket = Convert.ToInt32(reader["No_of_Tickets"]);
                booking.amount = Convert.ToInt32(reader["amount"]);
                booklist.Add(booking);
            }
            reader.Close();
            connection.Close();
            return View(booklist);
        }

        // GET: Booking/Create
        public ActionResult Create(int? cat_id, int? movie_id, int? no_of_ticket)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            List<SelectListItem> list = new List<SelectListItem>();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Bind_Category", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SelectListItem { Value = reader["Cat_id"].ToString(), Text = reader["Cat_Type"].ToString() });
            }
            ViewBag.CategoryList = list;
            ViewBag.MovieList = new List<SelectListItem>();
            // Bind Movie according to selected Category
            if (cat_id != null)
            {
                ViewBag.MovieList = Bind_Movie(cat_id.Value);
            }
            else
            {
                ViewBag.MovieList = new List<SelectListItem>();
            }

            Booking book = new Booking();

            if (cat_id != null)
            {
                book.cat_id = cat_id.Value;
            }

            if (movie_id != null)
            {
                book.movie_id = movie_id.Value;
            }

            if (no_of_ticket != null)
            {
                book.no_of_ticket = no_of_ticket.Value;
            }

            if (movie_id != null && no_of_ticket != null)
            {
                book.amount = Calculate_Price(movie_id.Value, no_of_ticket.Value);
                ViewBag.TotalPrice = book.amount;
            }
            return View(book);
        }

            // POST: Booking/Create
            [HttpPost]
        public ActionResult Create(Booking book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand("Insert_Booking", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.Parameters.AddWithValue("@User_id", Get_User_id());
                    cmd.Parameters.AddWithValue("@Cat_id", book.cat_id);
                    cmd.Parameters.AddWithValue("@Movie_id", book.movie_id);
                    cmd.Parameters.AddWithValue("@no_of_Tickets", book.no_of_ticket);
                    cmd.Parameters.AddWithValue("@amount", book.amount);
                    int i = cmd.ExecuteNonQuery();
                    connection.Close();
                    if (i > 0)
                    {
                        ViewBag.Message = "Booking Insert Successfully";
                        return View(book);
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex + " Insertion Failed";
                return View(book);
            }
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Booking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Booking/Delete/5
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
