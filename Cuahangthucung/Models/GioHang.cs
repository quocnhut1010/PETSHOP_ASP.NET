using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuahangthucung.Models
{
    public class GioHang
    {
        CuahangTCEntities1 db = new CuahangTCEntities1();
        public int iMaTC { get; set; }
        public string sTenTC { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }

        }
        public GioHang(int ms)
        {
            iMaTC = ms;
            THUCUNG s = db.THUCUNGs.Single(n => n.MaTC == iMaTC);
            sTenTC = s.TenTC;
            sAnhBia = s.AnhBia;


            dDonGia = double.Parse(s.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}