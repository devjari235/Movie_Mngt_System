using Movie_Mngt_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Mngt_System.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category cat)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string connectionstring = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                    SqlConnection con= new SqlConnection(connectionstring);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Insert_Cat", con);
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Cat_type", cat.cat_type);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ModelState.Clear();
                    ViewBag.Message = "Category inserted successfully!!";
                    return View(new Category());
                }
               

                return View(cat);
            }
            catch
            {
                ViewBag.Massage = "Error category successfully!!";
                return View();
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Category/Edit/5
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

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Category/Delete/5
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
