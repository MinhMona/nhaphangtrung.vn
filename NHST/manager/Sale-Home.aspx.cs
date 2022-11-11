using MB.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebUI.Business;
using NHST.Models;
using NHST.Bussiness;
using NHST.Controllers;
using System.Web.Services;

namespace NHST.manager
{
    public partial class Sale_Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                else
                {
                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);
                    LoadUser();
                    LoadOrder();
                }
            }
        }

        protected void LoadUser()
        {
            AccountController.updateTypePerson1();
            AccountController.updateTypePerson2();
            AccountController.updateTypePerson3();
        }

        private void LoadOrder()
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);

            var CurrentDate = DateTime.Now;
            var FirstDayOfMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);

            string fd = FirstDayOfMonth.ToString();
            string td = CurrentDate.ToString();

            var mo = MainOrderController.GetTotalOrderAllSale(Convert.ToInt32(ac.ID), fd, td);
            ltrTotalOrder.Text = mo.ToString();

            var mostt0 = MainOrderController.GetTotalOrderAllSaleStatus0(Convert.ToInt32(ac.ID), fd, td);
            ltrTotalOrderStatus0.Text = mostt0.ToString();

            var mo1 = MainOrderController.GetTotalOrderSale(1, Convert.ToInt32(ac.ID), fd, td);
            ltrOrder1.Text = mo1.ToString();

            var mo3 = MainOrderController.GetTotalOrderSale(3, Convert.ToInt32(ac.ID), fd, td);
            ltrOrder3.Text = mo3.ToString();

            var mo1stt0 = MainOrderController.GetTotalOrderSaleStatus0(1, Convert.ToInt32(ac.ID), fd, td);
            ltrOrder1Satus0.Text = mo1stt0.ToString();

            var mo3stt0 = MainOrderController.GetTotalOrderSaleStatus0(3, Convert.ToInt32(ac.ID), fd, td);
            ltrOrder3Satus0.Text = mo3stt0.ToString();

            var totalprice = MainOrderController.GetTotalPriceSale(Convert.ToInt32(ac.ID), "PriceVND", fd, td);
            ltrTotalPrice.Text = string.Format("{0:N0}", Convert.ToDouble(totalprice)) + " đ";

            var totalprice0 = MainOrderController.GetTotalPriceSaleCustom(Convert.ToInt32(ac.ID), "PriceVND", 1);
            ltrTotalPrice0.Text = string.Format("{0:N0}", Convert.ToDouble(totalprice0)) + " đ";

            var totalprice60 = MainOrderController.GetTotalPriceSaleCustom(Convert.ToInt32(ac.ID), "PriceVND", 2);
            ltrTotalPrice60.Text = string.Format("{0:N0}", Convert.ToDouble(totalprice60)) + " đ";

            var totalprice240 = MainOrderController.GetTotalPriceSaleCustom(Convert.ToInt32(ac.ID), "PriceVND", 3);
            ltrTotalPrice240.Text = string.Format("{0:N0}", Convert.ToDouble(totalprice240)) + " đ";


            #region Danh sách doanh thu sale
            var listsale = GetListSale(fd, td);
            if (listsale != null)
            {
                if (listsale.Count > 0)
                {
                    StringBuilder html = new StringBuilder();
                    for (int i = 0; i < listsale.Count; i++)
                    {
                        var item = listsale[i];
                        html.Append("<tr>");
                        html.Append("    <td>" + item.ID + "</td>");
                        html.Append("    <td>" + item.Username + "</td>");
                        html.Append("    <td>" + string.Format("{0:N0}", item.TotalPrice) + " đ</td>");
                        html.Append("    </td>");
                        html.Append("</tr>");
                    }
                    ltrListTotalPriceSale.Text = html.ToString();
                }    
            }
            #endregion
            #region Khách hàng có nhiều đơn hàng
            var listUserManyOrder = GetTop10UserHasAlotOrder(Convert.ToInt32(ac.ID), fd, td);
            if (listUserManyOrder != null)
            {
                if (listUserManyOrder.Count > 0)
                {
                    StringBuilder html = new StringBuilder();
                    for (int i = 0; i < listUserManyOrder.Count; i++)
                    {
                        var item = listUserManyOrder[i];
                        html.Append("<tr>");
                        html.Append("    <td>" + item.ID + "</td>");
                        html.Append("    <td>" + item.Username + "</td>");
                        html.Append("    <td>" + string.Format("{0:N0}", item.Wallet) + " đ</td>");
                        html.Append("    <td>" + item.TotalAll + "</td>");                       
                        html.Append("</tr>");
                    }
                    ltrTop10UserHasAlotOrder.Text = html.ToString();
                }
            }
            #endregion
            #region Khách hàng mới nạp tiền
            var listUserAddNewWallet = GetUserAddNewWallet(Convert.ToInt32(ac.ID));
            if (listUserAddNewWallet != null)
            {
                if (listUserAddNewWallet.Count > 0)
                {
                    StringBuilder html = new StringBuilder();
                    for (int i = 0; i < listUserAddNewWallet.Count; i++)
                    {
                        var item = listUserAddNewWallet[i];
                        html.Append("<tr>");
                        html.Append("    <td>" + item.STT + "</td>");
                        html.Append("    <td>" + item.Username + "</td>");
                        html.Append("    <td>" + string.Format("{0:N0}", item.Amount) + " đ</td>");
                        html.Append("    <td>" + item.CreatedDate.ToString("dd/MM/yyyy hh:mm") + "</td>");
                        html.Append("    <td>" + PJUtils.ReturnStatusAddNewWallet(item.Status) + "</td>");
                        //html.Append("    <td class=\"center-align\"><a href=\"/manager/HistorySendWallet\"><i class=\"material-icons teal-text text-darken-4\">remove_red_eye</i></a>");
                        html.Append("    </td>");
                        html.Append("</tr>");
                    }
                    ltrUserAddNewWallet.Text = html.ToString();
                }
            }
            #endregion
        }


        public partial class DoanhThuSale
        {
            public int ID { get; set; }
            public int UID { get; set; }
            public string Username { get; set; }           
            public double TotalPrice { get; set; }
        }
        public partial class KhachHangNhieuDon
        {
            public int ID { get; set; }
            public int UID { get; set; }
            public string Username { get; set; }
            public double Wallet { get; set; }         
            public int TotalAll { get; set; }
        }
        public partial class KhachNapTienMoi
        {
            public int ID { get; set; }
            public int STT { get; set; }
            public int UID { get; set; }
            public int Status { get; set; }
            public string Username { get; set; }
            public double Amount { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public List<DoanhThuSale> GetListSale(string fd, string td)
        {
            var sql = @" select  ac.ID, ac.Username, TotalPrice ";
            sql += " FROM tbl_Account as ac LEFT OUTER JOIN ";
            sql += " (SELECT SalerID, SUM(cast(PriceVND as float)) AS TotalPrice ";
            sql += " FROM tbl_MainOder where Status > 4  AND CreatedDate >= CONVERT(VARCHAR(24),'" + fd + "',113) AND CreatedDate <= CONVERT(VARCHAR(24),'" + td + "',113) GROUP BY SalerID) as mo ON mo.SalerID = ac.ID ";
            sql += " WHERE ac.RoleID = 6 AND ac.Status = 2 ";            
            sql += " ORDER BY TotalPrice desc ";
            List<DoanhThuSale> list = new List<DoanhThuSale>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int stt = 0;
            while (reader.Read())
            {
                stt++;
                var entity = new DoanhThuSale();

                entity.ID = stt;
                if (reader["ID"] != DBNull.Value)
                    entity.UID = reader["ID"].ToString().ToInt();

                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();

                if (reader["TotalPrice"] != DBNull.Value)
                    entity.TotalPrice = Convert.ToDouble(reader["TotalPrice"].ToString());              

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public List<KhachHangNhieuDon> GetTop10UserHasAlotOrder(int UID, string fd, string td)
        {
            var sql = @" select top 10 mo.ID, mo.Username, mo.Email, mo.Wallet, Total ";
            sql += " from tbl_Account as mo  ";
            sql += " LEFT JOIN (SELECT UID, COUNT(*) AS Total FROM tbl_MainOder where SalerID = '" + UID + "' AND Status > 1 AND CreatedDate >= CONVERT(VARCHAR(24),'" + fd + "',113) AND CreatedDate <= CONVERT(VARCHAR(24),'" + td + "',113) GROUP BY UID) as c ON c.UID = mo.ID ";         
            sql += " where mo.RoleID = 1 ";
            sql += " order by Total desc ";
            List<KhachHangNhieuDon> list = new List<KhachHangNhieuDon>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int stt = 0;
            while (reader.Read())
            {
                stt++;
                var entity = new KhachHangNhieuDon();

                entity.ID = stt;

                if (reader["ID"] != DBNull.Value)
                    entity.UID = reader["ID"].ToString().ToInt();

                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = Convert.ToDouble(reader["Wallet"].ToString());

                if (reader["Total"] != DBNull.Value)
                    entity.TotalAll = reader["Total"].ToString().ToInt();
                else
                    entity.TotalAll = 0;              

                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public List<KhachNapTienMoi> GetUserAddNewWallet(int UID)
        {
            var sql = @"select top 10 * ";
            sql += "from tbl_AdminSendUserWallet as mo ";
            sql += "LEFT OUTER JOIN tbl_Account as ac ON ac.ID = mo.UID ";
            sql += "Where mo.Status in(1,2) AND ac.SaleID = '" + UID + "' ";
            sql += "order by mo.id desc ";
            List<KhachNapTienMoi> list = new List<KhachNapTienMoi>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int stt = 0;
            while (reader.Read())
            {
                stt++;
                var entity = new KhachNapTienMoi();

                entity.STT = stt;

                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);               

                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt(0);

                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();

                if (reader["Amount"] != DBNull.Value)
                    entity.Amount = Convert.ToDouble(reader["Amount"].ToString());

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

    }
}