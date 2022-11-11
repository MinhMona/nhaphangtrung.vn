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
using System.Web.Services;
using System.Web.Script.Serialization;
using Microsoft.AspNet.SignalR;
using NHST.Hubs;

namespace NHST.manager
{
    public partial class PhieuYeuCauThanhToan : System.Web.UI.Page
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
                    HttpContext.Current.Session["ListStaff"] = null;
                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);
                    if (ac != null)
                    {
                        if (ac.RoleID == 0 || ac.RoleID == 7)
                        {

                        }
                        else
                            Response.Redirect("/manager/orderlist");
                        if (ac.RoleID == 0)
                        {
                            pnStaff.Visible = true;
                        }
                    }
                    //if (ac.RoleID == 0)
                    //    btnExcel.Visible = true;
                    if (Request.QueryString["page"] != null)
                    {
                        int a = Request.QueryString["page"].ToInt(0);
                        //gr.CurrentPageIndex = a;
                    }
                    //loadDateMain();
                    loadFilter();
                    LoadData();
                }
            }
        }
        public void loadFilter()
        {           
            ddlStatus.SelectedValue = "-1";
            var salers = AccountController.GetAllByRoleID(6);
            ddlStaffSaler.Items.Clear();
            ddlStaffSaler.Items.Insert(0, "Chọn nhân viên kinh doanh");
            if (salers.Count > 0)
            {
                ddlStaffSaler.DataSource = salers;
                ddlStaffSaler.DataBind();
            }
            var dathangs = AccountController.GetAllByRoleID(3);
            ddlStaffDH.Items.Clear();
            ddlStaffDH.Items.Insert(0, "Chọn nhân viên đặt hàng");
            if (dathangs.Count > 0)
            {
                ddlStaffDH.DataSource = dathangs;
                ddlStaffDH.DataBind();
            }
        }

        private void LoadData()
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                int OrderType = 1;
                int stype = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["stype"]))
                {
                    stype = int.Parse(Request.QueryString["stype"]);
                    ddlType.SelectedValue = stype.ToString();
                }

                int sort = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["sort"]))
                {
                    sort = Convert.ToInt32(Request.QueryString["sort"]);
                    ddlSortType.SelectedValue = sort.ToString();
                }

                int Role = Request.QueryString["role"].ToInt(-1);

                ddlPTVC.SelectedValue = Role.ToString();
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
                        for (int i = 0; i < ddlStatus.Items.Count; i++)
                        {
                            var item = ddlStatus.Items[i];
                            if (item.Value == list[j])
                            {
                                ddlStatus.Items[i].Selected = true;
                            }
                        }
                    }
                }
                string mvd = "";
                if (!string.IsNullOrEmpty(Request.QueryString["mvd"]))
                {
                    mvd = Request.QueryString["mvd"].ToString().Trim();
                    //txtSearchMVD.Text = mvd;
                }
                string mdh = "";
                if (!string.IsNullOrEmpty(Request.QueryString["mdh"]))
                {
                    mdh = Request.QueryString["mdh"].ToString().Trim();
                    //txtSearchMDH.Text = mdh;
                }

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
                if (Request.QueryString["ot"] != null)
                {
                    OrderType = Request.QueryString["ot"].ToInt(1);
                }
                if (OrderType > 0)
                {
                    int total = 0;
                    var la = MainOrderController.GetByUserIDInSQLHelperWithFilterOrderList_PhieuIn(Convert.ToInt32(ac.RoleID), Convert.ToInt32(ac.ID), OrderType, search, stype, fd, td, priceFrom, priceTo, st, Convert.ToBoolean(hasVMD), page, 20, mvd, mdh, sort, Role);
                    if (la.Count > 0)
                        total = la[0].totalrow;
                    pagingall(la, total);
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string stype = ddlType.SelectedValue;
            string searchname = tSearchName.Text.Trim();
            string fd = "";
            string td = "";
            string priceFrom = "";
            string priceTo = "";
            string status = ddlPTVC.SelectedValue;
            int SortType = Convert.ToInt32(ddlSortType.SelectedValue);

            string hasVMD = hdfCheckBox.Value;
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
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
            if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
            {
                List<string> myValues = new List<string>();
                for (int i = 0; i < ddlStatus.Items.Count; i++)
                {
                    var item = ddlStatus.Items[i];
                    if (item.Selected)
                    {
                        myValues.Add(item.Value);
                    }
                }
                st = String.Join(",", myValues.ToArray());
            }
            if (string.IsNullOrEmpty(stype) == true && string.IsNullOrEmpty(searchname) == true && fd == "" && td == "" && priceFrom == "" && priceTo == "" && status == "" && string.IsNullOrEmpty(st) == true && hasVMD == "0")
            {
                Response.Redirect("PhieuYeuCauThanhToan?ot=" + uID + "&sort=" + SortType + "");
            }
            else
            {
                Response.Redirect("PhieuYeuCauThanhToan?ot=" + uID + "&stype=" + stype + "&s=" + searchname + "&fd=" + fd + "&td=" + td + "&priceFrom=" + priceFrom + "&priceTo=" + priceTo + "&st=" + st + "&hasMVD=" + hasVMD + "&sort=" + SortType + "&role=" + status + "");
            }
        }

        [Serializable()]
        public class ListID
        {
            public int MainOrderID { get; set; }
        }


        [WebMethod]
        public static string CheckStaff(int MainOrderID)
        {
            List<ListID> ldep = new List<ListID>();
            var list = HttpContext.Current.Session["ListStaff"] as List<ListID>;
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
                        ListID d = new ListID();
                        d.MainOrderID = MainOrderID;
                        list.Add(d);
                    }
                }
                else
                {
                    ListID d = new ListID();
                    d.MainOrderID = MainOrderID;
                    list.Add(d);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(list);
            }
            else
            {
                ListID d = new ListID();
                d.MainOrderID = MainOrderID;
                ldep.Add(d);
                HttpContext.Current.Session["ListStaff"] = ldep;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(ldep);
            }
        }

        #region Pagging
        public void pagingall(List<OrderGetSQL> acs, int total)
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account acc = AccountController.GetByUsername(username_current);
            int PageSize = 20;
            if (total > 0)
            {
                tbl_Account obj_user = AccountController.GetByUsername(username_current);
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
                var list = HttpContext.Current.Session["ListStaff"] as List<ListID>;
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];                          

                    hcm.Append("<tr>");

                    hcm.Append("<td>");
                    hcm.Append("<a href =\"OrderDetail.aspx?id=" + item.ID + "\" target=\"_blank\" data-position=\"top\" ><p class=\"s-txt no-wrap\"><span class=\"total\">ID:</span><span>" + item.ID + "</span></p></a>");
                    if (!string.IsNullOrEmpty(item.MaDonTruoc.ToString()))
                    {
                        if (item.MaDonTruoc > 0)
                        {
                            hcm.Append("<a href =\"OrderDetail.aspx?id=" + item.MaDonTruoc + "\" target=\"_blank\" data-position=\"top\" ><p class=\"s-txt no-wrap\"><span class=\"total\">Được tạo từ đơn:</span><span>" + item.MaDonTruoc + "</span></p></a>");

                        }
                    }
                    hcm.Append("</td>");
                    hcm.Append("<td>" + item.Site + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"input-mvd\">");
                    if (!string.IsNullOrEmpty(item.MainOrderCode))
                    {
                        hcm.Append("<div><span class=\"value gicungduoc\" style=\"border: 1px solid lightgray; padding: 5px;\">" + item.MainOrderCode.Replace('|', ' ') + "</span></div>");
                    }
                    else
                    {
                        hcm.Append("<div><span class=\"value\" style=\"border: 1px solid lightgray; padding: 5px;\"></span></div>");
                    }
                    hcm.Append("</div>");
                    hcm.Append("</td>");

                    hcm.Append("<td style=\"font-weight:bold\">");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span>" + Math.Round(Convert.ToDouble(item.PriceVND) / Convert.ToDouble(item.Currency), 2) + " ¥</span></p>");
                    hcm.Append("</td>");

                    hcm.Append("<td style=\"font-weight:bold\">");
                    hcm.Append("<p class=\"s-txt no-wrap\"></span><span>" + Math.Round(Convert.ToDouble(item.TotalPriceReal) / Convert.ToDouble(item.Currency), 2) + " ¥</span></p>");
                    hcm.Append("</td>");

                    hcm.Append("<td style=\"font-weight:bold\">" + string.Format("{0:N0}", Convert.ToDouble(item.Deposit)) + " VND</td>");

                    hcm.Append("<td style=\"font-weight:bold\">" + string.Format("{0:N0}", Convert.ToDouble(item.HoaHongVND)) + " VND - " + item.HoaHongCYN + " ¥</td>");

                    hcm.Append("<td><textarea class=\"txtNote\">" + item.StaffNote + "</textarea>");
                    hcm.Append("<a href =\"javascript:;\" class=\"btn btn-info\" onclick=\"updateNote($(this),'" + item.ID + "')\">Cập nhật</a>");
                    hcm.Append("<span class=\"update-info\" style=\"width:100%; clear:both; float:left; color: green; display:none\">Cập nhật thành công</span></td>");

                    hcm.Append("<td style=\"font-weight:bold\">");
                    hcm.Append("<p class=\"s-txt  s-txt no-wrap\"><span>" + item.Uname + "</span></p>");                    
                    hcm.Append("</td>");                  

                    #region NV đặt hàng
                    hcm.Append("<td style=\"pointer-events: none;\">");
                    hcm.Append("<div>");
                    hcm.Append("<select name=\"\" onchange=\"ChooseDathang('" + item.ID + "', $(this))\" id=\"\">");
                    hcm.Append("  <option value=\"0\">Chọn nhân viên đặt hàng</option>");
                    var dathangs = AccountController.GetAllByRoleID(3);
                    if (dathangs.Count > 0)
                    {
                        foreach (var temp in dathangs)
                        {
                            if (temp.ID == item.DathangID)
                                hcm.Append("  <option selected value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                            else
                                hcm.Append("  <option value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                        }
                    }
                    hcm.Append("</select>");
                    hcm.Append("</div>");


                    hcm.Append("</td>");

                    #endregion

                    if (item.OrderDone)
                    {
                        hcm.Append("<td><input checked type=\"checkbox\" class=\"filled-in chk-check-option\" disabled/><span class=\"checkColor\"></span></td>");
                    }
                    else
                    {
                        hcm.Append("<td><input type=\"checkbox\" class=\"filled-in chk-check-option\" disabled/><span class=\"checkColor\"></span></td>");
                    }

                    hcm.Append("<td>");
                    hcm.Append(" <div class=\"action-table\">");                   
                    if (list != null)
                    {
                        var check = list.Where(x => x.MainOrderID == item.ID).FirstOrDefault();
                        if (check != null)
                        {
                            hcm.Append(" <label><input type=\"checkbox\" checked onchange=\"CheckStaff(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                        }
                        else
                        {
                            hcm.Append(" <label><input type=\"checkbox\" onchange=\"CheckStaff(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                        }
                    }
                    else
                    {
                        hcm.Append(" <label><input type=\"checkbox\" onchange=\"CheckStaff(" + item.ID + ")\"  data-id=\"" + item.ID + "\"><span></span></label>");
                    }
                    hcm.Append("</div>");
                    hcm.Append("</td>");

                    hcm.Append("</tr>");
                }
                ltr.Text = hcm.ToString();
            }
        }

        [WebMethod]
        public static string UpdateBrand(int ID, string Note)
        {
            var mo = MainOrderController.GetAllByID(ID);
            if (mo != null)
            {
                MainOrderController.UpdateBrand(ID, Note);
                return "ok";
            }
            else
                return "none";
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                int OrderType = 1;
                int stype = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["stype"]))
                {
                    stype = int.Parse(Request.QueryString["stype"]);
                    ddlType.SelectedValue = stype.ToString();
                }

                int sort = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["sort"]))
                {
                    sort = Convert.ToInt32(Request.QueryString["sort"]);
                    ddlSortType.SelectedValue = sort.ToString();
                }

                int Role = Request.QueryString["role"].ToInt(-1);

                ddlPTVC.SelectedValue = Role.ToString();
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
                        for (int i = 0; i < ddlStatus.Items.Count; i++)
                        {
                            var item = ddlStatus.Items[i];
                            if (item.Value == list[j])
                            {
                                ddlStatus.Items[i].Selected = true;
                            }
                        }
                    }
                }
                string mvd = "";
                if (!string.IsNullOrEmpty(Request.QueryString["mvd"]))
                {
                    mvd = Request.QueryString["mvd"].ToString().Trim();
                    //txtSearchMVD.Text = mvd;
                }
                string mdh = "";
                if (!string.IsNullOrEmpty(Request.QueryString["mdh"]))
                {
                    mdh = Request.QueryString["mdh"].ToString().Trim();
                    //txtSearchMDH.Text = mdh;
                }

                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                {
                    search = Request.QueryString["s"].ToString().Trim();
                    tSearchName.Text = search;
                }
                //int page = 0;
                //Int32 Page = GetIntFromQueryString("Page");
                //if (Page > 0)
                //{
                //    page = Page - 1;
                //}
                if (Request.QueryString["ot"] != null)
                {
                    OrderType = Request.QueryString["ot"].ToInt(1);
                }
                List<ListID> ldep = new List<ListID>();

                var la = MainOrderController.GetByUserIDInSQLHelperWithFilterOrderList_PhieuIn_excel(Convert.ToInt32(ac.RoleID), Convert.ToInt32(ac.ID), OrderType, search, stype, fd, td, priceFrom, priceTo, st, Convert.ToBoolean(hasVMD), mvd, mdh, sort, Role);

                var list1 = HttpContext.Current.Session["ListStaff"] as List<ListID>;
                if (list1 != null)
                {
                    if (list1.Count > 0)
                    {
                        foreach (var item1 in list1)
                        {
                            var a = MainOrderController.GetByID(item1.MainOrderID);     
                            string orderstatus = "";
                            int currentOrderStatus = Convert.ToInt32(a.Status);
                            switch (currentOrderStatus)
                            {
                                case 0:
                                    orderstatus = "Đơn mới";
                                    break;
                                case 1:
                                    orderstatus = "Đơn hàng hủy";
                                    break;
                                case 2:
                                    orderstatus = "Đơn đã cọc";
                                    break;
                                case 3:
                                    orderstatus = "Đơn người bán giao";
                                    break;
                                case 4:
                                    orderstatus = "Đơn chờ mua hàng";
                                    break;
                                case 5:
                                    orderstatus = "Đơn đã mua hàng";
                                    break;
                                case 6:
                                    orderstatus = "Kho Trung Quốc nhận hàng";
                                    break;
                                case 7:
                                    orderstatus = "Trên đường về Việt Nam";
                                    break;
                                case 8:
                                    orderstatus = "Trong kho Hà Nội";
                                    break;
                                case 9:
                                    orderstatus = "Đã thanh toán";
                                    break;
                                case 10:
                                    orderstatus = "Đã hoàn thành";
                                    break;
                                case 11:
                                    orderstatus = "Đang giao hàng";
                                    break;
                                case 12:
                                    orderstatus = "Đơn khiếu nại";
                                    break;
                                default:
                                    break;
                            }

                            if (currentOrderStatus != 5)
                            {
                                HistoryOrderChangeController.Insert(item1.MainOrderID, Convert.ToInt32(a.UID), ac.Username, ac.Username +
                                " đã đổi trạng thái của đơn hàng ID là: " + item1.MainOrderID + ", từ: " + orderstatus + ", sang: " + "Đơn đã mua hàng" + "", 0, DateTime.Now);
                            }       
                            if (a.DateBuyOK == null)                            
                                MainOrderController.UpdateDateBuyOK(a.ID, DateTime.Now);                           
                            MainOrderController.UpdateStatus(item1.MainOrderID, Convert.ToInt32(a.UID), 5);
                        }
                        //PJUtils.ShowMessageBoxSwAlert("Thanh toán yêu cầu thành công!", "s", true, Page);
                        Response.Redirect("/manager/PhieuYeuCauThanhToan.aspx");
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Không có đơn để thanh toán!", "e", true, Page);
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Không có đơn để thanh toán!", "e", true, Page);
                }
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

    }
}