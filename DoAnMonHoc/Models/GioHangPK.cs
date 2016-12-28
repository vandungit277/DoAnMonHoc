using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnMonHoc.Models
{
    public class GioHangPK
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        public int _MA_PK { set; get; }
        public string _TEN_PK { set; get; }
        public string _HINHANH_PK { set; get; }
        public Double _DONGIA_PK { set; get; }
        public int _SL_PK { set; get; }
        public Double ThanhTien
        {
            get { return _SL_PK * _DONGIA_PK; }
        }
        public GioHangPK(int MA_PK)
        {
            _MA_PK = MA_PK;
            PHUKIEN pk = data.PHUKIENs.Single(n => n.MA_PK == _MA_PK);
            _TEN_PK = pk.TEN_PK;
            _HINHANH_PK = pk.HINHANH_PK;
            _DONGIA_PK = double.Parse(pk.GIABAN_PK.ToString());
            _SL_PK = 1;
        }
    }
}