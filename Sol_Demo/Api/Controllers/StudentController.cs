using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet("list")]
        //[HttpPost("list")]
        [EnableQuery]
        public IActionResult StudentList()
        {
            var studentList = new List<StudentModel>();
            studentList.Add(new StudentModel()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Sharmila Naik",
                Score = 98
            });
            studentList.Add(new StudentModel()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Mahesh Naik",
                Score = 90
            });

            studentList.Add(new StudentModel()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = "Kishor Naik",
                Score = 70
            });

            return base.Ok(studentList);
        }
    }
}