using Cuahangthucung.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Cuahangthucung.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private CuahangTCEntities1 db = new CuahangTCEntities1();
        private List<THUCUNG> Laypetmoi(int count)
        {
            return db.THUCUNGs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
           public ActionResult Index(int ? page )
           {
               //if (Session["Taikhoanuser"] == null)
               //    return RedirectToAction("Login", "Admin");
               int pageNumber = (page ?? 1);
               int pageSize = 8;
               //return View(db.Books.ToList());
               return View(db.THUCUNGs.ToList().OrderBy(keySelector: n => n.MaTC).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index1(int? page)
        {
            //if (Session["Taikhoanuser"] == null)
            //    return RedirectToAction("Login", "Admin");
            int pageNumber = (page ?? 1);
            int pageSize = 8;
            //return View(db.Books.ToList());
            return View(db.THUCUNGs.ToList().OrderBy(keySelector: n => n.MaTC).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult SendMessage(Message message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Assuming you have a DbContext named 'CuahangTCEntities1' and a DbSet for messages named 'Messages'
                    using (var db = new CuahangTCEntities1())
                    {
                        // Create a new Message entity and populate it with the data from the form
                        var newMessage = new Message
                        {
                            Name = message.Name,
                            Email = message.Email,
                            Phone = message.Phone,
                            MessageText = message.MessageText,
                            CreatedAt = DateTime.Now
                        };

                        // Add the new message to the database
                        db.Messages.Add(newMessage);

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



        public ActionResult Details(int id)
        {
            var Thucung = from s in db.THUCUNGs
                       where s.MaTC == id
                       select s;
            return View(Thucung.Single());
        }
        public ActionResult Details1(int id)
        {
            var Thucung = from s in db.THUCUNGs
                          where s.MaTC == id
                          select s;
            return View(Thucung.Single());
        }
        public ActionResult PetByTopic1(int? page)
        {
            int iSize = 3;
            int iPageNumber = (page ?? 1);

            // Lấy danh sách thú cưng có MaLoaiTC = 1 chó
            var thucung = db.THUCUNGs.Where(s => s.MaLoaiTC == 1).ToList();

            return View(thucung.ToPagedList(iPageNumber, iSize));

        }
        public ActionResult PetByTopic(int? page)
        {
            int iSize = 3;
            int iPageNumber = (page ?? 1);

            // Lấy danh sách thú cưng có MaLoaiTC = 2 mèo
            var thucung = db.THUCUNGs.Where(s => s.MaLoaiTC == 2).ToList();

            return View(thucung.ToPagedList(iPageNumber, iSize));

        }
       
        public ActionResult PetByTopic2(int? page)
        {
            int iSize = 3;
            int iPageNumber = (page ?? 1);

            // Lấy danh sách thú cưng có MaLoaiTC = 1 chó
            var thucung = db.THUCUNGs.Where(s => s.MaLoaiTC == 3).ToList();

            return View(thucung.ToPagedList(iPageNumber, iSize));

        }
        public ActionResult PetByTopic3(int? page)
        {
            int iSize = 3;
            int iPageNumber = (page ?? 1);

            // Lấy danh sách thú cưng có MaLoaiTC = 1 chó
            var thucung = db.THUCUNGs.Where(s => s.MaLoaiTC == 4).ToList();

            return View(thucung.ToPagedList(iPageNumber, iSize));

        }
        [HttpGet]
        public ActionResult DangKi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKi(KHACHHANG kh, FormCollection f)
        {
            //Gan cac gia tri nguoi dung nhap du lieu cho cac bien
            var sHoTen = f["HoTen"];
            var sTenDN = f["TenDN"];
            var sMatKhau = f["Matkhau"];
            var sMatKhauNhapLai = f["MatKhauNL"];
            var sDiaChi = f["DiaChi"];
            var sEmail = f["Email"];
            var sDienThoai = f["DienThoai"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", f["NgaySinh"]);
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ và tên không được rỗng";
            }

            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }

            else if (String.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }

            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }

            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }

            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "Số điện thoại không được rỗng";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                //Gần giá trị cho đối tượng được tạo mới (kh)
                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTenDN;
                kh.MatKhau = f["Matkhau"];
                kh.Email = f["Email"];
                kh.DiaChi = sDiaChi;
                kh.DienThoai = sDienThoai;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();

                return RedirectToAction("Login","Admin");
            }
            return this.DangKi();
        }
        public ActionResult Find(string searchString)
        {
            var tc = db.THUCUNGs.Include(b => b.LOAITHUCUNG);
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                tc = tc.Where(b => b.TenTC.ToLower().Contains(searchString));
            }
            return View(tc.ToList());
        }

        public ActionResult ViewPartial()
        {
            // Execute the query and materialize the result to a list
            var messages = db.Messages.ToList();

            return View(messages);
        }

     //   public ActionResult Random()
     //   {
     //       // Lấy một sản phẩm ngẫu nhiên
     //       var randomProduct = db.THUCUNGs
     //.Where(x => x.ThoiGianKetThuc > DateTime.Now)
     //       .OrderBy(x => Guid.NewGuid())
     //       .FirstOrDefault();

     //       return View(randomProduct);


            
     //   }
        public ActionResult SPSale()
        {
            var Thucung = from s in db.THUCUNGs
                       where s.MaTC == 1
                       select s;
            return View(Thucung.Single());
        }
    
    }
}