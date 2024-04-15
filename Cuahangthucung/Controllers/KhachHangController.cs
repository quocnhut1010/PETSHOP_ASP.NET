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
    public class KhachHangController : Controller
    {
        // GET: KhachHang
         CuahangTCEntities1 db = new CuahangTCEntities1();
        public ActionResult Index()
        {
            int tongSoLuongKhachHang = db.KHACHHANGs.Count();
            ViewBag.TongSoLuongKhachHang = tongSoLuongKhachHang;

            return View();
        }
        public ActionResult KhachHang(int? page)
        {
            //if (Session["TaikhoanAdmin"] == null)
            //    return RedirectToAction("Login", "Admin");
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            //return View(db.THUCUNGs.ToList());
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));

        }
        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(KHACHHANG khachhang)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                  
                    db.KHACHHANGs.Add(khachhang);
                    db.SaveChanges();
                    return RedirectToAction("KhachHang");
                }
                catch { ViewBag.Mesage = "Không thành công"; }
            }

           
            return View(khachhang);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kh = db.KHACHHANGs.Find(id);
            if (kh == null)
            {
                return HttpNotFound();
            }
            return View(kh);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.BooID = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
  
        [HttpPost, ActionName("Delete")]
        public ActionResult XacNhanxoa(int id)
        {
            KHACHHANG khachhang = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.BooID = khachhang.MaKH;
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.KHACHHANGs.Remove(khachhang); // Corrected entity type to remove
            db.SaveChanges();
            return RedirectToAction("KhachHang");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KHACHHANG kh = db.KHACHHANGs.Find(id);

            if (kh == null)
            {
                return HttpNotFound();
            }

            return View(kh);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("KhachHang", "KhachHang");
            }

            return View(kh);
        }

    }
}