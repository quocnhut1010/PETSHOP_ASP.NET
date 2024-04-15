using Cuahangthucung.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace Cuahangthucung.Controllers
{
    public class DoanhThuController : Controller
    {
        CuahangTCEntities1 db = new CuahangTCEntities1();
        // GET: DoanhThu
        public ActionResult Index()
        {
            // Perform the calculation and retrieve data from the database
            var query = from ddh in db.DONDATHANGs
                        join ctdh in db.CHITIETDATHANGs on ddh.MaDonHang equals ctdh.MaDonHang
                        where ddh.DaThanhToan == false || ddh.DaThanhToan == true
                        group new { ctdh.SoLuong, ctdh.DonGia } by new { Month = (ddh.NgayDat ?? DateTime.MinValue).Month, DaThanhToan = ddh.DaThanhToan } into g
                        select new { g.Key.Month, g.Key.DaThanhToan, Revenue = g.Sum(x => x.SoLuong * x.DonGia) };

            // Create a list with all months from January to December
            List<int> allMonths = Enumerable.Range(1, 12).ToList();

            // Initialize lists for both DaThanhToan == false and DaThanhToan == true
            List<int> monthsFalse = new List<int>();
            List<decimal> revenuesFalse = new List<decimal>();
            List<int> monthsTrue = new List<int>();
            List<decimal> revenuesTrue = new List<decimal>();

            // Iterate through all months and set revenue to zero if no data for that month
            foreach (int month in allMonths)
            {
                var dataFalse = query.FirstOrDefault(item => item.DaThanhToan == false && item.Month == month);
                monthsFalse.Add(month);
                revenuesFalse.Add(dataFalse?.Revenue ?? 0);

                var dataTrue = query.FirstOrDefault(item => item.DaThanhToan == true && item.Month == month);
                monthsTrue.Add(month);
                revenuesTrue.Add(dataTrue?.Revenue ?? 0);
            }

            ViewBag.MonthsFalse = monthsFalse;
            ViewBag.RevenuesFalse = revenuesFalse;
            ViewBag.MonthsTrue = monthsTrue;
            ViewBag.RevenuesTrue = revenuesTrue;

            return View();


        }

        public ActionResult TinhTongTien()
        {
            decimal tongTien = TinhTongTienDonHang();
            ViewBag.TongTien = tongTien;

            return View();
        }
        private decimal TinhTongTienDonHang()
        {
            decimal tongTien = 0;

            try
            {
                // Sử dụng LINQ để thực hiện truy vấn và tính tổng tiền
                tongTien = db.CHITIETDATHANGs.Sum(ctdh => ctdh.SoLuong * ctdh.DonGia) ?? 0;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                // Điều này có thể bao gồm việc ghi log hoặc thông báo lỗi
                ViewBag.Error = "Đã xảy ra lỗi khi tính tổng tiền đơn hàng: " + ex.Message;
            }

            return tongTien;
        }
        
       








        public ActionResult TinhtongDonHang()
        {
            int tongSoLuongDH = db.DONDATHANGs.Count();
            ViewBag.TongSoLuongDH = tongSoLuongDH;

            return View();
        }
        public ActionResult TinhtongDonHangHomNay()
        {
            // Lấy ngày hiện tại
            DateTime ngayHienTai = DateTime.Now.Date;

            // Lọc và lấy danh sách đơn hàng hôm nay
            List<DONDATHANG> donHangHomNay = db.DONDATHANGs
                .Where(dh => dh.NgayDat.HasValue &&
                             DbFunctions.TruncateTime(dh.NgayDat.Value) == ngayHienTai)
                .ToList();

            // Cập nhật số lượng đơn hàng hôm nay
            int tongSoLuongDHHomNay = donHangHomNay.Count;

            ViewBag.TongSoLuongDHHomNay = tongSoLuongDHHomNay;
            ViewBag.DonHangHomNay = donHangHomNay;

            return View();
        }




    }
}
