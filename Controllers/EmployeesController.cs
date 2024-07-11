using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDbContext mVCDbContext;

        public EmployeesController(MVCDbContext mVCDbContext)

        {
           
                  this.mVCDbContext = mVCDbContext;
        }

        [HttpGet]
        public async Task <IActionResult> Index()
        {
          var employees = await mVCDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {

            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department,
            };

            await mVCDbContext.Employees.AddAsync(employee);
            await mVCDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        
        }

        [HttpGet]

        public async Task<IActionResult> View(Guid id) 
        {
           var employee = await mVCDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if(employee != null) 
            
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department,

                };

                return await Task.Run(() => View("View",viewModel));

            }


            return RedirectToAction("Index");
           
        }

        [HttpPost]

        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mVCDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name= model.Name;
                employee.Email= model.Email;    
                employee.Salary= model.Salary;
                employee.DateOfBirth= model.DateOfBirth;
                employee.Department= model.Department;

                await mVCDbContext.SaveChangesAsync();

               
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mVCDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                mVCDbContext.Employees.Remove(employee);
                await mVCDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
       



    }
}

