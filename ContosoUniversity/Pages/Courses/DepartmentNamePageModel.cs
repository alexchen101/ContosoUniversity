using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ContosoUniversity.Pages.Courses
{
    public class DepartmentNamePageModel : PageModel
    {
        public SelectList DepartmentNameSL { get; set; }
        public void PopulateDepartmentDropdwonList(SchoolContext _context
            , object selectedDepartment=null)
        {
            var departmentQuery = from d in _context.Departments
                                  orderby d.Name
                                  select d;
            DepartmentNameSL = new SelectList(departmentQuery.AsNoTracking()
                , "DepartmentID", "Name", selectedDepartment);

        }
    }
}
