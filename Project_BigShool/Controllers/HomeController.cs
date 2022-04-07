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
    public class HomeController : Controller
    {
        BigSchoolContext context = new BigSchoolContext();
        public ActionResult Index()
        {
            //var upcommingCourse = context.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            var upcommingCourse = context.Courses.ToList();

            //Lay user login hien tai dua vao ViewBag > de truyen qua view
            //Neu gia tri = null > nghia la chua dang nhap
            var loginUser = User.Identity.GetUserId();
            ViewBag.LoginUser = loginUser;
            foreach (Course i in upcommingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                i.Name = user.Name;
                Attendance find = context.Attendances.FirstOrDefault(p => p.CourseId == i.Id && p.Attendee == loginUser);
                
                //Kiem tra user co tham gia khoa hoc
                if (find == null)
                {
                    i.isShowGoing = true;
                }

                //Kiem tra user da theo doi giang vien cua khoa hoc
                Following findFollow = context.Followings.FirstOrDefault(p => p.FollowerId == loginUser && p.FolloweeId == i.LecturerId);
                if(findFollow == null)
                {
                    i.isShowFollow = true;
                }
            }
            return View(upcommingCourse);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}