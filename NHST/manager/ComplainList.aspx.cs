using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using static NHST.Controllers.ComplainController;

namespace NHST.manager
{
    public partial class ComplainList1 : System.Web.UI.Page
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
                    if (ac.RoleID != 0 && ac.RoleID != 2 && ac.RoleID != 9)
                        Response.Redirect("/trang-chu");
                    LoadData();
                    loadFilter();
                }
            }
        }
        public void loadFilter()
        {          
            var dathangs = AccountController.GetAllByRoleID(3);         
            searchNVDH.Items.Clear();
            searchNVDH.Items.Insert(0, new ListItem("Chọn nhân viên đặt hàng", "0"));
            if (dathangs.Count > 0)
            {
                searchNVDH.DataSource = dathangs;
                searchNVDH.DataBind();
            }
            var cskhs = AccountController.GetAllByRoleID(9);
            searchCSKH.Items.Clear();
            searchCSKH.Items.Insert(0, new ListItem("Chọn nhân viên cskh", "0"));
            if (cskhs.Count > 0)
            {
                searchCSKH.DataSource = cskhs;
                searchCSKH.DataBind();
            }          
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchname = search_name.Text.Trim();
            string status = ddlStatus.SelectedValue;
            string fd = "";
            string td = "";
            int nvdh = 0;
            if (!string.IsNullOrEmpty(searchNVDH.SelectedValue))
            {
                nvdh = searchNVDH.SelectedValue.ToInt(0);
            }            
            int nvcs = 0;
            if (!string.IsNullOrEmpty(searchCSKH.SelectedValue))
            {
                nvcs = searchCSKH.SelectedValue.ToInt(0);
            }
            if (!string.IsNullOrEmpty(rdatefrom.Text))
            {
                fd = rdatefrom.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rdateto.Text))
            {
                td = rdateto.Text.ToString();
            }

            if (!string.IsNullOrEmpty(searchname))
            {
                Response.Redirect("ComplainList?s=" + searchname + "&stt=" + status + "&fd=" + fd + "&td=" + td + "&nvdh=" + nvdh + "&nvcs=" + nvcs + "");
            }
            else
            {
                Response.Redirect("ComplainList?stt=" + status + "&fd=" + fd + "&td=" + td + "&nvdh=" + nvdh + "&nvcs=" + nvcs + "");
            }
        }
        private void LoadData()
        {
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }

            int status = Request.QueryString["stt"].ToInt(-1);
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];

            ddlStatus.SelectedValue = status.ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["fd"]))            
                rdatefrom.Text = fd;           
            if (!string.IsNullOrEmpty(Request.QueryString["td"]))
                rdateto.Text = td;           

            int nvdh = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["nvdh"]))
            {
                nvdh = Convert.ToInt32(Request.QueryString["nvdh"]);
                searchNVDH.SelectedValue = nvdh.ToString();
            }

            int nvcs = 0; 
            if (!string.IsNullOrEmpty(Request.QueryString["nvcs"]))
            {
                nvcs = Convert.ToInt32(Request.QueryString["nvcs"]);
                searchCSKH.SelectedValue = nvcs.ToString();
            }                   

            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            var total = ComplainController.GetTotal(search, fd, td, status, nvdh, nvcs);
            var la = ComplainController.GetAllBySQL(search, page, 20, fd, td, status, nvdh, nvcs);
            pagingall(la, total);
        }

        [WebMethod]
        public static string loadinfoComplain(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var com = ComplainController.GetByID(ID.ToInt(0));
            if (com != null)
            {
                ComplainListShow l = new ComplainListShow();
                if (com != null)
                {
                    var ordershop = MainOrderController.GetAllByID(Convert.ToInt32(com.OrderID));
                    if (ordershop != null)
                    {
                        l.TiGia = ordershop.CurrentCNYVN;
                        l.AmountCNY = string.Format("{0:N0}", Convert.ToDouble(com.Amount) / Convert.ToDouble(ordershop.CurrentCNYVN));
                        l.UserName = com.CreatedBy;
                        l.ShopID = com.OrderID.ToString();
                        l.AmountVND = com.Amount;
                        l.ComplainText = com.ComplainText;
                        l.Status = com.Status.ToString();
                        if (!string.IsNullOrEmpty(com.IMG))
                        {
                            var b = com.IMG.Split('|').ToList();
                            l.ListIMG = b;
                        }
                    }
                }
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        public partial class ComplainListShow
        {
            public string TiGia { get; set; }
            public string UserName { get; set; }
            public string AmountCNY { get; set; }
            public string AmountVND { get; set; }
            public string ComplainText { get; set; }
            public string Status { get; set; }
            public string UrlIMG { get; set; }
            public List<string> ListIMG { get; set; }
            public string ShopID { get; set; }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                if (ac.ID == 1)
                {
                    int ID = hdfID.Value.ToInt();
                    if (ID > 0)
                    {
                        var com = ComplainController.GetByID(ID);
                        if (com != null)
                        {
                            var kq = ComplainController.Delete(ID);
                            if (kq != null)
                            {
                                PJUtils.ShowMessageBoxSwAlert("Xóa khiếu nại thành công.", "s", true, Page);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý. Vui lòng thử lại.", "e", true, Page);
                            }
                        }
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Bạn không đủ quyền thực hiện chức năng này.", "e", true, Page);
                }
            }
        }

        #region Pagging
        public void pagingall(List<ListComplain> acs, int total)
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
                    var item = acs[i];    

                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.Username + "</td>");
                    hcm.Append("<td>" + item.MainOrderID + "</td>");
                    hcm.Append("<td>" + item.Quantity + "</td>");
                    hcm.Append("<td>" + item.DathangName + "</td>");
                    hcm.Append("<td>" + item.CskhName + "</td>");
                    hcm.Append("<td>" + item.TypeString + "</td>");
                    hcm.Append("<td>" + item.Amount + "</td>");
                    hcm.Append("<td>" + item.ComplainText + "</td>");
                    hcm.Append("<td>" + item.StatusString + "</td>");
                    hcm.Append("<td>" + item.CreatedDate.ToString().Remove(item.CreatedDate.ToString().Length - 6, 6) + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"javascript:;\" onclick=\"Complain(" + item.ID + ")\" class=\"edit-mode\" data-position=\"top\"> ");
                    hcm.Append(" <i class=\"material-icons\">remove_red_eye</i><span>Xem</span></a>");
                    hcm.Append("<a href =\"OrderDetail.aspx?id=" + item.MainOrderID + "\" target=\"_blank\" data-position=\"top\" ><i class=\"material-icons\">edit</i><span>Chi tiết đơn</span></a>");
                    if (ac.ID == 1)
                    {
                        if (item.Status != 3)
                        {
                            hcm.Append("<a href=\"javascript:;\" onclick=\"CancelOrder('" + item.ID + "',$(this))\" data-position=\"top\" ><i class=\"material-icons\">delete</i><span>Xóa</span></a>");
                        }
                    }
                    hcm.Append("</div>");
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
        #endregion

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int ID = hdfID.Value.ToInt(0);
            string username_current = Session["userLoginSystem"].ToString();
            var ac = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (ac != null)
            {
                if (ac.RoleID == 0 || ac.RoleID == 2 || ac.RoleID == 9)
                {
                    if (ID > 0)
                    {
                        var setNoti = SendNotiEmailController.GetByID(10);
                        var com = ComplainController.GetByID(ID);
                        if (com != null)
                        {
                            int status = lbStatus.SelectedValue.ToInt();
                            int statusold = Convert.ToInt32(com.Status);

                            ComplainController.UpdateAmount(com.ID, txtAmountVND.Text.Trim().ToString(), DateTime.Now, username_current, txtComplainText.Text.ToString());

                            if (status != 3)
                            {
                                if (status != statusold)
                                {
                                    ComplainController.Update(com.ID, txtAmountVND.Text.Trim().ToString(), status, DateTime.Now, username_current, txtComplainText.Text.ToString());
                                    if (status == 0)
                                    {
                                        string uReceive = hdfUserName.Value.Trim().ToLower();
                                        var u = AccountController.GetByUsername(uReceive);
                                        if (setNoti != null)
                                        {
                                            if (setNoti.IsSentNotiUser == true)
                                            {
                                                NotificationsController.Inser(Convert.ToInt32(u.ID),
                                                AccountController.GetByID(Convert.ToInt32(u.ID)).Username, Convert.ToInt32(com.OrderID), "BQT Nhập Sỉ China đã hủy khiếu nại đơn hàng: " + com.OrderID + " của bạn.", 5, currentDate, ac.Username, false);
                                            }
                                        }
                                    }
                                    else if (status == 4)
                                    {
                                        ComplainController.UpdateAdminStatus(com.ID, 1);
                                        if (setNoti != null)
                                        {
                                            if (setNoti.IsSentNotiAdmin == true)
                                            {
                                                var admins = AccountController.GetAllByRoleID(0);
                                                if (admins.Count > 0)
                                                {
                                                    foreach (var admin in admins)
                                                    {
                                                        NotificationsController.Inser(admin.ID, admin.Username, Convert.ToInt32(com.OrderID),
                                                        "Bạn có khiếu nại cần duyệt cho đơn hàng ID là #" + Convert.ToInt32(com.OrderID) + ". CLick vào để xem", 5, currentDate, ac.Username, false);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
                        }
                    }
                }
            }

            //if (ID > 0)
            //{
            //    var com = ComplainController.GetByID(ID);
            //    if (com != null)
            //    {
            //        var setNoti = SendNotiEmailController.GetByID(10);
            //        int status = lbStatus.SelectedValue.ToInt();

            //        ComplainController.Update(com.ID, txtAmountVND.Text.ToString(), status, DateTime.Now, username_current);
            //        if (status == 3)
            //        {
            //            string uReceive = hdfUserName.Value.Trim().ToLower();
            //            var u = AccountController.GetByUsername(uReceive);
            //            if (u != null)
            //            {
            //                int UID = u.ID;
            //                double wallet = Convert.ToDouble(u.Wallet);
            //                wallet = wallet + Convert.ToDouble(txtAmountVND.Text);

            //                AccountController.updateWallet(u.ID, wallet, currentDate, username_current);
            //                HistoryPayWalletController.Insert(u.ID, u.Username, Convert.ToInt32(com.OrderID), Convert.ToDouble(txtAmountVND.Text),
            //                    u.Username + " đã được hoàn tiền khiếu nại của đơn hàng: " + com.OrderID + " vào tài khoản.",
            //                    wallet, 2, 7, currentDate, username_current);
            //                if (setNoti != null)
            //                {
            //                    if (setNoti.IsSentNotiUser == true)
            //                    {
            //                        NotificationsController.Inser(Convert.ToInt32(u.ID),
            //                   AccountController.GetByID(Convert.ToInt32(u.ID)).Username, Convert.ToInt32(com.OrderID),
            //                   "Admin đã duyệt khiếu nại đơn hàng: " + com.OrderID + "  của bạn.",
            //                   5, currentDate, ac.Username, false);
            //                        string strPathAndQuery = Request.Url.PathAndQuery;
            //                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
            //                        string datalink = "" + strUrl + "khieu-nai/";
            //                        PJUtils.PushNotiDesktop(u.ID, "Admin đã duyệt khiếu nại đơn hàng: " + com.OrderID + "  của bạn.", datalink);
            //                    }

            //                    if (setNoti.IsSendEmailUser == true)
            //                    {
            //                        try
            //                        {
            //                            PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", u.Email,
            //                                "Thông báo tại NHAPSICHINA.COM.",
            //                                "Admin đã duyệt khiếu nại đơn hàng: "
            //                                + com.OrderID + "  của bạn.", "");
            //                        }
            //                        catch { }
            //                    }
            //                }

            //                //NotificationController.Inser(ac.ID, ac.Username, Convert.ToInt32(u.ID),
            //                //    AccountController.GetByID(Convert.ToInt32(u.ID)).Username, Convert.ToInt32(com.OrderID),
            //                //    "<a href=\"/khieu-nai?ordershopcode=" + com.OrderID + "\" target=\"_blank\">Admin đã duyệt khiếu nại đơn hàng: " + com.OrderID + "  của bạn.</a>", 0,
            //                //    5, currentDate, ac.Username);
            //            }
            //        }
            //        else if (status == 4)
            //        {
            //            string uReceive = hdfUserName.Value.Trim().ToLower();
            //            var u = AccountController.GetByUsername(uReceive);
            //            if (u != null)
            //            {
            //                int UID = u.ID;
            //                if (setNoti != null)
            //                {
            //                    if (setNoti.IsSentNotiUser == true)
            //                    {
            //                        NotificationsController.Inser(Convert.ToInt32(u.ID),
            //                AccountController.GetByID(Convert.ToInt32(u.ID)).Username, Convert.ToInt32(com.OrderID), "Admin đã hủy khiếu nại đơn hàng: " + com.OrderID + "  của bạn.", 5, currentDate, ac.Username, false);
            //                        string strPathAndQuery = Request.Url.PathAndQuery;
            //                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
            //                        string datalink = "" + strUrl + "khieu-nai/";
            //                        PJUtils.PushNotiDesktop(u.ID, "Admin đã hủy khiếu nại đơn hàng: " + com.OrderID + "  của bạn.", datalink);
            //                    }

            //                    if (setNoti.IsSendEmailUser == true)
            //                    {
            //                        try
            //                        {
            //                            PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy",
            //                                u.Email,
            //                                "Thông báo tại NHAPSICHINA.COM.", "Admin đã hủy khiếu nại đơn hàng: "
            //                                + com.OrderID + "  của bạn.", "");
            //                        }
            //                        catch { }
            //                    }
            //                }
            //                //NotificationController.Inser(ac.ID, ac.Username, Convert.ToInt32(u.ID),
            //                //   AccountController.GetByID(Convert.ToInt32(u.ID)).Username, Convert.ToInt32(com.OrderID),
            //                //   "<a href=\"/khieu-nai?ordershopcode=" + com.OrderID + "\" target=\"_blank\">Admin đã hủy khiếu nại đơn hàng: " + com.OrderID + "  của bạn.</a>", 0,
            //                //   5, currentDate, ac.Username);
            //            }
            //        }
            //        PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công", "s", true, BackLink, Page);
            //    }
            //}
        }

        protected void btnSaveAdd_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var ac = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (ac != null)
            {
                if (ac.RoleID == 0 || ac.RoleID == 2 || ac.RoleID == 9)
                {
                    int MainOrderID = Convert.ToInt32(txtOrderID.Text);
                    double Amount = Convert.ToInt32(txtMoney.Text.Trim());
                    string Content = txtContent.Text;
                    int Type = ddlType.SelectedValue.ToInt();
                    string value = hdfListIMG.Value;

                    var shops = MainOrderController.GetAllByID(MainOrderID);
                    if (shops != null)
                    {
                        int UID = Convert.ToInt32(shops.UID);

                        var Username = AccountController.GetByID(UID).Username;

                        string link = "";
                        string[] listIMG = value.Split('|');

                        for (int i = 0; i < listIMG.Length - 1; i++)
                        {
                            string imageData = listIMG[i];
                            string path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/KhieuNaiIMG/");
                            string date = DateTime.Now.ToString("dd-MM-yyyy");
                            string time = DateTime.Now.ToString("hh:mm tt");
                            Page page = (Page)HttpContext.Current.Handler;
                            //  TextBox txtCampaign = (TextBox)page.FindControl("txtCampaign");
                            string k = i.ToString();
                            string fileNameWitPath = path + k + "-" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "") + ".png";
                            string linkIMG = "/Uploads/KhieuNaiIMG/" + k + "-" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "") + ".png";

                            //   string fileNameWitPath = path + s + ".png";
                            byte[] data;
                            string convert;
                            string contenttype;

                            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                            {
                                using (BinaryWriter bw = new BinaryWriter(fs))
                                {
                                    if (imageData.Contains("data:image/png"))
                                    {
                                        convert = imageData.Replace("data:image/png;base64,", String.Empty);
                                        data = Convert.FromBase64String(convert);
                                        contenttype = ".png";
                                        var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                        if (result)
                                        {
                                            bw.Write(data);
                                            link += linkIMG + "|";
                                        }
                                    }
                                    else if (imageData.Contains("data:image/jpeg"))
                                    {
                                        convert = imageData.Replace("data:image/jpeg;base64,", String.Empty);
                                        data = Convert.FromBase64String(convert);
                                        contenttype = ".jpeg";
                                        var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                        if (result)
                                        {
                                            bw.Write(data);
                                            link += linkIMG + "|";
                                        }
                                    }
                                    else if (imageData.Contains("data:image/gif"))
                                    {
                                        convert = imageData.Replace("data:image/gif;base64,", String.Empty);
                                        data = Convert.FromBase64String(convert);
                                        contenttype = ".gif";
                                        var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                        if (result)
                                        {
                                            bw.Write(data);
                                            link += linkIMG + "|";
                                        }
                                    }
                                    else if (imageData.Contains("data:image/jpg"))
                                    {
                                        convert = imageData.Replace("data:image/jpg;base64,", String.Empty);
                                        data = Convert.FromBase64String(convert);
                                        contenttype = ".jpg";
                                        var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                                        if (result)
                                        {
                                            bw.Write(data);
                                            link += linkIMG + "|";
                                        }
                                    }
                                }
                            }
                        }

                        string kq = ComplainController.Insert(UID, MainOrderID, Amount.ToString(), link, Content, 1, DateTime.Now, Username, Type);

                        PJUtils.ShowMessageBoxSwAlert("Tạo khiếu nại thành công", "s", true, Page);
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Đơn hàng không tồn tại trong hệ thống", "e", true, Page);
                    }

                }
            }

        }
    }
}