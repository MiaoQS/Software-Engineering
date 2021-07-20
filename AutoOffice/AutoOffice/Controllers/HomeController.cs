using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoOffice.Models;
using Microsoft.AspNetCore.Identity;
using AutoOffice.Data;
using AutoOffice.Models.HomeViewModels;

namespace AutoOffice.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext db;

        public HomeController(
          UserManager<ApplicationUser> userManager,
          ApplicationDbContext injectedContext)
        {
            _userManager = userManager;
            db = injectedContext;
        }

        public async Task<IActionResult> Index()
        {
            // 验证是否登陆
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ViewData["UserName"] = "";
                return View();
            }
            else
            {
                ViewData["UserName"] = user.UserName;
                return View();
            }
        }

        public IActionResult About()
        {

            return View();
        }

        public IActionResult Contact()
        {

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // MARK: - 人事管理 HumanResourceManage
        public async Task<IActionResult> HumanResourceManage()
        {
            // 验证是否登陆
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var username = user.UserName;
            // 验证是否登为管理用户
            if (username != "root@root.com")
            {
                throw new ApplicationException($"You don't have this power, user {username}.");
            }

            var model = new HumanResourceManageModel
            {
                HumanManages = db.HumanManages.ToArray()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> HumanResourceManageSet(string email, string name, string department, string job)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var username = user.UserName;
            if (username != "root@root.com")
            {
                throw new ApplicationException($"You don't have this power, user {username}.");
            }

            HumanManage humanManageToUpdate = db.HumanManages.First(p => p.Email == email);
            humanManageToUpdate.Name = name;
            humanManageToUpdate.Job = job;
            humanManageToUpdate.Department = department;
            db.SaveChanges();

            return View();
        }

        // MARK: - 站内短信 Message 
        public async Task<IActionResult> Message()
        {
            // 验证是否登陆
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new HumanResourceManageModel
            {
                HumanManages = db.HumanManages
                                                .Where(h => h.Email != user.UserName)
                                                .ToArray()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MessageDetail(string toEmail)
        {
            // 验证是否登陆
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new MessageModel
            {
                Messages = db.Messages
                                    .Where(m => (m.FromEmail == toEmail && m.ToEmail == user.UserName) || (m.FromEmail == user.UserName && m.ToEmail == toEmail))
                                    .OrderBy(m => m.Time)
                                    .ToArray()
            };

            ViewData["UserName"] = user.UserName;
            ViewData["toEmail"] = toEmail;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MessageSend(string toEmail, string text)
        {
            // 验证是否登陆
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ViewData["toEmail"] = toEmail;

            if (text == null || text.Trim() == "") {
              return View();
            }
            Message message = new Message();
            message.FromEmail = user.UserName;
            message.ToEmail = toEmail;
            message.Text = text;
            message.Time = DateTime.Now;
            db.Messages.Add(message);
            db.SaveChanges();

            return View();
        }


    // MARK: - Job Remind
    public async Task<IActionResult> JobRemind() {
      // 验证是否登陆
      var user = await _userManager.GetUserAsync(User);
      if (user == null) {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      var model = new JobRemindModel {
        JobReminds = db.JobReminds
                                          .Where(h => h.Email == user.UserName)
                                          .ToArray()
      };

      ViewData["UserName"] = user.UserName;

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> JobRemindAdd(string content) {
      // 验证是否登陆
      var user = await _userManager.GetUserAsync(User);
      if (user == null) {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }
      JobRemind jobRemind = new JobRemind();
      jobRemind.Content = content;
      jobRemind.Email = user.Email;
      jobRemind.Finished = false;
      db.JobReminds.Add(jobRemind);
      db.SaveChanges();

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> JobRemindFinish(string id) {
      // 验证是否登陆
      var user = await _userManager.GetUserAsync(User);
      if (user == null) {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      db.JobReminds.Where(job => job.ID == id).First().Finished = true;
      db.SaveChanges();

      return View();
    }

    // MARK: - Official Papers
    public async Task<IActionResult> OfficialPaper() {
      // 验证是否登陆
      var user = await _userManager.GetUserAsync(User);
      if (user == null) {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      var model = new HumanResourceManageModel {
        HumanManages = db.HumanManages
                                                 .Where(h => h.Email != user.UserName)
                                                 .ToArray()
      };

      return View(model);
    }

    public async Task<IActionResult> OfficialPaperManage() {
      // 验证是否登陆
      var user = await _userManager.GetUserAsync(User);
      if (user == null) {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      var model = new OfficialPaperModel {
        OfficialPapers = db.OfficialPapers
                                                 .Where(h => h.ToEmail == user.UserName || h.FromEmail == user.UserName)
                                                 .ToArray()
      };

      ViewData["UserName"] = user.UserName;

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> OfficialPaperWrite(string toEmail) {
      // 验证是否登陆
      var user = await _userManager.GetUserAsync(User);
      if (user == null) {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }
      
      ViewData["ToEmail"] = toEmail;

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> OfficialPaperSend(string title, string content, string toEmail) {
      // 验证是否登陆
      var user = await _userManager.GetUserAsync(User);
      if (user == null) {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      OfficialPaper officialPaper = new OfficialPaper();
      officialPaper.FromEmail = user.UserName;
      officialPaper.ToEmail = toEmail;
      officialPaper.Title = title;
      officialPaper.Content = content;
      officialPaper.Time = DateTime.Now;
      officialPaper.Approvement = (int)ApprovementState.NotDecideYet;
      db.OfficialPapers.Add(officialPaper);
      db.SaveChanges();

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> OfficialPaperApprove(string id) {
      // 验证是否登陆
      var user = await _userManager.GetUserAsync(User);
      if (user == null) {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      db.OfficialPapers.Where(p => p.ID == id).First().Approvement = (int)ApprovementState.Approved;
      db.SaveChanges();

      return View();
    }

    [HttpPost]
    public async Task<IActionResult> OfficialPaperNotApprove(string id) {
      // 验证是否登陆
      var user = await _userManager.GetUserAsync(User);
      if (user == null) {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      db.OfficialPapers.Where(p => p.ID == id).First().Approvement = (int)ApprovementState.NotApproved;
      db.SaveChanges();

      return View();
    }

  }
}
