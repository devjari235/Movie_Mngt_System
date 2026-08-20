using Movie_Mngt_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Mngt_System.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Index()
        {
            return View();
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            SqlConnection con=new SqlConnection(connectionstring);
            List<SelectListItem>  list = new List<SelectListItem>();
            SqlCommand cmd = new SqlCommand("Bind_Category", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SelectListItem
                {
                    Value = reader["Cat_id"].ToString(),
                    Text = reader["Cat_type"].ToString(),
                });
            }
            con.Close();
            ViewBag.CategoryList=list;
            return View(new Movie());
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie mov)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    string connectionstring = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                    SqlConnection con = new SqlConnection(connectionstring);
                    SqlCommand cmd = new SqlCommand("Insert_Movie", con);
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Movie_name", mov.movie_name);
                    cmd.Parameters.AddWithValue("@Release_date", mov.relese_date);
                    cmd.Parameters.AddWithValue("@Cat_id", mov.cat_id);
                    cmd.Parameters.AddWithValue("@Rate", mov.rate);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ModelState.Clear();
                    ViewBag.Message = "Movie Added Successfully!!";
                    return View(new Movie());
                }

                return View(mov);
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex + ": Error";
                return View(mov);
            }
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Movie/Edit/5
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

        // GET: Movie/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Movie/Delete/5
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
