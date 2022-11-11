using MB.Extensions;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZLADIPJ.Business;
using static NHST.Controllers.MainOrderController;
using static NHST.Controllers.StaffIncomeController;

namespace NHST.manager
{
    public partial class Report_User_Transaction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/manager/Login.aspx");
                }
                else
                {
                    string Username = Session["userLoginSystem"].ToString();
                    var obj_user = AccountController.GetByUsername(Username);
                    if (obj_user != null)
                    {
                        if (obj_user.RoleID != 0 && obj_user.RoleID != 2)
                        {
                            Response.Redirect("/trang-chu");
                        }
                    }

                }
                LoadData();
            }
        }
        public void LoadData()
        {
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }
            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            var la = AccountController.GetListUserBySQL(search, page, 10);
            int total = AccountController.GetTotalUser(search);
            pagingall(la, total);
        }
        #region Pagging
        public void pagingall(List<View_UserList> acs, int total)
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            int PageSize = 20;
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
                for (int i = 0; i < acs.Count; i++)
                {

                    double MoneyinWallet = 0;
                    double MoneyRevert = 0;
                    double MoneySpend = 0;
                    double Withdrawal = 0;
                    double Money = 0;
                    var item = acs[i];

                    var lHistoryPay = HistoryPayWalletController.GetByUID(item.ID);
                    var lWithdraw = WithdrawController.GetBuyUID(item.ID).Where(x => x.Status == 2).ToList();
                    
                    if(lHistoryPay.Where(x => x.TradeType == 4).ToList().Count() > 0)
                    {
                        MoneyinWallet += lHistoryPay.Where(x => x.TradeType == 4).Sum(x => x.Amount.Value);
                    }
                    if (lHistoryPay.Where(x => x.TradeType == 2).ToList().Count() > 0)
                    {
                        MoneyRevert += lHistoryPay.Where(x => x.TradeType == 2).Sum(x => x.Amount.Value);
                    }
                    if (lHistoryPay.Where(x => x.TradeType == 1 || x.TradeType == 3).ToList().Count() > 0)
                    {
                        MoneySpend += lHistoryPay.Where(x => x.TradeType == 1 || x.TradeType == 3).Sum(x => x.Amount.Value);
                    }
                    if(lWithdraw.Count() > 0)
                    {
                        Withdrawal += lWithdraw.Sum(x => x.Amount.Value);
                    }

                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.Username + "</td>");
                    hcm.Append("<td>" + item.FirstName + " " + item.LastName + "</td>");
                    hcm.Append("<td>" + item.MobilePhonePrefix + item.MobilePhone + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(MoneyinWallet)) + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(MoneyRevert)) + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(MoneySpend)) + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(Withdrawal)) + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.Wallet)) + "</td>");
                    hcm.Append("</tr>");

                }
                ltr.Text = hcm.ToString();

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

        public class OrderReport
        {
            public int OrderCode { get; set; }
            public string Username { get; set; }
            public string TotalPrice { get; set; }
            public string TotalBuyProReal { get; set; }
            public string TotalIncome { get; set; }
            public string CreatedDate { get; set; }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            var tt = AccountController.GetAll_ViewUserListExcel("").Where(u => u.RoleID == 1).ToList();
            StringBuilder StrExport = new StringBuilder();
            StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            StrExport.Append("<DIV  style='font-size:12px;'>");
            StrExport.Append("<table border=\"1\">");
            StrExport.Append("  <tr>");
            StrExport.Append("      <th style=\"mso-number-format:'\\@'\" ><strong>ID</strong></th>");
            StrExport.Append("      <th><strong>UserName</strong></th>");
            StrExport.Append("      <th><strong>Họ Tên</strong></th>");
            StrExport.Append("      <th><strong>Số điện thoại</strong></th>");
            StrExport.Append("      <th><strong>Tiền nạp</strong></th>");
            StrExport.Append("      <th><strong>Tiền hoàn trả</strong></th>");
            StrExport.Append("      <th><strong>Tiền tiêu</strong></th>");
            StrExport.Append("      <th><strong>Tiền rút</strong></th>");
            StrExport.Append("      <th><strong>Tiền hiện còn</strong></th>");
            StrExport.Append("  </tr>");
            foreach (var item in tt)
            {

                double MoneyinWallet = 0;
                double MoneyRevert = 0;
                double MoneySpend = 0;
                double Withdrawal = 0;
                double Money = 0;

                var lHistoryPay = HistoryPayWalletController.GetByUID(item.ID);
                var lWithdraw = WithdrawController.GetBuyUID(item.ID).Where(x => x.Status == 2).ToList();

                if (lHistoryPay.Where(x => x.TradeType == 4).ToList().Count() > 0)
                {
                    MoneyinWallet += lHistoryPay.Where(x => x.TradeType == 4).Sum(x => x.Amount.Value);
                }
                if (lHistoryPay.Where(x => x.TradeType == 2).ToList().Count() > 0)
                {
                    MoneyRevert += lHistoryPay.Where(x => x.TradeType == 2).Sum(x => x.Amount.Value);
                }
                if (lHistoryPay.Where(x => x.TradeType == 1 || x.TradeType == 3).ToList().Count() > 0)
                {
                    MoneySpend += lHistoryPay.Where(x => x.TradeType == 1 || x.TradeType == 3).Sum(x => x.Amount.Value);
                }
                if (lWithdraw.Count() > 0)
                {
                    Withdrawal += lWithdraw.Sum(x => x.Amount.Value);
                }

                StrExport.Append("<tr>");
                StrExport.Append("<td>" + item.ID + "</td>");
                StrExport.Append("<td>" + item.Username + "</td>");
                StrExport.Append("<td>" + item.FirstName + " " + item.LastName + "</td>");
                StrExport.Append("<td>" + item.MobilePhonePrefix + item.MobilePhone + "</td>");
                StrExport.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(MoneyinWallet)) + "</td>");
                StrExport.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(MoneyRevert)) + "</td>");
                StrExport.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(MoneySpend)) + "</td>");
                StrExport.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(Withdrawal)) + "</td>");
                StrExport.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.Wallet)) + "</td>");
                StrExport.Append("</tr>");

            }
            StrExport.Append("</table>");
            StrExport.Append("</div></body></html>");
            string strFile = "Thong-ke-khach-hang.xls";
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
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchname = search_name.Text.Trim();
            if (!string.IsNullOrEmpty(searchname))
            {
                Response.Redirect("Report-User-Transaction?s=" + searchname);
            }
            else
            {
                Response.Redirect("Report-User-Transaction");
            }


        }
    }
}