using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using api_project.Models;

namespace api_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        { 
            string query = @"select ProductID, ProductName from dbo.Product";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ProjectApp");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand=new SqlCommand(query,myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);

        }

        [HttpPost]
        public JsonResult Post(Product prd)
        {
            string query = @"
                insert into dbo.Product
                values (@ProductName)
                ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ProjectApp");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ProductName", prd.ProductName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);

        }
    }
}
