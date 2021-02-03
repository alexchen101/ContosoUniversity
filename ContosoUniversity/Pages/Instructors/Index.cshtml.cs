using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ContosoUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        readonly ILogger<IndexModel> _logger;

        public IndexModel(ContosoUniversity.Data.SchoolContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public InstructorIndexData InstructorIndexData { get;set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }


        public async Task OnGetAsync(int? id, int? courseID)
        {
            _logger.LogInformation($"ID:{id}, CourseID:{courseID}");

            InstructorIndexData = new InstructorIndexData();

            InstructorIndexData.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                        .ThenInclude(i => i.Department)
                //.Include(i => i.CourseAssignments) //预先加载改为显示加载
                //    .ThenInclude(i => i.Course)
                //        .ThenInclude(i => i.Enrollments)
                //            .ThenInclude(i => i.Student)
                //.AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();

            if(id !=null)
            {
                InstructorID = id.Value;
                Instructor instructor =
                InstructorIndexData.Instructors.Single(i => i.ID == InstructorID);
                InstructorIndexData.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if(courseID != null)
            {
                CourseID = courseID.Value;
                var selectedCourse =
                InstructorIndexData.Courses.Single(c => c.CourseID == CourseID);
                //显示加载
                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach (var item in selectedCourse.Enrollments)
                {
                    await _context.Entry(item).Reference(x => x.Student).LoadAsync();
                }
                InstructorIndexData.Enrollments = selectedCourse.Enrollments;
            }
            
        }
    }
}
