using Microsoft.AspNet.Identity;
using Project_BigShool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project_BigShool.Controllers
{
    public class FollowingsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following following)
        {
            //Nguoi theo doi la user dang nhap
            //Nguoi duoc theo doi la follow.FolloweeId - truyen len tu script
            var loginUser = User.Identity.GetUserId();
            following.FollowerId = loginUser;
            BigSchoolContext context = new BigSchoolContext();

            //Kiem tra ma userId da duoc theo doi chua
            Following find = context.Followings.FirstOrDefault(p => p.FollowerId == following.FollowerId && p.FolloweeId == following.FolloweeId);
            if(find == null)
            {
                context.Followings.Add(following);
            }
            else
            {
                context.Followings.Remove(find);
            }
            context.SaveChanges();
            return Ok();
        }
    }
}
