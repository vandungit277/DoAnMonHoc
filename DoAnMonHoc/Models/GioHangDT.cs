using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnMonHoc.Models
{
    public class GioHangDT
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        public int _MA_DT { set; get; }
        public string _TEN_DT { set; get; }
        public string _HINHANH_DT { set; get; }
        public Double _DONGIA_DT { set; get; }
        public int _SL_DT { set; get; }
        public Double ThanhTien
        {
            get { return _SL_DT * _DONGIA_DT; }
        }
        public GioHangDT(int MA_DT)
        {
            _MA_DT = MA_DT;
            DIENTHOAI dt = data.DIENTHOAIs.Single(n => n.MA_DT == _MA_DT);
            _TEN_DT = dt.TEN_DT;
            _HINHANH_DT = dt.HINHANH_DT;
            _DONGIA_DT = double.Parse(dt.GIABAN_DT.ToString());
            _SL_DT = 1;
        }
    }
}