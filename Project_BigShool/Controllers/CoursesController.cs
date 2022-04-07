using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project_BigShool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_BigShool.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        BigSchoolContext context = new BigSchoolContext();

        public ActionResult Create()
        {
            Course objCourse = new Course();
            objCourse.listCategory = context.Categories.ToList();

            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            //Khong xet valid LectureId vi bang user dang nhap
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.listCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }

            //Lay Login user iD
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            //Add vao CSDL
            context.Courses.Add(objCourse);
            context.SaveChanges();


            return RedirectToAction("Index", "Home");
        }

        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            var userID = User.Identity.GetUserId();
            var listAttendances = context.Attendances.Where(p => p.Attendee == userID).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var loginUser = User.Identity.GetUserId();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(loginUser);
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Courses.Where(p => p.LecturerId == loginUser && p.IsCancled != true).ToList();
            foreach (var item in courses)
            {
                item.Name = user.Name;

            }
            return View(courses);
        }

        //[HttpPost]
        //public ActionResult Edit(int id)
        //{
        //    //BigSchoolContext context = new BigSchoolContext();
        //    var loginUser = User.Identity.GetUserId();
        //    var course = context.Courses.FirstOrDefault(c => c.LecturerId == loginUser && c.Id == id);
        //    if (course == null)
        //    {
        //        return HttpNotFound("Not found course");
        //    }
        //    course.listCategory = context.Categories.ToList();
        //    //context.SaveChanges();
        //    return View("Edit", course);
        //}
        //public ActionResult Delete(int id)
        //{
        //    var userID = User.Identity.GetUserId();
        //    BigSchoolContext context = new BigSchoolContext();
        //    var findCourse = context.Courses.FirstOrDefault(p => p.Id == id);
        //    findCourse.IsCancled = true;
        //    context.SaveChanges();
        //    return RedirectToAction("Mine");
        //}

        [HttpGet]
        public ActionResult Delete(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Course cs = context.Courses.Where(p => p.Id == id).FirstOrDefault();
            if (cs == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cs);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xacnhanxoa(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            var findCourse = context.Courses.FirstOrDefault(p => p.Id == id);
            findCourse.IsCancled = true;
            context.SaveChanges();
            return RedirectToAction("Mine");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Course cs = context.Courses.Where(p => p.Id == id).FirstOrDefault();
            cs.listCategory = context.Categories.ToList();
            if (cs == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cs);
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Xacnhansua(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course cs = context.Courses.FirstOrDefault(n => n.Id == id);
            UpdateModel(cs); ;
            context.SaveChanges();
            return RedirectToAction("Mine");
        }

        public ActionResult Following()
        {
            ApplicationUser loginUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listFollowings = context.Followings.Where(p => p.FollowerId == loginUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Following item in listFollowings)
            {
                //Lay tat ca cac khoa hoc cua gv duoc theo doi
                var listCourse = context.Courses.Where(p => p.LecturerId == item.FolloweeId).ToList();
                if(listCourse.Count > 0)
                {
                    //Tim ten cua gv
                    string Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(listCourse[0].LecturerId).Name;
                    foreach (Course i in listCourse)
                    {
                        i.Name = Name;
                    }
                    //Add vao course
                    courses.AddRange(listCourse);
                }
            }
            return View(courses);
        }
    }
}