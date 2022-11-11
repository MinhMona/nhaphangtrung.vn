using MB.Extensions;
using Microsoft.AspNet.SignalR;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class danh_sach_don_hang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Session["ListDep"] = null;
                HttpContext.Current.Session["ListPay"] = null;
                HttpContext.Current.Session["ListYCG"] = null;
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                LoadData();
            }
        }

        #region Button status
        //tất cả
        protected void btnAll_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = -1;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đơn mới
        protected void btn0_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 0;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đã hủy
        protected void btn1_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 1;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đã đặt cọc
        protected void btn2_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 2;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //chờ đã mua hàng
        protected void btn4_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 4;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đã mua hàng
        protected void btn5_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 5;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đơn người bán giao
        protected void btn3_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 3;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đã về kho tq
        protected void btn6_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 6;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //trên đường về kho vn
        protected void btn7_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 7;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //về kho vn
        protected void btn8_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 8;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đang giao hàng
        protected void btn11_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 11;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đã thanh toán
        protected void btn9_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 9;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đã hoàn thành
        protected void btn10_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 10;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }
        //đã khiếu nại
        protected void btn12_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["t"] != null)
            {
                uID = Request.QueryString["t"].ToInt(1);
            }
            int st = 12;
            Response.Redirect("/danh-sach-don-hang?t=" + uID + "&stt=" + st + "");
        }

        #endregion

        public void LoadData()
        {
            int t = Request.QueryString["t"].ToInt(1);
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int UID = obj_user.ID;

                //Khai báo biến
                double tongsodonhang = 0;
                double tongtrigiadonhang = 0;
                double tongtienlayhang = 0;

                double tongtienhangchuagiao = 0;
                double Tongtienhangcandatcoc = 0;
                double Tongtienhang = 0;
                double Tongtienhangchovekhotq = 0;
                double Tongtienhangdavekhotq = 0;
                double Tongtienhangdangokhovn = 0;

                double order_stt0 = 0;
                double order_stt2 = 0;
                double order_stt5 = 0;
                double order_stt6 = 0;
                double order_stt7 = 0;
                double order_stt10 = 0;

                string se = Request.QueryString["s"];
                int typesearch = Request.QueryString["l"].ToInt(0);
                int status = Request.QueryString["stt"].ToInt(-1);
                string fd = Request.QueryString["fd"];
                string td = Request.QueryString["td"];

                txtSearhc.Text = se;
                ddlType.SelectedValue = typesearch.ToString();
                ViewState["t"] = t.ToString();
                //var os = MainOrderController.GetAllByUIDNotHidden_SqlHelper(UID, status, fd, td);

                List<MainOrderController.mainorder> tos = new List<MainOrderController.mainorder>();
                var os = MainOrderController.GetAllByUIDAndOrderType(UID, t);

                if (!string.IsNullOrEmpty(Request.QueryString["fd"]))
                {
                    FD.Text = fd;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["td"]))
                    TD.Text = td;

                int page = 0;

                Int32 Page = GetIntFromQueryString("Page");
                if (Page > 0)
                {
                    page = Page - 1;
                }

                //tos = MainOrderController.GetAllByUIDNotHidden_SqlHelper(UID, status, fd, td, t);
                tos = MainOrderController.GetAllByUIDNotHidden_SqlHelperNew(UID, se, typesearch, status, fd, td, t, page);


                var stt0 = os.Where(o => o.Status == 0).ToList();
                var stt1 = os.Where(o => o.Status == 1).ToList();
                var stt2 = os.Where(o => o.Status == 2).ToList();
                var stt3 = os.Where(o => o.Status == 3).ToList();
                var stt4 = os.Where(o => o.Status == 4).ToList();
                var stt5 = os.Where(o => o.Status == 5).ToList();
                var stt6 = os.Where(o => o.Status == 6).ToList();
                var stt7 = os.Where(o => o.Status == 7).ToList();
                var stt8 = os.Where(o => o.Status == 8).ToList();
                var stt9 = os.Where(o => o.Status == 9).ToList();
                var stt10 = os.Where(o => o.Status == 10).ToList();
                var stt11 = os.Where(o => o.Status == 11).ToList();
                var stt12 = os.Where(o => o.Status == 12).ToList();

                bttnAll.Text = "Tất cả (" + os.Count + ")";
                btn0.Text = "Đơn mới (" + stt0.Count + ")";
                btn1.Text = "Đơn hàng hủy (" + stt1.Count + ")";
                btn2.Text = "Đã đặt cọc (" + stt2.Count + ")";
                btn4.Text = "Đơn chờ mua hàng (" + stt4.Count + ")";
                btn3.Text = "Đơn người bán giao (" + stt3.Count + ")";
                btn5.Text = "Đơn đã mua hàng (" + stt5.Count + ")";
                btn6.Text = "Kho Trung Quốc nhận hàng (" + stt6.Count + ")";
                btn7.Text = "Trên đường về Việt Nam (" + stt7.Count + ")";
                btn8.Text = "Trong kho Hà Nội (" + stt8.Count + ")";
                btn9.Text = "Đã thanh toán (" + stt9.Count + ")";
                btn10.Text = "Đã hoàn thành (" + stt10.Count + ")";
                btn11.Text = "Đang giao hàng (" + stt11.Count + ")";
                btn12.Text = "Đơn khiếu nại (" + stt12.Count + ")";
                if (tos.Count > 0)
                {
                    var orderstt0 = tos.Where(od => od.Status == 0).ToList();
                    var orderstt2 = tos.Where(od => od.Status == 2).ToList();
                    var orderstt5 = tos.Where(od => od.Status == 5).ToList();
                    var orderstt6 = tos.Where(od => od.Status == 6).ToList();
                    var orderstt7 = tos.Where(od => od.Status == 8).ToList();
                    var orderstt10 = tos.Where(od => od.Status == 10).ToList();

                    var totalorderchuagiao = tos.Where(od => od.Status == 2 || od.Status == 5 || od.Status == 6 || od.Status == 8).ToList();
                    if (totalorderchuagiao.Count > 0)
                    {
                        foreach (var item in totalorderchuagiao)
                        {
                            tongtienhangchuagiao += Convert.ToDouble(item.TotalPriceVND);
                        }
                    }

                    #region New
                    Tongtienhangcandatcoc = MainOrderController.GetTotalPrice(UID, 0, "AmountDeposit", t);
                    Tongtienhang = MainOrderController.GetTotalPrice(UID, 0, "TotalPriceVND", 1);
                    Tongtienhangchovekhotq = MainOrderController.GetTotalPrice(UID, 5, "TotalPriceVND", t);
                    Tongtienhangdavekhotq = MainOrderController.GetTotalPrice(UID, 6, "TotalPriceVND", t);
                    Tongtienhangdangokhovn = MainOrderController.GetTotalPrice(UID, 8, "TotalPriceVND", t);
                    #endregion

                    order_stt0 = orderstt0.Count;
                    order_stt2 = orderstt2.Count;
                    order_stt5 = orderstt5.Count;
                    order_stt6 = orderstt6.Count;
                    order_stt7 = orderstt7.Count;
                    order_stt10 = orderstt10.Count;

                    tongsodonhang = tos.Count;
                    var order_stt2morer = tos.Where(od => od.Status >= 2).ToList();
                    foreach (var o in order_stt2morer)
                    {
                        tongtrigiadonhang += Convert.ToDouble(o.TotalPriceVND);
                    }


                    double totalall7 = MainOrderController.GetTotalPrice(UID, 8, "TotalPriceVND", t);
                    double totalall7_deposit = MainOrderController.GetTotalPrice(UID, 8, "Deposit", t);
                    tongtienlayhang = totalall7 - totalall7_deposit;

                    DateTime checkdate = DateTime.Now;
                    var ts = tos.Where(x => x.Status == 0).ToList();
                    foreach (var item in ts)
                    {
                        if (item.Status == 0)
                        {
                            DateTime CreatedDate = Convert.ToDateTime(item.CreatedDate);
                            TimeSpan span = checkdate.Subtract(CreatedDate);
                            if (span.Days > 7)
                            {
                                //MainOrderController.UpdateIsHiddenTrue(item.ID);
                            }
                        }
                    }
                    //Ghi ra 
                    ltrAllOrderCount.Text = string.Format("{0:N0}", tongsodonhang).Replace(",", ".");
                    //ltrAllOrderPrice.Text = string.Format("{0:N0}", tongtrigiadonhang).Replace(",", ".");
                    //ltrTotalGetAllProduct.Text = string.Format("{0:N0}", tongtienlayhang).Replace(",", ".");
                    double totalp = 0;
                    totalp = HistoryPayWalletController.GetTotalAllAmount_BySQLUser("4", "", "", obj_user.ID);

                    ltrTongtienhangdanapvi.Text = string.Format("{0:N0}", totalp) + "";

                    ltrTongtienhangchuagiao.Text = string.Format("{0:N0}", tongtienhangchuagiao).Replace(",", ".");
                    ltrTongtienhangcandatcoc.Text = string.Format("{0:N0}", Tongtienhangcandatcoc).Replace(",", ".");
                    ltrTongtienhangchovekhotq.Text = string.Format("{0:N0}", Tongtienhangchovekhotq).Replace(",", ".");
                    ltrTongtienhangdavekhotq.Text = string.Format("{0:N0}", Tongtienhangdavekhotq).Replace(",", ".");
                    ltrTongtienhangdangokhovn.Text = string.Format("{0:N0}", Tongtienhangdangokhovn).Replace(",", ".");
                    ltrTongtienhangcanthanhtoandelayhang.Text = string.Format("{0:N0}", tongtienlayhang).Replace(",", ".");
                    ltrTongtienhangdatcoc.Text = string.Format("{0:N0}", Tongtienhang).Replace(",", ".");

                    ltrOrderStatus0.Text = string.Format("{0:N0}", order_stt0).Replace(",", ".");
                    ltrOrderStatus2.Text = string.Format("{0:N0}", order_stt2).Replace(",", ".");
                    ltrOrderStatus5.Text = string.Format("{0:N0}", order_stt5).Replace(",", ".");
                    ltrOrderStatus6.Text = string.Format("{0:N0}", order_stt6).Replace(",", ".");
                    ltrOrderStatus7.Text = string.Format("{0:N0}", order_stt7).Replace(",", ".");
                    ltrOrderStatus10.Text = string.Format("{0:N0}", order_stt10).Replace(",", ".");

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    var listDep = HttpContext.Current.Session["ListDep"] as List<DepAll>;
                    if (listDep != null)
                    {
                        if (listDep.Count > 0)
                        {
                            hdfShowDep.Value = serializer.Serialize(listDep);
                        }
                    }

                    var listPay = HttpContext.Current.Session["ListPay"] as List<PayAll>;
                    if (listPay != null)
                    {
                        if (listPay.Count > 0)
                        {

                            hdfShowPay.Value = serializer.Serialize(listPay);
                        }
                    }

                    var listYCG = HttpContext.Current.Session["ListYCG"] as List<YCG>;
                    if (listYCG != null)
                    {
                        if (listYCG.Count > 0)
                        {

                            hdfShowYCG.Value = serializer.Serialize(listYCG);
                        }
                    }

                    int total = MainOrderController.GetTotalItem(UID, status, fd, td, t);

                    pagingall(tos.OrderByDescending(x => x.ID).ToList(), total);
                }
            }
        }

        #region Paging
        public void pagingall(List<MainOrderController.mainorder> acs, int total)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int PageSize = 15;
                if (total > 0)
                {
                    int TotalItems = total;
                    if (TotalItems % PageSize == 0)
                        PageCount = TotalItems / PageSize;
                    else
                        PageCount = TotalItems / PageSize + 1;

                    Int32 Page = GetIntFromQueryString("Page");

                    if (Page == -1) Page = 1;
                    int FromRow = (Page - 1) * PageSize;
                    int ToRow = Page * PageSize - 1;
                    if (ToRow >= TotalItems)
                        ToRow = TotalItems - 1;
                    StringBuilder html = new StringBuilder();

                    for (int i = 0; i < acs.Count; i++)
                    {
                        var item = acs[i];
                        int status = Convert.ToInt32(item.Status);
                        if (status == 0)
                        {
                            double deposited = 0;
                            double TotalPriceVND = 0;
                            if (item.TotalPriceVND.ToFloat(0) > 0)
                            {
                                TotalPriceVND = Convert.ToDouble(item.TotalPriceVND);
                            }
                            if (item.Deposit.ToFloat(0) > 0)
                            {
                                deposited = Convert.ToDouble(item.Deposit);
                            }
                            double must_Deposit = Convert.ToDouble(item.AmountDeposit);
                            double must_Deposit_left = must_Deposit - deposited;

                            if (item.OrderType == 1)
                            {
                                html.Append("<tr data-action=\"deposit\">");
                                html.Append("<td>");
                                var list = HttpContext.Current.Session["ListDep"] as List<DepAll>;
                                if (list != null)
                                {
                                    var check = list.Where(x => x.MainOrderID == item.ID).FirstOrDefault();
                                    if (check != null)
                                    {
                                        html.Append(" <label><input type=\"checkbox\" checked onchange=\"CheckDepAll(" + item.ID + "," + Math.Round(must_Deposit_left, 0) + ")\" data-value=\"" + Math.Round(must_Deposit_left, 0) + "\" data-id=\"" + item.ID + "\"><span></span></label>");
                                    }
                                    else
                                    {
                                        html.Append(" <label><input type=\"checkbox\" onchange=\"CheckDepAll(" + item.ID + "," + Math.Round(must_Deposit_left, 0) + ")\" data-value=\"" + Math.Round(must_Deposit_left, 0) + "\" data-id=\"" + item.ID + "\"><span></span></label>");
                                    }
                                }
                                else
                                {
                                    html.Append(" <label><input type=\"checkbox\" onchange=\"CheckDepAll(" + item.ID + "," + Math.Round(must_Deposit_left, 0) + ")\" data-value=\"" + Math.Round(must_Deposit_left, 0) + "\" data-id=\"" + item.ID + "\"><span></span></label>");
                                }
                                html.Append("</td>");
                            }
                            else
                            {
                                if (item.IsCheckNotiPrice == false)
                                {
                                    html.Append("<tr data-action=\"deposit\">");
                                    html.Append("<td>");
                                    var list = HttpContext.Current.Session["ListDep"] as List<DepAll>;
                                    if (list != null)
                                    {
                                        var check = list.Where(x => x.MainOrderID == item.ID).FirstOrDefault();
                                        if (check != null)
                                        {
                                            html.Append(" <label><input type=\"checkbox\" checked onchange=\"CheckDepAll(" + item.ID + "," + Math.Round(must_Deposit_left, 0) + ")\" data-value=\"" + Math.Round(must_Deposit_left, 0) + "\" data-id=\"" + item.ID + "\"><span></span></label>");
                                        }
                                        else
                                        {
                                            html.Append(" <label><input type=\"checkbox\" onchange=\"CheckDepAll(" + item.ID + "," + Math.Round(must_Deposit_left, 0) + ")\" data-value=\"" + Math.Round(must_Deposit_left, 0) + "\" data-id=\"" + item.ID + "\"><span></span></label>");
                                        }
                                    }
                                    else
                                    {
                                        html.Append(" <label><input type=\"checkbox\" onchange=\"CheckDepAll(" + item.ID + "," + Math.Round(must_Deposit_left, 0) + ")\" data-value=\"" + Math.Round(must_Deposit_left, 0) + "\" data-id=\"" + item.ID + "\"><span></span></label>");
                                    }
                                    html.Append("</td>");
                                }
                                else
                                {
                                    html.Append("<tr>");
                                    html.Append("<td>");
                                    html.Append("</td>");
                                }
                            }
                        }
                        else if (status == 7)
                        {

                            double deposited = 0;
                            double TotalPriceVND = 0;
                            if (item.TotalPriceVND.ToFloat(0) > 0)
                            {
                                TotalPriceVND = Convert.ToDouble(item.TotalPriceVND);
                            }
                            if (item.Deposit.ToFloat(0) > 0)
                            {
                                deposited = Convert.ToDouble(item.Deposit);
                            }
                            double must_Pay_left = TotalPriceVND - deposited;

                            html.Append("<tr data-action=\"checkout\">");
                            html.Append("<td>");
                            var list = HttpContext.Current.Session["ListPay"] as List<PayAll>;
                            if (list != null)
                            {
                                var check = list.Where(x => x.MainOrderID == item.ID).FirstOrDefault();
                                if (check != null)
                                {
                                    html.Append(" <label><input type=\"checkbox\" checked onchange=\"CheckPayAll(" + item.ID + "," + Math.Round(must_Pay_left, 0) + ")\"  data-value=\"" + Math.Round(must_Pay_left, 0) + "\" data-id=\"" + item.ID + "\"><span></span></label>");
                                }
                                else
                                {
                                    html.Append(" <label><input type=\"checkbox\" onchange=\"CheckPayAll(" + item.ID + "," + Math.Round(must_Pay_left, 0) + ")\"  data-value=\"" + Math.Round(must_Pay_left, 0) + "\" data-id=\"" + item.ID + "\"><span></span></label>");
                                }
                            }
                            else
                            {
                                html.Append(" <label><input type=\"checkbox\" onchange=\"CheckPayAll(" + item.ID + "," + Math.Round(must_Pay_left, 0) + ")\"  data-value=\"" + Math.Round(must_Pay_left, 0) + "\" data-id=\"" + item.ID + "\"><span></span></label>");
                            }
                            html.Append("</td>");
                            //else
                            //{
                            //    html.Append("<tr data-action=\"GiaoHang\">");
                            //    html.Append("<td>");
                            //    var list = HttpContext.Current.Session["ListYCG"] as List<YCG>;
                            //    if (list != null)
                            //    {
                            //        var check = list.Where(x => x.MainOrderID == item.ID).FirstOrDefault();
                            //        if (check != null)
                            //        {
                            //            html.Append(" <label><input type=\"checkbox\" checked onchange=\"CheckYCG(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                            //        }
                            //        else
                            //        {
                            //            html.Append(" <label><input type=\"checkbox\" onchange=\"CheckYCG(" + item.ID + ")\"   data-id=\"" + item.ID + "\"><span></span></label>");
                            //        }
                            //    }
                            //    else
                            //    {
                            //        html.Append(" <label><input type=\"checkbox\" onchange=\"CheckYCG(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                            //    }
                            //    html.Append("</td>");
                            //}
                        }
                        else if (status == 9)
                        {
                            if (item.IsGiaohang != true)
                            {
                                html.Append("<tr data-action=\"GiaoHang\">");
                                html.Append("<td>");
                                var list = HttpContext.Current.Session["ListYCG"] as List<YCG>;
                                if (list != null)
                                {
                                    var check = list.Where(x => x.MainOrderID == item.ID).FirstOrDefault();
                                    if (check != null)
                                    {
                                        html.Append(" <label><input type=\"checkbox\" checked onchange=\"CheckYCG(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                                    }
                                    else
                                    {
                                        html.Append(" <label><input type=\"checkbox\" onchange=\"CheckYCG(" + item.ID + ")\"   data-id=\"" + item.ID + "\"><span></span></label>");
                                    }
                                }
                                else
                                {
                                    html.Append(" <label><input type=\"checkbox\" onchange=\"CheckYCG(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                                }
                                html.Append("</td>");
                            }
                            else
                            {
                                html.Append("<td>");
                                html.Append("</td>");
                            }
                        }
                        else
                        {
                            html.Append("<tr>");
                            html.Append("<td>");
                            html.Append("</td>");
                        }
                        html.Append("<td>" + item.ID + "</td>");

                        html.Append("<td><img class=\"materialboxed\" src=\"" + item.anhsanpham + "\" alt=\"\" width=\"75\" /></td>");
                        //html.Append("<td>" + item.TotalLink + "</td>");
                        html.Append("<td>" + item.Site + "</td>");
                        html.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.CurrentCNYVN)) + "</td>");
                        html.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPriceVND)) + "</td>");
                        html.Append("<td>" + Math.Round(Convert.ToDouble(item.AmountDeposit) * 100 / Convert.ToDouble(item.PriceVND)) + "</td>");
                        html.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.AmountDeposit)) + "</td>");
                        html.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.Deposit)) + "</td>");
                        //html.Append("<td class=\"no-wrap\">" + string.Format("{0:dd/MM/yyyy}", item.CreatedDate) + "</td>");
                        html.Append("<td style=\"text-align: left;\">");

                        html.Append(item.Created);
                        html.Append(item.DepostiDate);
                        html.Append(item.DateBuy);
                        html.Append(item.DateBuyOK);
                        html.Append(item.DateShipper);
                        html.Append(item.DateTQ);
                        html.Append(item.DateToVN);
                        html.Append(item.DateVN);
                        html.Append(item.DateToShip);
                        html.Append(item.DatePay);
                        html.Append(item.CompleteDate);
                        html.Append(item.DateToCancel);

                        html.Append("</td>");

                        if (item.OrderType == 3)
                        {
                            if (item.IsCheckNotiPrice == true)
                            {
                                html.Append("<td class=\"no-wrap\"><span class=\"badge yellow-gold darken-2 white-text border-radius-2\">Chờ báo giá</span></td>");
                            }
                            else
                            {
                                html.Append("<td class=\"no-wrap\">" + PJUtils.IntToRequestAdminNew(status) + "</td>");
                            }
                        }
                        else
                        {
                            html.Append("<td class=\"no-wrap\">" + PJUtils.IntToRequestAdminNew(status) + "</td>");
                        }

                        html.Append("<td>");
                        html.Append("<div class=\"action-table\">");

                        html.Append("     <a href=\"/chi-tiet-don-hang/" + item.ID + "\" data-position=\"top\" ><i class=\"material-icons\">remove_red_eye</i><span>Chi tiết</span></a>");
                        if (item.Status == 9 || item.Status == 10)
                        {
                            html.Append("     <a href=\"/them-khieu-nai/" + item.ID + "\" data-position=\"top\"><i class=\"material-icons\">report</i><span>Khiếu nại</span></a>");
                        }

                        html.Append("    <a href=\"javascript:;\" onclick=\"OrderSame('" + item.ID + "')\" data-position=\"top\"><i class=\"material-icons\">reorder</i>Tạo đơn tương tự</a>");


                        if (item.OrderType == 3)
                        {
                            if (item.IsCheckNotiPrice == true)
                            {

                            }
                            else
                            {
                                if (item.Status == 0)
                                {                                   
                                    html.Append("    <a href=\"javascript:;\" onclick=\"depositOrder('" + item.ID + "', $(this))\" data-position=\"top\" ><i class=\"material-icons\">attach_money</i><span>Đặt cọc</span></a>");
                                    //html.Append("    <a href=\"javascript:;\" onclick=\"depositOrder100('" + item.ID + "', $(this))\" data-position=\"top\"><i class=\"material-icons\">attach_money</i><span>Đặt cọc 100%</span></a>");
                                    //html.Append("    <a href=\"javascript:;\" onclick=\"depositOrder80('" + item.ID + "', $(this))\" data-position=\"top\"><i class=\"material-icons\">attach_money</i><span>Đặt cọc 70%</span></a>");
                                }
                                else if (Math.Round(Convert.ToDouble(item.Deposit), 0) < Math.Round(Convert.ToDouble(item.AmountDeposit), 0) && item.Status != 1)
                                {
                                    html.Append("    <a href=\"javascript:;\" onclick=\"depositOrderExtra('" + item.ID + "', $(this))\" data-position=\"top\" ><i class=\"material-icons\">attach_money</i><span>Đặt cọc thêm</span></a>");
                                }
                            }
                        }
                        else
                        {
                            if (item.Status == 0)
                            {                                
                                html.Append("    <a href=\"javascript:;\" onclick=\"depositOrder('" + item.ID + "', $(this))\" data-position=\"top\"><i class=\"material-icons\">attach_money</i><span>Đặt cọc</span></a>");
                                //html.Append("    <a href=\"javascript:;\" onclick=\"depositOrder100('" + item.ID + "', $(this))\" data-position=\"top\"><i class=\"material-icons\">attach_money</i><span>Đặt cọc 100%</span></a>");
                                //html.Append("    <a href=\"javascript:;\" onclick=\"depositOrder80('" + item.ID + "', $(this))\" data-position=\"top\"><i class=\"material-icons\">attach_money</i><span>Đặt cọc 70%</span></a>");
                            }
                            else if (Math.Round(Convert.ToDouble(item.Deposit), 0) < Math.Round(Convert.ToDouble(item.AmountDeposit), 0) && item.Status != 1)
                            {
                                html.Append("    <a href=\"javascript:;\" onclick=\"depositOrderExtra('" + item.ID + "', $(this))\" data-position=\"top\" ><i class=\"material-icons\">attach_money</i><span>Đặt cọc thêm</span></a>");
                            }
                        }

                        //Hiển thị nút thanh toán
                        double userdadeposit = 0;
                        if (item.Deposit != null)
                            userdadeposit = Math.Round(Convert.ToDouble(item.Deposit), 0);

                        double feewarehouse = 0;
                        if (item.FeeInWareHouse != null)
                            feewarehouse = Math.Round(Convert.ToDouble(item.FeeInWareHouse), 0);
                        double totalPrice = Math.Round(Convert.ToDouble(item.TotalPriceVND), 0);
                        double totalPay = totalPrice + feewarehouse;
                        double totalleft = totalPay - userdadeposit;

                        if (totalleft > 0)
                        {
                            if (item.Status == 8)
                            {
                                //html.Append("    <a href=\"javascript:;\" onclick=\"payallorder('" + item.ID + "')\" data-position=\"top\"><i class=\"material-icons\">payment</i><span>Thanh toán</span></a>");
                            }
                        }

                        html.Append("   </div>");
                        html.Append("  </td>");
                        html.Append("</tr>");
                    }
                    ltr.Text = html.ToString();
                }
            }
        }


        protected void btnOrderSame_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;          
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);               
            if (obj_user != null)
            {
                int OID = hdfOrderID.Value.ToInt();
                var mosame = MainOrderController.GetAllByID(OID);
                if (mosame != null)
                {
                    double current = Convert.ToDouble(ConfigurationController.GetByTop1().Currency);
                    if (!string.IsNullOrEmpty(obj_user.Currency.ToString()))
                    {
                        if (Convert.ToDouble(obj_user.Currency) > 0)
                        {
                            current = Convert.ToDouble(obj_user.Currency);
                        }
                    }

                    int salerID = obj_user.SaleID.ToString().ToInt(0);
                    int dathangID = obj_user.DathangID.ToString().ToInt(0);
                    int cskhID = obj_user.CSID.ToString().ToInt(0);
                    int UID = obj_user.ID;

                    double total = 0;
                    double fastprice = 0;
                    double pricepro = 0;
                    double priceproCYN = 0;

                    var odersame = OrderController.GetByMainOrderID(mosame.ID);
                    if (odersame.Count > 0)
                    {
                        foreach (var item in odersame)
                        {
                            int quantity = Convert.ToInt32(item.quantity);
                            double originprice = Convert.ToDouble(item.price_origin);
                            double promotionprice = Convert.ToDouble(item.price_promotion);
                            double u_pricecbuy = 0;
                            double u_pricevn = 0;
                            double e_pricebuy = 0;
                            double e_pricevn = 0;

                            if (promotionprice > 0)
                            {
                                if (promotionprice < originprice)
                                {
                                    u_pricecbuy = promotionprice;
                                    u_pricevn = promotionprice * current;
                                }
                                else
                                {
                                    u_pricecbuy = originprice;
                                    u_pricevn = originprice * current;
                                }
                            }
                            else
                            {
                                u_pricecbuy = originprice;
                                u_pricevn = originprice * current;
                            }

                            e_pricebuy = u_pricecbuy * quantity;
                            e_pricevn = u_pricevn * quantity;

                            pricepro += e_pricevn;
                            priceproCYN += e_pricebuy;
                        }                        
                    }

                    pricepro = Math.Round(pricepro, 0);
                    priceproCYN = Math.Round(priceproCYN, 2);
                    double PriceVND = pricepro;
                    double PriceCNY = priceproCYN;

                    double percentbuypro = 0;
                    double percentdeposit = 0;
                    double feebpnotdc = 0;
                    int LevelID = 0;

                    if (PriceVND > 10000000)
                        LevelID = 4;
                    else
                        LevelID = Convert.ToInt32(obj_user.LevelID);

                    var adminfeebuypro = FeeBuyProController.GetFeeLevelID(LevelID);
                    if (adminfeebuypro != null)
                    {
                        percentbuypro = Convert.ToDouble(adminfeebuypro.PercentService);
                        percentdeposit = Convert.ToDouble(adminfeebuypro.PercentDeposit);
                    }
                    if (Convert.ToDouble(obj_user.Deposit) > 0)
                    {
                        percentdeposit = Convert.ToDouble(obj_user.Deposit);
                    }
                    if (!string.IsNullOrEmpty(obj_user.FeeBuyPro))
                    {
                        if (obj_user.FeeBuyPro.ToFloat(0) > 0)
                        {
                            percentbuypro = Convert.ToDouble(obj_user.FeeBuyPro);
                            feebpnotdc = PriceVND * percentbuypro / 100;
                        }
                        else
                            feebpnotdc = PriceVND * percentbuypro / 100;
                    }
                    else
                        feebpnotdc = PriceVND * percentbuypro / 100;
                    double feebp = Math.Round(feebpnotdc, 0);
                    if (feebp < 20000)
                        feebp = 20000;

                    total = pricepro + feebp;
                    double FeeBuyPro = Math.Round(feebp, 0);
                    string AmountDeposit = Math.Round((PriceVND * percentdeposit / 100)).ToString();
                    string TotalPriceVND = Math.Round(total, 0).ToString();

                    string kq = MainOrderController.Insert_Same(Convert.ToInt32(mosame.UID), mosame.ShopID, mosame.ShopName, mosame.Site, false, "0", false, "0", false, "0", 
                    Convert.ToBoolean(mosame.IsPacked), "0", false, "0", PriceVND.ToString(), PriceCNY.ToString(), "0", FeeBuyPro.ToString(), "0", mosame.Note, mosame.FullName, mosame.Address, mosame.Email, mosame.Phone, 0,
                    "0", current.ToString(), TotalPriceVND, salerID, dathangID, currentDate, Convert.ToInt32(mosame.UID), AmountDeposit, Convert.ToInt32(mosame.OrderType), "0", OID, Convert.ToBoolean(mosame.IsCheckSpecial1), Convert.ToBoolean(mosame.IsCheckSpecial2));

                    string linkimage = "";
                    int idkq = Convert.ToInt32(kq);
                    if (idkq > 0)
                    {
                        foreach (var item in odersame)
                        {
                            int quantity = Convert.ToInt32(item.quantity);
                            double originprice = Convert.ToDouble(item.price_origin);
                            double promotionprice = Convert.ToDouble(item.price_promotion);
                            double u_pricecbuy = 0;
                            double u_pricevn = 0;
                            double e_pricebuy = 0;
                            double e_pricevn = 0;
                            if (promotionprice > 0)
                            {
                                if (promotionprice < originprice)
                                {
                                    u_pricecbuy = promotionprice;
                                    u_pricevn = promotionprice * current;
                                }
                                else
                                {
                                    u_pricecbuy = originprice;
                                    u_pricevn = originprice * current;
                                }
                            }
                            else
                            {
                                u_pricecbuy = originprice;
                                u_pricevn = originprice * current;
                            }

                            e_pricebuy = u_pricecbuy * quantity;
                            e_pricevn = u_pricevn * quantity;                           

                            string image = item.image_origin;
                            if (image.Contains("%2F"))
                            {
                                image = image.Replace("%2F", "/");
                            }
                            if (image.Contains("%3A"))
                            {
                                image = image.Replace("%3A", ":");
                            }
                            linkimage = image;
                            string ret = OrderController.Insert(Convert.ToInt32(mosame.UID), item.title_origin, item.title_translated, item.price_origin, item.price_promotion, item.property_translated,
                            item.property, item.data_value, image, image, item.shop_id, item.shop_name, item.seller_id, item.wangwang, item.quantity,
                            item.stock, item.location_sale, item.site, item.comment, item.item_id, item.link_origin, item.outer_id, item.error, item.weight, item.step, item.stepprice, item.brand,
                            item.category_name, item.category_id, item.tool, item.version, Convert.ToBoolean(item.is_translate), Convert.ToBoolean(item.IsForward), "0",
                            Convert.ToBoolean(item.IsFastDelivery), "0", Convert.ToBoolean(item.IsCheckProduct), "0", Convert.ToBoolean(item.IsPacked), "0", Convert.ToBoolean(item.IsFast),
                            fastprice.ToString(), pricepro.ToString(), priceproCYN.ToString(), item.Note, mosame.FullName, mosame.Address, mosame.Email,
                            mosame.Phone, 0, "0", current.ToString(), total.ToString(), idkq, DateTime.Now, Convert.ToInt32(mosame.UID));

                            if (item.price_promotion.ToFloat(0) > 0)
                                OrderController.UpdatePricePriceReal(ret.ToInt(0), item.price_origin, item.price_promotion);
                            else
                                OrderController.UpdatePricePriceReal(ret.ToInt(0), item.price_origin, item.price_origin);
                        }

                        MainOrderController.UpdateReceivePlace(idkq, Convert.ToInt32(mosame.UID), mosame.ReceivePlace, mosame.ShippingType.ToString().ToInt());
                        MainOrderController.UpdateFromPlace(idkq, Convert.ToInt32(mosame.UID), mosame.FromPlace.ToString().ToInt(), mosame.ShippingType.ToString().ToInt());
                        MainOrderController.UpdateLinkImage(idkq, linkimage);
                        MainOrderController.UpdatePercentDeposit(idkq, percentdeposit.ToString());
                        MainOrderController.UpdateCSKHID(idkq, cskhID);

                        var admins = AccountController.GetAllByRoleID(0);
                        if (admins.Count > 0)
                        {
                            foreach (var admin in admins)
                            {
                                NotificationController.Inser(Convert.ToInt32(obj_user.ID), username, admin.ID, admin.Username, idkq, "Có đơn hàng mới ID là: " + idkq, 0, 1, currentDate, username, false);
                            }
                        }

                        var managers = AccountController.GetAllByRoleID(2);
                        if (managers.Count > 0)
                        {
                            foreach (var manager in managers)
                            {
                                NotificationController.Inser(Convert.ToInt32(obj_user.ID), username, manager.ID, manager.Username, 0, "Có đơn hàng mới ID là: " + idkq, 0, 1, currentDate, username, false);
                            }
                        }

                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                        hubContext.Clients.All.addNewMessageToPage("", "");
                        Response.Redirect("/danh-sach-don-hang?t=1");
                    }
                }   
            }
            else
            {
                Response.Redirect("/trang-chu");
            }
        }


        public static Int32 GetIntFromQueryString(String key)
        {
            Int32 returnValue = -1;
            String queryStringValue = HttpContext.Current.Request.QueryString[key];
            try
            {
                if (queryStringValue == null)
                    return returnValue;
                if (queryStringValue.IndexOf("#") > 0)
                    queryStringValue = queryStringValue.Substring(0, queryStringValue.IndexOf("#"));
                returnValue = Convert.ToInt32(queryStringValue);
            }
            catch
            { }
            return returnValue;
        }
        private int PageCount;
        protected void DisplayHtmlStringPaging1()
        {
            Int32 CurrentPage = Convert.ToInt32(Request.QueryString["Page"]);
            if (CurrentPage == -1) CurrentPage = 1;
            string[] strText = new string[4] { "Trang đầu", "Trang cuối", "Trang sau", "Trang trước" };
            if (PageCount > 1)
                Response.Write(GetHtmlPagingAdvanced(6, CurrentPage, PageCount, Context.Request.RawUrl, strText));
        }
        private static string GetPageUrl(int currentPage, string pageUrl)
        {
            pageUrl = Regex.Replace(pageUrl, "(\\?|\\&)*" + "Page=" + currentPage, "");
            if (pageUrl.IndexOf("?") > 0)
            {
                pageUrl += "&Page={0}";
            }
            else
            {
                pageUrl += "?Page={0}";
            }
            return pageUrl;
        }
        public static string GetHtmlPagingAdvanced(int pagesToOutput, int currentPage, int pageCount, string currentPageUrl, string[] strText)
        {
            //Nếu Số trang hiển thị là số lẻ thì tăng thêm 1 thành chẵn
            if (pagesToOutput % 2 != 0)
            {
                pagesToOutput++;
            }

            //Một nửa số trang để đầu ra, đây là số lượng hai bên.
            int pagesToOutputHalfed = pagesToOutput / 2;

            //Url của trang
            string pageUrl = GetPageUrl(currentPage, currentPageUrl);


            //Trang đầu tiên
            int startPageNumbersFrom = currentPage - pagesToOutputHalfed; ;

            //Trang cuối cùng
            int stopPageNumbersAt = currentPage + pagesToOutputHalfed; ;

            StringBuilder output = new StringBuilder();

            //Nối chuỗi phân trang
            //output.Append("<div class=\"paging\">");
            //output.Append("<ul class=\"paging_hand\">");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                //output.Append("<li class=\"UnselectedPrev \" ><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\">|<</a></li>");
                //output.Append("<li class=\"UnselectedPrev\" ><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\"><i class=\"fa fa-angle-left\"></i></a></li>");
                output.Append("<a class=\"prev-page pagi-button\" title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\">Prev</a>");
                //output.Append("<span class=\"Unselect_prev\"><a href=\"" + string.Format(pageUrl, currentPage - 1) + "\"></a></span>");
            }

            /******************Xác định startPageNumbersFrom & stopPageNumbersAt**********************/
            if (startPageNumbersFrom < 1)
            {
                startPageNumbersFrom = 1;

                //As page numbers are starting at one, output an even number of pages.  
                stopPageNumbersAt = pagesToOutput;
            }

            if (stopPageNumbersAt > pageCount)
            {
                stopPageNumbersAt = pageCount;
            }

            if ((stopPageNumbersAt - startPageNumbersFrom) < pagesToOutput)
            {
                startPageNumbersFrom = stopPageNumbersAt - pagesToOutput;
                if (startPageNumbersFrom < 1)
                {
                    startPageNumbersFrom = 1;
                }
            }
            /******************End: Xác định startPageNumbersFrom & stopPageNumbersAt**********************/

            //Các dấu ... chỉ những trang phía trước  
            if (startPageNumbersFrom > 1)
            {
                output.Append("<a href=\"" + string.Format(GetPageUrl(currentPage - 1, pageUrl), startPageNumbersFrom - 1) + "\">&hellip;</a>");
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    output.Append("<a class=\"pagi-button current-active\">" + i.ToString() + "</a>");
                }
                else
                {
                    output.Append("<a class=\"pagi-button\" href=\"" + string.Format(pageUrl, i) + "\">" + i.ToString() + "</a>");
                }
            }

            //Các dấu ... chỉ những trang tiếp theo  
            if (stopPageNumbersAt < pageCount)
            {
                output.Append("<a href=\"" + string.Format(pageUrl, stopPageNumbersAt + 1) + "\">&hellip;</a>");
            }

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                //output.Append("<span class=\"Unselect_next\"><a href=\"" + string.Format(pageUrl, currentPage + 1) + "\"></a></span>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\"><i class=\"fa fa-angle-right\"></i></a></li>");
                output.Append("<a class=\"next-page pagi-button\" title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\">Next</a>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\">>|</a></li>");
            }
            //output.Append("</ul>");
            //output.Append("</div>");
            return output.ToString();
        }
        #endregion

        public class Danhsachorder
        {
            //public tbl_MainOder morder { get; set; }
            public int ID { get; set; }
            public string ProductImage { get; set; }
            public string ShopID { get; set; }
            public string ShopName { get; set; }
            public string Site { get; set; }
            public string TotalPriceVND { get; set; }
            public string AmountDeposit { get; set; }
            public string Deposit { get; set; }
            public int UID { get; set; }
            public string CreatedDate { get; set; }
            public string statusstring { get; set; }
            public string username { get; set; }
        }

        protected void btnSear_Click(object sender, EventArgs e)
        {
            int t = Request.QueryString["t"].ToInt(1);
            string text = txtSearhc.Text;
            string typesear = ddlType.SelectedValue;
            string status = ddlStatus.SelectedValue;
            string fd = "";
            string td = "";

            if (!string.IsNullOrEmpty(FD.Text))
            {
                fd = FD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(TD.Text))
            {
                td = TD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(td))
            {
                Response.Redirect("/danh-sach-don-hang?t=" + t + "&s=" + text + "&l=" + typesear + "&stt=" + status + "&fd=" + fd + "&td=" + td + "");
            }
            else
            {
                Response.Redirect("/danh-sach-don-hang?t=" + t + "&s=" + text + "&l=" + typesear + "&stt=" + status + "&fd=" + fd + "&td=" + td + "");
            }
        }

        protected void btnDeposit100_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int OID = hdfOrderID.Value.ToInt();
                if (OID > 0)
                {
                    int UID = obj_user.ID;
                    var o = MainOrderController.GetAllByUIDAndID(UID, OID);
                    if (o != null)
                    {
                        double userwallet = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);
                        if (userwallet > 0)
                        {
                            double orderdeposited = 0;
                            double amountdeposit = 0;
                            double pricevnd = 0;
                            double userbuypro = 0;
                            double phantramcoc = 0;
                            double userdeposit = 0;

                            if (o.Deposit.ToFloat(0) > 0)
                                orderdeposited = Math.Round(Convert.ToDouble(o.Deposit), 0);

                            if (o.PriceVND.ToFloat(0) > 0)
                                pricevnd = Math.Round(Convert.ToDouble(o.PriceVND), 0);

                            if (!string.IsNullOrEmpty(obj_user.Deposit.ToString()))
                                userdeposit = Convert.ToDouble(obj_user.Deposit);

                            if (userdeposit > 0)
                                phantramcoc = userdeposit;
                            else
                                phantramcoc = 100;

                            amountdeposit = pricevnd * phantramcoc / 100;

                            double custDeposit = amountdeposit - orderdeposited;

                            if (custDeposit > 0)
                            {
                                if (userwallet >= custDeposit)
                                {
                                    double wallet = userwallet - custDeposit;
                                    wallet = Math.Round(wallet, 0);
                                    int st = TransactionController.DepositAll(obj_user.ID, wallet, currentDate, obj_user.Username, o.ID, 2, o.Status.Value, amountdeposit.ToString(), custDeposit, obj_user.Username + " đã đặt cọc đơn hàng: " + o.ID, 1, 1, 2);
                                    if (st == 1)
                                    {
                                        double phantramdichvu = 0;
                                        double total = 0;

                                        if (!string.IsNullOrEmpty(obj_user.FeeBuyPro))
                                        {
                                            userbuypro = Convert.ToDouble(obj_user.FeeBuyPro);
                                        }
                                        if (userbuypro > 0)
                                        {
                                            phantramdichvu = userbuypro;
                                        }
                                        else if (pricevnd < 6000000)
                                        {
                                            phantramdichvu = 2;
                                        }
                                        else
                                        {
                                            phantramdichvu = 0.9;
                                        }
                                        double feebp = pricevnd * phantramdichvu / 100;
                                        if (feebp < 20000)
                                        {
                                            feebp = 20000;
                                        }
                                        total = pricevnd + feebp;

                                        MainOrderController.UpdateAmountDeposit(o.ID, amountdeposit.ToString());
                                        MainOrderController.UpdatePercentDeposit(o.ID, phantramcoc.ToString());
                                        MainOrderController.UpdateTotalFeeBuyPro(o.ID, total.ToString(), feebp.ToString());

                                        var setNoti = SendNotiEmailController.GetByID(6);
                                        if (setNoti != null)
                                        {
                                            if (setNoti.IsSentNotiAdmin == true)
                                            {
                                                var admins = AccountController.GetAllByRoleID(0);
                                                if (admins.Count > 0)
                                                {
                                                    foreach (var admin in admins)
                                                    {
                                                        NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", 1, currentDate, obj_user.Username, false);
                                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                        string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                        PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", datalink);
                                                    }
                                                }

                                                var managers = AccountController.GetAllByRoleID(2);
                                                if (managers.Count > 0)
                                                {
                                                    foreach (var manager in managers)
                                                    {
                                                        NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.",
                                                        1, currentDate, obj_user.Username, false);
                                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                        string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                        PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", datalink);
                                                    }
                                                }
                                            }
                                        }
                                        PJUtils.ShowMessageBoxSwAlert("Đặt cọc 100% đơn hàng thành công.", "s", true, Page);
                                    }
                                    else
                                        PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý.", "e", true, Page);
                                }
                                else
                                    PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này.", "i", true, Page);
                            }
                            else
                                PJUtils.ShowMessageBoxSwAlert("Đơn hàng này đã đặt cọc.", "i", true, Page);
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này.", "i", true, Page);
                        }
                    }
                }
            }
        }

        protected void btnDeposit80_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int OID = hdfOrderID.Value.ToInt();
                if (OID > 0)
                {
                    int UID = obj_user.ID;
                    var o = MainOrderController.GetAllByUIDAndID(UID, OID);
                    if (o != null)
                    {
                        double userwallet = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);
                        if (userwallet > 0)
                        {
                            double orderdeposited = 0;
                            double amountdeposit = 0;
                            double pricevnd = 0;
                            double userbuypro = 0;
                            double userdeposit = 0;
                            double phantramcoc = 0;

                            if (o.Deposit.ToFloat(0) > 0)
                                orderdeposited = Math.Round(Convert.ToDouble(o.Deposit), 0);

                            if (o.PriceVND.ToFloat(0) > 0)
                                pricevnd = Math.Round(Convert.ToDouble(o.PriceVND), 0);

                            if (!string.IsNullOrEmpty(obj_user.Deposit.ToString()))
                                userdeposit = Convert.ToDouble(obj_user.Deposit);

                            if (userdeposit > 0)
                                phantramcoc = userdeposit;
                            else
                                phantramcoc = 70;

                            amountdeposit = pricevnd * phantramcoc / 100;

                            double custDeposit = amountdeposit - orderdeposited;

                            if (custDeposit > 0)
                            {
                                if (userwallet >= custDeposit)
                                {
                                    double wallet = userwallet - custDeposit;
                                    wallet = Math.Round(wallet, 0);
                                    int st = TransactionController.DepositAll(obj_user.ID, wallet, currentDate, obj_user.Username, o.ID, 2, o.Status.Value, amountdeposit.ToString(), custDeposit, obj_user.Username + " đã đặt cọc đơn hàng: " + o.ID, 1, 1, 2);
                                    if (st == 1)
                                    {
                                        double phantramdichvu = 0;
                                        double total = 0;

                                        if (!string.IsNullOrEmpty(obj_user.FeeBuyPro))
                                        {
                                            userbuypro = Convert.ToDouble(obj_user.FeeBuyPro);
                                        }
                                        if (userbuypro > 0)
                                        {
                                            phantramdichvu = userbuypro;
                                        }
                                        else if (pricevnd < 6000000)
                                        {
                                            phantramdichvu = 2.5;
                                        }
                                        else
                                        {
                                            phantramdichvu = 1.8;
                                        }
                                        double feebp = pricevnd * phantramdichvu / 100;
                                        if (feebp < 20000)
                                        {
                                            feebp = 20000;
                                        }
                                        total = pricevnd + feebp;

                                        MainOrderController.UpdateAmountDeposit(o.ID, amountdeposit.ToString());
                                        MainOrderController.UpdatePercentDeposit(o.ID, phantramcoc.ToString());
                                        MainOrderController.UpdateTotalFeeBuyPro(o.ID, total.ToString(), feebp.ToString());

                                        var setNoti = SendNotiEmailController.GetByID(6);
                                        if (setNoti != null)
                                        {
                                            if (setNoti.IsSentNotiAdmin == true)
                                            {
                                                var admins = AccountController.GetAllByRoleID(0);
                                                if (admins.Count > 0)
                                                {
                                                    foreach (var admin in admins)
                                                    {
                                                        NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", 1, currentDate, obj_user.Username, false);
                                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                        string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                        PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", datalink);
                                                    }
                                                }

                                                var managers = AccountController.GetAllByRoleID(2);
                                                if (managers.Count > 0)
                                                {
                                                    foreach (var manager in managers)
                                                    {
                                                        NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.",
                                                        1, currentDate, obj_user.Username, false);
                                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                        string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                        PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", datalink);
                                                    }
                                                }
                                            }

                                            //if (setNoti.IsSentEmailAdmin == true)
                                            //{
                                            //    var admins = AccountController.GetAllByRoleID(0);
                                            //    if (admins.Count > 0)
                                            //    {
                                            //        foreach (var admin in admins)
                                            //        {
                                            //            try
                                            //            {
                                            //                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                            //                    "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được đặt cọc.", "");
                                            //            }
                                            //            catch { }
                                            //        }
                                            //    }

                                            //    var managers = AccountController.GetAllByRoleID(2);
                                            //    if (managers.Count > 0)
                                            //    {
                                            //        foreach (var manager in managers)
                                            //        {
                                            //            try
                                            //            {
                                            //                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                            //                    "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được đặt cọc.", "");
                                            //            }
                                            //            catch { }
                                            //        }
                                            //    }
                                            //}
                                        }
                                        PJUtils.ShowMessageBoxSwAlert("Đặt cọc 90% đơn hàng thành công.", "s", true, Page);
                                    }
                                    else
                                        PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý.", "e", true, Page);
                                }
                                else
                                    PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này.", "i", true, Page);
                            }
                            else
                                PJUtils.ShowMessageBoxSwAlert("Đơn hàng này đã đặt cọc.", "i", true, Page);
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này.", "i", true, Page);
                        }
                    }
                }
            }
        }

        protected void btnDeposit_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                if (obj_user.Wallet > 0)
                {
                    int OID = hdfOrderID.Value.ToInt();
                    if (OID > 0)
                    {
                        int uid = obj_user.ID;
                        var o = MainOrderController.GetAllByUIDAndID(uid, OID);
                        if (o != null)
                        {
                            double userwallet = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);

                            double orderdeposited = 0;
                            double amountdeposit = 0;

                            if (o.Deposit.ToFloat(0) > 0)
                                orderdeposited = Math.Round(Convert.ToDouble(o.Deposit), 0);

                            if (o.AmountDeposit.ToFloat(0) > 0)
                                amountdeposit = Math.Round(Convert.ToDouble(o.AmountDeposit), 0);

                            double custDeposit = amountdeposit - orderdeposited;

                            if (custDeposit > 0)
                            {
                                if (userwallet > 0)
                                {
                                    if (userwallet >= custDeposit)
                                    {
                                        //Cập nhật lại Wallet User

                                        double wallet = userwallet - custDeposit;
                                        wallet = Math.Round(wallet, 0);

                                        int st = TransactionController.DepositAll(obj_user.ID, wallet, currentDate, obj_user.Username, o.ID, 2, o.Status.Value, amountdeposit.ToString(), custDeposit, obj_user.Username + " đã đặt cọc đơn hàng: " + o.ID, 1, 1, 2);
                                        if (st == 1)
                                        {
                                            var setNoti = SendNotiEmailController.GetByID(6);
                                            if (setNoti != null)
                                            {
                                                if (setNoti.IsSentNotiAdmin == true)
                                                {

                                                    var admins = AccountController.GetAllByRoleID(0);
                                                    if (admins.Count > 0)
                                                    {
                                                        foreach (var admin in admins)
                                                        {
                                                            NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", 1, currentDate, obj_user.Username, false);
                                                            string strPathAndQuery = Request.Url.PathAndQuery;
                                                            string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                            string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                            PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", datalink);
                                                        }
                                                    }

                                                    var managers = AccountController.GetAllByRoleID(2);
                                                    if (managers.Count > 0)
                                                    {
                                                        foreach (var manager in managers)
                                                        {
                                                            NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.",
                                                            1, currentDate, obj_user.Username, false);
                                                            string strPathAndQuery = Request.Url.PathAndQuery;
                                                            string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                            string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                            PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", datalink);
                                                        }
                                                    }
                                                }

                                                //if (setNoti.IsSentEmailAdmin == true)
                                                //{
                                                //    var admins = AccountController.GetAllByRoleID(0);
                                                //    if (admins.Count > 0)
                                                //    {
                                                //        foreach (var admin in admins)
                                                //        {
                                                //            try
                                                //            {
                                                //                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                                //                    "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được đặt cọc.", "");
                                                //            }
                                                //            catch { }
                                                //        }
                                                //    }

                                                //    var managers = AccountController.GetAllByRoleID(2);
                                                //    if (managers.Count > 0)
                                                //    {
                                                //        foreach (var manager in managers)
                                                //        {
                                                //            try
                                                //            {
                                                //                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                                //                    "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được đặt cọc.", "");
                                                //            }
                                                //            catch { }
                                                //        }
                                                //    }
                                                //}
                                            }
                                            PJUtils.ShowMessageBoxSwAlert("Đặt cọc đơn hàng thành công.", "s", true, Page);
                                        }
                                        else
                                            PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý.", "e", true, Page);
                                    }
                                    else
                                    {
                                        PJUtils.ShowMessageBoxSwAlertBackToLink("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.", "e", true, "/chuyen-muc/huong-dan/nap-tien", Page);
                                    }
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlertBackToLink("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.", "e", true, "/chuyen-muc/huong-dan/nap-tien", Page);
                                }
                            }
                            else
                                PJUtils.ShowMessageBoxSwAlert("Đơn hàng này đã đặt cọc.", "i", true, Page);
                        }
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlertBackToLink("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.", "e", true, "/chuyen-muc/huong-dan/nap-tien", Page);
                }
            }
        }

        protected void btnDepositExtra_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                if (obj_user.Wallet > 0)
                {
                    int OID = hdfOrderID.Value.ToInt();
                    if (OID > 0)
                    {
                        int uid = obj_user.ID;
                        var o = MainOrderController.GetAllByUIDAndID(uid, OID);
                        if (o != null)
                        {
                            double orderdeposited = 0;
                            double amountdeposit = 0;

                            if (o.Deposit.ToFloat(0) > 0)
                                orderdeposited = Math.Round(Convert.ToDouble(o.Deposit), 0);

                            if (o.AmountDeposit.ToFloat(0) > 0)
                                amountdeposit = Math.Round(Convert.ToDouble(o.AmountDeposit), 0);
                            double custDeposit = amountdeposit - orderdeposited;
                            double userwallet = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);
                            if (userwallet > 0)
                            {
                                if (userwallet >= custDeposit)
                                {
                                    //Cập nhật lại Wallet User

                                    double wallet = userwallet - custDeposit;
                                    wallet = Math.Round(wallet, 0);

                                    int st = TransactionController.DepositExtra(obj_user.ID, wallet, currentDate, obj_user.Username, o.ID, 5, o.Status.Value, amountdeposit.ToString(), custDeposit, obj_user.Username + " đã đặt cọc đơn hàng: " + o.ID, 1, 1, 2);
                                    if (st == 1)
                                    {
                                        var wh = WarehouseController.GetByID(Convert.ToInt32(o.ReceivePlace));
                                        if (wh != null)
                                        {
                                            var ExpectedDate = currentDate.AddDays(Convert.ToInt32(wh.ExpectedDate));
                                            MainOrderController.UpdateExpectedDate(o.ID, ExpectedDate);
                                        }
                                        var setNoti = SendNotiEmailController.GetByID(6);
                                        if (setNoti != null)
                                        {
                                            if (setNoti.IsSentNotiAdmin == true)
                                            {

                                                var admins = AccountController.GetAllByRoleID(0);
                                                if (admins.Count > 0)
                                                {
                                                    foreach (var admin in admins)
                                                    {
                                                        NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc thêm.", 1, currentDate, obj_user.Username, false);
                                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                        string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                        PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã được đặt cọc thêm.", datalink);
                                                    }
                                                }

                                                var managers = AccountController.GetAllByRoleID(2);
                                                if (managers.Count > 0)
                                                {
                                                    foreach (var manager in managers)
                                                    {


                                                        NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc thêm.",
                                                        1, currentDate, obj_user.Username, false);
                                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                        string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                        PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + " đã được đặt cọc thêm.", datalink);
                                                    }
                                                }
                                            }

                                            if (setNoti.IsSentEmailAdmin == true)
                                            {
                                                var admins = AccountController.GetAllByRoleID(0);
                                                if (admins.Count > 0)
                                                {
                                                    foreach (var admin in admins)
                                                    {
                                                        try
                                                        {
                                                            PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                                                "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được đặt cọc thêm.", "");
                                                        }
                                                        catch { }
                                                    }
                                                }

                                                var managers = AccountController.GetAllByRoleID(2);
                                                if (managers.Count > 0)
                                                {
                                                    foreach (var manager in managers)
                                                    {
                                                        try
                                                        {
                                                            PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                                                "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được đặt cọc thêm.", "");
                                                        }
                                                        catch { }
                                                    }
                                                }

                                            }
                                        }
                                        PJUtils.ShowMessageBoxSwAlert("Đặt cọc thêm cho đơn hàng thành công.", "s", true, Page);
                                    }
                                    else
                                        PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý.", "e", true, Page);
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlertBackToLink("Số dư trong tài khoản của quý khách không đủ để đặt cọc thêm cho đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc thêm.", "e", true, "/chuyen-muc/huong-dan/nap-tien", Page);
                                }

                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlertBackToLink("Số dư trong tài khoản của quý khách không đủ để đặt cọc thêm cho đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc thêm.", "e", true, "/chuyen-muc/huong-dan/nap-tien", Page);
                            }
                        }

                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlertBackToLink("Số dư trong tài khoản của quý khách không đủ để đặt cọc thêm cho đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc thêm.", "e", true, "/chuyen-muc/huong-dan/nap-tien", Page);
                }
            }
        }

        protected void btnDepositSelected1_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int UID = obj_user.ID;
                double wallet = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);

                if (wallet > 0)
                {
                    var list = HttpContext.Current.Session["ListDep"] as List<DepAll>;
                    if (list != null)
                    {
                        if (list.Count > 0)
                        {
                            double totalMustPay = 0;
                            foreach (var item in list)
                            {
                                int orderID = item.MainOrderID;
                                var mainOrder = MainOrderController.GetAllByUIDAndID(UID, orderID);
                                if (mainOrder != null)
                                {
                                    if (mainOrder.Status == 0)
                                    {
                                        double Deposited = 0;
                                        double AmountDeposited = 0;
                                        if (mainOrder.Deposit.ToFloat(0) > 0)
                                        {
                                            Deposited = Math.Round(Convert.ToDouble(mainOrder.Deposit), 0);
                                        }
                                        if (mainOrder.AmountDeposit.ToFloat(0) > 0)
                                        {
                                            AmountDeposited = Math.Round(Convert.ToDouble(mainOrder.AmountDeposit), 0);
                                        }
                                        double mustDeposit = AmountDeposited - Deposited;
                                        if (mustDeposit > 0)
                                        {
                                            totalMustPay += mustDeposit;
                                        }
                                    }
                                }
                            }

                            if (wallet >= totalMustPay)
                            {
                                foreach (var item in list)
                                {
                                    int orderID = item.MainOrderID;
                                    var mainOrder = MainOrderController.GetAllByUIDAndID(UID, orderID);
                                    if (mainOrder != null)
                                    {
                                        if (mainOrder.Status == 0)
                                        {
                                            double Deposited = 0;
                                            double AmountDeposited = 0;
                                            if (mainOrder.Deposit.ToFloat(0) > 0)
                                            {
                                                Deposited = Math.Round(Convert.ToDouble(mainOrder.Deposit), 0);
                                            }
                                            if (mainOrder.AmountDeposit.ToFloat(0) > 0)
                                            {
                                                AmountDeposited = Math.Round(Convert.ToDouble(mainOrder.AmountDeposit), 0);
                                            }
                                            double mustDeposit = AmountDeposited - Deposited;
                                            int UIDOrder = Convert.ToInt32(mainOrder.UID);
                                            var accPay = AccountController.GetByID(UIDOrder);
                                            if (accPay != null)
                                            {
                                                double accWallet = Math.Round(Convert.ToDouble(accPay.Wallet), 0);
                                                if (mustDeposit > 0)
                                                {
                                                    if (accWallet >= mustDeposit)
                                                    {
                                                        double walletleft = accWallet - mustDeposit;
                                                        walletleft = Math.Round(walletleft, 0);
                                                        AccountController.updateWallet(obj_user.ID, walletleft, currentDate, obj_user.Username);
                                                        //Cập nhật lại MainOrder                                
                                                        MainOrderController.UpdateStatus(mainOrder.ID, obj_user.ID, 2);
                                                        MainOrderController.UpdateDeposit(mainOrder.ID, obj_user.ID, AmountDeposited.ToString());
                                                        MainOrderController.UpdateDepositDate(mainOrder.ID, currentDate);
                                                        HistoryPayWalletController.Insert(obj_user.ID, obj_user.Username, mainOrder.ID,
                                                        mustDeposit, obj_user.Username + " đã đặt cọc đơn hàng: " + mainOrder.ID + ".", walletleft, 1, 1, currentDate, obj_user.Username);
                                                        PayOrderHistoryController.Insert(mainOrder.ID, obj_user.ID, 2, mustDeposit, 2, currentDate, obj_user.Username);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                Session["ListDep"] = null;
                                PJUtils.ShowMessageBoxSwAlert("Đặt cọc đơn hàng thành công.", "s", true, Page);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.",
                                      "e", true, Page);
                            }
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Không có đơn hàng được chọn để đặt cọc.", "e", true, Page);
                        }
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Không có đơn hàng được chọn để đặt cọc.", "e", true, Page);
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.",
                                   "e", true, Page);
                }
            }
        }

        protected void btnPayAlllSelected_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int UID = obj_user.ID;
                double wallet = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);

                if (wallet > 0)
                {
                    var list = HttpContext.Current.Session["ListPay"] as List<PayAll>;
                    if (list != null)
                    {
                        if (list.Count > 0)
                        {
                            double totalMustPay = 0;
                            foreach (var item in list)
                            {
                                int orderID = item.MainOrderID;
                                var mainOrder = MainOrderController.GetAllByUIDAndID(UID, orderID);
                                if (mainOrder != null)
                                {
                                    if (mainOrder.Status == 7)
                                    {
                                        double Deposited = 0;
                                        if (mainOrder.Deposit.ToFloat(0) > 0)
                                        {
                                            Deposited = Math.Round(Convert.ToDouble(mainOrder.Deposit), 0);
                                        }

                                        double feewarehouse = 0;
                                        if (mainOrder.FeeInWareHouse.ToString().ToFloat(0) > 0)
                                            feewarehouse = Math.Round(Convert.ToDouble(mainOrder.FeeInWareHouse), 0);

                                        double totalPriceVND = 0;
                                        if (mainOrder.TotalPriceVND.ToFloat(0) > 0)
                                            totalPriceVND = Math.Round(Convert.ToDouble(mainOrder.TotalPriceVND), 0);
                                        double moneyleft = Math.Round((totalPriceVND + feewarehouse) - Deposited, 0);

                                        if (moneyleft > 0)
                                        {
                                            totalMustPay += moneyleft;
                                        }
                                    }
                                }
                            }

                            if (wallet >= totalMustPay)
                            {
                                foreach (var item in list)
                                {
                                    int orderID = item.MainOrderID;
                                    var mainOrder = MainOrderController.GetAllByUIDAndID(UID, orderID);
                                    if (mainOrder != null)
                                    {
                                        if (mainOrder.Status == 7)
                                        {
                                            double Deposited = 0;
                                            if (mainOrder.Deposit.ToFloat(0) > 0)
                                            {
                                                Deposited = Math.Round(Convert.ToDouble(mainOrder.Deposit), 0);
                                            }

                                            double feewarehouse = 0;
                                            if (mainOrder.FeeInWareHouse.ToString().ToFloat(0) > 0)
                                                feewarehouse = Math.Round(Convert.ToDouble(mainOrder.FeeInWareHouse), 0);

                                            double totalPriceVND = 0;
                                            if (mainOrder.TotalPriceVND.ToFloat(0) > 0)
                                                totalPriceVND = Math.Round(Convert.ToDouble(mainOrder.TotalPriceVND), 0);
                                            double moneyleft = Math.Round((totalPriceVND + feewarehouse) - Deposited, 0);

                                            int UIDOrder = Convert.ToInt32(mainOrder.UID);
                                            var accPay = AccountController.GetByID(UIDOrder);
                                            if (accPay != null)
                                            {
                                                double accWallet = Math.Round(Convert.ToDouble(accPay.Wallet), 0);
                                                if (moneyleft > 0)
                                                {
                                                    if (accWallet >= moneyleft)
                                                    {
                                                        double walletLeft = Math.Round(accWallet - moneyleft, 0);
                                                        double payalll = Math.Round(Deposited + moneyleft, 0);
                                                        walletLeft = Math.Round(walletLeft, 0);
                                                        MainOrderController.UpdateStatus(mainOrder.ID, UIDOrder, 9);
                                                        MainOrderController.UpdatePayDate(mainOrder.ID, currentDate);
                                                        AccountController.updateWallet(UIDOrder, walletLeft, currentDate, accPay.Username);

                                                        HistoryOrderChangeController.Insert(mainOrder.ID, UIDOrder, accPay.Username, accPay.Username +
                                                        " đã đổi trạng thái của đơn hàng ID là: " + mainOrder.ID + ", từ: Chờ thanh toán, sang: Khách đã thanh toán.", 1, currentDate);

                                                        HistoryPayWalletController.Insert(UIDOrder, accPay.Username, mainOrder.ID, moneyleft, accPay.Username + " đã thanh toán đơn hàng: " + mainOrder.ID + ".", walletLeft, 1, 3, currentDate, accPay.Username);
                                                        MainOrderController.UpdateDeposit(mainOrder.ID, UIDOrder, payalll.ToString());
                                                        PayOrderHistoryController.Insert(mainOrder.ID, UIDOrder, 9, moneyleft, 2, currentDate, accPay.Username);

                                                        //var wh = WarehouseController.GetByID(Convert.ToInt32(mainOrder.ReceivePlace));
                                                        //if (wh != null)
                                                        //{
                                                        //    var ExpectedDate = currentDate.AddDays(Convert.ToInt32(wh.ExpectedDate));
                                                        //    MainOrderController.UpdateExpectedDate(mainOrder.ID, ExpectedDate);
                                                        //}
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                Session["ListPay"] = null;
                                PJUtils.ShowMessageBoxSwAlert("Thanh toán đơn hàng thành công.", "s", true, Page);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để thanh toán đơn hàng. Quý khách vui lòng nạp thêm tiền để tiến hành thanh toán.",
                                      "e", true, Page);
                            }
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Không có đơn hàng được chọn để thanh toán.", "e", true, Page);
                        }
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Không có đơn hàng được chọn để thanh toán.", "e", true, Page);
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để thanh toán đơn hàng. Quý khách vui lòng nạp thêm tiền để tiến hành thanh toán.",
                                   "e", true, Page);
                }
            }
        }
        [Serializable()]
        public class DepAll
        {
            public int MainOrderID { get; set; }
            public double TotalDeposit { get; set; }
        }
        [Serializable()]
        public class PayAll
        {
            public int MainOrderID { get; set; }
            public double TotalPricePay { get; set; }
        }
        [Serializable()]
        public class YCG
        {
            public int MainOrderID { get; set; }
        }

        [WebMethod]
        public static string CheckDepAll(int MainOrderID, string TotalPrice)
        {
            List<DepAll> ldep = new List<DepAll>();
            var list = HttpContext.Current.Session["ListDep"] as List<DepAll>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    var check = list.Where(x => x.MainOrderID == MainOrderID).FirstOrDefault();
                    if (check != null)
                    {
                        list.Remove(check);
                    }
                    else
                    {
                        DepAll d = new DepAll();
                        d.MainOrderID = MainOrderID;
                        d.TotalDeposit = Convert.ToDouble(TotalPrice);
                        list.Add(d);
                    }
                }
                else
                {
                    DepAll d = new DepAll();
                    d.MainOrderID = MainOrderID;
                    d.TotalDeposit = Convert.ToDouble(TotalPrice);
                    list.Add(d);
                    //HttpContext.Current.Session["ListDep"] = ldep;
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(list);
            }
            else
            {
                DepAll d = new DepAll();
                d.MainOrderID = MainOrderID;
                d.TotalDeposit = Convert.ToDouble(TotalPrice);
                ldep.Add(d);
                HttpContext.Current.Session["ListDep"] = ldep;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ldep);
            }
        }

        [WebMethod]
        public static string CheckPayAll(int MainOrderID, string TotalPricePay)
        {
            List<PayAll> lpay = new List<PayAll>();
            var list = HttpContext.Current.Session["ListPay"] as List<PayAll>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    var check = list.Where(x => x.MainOrderID == MainOrderID).FirstOrDefault();
                    if (check != null)
                    {
                        list.Remove(check);
                    }
                    else
                    {
                        PayAll d = new PayAll();
                        d.MainOrderID = MainOrderID;
                        d.TotalPricePay = Convert.ToDouble(TotalPricePay);
                        list.Add(d);
                    }
                }
                else
                {
                    PayAll d = new PayAll();
                    d.MainOrderID = MainOrderID;
                    d.TotalPricePay = Convert.ToDouble(TotalPricePay);
                    list.Add(d);
                    // HttpContext.Current.Session["ListPay"] = lpay;
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(list);
            }
            else
            {
                PayAll d = new PayAll();
                d.MainOrderID = MainOrderID;
                d.TotalPricePay = Convert.ToDouble(TotalPricePay);
                lpay.Add(d);
                HttpContext.Current.Session["ListPay"] = lpay;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(lpay);
            }
        }

        [WebMethod]
        public static string CheckYCGAll(int MainOrderID)
        {
            List<YCG> lYCG = new List<YCG>();
            var list = HttpContext.Current.Session["ListYCG"] as List<YCG>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    var check = list.Where(x => x.MainOrderID == MainOrderID).FirstOrDefault();
                    if (check != null)
                    {
                        list.Remove(check);
                    }
                    else
                    {
                        YCG d = new YCG();
                        d.MainOrderID = MainOrderID;
                        list.Add(d);
                    }
                }
                else
                {
                    YCG d = new YCG();
                    d.MainOrderID = MainOrderID;
                    list.Add(d);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(list);
            }
            else
            {
                YCG d = new YCG();
                d.MainOrderID = MainOrderID;
                lYCG.Add(d);
                HttpContext.Current.Session["ListYCG"] = lYCG;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(lYCG);
            }
        }

        protected void btnYCG_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int UID = obj_user.ID;
                var list = HttpContext.Current.Session["ListYCG"] as List<YCG>;
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            var mo = MainOrderController.GetAllByID(item.MainOrderID);
                            if (mo != null)
                            {
                                var check = YCGController.GetByMainOrderID(item.MainOrderID);
                                if (check == null)
                                {
                                    YCGController.Insert(item.MainOrderID, txtFullName.Text, txtPhone.Text, txtAddress.Text, txtNote.Text, username_current, currentDate);
                                    MainOrderController.UpdateYCG(item.MainOrderID, true);

                                    var setNoti = SendNotiEmailController.GetByID(6);
                                    if (setNoti != null)
                                    {
                                        if (setNoti.IsSentNotiAdmin == true)
                                        {

                                            var admins = AccountController.GetAllByRoleID(0);
                                            if (admins.Count > 0)
                                            {
                                                foreach (var admin in admins)
                                                {
                                                    NotificationsController.Inser(admin.ID, admin.Username, item.MainOrderID, "Đơn hàng " + item.MainOrderID + " đã yêu cầu giao hàng.", 1, currentDate, obj_user.Username, false);
                                                    string strPathAndQuery = Request.Url.PathAndQuery;
                                                    string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                    string datalink = "" + strUrl + "manager/OrderDetail/" + item.MainOrderID;
                                                    PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + item.MainOrderID + " đã yêu cầu giao hàng.", datalink);
                                                }
                                            }

                                            var managers = AccountController.GetAllByRoleID(2);
                                            if (managers.Count > 0)
                                            {
                                                foreach (var manager in managers)
                                                {


                                                    NotificationsController.Inser(manager.ID, manager.Username, item.MainOrderID, "Đơn hàng " + item.MainOrderID + " đã yêu cầu giao hàng.",
                                                    1, currentDate, obj_user.Username, false);
                                                    string strPathAndQuery = Request.Url.PathAndQuery;
                                                    string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                    string datalink = "" + strUrl + "manager/OrderDetail/" + item.MainOrderID;
                                                    PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + item.MainOrderID + " đã yêu cầu giao hàng.", datalink);
                                                }
                                            }
                                        }

                                        if (setNoti.IsSentEmailAdmin == true)
                                        {
                                            var admins = AccountController.GetAllByRoleID(0);
                                            if (admins.Count > 0)
                                            {
                                                foreach (var admin in admins)
                                                {
                                                    try
                                                    {
                                                        PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                                            "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + item.MainOrderID + " đã yêu cầu giao hàng.", "");
                                                    }
                                                    catch { }
                                                }
                                            }

                                            var managers = AccountController.GetAllByRoleID(2);
                                            if (managers.Count > 0)
                                            {
                                                foreach (var manager in managers)
                                                {
                                                    try
                                                    {
                                                        PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                                            "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + item.MainOrderID + " đã yêu cầu giao hàng.", "");
                                                    }
                                                    catch { }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        Session["ListYCG"] = null;
                        PJUtils.ShowMessageBoxSwAlert("Tạo yêu cầu giao hàng thành công.", "s", true, Page);
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Chưa có đơn hàng nào được chọn, vui lòng thử lại.", "e", false, Page);
                }
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int uid = obj_user.ID;
                int id = hdfOrderID.Value.ToInt();
                DateTime currentDate = DateTime.Now;
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByUIDAndID(uid, id);
                    if (o != null)
                    {
                        double deposit = 0;
                        if (o.Deposit.ToFloat(0) > 0)
                            deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);

                        double wallet = 0;
                        if (obj_user.Wallet.ToString().ToFloat(0) > 0)
                            wallet = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);

                        double feewarehouse = 0;
                        if (o.FeeInWareHouse.ToString().ToFloat(0) > 0)
                            feewarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);

                        double totalPriceVND = 0;
                        if (o.TotalPriceVND.ToFloat(0) > 0)
                            totalPriceVND = Math.Round(Convert.ToDouble(o.TotalPriceVND), 0);
                        double moneyleft = Math.Round((totalPriceVND + feewarehouse) - deposit, 0);

                        if (wallet >= moneyleft)
                        {
                            if (moneyleft > 0)
                            {
                                double walletLeft = Math.Round(wallet - moneyleft, 0);
                                double payalll = Math.Round(deposit + moneyleft, 0);

                                int st = TransactionController.PayAll(o.ID, wallet, o.Status.ToString().ToInt(0), uid, currentDate, username, deposit, 1, moneyleft, 1, 3, 2);
                                if (st == 1)
                                {
                                    var setNoti = SendNotiEmailController.GetByID(11);
                                    if (setNoti != null)
                                    {
                                        if (setNoti.IsSentNotiAdmin == true)
                                        {
                                            var admins = AccountController.GetAllByRoleID(0);
                                            if (admins.Count > 0)
                                            {
                                                foreach (var admin in admins)
                                                {
                                                    NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã được thanh toán.",
                                                    1, currentDate, obj_user.Username, false);
                                                    string strPathAndQuery = Request.Url.PathAndQuery;
                                                    string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                    string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                    PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã được thanh toán.", datalink);
                                                }
                                            }

                                            var managers = AccountController.GetAllByRoleID(2);
                                            if (managers.Count > 0)
                                            {
                                                foreach (var manager in managers)
                                                {
                                                    NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã được thanh toán.",
                                                    1, currentDate, obj_user.Username, false);
                                                    string strPathAndQuery = Request.Url.PathAndQuery;
                                                    string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                    string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                    PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + " đã được thanh toán.", datalink);
                                                }
                                            }
                                        }

                                        //if (setNoti.IsSentEmailAdmin == true)
                                        //{
                                        //    var admins = AccountController.GetAllByRoleID(0);
                                        //    if (admins.Count > 0)
                                        //    {
                                        //        foreach (var admin in admins)
                                        //        {
                                        //            try
                                        //            {
                                        //                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                        //                    "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được thanh toán.", "");
                                        //            }
                                        //            catch { }
                                        //        }
                                        //    }

                                        //    var managers = AccountController.GetAllByRoleID(2);
                                        //    if (managers.Count > 0)
                                        //    {
                                        //        foreach (var manager in managers)
                                        //        {
                                        //            try
                                        //            {
                                        //                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                        //                    "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được thanh toán.", "");
                                        //            }
                                        //            catch { }
                                        //        }
                                        //    }
                                        //}
                                    }
                                    PJUtils.ShowMessageBoxSwAlert("Thanh toán thành công.", "s", true, Page);
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý, vui lòng thử lại sau.", "e", true, Page);
                                }
                            }


                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Số tiền trong tài khoản của bạn không đủ để thanh toán đơn hàng.", "e", true, Page);
                        }
                    }
                }
            }
        }

        protected void btnDepositAll_Click(object sender, EventArgs e)
        {
            int t = ViewState["t"].ToString().ToInt(0);
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                var setNoti = SendNotiEmailController.GetByID(6);
                if (obj_user.Wallet > 0)
                {
                    var lmain = MainOrderController.GetAllByUID_Deposit(obj_user.ID, t);
                    if (lmain.Count > 0)
                    {
                        double TongTienPhaiCoc = Math.Round(MainOrderController.GetTotalPrice(obj_user.ID, 0, "AmountDeposit", t), 0);
                        double TienKhachHang = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);
                        if (TienKhachHang >= TongTienPhaiCoc)
                        {
                            foreach (var o in lmain)
                            {
                                var user = AccountController.GetByID(obj_user.ID);
                                if (user != null)
                                {
                                    if (o.Status == 0 && Convert.ToDouble(o.Deposit) < Convert.ToDouble(o.AmountDeposit) && Convert.ToDouble(o.TotalPriceVND) > 0)
                                    {
                                        double orderdeposited = 0;
                                        double amountdeposit = 0;
                                        if (!string.IsNullOrEmpty(o.Deposit))
                                            orderdeposited = Math.Round(Convert.ToDouble(o.Deposit), 0);
                                        if (!string.IsNullOrEmpty(o.AmountDeposit))
                                            amountdeposit = Math.Round(Convert.ToDouble(o.AmountDeposit), 0);
                                        double custDeposit = amountdeposit - orderdeposited;
                                        double userwallet = Convert.ToDouble(user.Wallet);
                                        if (userwallet > 0)
                                        {
                                            if (userwallet >= custDeposit)
                                            {
                                                double wallet = userwallet - custDeposit;
                                                wallet = Math.Round(wallet, 0);
                                                AccountController.updateWallet(obj_user.ID, wallet, currentDate, obj_user.Username);
                                                //Cập nhật lại MainOrder                                
                                                MainOrderController.UpdateStatus(o.ID, obj_user.ID, 2);
                                                MainOrderController.UpdateDepositDate(o.ID, currentDate);
                                                MainOrderController.UpdateDeposit(o.ID, obj_user.ID, amountdeposit.ToString());
                                                HistoryPayWalletController.Insert(obj_user.ID, obj_user.Username, o.ID,
                                                 custDeposit, obj_user.Username + " đã đặt cọc đơn hàng: " + o.ID + ".", wallet, 1, 1, currentDate, obj_user.Username);
                                                PayOrderHistoryController.Insert(o.ID, obj_user.ID, 2, custDeposit, 2, currentDate, obj_user.Username);

                                                var wh = WarehouseController.GetByID(Convert.ToInt32(o.ReceivePlace));
                                                if (wh != null)
                                                {
                                                    var ExpectedDate = currentDate.AddDays(Convert.ToInt32(wh.ExpectedDate));
                                                    MainOrderController.UpdateExpectedDate(o.ID, ExpectedDate);
                                                }

                                                if (setNoti != null)
                                                {
                                                    if (setNoti.IsSentNotiAdmin == true)
                                                    {

                                                        var admins = AccountController.GetAllByRoleID(0);
                                                        if (admins.Count > 0)
                                                        {
                                                            foreach (var admin in admins)
                                                            {
                                                                NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", 1, currentDate, obj_user.Username, false);
                                                                string strPathAndQuery = Request.Url.PathAndQuery;
                                                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                                string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                                PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", datalink);
                                                            }
                                                        }

                                                        var managers = AccountController.GetAllByRoleID(2);
                                                        if (managers.Count > 0)
                                                        {
                                                            foreach (var manager in managers)
                                                            {


                                                                NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.",
                                                                1, currentDate, obj_user.Username, false);
                                                                string strPathAndQuery = Request.Url.PathAndQuery;
                                                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                                string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                                PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", datalink);
                                                            }
                                                        }
                                                    }

                                                    //if (setNoti.IsSentEmailAdmin == true)
                                                    //{
                                                    //    var admins = AccountController.GetAllByRoleID(0);
                                                    //    if (admins.Count > 0)
                                                    //    {
                                                    //        foreach (var admin in admins)
                                                    //        {
                                                    //            try
                                                    //            {
                                                    //                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                                    //                    "Thông báo tại Minh An Express.", "Đơn hàng " + o.ID + " đã được đặt cọc.", "");
                                                    //            }
                                                    //            catch { }
                                                    //        }
                                                    //    }

                                                    //    var managers = AccountController.GetAllByRoleID(2);
                                                    //    if (managers.Count > 0)
                                                    //    {
                                                    //        foreach (var manager in managers)
                                                    //        {
                                                    //            try
                                                    //            {
                                                    //                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                                    //                    "Thông báo tại Minh An Express.", "Đơn hàng " + o.ID + " đã được đặt cọc.", "");
                                                    //            }
                                                    //            catch { }
                                                    //        }
                                                    //    }

                                                    //}
                                                }
                                            }
                                            else
                                            {
                                                PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.",
                                                "e", true, Page);
                                            }

                                            PJUtils.ShowMessageBoxSwAlert("Đặt cọc đơn hàng thành công.", "s", true, Page);
                                        }
                                        else
                                        {
                                            PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.",
                                                "e", true, Page);
                                        }
                                    }
                                }

                            }
                            PJUtils.ShowMessageBoxSwAlert("Đặt cọc thành công!", "s", true, Page);
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Tài khoản không đủ tiền để đặt cọc tất cả.", "i", true, Page);
                        }
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Không có đơn hàng nào chờ đặt cọc.", "i", true, Page);
                    }
                }
            }

        }

        protected void btnPayAll_Click(object sender, EventArgs e)
        {
            int t = ViewState["t"].ToString().ToInt(0);
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                var setNoti = SendNotiEmailController.GetByID(11);
                if (obj_user.Wallet > 0)
                {
                    var lmain = MainOrderController.GetAllByUID_Payall(obj_user.ID, t);
                    if (lmain.Count > 0)
                    {
                        double TotalMustPay = 0;
                        foreach (var o in lmain)
                        {
                            double userdadeposit = Convert.ToDouble(o.Deposit);
                            double feewarehouse = 0;
                            if (o.FeeInWareHouse != null)
                                feewarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);
                            double totalPriceVND = Math.Round(Convert.ToDouble(o.TotalPriceVND), 0);
                            double totalPay = totalPriceVND + feewarehouse;
                            double moneyleft = totalPay - userdadeposit;
                            TotalMustPay += moneyleft;
                        }

                        if (Convert.ToDouble(obj_user.Wallet) >= TotalMustPay)
                        {
                            foreach (var o in lmain)
                            {
                                var user = AccountController.GetByID(obj_user.ID);
                                if (user != null)
                                {
                                    double wallet = 0;
                                    if (user.Wallet.ToString().ToFloat(0) > 0)
                                        wallet = Math.Round(Convert.ToDouble(user.Wallet), 0);
                                    double userdadeposit = Convert.ToDouble(o.Deposit);
                                    double feewarehouse = 0;
                                    if (o.FeeInWareHouse != null)
                                        feewarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);
                                    double totalPriceVND = Math.Round(Convert.ToDouble(o.TotalPriceVND), 0);
                                    double totalPay = totalPriceVND + feewarehouse;
                                    double moneyleft = totalPay - userdadeposit;

                                    if (moneyleft > 0)
                                    {
                                        double deposit = 0;
                                        if (o.Deposit.ToFloat(0) > 0)
                                            deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);

                                        if (wallet > 0)
                                        {
                                            if (wallet >= moneyleft)
                                            {
                                                double walletLeft = Math.Round(wallet - moneyleft, 0);
                                                double payalll = Math.Round(deposit + moneyleft, 0);

                                                MainOrderController.UpdateStatus(o.ID, obj_user.ID, 9);
                                                MainOrderController.UpdatePayDate(o.ID, currentDate);

                                                AccountController.updateWallet(obj_user.ID, walletLeft, currentDate, obj_user.Username);
                                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                " đã đổi trạng thái của đơn hàng ID là: " + o.ID + ", từ: Chờ thanh toán, sang: Khách đã thanh toán.", 1, currentDate);
                                                HistoryPayWalletController.Insert(obj_user.ID, obj_user.Username, o.ID, moneyleft, obj_user.Username + " đã thanh toán đơn hàng: " + o.ID + ".", walletLeft, 1, 3, currentDate, obj_user.Username);
                                                MainOrderController.UpdateDeposit(o.ID, obj_user.ID, payalll.ToString());
                                                PayOrderHistoryController.Insert(o.ID, obj_user.ID, 9, moneyleft, 2, currentDate, obj_user.Username);

                                                if (setNoti != null)
                                                {
                                                    if (setNoti.IsSentNotiAdmin == true)
                                                    {

                                                        var admins = AccountController.GetAllByRoleID(0);
                                                        if (admins.Count > 0)
                                                        {
                                                            foreach (var admin in admins)
                                                            {
                                                                NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã được thanh toán.", 1,
                                                                    currentDate, obj_user.Username, false);
                                                                string strPathAndQuery = Request.Url.PathAndQuery;
                                                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                                string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                                PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã được thanh toán.", datalink);
                                                            }
                                                        }

                                                        var managers = AccountController.GetAllByRoleID(2);
                                                        if (managers.Count > 0)
                                                        {
                                                            foreach (var manager in managers)
                                                            {
                                                                NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã được thanh toán.",
    1, currentDate, obj_user.Username, false);
                                                                string strPathAndQuery = Request.Url.PathAndQuery;
                                                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                                string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                                PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + " đã được thanh toán.", datalink);
                                                            }
                                                        }
                                                    }

                                                    if (setNoti.IsSentEmailAdmin == true)
                                                    {
                                                        var admins = AccountController.GetAllByRoleID(0);
                                                        if (admins.Count > 0)
                                                        {
                                                            foreach (var admin in admins)
                                                            {
                                                                try
                                                                {
                                                                    PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                                                        "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được thanh toán.", "");
                                                                }
                                                                catch { }
                                                            }
                                                        }

                                                        var managers = AccountController.GetAllByRoleID(2);
                                                        if (managers.Count > 0)
                                                        {
                                                            foreach (var manager in managers)
                                                            {
                                                                try
                                                                {
                                                                    PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                                                        "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được thanh toán.", "");
                                                                }
                                                                catch { }
                                                            }
                                                        }

                                                    }
                                                }
                                            }

                                        }

                                    }
                                }

                            }
                            PJUtils.ShowMessageBoxSwAlert("Thanh toán thành công!", "s", true, Page);
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Số dư trong tài khoản của quý khách không đủ để Thanh toán tất cả. Quý khách vui lòng nạp thêm tiền để tiến hành thanh toán.",
                                "e", true, Page);
                        }
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Không có đơn hàng nào chờ thanh toán!", "e", true, Page);
                    }
                }
            }
            //Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }


        protected void btnExcel_Click(object sender, EventArgs e)
        {
            int t = Request.QueryString["t"].ToInt(1);
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            int UID = obj_user.ID;
            string se = Request.QueryString["s"];
            int typesearch = Request.QueryString["l"].ToInt(0);
            int status = Request.QueryString["stt"].ToInt(-1);
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];

            txtSearhc.Text = se;
            ddlType.SelectedValue = typesearch.ToString();
            ViewState["t"] = t.ToString();
            //var os = MainOrderController.GetAllByUIDNotHidden_SqlHelper(UID, status, fd, td);



            if (!string.IsNullOrEmpty(Request.QueryString["fd"]))
            {
                FD.Text = fd;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["td"]))
                TD.Text = td;

            int page = 0;

            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }

            var os = MainOrderController.GetAllByUIDNotHidden_SqlHelperNew_Excel(UID, se, typesearch, status, fd, td, t);
            if (os.Count > 0)
            {
                StringBuilder StrExport = new StringBuilder();
                StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
                StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
                StrExport.Append("<DIV  style='font-size:12px;'>");
                StrExport.Append("<table border=\"1\">");
                StrExport.Append("  <tr>");
                StrExport.Append("      <th><strong>ID</strong></th>");
                StrExport.Append("      <th><strong>Tổng link</strong></th>");
                StrExport.Append("      <th><strong>Website</strong></th>");
                StrExport.Append("      <th><strong>Tổng tiền</strong></th>");
                StrExport.Append("      <th><strong>Cọc (%)</strong></th>");
                StrExport.Append("      <th><strong>Số tiền phải cọc</strong></th>");
                StrExport.Append("      <th><strong>Số tiền đã cọc</strong></th>");
                StrExport.Append("      <th><strong>Ngày đặt</strong></th>");
                StrExport.Append("      <th><strong>Trạng thái</strong></th>");
                StrExport.Append("  </tr>");
                foreach (var item in os)
                {
                    int status1 = Convert.ToInt32(item.Status);
                    StrExport.Append("  <tr>");
                    StrExport.Append("      <td>" + item.ID + "</td>");
                    StrExport.Append("      <td>" + item.TotalLink + "</td>");
                    StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + item.Site + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPriceVND)) + "</td>");
                    StrExport.Append("      <td>" + Math.Round(Convert.ToDouble(item.AmountDeposit) * 100 / Convert.ToDouble(item.PriceVND)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.AmountDeposit)) + "</td>");
                    StrExport.Append("      <td>" + string.Format("{0:N0}", Convert.ToDouble(item.Deposit)) + "</td>");
                    StrExport.Append("      <td>");
                    StrExport.Append(item.Created);
                    StrExport.Append(item.DepostiDate);
                    StrExport.Append(item.DateBuy);
                    StrExport.Append(item.DateBuyOK);
                    StrExport.Append(item.DateShipper);
                    StrExport.Append(item.DateTQ);
                    StrExport.Append(item.DateToVN);
                    StrExport.Append(item.DateVN);
                    StrExport.Append(item.DateToShip);
                    StrExport.Append(item.DatePay);
                    StrExport.Append(item.CompleteDate);
                    StrExport.Append(item.DateToCancel);

                    StrExport.Append("      </td>");
                    if (item.OrderType == 3)
                    {
                        if (item.IsCheckNotiPrice == true)
                        {
                            StrExport.Append("<td>Chờ báo giá</td>");
                        }
                        else
                        {
                            StrExport.Append("<td>" + PJUtils.IntToRequestAdminNew(status1) + "</td>");
                        }
                    }
                    else
                    {
                        StrExport.Append("<td >" + PJUtils.IntToRequestAdminNew(status1) + "</td>");
                    }
                    StrExport.Append("  </tr>");

                }
                StrExport.Append("</table>");
                StrExport.Append("</div></body></html>");
                string strFile = "Danh-sach-don-hang.xls";
                string strcontentType = "application/vnd.ms-excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(StrExport.ToString());
                Response.Flush();
                //Response.Close();
                Response.End();
            }
        }




    }
}