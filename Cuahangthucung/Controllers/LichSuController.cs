using Cuahangthucung.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Net;
using System.Data.Entity;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using System.Web.Configuration;

namespace Cuahangthucung.Controllers
{
    public class LichSuController : Controller
    {
        // GET: LichSu
        private CuahangTCEntities1 db = new CuahangTCEntities1();
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult DonHang(int? page)
        //{
        //    if (Session["TaikhoanUser"] == null)
        //        return RedirectToAction("Login", "Admin");
        //    int pageNumber = (page ?? 1);
        //    int pageSize = 6;
        //    //return View(db.THUCUNGs.ToList());
        //    return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));


        //}
        public ActionResult DonHang(int? page)
        {
            if (Session["TaikhoanUser"] == null)
                return RedirectToAction("Login", "Admin");
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            //return View(db.THUCUNGs.ToList());
            return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));


        }
    }
}