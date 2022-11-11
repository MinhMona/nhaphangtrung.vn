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
    public partial class OrderList : System.Web.UI.Page
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
                    {
                        if (ac.RoleID == 1)
                            Response.Redirect("/trang-chu");

                        if (ac.RoleID == 0 || ac.RoleID == 2)
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

        #region Button status
        //tất cả
        protected void btnAll_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = -1;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đơn mới
        protected void btn0_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 0;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã hủy
        protected void btn1_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 1;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã đặt cọc
        protected void btn2_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 2;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //chờ đã mua hàng
        protected void btn4_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 4;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã mua hàng
        protected void btn5_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 5;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đơn người bán giao
        protected void btn3_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 3;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã về kho tq
        protected void btn6_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 6;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //trên đường về kho vn
        protected void btn7_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 7;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //về kho vn
        protected void btn8_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 8;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đang giao hàng
        protected void btn11_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 11;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã thanh toán
        protected void btn9_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 9;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã hoàn thành
        protected void btn10_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 10;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }
        //đã khiếu nại
        protected void btn12_Click(object sender, EventArgs e)
        {
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            int st = 12;
            Response.Redirect("orderlist?ot=" + uID + "&st=" + st + "");
        }

        #endregion

        public void loadDateMain()
        {
            var o = MainOrderController.GetByStatus(9);
            if (o != null)
            {
                foreach (var item in o)
                {
                    int dem = 0;
                    var smalls = SmallPackageController.GetByMainOrderID(item.ID);
                    if (smalls != null)
                    {
                        dem = smalls.Where(x => x.Status != 6).ToList().Count();
                    }
                    if (item.DateToFis != null && dem == 0)
                    {
                        var date = DateTime.Now;
                        if (item.ReceivePlace.ToInt(0) == 1)
                        {
                            if ((date - item.DateToFis.Value).Days > 4)
                            {
                                MainOrderController.UpdateStatusByID(item.ID, 10);
                                if (item.CompleteDate == null)
                                {
                                    MainOrderController.UpdateCompleteDate(item.ID, date);
                                }
                            }
                        }
                        else if (item.ReceivePlace.ToInt(0) == 3)
                        {
                            if ((date - item.DateToFis.Value).Days > 8)
                            {
                                MainOrderController.UpdateStatusByID(item.ID, 10);
                                if (item.CompleteDate == null)
                                {
                                    MainOrderController.UpdateCompleteDate(item.ID, date);
                                }
                            }
                        }

                    }
                }
            }
        }
        public void loadFilter()
        {
            ddlStatus.SelectedValue = "-1";
            var salers = AccountController.GetAllByRoleID(6);
            ddlStaffSaler.Items.Clear();
            ddlStaffSaler.Items.Insert(0, "Chọn nhân viên kinh doanh");
            searchNVKD.Items.Clear();
            searchNVKD.Items.Insert(0, "Chọn nhân viên");
            if (salers.Count > 0)
            {
                ddlStaffSaler.DataSource = salers;
                ddlStaffSaler.DataBind();

                searchNVKD.DataSource = salers;
                searchNVKD.DataBind();
            }
            var dathangs = AccountController.GetAllByRoleID(3);
            ddlStaffDH.Items.Clear();
            ddlStaffDH.Items.Insert(0, "Chọn nhân viên đặt hàng");
            searchNVDH.Items.Clear();
            searchNVDH.Items.Insert(0, "Chọn nhân viên");
            if (dathangs.Count > 0)
            {
                ddlStaffDH.DataSource = dathangs;
                ddlStaffDH.DataBind();

                searchNVDH.DataSource = dathangs;
                searchNVDH.DataBind();
            }
            var cskhs = AccountController.GetAllByRoleID(9);
            searchCSKH.Items.Clear();
            searchCSKH.Items.Insert(0, "Chọn nhân viên");
            if (cskhs.Count > 0)
            {
                searchCSKH.DataSource = cskhs;
                searchCSKH.DataBind();
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

                int nvkd = Request.QueryString["nvkd"].ToInt(0);
                searchNVKD.SelectedValue = Request.QueryString["nvkd"];

                int nvdh = Request.QueryString["nvdh"].ToInt(0);
                searchNVDH.SelectedValue = Request.QueryString["nvdh"];

                int nvcs = Request.QueryString["nvcs"].ToInt(0);
                searchCSKH.SelectedValue = Request.QueryString["nvcs"];

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
                    txtSearchMVD.Text = mvd;
                }
                string mdh = "";
                if (!string.IsNullOrEmpty(Request.QueryString["mdh"]))
                {
                    mdh = Request.QueryString["mdh"].ToString().Trim();
                    txtSearchMDH.Text = mdh;
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
                    var la = MainOrderController.GetByUserIDInSQLHelperWithFilterOrderList(Convert.ToInt32(ac.RoleID), Convert.ToInt32(ac.ID), OrderType, search, stype, fd, td, priceFrom, priceTo, st, Convert.ToBoolean(hasVMD), page, 20, mvd, mdh, sort, Role, nvdh, nvkd, nvcs);
                    if (la.Count > 0)
                        total = la[0].totalrow;
                    pagingall(la, total);
                    var os = MainOrderController.GetAllByOrderType(OrderType);
                    if (ac.RoleID == 3)
                    {
                        var sttall = os.Where(o => o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt0 = os.Where(o => o.Status == 0 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt1 = os.Where(o => o.Status == 1 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt2 = os.Where(o => o.Status == 2 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt3 = os.Where(o => o.Status == 3 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt4 = os.Where(o => o.Status == 4 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt5 = os.Where(o => o.Status == 5 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt6 = os.Where(o => o.Status == 6 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt7 = os.Where(o => o.Status == 7 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt8 = os.Where(o => o.Status == 8 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt9 = os.Where(o => o.Status == 9 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt10 = os.Where(o => o.Status == 10 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt11 = os.Where(o => o.Status == 11 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();
                        var stt12 = os.Where(o => o.Status == 12 && o.DathangID == Convert.ToInt32(ac.ID)).ToList();

                        bttnAll.Text = "Tất cả (" + sttall.Count + ")";
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
                    }
                    else if (ac.RoleID == 6)
                    {
                        var sttall = os.Where(o => o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt0 = os.Where(o => o.Status == 0 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt1 = os.Where(o => o.Status == 1 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt2 = os.Where(o => o.Status == 2 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt3 = os.Where(o => o.Status == 3 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt4 = os.Where(o => o.Status == 4 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt5 = os.Where(o => o.Status == 5 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt6 = os.Where(o => o.Status == 6 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt7 = os.Where(o => o.Status == 7 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt8 = os.Where(o => o.Status == 8 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt9 = os.Where(o => o.Status == 9 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt10 = os.Where(o => o.Status == 10 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt11 = os.Where(o => o.Status == 11 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();
                        var stt12 = os.Where(o => o.Status == 12 && o.SalerID == Convert.ToInt32(ac.ID)).ToList();

                        bttnAll.Text = "Tất cả (" + sttall.Count + ")";
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
                    }
                    else if (ac.RoleID == 9)
                    {
                        var sttall = os.Where(o => o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt0 = os.Where(o => o.Status == 0 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt1 = os.Where(o => o.Status == 1 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt2 = os.Where(o => o.Status == 2 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt3 = os.Where(o => o.Status == 3 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt4 = os.Where(o => o.Status == 4 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt5 = os.Where(o => o.Status == 5 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt6 = os.Where(o => o.Status == 6 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt7 = os.Where(o => o.Status == 7 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt8 = os.Where(o => o.Status == 8 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt9 = os.Where(o => o.Status == 9 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt10 = os.Where(o => o.Status == 10 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt11 = os.Where(o => o.Status == 11 && o.CSID == Convert.ToInt32(ac.ID)).ToList();
                        var stt12 = os.Where(o => o.Status == 12 && o.CSID == Convert.ToInt32(ac.ID)).ToList();

                        bttnAll.Text = "Tất cả (" + sttall.Count + ")";
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
                    }
                    else
                    {
                        var sttall = os.ToList();
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

                        bttnAll.Text = "Tất cả (" + sttall.Count + ")";
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
                    }
                    if (ac.RoleID == 0)
                    {
                        double totalpricevnd = MainOrderController.GetTotalPriceVND(st, "mo.TotalPriceVND", search, stype, OrderType, Role, fd, td, nvdh, nvkd, nvcs);
                        double totaldeposit = MainOrderController.GetTotalPriceVND(st, "mo.Deposit", search, stype, OrderType, Role, fd, td, nvdh, nvkd, nvcs);
                        double totalnotpay = totalpricevnd - totaldeposit;

                        ltrTotalPriceVND.Text = string.Format("{0:N0}", totalpricevnd);
                        ltrDeposit.Text = string.Format("{0:N0}", totaldeposit);
                        ltrNotPay.Text = string.Format("{0:N0}", totalnotpay);
                        TotalMoney.Visible = true;
                    }
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
            int nvdh = 0;
            if (!string.IsNullOrEmpty(searchNVDH.SelectedValue))
            {
                nvdh = searchNVDH.SelectedValue.ToInt(0);
            }
            int nvkd = 0;
            if (!string.IsNullOrEmpty(searchNVKD.SelectedValue))
            {
                nvkd = searchNVKD.SelectedValue.ToInt(0);
            }
            int nvcs = 0;
            if (!string.IsNullOrEmpty(searchCSKH.SelectedValue))
            {
                nvcs = searchCSKH.SelectedValue.ToInt(0);
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
                Response.Redirect("orderlist?ot=" + uID + "&sort=" + SortType + "");
            }
            else
            {
                Response.Redirect("orderlist?ot=" + uID + "&stype=" + stype + "&s=" + searchname + "&fd=" + fd + "&td=" + td + "&priceFrom=" + priceFrom + "&priceTo=" + priceTo + "&st=" + st + "&hasMVD=" + hasVMD + "&sort=" + SortType + "&role=" + status + "&nvkd=" + nvkd + "&nvdh=" + nvdh + "&nvcs=" + nvcs + "");
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
                double tongtien = 0;
                double tiendacoc = 0;
                var list = HttpContext.Current.Session["ListStaff"] as List<ListID>;
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];
                    tongtien += Convert.ToDouble(item.TotalPriceVND);
                    tiendacoc += Convert.ToDouble(item.Deposit);
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


                    hcm.Append("<hr width=\"100%\" />");

                    hcm.Append(item.Cancel);
                    hcm.Append(item.Created);
                    hcm.Append(item.DepostiDate);
                    hcm.Append(item.DateBuy);
                    hcm.Append(item.DateBuyOK);
                    hcm.Append(item.DateShipper);
                    hcm.Append(item.DateTQ);
                    hcm.Append(item.DateToVN);
                    hcm.Append(item.DateVN);
                    hcm.Append(item.DateToShip);
                    hcm.Append(item.DatePay);
                    hcm.Append(item.CompleteDate);
                    hcm.Append(item.DateToCancel);
                    hcm.Append("</td>");
                    hcm.Append("<td>" + item.anhsanpham + "</td>");

                    hcm.Append("<td style=\"font-weight:bold\">");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Tỷ giá:</span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.Currency)) + " Đ</span></p>");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Tổng tệ:</span><span>¥" + Math.Round(Convert.ToDouble(item.PriceVND) / Convert.ToDouble(item.Currency), 2) + "</span></p>");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Tổng tiền:</span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.TotalPriceVND)) + " Đ</span></p>");
                    hcm.Append("<p class=\"s-txt blue-text no-wrap\"><span class=\"total\">Đã trả:</span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.Deposit)) + " Đ</span></p>");
                    hcm.Append("<p class=\"s-txt red-text no-wrap\"><span class=\"total\">Còn lại:</span><span>" + string.Format("{0:N0}", Math.Round(Convert.ToDouble(item.TotalPriceVND) - Convert.ToDouble(item.Deposit))) + " Đ</span></p>");
                    hcm.Append("<p class=\"s-txt  no-wrap\"><span class=\"total\">Shop TQ:</span><span>" + item.ShopName + "</span></p>");
                    hcm.Append("<p class=\"s-txt  no-wrap\"><span class=\"total\">Website:</span><span>" + item.Site + "</span></p>");
                    hcm.Append("<p class=\"s-txt  no-wrap\"><span class=\"total\">TK thanh toán:</span><span> " + item.StaffNote + "</span></p>");
                    hcm.Append("</td>");

                    //hcm.Append("<td>" + string.Format("{0:N0}", Convert.ToDouble(item.Deposit)) + " VNĐ</td>");
                    var ac = AccountController.GetByUsername(item.Uname);
                    var acif = AccountInfoController.GetByUserID(ac.ID);
                    hcm.Append("<td style=\"font-weight:bold\">");
                    hcm.Append("<p class=\"s-txt  \"><span>" + item.Uname + "</span></p>");
                    hcm.Append("<p class=\"s-txt  \"><span>" + acif.FirstName + " " + acif.LastName + "</span></p>");
                    if (obj_user.RoleID != 3)
                    {
                        hcm.Append("<p class=\"s-txt  \"><span>" + acif.Email + "</span></p>");
                        hcm.Append("<p class=\"s-txt  \"><span>" + acif.Phone + "</span></p>");
                        hcm.Append("<p class=\"s-txt  \"><span>" + acif.Address + "</span></p>");
                    }
                    hcm.Append("</td>");

                    #region NV đặt hàng
                    hcm.Append("<td>");

                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Nhân viên đặt hàng:</span></p>");
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
                    //NV kinh doanh
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Nhân viên kinh doanh:</span></p>");
                    hcm.Append("<div>");
                    hcm.Append("<select name=\"\" onchange=\"ChooseSaler('" + item.ID + "', $(this))\"  id=\"\">");
                    hcm.Append("  <option value=\"0\">Chọn nhân viên kinh doanh</option>");
                    var salers = AccountController.GetAllByRoleID(6);
                    if (salers.Count > 0)
                    {
                        foreach (var temp in salers)
                        {
                            if (temp.ID == item.SalerID)
                                hcm.Append("  <option selected value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                            else
                                hcm.Append("  <option value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                        }
                    }
                    hcm.Append("</select>");
                    hcm.Append("</div>");
                    ///NV CSKH
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Nhân viên CSKH:</span></p>");
                    hcm.Append("<div>");
                    hcm.Append("<select name=\"\" onchange=\"ChooseCSKH('" + item.ID + "', $(this))\"  id=\"\">");
                    hcm.Append("  <option value=\"0\">Chọn nhân viên CSKH</option>");
                    var cskh = AccountController.GetAllByRoleIDAndRoleID(2, 9);
                    if (cskh.Count > 0)
                    {
                        foreach (var temp in cskh)
                        {
                            if (temp.ID == item.CSID)
                                hcm.Append("  <option selected value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                            else
                                hcm.Append("  <option value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                        }
                    }
                    hcm.Append("</select>");
                    hcm.Append("</div>");

                    hcm.Append("</td>");
                    #endregion



                    if (item.Status != 1)
                    {
                        hcm.Append("<td>");
                        hcm.Append("<div class=\"input-mvd\">");
                        if (!string.IsNullOrEmpty(item.MainOrderCode))
                        {
                            hcm.Append("<div><p><span>Mã đơn hàng:</span></p><span class=\"value gicungduoc\" style=\"border: 1px solid lightgray; padding: 5px;\">" + item.MainOrderCode.Replace('|', ' ') + "</span></div>");
                        }
                        else
                        {
                            hcm.Append("<div><p><span>Mã đơn hàng:</span></p><span class=\"value\" style=\"border: 1px solid lightgray; padding: 5px;\"></span></div>");
                        }

                        if (!string.IsNullOrEmpty(item.OrderTransactionCode))
                        {
                            hcm.Append("<div><p><span>Mã vận đơn:</span></p>");
                            hcm.Append("<span class=\"value gicungduoc\" style=\"border: 1px solid lightgray; padding: 5px;\">" + item.OrderTransactionCode.Replace('|', ' ') + "</span><br><br>");
                            hcm.Append("</div>");
                        }
                        else
                        {
                            hcm.Append("<div><p><span>Mã vận đơn:</span></p>");
                            hcm.Append("<span class=\"value\" style=\"border: 1px solid lightgray; padding: 5px;\"></span><br><br>");
                            hcm.Append("</div>");
                        }
                        hcm.Append("</div>");
                        if (item.IsDoneSmallPackage)
                        {
                            hcm.Append("" + item.hasSmallpackage + "");
                        }
                        hcm.Append("</td>");
                    }
                    else
                    {
                        hcm.Append("<td></td>");
                    }

                    hcm.Append("<td>" + MainOrderController.GetAllByID(item.ID).Note + "</td>");
                    hcm.Append("<td><textarea class=\"txtNote\">" + MainOrderController.GetAllByID(item.ID).NoteManager + "</textarea>");
                    if (obj_user.RoleID == 0 || obj_user.RoleID == 7 || obj_user.RoleID == 3 || obj_user.RoleID == 2 || obj_user.RoleID == 9)
                    {
                        hcm.Append("<a href =\"javascript:;\" class=\"btn btn-info\" onclick=\"updateNote($(this),'" + item.ID + "')\">Cập nhật</a>");
                    }
                    hcm.Append("<span class=\"update-info\" style=\"width:100%; clear:both; float:left; color blue; display:none\">Cập nhật thành công</span></td>");
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
                    //hcm.Append("<a href =\"OrderDetail.aspx?id=" + item.ID + "\" target=\"_blank\" data-position=\"top\" ><i class=\"material-icons\">edit</i><span>Xem chi tiết</span></a>");
                    hcm.Append("<a href =\"Pay-Order.aspx?id=" + item.ID + "\" target=\"_blank\" data-position=\"top\" ><i class=\"material-icons\">payment</i><span>Thanh toán</span></a>");
                    if (acc.RoleID == 3 || acc.RoleID == 0)
                    {
                        hcm.Append("    <a href=\"javascript:;\" onclick=\"OrderSame('" + item.ID + "')\" data-position=\"top\"><i class=\"material-icons\">reorder</i><span>Tạo đơn tương tự</span></a>");
                    }
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    hcm.Append("</tr>");
                }
                ltr.Text = hcm.ToString();
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
                    int UID = Convert.ToInt32(mosame.UID);
                    var User = AccountController.GetByID(UID);
                    double current = Convert.ToDouble(ConfigurationController.GetByTop1().Currency);
                    if (!string.IsNullOrEmpty(User.Currency.ToString()))
                    {
                        if (Convert.ToDouble(User.Currency) > 0)
                        {
                            current = Convert.ToDouble(User.Currency);
                        }
                    }
                    int salerID = User.SaleID.ToString().ToInt(0);
                    int dathangID = User.DathangID.ToString().ToInt(0);
                    int cskhID = User.CSID.ToString().ToInt(0);

                    double total = 0;
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
                            "0", pricepro.ToString(), priceproCYN.ToString(), item.Note, mosame.FullName, mosame.Address, mosame.Email,
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

                        //var admins = AccountController.GetAllByRoleID(0);
                        //if (admins.Count > 0)
                        //{
                        //    foreach (var admin in admins)
                        //    {
                        //        NotificationController.Inser(Convert.ToInt32(obj_user.ID), username, admin.ID, admin.Username, idkq, "Có đơn hàng mới ID là: " + idkq, 0, 1, currentDate, username, false);
                        //    }
                        //}

                        //var managers = AccountController.GetAllByRoleID(2);
                        //if (managers.Count > 0)
                        //{
                        //    foreach (var manager in managers)
                        //    {
                        //        NotificationController.Inser(Convert.ToInt32(obj_user.ID), username, manager.ID, manager.Username, 0, "Có đơn hàng mới ID là: " + idkq, 0, 1, currentDate, username, false);
                        //    }
                        //}

                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                        hubContext.Clients.All.addNewMessageToPage("", "");
                        Response.Redirect("/manager/orderlist.aspx");
                    }
                }
            }
            else
            {
                Response.Redirect("/");
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

        protected void btnSearchMVD_Click(object sender, EventArgs e)
        {
            string mvd = txtSearchMVD.Text.Trim();
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(0);
            }
            if (!string.IsNullOrEmpty(mvd))
            {
                Response.Redirect("orderlist?ot=" + uID + "&mvd=" + mvd);
            }
            else
            {
                Response.Redirect("orderlist?ot=" + uID);
            }
        }

        protected void btnSearchMDH_Click(object sender, EventArgs e)
        {
            string mdh = txtSearchMDH.Text.Trim();
            int uID = 1;
            if (Request.QueryString["ot"] != null)
            {
                uID = Request.QueryString["ot"].ToInt(1);
            }
            if (!string.IsNullOrEmpty(mdh))
            {
                Response.Redirect("orderlist?ot=" + uID + "&mdh=" + mdh);
            }
            else
            {
                Response.Redirect("orderlist?ot=" + uID);
            }
        }

        [WebMethod]
        public static string UpdateStaff(int OrderID, int StaffID, int Type)
        {
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 2)
                {
                    var mo = MainOrderController.GetAllByID(OrderID);
                    if (mo != null)
                    {
                        if (Type == 1) //1:saler - 2:dathang
                        {

                            double feebp = Convert.ToDouble(mo.FeeBuyPro);
                            DateTime CreatedDate = Convert.ToDateTime(mo.CreatedDate);
                            double salepercent = 0;
                            double salepercentaf3m = 0;
                            double dathangpercent = 0;
                            var config = ConfigurationController.GetByTop1();
                            if (config != null)
                            {
                                salepercent = Convert.ToDouble(config.SalePercent);
                                salepercentaf3m = Convert.ToDouble(config.SalePercentAfter3Month);
                                dathangpercent = Convert.ToDouble(config.DathangPercent);
                            }
                            string salerName = "";
                            string dathangName = "";

                            int salerID_old = Convert.ToInt32(mo.SalerID);
                            int dathangID_old = Convert.ToInt32(mo.DathangID);
                            int UID = 0;
                            UID = mo.UID.Value;
                            #region Saler
                            if (StaffID > 0)
                            {
                                if (StaffID == salerID_old)
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, salerID_old);
                                    if (staff != null)
                                    {
                                        int rStaffID = staff.ID;
                                        int status = Convert.ToInt32(staff.Status);
                                        if (status == 1)
                                        {
                                            var sale = AccountController.GetByID(salerID_old);
                                            if (sale != null)
                                            {
                                                salerName = sale.Username;
                                                var createdDate = DateTime.Now;

                                                var top1 = MainOrderController.GetTop1ByUID(UID);
                                                if (top1.Count > 0)
                                                {

                                                    foreach (var item in top1)
                                                    {
                                                        createdDate = Convert.ToDateTime(item.CreatedDate);
                                                    }

                                                    int d = CreatedDate.Subtract(createdDate).Days;
                                                    if (d > 90)
                                                    {
                                                        salepercentaf3m = Convert.ToDouble(staff.PercentReceive);
                                                        double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                                        StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercentaf3m.ToString(), 1,
                                                            per.ToString(), false, currentDate, username);
                                                    }
                                                    else
                                                    {
                                                        salepercent = Convert.ToDouble(staff.PercentReceive);
                                                        double per = Math.Round(feebp * salepercent / 100, 0);
                                                        StaffIncomeController.Update(rStaffID, mo.TotalPriceVND, salepercent.ToString(), 1,
                                                            per.ToString(), false, currentDate, username);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var sale = AccountController.GetByID(StaffID);
                                        if (sale != null)
                                        {
                                            salerName = sale.Username;
                                            var createdDate = DateTime.Now;

                                            var top1 = MainOrderController.GetTop1ByUID(UID);
                                            if (top1.Count > 0)
                                            {

                                                foreach (var item in top1)
                                                {
                                                    createdDate = Convert.ToDateTime(item.CreatedDate);
                                                }
                                                int d = CreatedDate.Subtract(createdDate).Days;
                                                if (d > 90)
                                                {
                                                    double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                                    StaffIncomeController.Insert(mo.ID, per.ToString(), salepercentaf3m.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                    CreatedDate, currentDate, username);
                                                }
                                                else
                                                {
                                                    double per = Math.Round(feebp * salepercent / 100, 0);
                                                    StaffIncomeController.Insert(mo.ID, per.ToString(), salepercent.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                    CreatedDate, currentDate, username);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, salerID_old);
                                    var createdDate = DateTime.Now;
                                    if (staff != null)
                                    {
                                        createdDate = staff.CreatedDate.Value;
                                        StaffIncomeController.Delete(staff.ID);
                                    }
                                    var sale = AccountController.GetByID(StaffID);
                                    if (sale != null)
                                    {
                                        salerName = sale.Username;

                                        var top1 = MainOrderController.GetTop1ByUID(UID);
                                        if (top1.Count > 0)
                                        {
                                            foreach (var item in top1)
                                            {
                                                createdDate = Convert.ToDateTime(item.CreatedDate);
                                            }

                                            int d = CreatedDate.Subtract(createdDate).Days;
                                            if (d > 90)
                                            {
                                                double per = Math.Round(feebp * salepercentaf3m / 100, 0);
                                                StaffIncomeController.Insert(mo.ID, per.ToString(), salepercentaf3m.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                CreatedDate, createdDate, username);
                                            }
                                            else
                                            {
                                                double per = Math.Round(feebp * salepercent / 100, 0);
                                                StaffIncomeController.Insert(mo.ID, per.ToString(), salepercent.ToString(), StaffID, salerName, 6, 1, per.ToString(), false,
                                                CreatedDate, createdDate, username);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            MainOrderController.UpdateStaff(mo.ID, StaffID, Convert.ToInt32(mo.DathangID), Convert.ToInt32(mo.KhoTQID), Convert.ToInt32(mo.KhoVNID));
                        }
                        else if (Type == 3)
                        {
                            MainOrderController.UpdateCSKHID(mo.ID, StaffID);
                        }
                        else
                        {
                            double feebp = Convert.ToDouble(mo.FeeBuyPro);
                            DateTime CreatedDate = Convert.ToDateTime(mo.CreatedDate);
                            double salepercent = 0;
                            double salepercentaf3m = 0;
                            double dathangpercent = 0;
                            var config = ConfigurationController.GetByTop1();
                            if (config != null)
                            {
                                salepercent = Convert.ToDouble(config.SalePercent);
                                salepercentaf3m = Convert.ToDouble(config.SalePercentAfter3Month);
                                dathangpercent = Convert.ToDouble(config.DathangPercent);
                            }
                            string salerName = "";
                            string dathangName = "";

                            int salerID_old = Convert.ToInt32(mo.SalerID);
                            int dathangID_old = Convert.ToInt32(mo.DathangID);
                            #region Đặt hàng
                            if (StaffID > 0)
                            {
                                if (StaffID == dathangID_old)
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, dathangID_old);
                                    if (staff != null)
                                    {
                                        if (staff.Status == 1)
                                        {
                                            //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                            double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                            totalPrice = Math.Round(totalPrice, 0);
                                            double totalRealPrice = 0;
                                            if (!string.IsNullOrEmpty(mo.TotalPriceReal))
                                                totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                            if (totalRealPrice > 0)
                                            {
                                                double totalpriceloi = totalPrice - totalRealPrice;
                                                totalpriceloi = Math.Round(totalpriceloi, 0);
                                                dathangpercent = Convert.ToDouble(staff.PercentReceive);
                                                double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                                //double income = totalpriceloi;
                                                StaffIncomeController.Update(staff.ID, totalRealPrice.ToString(), dathangpercent.ToString(), 1,
                                                            income.ToString(), false, currentDate, username);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        var dathang = AccountController.GetByID(StaffID);
                                        if (dathang != null)
                                        {
                                            dathangName = dathang.Username;
                                            //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                            double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                            totalPrice = Math.Round(totalPrice, 0);
                                            double totalRealPrice = 0;
                                            if (!string.IsNullOrEmpty(mo.TotalPriceReal))
                                                totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                            if (totalRealPrice > 0)
                                            {
                                                double totalpriceloi = totalPrice - totalRealPrice;
                                                totalpriceloi = Math.Round(totalpriceloi, 0);
                                                double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                                //double income = totalpriceloi;
                                                StaffIncomeController.Insert(mo.ID, totalpriceloi.ToString(), dathangpercent.ToString(), StaffID, dathangName, 3, 1,
                                                    income.ToString(), false, CreatedDate, currentDate, username);
                                            }
                                            else
                                            {
                                                StaffIncomeController.Insert(mo.ID, "0", dathangpercent.ToString(), StaffID, dathangName, 3, 1, "0", false,
                                                CreatedDate, currentDate, username);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var staff = StaffIncomeController.GetByMainOrderIDUID(mo.ID, dathangID_old);
                                    if (staff != null)
                                    {
                                        currentDate = staff.CreatedDate.Value;
                                        StaffIncomeController.Delete(staff.ID);
                                    }
                                    var dathang = AccountController.GetByID(StaffID);
                                    if (dathang != null)
                                    {
                                        dathangName = dathang.Username;
                                        //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                        double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                        totalPrice = Math.Round(totalPrice, 0);
                                        double totalRealPrice = 0;
                                        if (!string.IsNullOrEmpty(mo.TotalPriceReal))
                                            totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                        if (totalRealPrice > 0)
                                        {
                                            double totalpriceloi = totalPrice - totalRealPrice;
                                            double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                            //double income = totalpriceloi;

                                            StaffIncomeController.Insert(mo.ID, totalpriceloi.ToString(), dathangpercent.ToString(), StaffID, dathangName, 3, 1,
                                                income.ToString(), false, CreatedDate, currentDate, username);
                                        }
                                        else
                                        {
                                            StaffIncomeController.Insert(mo.ID, "0", dathangpercent.ToString(), StaffID, dathangName, 3, 1, "0", false,
                                            CreatedDate, currentDate, username);
                                        }
                                    }
                                }
                            }
                            #endregion
                            MainOrderController.UpdateStaff(mo.ID, Convert.ToInt32(mo.SalerID), StaffID, Convert.ToInt32(mo.KhoTQID), Convert.ToInt32(mo.KhoVNID));
                        }
                        return "ok";
                    }
                }
                else
                {
                    return "notpermision";
                }
            }
            return "null";
        }
        [WebMethod]
        public static string UpdateNoteManager(int ID, string NoteManager)
        {
            var ex = MainOrderController.GetByID(ID);
            if (ex != null)
            {
                MainOrderController.UpdateNoteManager(ID, NoteManager);
                return "ok";
            }
            return "none";
        }
        protected void btnUpdateStaff_Click(object sender, EventArgs e)
        {

        }
    }
}