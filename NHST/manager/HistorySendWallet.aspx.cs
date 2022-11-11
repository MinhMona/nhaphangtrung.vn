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
    public partial class HistorySendWallet1 : System.Web.UI.Page
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
                    if (ac.RoleID == 0 || ac.RoleID == 2 || ac.RoleID == 7 || ac.RoleID == 9)
                        LoadData();
                    else
                        Response.Redirect("/trang-chu");                   
                }
            }
        }
        public void LoadData()
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];
            if (!string.IsNullOrEmpty(fd))
                rFD.Text = fd;
            if (!string.IsNullOrEmpty(td))
                rTD.Text = td;
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }
            string st = Request.QueryString["st"];
            if (!string.IsNullOrEmpty(st))
                select_by.SelectedIndex = st.ToInt(0);


            string banker = Request.QueryString["banker"];
            if (!string.IsNullOrEmpty(banker))
                lbBank.SelectedIndex = banker.ToInt(0);

            string ip = "-1";
            if (Request.QueryString["ip"] != null)
            {
                ip = Request.QueryString["ip"];
                ddlIsPayLoan.SelectedValue = ip;
            }
            else
            {
                ddlIsPayLoan.SelectedValue = ip;
            }

            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }

            if (ac.RoleID == 0)
            {
                bank.Visible = true;
            }
            else
            {
                bank.Visible = false;
            }
            if (ac.RoleID == 9)
            {
                var la = AdminSendUserWalletController.GetAllBYCSKH(search, st, page, fd, td, 10, ip, banker, ac.ID);
                int total = AdminSendUserWalletController.GetTotalListCSKH(search, st, fd, td, ip, banker, ac.ID);
                pagingall(la, total);
            }
            else
            {
                var la = AdminSendUserWalletController.GetBySQL_DK_bank(search, st, page, fd, td, 10, ip, banker);
                int total = AdminSendUserWalletController.GetTotalList_bank(search, st, fd, td, ip, banker);
                pagingall(la, total);
                pnadminmanager.Visible = true;
            }
            
            var las = AdminSendUserWalletController.GetBySQL_DK_NOpage_bank(search, st, fd, td, ip, banker).Where(x => x.Status != 3).ToList();
            double moneys = 0, money = 0, moneyappro = 0;
            foreach (var item in las)
            {
                if (item.Status == 1)
                {
                    money += Convert.ToDouble(item.Amount);
                }
                else if (item.Status == 2)
                {
                    moneyappro += Convert.ToDouble(item.Amount);
                }
                moneys += Convert.ToDouble(item.Amount);
            }
            if (ac.RoleID != 9)
            {
                ltrMoney.Text = string.Format("{0:N0}", money);
                ltrMoneyAppro.Text = string.Format("{0:N0}", moneyappro);
                ltrMoneys.Text = string.Format("{0:N0}", moneys);
            }           
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchname = search_name.Text.Trim();
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
            string st = select_by.SelectedValue;
            string banker = lbBank.SelectedValue;
            string ip = ddlIsPayLoan.SelectedValue;
            if (string.IsNullOrEmpty(searchname) == true && fd == "" && td == "" && st == "0" && banker == "0" && ip == "-1")
            {
                Response.Redirect("HistorySendWallet");
            }
            else
            {
                Response.Redirect("HistorySendWallet?s=" + searchname + "&fd=" + fd + "&td=" + td + "&st=" + st + "&banker=" + banker + "&ip=" + ip + "");
            }

        }
        public partial class WalletListShow
        {
            public List<string> ListIMG { get; set; }
            public string Username { get; set; }
            public string TradeContent { get; set; }
            public double Amount { get; set; }
            public int Status { get; set; }
            public bool ddlBank { get; set; }
            public bool IsLoan { get; set; }
            public bool IsPayLoan { get; set; }
            public int BankId { get; set; }


        }
        public partial class bankTarget
        {
            public int ID { get; set; }
            public string BankInfo { get; set; }

        }
        #region Webservice
        [WebMethod]
        public static string loadinfo(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var f = AdminSendUserWalletController.GetByID(ID.ToInt(0));
            var h = HistoryPayWalletController.GetByAdminSendUserWalletID(ID.ToInt(0));
            if (f != null)
            {
                WalletListShow l = new WalletListShow();
                l.ddlBank = true;
                if (f.Status == 2)
                {
                    var time = DateTime.Now - h.CreatedDate.Value;
                    if (time.Days < 11)
                    {
                        l.ddlBank = false;
                    }
                }
                l.Username = f.Username;
                l.TradeContent = f.TradeContent;
                l.Amount = Convert.ToDouble(f.Amount);
                l.Status = Convert.ToInt32(f.Status);
                l.IsLoan = Convert.ToBoolean(f.IsLoan);
                l.IsPayLoan = Convert.ToBoolean(f.IsPayLoan);
                l.BankId = f.BankID.Value;
                if (!string.IsNullOrEmpty(f.IMG))
                {
                    var b = f.IMG.Split('|').ToList();
                    l.ListIMG = b;
                }
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }
        [WebMethod]
        public static string GetData(int ID)
        {
            var nap = AdminSendUserWalletController.GetByID(ID);
            if (nap != null)
            {
                NaptienInfo n = new NaptienInfo();
                int UID = Convert.ToInt32(nap.UID);
                double Amount = Convert.ToDouble(nap.Amount);
                var ai = AccountInfoController.GetByUserID(UID);
                if (ai != null)
                {
                    n.FullName = ai.FirstName + " " + ai.LastName;
                    n.Address = ai.Address;
                }
                n.Money = string.Format("{0:N0}", Amount);
                if (!string.IsNullOrEmpty(nap.TradeContent))
                    n.Note = nap.TradeContent;
                DateTime currentDate = DateTime.Now;
                string CreateDate = "Ngày " + currentDate.Day + " tháng " + currentDate.Month + " năm " + currentDate.Year;
                n.CreateDate = CreateDate;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(n);
            }
            return "null";
        }
        public class NaptienInfo
        {
            public string FullName { get; set; }
            public string Address { get; set; }
            public string Money { get; set; }
            public string Note { get; set; }
            public string CreateDate { get; set; }
        }
        #endregion
        #region Pagging
        public void pagingall(List<AdminSendUserWalletController.ListShowNew> acs, int total)
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            double ToTalMoney = 0;
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
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];
                    if (item.IsLoan == true)
                        ToTalMoney += item.Amount;                  
                    string Bank = PJUtils.ReturnBank(item.BankID);
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.UserName + "</td>");
                    hcm.Append("<td>" + item.AmountString + "</td>");
                    hcm.Append("<td>" + Bank + "</td>");
                    hcm.Append("<td>" + item.note + "</td>");
                    hcm.Append("<td>" + item.StatusName + "</td>");
                    hcm.Append("<td>" + item.CreatedDateString + "</td>");
                    hcm.Append("<td>" + item.ModifiedBy + "</td>");
                    hcm.Append("<td>" + item.ModifiedDate + "</td>");
                    hcm.Append("<td><textarea class=\"txtNote\">" + item.NoteAdmin + "</textarea>");
                    if (ac.RoleID == 0)
                    {
                        hcm.Append("<a href =\"javascript:;\" class=\"btn btn-info\" onclick=\"updateNote($(this),'" + item.ID + "')\">Cập nhật</a>");
                        hcm.Append("<span class=\"update-info\" style=\"width:100%; clear:both; float:left; color blue; display:none\">Cập nhật thành công</span>");
                    }
                    hcm.Append("</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    if (ac.RoleID != 9)
                    {
                        hcm.Append("<a href=\"javascript:;\" onclick=\"EditFunction(" + item.ID + ")\" class=\"edit-mode\" data-position=\"top\">");
                        hcm.Append("<i class=\"material-icons\">edit</i><span>Cập nhật</span></a>");
                        hcm.Append("<a href=\"javascript:;\" onclick=\"printPhieuthu(" + item.ID + ")\" data-position=\"top\">");
                        hcm.Append("<i class=\"material-icons\">print</i><span>In phiếu thu</span></a>");
                    } 
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    hcm.Append("</tr>");
                }
                ltr.Text = hcm.ToString();
                ltrTotalAmount.Text = string.Format("{0:N0}", ToTalMoney) + " VNĐ";
                List<bankTarget> lB = new List<bankTarget>();
                var lBank = BankController.GetAll();
                if (lBank != null && lBank.Count() > 0)
                {
                    foreach (var item in lBank)
                    {
                        bankTarget b = new bankTarget();
                        b.ID = item.ID;
                        b.BankInfo = PJUtils.ReturnBank(item.ID);
                        lB.Add(b);
                    }
                    bankTarget b1 = new bankTarget();
                    b1.ID = 100;
                    b1.BankInfo = PJUtils.ReturnBank(100);
                    lB.Add(b1);
                }
                ddlBank.DataSource = lB;
                ddlBank.DataBind();
            }
        }


        #region Webservice
        [WebMethod]
        public static string UpdateStaffNote(int ID, string staffNote)
        {
            var ex = AdminSendUserWalletController.GetByID(ID);
            if (ex != null)
            {
                AdminSendUserWalletController.UpdateStaffNote(ID, staffNote);
                return "ok";
            }
            return "none";
        }
        #endregion

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

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username_current = Session["userLoginSystem"].ToString();
            int role = 0;
            var u_loginin = AccountController.GetByUsername(username_current);
            if (u_loginin != null)
            {
                role = u_loginin.RoleID.ToString().ToInt(0);
            }
            int id = hdfIDHSW.Value.ToInt(0);
            var h = AdminSendUserWalletController.GetByID(id);
            var hpw = HistoryPayWalletController.GetByAdminSendUserWalletID(id);
            int UID = h.UID.Value;
            var user_wallet = AccountController.GetByID(UID);

            double money = h.Amount.Value; //Convert.ToDouble(pWallet.Text);          
            int status = ddlStatus.SelectedValue.ToString().ToInt(1);
            bool IsLoan = Convert.ToBoolean(hdfIsLoan.Value.ToInt(0));
            bool IsPayLoan = Convert.ToBoolean(hdfIsPayLoan.Value.ToInt(0));
            DateTime currentdate = DateTime.Now;
            string content = pContent.Text;
            int bankID = Convert.ToInt32(ddlBank.SelectedValue);

            string BackLink = "/manager/HistorySendWallet.aspx";
            if (h != null)
            {
                if (h.Status == 2 && role == 0)
                {
                    var time = currentdate - hpw.CreatedDate.Value;
                    if (time.Days < 11)
                    {
                        AdminSendUserWalletController.UpdateBank(id, bankID, currentdate, username_current);
                        PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công.", "s", true, Page);
                    }
                }
                else if (h.Status != 1)
                {
                    AdminSendUserWalletController.UpdateCongNo(id, IsLoan, IsPayLoan);
                    PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công.", "s", true, Page);
                }
                else
                {
                    if (money > 0)
                    {
                        if (user_wallet != null)
                        {
                            double wallet = Convert.ToDouble(user_wallet.Wallet);
                            wallet = wallet + money;
                            if (role == 0 || role == 2 || role == 7)
                            {
                                if (status == 2)
                                {
                                    AdminSendUserWalletController.UpdateStatus(id, status, content, currentdate, username_current);
                                    AdminSendUserWalletController.UpdateBank(id, bankID, currentdate, username_current);
                                    AdminSendUserWalletController.UpdateCongNo(id, IsLoan, IsPayLoan);
                                    AccountController.updateWallet(user_wallet.ID, wallet, currentdate, username_current);
                                    if (string.IsNullOrEmpty(content))
                                        HistoryPayWalletController.Insert_AdminSendUserWalletID(user_wallet.ID, user_wallet.Username, 0, money, user_wallet.Username + " đã được nạp tiền vào tài khoản.", wallet, 2, 4, currentdate, username_current, id);
                                    else
                                        HistoryPayWalletController.Insert_AdminSendUserWalletID(user_wallet.ID, user_wallet.Username, 0, money, content, wallet, 2, 4, currentdate, username_current, id);

                                    NotificationController.Inser(u_loginin.ID, u_loginin.Username,
                                        Convert.ToInt32(user_wallet.ID), user_wallet.Username, 0,
                                        user_wallet.Username + " đã được nạp tiền vào tài khoản.", 0, 2,
                                        DateTime.Now, u_loginin.Username, true);


                                    var setNoti = SendNotiEmailController.GetByID(3);
                                    if (setNoti != null)
                                    {
                                        if (setNoti.IsSendEmailUser == true)
                                        {
                                            try
                                            {
                                                StringBuilder html = new StringBuilder();
                                                html.Append("<!DOCTYPE html>");
                                                html.Append("<html lang=\"en\">");
                                                html.Append("<head>");
                                                html.Append("   <meta charset=\"UTF-8\">");
                                                html.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                                                html.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\">");
                                                html.Append("<title>Document</title>");
                                                html.Append("</head>");
                                                html.Append("<body style=\"margin: 0; padding:0\">");
                                                html.Append("<table style=\"font-family: sans-serif; font-size: 14px; border-collapse: collapse; width: 500px; max-width: 100%; margin: auto\">");
                                                html.Append("<tr>");
                                                html.Append("<td style=\"padding: 10px; background-color: #fca777; color: #fff; text-align: center\"><strong><p>KÍNH CHÀO QUÝ KHÁCH!</p><p><a style=\"text-decoration: none\" href=\"https://NHAPSICHINA.COM/\" target=\"_blank\"><strong>NHAPSICHINA.COM<strong></a> THÔNG BÁO NẠP VÍ THÀNH CÔNG</p></strong></td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>THÔNG TIN GIAO DỊCH</td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>Tên KH: " + AccountInfoController.GetByUserID(user_wallet.ID).FirstName + " " + AccountInfoController.GetByUserID(user_wallet.ID).LastName + " </td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>SĐT:  " + AccountInfoController.GetByUserID(user_wallet.ID).Phone + "</td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>Username: " + user_wallet.Username + "</td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>Số tiền nạp ví: " + string.Format("{0:N0}", (Convert.ToDouble(money))) + " VNĐ</td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>Số dư hiện tại: " + string.Format("{0:N0}", wallet) + " VNĐ</td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>ID nạp ví: " + id + "</td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>Thời gian: " + currentdate + "</td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>Quý khách vui lòng truy cập tài khoản để kiểm tra chi tiết.</td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td><a style=\"text-decoration: none\" href=\"https://NHAPSICHINA.COM/\" target=\"_blank\"><strong>NHAPSICHINA.COM<strong></a> chân thành cảm ơn!</td>");
                                                html.Append("</tr>");
                                                html.Append("<tr>");
                                                html.Append("<td>==============================</td>");
                                                //html.Append("<tr>");
                                                //html.Append("<td>Mọi thắc mắc xin vui lòng liên hệ: <a href=\"tel:0785601688\">0785601688</a></td>");
                                                //html.Append("</tr>");
                                                html.Append("</tr>");
                                                html.Append("</table>");
                                                html.Append("</body>");
                                                html.Append("</html>");

                                                //"NHAPSICHINA.COM THÔNG BÁO NẠP VÍ THÀNH CÔNG",
                                                //                        "Kính Chào Quý Khách! <br>" +
                                                //                        "THÔNG TIN GIAO DỊCH <br>" +
                                                //                        "Tên KH: " + AccountInfoController.GetByUserID(u.ID).LastName + "<br>" +
                                                //                        "SĐT: " + AccountInfoController.GetByUserID(u.ID).Phone + "<br>" +
                                                //                        "Username: " + u.Username + "<br>" +
                                                //                        "Số tiền nạp ví: " + string.Format("{0:N0}", (Convert.ToDouble(pAmount.Value))).Replace(",", ".") + " VNĐ" + "<br>" +
                                                //                        //"Số dư hiện tại: " + string.Format("{0:N0}", u.Wallet).Replace(",", ".") + " VNĐ"  +"<br>" +
                                                //                        "ID nạp ví: " + u.ID + "<br>" +
                                                //                        "Thời gian: " + AdminSendUserWalletController.GetByUID_New(u.ID).CreatedDate + "<br>" +
                                                //                        "Quý khách vui lòng truy cập tài khoản để kiểm tra chi tiết.<br>" +
                                                //                        "BEE - SHIP.com chân thành cảm ơn! <br>" +
                                                //                        "Mọi thắc mắc xin vui lòng liên hệ: 09879 04 078", "");


                                                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", user_wallet.Email,
                                                                        "NHAPSICHINA.COM THÔNG BÁO NẠP VÍ THÀNH CÔNG",
                                                                        html.ToString(), "");

                                            }
                                            catch { }
                                        }
                                    }

                                }
                                else
                                {
                                    AdminSendUserWalletController.UpdateStatus(id, status, content, currentdate, username_current);
                                    AdminSendUserWalletController.UpdateBank(id, bankID, currentdate, username_current);
                                    AdminSendUserWalletController.UpdateCongNo(id, IsLoan, IsPayLoan);
                                }
                            }
                            //else
                            //{
                            //    AdminSendUserWalletController.Insert(user_wallet.ID, user_wallet.Username, money, 1, currentdate, username_current);
                            //}
                            PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công.", "s", true, BackLink, Page);
                            //Response.Redirect("/Admin/HistorySendWallet.aspx");
                        }
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Vui lòng nhập số tiền lớn hơn 0.", "e", true, Page);
                    }
                }
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
            }
            string st = Request.QueryString["st"];

            string banker = Request.QueryString["banker"];

            string ip = "-1";
            if (Request.QueryString["ip"] != null)
            {
                ip = Request.QueryString["ip"];
            }


            var la = AdminSendUserWalletController.GetBySQL_DK_NOpage_bank(search, st, fd, td, ip, banker);
            if (la.Count > 0)
            {
                StringBuilder StrExport = new StringBuilder();
                StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
                StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
                StrExport.Append("<DIV  style='font-size:12px;'>");
                StrExport.Append("<table border=\"1\">");
                StrExport.Append("  <tr>");
                StrExport.Append("      <th><strong>ID</strong></th>");
                StrExport.Append("      <th><strong>Username</strong></th>");
                StrExport.Append("      <th><strong>Số tiền nạp</strong></th>");
                StrExport.Append("      <th><strong>Ngân hàng</strong></th>");
                StrExport.Append("      <th><strong>Trạng thái</strong></th>");
                StrExport.Append("      <th><strong>Ngày nạp</strong></th>");
                StrExport.Append("  </tr>");
                foreach (var item in la)
                {
                    StrExport.Append("  <tr>");
                    StrExport.Append("      <td>" + item.ID + "</td>");
                    StrExport.Append("      <td>" + item.UserName + "</td>");
                    StrExport.Append("      <td>" + item.AmountString + "</td>");
                    StrExport.Append("      <td>" + PJUtils.ReturnBank(item.BankID) + "</td>");
                    StrExport.Append("      <td>" + item.StatusName + "</td>");
                    StrExport.Append("      <td>" + item.CreatedDateString + "</td>");
                    StrExport.Append("  </tr>");
                }
                StrExport.Append("</table>");
                StrExport.Append("</div></body></html>");
                string strFile = "Lich_su_nap_tien.xls";
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