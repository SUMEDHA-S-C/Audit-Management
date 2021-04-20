using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AuditManagementPortalClientMVC.Models;
using AuditManagementPortalClientMVC.Models.Context;
using AuditManagementPortalClientMVC.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuditManagementPortalClientMVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly ISeverityRepo _severityRepo;
        private readonly ILoginRepo _loginRepo;
        private readonly IUserRepo _userRepo;
        private readonly IChecklistRepo _checklistRepo;

        public HomeController(ISeverityRepo severityRepo, ILoginRepo loginRepo, IUserRepo userRepo, IChecklistRepo checklistRepo)
        {

            _severityRepo = severityRepo;
            _loginRepo = loginRepo;
            _userRepo = userRepo;
            _checklistRepo = checklistRepo;
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            //try
            //{
            //    var t = HttpContext.Session.GetString("token").ToString();

            //    if (t == "")
            //    {
            //        throw new Exception();
            //    }
            //    else { }
            //}
            //catch (Exception e)
            //{
            //    HttpContext.Session.Clear();
            //    ViewBag.ErrorMessage = "";
            //    return View();
            //}
            //return RedirectToAction("AuditType", "Home");
            return View();
        }


        [HttpPost]
        public IActionResult Login(User user)
        {
            bool value = _userRepo.CheckUser(user);
            if (value == true)
            {
                string token = _loginRepo.GetToken().Result;
                var t = token;

                if (t != "")
                {
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("uid", user.Name);
                    HttpContext.Session.SetString("token", t);
                    return RedirectToAction("AuditType", "Home");

                }


            }
            ViewBag.ErrorMessage = "• Invalid Username Or Password";
            return View();
        }

        [HttpGet]
        public IActionResult AuditType()
        {
            try
            {
                var t = HttpContext.Session.GetString("token").ToString();

                if (t == "")
                {
                    throw new Exception();
                }
                else { }
            }
            catch (Exception e)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Home");
            }
            ViewBag.User = HttpContext.Session.GetString("uid");

            return View();
        }

        [HttpGet]
        public IActionResult Checklist()
        {
            try
            {
                HttpContext.Session.Remove("audittype");
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            { }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult Checklist(string audittype)
        {
            if (audittype == "Internal")
            {

                List<string> listOfQuestions = new List<string>();
                listOfQuestions = _checklistRepo.ProvideChecklist("Internal");
                HttpContext.Session.SetString("audittype", audittype);
                return View(listOfQuestions);
            }
            if (audittype == "SOX")
            {

                List<string> listOfQuestions = new List<string>();
                listOfQuestions = _checklistRepo.ProvideChecklist("SOX");
                HttpContext.Session.SetString("audittype", audittype);
                return View(listOfQuestions);
            }

            return RedirectToAction("AuditType", "Home");
        }

        //[HttpGet]
        //public IActionResult Checklist() {
        //    return RedirectToAction("Severity", "Home");
        //}


        [HttpGet]
        public IActionResult Severity()
        {
            try
            {
                HttpContext.Session.Remove("audittype");
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            { }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult Severity(bool q1, bool q2, bool q3, bool q4, bool q5,
             string pnm, string mnm, string onm, DateTime dtee)
        {
            string dtees = dtee.ToString();
            string aType = HttpContext.Session.GetString("audittype").ToString();
            AuditRequest auditRequest = new AuditRequest();
            Questions qq = new Questions()
            {
                Question1 = q1,
                Question2 = q2,
                Question3 = q3,
                Question4 = q4,
                Question5 = q5
            };
            auditRequest.ProjectName = pnm;
            auditRequest.ProjectManagerName = mnm;
            auditRequest.ApplicationOwnerName = onm;
            auditRequest.Auditdetails = new AuditDetail() { Type = aType, Date = dtees, questions = qq };
            AuditResponse auditResponse = new AuditResponse();

            auditResponse = _severityRepo.GetResponse(auditRequest);

            StoreAuditResponse storeAudit = new StoreAuditResponse()
            {
                ProjectName = pnm,
                ProjectManagerName = mnm,
                ApplicationOwnerName = onm,
                AuditType = aType,
                AuditDate = dtees,
                AuditId = auditResponse.AuditId,
                ProjectExecutionStatus = auditResponse.ProjectExexutionStatus,
                RemedialActionDuration = auditResponse.RemedialActionDuration
            };
            try
            {

                _severityRepo.StoreResponse(storeAudit);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Home");
            }


            return View(auditResponse);
        }




    }
}
