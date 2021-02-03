using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Instructors
{
    public class InstructorCoursesPageModel : PageModel
    {
        public List<AssignedCourseData> AssignedCourseDataList { get; set; }

        public void PopulateAssignedCourseData(SchoolContext _context
            , Instructor instructor)
        {
            var allCourses = _context.Courses;
            var instructorCourses = new HashSet<int>(
                instructor.CourseAssignments.Select(s => s.CourseID));
            AssignedCourseDataList = new List<AssignedCourseData>();

            foreach (var item in allCourses)
            {
                AssignedCourseDataList.Add(new AssignedCourseData
                {
                    CourseID = item.CourseID,
                    Title = item.Title,
                    Assigned = instructorCourses.Contains(item.CourseID)
                });
            }
        }

        public void UpdateUpdateInstructorCourses(SchoolContext _context
            , string[] selectedCourses
            , Instructor instructorToUpdate)
        {
            if(selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCourseHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(
                instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));

            foreach (var item in _context.Courses)
            {
                if (selectedCourseHS.Contains(item.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(item.CourseID))
                    {
                        instructorToUpdate.CourseAssignments.Add(
                            new CourseAssignment
                            {
                                InstructorID = instructorToUpdate.ID,
                                CourseID = item.CourseID
                            });
                    }
                }
                else
                {
                    if (instructorCourses.Contains(item.CourseID))
                    {
                        CourseAssignment courseToRemove =
                            instructorToUpdate.CourseAssignments.SingleOrDefault(c => c.CourseID == item.CourseID);
                        instructorToUpdate.CourseAssignments.Remove(courseToRemove);
                    }
                }
            }
        }
    }
}
