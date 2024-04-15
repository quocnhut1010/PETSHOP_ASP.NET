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
namespace Cuahangthucung.Controllers
{
    public class DonHangController : Controller
    {
        // GET: DonHang
        CuahangTCEntities1 db = new CuahangTCEntities1();
        public ActionResult DonHang(int? page)
        {
            //if (Session["TaikhoanAdmin"] == null)
            //    return RedirectToAction("Login", "Admin");
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            //return View(db.THUCUNGs.ToList());
            return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));

        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
           DONDATHANG dh = db.DONDATHANGs.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (dh == null)
            {
                return HttpNotFound();
            }
            return View(dh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DONDATHANG dh)
        {

            if (ModelState.IsValid)
            {



                db.Entry(dh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DonHang","DonHang");
            }

            return View(dh);
        }
    }
}