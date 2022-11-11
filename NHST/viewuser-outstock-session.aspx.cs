using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using MB.Extensions;
using System.Text;

namespace NHST
{
    public partial class viewuser_outstock_session : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["userLoginSystem"] = "phuongnguyen";
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                else
                {
                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);

                    //if (ac.RoleID != 0 && ac.RoleID != 2 && ac.RoleID != 7)
                    //    Response.Redirect("/trang-chu");
                }
                LoadData();
            }
        }
        public void LoadData()
        {
            DateTime currentDate = DateTime.Now;
            string username_current = Session["userLoginSystem"].ToString();
            var ac = AccountController.GetByUsername(username_current);
            if (Request.QueryString["id"] != null)
            {
                int id = Request.QueryString["id"].ToInt(0);
                if (id > 0)
                {

                    ViewState["id"] = id;
                    ltrIDS.Text = "#" + id;
                    var os = OutStockSessionController.GetByID_UID(id, ac.ID);
                    if (os != null)
                    {
                        var a = AccountController.GetByID(Convert.ToInt32(os.UID));
                        var checkPrice = os.CheckPrice;
                        if (checkPrice != null)
                        {
                            chkPrice.Value = checkPrice.ToString();
                        }
                        else
                        {
                            chkPrice.Value = "false";
                        }
                        bool isShowButton = true;
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
                                                if (sm.Status != 4)
                                                {
                                                    isShowButton = false;
                                                }
                                                double weight = 0;
                                                double weightCN = Convert.ToDouble(sm.Weight);
                                                double weightKT = 0;
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

                                                string packagecode = sm.OrderTransactionCode;
                                                int Status = Convert.ToInt32(sm.Status);
                                                double payInWarehouse = 0;

                                                pg.ID = sm.ID;
                                                pg.weight = weight;

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
                                                totalPay += payInWarehouse;
                                                pg.payInWarehouse = payInWarehouse;
                                                sms.Add(pg);
                                                //SmallPackageController.UpdateWarehouseFeeDateOutWarehouse(sm.ID, payInWarehouse, currentDate);
                                                //OutStockSessionPackageController.update(p.ID, currentDate, totalDays, payInWarehouse);
                                            }
                                        }
                                    }
                                    op.totalPrice = totalPay;
                                    op.smallpackages = sms;
                                    double mustpay = 0;
                                    bool isPay = false;
                                    MainOrderController.UpdateFeeWarehouse(mID, totalPay);
                                    var ma = MainOrderController.GetAllByID(mID);
                                    if (ma != null)
                                    {
                                        double totalPriceVND = Convert.ToDouble(ma.TotalPriceVND);
                                        double deposited = Convert.ToDouble(ma.Deposit);
                                        double totalmustpay = totalPriceVND + totalPay;
                                        double totalleftpay = totalmustpay - deposited;
                                        if (totalmustpay <= deposited)
                                        {
                                            isPay = true;
                                        }
                                        else
                                        {
                                           // MainOrderController.UpdateStatus(mID, Convert.ToInt32(ma.UID), 11);
                                            mustpay = totalleftpay;
                                        }
                                    }
                                    if (isShowButton == true)
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
                        #region Render Data
                        txtFullname.Text = os.FullName;
                        txtPhone.Text = os.Phone;
                        txtUsername.Text = os.Username;
                        string listMainorder = "";
                        string listtransportationorder = "";
                        StringBuilder html = new StringBuilder();
                        StringBuilder htmlPrint = new StringBuilder();
                        if (ops.Count > 0)
                        {
                            foreach (var o in ops)
                            {
                                int orderType = o.OrderType;
                                bool isPay = o.isPay;
                                string status = "<span class=\"green-text font-weight-600\">Đã thanh toán</span>";
                                if (o.isPay == false)
                                {
                                    status = "<span class=\"red-text font-weight-600\">Chưa thanh toán</span>";
                                }

                                html.Append("<article class=\"pane-primary\">");
                                if (orderType == 1)
                                {
                                    if (isPay == true)
                                    {
                                        html.Append("<div class=\"responsive-tb package-item\">");
                                        html.Append("<span class=\"owner\">Đơn hàng mua hộ #" + o.OrderID + "</span>");
                                        //html.Append("   <div class=\"heading\"><h3 class=\"lb\">Đơn hàng mua hộ: #" + o.OrderID + "</h3></div>");

                                    }
                                    else
                                    {
                                        html.Append("<div class=\"responsive-tb package-item\">");
                                        html.Append("<span class=\"owner\">Đơn hàng mua hộ #" + o.OrderID + "</span>");
                                        //html.Append("   <div class=\"heading\" style=\"background:red!important\"><h3 class=\"lb\">Đơn hàng mua hộ: #" + o.OrderID + "</h3></div>");
                                        listMainorder += o.OrderID + "|";
                                    }
                                }
                                else
                                {
                                    if (isPay == true)
                                    {
                                        html.Append("<div class=\"responsive-tb package-item\">");
                                        html.Append("<span class=\"owner\">Đơn hàng VC hộ #" + o.OrderID + "</span>");
                                        //html.Append("   <div class=\"heading\"><h3 class=\"lb\">Đơn hàng VC hộ: #" + o.OrderID + "</h3></div>");
                                    }
                                    else
                                    {
                                        html.Append("<div class=\"responsive-tb package-item\">");
                                        html.Append("<span class=\"owner\">Đơn hàng vc hộ #" + o.OrderID + "</span>");
                                        //html.Append("   <div class=\"heading\" style=\"background:red!important\"><h3 class=\"lb\">Đơn hàng VC hộ: #" + o.OrderID + "</h3></div>");
                                        listtransportationorder += o.OrderID + "|";
                                    }
                                }
                                html.Append("<table class=\"table bordered\">");
                                html.Append("<thead>");
                                html.Append("<tr class=\"teal darken-4\">");
                                html.Append("<th>Mã kiện</th>");
                                html.Append("<th>Cân nặng (kg)</th>");
                                html.Append("<th>Ngày lưu kho (Ngày)</th>");
                                html.Append("<th>Trạng thái</th>");
                                html.Append("<th>Tiền lưu kho</th>");
                                html.Append("</tr>");
                                html.Append("</thead>");
                                html.Append("<tbody>");
                                //html.Append("   <article class=\"pane-primary\">");
                                //html.Append("       <table class=\"normal-table full-width\">");
                                //html.Append("           <tr>");
                                //html.Append("               <th>Mã kiện</th>");
                                //html.Append("               <th>Cân nặng (kg)</th>");
                                //html.Append("               <th>Ngày lưu kho (ngày)</th>");
                                //html.Append("               <th>Trạng thái</th>");
                                //html.Append("               <th>Tiền lưu kho</th>");
                                //html.Append("           </tr>");
                                var listpackages = o.smallpackages;
                                foreach (var p in listpackages)
                                {
                                    html.Append("<tr>");
                                    html.Append("<td><span>" + p.packagecode + "</span></td>");
                                    html.Append("<td><span>" + p.weight + "</span></td>");
                                    html.Append("<td><span>" + p.DateInWare + "</span></td>");
                                    html.Append("<td>" + PJUtils.IntToStringStatusSmallPackageNew(p.Status) + "</td>");
                                    html.Append("<td>" + string.Format("{0:N0}", p.payInWarehouse) + " VND</td>");
                                    html.Append("</tr>");
                                    //html.Append("           <tr>");
                                    //html.Append("               <td>" + p.packagecode + "</td>");
                                    //html.Append("               <td>" + p.weight + "</td>");
                                    //html.Append("               <td>" + p.DateInWare + "</td>");
                                    //html.Append("               <td>" + PJUtils.IntToStringStatusSmallPackage(p.Status) + "</td>");
                                    //html.Append("               <td>" + string.Format("{0:N0}", p.payInWarehouse) + " vnđ</td>");
                                    //html.Append("           </tr>");
                                }
                                html.Append("<tr>");
                                html.Append("<td colspan=\"4\"><span class=\"black-text font-weight-500\">Tổng tiền lưu kho</span></td>");
                                html.Append("<td><span class=\"black-text font-weight-600\">" + string.Format("{0:N0}", o.totalPrice) + " VND</span></td>");
                                html.Append("</tr>");
                                html.Append("<tr>");
                                html.Append("<td colspan=\"4\"><span class=\"black-text font-weight-500\">Trạng thái</span></td>");
                                html.Append("<td>" + status + "</td>");
                                html.Append("</tr>");
                                html.Append("<tr>");
                                html.Append("<td colspan=\"4\"><span class=\"black-text font-weight-500\">Tiền cần thanh toán</span></td>");
                                if (o.isPay == false)
                                {
                                    html.Append("<td><span class=\"red-text font-weight-700\">" + string.Format("{0:N0}", o.totalMustPay) + " VND</span></td>");
                                }
                                else
                                {
                                    html.Append("<td><span class=\"green-text font-weight-700\">" + string.Format("{0:N0}", o.totalMustPay) + " VND</span></td>");
                                }
                                html.Append("</tr>");
                                html.Append("</tbody>");
                                html.Append("</table>");
                                html.Append("</div>");
                                //html.Append("           <tr style=\"font-size: 15px; text-transform: uppercase\">");
                                //html.Append("               <td colspan=\"4\" class=\"text-align-right\">Tổng tiền lưu kho</td>");
                                //html.Append("               <td>" + string.Format("{0:N0}", o.totalPrice) + " vnđ</td>");
                                //html.Append("           </tr>");
                                //html.Append("           <tr style=\"font-size: 15px; text-transform: uppercase\">");
                                //html.Append("               <td colspan=\"4\" class=\"text-align-right\">Trạng thái</td>");
                                //html.Append("               <td>" + status + "</td>");
                                //html.Append("           </tr>");
                                //html.Append("           <tr style=\"font-size: 18px; text-transform: uppercase\">");
                                //html.Append("               <td colspan=\"4\" class=\"text-align-right\">Tiền cần thanh toán</td>");
                                //html.Append("               <td>" + string.Format("{0:N0}", o.totalMustPay) + " vnđ</td>");
                                //html.Append("           </tr>");
                                //html.Append("       </table>");
                                //html.Append("   </article>");
                                //html.Append("</article>");
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
                                htmlPrint.Append("               <th style=\"color:#000\">Ngày lưu kho (ngày)</th>");
                                //htmlPrint.Append("               <th style=\"color:#000\">Thành tiền</th>");
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
                                    htmlPrint.Append("               <td>" + p.DateInWare + "</td>");
                                    htmlPrint.Append("               <td style=\"display:none\"><span>" + string.Format("{0:N0}", p.payInWarehouse) + " vnđ</span></td>");
                                    htmlPrint.Append("           </tr>");
                                }

                                var mo = MainOrderController.GetByID(o.OrderID);
                                htmlPrint.Append("           <tr style=\"font-size: 15px;display:none;\">");
                                htmlPrint.Append("               <td colspan=\"4\"><span style=\"font-weight: 500;\">Tổng tiền lưu kho</span></td>");
                                htmlPrint.Append("               <td><span style=\"font-weight: 500;\">" + string.Format("{0:N0}", o.totalPrice) + " vnđ</span></td>");
                                htmlPrint.Append("           </tr>");
                                htmlPrint.Append("           <tr style=\"font-size: 15px;\">");
                                htmlPrint.Append("               <td colspan=\"3\"><span style=\"font-weight: 500;\">Phí mua hàng</span></td>");
                                htmlPrint.Append("               <td><span style=\"font-weight: 500;\">" + string.Format("{0:N0}", Convert.ToDouble(mo.FeeBuyPro)) + " vnđ</span></td>");
                                htmlPrint.Append("           </tr>");
                                htmlPrint.Append("           <tr style=\"font-size: 15px;\">");
                                htmlPrint.Append("               <td colspan=\"3\"><span style=\"font-weight: 500;\">Phí vận chuyển</span></td>");
                                htmlPrint.Append("               <td><span style=\"font-weight: 500;\">" + string.Format("{0:N0}", Convert.ToDouble(mo.FeeWeight)) + " vnđ</span></td>");
                                htmlPrint.Append("           </tr>");
                                htmlPrint.Append("           <tr style=\"font-size: 15px; \">");
                                htmlPrint.Append("               <td colspan=\"3\"><span style=\"font-weight: 500;\">Phí ship Trung Quốc</span></td>");
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
                            htmlPrint.Append("Số dư của khách: <span style=\"color: #F64302;\">" + string.Format("{0:N0}", a.Wallet));
                            htmlPrint.Append(" VNĐ</span></p>");
                            htmlPrint.Append("</div>");
                            htmlPrint.Append("<div class=\"input-field col s12 l4\">");
                            htmlPrint.Append("<p style=\"font-size: 16px; font-weight: bold;\">");
                            htmlPrint.Append("Số tiền cần nạp thêm: <span style=\"color: #F64302;\">" + string.Format("{0:N0}", totalPriceMustPay - a.Wallet > 0 ? totalPriceMustPay - a.Wallet : 0));
                            htmlPrint.Append(" VNĐ</span></p>");
                            htmlPrint.Append("</div>");
                            htmlPrint.Append("</div>");
                            if (totalPriceMustPay > 0)
                            {
                                //OutStockSessionController.updateTotalPay(id, totalPriceMustPay);
                            }

                            var ot = OutStockSessionController.GetByID(id);
                            if (ot != null)
                                txtTotalPrice1.Text = string.Format("{0:N0}", ot.TotalPay);
                            else
                                txtTotalPrice1.Text = string.Format("{0:N0}", 0);

                            lrtListPackage.Text = html.ToString();
                           
                            //if (totalPriceMustPay > 0)
                            //{
                            //    btncreateuser.Visible = true;
                            //}
                            //else
                            //{
                            //    btncreateuser.Visible = false;
                            //}
                            ViewState["totalPricePay"] = totalPriceMustPay;
                            ViewState["listmID"] = listMainorder;
                            ViewState["listtrans"] = listtransportationorder;
                            ViewState["content"] = htmlPrint.ToString();



                            var c = ConfigurationController.GetByTop1();

                            string content = ViewState["content"].ToString();
                            var htmlout = "";
                            htmlout += "<div class=\"print-bill\">";
                            htmlout += "   <div class=\"top\">";
                            htmlout += "       <div class=\"left\">";
                            htmlout += "           <span class=\"company-info\">NHAPHANGTRUNG.VN</span>";
                            htmlout += "          <span class=\"company-info\">Địa chỉ: Quan Hoa - Cầu Giấy - Hà Nội</span>";
                            htmlout += "       </div>";
                            htmlout += "       <div class=\"right\">";
                            htmlout += "           <span class=\"bill-num\">Mẫu số 01 - TT</span>";
                            htmlout += "           <span class=\"bill-promulgate-date\">(Ban hành theo Thông tư số 133/2016/TT-BTC ngày 26/8/2016 của Bộ Tài chính)</span>";
                            htmlout += "       </div>";
                            htmlout += "   </div>";
                            htmlout += "   <div class=\"bill-title\">";
                            htmlout += "       <h1>PHIẾU XUẤT KHO</h1>";
                            htmlout += "       <span class=\"bill-date\">" + string.Format("{0:dd/MM/yyyy HH:mm}", currentDate) + " </span>";
                            htmlout += "   </div>";
                            htmlout += "   <div class=\"bill-content\">";
                            htmlout += "       <div class=\"bill-row\">";
                            htmlout += "           <label class=\"row-name\">Họ và tên người đến nhận: </label>";
                            htmlout += "           <label class=\"row-info\">" + txtFullname.Text + "</label>";
                            htmlout += "       </div>";
                            htmlout += "       <div class=\"bill-row\">";
                            htmlout += "           <label class=\"row-name\">Số ĐT người đến nhận: </label>";
                            htmlout += "           <label class=\"row-info\">" + txtPhone.Text + "</label>";
                            htmlout += "       </div>";
                            htmlout += "       <div class=\"bill-row\" style=\"border:none\">";
                            htmlout += "           <label class=\"row-name\">Danh sách kiện: </label>";
                            htmlout += "           <label class=\"row-info\"></label>";
                            htmlout += "       </div>";
                            htmlout += "       <div class=\"bill-row\" style=\"border:none\">";
                            htmlout += htmlPrint;
                            htmlout += "       </div>";
                            htmlout += "   </div>";
                            htmlout += "   <div class=\"bill-footer\">";
                            htmlout += "       <div class=\"bill-row-two\">";
                            htmlout += "           <strong>Người xuất hàng</strong>";
                            htmlout += "           <span class=\"note\">(Ký, họ tên)</span>";
                            htmlout += "       </div>";
                            htmlout += "       <div class=\"bill-row-two\">";
                            htmlout += "           <strong>Người nhận hàng</strong>";
                            htmlout += "           <span class=\"note\">(Ký, họ tên)</span>";
                            htmlout += "           <span class=\"note\" style=\"margin-top:100px;\">" + txtFullname.Text + "</span>";
                            htmlout += "       </div>";
                            htmlout += "   </div>";
                            htmlout += "</div>";








                            ltrContentPrint.Text = htmlout;
                            ltrBtnPrint.Text = "<a href=\"javascript:;\" style=\"margin-right:5px;text-transform: uppercase;font-weight: bold;background-color: #F64302 !important;\" class=\"btn\" onclick=\"printReceitp()\">In phiếu</a>";
                            //if (os.Status == 2)
                            //{
                            //    ltrBtnPrint.Text = "<a href=\"javascript:;\" style=\"margin-right:5px;\" class=\"btn\" onclick=\"printReceitp()\">In phiếu</a>";
                            //}




                        }
                        #endregion
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
            public double DateInWare { get; set; }
            public int Status { get; set; }
            public double payInWarehouse { get; set; }

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