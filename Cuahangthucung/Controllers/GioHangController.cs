using Cuahangthucung.Models;
using Cuahangthucung.Other;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuahangthucung.Controllers
{
    public class GioHangController : Controller
    {
        CuahangTCEntities1 db = new CuahangTCEntities1();
        // GET: GioHang

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;

            }
            return lstGioHang;
        }
        public ActionResult ThemGiohang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
            }

            GioHang sp = lstGioHang.Find(n => n.iMaTC == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
                Session["GioHang"] = lstGioHang;
                TempData["ThongBao"] = "Thêm sản phẩm vào giỏ hàng thành công!";
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }
        //Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        //tong tien
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index1", "User");
            }
            if (TempData["ThongBao"] != null)
            {
                ViewBag.ThongBao = TempData["ThongBao"].ToString();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return PartialView();
        }
        //Xóa sản phẩm khỏi giỏ hàng
        public ActionResult XoaSPKhoiGioHang(int iMaTC)
        {
            //Lấy giỏ hàng

            List<GioHang> lstGioHang = LayGioHang();

            //Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaTC == iMaTC);

            //Xóa sản phẩm khỏi giỏ hàng
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaTC == iMaTC);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index1", "User");
                }
            }

            //Cập nhật lại giỏ hàng
            return RedirectToAction("GioHang");
        }
        //Cập nhật giỏ hàng
        public ActionResult CapNhatGioHang(int iMaTC, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaTC == iMaTC);

            // Nếu tồn tại thì cho sửa số lượng
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
                return RedirectToAction("GioHang");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index1", "User");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoanuser"] == null)
                return RedirectToAction("Login", "Admin");
            // Kiểm tra đăng nhập chưa
            if (Session["Taikhoanuser"] == null || Session["Taikhoanuser"].ToString() == "")
            {
                // In giá trị Session để kiểm tra
                return RedirectToAction("DatHang", "GioHang");
            }

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index1", "User");
            }

            // Lấy hàng từ Session
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstGioHang);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {

            if (Session["Taikhoanuser"] == null || Session["Taikhoanuser"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "GioHang");
            }

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index1", "User");
            }

            // Create the order
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoanuser"];
            List<GioHang> lstGioHang = LayGioHang();

            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.DaThanhToan = false;

            // Add the order to the database
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges(); // Commit changes to the database

            // Add order details
            foreach (var item in lstGioHang)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaTC = item.iMaTC;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CHITIETDATHANGs.Add(ctdh);
            }
            db.SaveChanges(); // Commit changes to the database
            // Update payment status
            //ddh.DaThanhToan = true;
            //db.Entry(ddh).State = EntityState.Modified;
            //db.SaveChanges();
            //// Add payment details
            //CHITIETTHANHTOAN chiTietThanhToan = new CHITIETTHANHTOAN
            //{
            //    MaDonHang = ddh.MaDonHang,
            //    MaGiaoDich = f["MaGiaoDich"],  // Cần lấy mã giao dịch từ cổng thanh toán thực tế
            //    TongTien = (decimal?)TongTien(),
            //    NgayThanhToan = DateTime.Now
            //};
            //db.CHITIETTHANHTOANs.Add(chiTietThanhToan);
            //db.SaveChanges();
            Session["GioHang"] = null; // Clear the shopping cart



            return RedirectToAction("XacNhanDonHang", "GioHang");



        }
        [HttpGet]
        public ActionResult DatHangOnline()
        {
            if (Session["Taikhoanuser"] == null)
                return RedirectToAction("Login", "Admin");
            // Kiểm tra đăng nhập chưa
            if (Session["Taikhoanuser"] == null || Session["Taikhoanuser"].ToString() == "")
            {
                // In giá trị Session để kiểm tra
                return RedirectToAction("DatHang", "GioHang");
            }

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index1", "User");
            }

            // Lấy hàng từ Session
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHangOnline(FormCollection f)
        {

            if (Session["Taikhoanuser"] == null || Session["Taikhoanuser"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "GioHang");
            }

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index1", "User");
            }

            // Create the order
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoanuser"];
            List<GioHang> lstGioHang = LayGioHang();

            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.DaThanhToan = false;

            // Add the order to the database
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges(); // Commit changes to the database

            // Add order details
            foreach (var item in lstGioHang)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaTC = item.iMaTC;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CHITIETDATHANGs.Add(ctdh);
            }
            db.SaveChanges(); // Commit changes to the database
            // Update payment status
            ddh.DaThanhToan = true;
            db.Entry(ddh).State = EntityState.Modified;
            db.SaveChanges();
            // Add payment details
            CHITIETTHANHTOAN chiTietThanhToan = new CHITIETTHANHTOAN
            {
                MaDonHang = ddh.MaDonHang,
                MaGiaoDich = f["MaGiaoDich"],  // Cần lấy mã giao dịch từ cổng thanh toán thực tế
                TongTien = (decimal?)TongTien(),
                NgayThanhToan = DateTime.Now
            };
            db.CHITIETTHANHTOANs.Add(chiTietThanhToan);
            db.SaveChanges();
            Session["GioHang"] = null; // Clear the shopping cart

            return RedirectToAction("PaymentConfirm", "GioHang");



        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatkhau = collection["MatKhau"];
            var url = collection["url"];
            if (String.IsNullOrEmpty(url))
            {
                url = "~/User/Index";
            }
            if (String.IsNullOrEmpty(sTenDN))
            {

                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }

            else if (String.IsNullOrEmpty(sMatkhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {

                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatkhau);
                if (kh != null)

                {

                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanuser"] = kh;
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return RedirectToAction("DatHang", "GioHang");
        }
        public ActionResult Payment()
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (TongTien() * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);


            return Redirect(paymentUrl);
        }

        public ActionResult PaymentConfirm()
        {
            Session["GioHang"] = null;
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    ViewBag.vnpayTranId = vnpayTranId; // Set the vnpayTranId in ViewBag

                    // Debugging statement
                    System.Diagnostics.Debug.WriteLine("vnpayTranId: " + vnpayTranId);

                    // Save vnpayTranId to the database
                    //SaveVNPayTransactionIdToDatabase(orderId, vnpayTranId);
                }

                else
                {
                    //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                   
                }
            }
            else
            {
                ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
            }

            return View();
        }
        //private void SaveVNPayTransactionIdToDatabase(long orderId, long vnpayTranId)
        //{
        //    // Retrieve the existing CHITIETTHANHTOAN record based on orderId or any other criteria
        //    var chiTietThanhToan = db.CHITIETTHANHTOANs.FirstOrDefault(x => x.MaDonHang == orderId);

        //    if (chiTietThanhToan != null)
        //    {
        //        // Update the VNPayTransactionId property
        //        chiTietThanhToan.VNPayTransactionId = vnpayTranId;

        //        // Save changes to the database
        //        db.Entry(chiTietThanhToan).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //}
        // Add this method to your existing class
        private double TinhTongTienTatCaDonHang()
        {
            double tongTien = 0;

            // Get all order details
            var allChiTietDonHang = db.CHITIETDATHANGs.ToList();

            // Calculate the total amount
            if (allChiTietDonHang != null && allChiTietDonHang.Count > 0)
            {
                tongTien = allChiTietDonHang.Sum(ct => (double)(ct.SoLuong * ct.DonGia));
            }

            return tongTien;
        }

        // Modify your controller to pass the total amount to the partial view
        public ActionResult GioHangPartial1()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            double tongTienTatCaDonHang = TinhTongTienTatCaDonHang();

            return View("TinhTongTienTatCaDonHang");
        }

    }
}