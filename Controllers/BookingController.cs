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
                booking.booking_id = Convert.ToInt32(reader["Booking_id"]);
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
        public ActionResult Edit(int id, int? cat_id, int? movie_id, int? no_of_ticket)
        {
            Booking book= new Booking();
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Get_Booking_ById", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Booking_id", id);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                book.booking_id = Convert.ToInt32(reader["Booking_id"]);
                book.user_id = Convert.ToInt32(reader["User_id"]);
                book.cat_id = Convert.ToInt32(reader["Cat_id"]);
                book.movie_id = Convert.ToInt32(reader["Movie_id"]);
                book.no_of_ticket = Convert.ToInt32(reader["No_of_Tickets"]);
                book.amount = Convert.ToInt32(reader["Amount"]);
            }

            reader.Close();
            connection.Close();

            List<SelectListItem> list = new List<SelectListItem>();

            connection = new SqlConnection(connectionString);
            cmd = new SqlCommand("Bind_Category", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = reader["Cat_id"].ToString(),
                    Text = reader["Cat_Type"].ToString()
                });
            }

            reader.Close();
            connection.Close();

            ViewBag.CategoryList = list;

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

            ViewBag.MovieList = new List<SelectListItem>();

            if (book.cat_id != 0)
            {
                ViewBag.MovieList = Bind_Movie(book.cat_id);
            }

            if (movie_id != null && no_of_ticket != null)
            {
                book.amount = Calculate_Price(movie_id.Value, no_of_ticket.Value);
                ViewBag.TotalPrice = book.amount;
            }
            else
            {
                ViewBag.TotalPrice = book.amount;
            }

            return View(book);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        public ActionResult Edit(Booking book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand("Update_Booking", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Booking_id", book.booking_id);
                    cmd.Parameters.AddWithValue("@User_id", Get_User_id());
                    cmd.Parameters.AddWithValue("@Cat_id", book.cat_id);
                    cmd.Parameters.AddWithValue("@Movie_id", book.movie_id);
                    cmd.Parameters.AddWithValue("@no_of_Tickets", book.no_of_ticket);
                    cmd.Parameters.AddWithValue("@amount", book.amount);
                    int i = cmd.ExecuteNonQuery();
                    connection.Close();
                    if (i > 0)
                    {
                        ViewBag.Message = "Booking Update Successfully";
                        return View(book);
                    }
                }

                return View(book);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex + " Updation Failed";
                return View(book);
            }
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int id)
        {
            Booking booking = new Booking();
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Get_Booking_ById", connection);
            cmd.CommandType = CommandType.StoredProcedure;
         
            connection.Open();
            cmd.Parameters.AddWithValue("@Booking_id",id);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
              
                booking.booking_id = Convert.ToInt32(reader["Booking_id"]);
                booking.user_id = Convert.ToInt32(reader["User_id"]);
                booking.cat_name = reader["Cat_type"].ToString();
                booking.movie_name = reader["Movie_name"].ToString();
                booking.no_of_ticket = Convert.ToInt32(reader["No_of_Tickets"]);
                booking.amount = Convert.ToInt32(reader["amount"]);
               
            }
            reader.Close();
            connection.Close();
            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Booking book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand("Delete_Booking", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Booking_id",id);
              
                    int i = cmd.ExecuteNonQuery();
                    connection.Close();
                    if (i > 0)
                    {
                        ViewBag.Message = "Delete Sucessfully";
                        return View(book);
                    }
                }

                return View(book);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex + " Delete Failed";
                return View(book);
            }
        }
    }
}
