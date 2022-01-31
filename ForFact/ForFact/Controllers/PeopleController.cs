using Dapper;
using ForFact.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ForFact.Controllers
{
    public class PeopleController : Controller
    {
        private readonly string ConnectionString = "Host=localhost;Port=5432;Database=CrudDb;Username=postgres;Password=12345;Maximum Pool Size=100";

        public IActionResult Index()
        {
            IDbConnection con;

            try
            {
                string selectQuery = "SELECT * FROM people";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                IEnumerable<People> listPeople = con.Query<People>(selectQuery).ToList();
                return View(listPeople);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create (People people)
        {
            if(ModelState.IsValid)
            {
                IDbConnection con;

                try
                {
                    string insertQuery = "INSERT INTO people( name, surname, email) VALUES(@Name, @Surname, @Email)";
                    con = new NpgsqlConnection(ConnectionString);
                    con.Open();
                    con.Execute(insertQuery, people);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex )
                {
                   throw ex;
                }
            }
            return View(people);
        }

        [HttpGet]
        public IActionResult Edit (int id)
        {
            IDbConnection con;

            try
            {
                string selectQuery = "SELECT * FROM people WHERE Id = @id";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                People people = con.Query<People>(selectQuery, new { id = id }).FirstOrDefault();
                con.Close();

                return View(people);
            }
            catch (Exception ex)
            {;
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Edit (int id, People people)
        {
            if (id != people.Id) 
                return NotFound(); 
           

            if(ModelState.IsValid)
            {
                IDbConnection con;

                try
                {
                    con = new NpgsqlConnection(ConnectionString);
                    string updateQuery = "UPDATE people SET Name = @name, Surname =@surname, Email = @email WHERE Id = @id";
                    con.Open();
                    con.Execute(updateQuery, people);
                    con.Close();
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View(people);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            IDbConnection con;

            try
            {
                string deleteQuery = "DELETE FROM people WHERE Id = @id";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                con.Execute(deleteQuery, new { id = id });
                con.Close();
                return RedirectToAction(nameof(Index));
                
            }
            catch (Exception ex)
            {
                throw ex;
                throw;
            }
        }
    }
}
