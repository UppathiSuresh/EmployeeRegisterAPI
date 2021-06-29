//using EmployeeRegisterAPI.Models;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace EmployeeRegisterAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeeController : ControllerBase
//    {
//        private readonly EmployeeDbContext _employeeDbContext;
//        private readonly IWebHostEnvironment _hostEnvironment;

//        public EmployeeController(EmployeeDbContext employeeDbContext, IWebHostEnvironment hostEnvironment)
//        {
//            _employeeDbContext = employeeDbContext;
//            this._hostEnvironment = hostEnvironment;
//        }
//        // GET: api/<EmployeeController>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<EmployeeModel>>> GetEmployees()
//        {
//            try
//            {
//            return await _employeeDbContext.Employees.ToListAsync();

//            }
//            catch (Exception)
//            {
//                return NotFound("Get Employees");
//            }
//            //return await _employeeDbContext.Employees
//            //    .Select(x => new EmployeeModel()
//            //    {
//            //        EmployeeID=x.EmployeeID,
//            //        EmployeeName=x.EmployeeName,
//            //        Occupation=x.Occupation,
//            //        ImageName=x.ImageName,
//            //        ImageSrc=string.Format("{0}://{1}{2}/Images/{3}",Request.Scheme,Request.Host,Request.PathBase,x.ImageName)
//            //    })
//            //    .ToListAsync();
//            //return new string[] { "value1", "value2" };
//        }

//        // GET api/<EmployeeController>/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<EmployeeModel>> GetEmployeeModel(int id)
//        {
//            var employeeModel = await _employeeDbContext.Employees.FindAsync(id);
//            if (employeeModel == null)
//            {
//                return NotFound();
//            }
//            return employeeModel;
//        }
//        // POST api/<EmployeeController>
//        [HttpPost]
//        public async Task<ActionResult<EmployeeModel>> PostEmployeeModel([FromForm] EmployeeModel employeeModel)
//        {
//            try
//            {
//                employeeModel.ImageName = await SaveImage(employeeModel.ImageFile);
//                _employeeDbContext.Employees.Add(employeeModel);
//                await _employeeDbContext.SaveChangesAsync();

//                return StatusCode(201);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(202);
//            }

//            //return CreatedAtAction("GetEmployeeModel",new {id=employeeModel.EmployeeID },employeeModel);
//        }

//        // PUT api/<EmployeeController>/5
//        [HttpPut("{id}")]
//        public async Task<ActionResult<EmployeeModel>> PutEmployeeModel(int id, EmployeeModel employeeModel)
//        {
//            try
//            {
//                if (id != employeeModel.EmployeeID)
//                {
//                    return BadRequest();
//                }
//                _employeeDbContext.Entry(employeeModel).State = EntityState.Modified;

//                await _employeeDbContext.SaveChangesAsync();

//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!EmployeeModelExists(id))
//                    return NotFound();
//                else
//                    throw;
//            }
//            return NoContent();
//        }

//        // DELETE api/<EmployeeController>/5
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<EmployeeModel>> DeleteEmployeeModel(int id)
//        {
//            var employModel = await _employeeDbContext.Employees.FindAsync(id);
//            if (employModel == null)
//            {
//                return NotFound();
//            }
//            _employeeDbContext.Employees.Remove(employModel);
//            await _employeeDbContext.SaveChangesAsync();

//            return employModel;
//        }
//        private bool EmployeeModelExists(int id)
//        {
//            return _employeeDbContext.Employees.Any(e => e.EmployeeID == id);
//        }

//        [NonAction]
//        public async Task<string> SaveImage(IFormFile imageFile)
//        {
//            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
//            imageName = imageName + DateTime.Now.ToString("yymmddss") + Path.GetFileNameWithoutExtension(imageFile.FileName);
//            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);

//            using (var fileStream = new FileStream(imagePath, FileMode.Create))
//            {
//                await imageFile.CopyToAsync(fileStream);
//            }

//            return imageName;

//        }
//    }
//}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeRegisterAPI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EmployeeRegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EmployeeController(EmployeeDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeModel>>> GetEmployees()
        {
            return await _context.Employees
                .Select(x => new EmployeeModel()
                {
                    EmployeeID = x.EmployeeID,
                    EmployeeName = x.EmployeeName,
                    Occupation = x.Occupation,
                    ImageName = x.ImageName,
                    ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageName)
                })
                .ToListAsync();
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeModel>> GetEmployeeModel(int id)
        {
            var employeeModel = await _context.Employees.FindAsync(id);

            if (employeeModel == null)
            {
                return NotFound();
            }

            return employeeModel;
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeModel(int id, [FromForm] EmployeeModel employeeModel)
        {
            if (id != employeeModel.EmployeeID)
            {
                return BadRequest();
            }

            if (employeeModel.ImageFile != null)
            {
                DeleteImage(employeeModel.ImageName);
                employeeModel.ImageName = await SaveImage(employeeModel.ImageFile);
            }

            _context.Entry(employeeModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employee
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EmployeeModel>> PostEmployeeModel([FromForm] EmployeeModel employeeModel)
        {
            employeeModel.ImageName = await SaveImage(employeeModel.ImageFile);
            _context.Employees.Add(employeeModel);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeModel>> DeleteEmployeeModel(int id)
        {
            var employeeModel = await _context.Employees.FindAsync(id);
            if (employeeModel == null)
            {
                return NotFound();
            }
            DeleteImage(employeeModel.ImageName);
            _context.Employees.Remove(employeeModel);
            await _context.SaveChangesAsync();

            return employeeModel;
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}