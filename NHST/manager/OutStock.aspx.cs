using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class OutStock1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnXuatKho.Text = "<a href=\"javascript:;\" id=\"xuatkhotatca\" style=\"display: none\" onclick=\"xuatkhotatcakien();\" class=\"btn waves-effect modal-trigger mt-1\">Xuất kho tất cả kiện</a>\r\n";
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if(ac.RoleID == 9)
            {
                btnXuatKho.Visible = false;
            }
        }

        #region Webservice mới
        [WebMethod]
        public static string getpackages(string barcode, string username, int Ware)
        {
            DateTime currentDate = DateTime.Now;
            username = username.Trim().ToLower();
            var accountInput = AccountController.GetByUsername(username);
            if (accountInput != null)
            {
                var lsmallpackage = SmallPackageController.GetByOrderCode(barcode);
                List<PackageGet> pgs = new List<PackageGet>();
                foreach (var temp in lsmallpackage)
                {
                    var smallpackage = temp;
                    if (smallpackage != null)
                    {
                        if (smallpackage.Status > 0)
                        {
                            int mID = Convert.ToInt32(smallpackage.MainOrderID);
                            int tID = Convert.ToInt32(smallpackage.TransportationOrderID);
                            if (mID > 0)
                            {
                                var mainorder = MainOrderController.GetAllByID(mID);
                                if (mainorder != null)
                                {
                                    if (mainorder.ReceivePlace.ToInt(0) == Ware || Ware == 0)
                                    {
                                        int UID = Convert.ToInt32(mainorder.UID);
                                        if (UID == accountInput.ID)
                                        {
                                            PackageGet p = new PackageGet();
                                            p.pID = smallpackage.ID;
                                            p.uID = UID;
                                            p.username = username;
                                            p.mID = mID;
                                            p.tID = 0;
                                            p.weight = Convert.ToDouble(smallpackage.Weight);
                                            p.status = Convert.ToInt32(smallpackage.Status);
                                            p.barcode = barcode;
                                            double day = 0;
                                            if (smallpackage.DateInLasteWareHouse != null)
                                            {
                                                DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                                TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                                day = Math.Floor(ts.TotalDays);
                                            }
                                            p.TotalDayInWarehouse = day;
                                            p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);
                                            if (mainorder.IsCheckProduct == true)
                                                p.kiemdem = "Có";
                                            else
                                                p.kiemdem = "Không";
                                            if (mainorder.IsPacked == true)
                                                p.donggo = "Có";
                                            else
                                                p.donggo = "Không";
                                            p.baohiem = "Không";
                                            p.OrderTypeString = "Đơn hàng mua hộ";
                                            p.OrderType = 1;
                                            double dai = 0;
                                            double rong = 0;
                                            double cao = 0;
                                            if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                            {
                                                dai = Convert.ToDouble(smallpackage.Length);
                                            }
                                            if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                            {
                                                rong = Convert.ToDouble(smallpackage.Width);
                                            }
                                            if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                            {
                                                cao = Convert.ToDouble(smallpackage.Height);
                                            }
                                            p.dai = dai;
                                            p.rong = rong;
                                            p.cao = cao;
                                            pgs.Add(p);
                                            //JavaScriptSerializer serializer = new JavaScriptSerializer();
                                            //return serializer.Serialize(p);
                                        }
                                    }
                                }
                            }
                            else if (tID > 0)
                            {
                                var t = TransportationOrderController.GetByID(tID);
                                if (t != null)
                                {
                                    int UID = Convert.ToInt32(t.UID);
                                    if (UID == accountInput.ID)
                                    {
                                        PackageGet p = new PackageGet();
                                        p.pID = smallpackage.ID;
                                        p.uID = UID;
                                        p.username = username;
                                        p.mID = 0;
                                        p.tID = tID;
                                        p.weight = Convert.ToDouble(smallpackage.Weight);
                                        p.status = Convert.ToInt32(smallpackage.Status);
                                        p.barcode = barcode;
                                        double day = 0;
                                        if (smallpackage.DateInLasteWareHouse != null)
                                        {
                                            DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                            TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                            day = Math.Floor(ts.TotalDays);
                                        }
                                        p.TotalDayInWarehouse = day;
                                        p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);

                                        if (smallpackage.IsCheckProduct == true)
                                            p.kiemdem = "Có";
                                        else
                                            p.kiemdem = "Không";

                                        if (smallpackage.IsPackaged == true)
                                            p.donggo = "Có";
                                        else
                                            p.donggo = "Không";

                                        if (smallpackage.IsInsurrance == true)
                                            p.baohiem = "Có";
                                        else
                                            p.baohiem = "Không";

                                        p.OrderTypeString = "Đơn hàng VC hộ";
                                        p.OrderType = 2;
                                        double dai = 0;
                                        double rong = 0;
                                        double cao = 0;
                                        if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                        {
                                            dai = Convert.ToDouble(smallpackage.Length);
                                        }
                                        if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                        {
                                            rong = Convert.ToDouble(smallpackage.Width);
                                        }
                                        if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                        {
                                            cao = Convert.ToDouble(smallpackage.Height);
                                        }
                                        p.dai = dai;
                                        p.rong = rong;
                                        p.cao = cao;
                                        //JavaScriptSerializer serializer = new JavaScriptSerializer();
                                        //return serializer.Serialize(p);
                                        pgs.Add(p);
                                    }
                                }
                            }
                            else
                            {
                                PackageGet p = new PackageGet();
                                p.pID = smallpackage.ID;
                                p.uID = 0;
                                p.username = "";
                                p.mID = 0;
                                p.tID = 0;
                                p.weight = Convert.ToDouble(smallpackage.Weight);
                                p.status = Convert.ToInt32(smallpackage.Status);
                                p.barcode = barcode;
                                double day = 0;
                                if (smallpackage.DateInLasteWareHouse != null)
                                {
                                    DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                    TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                    day = Math.Floor(ts.TotalDays);
                                }
                                p.TotalDayInWarehouse = day;
                                p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);
                                p.kiemdem = "Không";
                                p.donggo = "Không";
                                p.baohiem = "Không";
                                p.OrderTypeString = "Chưa xác định";
                                p.OrderType = 3;
                                double dai = 0;
                                double rong = 0;
                                double cao = 0;
                                if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                {
                                    dai = Convert.ToDouble(smallpackage.Length);
                                }
                                if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                {
                                    rong = Convert.ToDouble(smallpackage.Width);
                                }
                                if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                {
                                    cao = Convert.ToDouble(smallpackage.Height);
                                }
                                p.dai = dai;
                                p.rong = rong;
                                p.cao = cao;
                                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                                //return serializer.Serialize(p);
                                pgs.Add(p);
                            }
                        }

                    }
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(pgs);
            }
            else
            {
                return "notexistuser";
            }

            return "none";
        }

        [WebMethod]
        public static string getpackagesbyo(int orderID, string username, int type, int Ware)
        {
            DateTime currentDate = DateTime.Now;
            var account = AccountController.GetByUsername(username);
            if (account != null)
            {
                int UID = account.ID;
                if (orderID > 0)
                {
                    if (type == 1)
                    {
                        var mainorder = MainOrderController.GetAllByUIDAndID(UID, orderID);
                        if (mainorder != null)
                        {
                            if (mainorder.ReceivePlace.ToInt(0) == Ware || Ware == 0)
                            {
                                int mID = mainorder.ID;
                                var smallpackages = SmallPackageController.GetByMainOrderID(mainorder.ID);
                                if (smallpackages.Count > 0)
                                {
                                    List<PackageGet> pgs = new List<PackageGet>();
                                    foreach (var smallpackage in smallpackages)
                                    {
                                        PackageGet p = new PackageGet();
                                        p.pID = smallpackage.ID;
                                        p.uID = UID;
                                        p.username = username;
                                        p.mID = mID;
                                        p.tID = 0;
                                        p.weight = Convert.ToDouble(smallpackage.Weight);
                                        p.status = Convert.ToInt32(smallpackage.Status);
                                        p.barcode = smallpackage.OrderTransactionCode;
                                        double day = 0;
                                        if (smallpackage.DateInLasteWareHouse != null)
                                        {
                                            DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                            TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                            day = Math.Floor(ts.TotalDays);
                                        }
                                        p.TotalDayInWarehouse = day;
                                        p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);
                                        if (mainorder.IsCheckProduct == true)
                                            p.kiemdem = "Có";
                                        else
                                            p.kiemdem = "Không";
                                        if (mainorder.IsPacked == true)
                                            p.donggo = "Có";
                                        else
                                            p.donggo = "Không";
                                        p.baohiem = "Không";
                                        p.OrderTypeString = "Đơn hàng mua hộ";
                                        p.OrderType = 1;
                                        double dai = 0;
                                        double rong = 0;
                                        double cao = 0;
                                        if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                        {
                                            dai = Convert.ToDouble(smallpackage.Length);
                                        }
                                        if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                        {
                                            rong = Convert.ToDouble(smallpackage.Width);
                                        }
                                        if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                        {
                                            cao = Convert.ToDouble(smallpackage.Height);
                                        }
                                        p.dai = dai;
                                        p.rong = rong;
                                        p.cao = cao;
                                        pgs.Add(p);
                                    }
                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    return serializer.Serialize(pgs);
                                }
                            }
                        }
                    }
                    else if (type == 2)
                    {
                        var trs = TransportationOrderController.GetByIDAndUID(orderID, UID);
                        if (trs != null)
                        {
                            if (trs.WarehouseID == Ware || Ware == 0)
                            {
                                int tID = trs.ID;
                                var smallpackages = SmallPackageController.GetByTransportationOrderID(tID);
                                if (smallpackages.Count > 0)
                                {
                                    List<PackageGet> pgs = new List<PackageGet>();
                                    foreach (var smallpackage in smallpackages)
                                    {
                                        PackageGet p = new PackageGet();
                                        p.pID = smallpackage.ID;
                                        p.uID = UID;
                                        p.username = username;
                                        p.mID = 0;
                                        p.tID = tID;
                                        p.weight = Convert.ToDouble(smallpackage.Weight);
                                        p.status = Convert.ToInt32(smallpackage.Status);
                                        p.barcode = smallpackage.OrderTransactionCode;
                                        double day = 0;
                                        if (smallpackage.DateInLasteWareHouse != null)
                                        {
                                            DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                            TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                            day = Math.Floor(ts.TotalDays);
                                        }
                                        p.TotalDayInWarehouse = day;
                                        p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);

                                        if (smallpackage.IsCheckProduct == true)
                                            p.kiemdem = "Có";
                                        else
                                            p.kiemdem = "Không";
                                        if (smallpackage.IsPackaged == true)
                                            p.donggo = "Có";
                                        else
                                            p.donggo = "Không";

                                        if (smallpackage.IsInsurrance == true)
                                            p.baohiem = "Có";
                                        else
                                            p.baohiem = "Không";

                                        p.OrderTypeString = "Đơn hàng VC hộ";
                                        p.OrderType = 2;
                                        double dai = 0;
                                        double rong = 0;
                                        double cao = 0;
                                        if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                        {
                                            dai = Convert.ToDouble(smallpackage.Length);
                                        }
                                        if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                        {
                                            rong = Convert.ToDouble(smallpackage.Width);
                                        }
                                        if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                        {
                                            cao = Convert.ToDouble(smallpackage.Height);
                                        }
                                        p.dai = dai;
                                        p.rong = rong;
                                        p.cao = cao;
                                        pgs.Add(p);
                                    }
                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    return serializer.Serialize(pgs);
                                }
                            }
                        }
                    }
                    else
                    {
                        var mainorder = MainOrderController.GetAllByUIDAndID(UID, orderID);
                        if (mainorder != null)
                        {
                            if (mainorder.ReceivePlace.ToInt(0) == Ware || Ware == 0)
                            {
                                int mID = mainorder.ID;
                                var smallpackages = SmallPackageController.GetByMainOrderID(mainorder.ID);
                                if (smallpackages.Count > 0)
                                {
                                    List<PackageGet> pgs = new List<PackageGet>();
                                    foreach (var smallpackage in smallpackages)
                                    {
                                        PackageGet p = new PackageGet();
                                        p.pID = smallpackage.ID;
                                        p.uID = UID;
                                        p.username = username;
                                        p.mID = mID;
                                        p.tID = 0;
                                        p.weight = Convert.ToDouble(smallpackage.Weight);
                                        p.status = Convert.ToInt32(smallpackage.Status);
                                        p.barcode = smallpackage.OrderTransactionCode;
                                        double day = 0;
                                        if (smallpackage.DateInLasteWareHouse != null)
                                        {
                                            DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                            TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                            day = Math.Floor(ts.TotalDays);
                                        }
                                        p.TotalDayInWarehouse = day;
                                        p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);
                                        if (mainorder.IsCheckProduct == true)
                                            p.kiemdem = "Có";
                                        else
                                            p.kiemdem = "Không";
                                        if (mainorder.IsPacked == true)
                                            p.donggo = "Có";
                                        else
                                            p.donggo = "Không";
                                        p.baohiem = "Không";
                                        p.OrderTypeString = "Đơn hàng mua hộ";
                                        p.OrderType = 1;
                                        double dai = 0;
                                        double rong = 0;
                                        double cao = 0;
                                        if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                        {
                                            dai = Convert.ToDouble(smallpackage.Length);
                                        }
                                        if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                        {
                                            rong = Convert.ToDouble(smallpackage.Width);
                                        }
                                        if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                        {
                                            cao = Convert.ToDouble(smallpackage.Height);
                                        }
                                        p.dai = dai;
                                        p.rong = rong;
                                        p.cao = cao;
                                        pgs.Add(p);
                                    }
                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    return serializer.Serialize(pgs);
                                }
                            }
                        }
                        var trs = TransportationOrderController.GetByIDAndUID(orderID, UID);
                        if (trs != null)
                        {
                            if (trs.WarehouseID == Ware || Ware == 0)
                            {
                                int tID = trs.ID;
                                var smallpackages = SmallPackageController.GetByTransportationOrderID(tID);
                                if (smallpackages.Count > 0)
                                {
                                    List<PackageGet> pgs = new List<PackageGet>();
                                    foreach (var smallpackage in smallpackages)
                                    {
                                        PackageGet p = new PackageGet();
                                        p.pID = smallpackage.ID;
                                        p.uID = UID;
                                        p.username = username;
                                        p.mID = 0;
                                        p.tID = tID;
                                        p.weight = Convert.ToDouble(smallpackage.Weight);
                                        p.status = Convert.ToInt32(smallpackage.Status);
                                        p.barcode = smallpackage.OrderTransactionCode;
                                        double day = 0;
                                        if (smallpackage.DateInLasteWareHouse != null)
                                        {
                                            DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                            TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                            day = Math.Floor(ts.TotalDays);
                                        }
                                        p.TotalDayInWarehouse = day;
                                        p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);
                                        p.kiemdem = "Không";
                                        p.donggo = "Không";
                                        p.OrderTypeString = "Đơn hàng VC hộ";
                                        p.OrderType = 2;
                                        double dai = 0;
                                        double rong = 0;
                                        double cao = 0;
                                        if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                        {
                                            dai = Convert.ToDouble(smallpackage.Length);
                                        }
                                        if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                        {
                                            rong = Convert.ToDouble(smallpackage.Width);
                                        }
                                        if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                        {
                                            cao = Convert.ToDouble(smallpackage.Height);
                                        }
                                        p.dai = dai;
                                        p.rong = rong;
                                        p.cao = cao;
                                        pgs.Add(p);
                                    }
                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    return serializer.Serialize(pgs);
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (type == 1)
                    {
                        var mainorders = MainOrderController.GetAllByUID(UID);
                        if (mainorders.Count > 0)
                        {
                            List<PackageGet> pgs = new List<PackageGet>();
                            foreach (var mainorder in mainorders)
                            {
                                if (mainorder.ReceivePlace.ToInt(0) == Ware || Ware == 0)
                                {
                                    int mID = mainorder.ID;
                                    var smallpackages = SmallPackageController.GetByMainOrderIDAndStatus(mainorder.ID, 3);
                                    if (smallpackages.Count > 0)
                                    {

                                        foreach (var smallpackage in smallpackages)
                                        {
                                            PackageGet p = new PackageGet();
                                            p.pID = smallpackage.ID;
                                            p.uID = UID;
                                            p.username = username;
                                            p.mID = mID;
                                            p.tID = 0;
                                            p.weight = Convert.ToDouble(smallpackage.Weight);
                                            p.status = Convert.ToInt32(smallpackage.Status);
                                            p.barcode = smallpackage.OrderTransactionCode;
                                            double day = 0;
                                            if (smallpackage.DateInLasteWareHouse != null)
                                            {
                                                DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                                TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                                day = Math.Floor(ts.TotalDays);
                                            }
                                            p.TotalDayInWarehouse = day;
                                            p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);
                                            if (mainorder.IsCheckProduct == true)
                                                p.kiemdem = "Có";
                                            else
                                                p.kiemdem = "Không";
                                            if (mainorder.IsPacked == true)
                                                p.donggo = "Có";
                                            else
                                                p.donggo = "Không";
                                            p.baohiem = "Không";
                                            p.OrderTypeString = "Đơn hàng mua hộ";
                                            p.OrderType = 1;
                                            double dai = 0;
                                            double rong = 0;
                                            double cao = 0;
                                            if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                            {
                                                dai = Convert.ToDouble(smallpackage.Length);
                                            }
                                            if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                            {
                                                rong = Convert.ToDouble(smallpackage.Width);
                                            }
                                            if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                            {
                                                cao = Convert.ToDouble(smallpackage.Height);
                                            }
                                            p.dai = dai;
                                            p.rong = rong;
                                            p.cao = cao;
                                            pgs.Add(p);
                                        }

                                    }
                                }

                            }
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            return serializer.Serialize(pgs);
                        }
                    }
                    else if (type == 2)
                    {
                        var trss = TransportationOrderController.GetByUID(UID);
                        if (trss.Count > 0)
                        {
                            List<PackageGet> pgs = new List<PackageGet>();
                            foreach (var trs in trss)
                            {
                                if (trs.WarehouseID == Ware || Ware == 0)
                                {
                                    int tID = trs.ID;
                                    var smallpackages = SmallPackageController.GetByTransportationOrderIDAndStatus(tID, 3);
                                    if (smallpackages.Count > 0)
                                    {

                                        foreach (var smallpackage in smallpackages)
                                        {
                                            PackageGet p = new PackageGet();
                                            p.pID = smallpackage.ID;
                                            p.uID = UID;
                                            p.username = username;
                                            p.mID = 0;
                                            p.tID = tID;
                                            p.weight = Convert.ToDouble(smallpackage.Weight);
                                            p.status = Convert.ToInt32(smallpackage.Status);
                                            p.barcode = smallpackage.OrderTransactionCode;
                                            double day = 0;
                                            if (smallpackage.DateInLasteWareHouse != null)
                                            {
                                                DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                                TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                                day = Math.Floor(ts.TotalDays);
                                            }
                                            p.TotalDayInWarehouse = day;
                                            p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);

                                            if (smallpackage.IsCheckProduct == true)
                                                p.kiemdem = "Có";
                                            else
                                                p.kiemdem = "Không";
                                            if (smallpackage.IsPackaged == true)
                                                p.donggo = "Có";
                                            else
                                                p.donggo = "Không";

                                            if (smallpackage.IsInsurrance == true)
                                                p.baohiem = "Có";
                                            else
                                                p.baohiem = "Không";

                                            p.OrderTypeString = "Đơn hàng VC hộ";
                                            p.OrderType = 2;
                                            double dai = 0;
                                            double rong = 0;
                                            double cao = 0;
                                            if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                            {
                                                dai = Convert.ToDouble(smallpackage.Length);
                                            }
                                            if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                            {
                                                rong = Convert.ToDouble(smallpackage.Width);
                                            }
                                            if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                            {
                                                cao = Convert.ToDouble(smallpackage.Height);
                                            }
                                            p.dai = dai;
                                            p.rong = rong;
                                            p.cao = cao;
                                            pgs.Add(p);
                                        }

                                    }
                                }

                            }
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            return serializer.Serialize(pgs);
                        }

                    }
                    else
                    {
                        var smallpackages = SmallPackageController.GetAllByUIDAndStatus(UID, 3);
                        if (smallpackages.Count > 0)
                        {
                            List<PackageGet> pgs = new List<PackageGet>();
                            foreach (var smallpackage in smallpackages)
                            {
                                int mID = Convert.ToInt32(smallpackage.MainOrderID);
                                int tID = Convert.ToInt32(smallpackage.TransportationOrderID);
                                PackageGet p = new PackageGet();
                                if (mID > 0)
                                {
                                    var mainorder = MainOrderController.GetAllByID(mID);
                                    if (mainorder != null)
                                    {
                                        if (mainorder.ReceivePlace.ToInt(0) == Ware || Ware == 0)
                                        {
                                            p.pID = smallpackage.ID;
                                            p.uID = UID;
                                            p.username = username;
                                            p.mID = mID;
                                            p.tID = tID;
                                            p.weight = Convert.ToDouble(smallpackage.Weight);
                                            p.status = Convert.ToInt32(smallpackage.Status);
                                            p.barcode = smallpackage.OrderTransactionCode;
                                            if (mainorder.IsCheckProduct == true)
                                                p.kiemdem = "Có";
                                            else
                                                p.kiemdem = "Không";
                                            if (mainorder.IsPacked == true)
                                                p.donggo = "Có";
                                            else
                                                p.donggo = "Không";
                                            p.baohiem = "Không";
                                            p.OrderTypeString = "Đơn hàng mua hộ";
                                            p.OrderType = 1;
                                            double day = 0;
                                            if (smallpackage.DateInLasteWareHouse != null)
                                            {
                                                DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                                TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                                day = Math.Floor(ts.TotalDays);
                                            }
                                            p.TotalDayInWarehouse = day;
                                            p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);
                                            double dai = 0;
                                            double rong = 0;
                                            double cao = 0;
                                            if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                            {
                                                dai = Convert.ToDouble(smallpackage.Length);
                                            }
                                            if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                            {
                                                rong = Convert.ToDouble(smallpackage.Width);
                                            }
                                            if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                            {
                                                cao = Convert.ToDouble(smallpackage.Height);
                                            }
                                            p.dai = dai;
                                            p.rong = rong;
                                            p.cao = cao;

                                            pgs.Add(p);
                                        }
                                    }

                                }
                            }
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            return serializer.Serialize(pgs);
                        }
                    }

                }

            }
            return "none";
        }

        [WebMethod]
        public static string addpackagetoprder(int ordertype, string username, int orderid, int pID)
        {
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            username = username.Trim().ToLower();
            var accountInput = AccountController.GetByUsername(username);
            if (accountInput != null)
            {
                int UID = accountInput.ID;
                if (ordertype == 1)
                {
                    var mainorder = MainOrderController.GetAllByUIDAndID(UID, orderid);
                    if (mainorder != null)
                    {
                        var small = SmallPackageController.GetByID(pID);
                        if (small != null)
                        {
                            int MainOrderCodeID = 0;
                            var lMainOrderCode = MainOrderCodeController.GetAllByMainOrderID(mainorder.ID);
                            if (lMainOrderCode.Count > 0)
                            {
                                MainOrderCodeID = lMainOrderCode[0].ID;
                            }

                            SmallPackageController.UpdateMainOrderID(small.ID, orderid);
                            SmallPackageController.UpdateMainOrderCodeID(small.ID, MainOrderCodeID);
                            #region update mainorder
                            int orderID = mainorder.ID;
                            int warehouse = mainorder.ReceivePlace.ToInt(1);
                            int shipping = Convert.ToInt32(mainorder.ShippingType);
                            int warehouseFrom = Convert.ToInt32(mainorder.FromPlace);

                            bool checkIsChinaCome = true;
                            double totalweight = 0;
                            var packages = SmallPackageController.GetByMainOrderID(mainorder.ID);
                            if (packages.Count > 0)
                            {
                                foreach (var p in packages)
                                {
                                    if (p.Status < 2)
                                        checkIsChinaCome = false;
                                    totalweight += Convert.ToDouble(p.Weight);
                                }
                            }
                            var usercreate = AccountController.GetByID(Convert.ToInt32(mainorder.UID));

                            double FeeWeight = 0;
                            double FeeWeightDiscount = 0;
                            double ckFeeWeight = 0;
                            ckFeeWeight = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeWeight.ToString());
                            double returnprice = 0;
                            double pricePerWeight = 0;
                            double finalPriceOfPackage = 0;
                            var smallpackage1 = SmallPackageController.GetByMainOrderID(orderID);
                            if (smallpackage1.Count > 0)
                            {
                                double totalWeight = 0;
                                foreach (var item in smallpackage1)
                                {

                                    double totalWeightCN = Convert.ToDouble(item.Weight);
                                    double totalWeightTT = 0;
                                    double pDai = Convert.ToDouble(item.Length);
                                    double pRong = Convert.ToDouble(item.Width);
                                    double pCao = Convert.ToDouble(item.Height);
                                    if (pDai > 0 && pRong > 0 && pCao > 0)
                                    {
                                        totalWeightTT = (pDai * pRong * pCao) / 6000;
                                    }
                                    if (totalWeightCN > totalWeightTT)
                                    {
                                        totalWeight += totalWeightCN;
                                    }
                                    else
                                    {
                                        totalWeight += totalWeightTT;
                                    }
                                }

                                var fee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(warehouseFrom, warehouse, shipping, false);
                                if (fee.Count > 0)
                                {
                                    foreach (var f in fee)
                                    {
                                        if (Convert.ToDouble(mainorder.PriceVND) > f.WeightFrom && Convert.ToDouble(mainorder.PriceVND) <= f.WeightTo)
                                        {
                                            pricePerWeight = Convert.ToDouble(f.Price);
                                            returnprice = totalWeight * Convert.ToDouble(f.Price);
                                            break;
                                        }
                                    }
                                }
                                foreach (var item in smallpackage1)
                                {
                                    double compareweight = 0;
                                    double compareSize = 0;

                                    double weight = Convert.ToDouble(item.Weight);
                                    compareweight = weight * pricePerWeight;

                                    double weigthTT = 0;
                                    double pDai = Convert.ToDouble(item.Length);
                                    double pRong = Convert.ToDouble(item.Width);
                                    double pCao = Convert.ToDouble(item.Height);
                                    if (pDai > 0 && pRong > 0 && pCao > 0)
                                    {
                                        weigthTT = (pDai * pRong * pCao) / 6000;
                                    }
                                    compareSize = weigthTT * pricePerWeight;

                                    if (compareweight >= compareSize)
                                    {
                                        finalPriceOfPackage += compareweight;
                                        SmallPackageController.UpdateTotalPrice(item.ID, compareweight);
                                    }
                                    else
                                    {
                                        finalPriceOfPackage += compareSize;
                                        SmallPackageController.UpdateTotalPrice(item.ID, compareSize);
                                    }
                                }
                            }
                            double currency = Convert.ToDouble(mainorder.CurrentCNYVN);
                            //FeeWeight = returnprice * currency;
                            returnprice = finalPriceOfPackage;
                            FeeWeight = returnprice;
                            FeeWeightDiscount = FeeWeight * ckFeeWeight / 100;
                            FeeWeight = FeeWeight - FeeWeightDiscount;

                            double FeeShipCN = Math.Floor(Convert.ToDouble(mainorder.FeeShipCN));
                            double FeeBuyPro = Convert.ToDouble(mainorder.FeeBuyPro);
                            double IsCheckProductPrice = Convert.ToDouble(mainorder.IsCheckProductPrice);
                            double IsPackedPrice = Convert.ToDouble(mainorder.IsPackedPrice);
                            double IsPriceSepcial = Convert.ToDouble(mainorder.IsCheckPriceSpecial);
                            double IsFastDeliveryPrice = Convert.ToDouble(mainorder.IsFastDeliveryPrice);
                            double InsuranceMoney = Math.Round(Convert.ToDouble(mainorder.InsuranceMoney), 0);
                            double isfastprice = 0;
                            if (mainorder.IsFastPrice.ToFloat(0) > 0)
                                isfastprice = Convert.ToDouble(mainorder.IsFastPrice);
                            double pricenvd = 0;
                            if (mainorder.PriceVND.ToFloat(0) > 0)
                                pricenvd = Convert.ToDouble(mainorder.PriceVND);
                            double Deposit = Convert.ToDouble(mainorder.Deposit);

                            double TotalPriceVND = FeeShipCN + FeeBuyPro + FeeWeight + IsCheckProductPrice + IsPackedPrice
                                                         + IsFastDeliveryPrice + isfastprice + pricenvd + InsuranceMoney + IsPriceSepcial;

                            MainOrderController.UpdateFee(mainorder.ID, Deposit.ToString(), FeeShipCN.ToString(), FeeBuyPro.ToString(), FeeWeight.ToString(),
                                IsCheckProductPrice.ToString(),
                                IsPackedPrice.ToString(), IsFastDeliveryPrice.ToString(), TotalPriceVND.ToString(), IsPriceSepcial.ToString());
                            MainOrderController.UpdateTotalWeight(mainorder.ID, totalweight.ToString(), totalweight.ToString());
                            var accChangeData = AccountController.GetByUsername(username_current);
                            if (accChangeData != null)
                            {
                                if (checkIsChinaCome == true)
                                {
                                    int MainorderID = mainorder.ID;
                                    //MainOrderController.UpdateStatus(mainorder.ID, Convert.ToInt32(mainorder.UID), 6);
                                    var smallpackages = SmallPackageController.GetByMainOrderID(MainorderID);
                                    if (smallpackages.Count > 0)
                                    {
                                        bool isChuaVekhoTQ = true;
                                        var sp_main = smallpackages.Where(s => s.IsTemp != true).ToList();
                                        var sp_support_isvekhotq = smallpackages.Where(s => s.IsTemp == true && s.Status >= 3).ToList();
                                        var sp_main_isvekhotq = smallpackages.Where(s => s.IsTemp != true && s.Status >= 3).ToList();
                                        double che = sp_support_isvekhotq.Count + sp_main_isvekhotq.Count;
                                        if (che >= sp_main.Count)
                                        {
                                            isChuaVekhoTQ = false;
                                        }
                                        if (isChuaVekhoTQ == false)
                                        {
                                            MainOrderController.UpdateStatus(MainorderID, Convert.ToInt32(mainorder.UID), 8);
                                        }
                                    }
                                    HistoryOrderChangeController.Insert(mainorder.ID, accChangeData.ID, accChangeData.Username, accChangeData.Username +
                                                       " đã đổi trạng thái đơn hàng ID là: " + mainorder.ID + ", là: Đã về kho đích", 8, currentDate);
                                }
                            }
                            #endregion
                            #region update package và lấy ra
                            var smallpackage = SmallPackageController.GetByID(pID);
                            {
                                PackageGet p = new PackageGet();
                                p.pID = smallpackage.ID;
                                p.uID = UID;
                                p.username = username;
                                p.mID = mainorder.ID;
                                p.tID = 0;
                                p.weight = Convert.ToDouble(smallpackage.Weight);
                                p.status = Convert.ToInt32(smallpackage.Status);
                                p.barcode = smallpackage.OrderTransactionCode;
                                double day = 0;
                                if (smallpackage.DateInLasteWareHouse != null)
                                {
                                    DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                    TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                    day = Math.Floor(ts.TotalDays);
                                }
                                p.TotalDayInWarehouse = day;
                                p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);
                                if (mainorder.IsCheckProduct == true)
                                    p.kiemdem = "Có";
                                else
                                    p.kiemdem = "Không";
                                if (mainorder.IsPacked == true)
                                    p.donggo = "Có";
                                else
                                    p.donggo = "Không";
                                p.OrderTypeString = "Đơn hàng mua hộ";
                                p.OrderType = 1;
                                double dai = 0;
                                double rong = 0;
                                double cao = 0;
                                if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                {
                                    dai = Convert.ToDouble(smallpackage.Length);
                                }
                                if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                {
                                    rong = Convert.ToDouble(smallpackage.Width);
                                }
                                if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                {
                                    cao = Convert.ToDouble(smallpackage.Height);
                                }
                                p.dai = dai;
                                p.rong = rong;
                                p.cao = cao;
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                return serializer.Serialize(p);
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    var transportation = TransportationOrderController.GetByIDAndUID(orderid, UID);
                    if (transportation != null)
                    {
                        int tID = transportation.ID;

                        #region Update package và lấy ra
                        var small = SmallPackageController.GetByID(pID);
                        if (small != null)
                        {

                            SmallPackageController.UpdateTransportationOrderID(small.ID, orderid);
                            #region Update đơn
                            double totalWeight = 0;
                            int warehouseFrom = Convert.ToInt32(transportation.WarehouseFromID);
                            int warehouseTo = Convert.ToInt32(transportation.WarehouseID);
                            int shippingType = Convert.ToInt32(transportation.ShippingTypeID);
                            int status = Convert.ToInt32(transportation.Status);
                            double currency = Convert.ToDouble(transportation.Currency);
                            double price = 0;
                            double pricePerWeight = 0;
                            double finalPriceOfPackage = 0;
                            bool isExist = false;
                            double totalprice = 0;
                            var smallpackages = SmallPackageController.GetByTransportationOrderID(tID);
                            if (smallpackages.Count > 0)
                            {
                                foreach (var s in smallpackages)
                                {
                                    //totalWeight += Convert.ToDouble(s.Weight);
                                    double totalWeightCN = Convert.ToDouble(s.Weight);
                                    double totalWeightTT = 0;

                                    double pDai = Convert.ToDouble(s.Length);
                                    double pRong = Convert.ToDouble(s.Width);
                                    double pCao = Convert.ToDouble(s.Height);
                                    if (pDai > 0 && pRong > 0 && pCao > 0)
                                    {
                                        totalWeightTT = (pDai * pRong * pCao) / 6000;
                                    }
                                    if (totalWeightCN > totalWeightTT)
                                    {
                                        totalWeight += totalWeightCN;
                                    }
                                    else
                                    {
                                        totalWeight += totalWeightTT;
                                    }
                                }
                                isExist = true;
                            }
                            else
                            {
                                var transportationDetail = TransportationOrderDetailController.GetByTransportationOrderID(tID);
                                if (transportationDetail.Count > 0)
                                {
                                    foreach (var p in transportationDetail)
                                    {
                                        totalWeight += Convert.ToDouble(p.Weight);
                                    }
                                }
                            }

                            var tf = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(warehouseFrom,
                                        warehouseTo, shippingType, true);

                            if (tf.Count > 0)
                            {
                                foreach (var w in tf)
                                {
                                    if (w.WeightFrom < totalWeight && totalWeight <= w.WeightTo)
                                    {
                                        pricePerWeight = Convert.ToDouble(w.Price);
                                        price = Convert.ToDouble(w.Price);
                                        break;
                                    }
                                }
                            }
                            foreach (var item in smallpackages)
                            {
                                double compareweight = 0;
                                double compareSize = 0;

                                double weight = Convert.ToDouble(item.Weight);
                                compareweight = weight * pricePerWeight;

                                double weigthTT = 0;
                                double pDai = Convert.ToDouble(item.Length);
                                double pRong = Convert.ToDouble(item.Width);
                                double pCao = Convert.ToDouble(item.Height);
                                if (pDai > 0 && pRong > 0 && pCao > 0)
                                {
                                    weigthTT = (pDai * pRong * pCao) / 6000;
                                }
                                compareSize = weigthTT * pricePerWeight;

                                if (compareweight >= compareSize)
                                {
                                    finalPriceOfPackage += compareweight;
                                    SmallPackageController.UpdateTotalPrice(item.ID, compareweight);
                                }
                                else
                                {
                                    finalPriceOfPackage += compareSize;
                                    SmallPackageController.UpdateTotalPrice(item.ID, compareSize);
                                }
                            }
                            //totalprice = price * totalWeight * currency;
                            //totalprice = Convert.ToDouble(rTotalPrice.Value);
                            //totalprice = price * totalWeight;
                            totalprice = finalPriceOfPackage;
                            TransportationOrderController.Update(tID, UID, transportation.Username, warehouseFrom, warehouseTo, shippingType,
                                    status, totalWeight, currency, totalprice, "", currentDate, username_current);
                            if (isExist == false)
                            {
                                var transportationDetail = TransportationOrderDetailController.GetByTransportationOrderID(tID);
                                if (transportationDetail.Count > 0)
                                {
                                    foreach (var p in transportationDetail)
                                    {
                                        SmallPackageController.InsertWithTransportationID(transportation.ID, 0, p.TransportationOrderCode, "",
                                            0, Convert.ToDouble(p.Weight), 0, 1, currentDate, username_current,
                                            Convert.ToInt32(transportation.UID), transportation.Username);
                                    }
                                }
                            }
                            if (status == 0)
                            {
                                var smallpacs = SmallPackageController.GetByTransportationOrderID(tID);
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.Delete(item.ID);
                                    }
                                }
                                double deposited = Convert.ToDouble(transportation.Deposited);
                                if (deposited > 0)
                                {
                                    var user_deposited = AccountController.GetByID(Convert.ToInt32(transportation.UID));
                                    if (user_deposited != null)
                                    {
                                        double wallet = Convert.ToDouble(user_deposited);
                                        double walletleft = wallet + deposited;
                                        AccountController.updateWallet(UID, walletleft, currentDate, username_current);
                                        HistoryPayWalletController.InsertTransportation(UID, username_current, 0, deposited,
                                        username_current + " nhận lại tiền của đơn hàng vận chuyển hộ: " + transportation.ID + ".",
                                        walletleft, 2, 11, currentDate, username_current, transportation.ID);
                                    }
                                }
                            }
                            else if (status == 1)
                            {
                                var smallpacs = SmallPackageController.GetByTransportationOrderID(tID);
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.Delete(item.ID);
                                    }
                                }
                            }
                            else if (status == 4)
                            {
                                var smallpacs = SmallPackageController.GetByTransportationOrderID(tID);
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.UpdateStatus(item.ID, 2, currentDate, username_current);
                                    }
                                }
                            }
                            else if (status == 5)
                            {
                                var smallpacs = SmallPackageController.GetByTransportationOrderID(tID);
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.UpdateStatus(item.ID, 3, currentDate, username_current);
                                    }
                                }
                            }
                            else if (status == 7)
                            {
                                var smallpacs = SmallPackageController.GetByTransportationOrderID(tID);
                                if (smallpacs.Count > 0)
                                {
                                    foreach (var item in smallpacs)
                                    {
                                        SmallPackageController.UpdateStatus(item.ID, 4, currentDate, username_current);
                                    }
                                }
                            }
                            #endregion
                            var smallpackage = SmallPackageController.GetByID(pID);
                            {
                                PackageGet p = new PackageGet();
                                p.pID = smallpackage.ID;
                                p.uID = UID;
                                p.username = username;
                                p.mID = 0;
                                p.tID = tID;
                                p.weight = Convert.ToDouble(smallpackage.Weight);
                                p.status = Convert.ToInt32(smallpackage.Status);
                                p.barcode = smallpackage.OrderTransactionCode;
                                double day = 0;
                                if (smallpackage.DateInLasteWareHouse != null)
                                {
                                    DateTime dateinwarehouse = Convert.ToDateTime(smallpackage.DateInLasteWareHouse);
                                    TimeSpan ts = currentDate.Subtract(dateinwarehouse);
                                    day = Math.Floor(ts.TotalDays);
                                }
                                p.TotalDayInWarehouse = day;
                                p.dateInWarehouse = string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse);
                                p.kiemdem = "Không";
                                p.donggo = "Không";
                                p.OrderTypeString = "Đơn hàng VC hộ";
                                p.OrderType = 2;
                                double dai = 0;
                                double rong = 0;
                                double cao = 0;
                                if (smallpackage.Length.ToString().ToFloat(0) > 0)
                                {
                                    dai = Convert.ToDouble(smallpackage.Length);
                                }
                                if (smallpackage.Width.ToString().ToFloat(0) > 0)
                                {
                                    rong = Convert.ToDouble(smallpackage.Width);
                                }
                                if (smallpackage.Height.ToString().ToFloat(0) > 0)
                                {
                                    cao = Convert.ToDouble(smallpackage.Height);
                                }
                                p.dai = dai;
                                p.rong = rong;
                                p.cao = cao;
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                return serializer.Serialize(p);
                            }
                        }
                        #endregion
                    }
                }
            }
            return "none";
        }
        #endregion

        public class Total
        {
            public string totalMoney { get; set; }
            public string MoneyUser { get; set; }
            public string Moneyset { get; set; }
            public string totalPackage { get; set; }
            public string totalPackageScan { get; set; }
        }
        public class PackageGet
        {
            public int pID { get; set; }
            public int mID { get; set; }
            public int tID { get; set; }
            public int uID { get; set; }
            public string username { get; set; }
            public double weight { get; set; }
            public int status { get; set; }
            public string kiemdem { get; set; }
            public string donggo { get; set; }
            public string baohiem { get; set; }
            public string barcode { get; set; }
            public string dateInWarehouse { get; set; }
            public string OrderTypeString { get; set; }
            public int OrderType { get; set; }
            public double TotalDayInWarehouse { get; set; }
            public double dai { get; set; }
            public double rong { get; set; }
            public double cao { get; set; }
        }

        public class OrderGet
        {
            public int ID { get; set; }
            public int MainorderID { get; set; }
            public int UID { get; set; }
            public string Username { get; set; }
            public double Wallet { get; set; }
            public string OrderShopCode { get; set; }
            public string BarCode { get; set; }
            public string TotalWeight { get; set; }
            public string TotalPriceVND { get; set; }
            public double TotalPriceVNDNum { get; set; }
            public int Status { get; set; }
            public int MainOrderStatus { get; set; }
            public string Kiemdem { get; set; }
            public string Donggo { get; set; }
        }

        protected void btnAllOutstock_Click(object sender, EventArgs e)
        {
            if (Session["userLoginSystem"] == null)
            {
            }
            else
            {
                string username_current = Session["userLoginSystem"].ToString();
                tbl_Account ac = AccountController.GetByUsername(username_current);
                if (ac.RoleID != 0 && ac.RoleID != 5 && ac.RoleID != 2)
                {

                }
                else
                {
                    DateTime currentDate = DateTime.Now;
                    string usernameout = hdfUsername.Value;
                    var acc = AccountController.GetByUsername(usernameout);
                    if (acc != null)
                    {
                        string fullname = "";
                        string phone = "";
                        var ai = AccountInfoController.GetByUserID(acc.ID);
                        if (ai != null)
                        {
                            fullname = ai.FirstName + " " + ai.LastName;
                            phone = ai.Phone;
                        }
                        string kq = OutStockSessionController.Insert(acc.ID, usernameout, fullname, phone, 0, currentDate, username_current);
                        if (kq.ToInt(0) > 0)
                        {
                            int ousID = kq.ToInt(0);
                            string listpack = hdfListPID.Value;
                            List<int> mainid = new List<int>();
                            string[] packs = listpack.Split('|');
                            for (int i = 0; i < packs.Length - 1; i++)
                            {
                                int smID = packs[i].ToInt(0);
                                var small = SmallPackageController.GetByID(smID);
                                if (small != null)
                                {
                                    OutStockSessionPackageController.Insert(ousID, small.ID, small.OrderTransactionCode,
                                        Convert.ToInt32(small.MainOrderID), Convert.ToInt32(small.TransportationOrderID),
                                        currentDate, username_current);
                                    mainid.Add(small.MainOrderID.Value);
                                }
                                OutStockSessionController.update_mainorderID(kq.ToInt(0), Convert.ToInt32(small.MainOrderID), currentDate, username_current);
                            }
                            List<int> ID = mainid.Distinct().ToList();
                            foreach (var item in ID)
                            {
                                if (MainOrderController.GetByID(item).DateToShip == null)
                                {
                                    MainOrderController.UpdateDateToShip(item, currentDate);
                                }
                                MainOrderController.UpdateStatusByID(item, 11);
                                HistoryOrderChangeController.Insert(item, ac.ID, username_current, username_current +
                                                       " đã đổi trạng thái đơn hàng ID là: " + item + ", là: Đang giao hàng", 8, currentDate);
                            }
                            LoadDataWare(ousID);
                            btncreateuser_Click(sender, e);
                        }
                    }
                }
            }
        }

        public void LoadDataWare(int ousID)
        {
            DateTime currentDate = DateTime.Now;
            string username_current = Session["userLoginSystem"].ToString();
            var UID = 0;
            int id = ousID;
            if (id > 0)
            {
                ViewState["id"] = id;
                var os = OutStockSessionController.GetByID(id);
                if (os != null)
                {
                    bool isShowButton = false;
                    double totalPriceMustPay = 0;
                    List<OrderPackage> ops = new List<OrderPackage>();
                    #region Đơn hàng mua hộ
                    var listmainorder = OutStockSessionPackageController.GetByOutStockSessionIDGroupByMainOrderID(id);
                    if (listmainorder.Count > 0)
                    {
                        foreach (var m in listmainorder)
                        {
                            var mainorder = MainOrderController.GetAllByID(Convert.ToInt32(m));
                            if (mainorder != null)
                            {
                                int mID = mainorder.ID;
                                UID = mainorder.UID.Value;
                                double totalPay = 0;
                                OrderPackage op = new OrderPackage();
                                op.OrderID = Convert.ToInt32(m);
                                op.OrderType = 1;
                                List<SmallpackageGet> sms = new List<SmallpackageGet>();
                                var packsmain = OutStockSessionPackageController.GetAllByOutStockSessionIDAndMainOrderID(id, Convert.ToInt32(m));
                                if (packsmain.Count > 0)
                                {
                                    foreach (var p in packsmain)
                                    {
                                        var sm = SmallPackageController.GetByID(Convert.ToInt32(p.SmallPackageID));
                                        if (sm != null)
                                        {
                                            SmallpackageGet pg = new SmallpackageGet();
                                            if (sm.Status == 4)
                                            {
                                                isShowButton = true;
                                            }
                                            double weight = Convert.ToDouble(sm.Weight);
                                            double weightCN = Convert.ToDouble(sm.Weight);
                                            double volume = 0;
                                            double dai = 0;
                                            double rong = 0;
                                            double cao = 0;
                                            if (sm.Length != null)
                                                dai = Convert.ToDouble(sm.Length);
                                            if (sm.Width != null)
                                                rong = Convert.ToDouble(sm.Width);
                                            if (sm.Height != null)
                                                cao = Convert.ToDouble(sm.Height);

                                            if (dai > 0 && rong > 0 && cao > 0)
                                                volume = dai * rong * cao / 1000000;

                                            weight = Math.Round(weight, 1);

                                            string packagecode = sm.OrderTransactionCode;
                                            int Status = Convert.ToInt32(sm.Status);
                                            double payInWarehouse = 0;

                                            pg.ID = sm.ID;
                                            pg.weight = weight;
                                            pg.volume = volume;

                                            pg.packagecode = packagecode;
                                            pg.Status = Status;
                                            var feeweightinware = InWarehousePriceController.GetAll();
                                            double payperday = 0;
                                            double maxday = 0;
                                            foreach (var item in feeweightinware)
                                            {
                                                if (item.WeightFrom < weight && weight <= item.WeightTo)
                                                {
                                                    maxday = Convert.ToDouble(item.MaxDay);
                                                    payperday = Convert.ToDouble(item.PricePay);
                                                    break;
                                                }
                                            }
                                            double totalDays = 0;
                                            if (sm.DateInLasteWareHouse != null)
                                            {
                                                DateTime diw = Convert.ToDateTime(sm.DateInLasteWareHouse);
                                                TimeSpan ts = currentDate.Subtract(diw);
                                                if (ts.TotalDays > 0)
                                                    totalDays = Math.Floor(ts.TotalDays);
                                            }

                                            double dayin = totalDays - maxday;
                                            if (dayin > 0)
                                            {
                                                payInWarehouse = dayin * payperday * weight;
                                            }
                                            pg.DateInWare = totalDays;
                                            totalPay += Math.Round(payInWarehouse, 0);
                                            pg.payInWarehouse = payInWarehouse;
                                            sms.Add(pg);
                                            SmallPackageController.UpdateWarehouseFeeDateOutWarehouse(sm.ID, payInWarehouse, currentDate);
                                            OutStockSessionPackageController.update(p.ID, currentDate, totalDays, payInWarehouse);
                                        }

                                    }
                                }
                                totalPay = Math.Round(totalPay, 0);
                                op.totalPrice = totalPay;
                                op.smallpackages = sms;
                                double mustpay = 0;
                                bool isPay = false;
                                MainOrderController.UpdateFeeWarehouse(mID, totalPay);
                                var ma = MainOrderController.GetAllByID(mID);
                                if (ma != null)
                                {
                                    double totalPriceVND = Math.Round(Convert.ToDouble(ma.TotalPriceVND), 0);
                                    double deposited = Math.Round(Convert.ToDouble(ma.Deposit), 0);
                                    double totalmustpay = Math.Round(totalPriceVND + totalPay, 0);
                                    double totalleftpay = Math.Round(totalmustpay - deposited, 0);
                                    if (totalmustpay <= deposited)
                                    {
                                        isPay = true;
                                    }
                                    else
                                    {
                                        double totalleft = Math.Round(totalmustpay - deposited, 0);
                                        if (totalleft > 100)
                                        {
                                            MainOrderController.UpdateStatus(mID, Convert.ToInt32(ma.UID), 11);
                                            mustpay = totalleftpay;
                                        }
                                        else
                                        {
                                            isPay = true;
                                        }
                                    }
                                }
                                if (isShowButton == false)
                                {
                                    if (isPay == false)
                                    {
                                        isShowButton = false;
                                    }
                                }
                                op.totalMustPay = mustpay;
                                op.isPay = isPay;
                                ops.Add(op);
                            }
                        }
                    }
                    #endregion
                    #region Đơn hàng VC hộ
                    var listtransportation = OutStockSessionPackageController.GetByOutStockSessionIDGroupByTransportationID(id);
                    if (listtransportation.Count > 0)
                    {
                        foreach (var t in listtransportation)
                        {
                            int tID = Convert.ToInt32(t);
                            var tran = TransportationOrderController.GetByID(tID);
                            if (tran != null)
                            {
                                double totalPay = 0;
                                OrderPackage op = new OrderPackage();
                                op.OrderID = tID;
                                op.OrderType = 2;
                                List<SmallpackageGet> sms = new List<SmallpackageGet>();
                                var packsmain = OutStockSessionPackageController.GetAllByOutStockSessionIDAndTransporationID(id, tID);
                                if (packsmain.Count > 0)
                                {
                                    foreach (var p in packsmain)
                                    {
                                        var sm = SmallPackageController.GetByID(Convert.ToInt32(p.SmallPackageID));
                                        if (sm != null)
                                        {
                                            SmallpackageGet pg = new SmallpackageGet();
                                            if (sm.Status != 3)
                                            {
                                                isShowButton = false;
                                            }
                                            double weight = Convert.ToDouble(sm.Weight);
                                            double weightCN = Convert.ToDouble(sm.Weight);
                                            double volume = 0;
                                            double dai = 0;
                                            double rong = 0;
                                            double cao = 0;
                                            if (sm.Length != null)
                                                dai = Convert.ToDouble(sm.Length);
                                            if (sm.Width != null)
                                                rong = Convert.ToDouble(sm.Width);
                                            if (sm.Height != null)
                                                cao = Convert.ToDouble(sm.Height);

                                            if (dai > 0 && rong > 0 && cao > 0)
                                                volume = dai * rong * cao / 1000000;

                                            weight = Math.Round(weight, 1);
                                            string packagecode = sm.OrderTransactionCode;
                                            int Status = Convert.ToInt32(sm.Status);
                                            double payInWarehouse = 0;

                                            pg.ID = sm.ID;
                                            pg.weight = weight;
                                            pg.volume = volume;

                                            pg.packagecode = packagecode;
                                            pg.Status = Status;
                                            var feeweightinware = InWarehousePriceController.GetAll();
                                            double payperday = 0;
                                            double maxday = 0;
                                            foreach (var item in feeweightinware)
                                            {
                                                if (item.WeightFrom < weight && weight <= item.WeightTo)
                                                {
                                                    maxday = Convert.ToDouble(item.MaxDay);
                                                    payperday = Convert.ToDouble(item.PricePay);
                                                    break;
                                                }
                                            }
                                            double totalDays = 0;
                                            if (sm.DateInLasteWareHouse != null)
                                            {
                                                DateTime diw = Convert.ToDateTime(sm.DateInLasteWareHouse);
                                                TimeSpan ts = currentDate.Subtract(diw);
                                                if (ts.TotalDays > 0)
                                                    totalDays = Math.Floor(ts.TotalDays);
                                            }
                                            double dayin = totalDays - maxday;
                                            if (dayin > 0)
                                            {
                                                payInWarehouse = dayin * payperday * weight;
                                            }
                                            totalPay += Math.Round(payInWarehouse, 0);
                                            pg.DateInWare = totalDays;
                                            pg.payInWarehouse = payInWarehouse;
                                            sms.Add(pg);
                                            SmallPackageController.UpdateWarehouseFeeDateOutWarehouse(sm.ID, payInWarehouse, currentDate);
                                            OutStockSessionPackageController.update(p.ID, currentDate, totalDays, payInWarehouse);
                                        }
                                    }
                                }
                                totalPay = Math.Round(totalPay, 0);
                                op.totalPrice = totalPay;
                                op.smallpackages = sms;
                                double mustpay = 0;
                                bool isPay = false;
                                TransportationOrderController.UpdateWarehouseFee(tID, totalPay);
                                var tr = TransportationOrderController.GetByID(tID);
                                if (tr != null)
                                {
                                    double totalPriceVND = Math.Round(Convert.ToDouble(tr.TotalPrice), 0);
                                    double deposited = Math.Round(Convert.ToDouble(tr.Deposited), 0);
                                    double totalmustpay = Math.Round(totalPriceVND + totalPay, 0);
                                    double totalleftpay = Math.Round(totalmustpay - deposited, 0);
                                    if (totalmustpay <= deposited)
                                    {
                                        isPay = true;
                                    }
                                    else
                                    {
                                        double totalleft = Math.Round(totalmustpay - deposited, 0);
                                        if (totalleft > 100)
                                        {
                                            TransportationOrderController.UpdateStatus(tID, 5, currentDate, username_current);
                                            mustpay = Math.Round(totalleftpay, 0);
                                        }
                                        else
                                        {
                                            isPay = true;
                                        }

                                    }
                                }
                                if (isShowButton == true)
                                {
                                    if (isPay == false)
                                    {
                                        isShowButton = false;
                                    }
                                }
                                op.totalMustPay = Math.Round(mustpay, 0);
                                op.isPay = isPay;
                                ops.Add(op);
                            }
                        }
                    }
                    #endregion
                    #region Render Data
                    txtFullname.Text = os.FullName;
                    txtPhone.Text = os.Phone;
                    var ac = AccountController.GetByID(UID);
                    StringBuilder html = new StringBuilder();
                    StringBuilder htmlPrint = new StringBuilder();
                    if (ops.Count > 0)
                    {
                        foreach (var o in ops)
                        {
                            int orderType = o.OrderType;
                            bool isPay = o.isPay;
                            var listpackages = o.smallpackages;
                            totalPriceMustPay += o.totalMustPay;


                            htmlPrint.Append("<article class=\"pane-primary\" style=\"color:#000\">");
                            if (orderType == 1)
                            {
                                htmlPrint.Append("   <div class=\"heading\"><h3 class=\"lb\" style=\"color:#000\">Đơn hàng mua hộ: <span style=\"text-align:right\">#" + o.OrderID + "</span></h3></div>");
                            }
                            else
                            {
                                htmlPrint.Append("   <div class=\"heading\"><h3 class=\"lb\" style=\"color:#000\">Đơn hàng VC hộ: <span style=\"text-align:right\">#" + o.OrderID + "</span></h3></div>");
                            }

                            htmlPrint.Append("   <article class=\"pane-primary\">");
                            htmlPrint.Append("       <table class=\"rgMasterTable normal-table full-width\" style=\"text-align:center\">");
                            htmlPrint.Append("           <tr>");
                            htmlPrint.Append("               <th style=\"color:#000\">Mã kiện</th>");
                            htmlPrint.Append("               <th style=\"color:#000\">Cân nặng (kg)</th>");
                            htmlPrint.Append("               <th style=\"color:#000\">Kích thước</th>");
                            htmlPrint.Append("               <th style=\"color:#000\">Số khối</th>");
                            htmlPrint.Append("               <th style=\"color:#000\">Ngày lưu kho (ngày)</th>");
                            htmlPrint.Append("               <th style=\"color:#000\">Thành tiền</th>");
                            htmlPrint.Append("           </tr>");

                            foreach (var p in listpackages)
                            {
                                var small = SmallPackageController.GetByID(p.ID);
                                double dai = 0;
                                double rong = 0;
                                double cao = 0;
                                if (small.Length.ToString().ToFloat(0) > 0)
                                {
                                    dai = Convert.ToDouble(small.Length);
                                }
                                if (small.Width.ToString().ToFloat(0) > 0)
                                {
                                    rong = Convert.ToDouble(small.Width);
                                }
                                if (small.Height.ToString().ToFloat(0) > 0)
                                {
                                    cao = Convert.ToDouble(small.Height);
                                }
                                htmlPrint.Append("           <tr>");
                                htmlPrint.Append("               <td>" + p.packagecode + "</td>");
                                htmlPrint.Append("               <td>" + p.weight + "</td>");
                                htmlPrint.Append("               <td><p><span>d: " + dai + "</span> <b>x</b> <span>r: " + rong + "</span><b>x</b> <span>c: " + cao + "</span></p></td>");
                                htmlPrint.Append("               <td>" + p.volume + "</td>");
                                htmlPrint.Append("               <td>" + p.DateInWare + "</td>");
                                htmlPrint.Append("               <td><span>" + string.Format("{0:N0}", p.payInWarehouse) + " vnđ</span></td>");
                                htmlPrint.Append("           </tr>");
                            }
                            var mo = MainOrderController.GetByID(o.OrderID);
                            htmlPrint.Append("           <tr style=\"font-size: 15px;\">");
                            htmlPrint.Append("               <td colspan=\"5\"><span style=\"font-weight: 500;\">Tổng tiền lưu kho</span></td>");
                            htmlPrint.Append("               <td><span style=\"font-weight: 500;\">" + string.Format("{0:N0}", o.totalPrice) + " vnđ</span></td>");
                            htmlPrint.Append("           </tr>");
                            htmlPrint.Append("           <tr style=\"font-size: 15px;\">");
                            htmlPrint.Append("               <td colspan=\"5\"><span style=\"font-weight: 500;\">Phí mua hàng</span></td>");
                            htmlPrint.Append("               <td><span style=\"font-weight: 500;\">" + string.Format("{0:N0}", Convert.ToDouble(mo.FeeBuyPro)) + " vnđ</span></td>");
                            htmlPrint.Append("           </tr>");
                            htmlPrint.Append("           <tr style=\"font-size: 15px;\">");
                            htmlPrint.Append("               <td colspan=\"5\"><span style=\"font-weight: 500;\">Phí vận chuyển</span></td>");
                            htmlPrint.Append("               <td><span style=\"font-weight: 500;\">" + string.Format("{0:N0}", Convert.ToDouble(mo.FeeWeight)) + " vnđ</span></td>");
                            htmlPrint.Append("           </tr>");
                            htmlPrint.Append("           <tr style=\"font-size: 15px; \">");
                            htmlPrint.Append("               <td colspan=\"5\"><span style=\"font-weight: 500;\">Phí ship Trung Quốc</span></td>");
                            htmlPrint.Append("               <td><span style=\"font-weight: 500;\">" + string.Format("{0:N0}", Convert.ToDouble(mo.FeeShipCN)) + " vnđ</span></td>");
                            htmlPrint.Append("           </tr>");
                            htmlPrint.Append("       </table>");
                            htmlPrint.Append("   </article>");
                            htmlPrint.Append("</article>");
                        }
                        htmlPrint.Append("<div class=\"row\">");
                        htmlPrint.Append("<div class=\"input-field col s12 l4\">");
                        htmlPrint.Append("<p style=\"font-size: 16px; font-weight: bold;\">");
                        htmlPrint.Append("Tổng tiền cần thanh toán: <span style=\"color: #F64302;\">" + string.Format("{0:N0}", totalPriceMustPay));
                        htmlPrint.Append(" VNĐ</span></p>");
                        htmlPrint.Append("</div>");
                        htmlPrint.Append("<div class=\"input-field col s12 l4\">");
                        htmlPrint.Append("<p style=\"font-size: 16px; font-weight: bold;\">");
                        htmlPrint.Append("Số dư của khách: <span style=\"color: #F64302;\">" + string.Format("{0:N0}", ac.Wallet));
                        htmlPrint.Append(" VNĐ</span></p>");
                        htmlPrint.Append("</div>");
                        htmlPrint.Append("<div class=\"input-field col s12 l4\">");
                        htmlPrint.Append("<p style=\"font-size: 16px; font-weight: bold;\">");
                        htmlPrint.Append("Số tiền cần nạp thêm: <span style=\"color: #F64302;\">" + string.Format("{0:N0}", totalPriceMustPay - ac.Wallet > 0 ? totalPriceMustPay - ac.Wallet : 0));
                        htmlPrint.Append(" VNĐ</span></p>");
                        htmlPrint.Append("</div>");
                        htmlPrint.Append("</div>");
                        ViewState["content"] = htmlPrint.ToString();
                    }
                    #endregion
                    if (totalPriceMustPay > 0)
                    {
                        OutStockSessionController.updateTotalPay(id, totalPriceMustPay);
                    }
                }
            }

        }

        public class OrderPackage
        {
            public int OrderID { get; set; }
            public int OrderType { get; set; }
            public List<SmallpackageGet> smallpackages { get; set; }
            public double totalPrice { get; set; }
            public bool isPay { get; set; }
            public double totalMustPay { get; set; }
        }

        public class SmallpackageGet
        {
            public int ID { get; set; }
            public string packagecode { get; set; }
            public double weight { get; set; }
            public double volume { get; set; }
            public double DateInWare { get; set; }
            public int Status { get; set; }
            public double payInWarehouse { get; set; }

        }

        [WebMethod]
        public static string TotalPrice(string ListpackId, string username)
        {
            DateTime currentDate = DateTime.Now;
            string usernameout = username;
            var acc = AccountController.GetByUsername(usernameout);
            if (acc != null)
            {
                string listpack = ListpackId;
                Total totalPG = new Total();
                double moneys = 0;
                double totalPay = 0;
                List<int> MainId = new List<int>();
                string[] packs = listpack.Split('|');
                for (int i = 0; i < packs.Length - 1; i++)
                {
                    int smID = packs[i].ToInt(0);
                    var small = SmallPackageController.GetByID(smID);
                    if (small != null)
                    {
                        MainId.Add(small.MainOrderID.Value);
                        double dai = 0;
                        double rong = 0;
                        double cao = 0;
                        if (small.Length.ToString().ToFloat(0) > 0)
                        {
                            dai = Convert.ToDouble(small.Length);
                        }
                        if (small.Width.ToString().ToFloat(0) > 0)
                        {
                            rong = Convert.ToDouble(small.Width);
                        }
                        if (small.Height.ToString().ToFloat(0) > 0)
                        {
                            cao = Convert.ToDouble(small.Height);
                        }
                        double weight = 0;
                        double weightCN = Convert.ToDouble(small.Weight);
                        double weightKT = 0;

                        if (dai > 0 && rong > 0 && cao > 0)
                            weightKT = dai * rong * cao / 6000;
                        if (weightKT > 0)
                        {
                            if (weightKT > weightCN)
                            {
                                weight = weightKT;
                            }
                            else
                            {
                                weight = weightCN;
                            }
                        }
                        else
                        {
                            weight = weightCN;
                        }
                        weight = Math.Round(weight, 1);
                        double payInWarehouse = 0;
                        var feeweightinware = InWarehousePriceController.GetAll();
                        double payperday = 0;
                        double maxday = 0;
                        foreach (var item in feeweightinware)
                        {
                            if (item.WeightFrom < weight && weight <= item.WeightTo)
                            {
                                maxday = Convert.ToDouble(item.MaxDay);
                                payperday = Convert.ToDouble(item.PricePay);
                                break;
                            }
                        }
                        double totalDays = 0;
                        if (small.DateInLasteWareHouse != null)
                        {
                            DateTime diw = Convert.ToDateTime(small.DateInLasteWareHouse);
                            TimeSpan ts = currentDate.Subtract(diw);
                            if (ts.TotalDays > 0)
                                totalDays = Math.Floor(ts.TotalDays);
                        }

                        double dayin = totalDays - maxday;
                        if (dayin > 0)
                        {
                            payInWarehouse = dayin * payperday * weight;
                        }
                        totalPay += Math.Round(payInWarehouse, 0);
                    }
                }
                var IDmain = MainId.Distinct().ToList();
                foreach (var item in IDmain)
                {
                    var ma = MainOrderController.GetByID(item);
                    double totalPriceVND = Math.Round(Convert.ToDouble(ma.TotalPriceVND), 0);
                    double deposited = Math.Round(Convert.ToDouble(ma.Deposit), 0);
                    double totalmustpay = Math.Round(totalPriceVND + totalPay, 0);
                    moneys += Math.Round(totalmustpay - deposited, 0);
                }


                if (acc.Wallet - moneys >= 0)
                {
                    totalPG.Moneyset = string.Format("{0:N0}", 0);
                }
                else
                {
                    totalPG.Moneyset = string.Format("{0:N0}", moneys - acc.Wallet);
                }
                totalPG.totalMoney = string.Format("{0:N0}", moneys);
                totalPG.MoneyUser = string.Format("{0:N0}", acc.Wallet);
                totalPG.totalPackageScan = (packs.Length - 1).ToString();
                totalPG.totalPackage = SmallPackageController.GetAllByUIDAndStatus(acc.ID, 3).Count().ToString();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(totalPG);
            }
            else
            {
                return "none";
            }
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username_current = Session["userLoginSystem"].ToString();
            var userAdmin = AccountController.GetByUsername(username_current);
            int id = ViewState["id"].ToString().ToInt(0);
            if (id > 0)
            {
                int UID = 0;
                var outs = OutStockSessionController.GetByID(id);
                if (outs != null)
                {
                    UID = Convert.ToInt32(outs.UID);
                }


                OutStockSessionController.update(id, txtFullname.Text, txtPhone.Text, 1, currentDate, username_current);
                var sessionpack = OutStockSessionPackageController.GetAllByOutStockSessionID(id);
                if (sessionpack.Count > 0)
                {
                    List<Main> mo = new List<Main>();
                    List<Trans> to = new List<Trans>();
                    foreach (var item in sessionpack)
                    {
                        SmallPackageController.UpdateStatus(Convert.ToInt32(item.SmallPackageID), 6, currentDate, username_current);
                        SmallPackageController.UpdateDateOutWarehouse(Convert.ToInt32(item.SmallPackageID), username_current, currentDate);

                        if (item.MainOrderID > 0)
                        {
                            bool check = mo.Any(x => x.MainOrderID == Convert.ToInt32(item.MainOrderID));
                            if (check != true)
                            {
                                Main m = new Main();
                                m.MainOrderID = Convert.ToInt32(item.MainOrderID);
                                mo.Add(m);
                            }
                        }
                        else
                        {
                            bool check = to.Any(x => x.TransportationOrderID == Convert.ToInt32(item.TransportationID));
                            if (check != true)
                            {
                                Trans t = new Trans();
                                t.TransportationOrderID = Convert.ToInt32(item.TransportationID);
                                to.Add(t);
                            }
                        }
                    }
                    if (mo.Count > 0)
                    {
                        int dem = 0;
                        foreach (var item in mo)
                        {
                            var m = MainOrderController.GetAllByID(item.MainOrderID);
                            if (m != null)
                            {
                                bool checkIsChinaCome = true;
                                var packages = SmallPackageController.GetByMainOrderID(item.MainOrderID);
                                if (packages.Count > 0)
                                {
                                    foreach (var p in packages)
                                    {
                                        if (p.Status < 6)
                                            checkIsChinaCome = false;
                                    }
                                }
                                if (checkIsChinaCome == true)
                                {
                                    MainOrderController.UpdateDateToFis(m.ID, currentDate);
                                }
                                if (outs.TotalPay > 0)
                                {
                                    var obj_user = AccountController.GetByID(UID);
                                    if (obj_user != null)
                                    {
                                        double deposited = 0;
                                        if (m.Deposit.ToFloat(0) > 0)
                                            deposited = Convert.ToDouble(m.Deposit);
                                        double totalPrice = Convert.ToDouble(m.TotalPriceVND);
                                        double totalPriceInwarehouse = 0;
                                        if (m.FeeInWareHouse > 0)
                                            totalPriceInwarehouse = Convert.ToDouble(m.FeeInWareHouse);
                                        double finalPrice = totalPrice + totalPriceInwarehouse;
                                        double leftpay = finalPrice - deposited;
                                        //MainOrderController.UpdateDeposit(m.ID, Convert.ToInt32(m.UID), totalPrice.ToString());

                                        double wallet = 0;
                                        if (obj_user.Wallet.ToString().ToFloat(0) > 0)
                                            wallet = Convert.ToDouble(obj_user.Wallet);

                                        if (wallet >= leftpay && leftpay > 0)
                                        {
                                            double walletLeft = wallet - leftpay;
                                            //MainOrderController.UpdateStatus(o.ID, UID, 9);
                                            AccountController.updateWallet(UID, walletLeft, currentDate, username_current);


                                            HistoryPayWalletController.Insert(UID, obj_user.Username, m.ID, leftpay, obj_user.Username + " đã thanh toán đơn hàng: " + m.ID + ".", walletLeft, 1, 3, currentDate, username_current);
                                            string kq = MainOrderController.UpdateDeposit(m.ID, UID, finalPrice.ToString());
                                            PayOrderHistoryController.Insert(id, UID, 9, leftpay, 2, currentDate, username_current);
                                            //OutStockSessionController.updateStatus(id, 2, currentDate, username_current);

                                            //cong sua cho nay bo update tat ca ma van don
                                            //var smalls = SmallPackageController.GetByMainOrderID(m.ID);
                                            //foreach(var temp in smalls)
                                            //{
                                            //    SmallPackageController.UpdateStatus(temp.ID, 4, currentDate, username_current);
                                            //}
                                            HistoryOrderChangeController.Insert(m.ID, userAdmin.ID, username_current, username_current +
                                                        " đã đổi trạng thái của đơn hàng ID là: " + m.ID + ", từ: Đã đang giao hàng, sang: Khách đã thanh toán.", 1, currentDate);
                                            MainOrderController.UpdateStatusByID(m.ID, 9);
                                            if (m.PayDate == null)
                                            {
                                                MainOrderController.UpdatePayDate(m.ID, currentDate);
                                            }
                                            dem++;
                                        }
                                        else
                                        {
                                            MainOrderController.UpdateIsDebt(m.ID, true);
                                        }    
                                    }
                                }                                
                            }
                        }
                        if (dem == mo.Count())
                        {
                            OutStockSessionController.updateCheckPrice(id, true);
                            OutStockSessionController.updateStatus(id, 2, currentDate, username_current);
                            AccountantOutStockPaymentController.Insert(outs.ID, outs.TotalPay.Value, Convert.ToInt32(outs.UID), outs.Username, "Thanh toán bằng ví điện tử", currentDate, username_current);
                        }
                    }
                }
                var c = ConfigurationController.GetByTop1();

                string content = ViewState["content"].ToString();
                var html = "";
                html += "<div class=\"print-bill\">";
                html += "   <div class=\"top\">";
                html += "       <div class=\"left\">";
                html += "           <span class=\"company-info\">NHAPHANGTRUNG.VN</span>";
                html += "          <span class=\"company-info\">Địa chỉ: Quan Hoa - Cầu Giấy - Hà Nội</span>";
                html += "       </div>";
                html += "       <div class=\"right\">";
                html += "           <span class=\"bill-num\">Mẫu số 01 - TT</span>";
                html += "           <span class=\"bill-promulgate-date\">(Ban hành theo Thông tư số 133/2016/TT-BTC ngày 26/8/2016 của Bộ Tài chính)</span>";
                html += "       </div>";
                html += "   </div>";
                html += "   <div class=\"bill-title\">";
                html += "       <h1>PHIẾU XUẤT KHO</h1>";
                html += "       <span class=\"bill-date\">" + string.Format("{0:dd/MM/yyyy HH:mm}", currentDate) + " </span>";
                html += "   </div>";
                html += "   <div class=\"bill-content\">";
                html += "       <div class=\"bill-row\">";
                html += "           <label class=\"row-name\">Họ và tên người đến nhận: </label>";
                html += "           <label class=\"row-info\">" + txtFullname.Text + "</label>";
                html += "       </div>";
                html += "       <div class=\"bill-row\">";
                html += "           <label class=\"row-name\">Số ĐT người đến nhận: </label>";
                html += "           <label class=\"row-info\">" + txtPhone.Text + "</label>";
                html += "       </div>";
                html += "       <div class=\"bill-row\" style=\"border:none\">";
                html += "           <label class=\"row-name\">Danh sách kiện: </label>";
                html += "           <label class=\"row-info\"></label>";
                html += "       </div>";
                html += "       <div class=\"bill-row\" style=\"border:none\">";
                html += content;
                html += "       </div>";
                html += "   </div>";
                html += "   <div class=\"bill-footer\">";
                html += "       <div class=\"bill-row-two\">";
                html += "           <strong>Người xuất hàng</strong>";
                html += "           <span class=\"note\">(Ký, họ tên)</span>";
                html += "       </div>";
                html += "       <div class=\"bill-row-two\">";
                html += "           <strong>Người nhận hàng</strong>";
                html += "           <span class=\"note\">(Ký, họ tên)</span>";
                html += "           <span class=\"note\" style=\"margin-top:100px;\">" + txtFullname.Text + "</span>";
                html += "       </div>";
                html += "   </div>";
                html += "</div>";

                StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script language='javascript'>");

                sb.Append(@"VoucherPrint('" + html + "')");
                sb.Append(@"</script>");

                ///hàm để đăng ký javascript và thực thi đoạn script trên
                if (!ClientScript.IsStartupScriptRegistered("JSScript"))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "JSScript", sb.ToString());

                }
            }
        }


        public class Main
        {
            public int MainOrderID { get; set; }
        }

        public class Trans
        {
            public int TransportationOrderID { get; set; }
        }
    }
}