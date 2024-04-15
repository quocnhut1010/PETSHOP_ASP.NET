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
using System.Xml.Linq;

namespace Cuahangthucung.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
      private CuahangTCEntities1 db = new CuahangTCEntities1();
        public ActionResult Index()
        {
            //if (Session["TenDN"] != null)
                return View();
            //else
              //  return RedirectToAction("Login");

        }
       
        public ActionResult Pet(int? page)
         {
               if (Session["TaikhoanAdmin"] == null)
               return RedirectToAction("Login", "Admin");
              int pageNumber = (page ?? 1);
              int pageSize = 6;
             //return View(db.THUCUNGs.ToList());
             return View(db.THUCUNGs.ToList().OrderBy(n => n.MaTC).ToPagedList(pageNumber, pageSize));

         }
       
        



        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn) == true)
            {
                ViewBag.Loi1 = "Hãy nhập username";
            }
            if (String.IsNullOrEmpty(matkhau) == true)
            {
                ViewBag.Loi2 = "Hãy nhạp password";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == tendn && n.MatKhau == matkhau);
                KHACHHANG us = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (ad != null)
                {
                    Session["TaikhoanAdmin"] = ad;
                    return RedirectToAction("Index", "DoanhThu");
                }
                else if (us != null)
                {
                    Session["TaikhoanUser"] = us;
                    return RedirectToAction("Index1", "User");
                }
                else { ViewBag.Thongbao = "Username hoặc Password không đúng"; }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["TaiKhoanAdmin"] = null;
            Session["TaiKhoanUser"] = null;
            return RedirectToAction("Index1", "User");
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiTC = new SelectList(db.LOAITHUCUNGs.ToList().OrderBy(n => n.TenLoaiTC), "MaLoaiTC", "TenLoaiTC");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(THUCUNG thucung, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid == true)
            {
                try
                {
                    if (Image.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(Image.FileName);
                        string path = Path.Combine(Server.MapPath("~/Image"), filename);
                        Image.SaveAs(path);
                        thucung.AnhBia = filename;
                    }
                    thucung.NgayCapNhat = DateTime.Now;

                    db.THUCUNGs.Add(thucung);
                    db.SaveChanges();
                    return RedirectToAction("Pet");
                }
                catch { ViewBag.Mesage = "Không thành công"; }
            }

           
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiTC = new SelectList(db.LOAITHUCUNGs.ToList().OrderBy(n => n.TenLoaiTC), "MaLoaiTC", "TenLoaiTC");
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THUCUNG thucung = db.THUCUNGs.Find(id);
            if (thucung == null)
            {
                return HttpNotFound();
            }
            return View(thucung);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            THUCUNG thucung = db.THUCUNGs.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            if (thucung == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs, "MaNCC", "TenNCC", thucung.MaNCC);
            ViewBag.MaLoaiTC = new SelectList(db.LOAITHUCUNGs, "MaLoaiTC", "TenLoaiTC", thucung.MaLoaiTC);
            return View(thucung);
        }
        [HttpPost]
       
        [ValidateInput(false)]
        public ActionResult Edit(THUCUNG thucung, HttpPostedFileBase FileUpload)
        {
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs, "MaNCC", "TenNCC", thucung.MaNCC);
            ViewBag.MaLoaiTC = new SelectList(db.LOAITHUCUNGs, "MaLoaiTC", "TenLoaiTC", thucung.MaLoaiTC);
            if (ModelState.IsValid)
            {

                if (FileUpload != null) // Kiểm tra để xác nhận cho thay đổi ảnh bìa
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(FileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Image"), sFileName); // Kiếm tra file đã tồn tại chưa
                    if (!System.IO.File.Exists(path))
                    {
                        FileUpload.SaveAs(path);

                    }
                    thucung.AnhBia = sFileName;

                }
               

                db.Entry(thucung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Pet");
            }

            return View(thucung);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            THUCUNG thucung = db.THUCUNGs.SingleOrDefault(n => n.MaTC == id);
            ViewBag.BooID = thucung.MaTC;
            if (thucung == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thucung);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult XacNhanxoa(int id)
        {
            THUCUNG thucung = db.THUCUNGs.SingleOrDefault(n => n.MaTC == id);
            ViewBag.BooID = thucung.MaTC;
            if (thucung == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.THUCUNGs.Remove(thucung);
            db.SaveChanges();
            return RedirectToAction("Pet");
        }

        public ActionResult TinhtongPet()
        {
            int tongSoLuongPet = db.THUCUNGs.Count();
            ViewBag.TongSoLuongPet = tongSoLuongPet;

            return View();
        }


        public ActionResult ViewPartial()
        {
            // Execute the query and materialize the result to a list
            var comment = db.Comments.ToList();

            return View(comment);
        }
        [HttpPost]
        public ActionResult SendMessage(Comment comment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Assuming you have a DbContext named 'CuahangTCEntities1' and a DbSet for messages named 'Messages'
                    using (var db = new CuahangTCEntities1())
                    {
                        // Create a new Message entity and populate it with the data from the form
                        var newComment = new Comment
                        {
                            Name = comment.Name,
                            Email = comment.Email,
                            MessageText = comment.MessageText,
                            CreatedAt = DateTime.Now
                        };

                        // Add the new message to the database
                        db.Comments.Add(newComment);

                        // Save changes to the database
                        db.SaveChanges();
                    }

                    // Redirect to the original page or another page as needed
                    return RedirectToAction("Index1", "User");
                }

                // If the model state is not valid, return to the same page with validation errors
                return View(); // Update this line as needed
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as appropriate for your application
                // You can also return an error view or redirect to an error page
                return View("Error"); // Update this line as needed
            }
        }

    }
}