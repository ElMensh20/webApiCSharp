using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using EmployeeDataAccess;

namespace EmployeeServies.Controllers
{

    [EnableCorsAttribute("*", "*", "*")]  //To enable cors in this controller
    public class EmployeeController : ApiController
    {
        private EmployeeDBEntities entities = new EmployeeDBEntities();
        //[HttpGet] // u can use it or set the prefix of GET
        [BasicAuthentication]
        public HttpResponseMessage Get()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            if(username==null || username == "")
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ourt username is empty");
            }
            switch (username.ToLower())
            {
                
                case "male":
                    return Request.CreateResponse(HttpStatusCode.OK,
                        entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                case "female":
                    return Request.CreateResponse(HttpStatusCode.OK,
                        entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ourt error msg is "+username);
            }
        }
        [HttpGet] // u can use it or set the prefix of GET
        public HttpResponseMessage LoadEmployeeById(int id)
        {
            var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
            if (entity != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Employee with id " + id.ToString() + " is not found");
            }
        }
        [HttpPost] // u can use it or set the prefix of POST
        public HttpResponseMessage SetEmployee([FromBody] Employee employee)
        {
            try{
                entities.Employees.Add(employee);
                entities.SaveChanges();
                var msg = Request.CreateResponse(HttpStatusCode.Created, employee);
                msg.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                return msg;
            }
            catch(Exception ex)
            {
               return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpDelete] // u can use it or set the prefix of DELETE
        public HttpResponseMessage RemoveEmployee (int id)
        {
            try
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    entities.Employees.Remove(entity);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with id " + id.ToString() + " is not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpPut] // u can use it or set the prefix of GET
        [DisableCors] //To disable cors
        public HttpResponseMessage UpdateEmployee([FromUri]int id,[FromBody] Employee employee)
        {
            try
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    entity.FirstName = employee.FirstName;
                    entity.LastName = employee.LastName;
                    entity.Salary = employee.Salary;
                    entity.Gender = employee.Gender;
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with id " + id.ToString() + " is not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
