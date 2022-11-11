using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
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

namespace NHST.manager
{
    public partial class SMSForward : System.Web.UI.Page
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

                    if (ac.RoleID == 0 || ac.RoleID == 2 || ac.RoleID == 7)
                        LoadData();
                    else
                        Response.Redirect("/");
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchname = txtSearchName.Text.Trim();
            string status = ddlStatus.SelectedValue;
            string fd = "";
            string td = "";
            if (!string.IsNullOrEmpty(rFD.Text))
            {
                fd = rFD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rTD.Text))
            {
                td = rTD.Text.ToString();
            }           
            if (string.IsNullOrEmpty(searchname) == true && fd == "" && td == "" && status == "0")
            {
                Response.Redirect("SMSForward");
            }
            else
            {
                Response.Redirect("SMSForward?s=" + searchname + "&stt=" + status + "&fd=" + fd + "&td=" + td);
            }
        }
        private void LoadData()
        {
            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }

            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                txtSearchName.Text = search;
            }          
            
            int status = Request.QueryString["stt"].ToInt(0);
            ddlStatus.SelectedValue = status.ToString();

            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;

            var total = SmsForwardController.GetTotal(search, fd, td, status);
            var la = SmsForwardController.GetAllBySQL(search,fd, td, status, page, 20);
            pagingall(la, total);
        }

        public void pagingall(List<tbl_SmsForward> acs, int total)
        {
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
                    var item = acs[i];
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.ten_bank + "</td>");
                    hcm.Append("<td>" + item.Username + "</td>");
                    hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.so_tien)) + "</td>");
                    hcm.Append("<td>" + item.thoi_gian + "</td>");
                    hcm.Append("<td>" + PJUtils.ReturnStatusTruyVan(Convert.ToInt32(item.Status)) + "</td>");
                    hcm.Append("<td>" + item.noi_dung + "</td>");
                    hcm.Append("<td>" + item.CreatedDate + "</td>");
                    hcm.Append("<td>");
                    if (item.Status == 1)
                    {
                        hcm.Append("<div class=\"action-table\">");
                        hcm.Append("<a href=\"javascript:;\" onclick=\"addCodeTemp(" + item.ID + ")\" class=\"edit-mode\" data-position=\"top\">");
                        hcm.Append("<i class=\"material-icons\">edit</i><span>NẠP TIỀN</span></a>");

                        hcm.Append("<a href=\"javascript:;\" onclick=\"CancelNaptien(" + item.ID + ")\" data-position=\"top\">");
                        hcm.Append("<i class=\"material-icons\">delete</i><span>HỦY YÊU CẦU</span></a>");
                        hcm.Append("</div>");
                    }
                    hcm.Append("</td>");
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

        #region Webservice
        [WebMethod]
        public static string GetData(int ID)
        {
            var nap = SmsForwardController.GetByID(ID);
            if (nap != null)
            {
                NaptienInfo n = new NaptienInfo();
                n.ID = nap.ID;
                double Amount = Convert.ToDouble(nap.so_tien);
                n.ten_bank = nap.ten_bank;
                n.so_tien = string.Format("{0:N0}", Amount);
                if (!string.IsNullOrEmpty(nap.noi_dung))
                    n.noi_dung = nap.noi_dung;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(n);
            }
            return "null";
        }

        public class NaptienInfo
        {
            public string noi_dung { get; set; }
            public string so_tien { get; set; }
            public string ten_bank { get; set; }
            public int ID { get; set; }
        }
        #endregion

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();
            var acc = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            int ID = hdfIDWR.Value.ToInt(0);
            int RoleID = Convert.ToInt32(acc.RoleID);
            if (RoleID == 0)
            {
                string UsernameKhach = pUsername.Text.Trim();
                var u = AccountController.GetByUsername(UsernameKhach);
                if (u != null)
                {
                    var sms = SmsForwardController.GetByID(ID);
                    if (sms != null)
                    {
                        int BankID = 0;
                        string nganhang = sms.ten_bank; 
                        if (!string.IsNullOrEmpty(nganhang))
                        {
                            var temp = nganhang.Split(' ');
                            string key = temp[0].ToLower();
                            if (key.ToLower() == "tpbank")
                            {
                                BankID = 5;
                            }
                            else if (key.ToLower() == "techcombank")
                            {
                                BankID = 6;
                            }
                        }
                        double money = Convert.ToDouble(sms.so_tien);
                        double wallet = Math.Round(Convert.ToDouble(u.Wallet), 0);
                        wallet = Math.Round(wallet + money, 0);

                        SmsForwardController.updateStatus(sms.ID, 2, 1);
                        AdminSendUserWalletController.InsertNew(u.ID, u.Username, money, 2, sms.noi_dung, currentDate, username, sms.ID, BankID);
                        AccountController.updateWallet(u.ID, wallet, currentDate, username);
                        HistoryPayWalletController.Insert(u.ID, u.Username, 0, money, u.Username + " đã được nạp tiền vào tài khoản.", wallet, 2, 4, currentDate, username);
                        NotificationController.Inser(u.ID, u.Username, Convert.ToInt32(u.ID), u.Username, 0, u.Username + " đã được nạp tiền vào tài khoản.", 0, 2, currentDate, username, false);

                        PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công.", "s", true, Page);
                    }
                }
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Bạn không có quyền duyệt yêu cầu này", "i", true, Page);
            }
        }  

        [WebMethod]
        public static string Cancel1(int ID)
        {
            DateTime currentdate = DateTime.Now;
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {               
                string username_check = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user_check = AccountController.GetByUsername(username_check);
                if (user_check != null)
                {
                    int userRole_check = Convert.ToInt32(user_check.RoleID);
                    if (userRole_check == 0)
                    {
                        var e = SmsForwardController.GetByID(ID);
                        if (e != null)
                        {
                            SmsForwardController.updateStatus(e.ID, 3, 0);

                            return "success";
                        }
                        else
                            return "notrightSMS";
                    }
                    else
                        return "none";
                }
                else
                {
                    return "none";
                }
            }
            else
            {
                return "none";
            }
        }
    }
}