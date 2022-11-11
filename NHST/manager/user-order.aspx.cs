using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLADIPJ.Business;
using Telerik.Web.UI;
using MB.Extensions;
using System.Text;
using static NHST.Controllers.MainOrderController;
using System.Text.RegularExpressions;

namespace NHST.manager
{
    public partial class user_order : System.Web.UI.Page
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
                    if (ac != null)
                        if (ac.RoleID != 2 && ac.RoleID != 0 && ac.RoleID != 6)
                            Response.Redirect("/trang-chu");
                    LoadData();
                }
            }
        }

        private void LoadData()
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            int uID = 0;
            int stype = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["stype"]))
            {
                stype = int.Parse(Request.QueryString["stype"]);
                ddlType.SelectedValue = stype.ToString();
            }

            string fd = Request.QueryString["fd"];
            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;
            string priceTo = Request.QueryString["priceTo"];
            if (!string.IsNullOrEmpty(priceTo))
                rPriceTo.Text = priceTo;
            string priceFrom = Request.QueryString["priceFrom"];
            if (!string.IsNullOrEmpty(priceFrom))
                rPriceFrom.Text = priceFrom;
            string search = "";

            int hasVMD = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["hasMVD"]))
            {
                hasVMD = Request.QueryString["hasMVD"].ToString().ToInt(0);
                hdfCheckBox.Value = hasVMD.ToString();
            }



            string st = Request.QueryString["st"];
            if (!string.IsNullOrEmpty(st))
            {
                var list = st.Split(',').ToList();

                for (int j = 0; j < list.Count; j++)
                {
                    for (int i = 0; i < ddlStatuss.Items.Count; i++)
                    {
                        var item = ddlStatuss.Items[i];
                        if (item.Value == list[j])
                        {
                            ddlStatuss.Items[i].Selected = true;
                        }
                    }
                }

            }
            //if (!string.IsNullOrEmpty(st))
            //    ddlStatuss.SelectedIndex = st.ToInt(0);
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                tSearchName.Text = search;
            }
            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            if (Request.QueryString["uid"] != null)
            {
                uID = Request.QueryString["uid"].ToInt(0);
            }
            var user = AccountController.GetByID(uID);
            if (uID > 0)
            {
                var total = MainOrderController.GetTotalNewOfDK(uID, search, stype, fd, td, priceFrom, priceTo, st, Convert.ToBoolean(hasVMD), Convert.ToInt32(user.RoleID), ac.ID);
                var la = MainOrderController.GetByUserIDInSQLHelperWithFilterNew(uID, search, stype, fd, td, priceFrom, priceTo, st, Convert.ToBoolean(hasVMD), page, 10,Convert.ToInt32(user.RoleID), ac.ID);
                pagingall(la, total);
            }
           
            ltrUsername.Text = user.Username;



        }
        #region Pagging
        public void pagingall(List<OrderGetSQL> acs, int total)
        {
            int PageSize = 10;
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
                StringBuilder hcm = new StringBuilder();
                double tongtien = 0;
                double tiendacoc = 0;
                double tienchuathanhtoan;
                int UIDD = 0;
                if (Request.QueryString["uid"] != null)
                {
                    UIDD = Request.QueryString["uid"].ToInt(0);
                }
                List<SQLsumtotal> totalprice = new List<SQLsumtotal>();

                totalprice = MainOrderController.GetByUsernameInSQLHelper_Sumtotal(UIDD);
                foreach (var mo in totalprice)
                {
                    tongtien += Convert.ToDouble(mo.tongtienhang);
                    tiendacoc += Convert.ToDouble(mo.tongtiencoc);
                }
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];
                    //if (item.Status >= 2)
                    //{
                    //    tongtien += Convert.ToDouble(item.TotalPriceVND);
                    //    tiendacoc += Convert.ToDouble(item.Deposit);
                    //}
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.anhsanpham + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPriceVND)) + " VNĐ</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.Deposit)) + " VNĐ</td>");
                    hcm.Append("<td>" + item.Uname + "</td>");
                    hcm.Append("<td>" + item.dathang + "</td>");
                    hcm.Append("<td>" + item.saler + "</td>");
                    hcm.Append("<td>" + item.CreatedDate + "</td>");
                    hcm.Append("<td>" + item.hasSmallpackage + "</td>");
                    hcm.Append("<td>" + item.statusstring + "</td>");
                    hcm.Append("<td>");
                    hcm.Append(" <div class=\"action-table\">");
                    hcm.Append("<a href = \"OrderDetail.aspx?id=" + item.ID + "\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Cập nhật\"><i class=\"material-icons\">edit</i></a>");

                    hcm.Append("<a href = \"Pay-Order.aspx?id=" + item.ID + "\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Thanh toán ngay\"><i class=\"material-icons\">payment</i></a>");
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    hcm.Append("</tr>");
                }
                tienchuathanhtoan = tongtien - tiendacoc;
                ltr.Text = hcm.ToString();
                ltrTongTien.Text = string.Format("{0:N0}", tongtien) + " VNĐ";
                ltrTienDaThanhToan.Text = string.Format("{0:N0}", tiendacoc) + " VNĐ";
                ltrTienChuaThanhToan.Text = string.Format("{0:N0}", tienchuathanhtoan) + " VNĐ";

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
                if (pageUrl.IndexOf("Page=") > 0)
                {
                    int a = pageUrl.IndexOf("Page=");
                    int b = pageUrl.Length;
                    pageUrl.Remove(a, b - a);
                }
                else
                {
                    pageUrl += "&Page={0}";
                }

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

        #region grid event

        bool isGrouping = false;
        protected void r_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //int uID = 0;
            //if (Request.QueryString["uid"] != null)
            //{
            //    uID = Request.QueryString["uid"].ToInt(0);
            //}
            //if (uID > 0)
            //{
            //    string username_current = Session["userLoginSystem"].ToString();
            //    tbl_Account ac = AccountController.GetByUsername(username_current);
            //    if (ac != null)
            //    {
            //        string s = tSearchName.Text.Trim();
            //        int type = ddlType.SelectedValue.ToString().ToInt(1);
            //        double priceFrom = Convert.ToDouble(rPriceFrom.Value);
            //        double priceTo = Convert.ToDouble(rPriceTo.Value);
            //        string fromdate = rFD.SelectedDate.ToString();
            //        string todate = rTD.SelectedDate.ToString();
            //        string status1 = hdfStatus.Value;
            //        int status = ddlStatus.SelectedValue.ToInt(-1);
            //        int orderType = 1;
            //        if (Request.QueryString["ot"] != null)
            //        {
            //            orderType = Request.QueryString["ot"].ToInt(1);
            //        }

            //        if (string.IsNullOrEmpty(s) && priceFrom == 0 && priceTo == 0 && string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate) &&
            //            status1 == "-1" && chkIsnotcode.Checked == false)
            //        {
            //            int RoleID = Convert.ToInt32(ac.RoleID);
            //            int StaffID = ac.ID;

            //            //int totalRow = MainOrderController.getOrderByUID_SQL(uID);
            //            int totalRow = 0;
            //            var totalOrders = MainOrderController.GetByUserIDInSQLHelper_WithNoPaging(uID);
            //            if (totalOrders.Count > 0)
            //            {
            //                double totalPriceAll = 0;
            //                double depositAll = 0;
            //                double totalLeft = 0;
            //                totalRow = totalOrders.Count;
            //                foreach (var o in totalOrders)
            //                {
            //                    double totalprice = 0;
            //                    double deposit = 0;
            //                    if (o.TotalPriceVND.ToFloat(0) > 0)
            //                        totalprice = Convert.ToDouble(o.TotalPriceVND);
            //                    if (o.Deposit.ToFloat(0) > 0)
            //                        deposit = Convert.ToDouble(o.Deposit);

            //                    totalPriceAll += totalprice;
            //                    depositAll += deposit;
            //                }
            //                totalLeft = totalPriceAll - depositAll;
            //                lblTotalPrice.Text = string.Format("{0:N0}", totalPriceAll) + " VNĐ";
            //                lblTotaldeposit.Text = string.Format("{0:N0}", depositAll) + " VNĐ";
            //                lblTotalNotPay.Text = string.Format("{0:N0}", totalLeft) + " VNĐ";
            //            }
            //            int maximumRows = (ShouldApplySortFilterOrGroup()) ? totalRow : gr.PageSize;
            //            gr.VirtualItemCount = totalRow;
            //            int Page = (ShouldApplySortFilterOrGroup()) ? 0 : gr.CurrentPageIndex;
            //            var lo = MainOrderController.GetByUserIDInSQLHelper_WithPaging(uID, Page, maximumRows);
            //            gr.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            //            gr.DataSource = lo;
            //        }
            //        else
            //        {
            //            #region Cách mới
            //            string fd = rFD.SelectedDate.ToString();
            //            string td = rTD.SelectedDate.ToString();
            //            var la = MainOrderController.GetByUserIDInSQLHelperWithFilter(uID, tSearchName.Text.Trim(),
            //               ddlType.SelectedValue.ToString().ToInt(1), fd, td, priceFrom, priceTo, chkIsnotcode.Checked);
            //            double totalPriceAll = 0;
            //            double depositAll = 0;
            //            double totalLeft = 0;
            //            if (la.Count > 0)
            //            {
            //                if (status1 != "-1")
            //                {
            //                    var la1 = new List<OrderGetSQL>();
            //                    string[] sts = status1.Split(',');
            //                    for (int i = 0; i < sts.Length; i++)
            //                    {
            //                        int stat = sts[i].ToInt();
            //                        if (stat > -1)
            //                        {
            //                            var la2 = new List<OrderGetSQL>();
            //                            la2 = la.Where(o => o.Status == stat).ToList();
            //                            if (la2.Count > 0)
            //                            {
            //                                foreach (var item in la2)
            //                                {
            //                                    la1.Add(item);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    foreach (var o in la1)
            //                    {
            //                        double totalprice = 0;
            //                        double deposit = 0;
            //                        if (o.TotalPriceVND.ToFloat(0) > 0)
            //                            totalprice = Convert.ToDouble(o.TotalPriceVND);
            //                        if (o.Deposit.ToFloat(0) > 0)
            //                            deposit = Convert.ToDouble(o.Deposit);

            //                        totalPriceAll += totalprice;
            //                        depositAll += deposit;
            //                    }
            //                    totalLeft = totalPriceAll - depositAll;
            //                    gr.VirtualItemCount = la1.Count;
            //                    gr.DataSource = la1;
            //                }
            //                else
            //                {
            //                    foreach (var o in la)
            //                    {
            //                        double totalprice = 0;
            //                        double deposit = 0;
            //                        if (o.TotalPriceVND.ToFloat(0) > 0)
            //                            totalprice = Convert.ToDouble(o.TotalPriceVND);
            //                        if (o.Deposit.ToFloat(0) > 0)
            //                            deposit = Convert.ToDouble(o.Deposit);

            //                        totalPriceAll += totalprice;
            //                        depositAll += deposit;
            //                    }
            //                    totalLeft = totalPriceAll - depositAll;
            //                    gr.VirtualItemCount = la.Count;
            //                    gr.DataSource = la;
            //                }
            //            }
            //            else
            //            {
            //                gr.VirtualItemCount = la.Count;
            //                gr.DataSource = la;
            //            }
            //            lblTotalPrice.Text = string.Format("{0:N0}", totalPriceAll) + " VNĐ";
            //            lblTotaldeposit.Text = string.Format("{0:N0}", depositAll) + " VNĐ";
            //            lblTotalNotPay.Text = string.Format("{0:N0}", totalLeft) + " VNĐ";
            //            #endregion
            //        }
            //    }
            //}
            //else
            //{
            //    Response.Redirect("/manager/home.aspx");
            //}

        }
        #endregion

        #region button event

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string stype = ddlType.SelectedValue;
            string searchname = tSearchName.Text.Trim();
            string fd = "";
            string td = "";
            string priceFrom = "";
            string priceTo = "";
            int uID = 0;
            string hasVMD = hdfCheckBox.Value;
            if (Request.QueryString["uid"] != null)
            {
                uID = Request.QueryString["uid"].ToInt(0);
            }
            if (!string.IsNullOrEmpty(rFD.Text))
            {
                fd = rFD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rTD.Text))
            {
                td = rTD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rPriceFrom.Text))
            {
                priceFrom = rPriceFrom.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rPriceTo.Text))
            {
                priceTo = rPriceTo.Text.ToString();
            }
            string st = "";
            if (!string.IsNullOrEmpty(ddlStatuss.SelectedValue))
            {
                List<string> myValues = new List<string>();
                for (int i = 0; i < ddlStatuss.Items.Count; i++)
                {
                    var item = ddlStatuss.Items[i];
                    if (item.Selected)
                    {
                        myValues.Add(item.Value);
                    }
                }
                st = String.Join(",", myValues.ToArray());
            }
            if (string.IsNullOrEmpty(stype) == true && string.IsNullOrEmpty(searchname) == true && fd == "" && td == "" && priceFrom == "" && priceTo == "" && string.IsNullOrEmpty(st) == true && hasVMD == "0")
            {
                Response.Redirect("user-order?uid=" + uID);
            }
            else
            {
                Response.Redirect("user-order?uid=" + uID + "&stype=" + stype + "&s=" + searchname + "&fd=" + fd + "&td=" + td + "&priceFrom=" + priceFrom + "&priceTo=" + priceTo + "&st=" + st + "&hasMVD=" + hasVMD);
            }

        }
        #endregion
        public class Danhsachorder
        {
            //public tbl_MainOder morder { get; set; }
            public int ID { get; set; }
            public int STT { get; set; }
            public string ProductImage { get; set; }
            public string ShopID { get; set; }
            public string ShopName { get; set; }
            public string TotalPriceVND { get; set; }
            public string Deposit { get; set; }
            public int UID { get; set; }
            public string CreatedDate { get; set; }
            public string statusstring { get; set; }
            public string username { get; set; }
            public string dathang { get; set; }
            public string kinhdoanh { get; set; }
            public string khotq { get; set; }
            public string khovn { get; set; }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            var la = MainOrderController.GetAll();
            StringBuilder StrExport = new StringBuilder();
            StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            StrExport.Append("<DIV  style='font-size:12px;'>");
            StrExport.Append("<table border=\"1\">");
            StrExport.Append("  <tr>");
            StrExport.Append("      <th><strong>OrderID</strong></th>");
            StrExport.Append("      <th><strong>Người đặt</strong></th>");
            StrExport.Append("      <th><strong>Sản phẩm</strong></th>");
            StrExport.Append("      <th><strong>Tổng tiền</strong></th>");
            StrExport.Append("      <th><strong>Trạng thái</strong></th>");
            StrExport.Append("      <th><strong>Ngày tạo</strong></th>");
            StrExport.Append("  </tr>");
            foreach (var item in la)
            {
                string htmlproduct = "";
                string username = "";
                var ui = AccountController.GetByID(item.UID.ToString().ToInt(1));
                if (ui != null)
                {
                    username = ui.Username;
                }
                var products = OrderController.GetByMainOrderID(item.ID);
                foreach (var p in products)
                {
                    string image_src = p.image_origin;
                    if (!image_src.Contains("http:") && !image_src.Contains("https:"))
                        htmlproduct += "https:" + p.image_origin + " <br/> " + p.title_origin + "<br/><br/>";
                    else
                        htmlproduct += "" + p.image_origin + " <br/> " + p.title_origin + "<br/><br/>";
                }
                StrExport.Append("  <tr>");
                StrExport.Append("      <td>" + item.ID + "</td>");
                StrExport.Append("      <td>" + username + "</td>");
                StrExport.Append("      <td>" + htmlproduct + "</td>");
                StrExport.Append("      <td>" + string.Format("{0:N0}", Math.Floor(item.TotalPriceVND.ToFloat())) + "</td>");
                StrExport.Append("      <td>" + PJUtils.IntToRequestAdmin(Convert.ToInt32(item.Status)) + "</td>");
                StrExport.Append("      <td>" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</td>");
                StrExport.Append("  </tr>");
            }
            StrExport.Append("</table>");
            StrExport.Append("</div></body></html>");
            string strFile = "ExcelReportOrderList.xls";
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