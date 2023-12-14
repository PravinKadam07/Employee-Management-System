using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mVCDemoDbContext;

        public EmployeesController(MVCDemoDbContext mVCDemoDbContext)
        {
            this.mVCDemoDbContext = mVCDemoDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department

            };
           await mVCDemoDbContext.Employees.AddAsync(employee);
          await  mVCDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
       var employee=  await   mVCDemoDbContext.Employees.ToListAsync();
            return View(employee);
        }

        [HttpGet]
        public async Task <IActionResult> View(Guid id)
        {
           var employee= await  mVCDemoDbContext.Employees.FirstOrDefaultAsync(x=> x.Id == id);

            if(employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department
                };
                return await Task.Run(()=> View("View",viewModel));
            }
           
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel Model)
        {
            var employee= await mVCDemoDbContext.Employees.FindAsync(Model.Id);
            if(employee != null)
            {
                employee.Name = Model.Name;
                employee.Email = Model.Email;
                employee.Salary = Model.Salary;     
                employee.DateOfBirth = Model.DateOfBirth;   
                employee.Department = Model.Department;

              await  mVCDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee= await mVCDemoDbContext.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                mVCDemoDbContext.Employees.Remove(employee);
                await mVCDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }

}
