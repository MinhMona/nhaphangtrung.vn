using NHST.Models;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net;
using Supremes;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NHST.Bussiness;
using MB.Extensions;
using Telerik.Web.UI;
using Microsoft.AspNet.SignalR;
using NHST.Hubs;
using System.Web.Script.Serialization;

namespace NHST.manager
{
    public partial class OrderDetail : System.Web.UI.Page
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
                    int RoleID = Convert.ToInt32(ac.RoleID);
                    if (ac.RoleID == 1)
                        Response.Redirect("/trang-chu");
                    else
                    {
                        if (RoleID == 4 || RoleID == 5 || RoleID == 8)
                        {
                            Response.Redirect("/manager/home.aspx");
                        }
                    }
                }
                //UpdatePrice();
                checkOrderStaff();
                LoadDDL();
                loaddata();
            }
        }

        protected void btnCurrency_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                DateTime currentDate = DateTime.Now;
                int ID = ViewState["MOID"].ToString().ToInt(0);
                if (ID > 0)
                {
                    var mo = MainOrderController.GetAllByID(ID);
                    if (mo != null)
                    {
                        if (mo.Status == 0)
                        {
                            var acc = AccountController.GetByID(Convert.ToInt32(mo.UID));

                            double TotalPriceVND = 0;
                            double Currency = Convert.ToDouble(tbCurrentCNYVN.Text);
                            double CurrencyCurrent = Convert.ToDouble(mo.CurrentCNYVN);
                            double InsurancePercent = Convert.ToDouble(mo.InsurancePercent);

                            if (Currency != InsurancePercent)
                            {
                                var listorder = OrderController.GetByMainOrderID(ID);
                                if (listorder != null)
                                {
                                    if (listorder.Count > 0)
                                    {
                                        double pricevnd = 0;
                                        double pricecyn = 0;

                                        foreach (var item in listorder)
                                        {
                                            double originprice = Math.Round(Convert.ToDouble(item.price_origin), 2);
                                            double promotionprice = Math.Round(Convert.ToDouble(item.price_promotion), 2);
                                            double oprice = 0;
                                            if (promotionprice > 0)
                                            {
                                                if (promotionprice < originprice)
                                                {
                                                    pricecyn += promotionprice;
                                                    oprice = promotionprice * Convert.ToDouble(item.quantity) * Currency;
                                                }
                                                else
                                                {
                                                    pricecyn += originprice;
                                                    oprice = originprice * Convert.ToDouble(item.quantity) * Currency;
                                                }
                                            }
                                            else
                                            {
                                                pricecyn += originprice;
                                                oprice = originprice * Convert.ToDouble(item.quantity) * Currency;
                                            }
                                            pricevnd += oprice;
                                        }

                                        pricevnd = Math.Round(pricevnd, 0);
                                        pricecyn = Math.Round(pricecyn, 2);

                                        double userbuypro = 0;
                                        double userdeposit = 0;
                                        double phantramdichvu = 0;
                                        double phantramcoc = 0;

                                        if (!string.IsNullOrEmpty(acc.Deposit.ToString()))
                                        {
                                            userdeposit = Convert.ToDouble(acc.Deposit);
                                        }
                                        if (!string.IsNullOrEmpty(acc.FeeBuyPro))
                                        {
                                            userbuypro = Convert.ToDouble(acc.FeeBuyPro);
                                        }

                                        if (userdeposit > 0)
                                        {
                                            if (userbuypro > 0)
                                            {
                                                phantramdichvu = userbuypro;
                                            }
                                            else if (pricevnd < 6000000)
                                            {
                                                phantramdichvu = 3;
                                            }
                                            else
                                            {
                                                phantramdichvu = 1.8;
                                            }
                                            phantramcoc = userdeposit;
                                        }
                                        if (mo.PercentDeposit == "100")
                                        {
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
                                            phantramcoc = 100;
                                        }
                                        else if (mo.PercentDeposit == "80")
                                        {
                                            if (userbuypro > 0)
                                            {
                                                phantramdichvu = userbuypro;
                                            }
                                            else if (pricevnd < 6000000)
                                            {
                                                phantramdichvu = 3;
                                            }
                                            else
                                            {
                                                phantramdichvu = 1.8;
                                            }
                                            phantramcoc = 80;
                                        }
                                        else if (mo.PercentDeposit == "90")
                                        {
                                            if (userbuypro > 0)
                                            {
                                                phantramdichvu = userbuypro;
                                            }
                                            else if (pricevnd < 6000000)
                                            {
                                                phantramdichvu = 3;
                                            }
                                            else
                                            {
                                                phantramdichvu = 1.8;
                                            }
                                            phantramcoc = 90;
                                        }
                                        else if (mo.PercentDeposit == "70")
                                        {
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
                                            phantramcoc = 70;
                                        }
                                        else
                                        {
                                            phantramdichvu = 2;
                                            phantramcoc = 100;
                                        }

                                        double feebp = Math.Round(pricevnd * phantramdichvu / 100, 0);
                                        if (feebp < 20000)
                                        {
                                            feebp = 20000;
                                        }

                                        double FeeBuyPro = feebp;
                                        double PriceVND = pricevnd;
                                        double PriceCNY = Math.Round(PriceVND / Currency, 2);

                                        double IsSpecial = 0;
                                        IsSpecial = Math.Round(Convert.ToDouble(mo.IsCheckPriceSpecial), 0);

                                        double IsPackedPrice = 0;
                                        if (mo.IsPacked == true)
                                            IsPackedPrice = Math.Round(Convert.ToDouble(mo.IsPackedPrice), 0);

                                        double TotalFeeSupport = 0;
                                        TotalFeeSupport = Math.Round(Convert.ToDouble(mo.TotalFeeSupport), 0);

                                        double FeeWeight = 0;
                                        FeeWeight = Math.Round(Convert.ToDouble(mo.FeeWeight), 0);

                                        double FeeShipCN = 0;
                                        double FeeShipCNCYN = Math.Round(Convert.ToDouble(mo.FeeShipCNCYN), 0);
                                        if (FeeShipCNCYN > 0)
                                            FeeShipCN = FeeShipCNCYN * Currency;

                                        double FeeShipCNReal = 0;
                                        double FeeShipCNRealCYN = Math.Round(Convert.ToDouble(mo.FeeShipCNCYN), 0);
                                        if (FeeShipCNRealCYN > 0)
                                            FeeShipCNReal = FeeShipCNRealCYN * Currency;

                                        double TotalPriceReal = 0;
                                        double TotalPriceRealCYN = Math.Round(Convert.ToDouble(mo.TotalPriceRealCYN), 0);
                                        if (TotalPriceRealCYN > 0)
                                            TotalPriceReal = TotalPriceRealCYN * Currency;

                                        double Deposit = 0;
                                        Deposit = Math.Round(Convert.ToDouble(mo.Deposit), 0);

                                        TotalPriceVND = PriceVND + FeeBuyPro + IsSpecial + IsPackedPrice + TotalFeeSupport + FeeWeight + FeeShipCN;
                                        TotalPriceVND = Math.Round(TotalPriceVND, 0);

                                        double AmountDeposit = Math.Round((PriceVND * phantramcoc) / 100, 0);

                                        if (Deposit > TotalPriceVND)
                                        {
                                            double drefund = Math.Round(Deposit - TotalPriceVND, 0);
                                            double userwallet = 0;
                                            if (acc.Wallet.ToString() != null)
                                                userwallet = Math.Round(Convert.ToDouble(acc.Wallet.ToString()), 0);
                                            double wallet = userwallet + drefund;
                                            wallet = Math.Round(wallet, 0);
                                            AccountController.updateWallet(acc.ID, wallet, currentDate, obj_user.Username);
                                            PayOrderHistoryController.Insert(mo.ID, obj_user.ID, 12, drefund, 2, currentDate, obj_user.Username);
                                            HistoryPayWalletController.Insert(acc.ID, acc.Username, mo.ID, drefund, "Thay đổi tỷ giá đơn hàng: " + mo.ID, wallet, 2, 2, currentDate, obj_user.Username);

                                            Deposit = TotalPriceVND;
                                        }

                                        MainOrderController.UpdateEditCurrency(mo.ID, Currency.ToString(), AmountDeposit.ToString(), Deposit.ToString(), FeeShipCN.ToString(),
                                        FeeShipCNReal.ToString(), FeeBuyPro.ToString(), PriceVND.ToString(), PriceCNY.ToString(), TotalPriceVND.ToString(), TotalPriceReal.ToString());

                                        HistoryOrderChangeController.Insert(mo.ID, obj_user.ID, obj_user.Username, obj_user.Username + " Đã thay đổi tỉ giá của đơn hàng ID là: " + mo.ID + "," +
                                        " từ: " + string.Format("{0:#.##}", CurrencyCurrent) + "," + " sang: " + string.Format("{0:#.##}", Currency) + "", 7, currentDate);

                                        Response.Redirect("/manager/OrderDetail.aspx?id=" + mo.ID);
                                    }
                                }
                            }
                        }                            
                    }
                }
            }
        }

        public void UpdatePrice()
        {
            bool checkMVD = false;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int uid = obj_user.ID;

                int RoleID = obj_user.RoleID.ToString().ToInt();
                //var id = Convert.ToInt32(Request.QueryString["id"]);
                var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {
                        int uidmuahang = Convert.ToInt32(o.UID);
                        string usermuahang = "";

                        var accmuahan = AccountController.GetByID(uidmuahang);
                        if (accmuahan != null)
                        {
                            usermuahang = accmuahan.Username;
                        }
                        string CurrentOrderTransactionCode = o.OrderTransactionCode;
                        string CurrentOrderTransactionCode2 = o.OrderTransactionCode2;
                        string CurrentOrderTransactionCode3 = o.OrderTransactionCode3;
                        string CurrentOrderTransactionCode4 = o.OrderTransactionCode4;
                        string CurrentOrderTransactionCode5 = o.OrderTransactionCode5;

                        string CurrentOrderTransactionCodeWeight = o.OrderTransactionCodeWeight;
                        string CurrentOrderTransactionCodeWeight2 = o.OrderTransactionCodeWeight2;
                        string CurrentOrderTransactionCodeWeight3 = o.OrderTransactionCodeWeight3;
                        string CurrentOrderTransactionCodeWeight4 = o.OrderTransactionCodeWeight4;
                        string CurrentOrderTransactionCodeWeight5 = o.OrderTransactionCodeWeight5;

                        string CurrentOrderWeight = o.OrderWeight;

                        bool ischeckmvd = true;
                        string listmvd_ne = "";

                        #region cập nhật và tạo mới smallpackage
                        string tcl = hdfCodeTransactionList.Value;
                        string listmvd = hdfCodeTransactionListMVD.Value;
                        if (!string.IsNullOrEmpty(tcl))
                        {
                            checkMVD = true;
                            string[] list = tcl.Split('|');
                            for (int i = 0; i < list.Length - 1; i++)
                            {
                                string[] item = list[i].Split(',');
                                int ID = item[0].ToInt(0);
                                string code = item[1].Trim();
                                string weight = item[2];
                                double weightin = 0;
                                if (!string.IsNullOrEmpty(weight))
                                    weightin = Math.Round(Convert.ToDouble(weight), 1);
                                int smallpackage_status = item[3].ToInt(1);
                                string description = item[4];
                                string mainOrderCodeID = item[5];
                                var MainOrderCode = MainOrderCodeController.GetByID(mainOrderCodeID.ToInt(0));
                                if (MainOrderCode == null)
                                    PJUtils.ShowMessageBoxSwAlert("Lỗi, không có mã đơn hàng", "e", false, Page);
                                if (ID > 0)
                                {
                                    var smp = SmallPackageController.GetByID(ID);
                                    if (smp != null)
                                    {
                                        int bigpackageID = Convert.ToInt32(smp.BigPackageID);
                                        bool check = false;
                                        var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                        if (getsmallcheck.Count > 0)
                                        {
                                            foreach (var sp in getsmallcheck)
                                            {
                                                if (sp.ID == ID)
                                                {
                                                    check = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            check = true;
                                        }
                                        if (check)
                                        {
                                            SmallPackageController.UpdateNew(ID, accmuahan.ID, usermuahang, bigpackageID, code,
                                                smp.ProductType, Math.Round(Convert.ToDouble(smp.FeeShip), 0),
                                            weightin, Math.Round(Convert.ToDouble(smp.Volume), 1), smallpackage_status,
                                            description, currentDate, username, mainOrderCodeID.ToInt(0));

                                            if (smallpackage_status == 2)
                                            {
                                                SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                            }
                                            else if (smallpackage_status == 3)
                                            {
                                                SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                            }
                                            var bigpack = BigPackageController.GetByID(bigpackageID);
                                            if (bigpack != null)
                                            {
                                                int TotalPackageWaiting = SmallPackageController.GetCountByBigPackageIDStatus(bigpackageID, 1, 2);
                                                if (TotalPackageWaiting == 0)
                                                {
                                                    BigPackageController.UpdateStatus(bigpackageID, 2, currentDate, username);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                        //if (getsmallcheck.Count > 0)
                                        //{
                                        //    PJUtils.ShowMessageBoxSwAlert("Mã kiện đã tồn tại, vui lòng tạo mã khác", "e", true, Page);
                                        //}
                                        //else
                                        //{

                                        var checkbarcode = SmallPackageController.GetByOrderTransactionCode(code);
                                        if (checkbarcode == null)
                                        {
                                            SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(id, accmuahan.ID, usermuahang,
                                            0, code, "", 0, weightin, 0,
                                        smallpackage_status, description, currentDate, username, mainOrderCodeID.ToInt(0), 0);


                                            var quantitymvd1 = SmallPackageController.GetByMainOrderID(id);
                                            if (quantitymvd1.Count > 0)
                                            {
                                                if (quantitymvd1 != null)
                                                {
                                                    MainOrderController.UpdateListMVD(id, listmvd, quantitymvd1.Count);
                                                }
                                            }




                                            //       HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            //" đã thêm mã vận đơn của đơn hàng ID là: " + o.ID + ", Mã vận đơn: " + code + ", cân nặng: " + weightin + "", 8, currentDate);

                                            if (smallpackage_status == 2)
                                            {
                                                SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                            }
                                            else if (smallpackage_status == 3)
                                            {
                                                SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                            }
                                            //}
                                        }
                                        else
                                        {
                                            ischeckmvd = false;
                                            listmvd_ne += code;
                                        }


                                    }
                                }
                                else
                                {
                                    //bool check = false;
                                    //var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                    //if (getsmallcheck.Count > 0)
                                    //{
                                    //    PJUtils.ShowMessageBoxSwAlert("Mã kiện đã tồn tại, vui lòng tạo mã khác", "e", true, Page);
                                    //}
                                    //else
                                    //{
                                    var checkbarcode = SmallPackageController.GetByOrderTransactionCode(code);
                                    if (checkbarcode == null)
                                    {
                                        SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(id, accmuahan.ID, usermuahang, 0,
                                        code, "", 0, weightin, 0,
                                    smallpackage_status, description, currentDate, username, mainOrderCodeID.ToInt(0), 0);


                                        var quantitymvd2 = SmallPackageController.GetByMainOrderID(id);
                                        if (quantitymvd2.Count > 0)
                                        {
                                            if (quantitymvd2 != null)
                                            {
                                                MainOrderController.UpdateListMVD(id, listmvd, quantitymvd2.Count);
                                            }
                                        }


                                        if (smallpackage_status == 2)
                                        {
                                            SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                        }
                                        else if (smallpackage_status == 3)
                                        {
                                            SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                        }
                                        //}
                                    }
                                    else
                                    {
                                        ischeckmvd = false;
                                        listmvd_ne += code;
                                    }

                                }
                            }
                        }
                        #endregion



                        if (ischeckmvd)
                        {
                            double TotalFeeSupport = 0;
                            TotalFeeSupport = Convert.ToDouble(o.TotalFeeSupport);

                            #region Lấy ra text của trạng thái đơn hàng
                            string orderstatus = "";
                            int currentOrderStatus = Convert.ToInt32(o.Status);
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
                            #endregion
                            #region Cập nhật nhân viên KhoTQ và nhân viên KhoVN
                            if (RoleID == 4)
                            {
                                if (o.KhoTQID == uid || o.KhoTQID == 0)
                                {
                                    MainOrderController.UpdateStaff(o.ID, o.SalerID.ToString().ToInt(0), o.DathangID.ToString().ToInt(0), uid, o.KhoVNID.ToString().ToInt(0));
                                }
                                else
                                {
                                    //PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý", "e", true, Page);
                                }
                            }
                            else if (RoleID == 5)
                            {
                                if (o.KhoVNID == uid || o.KhoTQID == 0)
                                {
                                    MainOrderController.UpdateStaff(o.ID, o.SalerID.ToString().ToInt(0), o.DathangID.ToString().ToInt(0), o.KhoTQID.ToString().ToInt(0), uid);
                                }
                                else
                                {
                                    //PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý", "e", true, Page);
                                }
                            }
                            #endregion
                            #region cập nhật thông tin của đơn hàng
                            double feeeinwarehouse = 0;
                            int status = ddlStatus.SelectedValue.ToString().ToInt(0);
                            if (status == 1)
                            {
                                if (RoleID == 0 || RoleID == 2 || RoleID == 9)
                                {

                                    MainOrderController.UpdateStatusByID(o.ID, 1);
                                    double Deposit = 0;
                                    if (o.Deposit.ToFloat(0) > 0)
                                        Deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);
                                    if (Deposit > 0)
                                    {
                                        var user_order = AccountController.GetByID(o.UID.ToString().ToInt());
                                        if (user_order != null)
                                        {
                                            double wallet = 0;
                                            if (user_order.Wallet.ToString().ToFloat(0) > 0)
                                                wallet = Math.Round(Convert.ToDouble(user_order.Wallet), 0);
                                            wallet = wallet + Deposit;
                                            //HistoryPayWalletController.Insert(user_order.ID, user_order.Username, o.ID, Deposit,
                                            //    "Đơn hàng: " + o.ID + " bị hủy và hoàn tiền cọc cho khách.", wallet, 2, 2, currentDate, obj_user.Username);
                                            //HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            //    " đã đổi trạng thái của đơn hàng: " + o.ID + " từ " + orderstatus + " sang " + ddlStatus.SelectedItem + "", 0, currentDate);
                                            //AccountController.updateWallet(user_order.ID, wallet, currentDate, obj_user.Username);
                                            //MainOrderController.UpdateDeposit(o.ID, "0");
                                            //PayOrderHistoryController.Insert(o.ID, user_order.ID, 4, Deposit, 2, currentDate, obj_user.Username);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                double OCurrent_deposit = 0;
                                if (o.Deposit.ToFloat(0) > 0)
                                    OCurrent_deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);

                                double OCurrent_FeeShipCN = 0;
                                if (o.FeeShipCN.ToFloat(0) > 0)
                                    OCurrent_FeeShipCN = Math.Round(Convert.ToDouble(o.FeeShipCN), 2);

                                double OCurrent_FeeBuyPro = 0;
                                if (o.FeeBuyPro.ToFloat(0) > 0)
                                    OCurrent_FeeBuyPro = Math.Round(Convert.ToDouble(o.FeeBuyPro), 0);

                                double OCurrent_FeeWeight = 0;
                                if (o.FeeWeight.ToFloat(0) > 0)
                                    OCurrent_FeeWeight = Math.Round(Convert.ToDouble(o.FeeWeight), 0);

                                double OCurrent_IsCheckProductPrice = 0;
                                if (o.IsCheckProductPrice.ToFloat(0) > 0)
                                    OCurrent_IsCheckProductPrice = Math.Round(Convert.ToDouble(o.IsCheckProductPrice), 0);

                                double OCurrent_IsPackedPrice = 0;
                                if (o.IsPackedPrice.ToFloat(0) > 0)
                                    OCurrent_IsPackedPrice = Math.Round(Convert.ToDouble(o.IsPackedPrice), 0);

                                double OCurrent_IsSpecial = 0;
                                if (o.IsCheckPriceSpecial.ToFloat(0) > 0)
                                    OCurrent_IsSpecial = Math.Round(Convert.ToDouble(o.IsCheckPriceSpecial), 0);

                                double OCurrent_IsFastDeliveryPrice = 0;
                                if (o.IsFastDeliveryPrice.ToFloat(0) > 0)
                                    OCurrent_IsFastDeliveryPrice = Math.Round(Convert.ToDouble(o.IsFastDeliveryPrice), 0);

                                double OCurrent_TotalPriceReal = 0;
                                if (o.TotalPriceReal.ToFloat(0) > 0)
                                    OCurrent_TotalPriceReal = Math.Round(Convert.ToDouble(o.TotalPriceReal), 0);

                                double OCurrent_TotalPriceRealCYN = 0;
                                if (o.TotalPriceRealCYN.ToFloat(0) > 0)
                                    OCurrent_TotalPriceRealCYN = Math.Round(Convert.ToDouble(o.TotalPriceRealCYN), 2);

                                //double OCurrent_FeeShipCNToVN = Convert.ToDouble(o.FeeShipCNToVN);

                                double Deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);
                                double FeeShipCN = Math.Round(Convert.ToDouble(o.FeeShipCN), 0);
                                double FeeShipCNCYN = Math.Round(Convert.ToDouble(o.FeeShipCNCYN), 2);
                                double FeeShipCNReal = Math.Round(Convert.ToDouble(o.FeeShipCNReal), 0);
                                double FeeShipCNRealCYN = Math.Round(Convert.ToDouble(o.FeeShipCNRealCYN), 2);
                                double FeeBuyPro = Math.Round(Convert.ToDouble(o.FeeBuyPro), 0);
                                double FeeWeight = Math.Round(Convert.ToDouble(o.FeeWeight), 0);
                                double TotalPriceReal = Math.Round(Convert.ToDouble(o.TotalPriceReal), 0);
                                double TotalPriceRealCYN = Math.Round(Convert.ToDouble(o.TotalPriceRealCYN), 2);

                                if (o.FeeInWareHouse != null)
                                    feeeinwarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);
                                //double FeeShipCNToVN = Convert.ToDouble(pWeight.Value);

                                double IsCheckProductPrice = 0;
                                if (Convert.ToDouble(o.IsCheckProductPrice) > 0)
                                    IsCheckProductPrice = Math.Round(Convert.ToDouble(o.IsCheckProductPrice), 0);
                                double IsCheckProductPriceCYN = 0;
                                if (Convert.ToDouble(o.IsCheckProductPriceCYN) > 0)
                                    IsCheckProductPriceCYN = Math.Round(Convert.ToDouble(o.IsCheckProductPriceCYN), 0);
                                //if (o.IsCheckProduct == true)
                                //    IsCheckProductPrice = Convert.ToDouble(pCheck.Value);
                                //else
                                //    IsCheckProductPrice = Convert.ToDouble(o.IsCheckProductPrice);

                                double IsPackedPrice = 0;
                                if (Convert.ToDouble(o.IsPackedPrice) > 0)
                                    IsPackedPrice = Math.Round(Convert.ToDouble(o.IsPackedPrice), 0);

                                double IsPriceSepcial = 0;
                                if (Convert.ToDouble(o.IsCheckPriceSpecial) > 0)
                                    IsPriceSepcial = Math.Round(Convert.ToDouble(o.IsCheckPriceSpecial), 0);




                                double IsPackedPriceCYN = 0;
                                if (Convert.ToDouble(o.IsPackedPriceCYN) > 0)
                                    IsPackedPriceCYN = Math.Round(Convert.ToDouble(o.IsPackedPriceCYN), 0);
                                //if (o.IsPacked == true)
                                //    IsPackedPrice = Convert.ToDouble(pPacked.Value);
                                //else
                                //    IsPackedPrice = Convert.ToDouble(o.IsPackedPrice);

                                double IsFastDeliveryPrice = 0;
                                if (Convert.ToDouble(o.IsFastDeliveryPrice) > 0)
                                    IsFastDeliveryPrice = Math.Round(Convert.ToDouble(o.IsFastDeliveryPrice), 0);

                                //if (o.IsFastDelivery == true)
                                //    IsFastDeliveryPrice = Convert.ToDouble(pShipHome.Value);
                                //else
                                //    IsFastDeliveryPrice = Convert.ToDouble(o.IsFastDeliveryPrice);


                                #region Ghi lịch sử chỉnh sửa các loại giá
                                if (OCurrent_deposit != Deposit)
                                {

                                }
                                if (OCurrent_FeeShipCN != FeeShipCN)
                                {

                                }
                                if (OCurrent_FeeBuyPro < FeeBuyPro || OCurrent_FeeBuyPro > FeeBuyPro)
                                {

                                }
                                if (OCurrent_TotalPriceReal < TotalPriceReal || OCurrent_TotalPriceReal > TotalPriceReal)
                                {

                                }
                                if (OCurrent_FeeWeight != FeeWeight)
                                {

                                }
                                if (OCurrent_IsCheckProductPrice != IsCheckProductPrice)
                                {

                                }
                                if (OCurrent_IsPackedPrice != IsPackedPrice)
                                {

                                }
                                if (OCurrent_IsFastDeliveryPrice != IsFastDeliveryPrice)
                                {

                                }

                                #endregion









                                double isfastprice = 0;
                                if (o.IsFastPrice.ToFloat(0) > 0)
                                    isfastprice = Math.Round(Convert.ToDouble(o.IsFastPrice), 0);

                                double pricenvd = 0;
                                if (o.PriceVND.ToFloat(0) > 0)
                                    pricenvd = Math.Round(Convert.ToDouble(o.PriceVND), 0);

                                var conf = ConfigurationController.GetByTop1();
                                double cannangdonggo = 0;
                                double cannangdacbiet = 0;
                                if (!string.IsNullOrEmpty(o.TongCanNang))
                                {
                                    cannangdonggo = Convert.ToDouble(o.TongCanNang);
                                    cannangdacbiet = Convert.ToDouble(o.TongCanNang);

                                }
                                if (o.IsPacked == true)
                                {
                                    if (cannangdonggo > 0)
                                    {
                                        if (cannangdonggo <= 1)
                                        {
                                            IsPackedPrice = Convert.ToDouble(conf.FeeDongGoKgDau);
                                        }
                                        else
                                        {
                                            cannangdonggo = cannangdonggo - 1;
                                            IsPackedPrice = Convert.ToDouble(conf.FeeDongGoKgDau) + (cannangdonggo * Convert.ToDouble(conf.FeeDongGoKgSau));
                                        }
                                    }
                                }


                                if (o.IsCheckSpecial1 == true)
                                {
                                    if (cannangdacbiet > 0)
                                    {
                                        IsPriceSepcial = (cannangdacbiet * Convert.ToDouble(conf.FeeDacBiet1));
                                    }
                                }
                                if (o.IsCheckSpecial2 == true)
                                {
                                    if (cannangdacbiet > 0)
                                    {
                                        IsPriceSepcial = (cannangdacbiet * Convert.ToDouble(conf.FeeDacBiet2));
                                    }
                                }
                                if (o.IsCheckSpecial1 == true && o.IsCheckSpecial2 == true)
                                {
                                    if (cannangdacbiet > 0)
                                    {
                                        IsPriceSepcial = (cannangdacbiet * (Convert.ToDouble(conf.FeeDacBiet1) + Convert.ToDouble(conf.FeeDacBiet2)));
                                    }
                                }

                                IsPackedPrice = Math.Round(IsPackedPrice, 0);
                                IsPriceSepcial = Math.Round(IsPriceSepcial, 0);
                                double TotalPriceVND = FeeShipCN + FeeBuyPro + FeeWeight + IsCheckProductPrice + IsPackedPrice
                                                    + IsFastDeliveryPrice + isfastprice + pricenvd + TotalFeeSupport + IsPriceSepcial;
                                TotalPriceVND = Math.Round(TotalPriceVND, 0);
                               
                                MainOrderController.UpdateFee_OrderDetail(o.ID, Deposit.ToString(), FeeShipCN.ToString(), FeeBuyPro.ToString(), FeeWeight.ToString(), IsCheckProductPrice.ToString(),
                                                            IsPackedPrice.ToString(), IsFastDeliveryPrice.ToString(), TotalPriceVND.ToString(), FeeShipCNReal.ToString(), IsPriceSepcial.ToString());
                                MainOrderController.UpdateCYN(o.ID, FeeShipCNRealCYN.ToString(), FeeShipCNCYN.ToString(), IsCheckProductPriceCYN.ToString(), IsPackedPriceCYN.ToString());                                
                            }

                            string CurrentShippingType = o.ShippingType.ToString();
                            string ShippingType = ddlShippingType.SelectedValue;
                            string CurrentNameLine = "";
                            int line = Convert.ToInt32(o.ShippingType);
                            if (line > 0)
                            {
                                var shipping = ShippingTypeToWareHouseController.GetByID(line);
                                if (shipping != null)
                                {
                                    CurrentNameLine = shipping.ShippingTypeName;
                                }    
                            }    
                            
                            if (CurrentShippingType != ShippingType)
                            {
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username + " đã thay đổi Line từ: " + CurrentNameLine + ", sang: " + ShippingType + "", 8, currentDate);
                            }    

                            //string CurrentReceivePlace = o.ReceivePlace;
                            //string ReceivePlace = ddlReceivePlace.SelectedValue;

                            //if (CurrentReceivePlace != ReceivePlace)
                            //{

                            //}

                            string CurrentAmountDeposit = o.AmountDeposit.Trim();
                            CurrentAmountDeposit = Math.Round(Convert.ToDouble(CurrentAmountDeposit), 0).ToString();

                            string AmountDeposit = o.AmountDeposit;
                            AmountDeposit = Math.Round(Convert.ToDouble(AmountDeposit), 0).ToString();                            

                            bool Currentcheckpro = new bool();
                            bool CurrentOrderDone = new bool();
                            bool CurrentOrderPrice = new bool();
                            bool checkpro = new bool();
                            bool Package = new bool();
                            bool MoveIsFastDelivery = new bool();
                            bool baogia = new bool();
                            bool smallPackage = new bool();
                            bool ycg = new bool();
                            bool baohiem = new bool();
                            bool orderdone = new bool();
                            bool orderPrice = new bool();
                            bool special1 = new bool();
                            bool special2 = new bool();
                            var listCheck = hdfListCheckBox.Value.Split('|').ToList();
                            foreach (var item in listCheck)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    var ck = item.Split(',').ToList();

                                    if (ck != null)
                                    {
                                        if (ck[0] == "1")
                                        {
                                            smallPackage = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "2")
                                        {
                                            baogia = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "3")
                                        {
                                            checkpro = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "4")
                                        {
                                            Package = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "5")
                                        {
                                            MoveIsFastDelivery = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "6")
                                        {
                                            ycg = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "7")
                                        {
                                            baohiem = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "8")
                                        {
                                            orderdone = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "9")
                                        {
                                            orderPrice = Convert.ToBoolean(ck[1].ToInt(0));
                                        }

                                    }
                                }
                            }

                            if (Currentcheckpro != checkpro)
                            {
                                //HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                //           " đã đổi dịch vụ kiểm tra đơn hàng của đơn hàng ID là: " + o.ID + ", từ: " + ConvertBoolHistory(Currentcheckpro, "kiểm tra đơn hàng") + ", sang: " + ConvertBoolHistory(checkpro, "kiểm tra đơn hàng") + "",
                                //           8, currentDate);
                            }
                            bool CurrentPackage = o.IsPacked.ToString().ToBool();

                            if (CurrentPackage != Package)
                            {
                                //HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                //           " đã đổi dịch vụ đóng gỗ của đơn hàng ID là: " + o.ID + ", từ: " + ConvertBoolHistory(CurrentPackage, "đóng gỗ") + ", sang: " + ConvertBoolHistory(Package, "đóng gỗ") + "",
                                //           8, currentDate);
                            }
                            bool CurrentIsFastDelivery = o.IsFastDelivery.ToString().ToBool();



                            //if (CurrentOrderDone != orderdone)
                            //{
                            //    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                            //               " đã đổi ĐH đã xử lý xong của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderDone + ", sang: " + orderdone + "",
                            //               8, currentDate);
                            //}
                            //if (CurrentOrderPrice != orderPrice)
                            //{
                            //    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                            //               " đã đổi ĐH đã xử lý xong của đơn hàng ID là: " + o.ID + ", từ: " + CurrentOrderPrice + ", sang: " + orderPrice + "",
                            //               8, currentDate);
                            //}


                            //string TotalPriceReal1 = rTotalPriceReal.Text.ToString();
                            //TotalPriceReal1 = Math.Round(Convert.ToDouble(TotalPriceReal1), 0).ToString();
                            //string TotalPriceRealCYN1 = rTotalPriceRealCYN.Text.ToString();
                            //TotalPriceRealCYN1 = Math.Round(Convert.ToDouble(TotalPriceRealCYN1), 2).ToString();
                            //if (CurrentIsFastDelivery != MoveIsFastDelivery)
                            //{
                            //    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                            //               " đã đổi dịch vụ giao hàng tận nhà của đơn hàng ID là: " + o.ID + ", từ: " + ConvertBoolHistory(CurrentIsFastDelivery, "giao hàng tận nhà") + ", sang: " + ConvertBoolHistory(MoveIsFastDelivery, "giao hàng tận nhà") + "",
                            //               8, currentDate);
                            //}

                            Package = Convert.ToBoolean(o.IsPacked);

                            special1 = Convert.ToBoolean(o.IsCheckSpecial1);
                            special2 = Convert.ToBoolean(o.IsCheckSpecial2);

                            MainOrderController.UpdateTotalFeeSupport(o.ID, TotalFeeSupport.ToString());
                            // MainOrderController.UpdateOrderWeight(o.ID, OrderWeight);
                            // MainOrderController.UpdateCheckPro(o.ID, checkpro);
                            //MainOrderController.UpdateBaogia(o.ID, baogia);
                            //MainOrderController.UpdateIsGiaohang(o.ID, ycg);
                            //MainOrderController.UpdateOrderDone(o.ID, orderdone);
                            //MainOrderController.UpdateOrderPrice(o.ID, orderPrice);
                            MainOrderController.UpdateIsPacked(o.ID, Package);
                            MainOrderController.UpdateIsSpecial(o.ID, special1, special2);
                            //MainOrderController.UpdateIsFastDelivery(o.ID, MoveIsFastDelivery);
                            MainOrderController.UpdateAmountDeposit(o.ID, AmountDeposit);
                            MainOrderController.UpdateFeeWarehouse(o.ID, feeeinwarehouse);
                            //MainOrderController.UpdateReceivePlace(o.ID, Convert.ToInt32(o.UID), ddlReceivePlace.SelectedValue.ToString());
                            double FeeweightPriceDiscount = 0;
                            if (!string.IsNullOrEmpty(hdfFeeweightPriceDiscount.Value))
                            {
                                FeeweightPriceDiscount = Math.Round(Convert.ToDouble(hdfFeeweightPriceDiscount.Value));
                            }
                            MainOrderController.UpdateFeeWeightDC(o.ID, FeeweightPriceDiscount.ToString());
                            //MainOrderController.UpdateStatusByID(o.ID, Convert.ToInt32(ddlStatus.SelectedValue));
                            MainOrderController.UpdateOrderWeightCK(o.ID, FeeweightPriceDiscount.ToString());
                            // MainOrderController.UpdateTQVNWeight(o.ID, o.UID.ToString().ToInt(), Math.Round(Convert.ToDouble(pWeightNDT.Text.ToString()), 2).ToString());
                            //MainOrderController.UpdateTotalPriceReal(o.ID, TotalPriceReal1.ToString(), TotalPriceRealCYN1.ToString());
                            //MainOrderController.UpdateMainOrderCode(o.ID, o.UID.ToString().ToInt(), txtMainOrderCode.Text);
                            //MainOrderController.UpdateFTS(o.ID, o.UID.ToString().ToInt(), ddlWarehouseFrom.SelectedValue.ToInt(),
                            //    ddlReceivePlace.SelectedValue, ddlShippingType.SelectedValue.ToInt());

                            //MainOrderController.UpdateDoneSmallPackage(o.ID, smallPackage);

                            MainOrderController.UpdateIsInsurrance(o.ID, baohiem);

                            #region update liên quan đến status
                            int currentstt = Convert.ToInt32(o.Status);
                            var imo = MainOrderController.GetByID(o.ID);
                            if (currentstt < 3 || currentstt > 7)
                            {
                                if (imo.Status != currentstt)
                                {

                                }
                            }
                            else if (currentstt > 2 && currentstt < 8)
                            {
                                if (imo.Status < 3 || imo.Status > 7)
                                {

                                }
                            }
                            #region Ghi lịch sử update status của đơn hàng
                            if (imo.Status != currentstt)
                            {
                                string ustatus = "";
                                switch (imo.Status.Value)
                                {
                                    case 0:
                                        ustatus = "Đơn mới";
                                        break;
                                    case 1:
                                        ustatus = "Đơn hàng hủy";
                                        break;
                                    case 2:
                                        ustatus = "Đơn đã cọc";
                                        break;
                                    case 3:
                                        ustatus = "Đơn người bán giao";
                                        break;
                                    case 4:
                                        ustatus = "Đơn chờ mua hàng";
                                        break;
                                    case 5:
                                        ustatus = "Đơn đã mua hàng";
                                        break;
                                    case 6:
                                        ustatus = "Kho Trung Quốc nhận hàng";
                                        break;
                                    case 7:
                                        ustatus = "Trên đường về Việt Nam";
                                        break;
                                    case 8:
                                        ustatus = "Trong kho Hà Nội";
                                        break;
                                    case 9:
                                        ustatus = "Đã thanh toán";
                                        break;
                                    case 10:
                                        ustatus = "Đã hoàn thành";
                                        break;
                                    case 11:
                                        ustatus = "Đang giao hàng";
                                        break;
                                    case 12:
                                        ustatus = "Đơn khiếu nại";
                                        break;
                                    default:
                                        break;
                                }

                            }
                            #endregion
                            if (imo.Status == 5 && imo.Status != currentstt)
                            {
                                var setNoti = SendNotiEmailController.GetByID(7);
                                if (setNoti != null)
                                {

                                }
                            }
                            #endregion


                            //if (o.CurrentCNYVN == null || o.CurrentCNYVN != tbCurrentCNYVN.Text.Trim())
                            //{
                            //    MainOrderController.UpdateCurrentCNYVN(o.ID, tbCurrentCNYVN.Text.Trim());
                            //}

                            if (baohiem == false)
                            {
                                MainOrderController.UpdateInsurranceMoney(o.ID, "0", o.InsurancePercent);
                            }
                            else
                            {
                                double InsurranceMoney = Convert.ToDouble(o.PriceVND) * (Convert.ToDouble(o.InsurancePercent) / 100);
                                MainOrderController.UpdateInsurranceMoney(o.ID, InsurranceMoney.ToString(), o.InsurancePercent);
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        public void checkOrderStaff()
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int RoleID = obj_user.RoleID.ToString().ToInt();
                int UID = obj_user.ID;
                var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {
                        int status_order = Convert.ToInt32(o.Status);
                        if (RoleID == 0 || RoleID == 2)
                        {

                        }
                        else if (RoleID == 4)
                        {
                            if (status_order >= 5 && status_order < 7)
                            {
                                //Role kho TQ
                                if (o.KhoTQID == UID || o.KhoTQID == 0)
                                {

                                }
                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }

                        }
                        else if (RoleID == 5)
                        {
                            if (status_order >= 5 && status_order <= 7)
                            {
                                //Role Kho VN
                                if (o.KhoVNID == UID || o.KhoVNID == 0)
                                {

                                }
                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }

                        }
                        else if (RoleID == 6)
                        {
                            if (status_order != 1)
                            {
                                if (o.SalerID == UID)
                                {

                                }
                                else
                                {
                                    Response.Redirect("/manager/OrderList.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }
                        }
                        else if (RoleID == 7)
                        {
                            if (status_order >= 2)
                            {

                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }
                        }
                        else if (RoleID == 8)
                        {
                            if (status_order >= 9 && status_order < 10)
                            {

                            }
                            else
                            {
                                Response.Redirect("/manager/OrderList.aspx");
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("/manager/OrderList.aspx");
                }
            }
        }

        public void LoadDDL()
        {
            ddlSaler.Items.Clear();
            ddlSaler.Items.Insert(0, "Chọn Saler");

            ddlDatHang.Items.Clear();
            ddlDatHang.Items.Insert(0, "Chọn nhân viên đặt hàng");

            ddlCSKH.Items.Clear();
            ddlCSKH.Items.Insert(0, "Chọn nhân viên chăm sóc khách hàng");

            ddlKhoTQ.Items.Clear();
            ddlKhoTQ.Items.Insert(0, "Chọn nhân viên kho TQ");

            ddlKhoVN.Items.Clear();
            ddlKhoVN.Items.Insert(0, "Chọn nhân viên kho đích");

            var salers = AccountController.GetAllByRoleID(6);
            if (salers.Count > 0)
            {
                ddlSaler.DataSource = salers;
                ddlSaler.DataBind();
            }

            var dathangs = AccountController.GetAllByRoleID(3);
            if (dathangs.Count > 0)
            {
                ddlDatHang.DataSource = dathangs;
                ddlDatHang.DataBind();
            }

            var cskh = AccountController.GetAllByRoleIDAndRoleID(2, 9);
            if (cskh.Count > 0)
            {
                ddlCSKH.DataSource = cskh;
                ddlCSKH.DataBind();
            }


            var khotqs = AccountController.GetAllByRoleID(4);
            if (khotqs.Count > 0)
            {
                ddlKhoTQ.DataSource = khotqs;
                ddlKhoTQ.DataBind();
            }

            var khovns = AccountController.GetAllByRoleID(5);
            if (khovns.Count > 0)
            {
                ddlKhoVN.DataSource = khovns;
                ddlKhoVN.DataBind();
            }
            var warehousefrom = WarehouseFromController.GetAllWithIsHidden(false);
            if (warehousefrom.Count > 0)
            {
                ddlWarehouseFrom.DataSource = warehousefrom;
                ddlWarehouseFrom.DataBind();
            }


            var warehouse = WarehouseController.GetAllWithIsHidden(false);
            if (warehouse.Count > 0)
            {
                ddlReceivePlace.DataSource = warehouse;
                ddlReceivePlace.DataBind();
            }

            var shippingtype = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shippingtype.Count > 0)
            {
                ddlShippingType.DataSource = shippingtype;
                ddlShippingType.DataBind();
            }
        }

        public void loaddata()
        {

            var config = ConfigurationController.GetByTop1();
            double currency = 0;
            double currency1 = 0;
            if (config != null)
            {
                double currencyconfig = 0;
                if (!string.IsNullOrEmpty(config.Currency))
                    currencyconfig = Convert.ToDouble(config.Currency);

                currency = Math.Round(currencyconfig, 0);
                currency1 = Math.Round(currencyconfig, 0);
            }

            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);

            int uid = obj_user.ID;

            var id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                var o = MainOrderController.GetAllByID(id);
                if (o != null)
                {
                    hdfOrderID.Value = o.ID.ToString();
                    if (o.OrderType == 3)
                    {
                        pnbaogia.Visible = true;
                        hdfBaoGiaVisible.Value = "1";
                    }
                    else
                    {
                        pnbaogia.Visible = false;
                        hdfBaoGiaVisible.Value = "0";
                    }
                    chkIsCheckPrice.Value = Convert.ToBoolean(o.IsCheckNotiPrice).ToString();
                    chkIsDoneSmallPackage.Value = Convert.ToBoolean(o.IsDoneSmallPackage).ToString();

                    ViewState["ID"] = id;
                    //ltrPrint.Text += "<a class=\"btn btn border-btn\" target=\"_blank\" href='/manager/PrintStamp.aspx?id=" + id + "'>In Tem</a>";
                    double currentcyynn = 0;
                    if (!string.IsNullOrEmpty(o.CurrentCNYVN))
                        currentcyynn = Math.Round(Convert.ToDouble(o.CurrentCNYVN), 0);
                    currency = currentcyynn;
                    currency1 = currency;
                    hdfcurrent.Value = Math.Round(currency, 0).ToString();
                    ViewState["MOID"] = id;
                    #region Lịch sử thanh toán
                    StringBuilder htmlPaid = new StringBuilder();
                    var PayorderHistory = PayOrderHistoryController.GetAllByMainOrderID(o.ID);
                    if (PayorderHistory.Count > 0)
                    {

                        foreach (var item in PayorderHistory)
                        {
                            htmlPaid.Append("<tr>");
                            htmlPaid.Append("    <td>" + item.CreatedDate + "</td>");
                            htmlPaid.Append("    <td>" + PJUtils.ShowStatusPayHistoryNew(item.Status.ToString().ToInt(0)) + "</td>");
                            if (item.Type.ToString() == "1")
                            {
                                htmlPaid.Append("    <td>Trực tiếp</td>");
                            }
                            else
                            {
                                htmlPaid.Append("    <td>Ví điện tử</td>");
                            }
                            htmlPaid.Append("    <td>" + string.Format("{0:N0}", item.Amount.Value) + " VNÐ</td>");
                            htmlPaid.Append("</tr>");
                        }
                        //rptPayment.DataSource = PayorderHistory;
                        //rptPayment.DataBind();
                    }
                    else
                    {

                        htmlPaid.Append("<tr class=\"noti\"><td class=\"red-text\" colspan=\"4\">Không có lịch sử thanh toán nào</td></tr>");
                        //ltrpa.Text = "<tr>Chưa có lịch sử thanh toán nào.</tr>";
                    }
                    #endregion
                    ltrpa.Text = htmlPaid.ToString();

                    if (obj_user != null)
                    {
                        hdfID.Value = obj_user.ID.ToString();
                        #region CheckRole
                        int RoleID = Convert.ToInt32(obj_user.RoleID);
                        if (RoleID == 7)
                        {

                            ddlStatus.Items.Add(new ListItem("Đơn mới", "0"));
                            ddlStatus.Items.Add(new ListItem("Đơn hàng hủy", "1"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đơn chờ mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                            if (o.Status > 5)
                            {
                                ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                                ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                                ddlStatus.Items.Add(new ListItem("Trong kho Hà Nội", "8"));
                                ddlStatus.Items.Add(new ListItem("Đang giao hàng", "11"));
                                ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                ddlStatus.Items.Add(new ListItem("Đơn khiếu nại", "12"));
                                ddlStatus.Enabled = false;
                            }
                            pCNShipFeeNDT.Visible = false;
                            ////pBuyNDT.Visible = false;
                            pWeightNDT.Visible = false;
                            pVolume.Enabled = false;
                            pCheckNDT.Visible = false;
                            pPackedNDT.Visible = false;
                            pnOrderPrice.Enabled = true;
                            pDeposit.Enabled = false;
                            pCNShipFee.Enabled = false;
                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pCheck.Enabled = false;
                            pWeight.Enabled = false;
                            pPacked.Enabled = false;
                            pShipHome.Enabled = true;
                            pCNShipFeeReal.Enabled = false;
                            ltr_OrderFee_UserInfo.Visible = false;
                            pnAdmin.Enabled = false;
                            ltr_AddressReceive.Visible = false;
                            //btnUpdate.Visible = true;
                            btnThanhtoan.Visible = true;
                            ////pShipHomeNDT.Visible = true;
                            ////pnadminmanager.Visible = true;
                            ddlWarehouseFrom.Enabled = true;
                            ddlReceivePlace.Enabled = true;
                            rTotalPriceReal.Enabled = false;
                            ddlShippingType.Enabled = true;
                            pnCurrentCNYVN.Visible = false;
                            pAmountDeposit.Enabled = false;
                        }
                        else if (RoleID == 3)
                        {
                            ddlStatus.Items.Add(new ListItem("Đơn mới", "0"));
                            ddlStatus.Items.Add(new ListItem("Đơn hàng hủy", "1"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đơn chờ mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                            pCNShipFeeNDT.Visible = true;
                            pCNShipFee.Enabled = true;
                            ddlStatus.Enabled = false;
                            rTotalPriceReal.Enabled = true;
                            pCNShipFeeReal.Enabled = true;

                            if (o.Status > 4)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                                ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                                ddlStatus.Items.Add(new ListItem("Trong kho Hà Nội", "8"));
                                ddlStatus.Items.Add(new ListItem("Đang giao hàng", "11"));
                                ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                ddlStatus.Items.Add(new ListItem("Đơn khiếu nại", "12"));
                                pCNShipFeeNDT.Visible = true;

                                //pCNShipFee.Enabled = false;
                                rTotalPriceReal.Enabled = false;
                                pCNShipFeeReal.Enabled = false;

                                //cong sua cho nay
                                rTotalPriceRealCYN.Enabled = false;
                                rTotalPriceReal.Enabled = false;
                                //pCNShipFeeNDT.Visible = true;
                                pCNShipFeeNDT.Enabled = false;
                                //pCNShipFee.Visible = true;
                                pCNShipFee.Enabled = false;
                                pCNShipFeeNDTReal.Enabled = false;
                                pCNShipFeeReal.Enabled = false;
                                pBuyNDT.Enabled = false;
                                pBuy.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pVolume.Enabled = false;
                                pWeight.Enabled = false;
                                tbCurrentCNYVN.Enabled = false;
                                pAmountDeposit.Enabled = false;
                                //pDeposit.Visible = false;
                                pDeposit.Enabled = false;
                                //end sua
                            }
                            ////ltraddordercode.Text = "<div class=\"ordercode addordercode\"><a href=\"javascript:;\" onclick=\"addordercode()\">Thêm mã vận đơn</a></div>";
                            ////pDepositNDT.Visible = false;
                            ////pBuyNDT.Visible = false;

                            //pWeightNDT.Visible = false;
                            //pCheckNDT.Visible = false;
                            //pPackedNDT.Visible = false;
                            //pDeposit.Enabled = false;

                            //pBuyNDT.Enabled = false;
                            //pBuy.Enabled = false;
                            //pCheck.Enabled = false;
                            //pWeight.Enabled = false;
                            //pPacked.Enabled = false;
                            //pShipHome.Enabled = false;
                            //ltr_OrderFee_UserInfo.Visible = true;
                            //ltr_AddressReceive.Visible = true;
                            //ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";

                            pnAdmin.Enabled = false;
                            hhvc.Enabled = true;
                            phuphi.Visible = false;
                            pBuyNDT.Enabled = false;
                            //pricereal.Enabled = true;
                            pBuy.Enabled = false;
                            pWeightNDT.Enabled = false;
                            pVolume.Enabled = false;
                            pWeight.Enabled = false;
                            phituychon.Enabled = false;
                            pDeposit.Enabled = false;
                            txtOrderWeight.Enabled = true;

                            btnThanhtoan.Visible = true;
                            pAmountDeposit.Enabled = true;
                            pnadminmanager.Visible = true;
                            pnOrderPrice.Enabled = true;
                            btnStaffUpdate.Visible = false;
                            chkCheck.Value += "true";
                            chkPackage.Value += "true";
                            chkShiphome.Value += "true";
                            pnCurrentCNYVN.Visible = false;
                            ddlWarehouseFrom.Enabled = true;
                            ddlReceivePlace.Enabled = true;
                            ddlShippingType.Enabled = true;
                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                        }
                        else if (RoleID == 4)
                        {
                            ////ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            ////ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                            ////ddlStatus.Items.Add(new ListItem("Đang về Việt Nam", "6"));
                            if (o.Status < 5)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                                ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                                ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                                ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                //btnUpdate.Visible = false;
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pVolume.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            else if (o.Status >= 5 && o.Status < 6)
                            {
                                ddlStatus.Enabled = true;
                                pPackedNDT.Enabled = true;
                                pPacked.Enabled = true;

                                pWeightNDT.Enabled = true;
                                pWeight.Enabled = true;
                                pVolume.Enabled = true;


                                ////ltraddordercode.Text = "<div class=\"ordercode addordercode\"><a href=\"javascript:;\" onclick=\"addordercode()\">Thêm mã vận đơn</a></div>";

                                ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                                ddlStatus.Items.Add(new ListItem("Đang về Việt Nam", "6"));
                            }
                            else if (o.Status >= 6)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                                ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                                ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                                ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                //btnUpdate.Visible = false;
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pVolume.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            ////pDepositNDT.Visible = false;                            

                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;

                            pCheck.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pCNShipFeeReal.Enabled = false;
                            rTotalPriceReal.Enabled = false;
                            pnAdmin.Enabled = false;
                            pDeposit.Enabled = false;
                            pBuyNDT.Enabled = false;

                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;

                            pnCurrentCNYVN.Visible = false;

                            pShipHome.Enabled = false;
                            ltr_OrderFee_UserInfo.Visible = false;
                            ltr_AddressReceive.Visible = false;

                            txtOrderWeight.Enabled = true;

                            ////pShipHomeNDT.Visible = false;
                        }
                        else if (RoleID == 5)
                        {
                            //ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            //ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));

                            if (o.Status < 5)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                                ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                                ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                                ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                //btnUpdate.Visible = false;
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;
                                pWeightNDT.Enabled = false;
                                pVolume.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            else if (o.Status >= 5)
                            {
                                ddlStatus.Enabled = true;
                                ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                                ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                                ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                                ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                                ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                                ////ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                                ////ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                pPackedNDT.Enabled = false;
                                pPacked.Enabled = false;

                                pWeightNDT.Enabled = false;
                                pVolume.Enabled = false;
                                pWeight.Enabled = false;
                            }
                            ////if (o.Status >= 7)
                            ////{
                            ////    ddlStatus.Enabled = false;
                            ////    ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            ////    ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                            ////    ddlStatus.Items.Add(new ListItem("Đang về Việt Nam", "6"));
                            ////    ddlStatus.Items.Add(new ListItem("Đã nhận hàng tại VN", "7"));
                            ////    ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            ////    ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                            ////    btnUpdate.Visible = false;
                            ////    pPackedNDT.Enabled = false;
                            ////    pPacked.Enabled = false;
                            ////    pWeightNDT.Enabled = false;
                            ////    pWeight.Enabled = false;
                            ////}

                            ////pDepositNDT.Visible = false;

                            pCNShipFeeNDT.Enabled = false;

                            rTotalPriceReal.Enabled = false;
                            pnAdmin.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;
                            pCNShipFeeReal.Enabled = false;
                            pDeposit.Enabled = false;
                            pnCurrentCNYVN.Visible = false;
                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;

                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;

                            pShipHome.Enabled = false;

                            ltr_OrderFee_UserInfo.Visible = false;
                            ltr_AddressReceive.Visible = false;
                            txtOrderWeight.Enabled = true;

                            ////ltraddordercode.Text = "<div class=\"ordercode addordercode\"><a href=\"javascript:;\" onclick=\"addordercode()\" >Thêm mã vận đơn</a></div>";
                            ////pShipHomeNDT.Visible = false;
                        }
                        else if (RoleID == 0)
                        {
                            pnadminmanager.Visible = true;
                            ddlStatus.Items.Add(new ListItem("Đơn mới", "0"));
                            ddlStatus.Items.Add(new ListItem("Đơn hàng hủy", "1"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đơn chờ mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                            ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                            ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                            ddlStatus.Items.Add(new ListItem("Trong kho Hà Nội", "8"));
                            ddlStatus.Items.Add(new ListItem("Đang giao hàng", "11"));
                            ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            ddlStatus.Items.Add(new ListItem("Đơn khiếu nại", "12"));
                            ddlWarehouseFrom.Enabled = true;
                            ddlReceivePlace.Enabled = true;
                            ddlShippingType.Enabled = true;

                            if (o.Status > 2)
                            {
                                if (obj_user.ID == 1)
                                {
                                    pDeposit.Enabled = true;
                                    pAmountDeposit.Enabled = true;
                                }
                                else
                                {
                                    pDeposit.Enabled = false;
                                    pAmountDeposit.Enabled = false;
                                }
                            }

                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                        }
                        else if (RoleID == 2)
                        {
                            pnadminmanager.Visible = true;
                            ddlStatus.Items.Add(new ListItem("Đơn mới", "0"));
                            ddlStatus.Items.Add(new ListItem("Đơn hàng hủy", "1"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đơn chờ mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                            ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                            ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                            ddlStatus.Items.Add(new ListItem("Trong kho Hà Nội", "8"));
                            ddlStatus.Items.Add(new ListItem("Đang giao hàng", "11"));
                            ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            ddlStatus.Items.Add(new ListItem("Đơn khiếu nại", "12"));
                            ddlStatus.Enabled = true;
                            pDeposit.Enabled = false;
                            pCNShipFeeNDT.Enabled = true;
                            pCNShipFee.Enabled = true;
                            pnOrderPrice.Enabled = true;
                            pBuy.Enabled = false;
                            pWeightNDT.Enabled = false;
                            pWeight.Enabled = false;
                            pVolume.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;
                            pCNShipFeeReal.Enabled = true;
                            hhvc.Enabled = true;
                            pPackedNDT.Enabled = true;
                            pPacked.Enabled = true;
                            pShipHome.Enabled = true;
                            ddlWarehouseFrom.Enabled = true;
                            ddlReceivePlace.Enabled = true;
                            rTotalPriceReal.Enabled = true;
                            ddlShippingType.Enabled = true;
                            pnCurrentCNYVN.Visible = false;
                            pnAdmin.Enabled = false;

                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                        }
                        else if (RoleID == 6)
                        {
                            ddlStatus.Items.Add(new ListItem("Đơn mới", "0"));
                            ddlStatus.Items.Add(new ListItem("Đơn hàng hủy", "1"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đơn chờ mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                            ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                            ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                            ddlStatus.Items.Add(new ListItem("Trong kho Hà Nội", "8"));
                            ddlStatus.Items.Add(new ListItem("Đang giao hàng", "11"));
                            ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            ddlStatus.Items.Add(new ListItem("Đơn khiếu nại", "12"));
                            ddlStatus.Enabled = false;
                            pDeposit.Enabled = false;
                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;
                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pWeightNDT.Enabled = false;
                            pVolume.Enabled = false;
                            pWeight.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pnCurrentCNYVN.Visible = false;
                            pnAdmin.Enabled = false;
                            pCheck.Enabled = false;
                            pPackedNDT.Enabled = false;
                            pPacked.Enabled = false;
                            pCNShipFeeReal.Enabled = false;
                            pShipHome.Enabled = false;
                            pricereal.Visible = false;
                            mvd.Visible = false;
                            rTotalPriceRealCYN.Visible = false;
                            rTotalPriceReal.Enabled = false;
                            tienhh.Visible = false;
                            phuphi.Visible = false;
                            pHHCYN.Visible = false;
                            pHHVND.Visible = false;
                            //txtComment.Visible = true;
                            ////ddlTypeComment.Visible = true;
                            //btnSend.Visible = true;
                            //btnUpdate.Visible = false;

                        }
                        else if (RoleID == 9)
                        {
                            ddlStatus.Items.Add(new ListItem("Đơn mới", "0"));
                            ddlStatus.Items.Add(new ListItem("Đơn hàng hủy", "1"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            ddlStatus.Items.Add(new ListItem("Đơn chờ mua hàng", "4"));
                            ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                            ddlStatus.Items.Add(new ListItem("Đơn người bán giao", "3"));
                            pCNShipFeeNDT.Visible = true;
                            pCNShipFee.Enabled = false;
                            ddlStatus.Enabled = false;
                            if (o.Status > 5)
                            {
                                ddlStatus.Enabled = false;
                                ddlStatus.Items.Add(new ListItem("Kho Trung Quốc nhận hàng", "6"));
                                ddlStatus.Items.Add(new ListItem("Trên đường về Việt Nam", "7"));
                                ddlStatus.Items.Add(new ListItem("Trong kho Hà Nội", "8"));
                                ddlStatus.Items.Add(new ListItem("Đang giao hàng", "11"));
                                ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                                ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                                ddlStatus.Items.Add(new ListItem("Đơn khiếu nại", "12"));
                                pCNShipFeeNDT.Visible = false;
                                pCNShipFee.Enabled = false;
                            }
                            if (o.Status > 2)
                            {
                                pDeposit.Enabled = false;
                                pAmountDeposit.Enabled = false;
                            }

                            pnCurrentCNYVN.Visible = false;
                            nhanhangtai.Enabled = true;
                            hhvc.Enabled = true;
                            phuphi.Visible = false;
                            pBuyNDT.Enabled = false;
                            pricereal.Enabled = false;
                            pBuy.Enabled = false;
                            pWeightNDT.Enabled = false;
                            pVolume.Enabled = false;
                            pWeight.Enabled = false;
                            pCNShipFeeReal.Enabled = false;
                            phituychon.Enabled = false;
                            txtOrderWeight.Enabled = true;
                            btnThanhtoan.Visible = true;
                            rTotalPriceReal.Enabled = false;
                            chkCheck.Value += "true";
                            chkPackage.Value += "true";
                            chkShiphome.Value += "true";
                            ddlWarehouseFrom.Enabled = true;
                            ddlReceivePlace.Enabled = true;
                            ddlShippingType.Enabled = true;
                            ltrBtnUpdate.Text = "<a href=\"javascript:;\" class=\"btn mt-2\" onclick=\"UpdateOrder()\">CẬP NHẬT</a>";
                        }
                        else if (RoleID == 8)
                        {
                            //ddlStatus.Items.Add(new ListItem("Đơn mới", "0"));
                            //ddlStatus.Items.Add(new ListItem("Đơn hàng hủy", "1"));
                            //ddlStatus.Items.Add(new ListItem("Đơn đã cọc", "2"));
                            //ddlStatus.Items.Add(new ListItem("Đơn đã mua hàng", "5"));
                            //ddlStatus.Items.Add(new ListItem("Đang về Việt Nam", "6"));
                            //ddlStatus.Items.Add(new ListItem("Đã nhận hàng tại VN", "7"));
                            //ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            ddlStatus.Items.Add(new ListItem("Đã thanh toán", "9"));
                            ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            ddlStatus.Enabled = true;
                            pDeposit.Enabled = false;
                            pCNShipFeeNDT.Enabled = false;
                            pCNShipFee.Enabled = false;
                            pBuyNDT.Enabled = false;
                            pBuy.Enabled = false;
                            pWeightNDT.Enabled = false;
                            pVolume.Enabled = false;
                            pWeight.Enabled = false;
                            pCheckNDT.Enabled = false;
                            pCheck.Enabled = false;
                            pPackedNDT.Enabled = false;
                            pPacked.Enabled = false;
                            pCNShipFeeReal.Enabled = false;
                            pShipHome.Enabled = false;
                            pnCurrentCNYVN.Visible = false;
                            rTotalPriceReal.Enabled = false;
                            //btnUpdate.Visible = true;
                            pnAdmin.Enabled = false;
                            //txtComment.Visible = true;
                            ////ddlTypeComment.Visible = true;
                            //btnSend.Visible = true;
                            txtOrderWeight.Enabled = false;
                        }
                        int countOc = 1;
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode2) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight2))
                        {
                            hdfoc2.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc2.Value = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode3) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight3))
                        {
                            hdfoc3.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc3.Value = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode4) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight4))
                        {
                            hdfoc4.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc4.Value = "0";
                        }
                        if (!string.IsNullOrEmpty(o.OrderTransactionCode5) || !string.IsNullOrEmpty(o.OrderTransactionCodeWeight5))
                        {
                            hdfoc5.Value = "1";
                            countOc++;
                        }
                        else
                        {
                            hdfoc5.Value = "0";
                        }
                        hdforderamount.Value = countOc.ToString();
                        #endregion
                        #region Lấy thông tin nhân viên
                        ddlSaler.SelectedValue = o.SalerID.ToString();
                        ddlDatHang.SelectedValue = o.DathangID.ToString();
                        ddlCSKH.SelectedValue = o.CSID.ToString();
                        ddlKhoTQ.SelectedValue = o.KhoTQID.ToString();
                        ddlKhoVN.SelectedValue = o.KhoVNID.ToString();
                        #endregion
                        #region Lấy thông tin người đặt
                        var usercreate = AccountController.GetByID(Convert.ToInt32(o.UID));
                        double ckFeeBuyPro = 0;
                        double ckFeeWeight = 0;
                        if (usercreate != null)
                        {
                            ckFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeBuyPro.ToString());
                            ckFeeWeight = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeWeight.ToString());

                            lblCKFeebuypro.Text = ckFeeBuyPro.ToString();
                            lblCKFeeWeight.Text = ckFeeWeight.ToString();

                            hdfFeeBuyProDiscount.Value = ckFeeBuyPro.ToString();
                            hdfFeeWeightDiscount.Value = ckFeeWeight.ToString();
                        }
                        else
                        {
                            lblCKFeebuypro.Text = "0";
                            lblCKFeeWeight.Text = "0";
                            hdfFeeBuyProDiscount.Value = "0";
                            hdfFeeWeightDiscount.Value = "0";
                        }

                        if (RoleID != 8)
                        {
                            StringBuilder customerInfo = new StringBuilder();
                            if (RoleID == 9)
                            {
                                customerInfo.Append("<span>Tài khoản không đủ quyền xem thông tin này</span>");
                                //ltr_OrderFee_UserInfo.Text += "Tài khoản không đủ quyền xem thông tin này";
                            }
                            else
                            {
                                var ui = AccountInfoController.GetByUserID(Convert.ToInt32(o.UID));
                                if (ui != null)
                                {
                                    string phone = ui.Phone;

                                    //ltr_OrderFee_UserInfo.Text += "<dt>Tên:</dt>";
                                    //ltr_OrderFee_UserInfo.Text += "<dd>" + ui.FirstName + " " + ui.LastName + "</dd>";
                                    //ltr_OrderFee_UserInfo.Text += "<dt>Địa chỉ:</dt>";
                                    //ltr_OrderFee_UserInfo.Text += "<dd>" + ui.Address + "</dd>";
                                    //ltr_OrderFee_UserInfo.Text += "<dt>Email:</dt>";
                                    //ltr_OrderFee_UserInfo.Text += "<dd><a href=\"" + ui.Email + "\">" + ui.Email + "</a></dd>";
                                    //ltr_OrderFee_UserInfo.Text += "<dt>Số dt:</dt>";
                                    //ltr_OrderFee_UserInfo.Text += "<dd><a href=\"tel:+" + phone + "\">" + phone + "</a></dd>";
                                    //ltr_OrderFee_UserInfo.Text += "<dt>Ghi chú:</dt>";
                                    //ltr_OrderFee_UserInfo.Text += "<dd>" + o.Note + "</dd>";

                                    customerInfo.Append("<table class=\"table\">");
                                    customerInfo.Append("    <tbody>");
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Username</td>");
                                    customerInfo.Append("            <td>" + AccountController.GetByID(Convert.ToInt32(o.UID)).Username + "</td>");
                                    customerInfo.Append("        </tr>");
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Địa chỉ</td>");
                                    customerInfo.Append("            <td>" + ui.Address + "</td>");
                                    customerInfo.Append("        </tr>");
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Email</td>");
                                    customerInfo.Append("            <td><a href=\"" + ui.Email + "\">" + ui.Email + "</a></td>");
                                    customerInfo.Append("        </tr>");
                                    if (RoleID != 3)
                                    {
                                        customerInfo.Append("        <tr>");
                                        customerInfo.Append("            <td>Số ĐT</td>");
                                        customerInfo.Append("            <td><a href=\"tel:+" + phone + "\">" + phone + "</a></td>");
                                        customerInfo.Append("        </tr>");
                                    }
                                    customerInfo.Append("        <tr>");
                                    customerInfo.Append("            <td>Ghi chú</td>");
                                    customerInfo.Append("            <td>" + o.Note + "</td>");
                                    customerInfo.Append("        </tr>");
                                    customerInfo.Append("    </tbody>");
                                    customerInfo.Append("</table>");


                                }
                            }
                            ltr_OrderFee_UserInfo.Text = customerInfo.ToString();
                        }

                        ltr_OrderCode.Text += "<div class=\"order-panel\">";
                        ltr_OrderCode.Text += " <div class=\"title\">Mã đơn hàng</div>";
                        ltr_OrderCode.Text += "     <div class=\"cont\">";
                        ltr_OrderCode.Text += "         <p><strong>" + o.ID + "</strong></p>";
                        ltr_OrderCode.Text += "     </div>";
                        ltr_OrderCode.Text += "</div>";

                        ltr_OrderID.Text += "<strong>" + o.ID + "</strong>";



                        //var use = AccountController.GetByID(Convert.ToInt32(o.UID));
                        //if (use != null)
                        //{
                        //    ltr_OrderFee_UserInfo1.Text += "<dt>User đặt hàng</dt>";
                        //    ltr_OrderFee_UserInfo1.Text += "<dd>" + use.Username + "</dd>";
                        //    ltr_OrderFee_UserInfo1.Text += "<dt>Ghi chú:</dt>";
                        //}



                        var kd = AccountController.GetByID(Convert.ToInt32(o.SalerID));
                        var dathang = AccountController.GetByID(Convert.ToInt32(o.DathangID));
                        var khotq = AccountController.GetByID(Convert.ToInt32(o.KhoTQID));
                        var khovn = AccountController.GetByID(Convert.ToInt32(o.KhoVNID));
                        if (kd != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên kinh doanh:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + kd.Username + "</strong></dd>";
                        }
                        if (dathang != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên đặt hàng:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + dathang.Username + "</strong></dd>";
                        }
                        if (khotq != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên kho TQ:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + khotq.Username + "</strong></dd>";
                        }
                        if (khovn != null)
                        {
                            ltr_OrderFee_UserInfo2.Text += "    <dt style=\"width: 200px;\">Nhân viên kho đích:</dt>";
                            ltr_OrderFee_UserInfo2.Text += "    <dd><strong>" + khovn.Username + "</strong></dd>";
                        }
                        #endregion
                        #region Lấy thông tin đơn hàng
                        txtMainOrderCode.Text = o.MainOrderCode;

                        //NEW HDK
                        var listMainOrderCode = MainOrderCodeController.GetAllByMainOrderID(o.ID);
                        ListItem ddlitem = new ListItem("Chọn mã đơn hàng", "0");
                        ddlMainOrderCode.Items.Add(ddlitem);
                        if (listMainOrderCode != null)
                        {

                            if (listMainOrderCode.Count > 0)
                            {
                                StringBuilder html = new StringBuilder();
                                foreach (var item in listMainOrderCode)
                                {
                                    ListItem listitem = new ListItem(item.MainOrderCode, item.ID.ToString());
                                    ddlMainOrderCode.Items.Add(listitem);
                                    //html.Append("<tr>");
                                    //html.Append("    <td class=\"MainOrderInPut\">");
                                    //html.Append("        <input class=\"MainOrderCode\" data-orderCodeID=\"" + item.ID + "\" onkeypress=\"myFunction($(this))\" type=\"text\" value=\"" + item.MainOrderCode+"\">");
                                    //html.Append("    </td>");
                                    //html.Append("    <td style=\"width:24px\">");
                                    //html.Append("        <a href=\"javascript:;\" onclick=\"deleteMVD($(this))\"><i class=\"material-icons valign-center\">remove_circle</i></a>");
                                    //html.Append("    </td>");
                                    //html.Append("</tr>");

                                    html.Append("<div class=\"row order-wrap\">");
                                    html.Append("    <div class=\"input-field col s10 m11 MainOrderInPut\">");
                                    html.Append("        <input type=\"text\" class=\"MainOrderCode\"  data-orderCodeID=\"" + item.ID + "\"  onkeypress=\"myFunction($(this))\" value=\"" + item.MainOrderCode + "\">");
                                    html.Append("       <span class=\"helper-text hide\" style=\"position:absolute;\">");
                                    html.Append("       <label style=\"color:green\">Đã cập nhật</label>");
                                    html.Append("       </span>");
                                    html.Append("    </div>");
                                    html.Append("    <a href=\"javascript:;\" onclick=\"deleteMVD($(this))\" style=\"line-height:80px;position:absolute\" class=\"remove-order tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons\">remove_circle</i></a>");
                                    html.Append("</div>");
                                }
                                lrtMainOrderCode.Text = html.ToString();
                            }
                        }

                        chkCheck.Value = o.IsCheckProduct.ToString().ToBool().ToString();
                        chkPackage.Value = o.IsPacked.ToString().ToBool().ToString();
                        chkSpecial1.Value = o.IsCheckSpecial1.ToString().ToBool().ToString();
                        chkSpecial2.Value = o.IsCheckSpecial2.ToString().ToBool().ToString();
                        chkShiphome.Value = o.IsFastDelivery.ToString().ToBool().ToString();
                        hdfIsInsurrance.Value = Convert.ToBoolean(o.IsInsurrance).ToString();
                        //chkIsFast.Checked = o.IsFast.ToString().ToBool();
                        double feeeinwarehouse = 0;
                        if (o.FeeInWareHouse != null)
                            feeeinwarehouse = Convert.ToDouble(o.FeeInWareHouse);
                        rFeeWarehouse.Text = Math.Round(feeeinwarehouse, 0).ToString();

                        if (o.IsGiaohang != null)
                        {
                            chkIsGiaohang.Value = o.IsGiaohang.ToString();
                        }
                        else
                        {
                            chkIsGiaohang.Value = "false";
                        }

                        tbCurrentCNYVN.Text = o.CurrentCNYVN;



                        if (o.OrderDone != null)
                        {
                            chkOrderDone.Value = o.OrderDone.ToString();
                        }
                        else
                        {
                            chkOrderDone.Value = "false";
                        }
                        if (o.OrderPrice != null)
                        {
                            chkOrderPrice.Value = o.OrderPrice.ToString();
                        }
                        else
                        {
                            chkOrderPrice.Value = "false";
                        }

                        if (!string.IsNullOrEmpty(o.AmountDeposit))
                        {
                            double amountdeposit = Math.Round(Convert.ToDouble(o.AmountDeposit.ToString()), 0);
                            pAmountDeposit.Value = amountdeposit;
                            //lblAmountDeposit.Text = string.Format("{0:N0}", amountdeposit) + " ";
                        }
                        else
                        {
                            pAmountDeposit.Value = 0;
                            //lblAmountDeposit.Text = "0 ";
                        }









                        if (!string.IsNullOrEmpty(o.TotalPriceReal))
                            rTotalPriceReal.Text = string.Format("{0:N0}", Math.Round(Convert.ToDouble(o.TotalPriceReal)));
                        else
                            rTotalPriceReal.Text = "0";

                        if (!string.IsNullOrEmpty(o.TotalPriceRealCYN))
                            rTotalPriceRealCYN.Text = Math.Round(Convert.ToDouble(o.TotalPriceRealCYN), 2).ToString();
                        else
                            rTotalPriceRealCYN.Text = "0";

                        ddlStatus.SelectedValue = o.Status.ToString();
                        if (!string.IsNullOrEmpty(o.Deposit))
                            pDeposit.Value = Math.Round(Convert.ToDouble(o.Deposit));

                        double fscn = 0;
                        double fscn1 = 0;
                        if (!string.IsNullOrEmpty(o.FeeShipCNCYN) && Convert.ToDouble(o.FeeShipCNCYN) > 0)
                        {
                            fscn = Convert.ToDouble(o.FeeShipCNCYN);
                            fscn1 = Convert.ToDouble(o.FeeShipCN);
                            pCNShipFeeNDT.Text = (fscn).ToString();
                            pCNShipFee.Text = string.Format("{0:N0}", Convert.ToDouble(fscn * currency1));
                            lblShipTQ.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeShipCN));
                        }
                        else
                        {
                            double phishipcn = 0;
                            phishipcn = Convert.ToDouble(o.FeeShipCN) / currency1;
                            fscn1 = Convert.ToDouble(o.FeeShipCN);
                            lblShipTQ.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeShipCN));

                            if (Convert.ToDouble(phishipcn) > 0)
                            {
                                pCNShipFeeNDT.Text = string.Format("{0:#.##}", phishipcn);

                            }
                            else
                            {
                                pCNShipFeeNDT.Text = "0";

                            }

                            pCNShipFee.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeShipCN));
                        }

                        double fscnreal = 0;
                        if (!string.IsNullOrEmpty(o.FeeShipCNRealCYN) && Convert.ToDouble(o.FeeShipCNRealCYN) > 0)
                        {
                            fscnreal = Convert.ToDouble(o.FeeShipCNRealCYN);
                            pCNShipFeeNDTReal.Text = (fscnreal).ToString();
                            pCNShipFeeReal.Text = string.Format("{0:N0}", Convert.ToDouble(fscnreal * currency1));
                        }
                        else
                        {
                            double tienmuathat = 0;
                            tienmuathat = Convert.ToDouble(o.FeeShipCNReal) / currency1;
                            if (Convert.ToDouble(tienmuathat) > 0)
                            {
                                pCNShipFeeNDTReal.Text = string.Format("{0:#.##}", tienmuathat);

                            }
                            else
                            {
                                pCNShipFeeNDTReal.Text = "0";

                            }
                            pCNShipFeeReal.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeShipCNReal));
                        }
                        double realprice = 0;
                        if (!string.IsNullOrEmpty(o.TotalPriceReal))
                            realprice = Convert.ToDouble(o.TotalPriceReal);

                        double tot = Convert.ToDouble(o.PriceVND) + fscn1 - realprice;
                        double totCYN = tot / currency1;
                        pHHCYN.Text = totCYN.ToString();
                        pHHVND.Text = string.Format("{0:N0}", Convert.ToDouble(tot));

                        if (!string.IsNullOrEmpty(o.FeeBuyPro))
                        {
                            pBuy.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeBuyPro));
                            lblFeeBuyProduct.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeBuyPro));
                        }
                        if (!string.IsNullOrEmpty(o.FeeWeight))
                        {
                            pWeight.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeWeight));
                        }
                        else
                        {
                            pWeight.Text = "0";
                        }
                        if (!string.IsNullOrEmpty(o.TQVNWeight))
                        {
                            pWeightNDT.Text = Convert.ToDouble(o.TQVNWeight).ToString();
                        }
                        else
                        {
                            pWeightNDT.Text = "0";
                        }
                        if (!string.IsNullOrEmpty(o.TQVNVolume))
                        {
                            pVolume.Text = Convert.ToDouble(o.TQVNVolume).ToString();
                        }
                        else
                        {
                            pVolume.Text = "0";
                        }


                        double checkproductprice = Convert.ToDouble(o.IsCheckProductPrice);
                        pCheck.Text = string.Format("{0:N0}", checkproductprice);
                        pCheckNDT.Text = (checkproductprice / currency).ToString();


                        double packagedprice = Convert.ToDouble(o.IsPackedPrice);
                        pPacked.Text = string.Format("{0:N0}", packagedprice);
                        pPackedNDT.Text = (packagedprice / currency).ToString();

                        double SpecialPrice = Convert.ToDouble(o.IsCheckPriceSpecial);
                        txtFeeSpecial.Text = string.Format("{0:N0}", SpecialPrice);

                        double InsuranceMoney = Convert.ToDouble(o.InsuranceMoney);
                        txtInsuranceMoney.Text = string.Format("{0:N0}", InsuranceMoney);

                        pShipHome.Text = Convert.ToDouble(o.IsFastDeliveryPrice).ToString();

                        double TotalFeeSupport = Convert.ToDouble(o.TotalFeeSupport);

                        lblMoneyNotFee.Text = string.Format("{0:N0}", Convert.ToDouble(o.PriceVND));
                        lblTotalMoneyVND1.Text = string.Format("{0:N0}", Convert.ToDouble(o.PriceVND));
                        lblTotalMoneyCNY1.Text = string.Format("{0:#.##}", Convert.ToDouble(o.PriceVND) / currency);
                        double totalFee = Convert.ToDouble(o.IsCheckProductPrice) + Convert.ToDouble(o.IsPackedPrice) +
                           Convert.ToDouble(o.IsFastDeliveryPrice) + Convert.ToDouble(o.IsFastPrice) + InsuranceMoney + SpecialPrice;
                        lblAllFee.Text = string.Format("{0:N0}", totalFee);
                        lblFeeTQVN.Text = string.Format("{0:N0}", Convert.ToDouble(o.FeeWeight));
                        double odweight = 0;
                        if (!string.IsNullOrEmpty(o.OrderWeight))
                            odweight = Convert.ToDouble(o.OrderWeight);
                        txtOrderWeight.Text = odweight.ToString();
                        string orderweightfeedc = o.FeeWeightCK;

                        ddlWarehouseFrom.SelectedValue = o.FromPlace.ToString();
                        hdfFromPlace.Value = o.FromPlace.ToString();

                        ddlReceivePlace.SelectedValue = o.ReceivePlace;
                        hdfReceivePlace.Value = o.ReceivePlace;

                        ddlShippingType.SelectedValue = o.ShippingType.ToString();
                        hdfShippingType.Value = o.ShippingType.ToString();

                        if (string.IsNullOrEmpty(orderweightfeedc))
                        {
                            lblCKFeeweightPrice.Text = "0";
                            hdfFeeweightPriceDiscount.Value = "0";
                        }
                        else
                        {
                            lblCKFeeweightPrice.Text = orderweightfeedc;
                            hdfFeeweightPriceDiscount.Value = orderweightfeedc;
                        }
                        //lblCKFeeweightPrice.Text = string.Format("{0:N0}", Convert.ToDouble(orderweightfeedc));

                        double alltotal = Math.Round(Convert.ToDouble(o.TotalPriceVND));

                        lblAllTotal.Text = string.Format("{0:N0}", alltotal);
                        lblDeposit.Text = string.Format("{0:N0}", Convert.ToDouble(o.Deposit));
                        lblLeftPay.Text = string.Format("{0:N0}", alltotal - Convert.ToDouble(o.Deposit));

                        ltrlblAllTotal1.Text = string.Format("{0:N0}", alltotal);
                        lblDeposit1.Text = string.Format("{0:N0}", Convert.ToDouble(o.Deposit));
                        lblLeftPay1.Text = string.Format("{0:N0}", alltotal - Convert.ToDouble(o.Deposit));

                        string statreturn = PJUtils.IntToRequestAdminReturnBG(Convert.ToInt32(o.Status));
                        //ltrStatus1.Text += "<div class=\"inner inline-lb-info " + statreturn + "\">";
                        //ltrStatus1.Text += "<div class=\"lb\">Trạng thái</div>";
                        //ltrStatus1.Text += "<div class=\"info\">" + PJUtils.IntToRequestAdmin(Convert.ToInt32(o.Status)) + "</div>";
                        //ltrStatus1.Text += "</div>";
                        #endregion
                        #region Lấy thông tin nhận hàng
                        StringBuilder customerInfo2 = new StringBuilder();
                        if (RoleID == 3 || RoleID == 9)
                        {
                            //ltr_AddressReceive.Text = "Tài khoản không đủ quyền xem thông tin này";
                            customerInfo2.Append("<span>Tài khoản không đủ quyền xem thông tin này</span>");
                        }
                        else
                        {
                            //ltr_AddressReceive.Text += "<dt>Tên:</dt>";
                            //ltr_AddressReceive.Text += "<dd>" + o.FullName + "</dd>";
                            //ltr_AddressReceive.Text += "<dt>Địa chỉ:</dt>";
                            //ltr_AddressReceive.Text += "<dd>" + o.Address + "</dd>";
                            //ltr_AddressReceive.Text += "<dt>Email:</dt>";
                            //ltr_AddressReceive.Text += "<dd><a href=\"" + o.Email + "\">" + o.Email + "</a></dd>";
                            //ltr_AddressReceive.Text += "<dt>Số dt:</dt>";
                            //ltr_AddressReceive.Text += "<dd><a href=\"tel:+" + o.Phone + "\">" + o.Phone + "</a></dd>";
                            ////ltr_AddressReceive.Text += "<dt>Ghi chú:</dt>";
                            ////ltr_AddressReceive.Text += "<dd>" + o.Note + "</dd>";

                            customerInfo2.Append("<table class=\"table\">");
                            customerInfo2.Append("    <tbody>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Tên</td>");
                            customerInfo2.Append("            <td>" + o.FullName + "</td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Địa chỉ</td>");
                            customerInfo2.Append("            <td>" + o.Address + "</td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Email</td>");
                            customerInfo2.Append("            <td><a href=\"" + o.Email + "\">" + o.Email + "</a></td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Sô´ ÐT</td>");
                            customerInfo2.Append("            <td><a href=\"tel:+" + o.Phone + "\">" + o.Phone + "</a></td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("        <tr>");
                            customerInfo2.Append("            <td>Ghi chú</td>");
                            customerInfo2.Append("            <td>" + o.Note + "</td>");
                            customerInfo2.Append("        </tr>");
                            customerInfo2.Append("    </tbody>");
                            customerInfo2.Append("</table>");
                        }
                        ltr_AddressReceive.Text = customerInfo2.ToString();
                        #endregion
                        #region Lấy sản phẩm
                        int totalproduct = 0;
                        List<tbl_Order> lo = new List<tbl_Order>();
                        lo = OrderController.GetByMainOrderID(o.ID);
                        if (lo.Count > 0)
                        {
                            //rpt.DataSource = lo;
                            //rpt.DataBind();
                            int stt = 1;
                            StringBuilder html = new StringBuilder();
                            foreach (var item in lo)
                            {
                                double currentcyt = Convert.ToDouble(item.CurrentCNYVN);
                                double price = 0;
                                double pricepromotion = Convert.ToDouble(item.price_promotion);
                                double priceorigin = Convert.ToDouble(item.price_origin);
                                if (pricepromotion > 0)
                                {
                                    if (priceorigin > pricepromotion)
                                    {
                                        price = pricepromotion;
                                    }
                                    else
                                    {
                                        price = priceorigin;
                                    }
                                }
                                else
                                {
                                    price = priceorigin;
                                }
                                double vndprice = price * currentcyt;
                                //ltrProducts.Text += "<tr>";
                                ////ltrProducts.Text += "<td rowspan=\"2\">" + item.ID + "</td>";
                                //ltrProducts.Text += "<td rowspan=\"2\">" + stt + "</td>";
                                //ltrProducts.Text += "<td>";
                                //ltrProducts.Text += "<div class=\"product-infobox\">";
                                //ltrProducts.Text += "<a href=\"" + item.link_origin + "\" target=\"_blank\" class=\"img\">";
                                //ltrProducts.Text += "<img src=\"" + item.image_origin + "\" alt=\"\"></a>";
                                //ltrProducts.Text += "<div class=\"info\">";
                                //ltrProducts.Text += "<a href=\"" + item.link_origin + "\" target=\"_blank\">" + item.title_origin + "</a>";
                                //ltrProducts.Text += "</div>";
                                //ltrProducts.Text += "</div>";
                                //ltrProducts.Text += "</td>";
                                //ltrProducts.Text += "<td>" + item.quantity + "</td>";
                                //ltrProducts.Text += "<td>";
                                //ltrProducts.Text += "<p>" + string.Format("{0:N0}", vndprice) + " vnđ</p>";
                                //ltrProducts.Text += "<p>¥ " + string.Format("{0:0.##}", price) + "</p>";
                                //ltrProducts.Text += "</td>";
                                //if (string.IsNullOrEmpty(item.ProductStatus.ToString()))
                                //{
                                //    ltrProducts.Text += "<td>Còn hàng</td>";
                                //}
                                //else
                                //{
                                //    if (item.ProductStatus == 2)
                                //        ltrProducts.Text += "<td>Hết hàng</td>";
                                //    else
                                //        ltrProducts.Text += "<td>Còn hàng</td>";
                                //}
                                //ltrProducts.Text += "<td rowspan=\"2\" class=\"hl-txt\">";
                                //ltrProducts.Text += "<a href=\"/manager/ProductEdit.aspx?id=" + item.ID + "\" class=\"left edit-btn\">Sửa</a>";
                                //ltrProducts.Text += "<a href=\"javascript:;\" class=\"toggle-detail-row right\"><i class=\"fa fa-caret-down\"></i></a>";
                                //ltrProducts.Text += "</td>";
                                //ltrProducts.Text += "</tr>";
                                //ltrProducts.Text += "<tr class=\"detail-row\" style=\"display:block\">";
                                //ltrProducts.Text += "<td colspan=\"4\">";
                                //ltrProducts.Text += "<dl class=\"dl\">";
                                //ltrProducts.Text += "<dt>Thuộc tính</dt>";
                                //ltrProducts.Text += "<dd>" + item.property + "</dd>";
                                //ltrProducts.Text += "<dt>Ghi chú:</dt>";
                                //ltrProducts.Text += "<dd>" + item.brand + "</dd>";
                                //ltrProducts.Text += "</dl>";
                                //ltrProducts.Text += "</td>";
                                //ltrProducts.Text += "</tr>";


                                html.Append("<div class=\"item-wrap\">");
                                html.Append("    <div class=\"item-name\">");
                                html.Append("        <div class=\"number\">");
                                html.Append("            <span class=\"count\">" + stt + "</span>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"orderid\">");
                                html.Append("            <span>" + item.ID + "</span>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"name\">");
                                html.Append("            <span class=\"item-img\">");
                                html.Append("                <a href=\"" + item.link_origin + "\"><img src=\"" + item.image_origin + "\" alt=\"image\"></a>");
                                html.Append("            </span>");
                                html.Append("            <div class=\"caption\">");
                                html.Append("                <a href=\"" + item.link_origin + "\" target=\"_blank\" class=\"title black-text\">" + item.title_origin + "</a>");
                                html.Append("                <div class=\"item-price mt-1\">");
                                html.Append("                    <span class=\"pr-2 black-text font-weight-600\">Thuộc tính: </span><span class=\"pl-2 black-text font-weight-600\">" + item.property + "</span>");
                                html.Append("                </div>");
                                html.Append("                <div class=\"note\">");
                                html.Append("                    <span class=\"black-text font-weight-500\">Ghi chú: </span>");
                                html.Append("                    <div class=\"input-field inline\">");
                                html.Append("                        <input type=\"text\" value=\"" + item.brand + "\" class=\"validate\">");
                                html.Append("                    </div>");
                                html.Append("                </div>");
                                html.Append("            </div>");
                                html.Append("        </div>");
                                html.Append("    </div>");
                                html.Append("    <div class=\"item-info\">");
                                html.Append("        <div class=\"item-num column\">");
                                html.Append("            <span class=\"black-text\"><strong>Số lượng</strong></span>");
                                html.Append("            <p>" + item.quantity + "</p>");
                                html.Append("            <p></p>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"item-price column\">");
                                html.Append("            <span class=\"black-text\"><strong>Đơn giá</strong></span>");
                                html.Append("            <p class=\"grey-text font-weight-500\">¥" + string.Format("{0:0.##}", price) + "</p>");
                                html.Append("            <p class=\"grey-text font-weight-500\">" + string.Format("{0:N0}", vndprice) + " VNÐ</p>");
                                html.Append("        </div>");
                                html.Append("        <div class=\"item-status column\">");
                                html.Append("            <span class=\"black-text\"><strong>Trạng thái</strong></span>");
                                if (string.IsNullOrEmpty(item.ProductStatus.ToString()))
                                {
                                    html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                }
                                else
                                {
                                    if (item.ProductStatus == 2)
                                        html.Append("            <p class=\"red-text\">Hết hàng</p>");
                                    else
                                        html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                }
                                html.Append("        </div>");
                                //html.Append("        <div class=\"item-status column\">");
                                //html.Append("            <span class=\"black-text\"><strong>Mua hàng</strong></span>");
                                //if (Convert.ToBoolean(item.IsBuy))
                                //{
                                //    html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                //}
                                //else
                                //{
                                //    html.Append("            <p class=\"green-text\">Còn hàng</p>");
                                //}
                                //html.Append("        </div>");
                                html.Append("        <div class=\"delete\">");
                                if (RoleID == 3 || RoleID == 2 || RoleID == 9)
                                {
                                    if (o.Status > 5 || o.Status == 3)
                                    {
                                        pnAnhet.Enabled = false;
                                    }
                                    else
                                    {
                                        pnAnhet.Enabled = true;
                                        html.Append("            <a href=\"/manager/ProductEdit.aspx?id=" + item.ID + "\" class=\"btn-update tooltipped\" data-position=\"top\" data-tooltip=\"Sửa\"><i class=\"material-icons\">edit</i></a>");
                                    }
                                }
                                else if (RoleID == 0)
                                {
                                    if (o.Status > 5 || o.Status == 3)
                                    {

                                    }
                                    else
                                    {
                                        html.Append("            <a href=\"/manager/ProductEdit.aspx?id=" + item.ID + "\" class=\"btn-update tooltipped\" data-position=\"top\" data-tooltip=\"Sửa\"><i class=\"material-icons\">edit</i></a>");
                                    }
                                }

                                html.Append("        </div>");
                                html.Append("    </div>");
                                html.Append("</div>");

                                totalproduct += Convert.ToInt32(item.quantity);

                                //Print
                                //ltrProductPrint.Text += "<tr>";
                                //ltrProductPrint.Text += "<td class=\"pro\">" + item.ID + "</td>";
                                //ltrProductPrint.Text += "<td class=\"pro\">";
                                //ltrProductPrint.Text += "   <div class=\"thumb-product\">";
                                //ltrProductPrint.Text += "       <div class=\"pd-img\"><img src=\"" + item.image_origin + "\" alt=\"\"></div>";
                                //ltrProductPrint.Text += "       <div class=\"info\">" + item.title_origin + "</div>";
                                //ltrProductPrint.Text += "   </div>";
                                //ltrProductPrint.Text += "</td>";
                                //ltrProductPrint.Text += "<td class=\"pro\">" + item.property + "</td>";
                                //ltrProductPrint.Text += "<td class=\"qty\">" + item.quantity + "</td>";

                                //ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">" + string.Format("{0:N0}", vndprice) + " vnđ</p></td>";
                                //ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">¥" + string.Format("{0:0.##}", price) + "</p></td>";

                                //ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">" + item.brand + "</p></td>";
                                //if (string.IsNullOrEmpty(item.ProductStatus.ToString()))
                                //{
                                //    ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">Còn hàng</p></td>";
                                //}
                                //else
                                //{
                                //    if (item.ProductStatus == 2)
                                //        ltrProductPrint.Text += "<td class=\"price\"><p class=\"bg-red\">Hết hàng</p></td>";
                                //    else
                                //        ltrProductPrint.Text += "<td class=\"price\"><p class=\"\">Còn hàng</p></td>";
                                //}
                                //ltrProducts.Text += "</tr>";
                                stt++;
                            }
                            ltrProducts.Text = html.ToString();
                        }
                        ltrTotalProduct.Text = totalproduct.ToString();
                        #endregion
                        #region Lấy bình luận nội bộ
                        StringBuilder chathtml = new StringBuilder();
                        var cs = OrderCommentController.GetByOrderIDAndType(o.ID, 2);
                        if (cs != null)
                        {
                            if (cs.Count > 0)
                            {
                                foreach (var item in cs)
                                {
                                    string fullname = "";
                                    int role = 0;
                                    int user_postID = 0;
                                    var user = AccountController.GetByID(Convert.ToInt32(item.CreatedBy));
                                    if (user != null)
                                    {
                                        user_postID = user.ID;
                                        role = Convert.ToInt32(user.RoleID);
                                        var userinfo = AccountController.GetByID(user.ID);
                                        if (userinfo != null)
                                        {
                                            fullname = userinfo.Username;

                                        }
                                    }

                                    if (uid == user_postID)
                                    {
                                        //ltrInComment.Text += "<div class=\"mess-item mymess\">";
                                        chathtml.Append("<div class=\"chat chat-right\">");
                                    }
                                    else
                                    {
                                        //ltrInComment.Text += "<div class=\"mess-item \">";
                                        chathtml.Append("<div class=\"chat\">");
                                    }
                                    chathtml.Append("<div class=\"chat-avatar\">");
                                    chathtml.Append("    <p class=\"name\">" + fullname + "</p>");
                                    //chathtml.Append("    <p class=\"role\">"+RoleController.GetByID(user.RoleID.Value).RoleName+"</p>");
                                    chathtml.Append("</div>");
                                    chathtml.Append("<div class=\"chat-body\">");
                                    chathtml.Append("        <div class=\"chat-text\">");
                                    chathtml.Append("                <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</div>");
                                    chathtml.Append("                <div class=\"text-content\">");
                                    chathtml.Append("                    <div class=\"content\">");
                                    if (!string.IsNullOrEmpty(item.Link))
                                    {
                                        chathtml.Append("<div class=\"content-img\">");
                                        //if (uid == user_postID)
                                        //{
                                        //    chathtml.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        //    //ltrInComment.Text += "<div class=\"mess-item mymess\">";

                                        //}
                                        //else
                                        //{
                                        //    //ltrInComment.Text += "<div class=\"mess-item \">";
                                        //    chathtml.Append("<div class=\"content-img\">");
                                        //}
                                        chathtml.Append("   <div class=\"img-block\">");
                                        if (item.Link.Contains(".doc"))
                                        {
                                            chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (item.Link.Contains(".xls"))
                                        {
                                            chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title =\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }

                                        //chathtml.Append("       <img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"materialboxed\" height=\"50\"/>");
                                        chathtml.Append("   </div>");
                                        chathtml.Append("</div>");
                                    }
                                    else
                                    {
                                        chathtml.Append("                    <p>" + item.Comment + "</p>");
                                    }
                                    chathtml.Append("                    </div>");
                                    chathtml.Append("                </div>");
                                    chathtml.Append("        </div>");
                                    chathtml.Append("</div>");
                                    chathtml.Append("</div>");
                                }
                            }
                            else
                            {
                                //chathtml.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                            }
                        }
                        else
                        {

                            //chathtml.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                        }
                        ltrInComment.Text = chathtml.ToString();
                        #endregion
                        #region Lấy bình luận ngoài
                        StringBuilder chathtml2 = new StringBuilder();
                        var cs1 = OrderCommentController.GetByOrderIDAndType(o.ID, 1);
                        if (cs1 != null)
                        {
                            if (cs1.Count > 0)
                            {
                                foreach (var item in cs1)
                                {
                                    string fullname = "";

                                    int role = 0;
                                    int user_postID = 0;
                                    var user = AccountController.GetByID(Convert.ToInt32(item.CreatedBy));
                                    if (user != null)
                                    {
                                        user_postID = user.ID;
                                        role = Convert.ToInt32(user.RoleID);
                                        var userinfo = AccountController.GetByID(user.ID);
                                        if (userinfo != null)
                                        {
                                            fullname = userinfo.Username;
                                        }
                                    }
                                    if (uid == user_postID)
                                    {
                                        //ltrOutComment.Text += "<div class=\"mess-item mymess\">";
                                        chathtml2.Append("<div class=\"chat chat-right\">");
                                    }
                                    else
                                    {
                                        //ltrOutComment.Text += "<div class=\"mess-item \">";
                                        chathtml2.Append("<div class=\"chat\">");
                                    }
                                    chathtml2.Append("<div class=\"chat-avatar\">");
                                    chathtml2.Append("    <p class=\"name\">" + fullname + "</p>");
                                    //chathtml2.Append("    <p class=\"role\">" + RoleController.GetByID(user.RoleID.Value).RoleName + "</p>");
                                    chathtml2.Append("</div>");
                                    chathtml2.Append("<div class=\"chat-body\">");
                                    chathtml2.Append("        <div class=\"chat-text\">");
                                    chathtml2.Append("                <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</div>");
                                    chathtml2.Append("                <div class=\"text-content\">");
                                    chathtml2.Append("                    <div class=\"content\">");
                                    if (!string.IsNullOrEmpty(item.Link))
                                    {
                                        chathtml2.Append("<div class=\"content-img\">");
                                        //if (uid == user_postID)
                                        //{
                                        //    chathtml2.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        //    //ltrInComment.Text += "<div class=\"mess-item mymess\">";

                                        //}
                                        //else
                                        //{
                                        //    //ltrInComment.Text += "<div class=\"mess-item \">";
                                        //    chathtml2.Append("<div class=\"content-img\">");
                                        //}
                                        chathtml2.Append("<div class=\"img-block\">");
                                        if (item.Link.Contains(".doc"))
                                        {
                                            chathtml2.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (item.Link.Contains(".xls"))
                                        {
                                            chathtml2.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            chathtml2.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                        }                                        //chathtml2.Append("<img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"materialboxed\" height=\"50\"/>");
                                        chathtml2.Append("</div>");
                                        chathtml2.Append("</div>");
                                    }
                                    else
                                    {
                                        chathtml2.Append("                    <p>" + item.Comment + "</p>");
                                    }
                                    chathtml2.Append("                    </div>");
                                    chathtml2.Append("                </div>");
                                    chathtml2.Append("        </div>");
                                    chathtml2.Append("</div>");
                                    chathtml2.Append("</div>");

                                }
                            }
                            else
                            {
                                //ltrOutComment.Text += "<span class=\"no-comment-cust\">Hiện chưa có đánh giá nào.</span>";
                                //chathtml2.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                            }
                        }
                        else
                        {
                            //ltrOutComment.Text += "<span class=\"no-comment-cust\">Hiện chưa có đánh giá nào.</span>";
                            //chathtml2.Append("<span class=\"no-comment-staff\">Hiện chưa có đánh giá nào.</span>");
                        }
                        ltrOutComment.Text = chathtml2.ToString();
                        #endregion
                        #region Lấy danh sách bao nhỏ
                        StringBuilder spsList = new StringBuilder();
                        var smallpackages = SmallPackageController.GetByMainOrderID(id);
                        if (smallpackages.Count > 0)
                        {
                            foreach (var s in smallpackages)
                            {
                                double canquydoi = 0;
                                if (s.Height > 0 && s.Width > 0 && s.Length > 0)
                                {
                                    canquydoi = Convert.ToDouble(s.Height) * Convert.ToDouble(s.Width) * Convert.ToDouble(s.Length) / 1000000;
                                }
                                int status = Convert.ToInt32(s.Status);
                                ltrMavandon.Text += "<tr>";
                                ltrMavandon.Text += "   <td>" + s.OrderTransactionCode + "</td>";
                                ltrMavandon.Text += "   <td>" + s.Weight + "</td>";
                                ltrMavandon.Text += "   <td>" + Math.Round(canquydoi, 5) + "</td>";
                                if (status == 1)
                                    ltrMavandon.Text += "<td>Chưa về kho TQ</td>";
                                else if (status == 2)
                                    ltrMavandon.Text += "<td>Đã về kho TQ</td>";
                                else if (status == 3)
                                    ltrMavandon.Text += "<td>Đã về kho đích</td>";
                                else if (status == 4)
                                    ltrMavandon.Text += "<td>Đã giao khách hàng</td>";
                                else if (status == 0)
                                    ltrMavandon.Text += "<td>Đã hủy</td>";
                                ltrMavandon.Text += "</tr>";

                                spsList.Append("            <tr class=\"ordercode order-versionnew\" data-packageID=\"" + s.ID + "\">");
                                spsList.Append("                <td>");
                                spsList.Append("                    <input class=\"transactionCode\" type=\"text\" value=\"" + s.OrderTransactionCode + "\"></td>");
                                if (RoleID != 6)
                                {
                                    if (RoleID != 3)
                                    {

                                        spsList.Append("                <td>");
                                        spsList.Append("                    <input class=\"transactionWeight\" onkeyup=\"returnWeightFee()\" data-type=\"text\" type=\"text\" value=\"" + s.Weight + "\"></td>");
                                        spsList.Append("<td style=\"pointer-events: none\"> <input  class=\"transactionWeight\" onkeyup=\"returnWeightFee()\" data-type=\"text\" type=\"text\" value=\"" + Math.Round(canquydoi, 5) + "\"></td>");
                                    }
                                    else
                                    {
                                        spsList.Append("                <td style=\"pointer-events: none;\" >");
                                        spsList.Append("                    <input class=\"transactionWeight\" onkeyup=\"returnWeightFee()\" data-type=\"text\" type=\"text\" value=\"" + s.Weight + "\"></td>");
                                        spsList.Append("<td style=\"pointer-events: none\"> <input class=\"transactionWeight\" onkeyup=\"returnWeightFee()\" data-type=\"text\" type=\"text\" value=\"" + Math.Round(canquydoi, 5) + "\"></td>");
                                    }


                                    spsList.Append("                <td>");
                                    spsList.Append("                    <div class=\"input-field\">");
                                    spsList.Append("                        <select class=\"transactionCodeMainOrderCode\">");

                                    var ListMainOrderCode = MainOrderCodeController.GetAllByMainOrderID(o.ID);
                                    if (ListMainOrderCode != null)
                                    {

                                        var mainOrderCode = MainOrderCodeController.GetByID(Convert.ToInt32(s.MainOrderCodeID));
                                        if (mainOrderCode != null)
                                        {
                                            spsList.Append("            <option value=\"0\">Chọn mã đơn hàng</option>");
                                            foreach (var item in ListMainOrderCode)
                                            {
                                                if (mainOrderCode.MainOrderCode == item.MainOrderCode)
                                                {
                                                    spsList.Append("            <option value=\"" + item.ID + "\" selected>" + item.MainOrderCode + "</option>");
                                                }
                                                else
                                                {
                                                    spsList.Append("            <option value=\"" + item.ID + "\">" + item.MainOrderCode + "</option>");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            spsList.Append("            <option value=\"0\">Chọn mã đơn hàng</option>");
                                            foreach (var item in ListMainOrderCode)
                                            {
                                                spsList.Append("            <option value=\"" + item.ID + "\">" + item.MainOrderCode + "</option>");
                                            }
                                        }

                                    }
                                    else
                                    {
                                        spsList.Append("            <option value=\"0\">Chọn mã đơn hàng</option>");
                                    }

                                    spsList.Append("                        </select>");
                                    spsList.Append("                    </div>");
                                    spsList.Append("                </td>");
                                    if (RoleID != 3)
                                    {
                                        spsList.Append("                <td>");
                                        spsList.Append("                    <div class=\"input-field\">");
                                        spsList.Append("                        <select class=\"transactionCodeStatus\">");
                                        if (status == 1)
                                            spsList.Append("            <option value=\"1\" selected>Chưa về kho TQ</option>");
                                        else
                                            spsList.Append("            <option value=\"1\">Chưa về kho TQ</option>");
                                        if (status == 2)
                                            spsList.Append("            <option value=\"2\" selected>Đã về kho TQ</option>");
                                        else
                                            spsList.Append("            <option value=\"2\">Đã về kho TQ</option>");
                                        if (status == 5)
                                            spsList.Append("            <option value=\"5\" selected>Đang về kho đích</option>");
                                        else
                                            spsList.Append("            <option value=\"5\">Đang về kho đích</option>");
                                        if (status == 3)
                                            spsList.Append("            <option value=\"3\" selected>Đã về kho đích</option>");
                                        else
                                            spsList.Append("            <option value=\"3\">Đã về kho đích</option>");
                                        if (status == 4)
                                            spsList.Append("            <option value=\"4\" selected>Đã thanh toán</option>");
                                        else
                                            spsList.Append("            <option value=\"4\">Đã thanh toán</option>");
                                        if (status == 6)
                                            spsList.Append("            <option value=\"4\" selected>Đã giao khách hàng</option>");
                                        else
                                            spsList.Append("            <option value=\"4\">Đã giao khách hàng</option>");
                                        if (status == 0)
                                            spsList.Append("            <option value=\"0\" selected>Đã hủy</option>");
                                        else
                                            spsList.Append("            <option value=\"0\">Đã hủy</option>");

                                        spsList.Append("                        </select>");
                                        spsList.Append("                    </div>");
                                        spsList.Append("                </td>");
                                    }
                                    else
                                    {
                                        spsList.Append("                <td style=\"pointer-events: none;\">");
                                        spsList.Append("                    <div class=\"input-field\">");
                                        spsList.Append("                        <select class=\"transactionCodeStatus\">");
                                        if (status == 1)
                                            spsList.Append("            <option value=\"1\" selected>Chưa về kho TQ</option>");
                                        else
                                            spsList.Append("            <option value=\"1\">Chưa về kho TQ</option>");
                                        if (status == 2)
                                            spsList.Append("            <option value=\"2\" selected>Đã về kho TQ</option>");
                                        else
                                            spsList.Append("            <option value=\"2\">Đã về kho TQ</option>");
                                        if (status == 5)
                                            spsList.Append("            <option value=\"5\" selected>Đang về kho đích</option>");
                                        else
                                            spsList.Append("            <option value=\"5\">Đang về kho đích</option>");
                                        if (status == 3)
                                            spsList.Append("            <option value=\"3\" selected>Đã về kho đích</option>");
                                        else
                                            spsList.Append("            <option value=\"3\">Đã về kho đích</option>");
                                        if (status == 4)
                                            spsList.Append("            <option value=\"4\" selected>Đã thanh toán</option>");
                                        else
                                            spsList.Append("            <option value=\"4\">Đã thanh toán</option>");
                                        if (status == 6)
                                            spsList.Append("            <option value=\"4\" selected>Đã giao khách hàng</option>");
                                        else
                                            spsList.Append("            <option value=\"4\">Đã giao khách hàng</option>");
                                        if (status == 0)
                                            spsList.Append("            <option value=\"0\" selected>Đã hủy</option>");
                                        else
                                            spsList.Append("            <option value=\"0\">Đã hủy</option>");

                                        spsList.Append("                        </select>");
                                        spsList.Append("                    </div>");
                                        spsList.Append("                </td>");
                                    }


                                    spsList.Append("                <td>");
                                    spsList.Append("                    <input class=\"transactionDescription\" type=\"text\" value=\"" + s.Description + "\"></td>");
                                    spsList.Append("                </td>");
                                    spsList.Append("            <td class=\"\">");
                                    spsList.Append("                <a href='javascript:;' onclick=\"deleteOrderCode($(this))\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons valign-center\">remove_circle</i></a>");
                                    spsList.Append("            </td>");
                                }
                                spsList.Append("            </tr>");
                            }
                            ltrCodeList.Text = spsList.ToString();
                        }
                        #endregion
                        #region Lấy danh sách phụ phí
                        var listsp = FeeSupportController.GetAllByMainOrderID(o.ID);
                        if (listsp.Count > 0)
                        {
                            foreach (var item in listsp)
                            {
                                ltrFeeSupport.Text += "<tr class=\"feesupport fee-versionnew\" data-feesupportid=\"" + item.ID + "\">";
                                ltrFeeSupport.Text += "<td><input class=\"feesupportname\" type=\"text\" value=\"" + item.SupportName + "\"></td>";
                                ltrFeeSupport.Text += "<td><input class=\"feesupportvnd\" type=\"text\" value=\"" + item.SupportInfoVND + "\"></td>";
                                ltrFeeSupport.Text += "<td class=\"\"><a href=\"javascript:;\" onclick=\"deleteSupportFee($(this))\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons valign-center\">remove_circle</i></a></td>";
                                ltrFeeSupport.Text += "</tr>";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Response.Redirect("/trang-chu");
                    }
                    StringBuilder hisChange = new StringBuilder();
                    var historyorder = HistoryOrderChangeController.GetByMainOrderID(o.ID);
                    if (historyorder.Count > 0)
                    {
                        foreach (var item in historyorder)
                        {
                            string username = item.Username;
                            string rolename = "admin";
                            var acc = AccountController.GetByUsername(username);
                            if (acc != null)
                            {
                                int role = Convert.ToInt32(acc.RoleID);

                                var r = RoleController.GetByID(role);
                                if (r != null)
                                {
                                    rolename = r.RoleDescription;
                                }
                            }
                            hisChange.Append("<tr>");
                            hisChange.Append("    <td class=\"no-wrap\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</td>");
                            hisChange.Append("    <td class=\"no-wrap\">" + username + "</td>");
                            hisChange.Append("    <td class=\"no-wrap\">" + rolename + "</td>");
                            hisChange.Append("    <td>" + item.HistoryContent + "</td>");
                            hisChange.Append("</tr>");
                        }

                    }
                    else
                    {
                        hisChange.Append("<tr class=\"noti\">");
                        hisChange.Append("    <td class=\"red-text\" colspan=\"4\">Không có lịch sử thay đổi nào.</td>");
                        hisChange.Append("</tr>");
                    }
                    //lrtHistoryChange.Text = hisChange.ToString();

                }

            }


        }

        #region Button
        protected void btnSend1_Click(object sender, EventArgs e)
        {
            var orderID = hdfOrderID.Value.ToString();
            //var comment = txtComment1.Text;
            //sendcustomercomment(comment, orderID.ToInt(0));
            //if (!Page.IsValid) return;
            //string username = Session["userLoginSystem"].ToString();
            //var obj_user = AccountController.GetByUsername(username);
            //DateTime currentDate = DateTime.Now;
            //if (obj_user != null)
            //{
            //    int uid = obj_user.ID;
            //    //var id = Convert.ToInt32(Request.QueryString["id"]);
            //    var id = Convert.ToInt32(ViewState["ID"]);
            //    if (id > 0)
            //    {
            //        var o = MainOrderController.GetAllByID(id);
            //        if (o != null)
            //        {
            //            int type = 1;
            //            if (type > 0)
            //            {
            //                //txtComment1.Text
            //                string kq = OrderCommentController.Insert(id, txtComment1.Text, true, type, DateTime.Now, uid);
            //                if (type == 1)
            //                {
            //                    NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
            //                        AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
            //                        1, currentDate, obj_user.Username, true);
            //                    try
            //                    {
            //                        PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net",
            //                            "jrbhnznozmlrmwvy",
            //                            AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
            //                            "Thông báo tại NHAPSICHINA.COM.",
            //                            "Đã có đánh giá mới cho đơn hàng #" + id
            //                            + " của bạn. CLick vào để xem", "");
            //                    }
            //                    catch { }
            //                }
            //                if (Convert.ToInt32(kq) > 0)
            //                {
            //                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //                    hubContext.Clients.All.addNewMessageToPage("", "");
            //                    PJUtils.ShowMsg("Gửi đánh giá thành công.", true, Page);
            //                }
            //            }
            //            else
            //            {
            //                PJUtils.ShowMessageBoxSwAlert("Vui lòng chọn khu vực", "e", false, Page);
            //            }
            //        }
            //    }
            //}
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            var orderID = hdfOrderID.Value.ToString();
            //var comment = txtComment.Text;
            //sendstaffcomment(comment, orderID.ToInt(0));
            //if (!Page.IsValid) return;
            //string username = Session["userLoginSystem"].ToString();
            //var obj_user = AccountController.GetByUsername(username);
            //DateTime currentDate = DateTime.Now;
            //if (obj_user != null)
            //{
            //    int uid = obj_user.ID;
            //    //var id = Convert.ToInt32(Request.QueryString["id"]);
            //    var id = Convert.ToInt32(ViewState["ID"]);
            //    if (id > 0)
            //    {
            //        var o = MainOrderController.GetAllByID(id);
            //        if (o != null)
            //        {
            //            int type = 2;
            //            if (type > 0)
            //            {
            //                //txtComment.Text
            //                string kq = OrderCommentController.Insert(id, txtComment.Text, true, type, DateTime.Now, uid);
            //                if (type == 1)
            //                {
            //                    NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(o.UID),
            //                        AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0,
            //                        1, currentDate, obj_user.Username, false);
            //                    try
            //                    {
            //                        PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy",
            //                            AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
            //                            "Thông báo tại NHAPSICHINA.COM.",
            //                            "Đã có đánh giá mới cho đơn hàng #" + id
            //                            + " của bạn. CLick vào để xem", "");
            //                    }
            //                    catch { }
            //                }
            //                if (Convert.ToInt32(kq) > 0)
            //                {
            //                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //                    hubContext.Clients.All.addNewMessageToPage("", "");
            //                    PJUtils.ShowMsg("Gửi đánh giá thành công.", true, Page);
            //                }
            //            }
            //            else
            //            {
            //                PJUtils.ShowMessageBoxSwAlert("Vui lòng chọn khu vực", "e", false, Page);
            //            }
            //        }
            //    }
            //}
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            bool checkMVD = false;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int uid = obj_user.ID;
                int RoleID = obj_user.RoleID.ToString().ToInt();              
                var id = Convert.ToInt32(ViewState["ID"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {
                        int uidmuahang = Convert.ToInt32(o.UID);
                        string usermuahang = "";

                        var accmuahan = AccountController.GetByID(uidmuahang);
                        usermuahang = accmuahan.Username;

                        string CurrentOrderWeight = o.OrderWeight;
                        bool ischeckmvd = true;
                        string listmvd_ne = "";

                        #region cập nhật và tạo mới smallpackage
                        string tcl = hdfCodeTransactionList.Value;
                        string listmvd = hdfCodeTransactionListMVD.Value;
                        if (!string.IsNullOrEmpty(tcl))
                        {
                            checkMVD = true;
                            string[] list = tcl.Split('|');
                            for (int i = 0; i < list.Length - 1; i++)
                            {
                                string[] item = list[i].Split(',');
                                int ID = item[0].ToInt(0);
                                string code = item[1].Trim();
                                string weight = item[2];
                                double weightin = 0;
                                if (!string.IsNullOrEmpty(weight))
                                    weightin = Math.Round(Convert.ToDouble(weight), 1);
                                int smallpackage_status = item[3].ToInt(1);
                                string description = item[4];
                                string mainOrderCodeID = item[5];
                                var MainOrderCode = MainOrderCodeController.GetByID(mainOrderCodeID.ToInt(0));
                                if (MainOrderCode == null)
                                    PJUtils.ShowMessageBoxSwAlert("Lỗi, không có mã đơn hàng", "e", false, Page);
                                if (ID > 0)
                                {
                                    var smp = SmallPackageController.GetByID(ID);
                                    if (smp != null)
                                    {
                                        int bigpackageID = Convert.ToInt32(smp.BigPackageID);
                                        bool check = false;
                                        var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                        if (getsmallcheck.Count > 0)
                                        {
                                            foreach (var sp in getsmallcheck)
                                            {
                                                if (sp.ID == ID)
                                                {
                                                    check = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            check = true;
                                        }
                                        if (check)
                                        {
                                            SmallPackageController.UpdateNew(ID, accmuahan.ID, usermuahang, bigpackageID, code,
                                                smp.ProductType, Math.Round(Convert.ToDouble(smp.FeeShip), 0),
                                            weightin, Math.Round(Convert.ToDouble(smp.Volume), 1), smallpackage_status,
                                            description, currentDate, username, mainOrderCodeID.ToInt(0));

                                            if (smallpackage_status == 2)
                                            {
                                                SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                            }
                                            else if (smallpackage_status == 3)
                                            {
                                                SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                            }
                                            var bigpack = BigPackageController.GetByID(bigpackageID);
                                            if (bigpack != null)
                                            {
                                                int TotalPackageWaiting = SmallPackageController.GetCountByBigPackageIDStatus(bigpackageID, 1, 2);
                                                if (TotalPackageWaiting == 0)
                                                {
                                                    BigPackageController.UpdateStatus(bigpackageID, 2, currentDate, username);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                        //if (getsmallcheck.Count > 0)
                                        //{
                                        //    PJUtils.ShowMessageBoxSwAlert("Mã kiện đã tồn tại, vui lòng tạo mã khác", "e", true, Page);
                                        //}
                                        //else
                                        //{
                                        var checkbarcode = SmallPackageController.GetByOrderTransactionCode(code);
                                        if (checkbarcode == null)
                                        {
                                            SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(id, accmuahan.ID, usermuahang,
                                            0, code, "", 0, weightin, 0,
                                        smallpackage_status, description, currentDate, username, mainOrderCodeID.ToInt(0), 0);


                                            var quantitymvd1 = SmallPackageController.GetByMainOrderID(id);
                                            if (quantitymvd1.Count > 0)
                                            {
                                                if (quantitymvd1 != null)
                                                {
                                                    MainOrderController.UpdateListMVD(id, listmvd, quantitymvd1.Count);
                                                }
                                            }

                                            HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                     " đã thêm mã vận đơn của đơn hàng ID là: " + o.ID + ", Mã vận đơn: " + code + ", cân nặng: " + weightin + "", 8, currentDate);

                                            if (smallpackage_status == 2)
                                            {
                                                SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                            }
                                            else if (smallpackage_status == 3)
                                            {
                                                SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                            }
                                            //}
                                        }
                                        else
                                        {
                                            ischeckmvd = false;
                                            listmvd_ne += code + " - ";
                                        }


                                    }
                                }
                                else
                                {
                                    //bool check = false;
                                    //var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                                    //if (getsmallcheck.Count > 0)
                                    //{
                                    //    PJUtils.ShowMessageBoxSwAlert("Mã kiện đã tồn tại, vui lòng tạo mã khác", "e", true, Page);
                                    //}
                                    //else
                                    //{
                                    var checkbarcode = SmallPackageController.GetByOrderTransactionCode(code);
                                    if (checkbarcode == null)
                                    {
                                        SmallPackageController.InsertWithMainOrderIDUIDUsernameNew(id, accmuahan.ID, usermuahang, 0,
                                        code, "", 0, weightin, 0,
                                    smallpackage_status, description, currentDate, username, mainOrderCodeID.ToInt(0), 0);

                                        var quantitymvd2 = SmallPackageController.GetByMainOrderID(id);
                                        if (quantitymvd2.Count > 0)
                                        {
                                            if (quantitymvd2 != null)
                                            {
                                                MainOrderController.UpdateListMVD(id, listmvd, quantitymvd2.Count);
                                            }
                                        }



                                        if (smallpackage_status == 2)
                                        {
                                            SmallPackageController.UpdateDateInTQWareHouse(ID, username, currentDate);
                                        }
                                        else if (smallpackage_status == 3)
                                        {
                                            SmallPackageController.UpdateDateInVNWareHouse(ID, username, currentDate);
                                        }
                                        //}
                                    }
                                    else
                                    {
                                        ischeckmvd = false;
                                        listmvd_ne += code + " - ";
                                    }

                                }
                            }
                        }                        
                        #endregion
                        if (ischeckmvd)
                        {
                            #region Cập nhật và tạo mới phụ phí
                            double TotalFeeSupport = 0;
                            string lsp = hdfListFeeSupport.Value;
                            if (!string.IsNullOrEmpty(lsp))
                            {
                                string[] list = lsp.Split('|');
                                for (int i = 0; i < list.Length - 1; i++)
                                {
                                    string[] item = list[i].Split(',');
                                    int ID = item[0].ToInt(0);
                                    string fname = item[1];
                                    double FeeSupport = Convert.ToDouble(item[2]);
                                    TotalFeeSupport += FeeSupport;
                                    if (ID > 0)
                                    {
                                        var check = FeeSupportController.GetByID(ID);
                                        if (check != null)
                                        {
                                            FeeSupportController.Update(check.ID, fname, FeeSupport.ToString(), obj_user.Username, currentDate);
                                            if (check.SupportName != fname)
                                            {
                                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                          " đã thay đổi tên phụ phí của đơn hàng ID là: " + o.ID + ", Từ: " + check.SupportName + ", Sang: "
                                          + fname + "", 10, currentDate);
                                            }

                                            if (check.SupportInfoVND != FeeSupport.ToString())
                                            {
                                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                          " đã thay đổi tiền phụ phí của đơn hàng ID là: " + o.ID + ", Tên phụ phí: " + fname + ", Số tiền từ: "
                                          + string.Format("{0:N0}", Convert.ToDouble(check.SupportInfoVND)) + ", Sang: "
                                          + string.Format("{0:N0}", Convert.ToDouble(FeeSupport)) + "", 10, currentDate);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        FeeSupportController.Insert(o.ID, fname, FeeSupport.ToString(), obj_user.Username, currentDate);
                                        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                           " đã thêm phụ phí của đơn hàng ID là: " + o.ID + ", Tên phụ phí: " + fname + ", Số tiền: "
                                           + string.Format("{0:N0}", Convert.ToDouble(FeeSupport)) + "", 10, currentDate);

                                    }
                                }
                            }
                            #endregion
                            #region Lấy ra text của trạng thái đơn hàng
                            string orderstatus = "";
                            int currentOrderStatus = Convert.ToInt32(o.Status);
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
                            #endregion
                            #region Cập nhật nhân viên KhoTQ và nhân viên KhoVN
                            if (RoleID == 4)
                            {
                                if (o.KhoTQID == uid || o.KhoTQID == 0)
                                {
                                    MainOrderController.UpdateStaff(o.ID, o.SalerID.ToString().ToInt(0), o.DathangID.ToString().ToInt(0), uid, o.KhoVNID.ToString().ToInt(0));
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý", "e", true, Page);
                                }
                            }
                            else if (RoleID == 5)
                            {
                                if (o.KhoVNID == uid || o.KhoTQID == 0)
                                {
                                    MainOrderController.UpdateStaff(o.ID, o.SalerID.ToString().ToInt(0), o.DathangID.ToString().ToInt(0), o.KhoTQID.ToString().ToInt(0), uid);
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý", "e", true, Page);
                                }
                            }
                            #endregion
                            #region cập nhật thông tin của đơn hàng
                            double feeeinwarehouse = 0;
                            int status = ddlStatus.SelectedValue.ToString().ToInt(0);
                            if (status == 1)
                            {
                                if (RoleID == 0 || RoleID == 2 || RoleID == 9)
                                {

                                    MainOrderController.UpdateStatusByID(o.ID, 1);
                                    double Deposit = 0;
                                    if (o.Deposit.ToFloat(0) > 0)
                                        Deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);
                                    if (Deposit > 0)
                                    {
                                        var user_order = AccountController.GetByID(o.UID.ToString().ToInt());
                                        if (user_order != null)
                                        {
                                            double wallet = 0;
                                            if (user_order.Wallet.ToString().ToFloat(0) > 0)
                                                wallet = Math.Round(Convert.ToDouble(user_order.Wallet), 0);
                                            wallet = wallet + Deposit;
                                            HistoryPayWalletController.Insert(user_order.ID, user_order.Username, o.ID, Deposit,
                                            "Đơn hàng: " + o.ID + " bị hủy và hoàn tiền cọc cho khách.", wallet, 2, 2, currentDate, obj_user.Username);                                           
                                            AccountController.updateWallet(user_order.ID, wallet, currentDate, obj_user.Username);
                                            MainOrderController.UpdateDeposit(o.ID, "0");
                                            PayOrderHistoryController.Insert(o.ID, user_order.ID, 4, Deposit, 2, currentDate, obj_user.Username);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                double OCurrent_deposit = 0;
                                if (o.Deposit.ToFloat(0) > 0)
                                    OCurrent_deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);

                                double OCurrent_FeeShipCN = 0;
                                if (o.FeeShipCN.ToFloat(0) > 0)
                                    OCurrent_FeeShipCN = Math.Round(Convert.ToDouble(o.FeeShipCN), 2);

                                double OCurrent_FeeBuyPro = 0;
                                if (o.FeeBuyPro.ToFloat(0) > 0)
                                    OCurrent_FeeBuyPro = Math.Round(Convert.ToDouble(o.FeeBuyPro), 0);

                                double OCurrent_FeeWeight = 0;
                                if (o.FeeWeight.ToFloat(0) > 0)
                                    OCurrent_FeeWeight = Math.Round(Convert.ToDouble(o.FeeWeight), 0);

                                double OCurrent_IsCheckProductPrice = 0;
                                if (o.IsCheckProductPrice.ToFloat(0) > 0)
                                    OCurrent_IsCheckProductPrice = Math.Round(Convert.ToDouble(o.IsCheckProductPrice), 0);

                                double OCurrent_IsPackedPrice = 0;
                                if (o.IsPackedPrice.ToFloat(0) > 0)
                                    OCurrent_IsPackedPrice = Math.Round(Convert.ToDouble(o.IsPackedPrice), 0);

                                double OCurrent_IsSpecial = 0;
                                if (o.IsCheckPriceSpecial.ToFloat(0) > 0)
                                    OCurrent_IsSpecial = Math.Round(Convert.ToDouble(o.IsCheckPriceSpecial), 0);

                                double OCurrent_IsFastDeliveryPrice = 0;
                                if (o.IsFastDeliveryPrice.ToFloat(0) > 0)
                                    OCurrent_IsFastDeliveryPrice = Math.Round(Convert.ToDouble(o.IsFastDeliveryPrice), 0);

                                double OCurrent_TotalPriceReal = 0;
                                if (o.TotalPriceReal.ToFloat(0) > 0)
                                    OCurrent_TotalPriceReal = Math.Round(Convert.ToDouble(o.TotalPriceReal), 0);

                                double OCurrent_TotalPriceRealCYN = 0;
                                if (o.TotalPriceRealCYN.ToFloat(0) > 0)
                                    OCurrent_TotalPriceRealCYN = Math.Round(Convert.ToDouble(o.TotalPriceRealCYN), 2);                                

                                double Deposit = Math.Round(Convert.ToDouble(pDeposit.Value), 0);
                                double FeeShipCN = Math.Round(Convert.ToDouble(pCNShipFee.Text), 0);
                                double FeeShipCNCYN = Math.Round(Convert.ToDouble(pCNShipFeeNDT.Text), 2);
                                double FeeShipCNReal = Math.Round(Convert.ToDouble(pCNShipFeeReal.Text), 0);
                                double FeeShipCNRealCYN = Math.Round(Convert.ToDouble(pCNShipFeeNDTReal.Text), 2);
                                double FeeBuyPro = Math.Round(Convert.ToDouble(pBuy.Text), 0);
                                double FeeWeight = Math.Round(Convert.ToDouble(pWeight.Text), 0);
                                double TotalPriceReal = Math.Round(Convert.ToDouble(rTotalPriceReal.Text), 0);
                                double TotalPriceRealCYN = Math.Round(Convert.ToDouble(rTotalPriceRealCYN.Text), 2);

                                if (o.FeeInWareHouse != null)
                                    feeeinwarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);                                

                                double IsCheckProductPrice = 0;
                                if (pCheck.Text.ToString().ToFloat(0) > 0)
                                    IsCheckProductPrice = Math.Round(Convert.ToDouble(pCheck.Text), 0);

                                double IsCheckProductPriceCYN = 0;
                                if (pCheckNDT.Text.ToString().ToFloat(0) > 0)
                                    IsCheckProductPriceCYN = Math.Round(Convert.ToDouble(pCheckNDT.Text), 0);
                              

                                double IsPackedPrice = 0;
                                if (pPacked.Text.ToString().ToFloat(0) > 0)
                                    IsPackedPrice = Math.Round(Convert.ToDouble(pPacked.Text), 0);

                                double IsPriceSepcial = 0;
                                if (txtFeeSpecial.Text.ToString().ToFloat(0) > 0)
                                    IsPriceSepcial = Math.Round(Convert.ToDouble(txtFeeSpecial.Text), 0);

                                double IsPackedPriceCYN = 0;
                                if (pPackedNDT.Text.ToString().ToFloat(0) > 0)
                                    IsPackedPriceCYN = Math.Round(Convert.ToDouble(pPackedNDT.Text), 0);                                

                                double IsFastDeliveryPrice = 0;
                                if (pShipHome.Text.ToString().ToFloat(0) > 0)
                                    IsFastDeliveryPrice = Math.Round(Convert.ToDouble(pShipHome.Text), 0);                            


                                #region Ghi lịch sử chỉnh sửa các loại giá
                                if (obj_user.ID == 1)
                                {
                                    if (OCurrent_deposit != Deposit)
                                    {
                                        HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                        " đã đổi tiền đặt cọc từ: " + string.Format("{0:N0}", OCurrent_deposit) + ", sang: "
                                         + string.Format("{0:N0}", Deposit) + "", 1, currentDate);

                                        MainOrderController.UpdateDeposit(o.ID, Deposit.ToString());
                                    }
                                }   
                                if (OCurrent_FeeShipCN != FeeShipCN)
                                {                                   
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí ship Trung Quốc từ " + string.Format("{0:N0}", OCurrent_FeeShipCN) + " sang "
                                            + string.Format("{0:N0}", FeeShipCN) + "", 2, currentDate);
                                }
                                if (OCurrent_FeeBuyPro < FeeBuyPro || OCurrent_FeeBuyPro > FeeBuyPro)
                                {                                   
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi tiền phí mua hàng từ: " + string.Format("{0:N0}", OCurrent_FeeBuyPro) + " sang: "
                                    + string.Format("{0:N0}", FeeBuyPro) + "", 3, currentDate);
                                }
                                if (OCurrent_TotalPriceReal < TotalPriceReal || OCurrent_TotalPriceReal > TotalPriceReal)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí mua thật từ: " + string.Format("{0:N0}", OCurrent_TotalPriceReal) + " sang: "
                                            + string.Format("{0:N0}", TotalPriceReal) + "", 3, currentDate);
                                }
                                if (OCurrent_FeeWeight != FeeWeight)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí TQ-VN từ: " + string.Format("{0:N0}", OCurrent_FeeWeight) + " sang: "
                                            + string.Format("{0:N0}", FeeWeight) + "", 4, currentDate);
                                }
                                if (OCurrent_IsCheckProductPrice != IsCheckProductPrice)
                                {
                                    
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí kiểm tra sản phẩm từ: " + string.Format("{0:N0}", OCurrent_IsCheckProductPrice) + " sang: "
                                            + string.Format("{0:N0}", IsCheckProductPrice) + "", 5, currentDate);
                                }
                                if (OCurrent_IsPackedPrice != IsPackedPrice)
                                {
                                    
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí đóng gỗ của đơn hàng ID là: " + o.ID + ", từ: " + string.Format("{0:N0}", OCurrent_IsPackedPrice) + " sang: "
                                            + string.Format("{0:N0}", IsPackedPrice) + "", 6, currentDate);
                                }

                                if (OCurrent_IsSpecial != IsPriceSepcial)
                                {
                                     HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí đặc biệt từ: " + string.Format("{0:N0}", OCurrent_IsSpecial) + " sang: "
                                            + string.Format("{0:N0}", IsPriceSepcial) + "", 6, currentDate);
                                }

                                if (OCurrent_IsFastDeliveryPrice != IsFastDeliveryPrice)
                                {
                                   
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi tiền phí ship giao hàng tận nhà từ: " + string.Format("{0:N0}", OCurrent_IsFastDeliveryPrice) + " sang: "
                                            + string.Format("{0:N0}", IsFastDeliveryPrice) + "", 7, currentDate);
                                }                               
                                #endregion                               

                                double pricenvd = 0;
                                if (o.PriceVND.ToFloat(0) > 0)
                                    pricenvd = Math.Round(Convert.ToDouble(o.PriceVND), 0);

                                var conf = ConfigurationController.GetByTop1();
                                double cannangdonggo = 0;
                                double cannangdacbiet = 0;
                                if (!string.IsNullOrEmpty(o.TongCanNang))
                                {
                                    cannangdonggo = Convert.ToDouble(o.TongCanNang);
                                    cannangdacbiet = Convert.ToDouble(o.TongCanNang);
                                }
                                if (o.IsPacked == true)
                                {
                                    if (cannangdonggo > 0)
                                    {
                                        if (cannangdonggo <= 1)
                                        {
                                            IsPackedPrice = Convert.ToDouble(conf.FeeDongGoKgDau);
                                        }
                                        else
                                        {
                                            cannangdonggo = cannangdonggo - 1;
                                            IsPackedPrice = Convert.ToDouble(conf.FeeDongGoKgDau) + (cannangdonggo * Convert.ToDouble(conf.FeeDongGoKgSau));
                                        }
                                    }
                                }

                                if (o.IsCheckSpecial1 == true)
                                {
                                    if (cannangdacbiet > 0)
                                    {
                                        IsPriceSepcial = (cannangdacbiet * Convert.ToDouble(conf.FeeDacBiet1));
                                    }
                                }
                                if (o.IsCheckSpecial2 == true)
                                {
                                    if (cannangdacbiet > 0)
                                    {
                                        IsPriceSepcial = (cannangdacbiet * Convert.ToDouble(conf.FeeDacBiet2));
                                    }
                                }
                                if (o.IsCheckSpecial1 == true && o.IsCheckSpecial2 == true)
                                {
                                    if (cannangdacbiet > 0)
                                    {
                                        IsPriceSepcial = (cannangdacbiet * (Convert.ToDouble(conf.FeeDacBiet1) + Convert.ToDouble(conf.FeeDacBiet2)));
                                    }
                                }

                                IsPriceSepcial = Math.Round(IsPriceSepcial, 0);
                                IsPackedPrice = Math.Round(IsPackedPrice, 0);
                                double TotalPriceVND = FeeShipCN + FeeBuyPro + FeeWeight + IsCheckProductPrice + IsPackedPrice
                                                           + IsFastDeliveryPrice + pricenvd + TotalFeeSupport + IsPriceSepcial;
                                TotalPriceVND = Math.Round(TotalPriceVND, 0);
                               
                                MainOrderController.UpdateFee_OrderDetail(o.ID, Deposit.ToString(), FeeShipCN.ToString(), FeeBuyPro.ToString(), FeeWeight.ToString(), IsCheckProductPrice.ToString(),
                                IsPackedPrice.ToString(), IsFastDeliveryPrice.ToString(), TotalPriceVND.ToString(), FeeShipCNReal.ToString(), IsPriceSepcial.ToString());
                                MainOrderController.UpdateCYN(o.ID, FeeShipCNRealCYN.ToString(), FeeShipCNCYN.ToString(), IsCheckProductPriceCYN.ToString(), IsPackedPriceCYN.ToString());
                                
                            }                           
                            string OrderWeight = txtOrderWeight.Text.ToString();
                            OrderWeight = Math.Round(Convert.ToDouble(OrderWeight), 1).ToString();
                            if (string.IsNullOrEmpty(CurrentOrderWeight))
                            {
                                if (CurrentOrderWeight != OrderWeight)
                                {                                    
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi cân nặng là: " + OrderWeight + "", 8, currentDate);
                                }
                            }
                            else
                            {
                                if (CurrentOrderWeight != OrderWeight)
                                {                                   
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi cân nặng từ: " + CurrentOrderWeight + ", sang: " + OrderWeight + "", 9, currentDate);
                                }
                            }                           

                            string CurrentReceivePlace = o.ReceivePlace;
                            string ReceivePlace = ddlReceivePlace.SelectedValue;

                            if (CurrentReceivePlace != ReceivePlace)
                            {
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                " đã thay đổi kho nhận hàng từ: " + CurrentReceivePlace + ", sang: " + ReceivePlace + "", 8, currentDate);
                            }

                            string CurrentShippingType = o.ShippingType.ToString();
                            string ShippingType = ddlShippingType.SelectedValue;
                            string CurrentLine = "";
                            string NewLine = "";
                            int newline = Convert.ToInt32(ddlShippingType.SelectedValue);
                            int line = Convert.ToInt32(o.ShippingType);
                            if (line > 0)
                            {
                                var shipping = ShippingTypeToWareHouseController.GetByID(line);
                                if (shipping != null)
                                {
                                    CurrentLine = shipping.ShippingTypeName;
                                }
                            }                           
                            if (newline > 0)
                            {
                                var shipping = ShippingTypeToWareHouseController.GetByID(newline);
                                if (shipping != null)
                                {
                                    NewLine = shipping.ShippingTypeName;
                                }
                            }

                            if (CurrentShippingType != ShippingType)
                            {
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username + " đã thay đổi Line từ: " + CurrentLine + " sang: " + NewLine + "", 8, currentDate);
                            }

                            string CurrentAmountDeposit = o.AmountDeposit.Trim();
                            CurrentAmountDeposit = Math.Round(Convert.ToDouble(CurrentAmountDeposit), 0).ToString();
                            string AmountDeposit = pAmountDeposit.Value.ToString().Trim();
                            AmountDeposit = Math.Round(Convert.ToDouble(AmountDeposit), 0).ToString();

                            if (obj_user.ID == 1)
                            {
                                if (CurrentAmountDeposit != AmountDeposit)
                                {
                                    HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    " đã đổi số tiền phải cọc từ: " + CurrentAmountDeposit + ", sang: " + AmountDeposit + "", 8, currentDate);

                                    MainOrderController.UpdateAmountDeposit(o.ID, AmountDeposit);
                                }
                            }    
                           
                          
                            bool Currentcheckpro = new bool();                           
                            bool checkpro = new bool();
                            bool Package = new bool();
                            bool MoveIsFastDelivery = new bool();
                            bool baogia = new bool();
                            bool smallPackage = new bool();
                            bool ycg = new bool();
                            bool baohiem = new bool();
                            bool orderdone = new bool();
                            bool orderPrice = new bool();
                            bool special1 = new bool();
                            bool special2 = new bool();

                            var listCheck = hdfListCheckBox.Value.Split('|').ToList();
                            foreach (var item in listCheck)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    var ck = item.Split(',').ToList();

                                    if (ck != null)
                                    {
                                        if (ck[0] == "1")
                                        {
                                            smallPackage = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "2")
                                        {
                                            baogia = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "3")
                                        {
                                            checkpro = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "4")
                                        {
                                            Package = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "5")
                                        {
                                            MoveIsFastDelivery = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "6")
                                        {
                                            ycg = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "7")
                                        {
                                            baohiem = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "8")
                                        {
                                            orderdone = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "9")
                                        {
                                            orderPrice = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "10")
                                        {
                                            special1 = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                        if (ck[0] == "11")
                                        {
                                            special2 = Convert.ToBoolean(ck[1].ToInt(0));
                                        }
                                    }
                                }
                            }

                            if (Currentcheckpro != checkpro)
                            {
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                           " đã đổi dịch vụ kiểm tra đơn hàng từ: " + ConvertBoolHistory(Currentcheckpro, "kiểm tra đơn hàng") + " sang: " + ConvertBoolHistory(checkpro, "kiểm tra đơn hàng") + "",
                                           8, currentDate);
                            }
                            bool CurrentPackage = o.IsPacked.ToString().ToBool();

                            if (CurrentPackage != Package)
                            {
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                           " đã đổi dịch vụ đóng gỗ từ: " + ConvertBoolHistory(CurrentPackage, "đóng gỗ") + " sang: " + ConvertBoolHistory(Package, "đóng gỗ") + "",
                                           8, currentDate);
                            }
                            bool CurrentSpecial1 = o.IsCheckSpecial1.ToString().ToBool();
                            if (CurrentSpecial1 != special1)
                            {
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                           " đã đổi phí đặc biệt 1 từ: " + ConvertBoolHistory(CurrentSpecial1, "đặc biệt 1") + " sang: " + ConvertBoolHistory(special1, "đặc biệt 1") + "",
                                           8, currentDate);
                            }

                            bool CurrentSpecial2 = o.IsCheckSpecial2.ToString().ToBool();
                            if (CurrentSpecial2 != special2)
                            {
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                           " đã đổi phí đặc biệt 2 từ: " + ConvertBoolHistory(CurrentSpecial2, "đặc biệt 2") + " sang: " + ConvertBoolHistory(special2, "đặc biệt 2") + "",
                                           8, currentDate);
                            }

                            bool CurrentIsFastDelivery = o.IsFastDelivery.ToString().ToBool();
                            string TotalPriceReal1 = rTotalPriceReal.Text.ToString();
                            TotalPriceReal1 = Math.Round(Convert.ToDouble(TotalPriceReal1), 0).ToString();
                            string TotalPriceRealCYN1 = rTotalPriceRealCYN.Text.ToString();
                            TotalPriceRealCYN1 = Math.Round(Convert.ToDouble(TotalPriceRealCYN1), 2).ToString();
                            if (CurrentIsFastDelivery != MoveIsFastDelivery)
                            {
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                " đã đổi dịch vụ giao hàng tận nhà từ: " + ConvertBoolHistory(CurrentIsFastDelivery, "giao hàng tận nhà") + " sang: " + ConvertBoolHistory(MoveIsFastDelivery, "giao hàng tận nhà") + "",
                                           8, currentDate);
                            }
                            MainOrderController.UpdateTotalFeeSupport(o.ID, TotalFeeSupport.ToString());
                            MainOrderController.UpdateOrderWeight(o.ID, OrderWeight);
                            MainOrderController.UpdateCheckPro(o.ID, checkpro);
                            MainOrderController.UpdateBaogia(o.ID, baogia);
                            MainOrderController.UpdateIsGiaohang(o.ID, ycg);
                            MainOrderController.UpdateOrderDone(o.ID, orderdone);
                            MainOrderController.UpdateOrderPrice(o.ID, orderPrice);
                            MainOrderController.UpdateIsPacked(o.ID, Package);
                            MainOrderController.UpdateIsSpecial(o.ID, special1, special2);
                            MainOrderController.UpdateIsFastDelivery(o.ID, MoveIsFastDelivery);                            
                            MainOrderController.UpdateFeeWarehouse(o.ID, feeeinwarehouse);                           
                            double FeeweightPriceDiscount = 0;
                            if (!string.IsNullOrEmpty(hdfFeeweightPriceDiscount.Value))
                            {
                                FeeweightPriceDiscount = Math.Round(Convert.ToDouble(hdfFeeweightPriceDiscount.Value));
                            }
                            MainOrderController.UpdateFeeWeightDC(o.ID, FeeweightPriceDiscount.ToString());                           
                            MainOrderController.UpdateOrderWeightCK(o.ID, FeeweightPriceDiscount.ToString());
                            MainOrderController.UpdateTQVNWeight(o.ID, o.UID.ToString().ToInt(), Math.Round(Convert.ToDouble(pWeightNDT.Text.ToString()), 2).ToString());
                            MainOrderController.UpdateTotalPriceReal(o.ID, TotalPriceReal1.ToString(), TotalPriceRealCYN1.ToString());                           
                            MainOrderController.UpdateFTS(o.ID, o.UID.ToString().ToInt(), ddlWarehouseFrom.SelectedValue.ToInt(), ddlReceivePlace.SelectedValue, ddlShippingType.SelectedValue.ToInt());
                            MainOrderController.UpdateDoneSmallPackage(o.ID, smallPackage);
                            MainOrderController.UpdateIsInsurrance(o.ID, baohiem);
                            if (baohiem == false)
                            {
                                MainOrderController.UpdateInsurranceMoney(o.ID, "0", o.InsurancePercent);
                            }
                            else
                            {
                                double InsurranceMoney = Convert.ToDouble(o.PriceVND) * (Convert.ToDouble(o.InsurancePercent) / 100);
                                MainOrderController.UpdateInsurranceMoney(o.ID, InsurranceMoney.ToString(), o.InsurancePercent);
                            }
                            #region update status
                            if (status != 1)
                            {
                                if (RoleID == 0)
                                {                                    
                                    if ((status == 4 && orderdone && ddlDatHang.SelectedValue.ToInt(0) > 0) || (orderdone && ddlDatHang.SelectedValue.ToInt(0) > 0))
                                    {
                                        if (o.DateBuy == null)
                                        {
                                            MainOrderController.UpdateDateBuy(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 4);
                                    }
                                    if ((status == 5 && orderPrice && !string.IsNullOrEmpty(rTotalPriceReal.Text)) || (orderPrice && !string.IsNullOrEmpty(rTotalPriceReal.Text)))
                                    {
                                        if (o.DateBuyOK == null)
                                        {
                                            MainOrderController.UpdateDateBuyOK(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 5);
                                    }
                                    if ((status == 3 && checkMVD) || (checkMVD))
                                    {
                                        if (o.DateShipper == null)
                                        {
                                            MainOrderController.UpdateDateShipper(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 3);
                                    }
                                    if (status == 6)
                                    {
                                        if (o.DateTQ == null)
                                        {
                                            MainOrderController.UpdateDateTQ(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 6);
                                    }
                                    if (status == 7)
                                    {
                                        if (o.DateToVN == null)
                                        {
                                            MainOrderController.UpdateDateToVN(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 7);
                                    }
                                    if (status == 8)
                                    {
                                        if (o.DateVN == null)
                                        {
                                            MainOrderController.UpdateDateVN(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 8);
                                    }
                                    if (status == 9)
                                    {
                                        if (o.PayDate == null)
                                        {
                                            MainOrderController.UpdatePayDate(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 9);
                                    }
                                    if (status == 10)
                                    {
                                        if (o.CompleteDate == null)
                                        {
                                            MainOrderController.UpdateCompleteDate(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 10);
                                    }
                                    if (status == 11)
                                    {
                                        if (o.DateToShip == null)
                                        {
                                            MainOrderController.UpdateDateToShip(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 11);
                                    }
                                    if (status == 12)
                                    {
                                        if (o.DateToCancel == null)
                                        {
                                            MainOrderController.UpdateDateToCancel(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 12);
                                    }
                                }
                                else if (RoleID == 2)
                                {                                    
                                    if ((status == 4 && orderdone && ddlDatHang.SelectedValue.ToInt(0) > 0) || (orderdone && ddlDatHang.SelectedValue.ToInt(0) > 0))
                                    {
                                        if (o.DateBuy == null)
                                        {
                                            MainOrderController.UpdateDateBuy(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 4);
                                    }
                                    if ((status == 5 && orderPrice && !string.IsNullOrEmpty(rTotalPriceReal.Text)) || (orderPrice && !string.IsNullOrEmpty(rTotalPriceReal.Text)))
                                    {
                                        if (o.DateBuyOK == null)
                                        {
                                            MainOrderController.UpdateDateBuyOK(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 5);
                                    }
                                    if ((status == 3 && checkMVD) || (checkMVD))
                                    {
                                        if (o.DateShipper == null)
                                        {
                                            MainOrderController.UpdateDateShipper(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 3);
                                    }
                                    if (status == 6)
                                    {
                                        if (o.DateTQ == null)
                                        {
                                            MainOrderController.UpdateDateTQ(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 6);
                                    }
                                    if (status == 7)
                                    {
                                        if (o.DateToVN == null)
                                        {
                                            MainOrderController.UpdateDateToVN(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 7);
                                    }
                                    if (status == 8)
                                    {
                                        if (o.DateVN == null)
                                        {
                                            MainOrderController.UpdateDateVN(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 8);
                                    }
                                    if (status == 9)
                                    {
                                        if (o.PayDate == null)
                                        {
                                            MainOrderController.UpdatePayDate(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 9);
                                    }
                                    if (status == 10)
                                    {
                                        if (o.CompleteDate == null)
                                        {
                                            MainOrderController.UpdateCompleteDate(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 10);
                                    }
                                    if (status == 11)
                                    {
                                        if (o.DateToShip == null)
                                        {
                                            MainOrderController.UpdateDateToShip(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 11);
                                    }
                                    if (status == 12)
                                    {
                                        if (o.DateToCancel == null)
                                        {
                                            MainOrderController.UpdateDateToCancel(o.ID, currentDate);
                                        }
                                        MainOrderController.UpdateStatusByID(o.ID, 12);
                                    }
                                }
                                else if (RoleID == 3)
                                {
                                    if ((status == 4 && orderdone && ddlDatHang.SelectedValue.ToInt(0) > 0) || (orderdone && ddlDatHang.SelectedValue.ToInt(0) > 0))
                                    {
                                        if (o.DateBuy == null)
                                        {
                                            MainOrderController.UpdateDateBuy(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 4);
                                        }
                                    }
                                    if ((status == 3 && checkMVD) || (checkMVD))
                                    {
                                        if (o.DateShipper == null)
                                        {
                                            MainOrderController.UpdateDateShipper(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 3);
                                        }
                                    }
                                }
                                else if (RoleID == 9)
                                {
                                    if ((status == 3 && checkMVD) || (checkMVD))
                                    {
                                        if (o.DateShipper == null)
                                        {
                                            MainOrderController.UpdateDateShipper(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 3);
                                        }
                                    }
                                }
                                else if (RoleID == 4)
                                {
                                    if (status == 6)
                                    {
                                        if (o.DateTQ == null)
                                        {
                                            MainOrderController.UpdateDateTQ(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 6);
                                        }
                                    }
                                    if (status == 7)
                                    {
                                        if (o.DateToVN == null)
                                        {
                                            MainOrderController.UpdateDateToVN(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 7);
                                        }
                                    }
                                }
                                else if (RoleID == 5)
                                {
                                    if (status == 8)
                                    {
                                        if (o.DateVN == null)
                                        {
                                            MainOrderController.UpdateDateVN(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 8);
                                        }
                                    }
                                    if (status == 9)
                                    {
                                        if (o.PayDate == null)
                                        {
                                            MainOrderController.UpdatePayDate(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 9);
                                        }
                                    }
                                    if (status == 11)
                                    {
                                        if (o.DateToShip == null)
                                        {
                                            MainOrderController.UpdateDateToShip(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 11);
                                        }
                                    }
                                }
                                else if (RoleID == 7)
                                {
                                    if ((status == 4 && orderdone && ddlDatHang.SelectedValue.ToInt(0) > 0) || (orderdone && ddlDatHang.SelectedValue.ToInt(0) > 0))
                                    {
                                        if (o.DateBuy == null)
                                        {
                                            MainOrderController.UpdateDateBuy(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 4);
                                        }
                                    }
                                    if ((status == 5 && orderPrice && !string.IsNullOrEmpty(rTotalPriceReal.Text)) || (orderPrice && !string.IsNullOrEmpty(rTotalPriceReal.Text)))
                                    {
                                        if (o.DateBuyOK == null)
                                        {
                                            MainOrderController.UpdateDateBuyOK(o.ID, currentDate);
                                            MainOrderController.UpdateStatusByID(o.ID, 5);
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region update liên quan đến status
                            int currentstt = Convert.ToInt32(o.Status);
                            var imo = MainOrderController.GetByID(o.ID);
                            if (currentstt < 3 || currentstt > 7)
                            {
                                if (imo.Status != currentstt)
                                {
                                     OrderCommentController.Insert(id, "Đã có cập nhật mới cho đơn hàng #" + id + " của bạn.", true, 1, DateTime.Now, uid);                                    
                                }
                            }
                            else if (currentstt > 2 && currentstt < 8)
                            {
                                if (imo.Status < 3 || imo.Status > 7)
                                {                                  
                                    OrderCommentController.Insert(id, "Đã có cập nhật mới cho đơn hàng #" + id + " của bạn.", true, 1, DateTime.Now, uid);

                                    try
                                    {
                                        //PJUtils.SendMailGmail("cskh@1688pgs.vn", "1688pegasus", AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email, 
                                        //    "Thông báo tại 1688PGS.", 
                                        //    "Đã có cập nhật trạng thái cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                        PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy",
                                            AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                                    "Thông báo tại NHAPSICHINA.COM",
                                                    "Đã có cập nhật trạng thái cho đơn hàng #" + id
                                                    + " của bạn. CLick vào để xem", "");
                                    }
                                    catch { }
                                }
                            }
                            #region Ghi lịch sử update status của đơn hàng
                            if (imo.Status != currentstt)
                            {
                                string ustatus = "";
                                switch (imo.Status.Value)
                                {
                                    case 0:
                                        ustatus = "Đơn mới";
                                        break;
                                    case 1:
                                        ustatus = "Đơn hàng hủy";
                                        break;
                                    case 2:
                                        ustatus = "Đơn đã cọc";
                                        break;
                                    case 3:
                                        ustatus = "Đơn người bán giao";
                                        break;
                                    case 4:
                                        ustatus = "Đơn chờ mua hàng";
                                        break;
                                    case 5:
                                        ustatus = "Đơn đã mua hàng";
                                        break;
                                    case 6:
                                        ustatus = "Kho Trung Quốc nhận hàng";
                                        break;
                                    case 7:
                                        ustatus = "Trên đường về Việt Nam";
                                        break;
                                    case 8:
                                        ustatus = "Trong kho Hà Nội";
                                        break;
                                    case 9:
                                        ustatus = "Đã thanh toán";
                                        break;
                                    case 10:
                                        ustatus = "Đã hoàn thành";
                                        break;
                                    case 11:
                                        ustatus = "Đang giao hàng";
                                        break;
                                    case 12:
                                        ustatus = "Đơn khiếu nại";
                                        break;
                                    default:
                                        break;
                                }
                                HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                           " đã đổi trạng thái của đơn hàng ID là: " + o.ID + ", từ: " + orderstatus + ", sang: " + ustatus + "", 0, currentDate);
                            }
                            #endregion
                            if (imo.Status == 5 && imo.Status != currentstt)
                            {
                                var setNoti = SendNotiEmailController.GetByID(7);
                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiUser == true)
                                    {
                                        if (o.OrderType == 1)
                                        {
                                            NotificationsController.Inser(accmuahan.ID,
                                              accmuahan.Username, o.ID,
                                              "Đơn hàng " + o.ID + " đã được mua hàng.", 1,
                                              currentDate, obj_user.Username, true);
                                            string strPathAndQuery = Request.Url.PathAndQuery;
                                            string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                            string datalink = "" + strUrl + "chi-tiet-don-hang/" + o.ID;
                                            PJUtils.PushNotiDesktop(accmuahan.ID, "Đơn hàng " + o.ID + " đã được mua hàng.", datalink);
                                        }
                                        else
                                        {
                                            NotificationsController.Inser(accmuahan.ID,
                                              accmuahan.Username, o.ID,
                                              "Đơn hàng " + o.ID + " đã được mua hàng.", 11,
                                              currentDate, obj_user.Username, true);
                                            string strPathAndQuery = Request.Url.PathAndQuery;
                                            string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                            string datalink = "" + strUrl + "chi-tiet-don-hang/" + o.ID;
                                            PJUtils.PushNotiDesktop(accmuahan.ID, "Đơn hàng " + o.ID + " đã được mua hàng.", datalink);
                                        }

                                    }

                                    //if (setNoti.IsSendEmailUser == true)
                                    //{
                                    //    try
                                    //    {
                                    //        PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy",
                                    //            accmuahan.Email,
                                    //            "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID
                                    //            + " đã được mua hàng.", "");
                                    //    }
                                    //    catch { }
                                    //}
                                }
                            }
                            #endregion   
                            #endregion                           
                            #region Cập nhật thông tin nhân viên sale và đặt hàng
                            int SalerID = ddlSaler.SelectedValue.ToString().ToInt(0);
                            int DathangID = ddlDatHang.SelectedValue.ToString().ToInt(0);
                            int CSKHID = ddlCSKH.SelectedValue.ToString().ToInt(0);
                            int KhoTQID = ddlKhoTQ.SelectedValue.ToString().ToInt(0);
                            int khoVNID = ddlKhoVN.SelectedValue.ToString().ToInt(0);
                            var mo = MainOrderController.GetAllByID(id);
                            if (mo != null)
                            {
                                double feebp = Math.Round(Convert.ToDouble(mo.FeeBuyPro), 0);
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
                                int cskh_old = Convert.ToInt32(mo.CSID);

                                int UID = 0;
                                UID = mo.UID.Value;

                                if (CSKHID != cskh_old)
                                {
                                    MainOrderController.UpdateCSKHID(mo.ID, CSKHID);
                                }

                                #region Saler
                                if (SalerID > 0)
                                {
                                    if (SalerID == salerID_old)
                                    {
                                        var staff = StaffIncomeController.GetByMainOrderIDUID(id, salerID_old);
                                        if (staff != null)
                                        {
                                            int rStaffID = staff.ID;
                                            int staffstatus = Convert.ToInt32(staff.Status);
                                            if (staffstatus == 1)
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
                                    }
                                    else
                                    {
                                        var createdDate = DateTime.Now;
                                        var staff = StaffIncomeController.GetByMainOrderIDUID(id, salerID_old);
                                        if (staff != null)
                                        {
                                            createdDate = staff.CreatedDate.Value;
                                            StaffIncomeController.Delete(staff.ID);
                                        }
                                        var sale = AccountController.GetByID(SalerID);
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
                                                    StaffIncomeController.Insert(id, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                                    CreatedDate, currentDate, username);
                                                }
                                                else
                                                {
                                                    double per = Math.Round(feebp * salepercent / 100, 0);
                                                    StaffIncomeController.Insert(id, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                                    CreatedDate, currentDate, username);
                                                }

                                            }




                                        }
                                    }
                                }
                                #endregion
                                #region Đặt hàng
                                if (DathangID > 0)
                                {
                                    if (DathangID == dathangID_old)
                                    {
                                        var staff = StaffIncomeController.GetByMainOrderIDUID(id, dathangID_old);
                                        if (staff != null)
                                        {
                                            if (staff.Status == 1)
                                            {
                                                //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                                double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                                totalPrice = Math.Round(totalPrice, 0);
                                                double totalRealPrice = 0;
                                                if (mo.TotalPriceReal.ToFloat(0) > 0)
                                                    totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                                if (totalRealPrice > 0)
                                                {
                                                    double totalpriceloi = totalPrice - totalRealPrice;
                                                    dathangpercent = Convert.ToDouble(staff.PercentReceive);
                                                    double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                                    //double income = totalpriceloi;
                                                    StaffIncomeController.Update(staff.ID, totalRealPrice.ToString(), dathangpercent.ToString(), 1,
                                                                income.ToString(), false, currentDate, username);
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        var createdDate = DateTime.Now;
                                        var staff = StaffIncomeController.GetByMainOrderIDUID(id, dathangID_old);
                                        if (staff != null)
                                        {
                                            createdDate = staff.CreatedDate.Value;
                                            StaffIncomeController.Delete(staff.ID);
                                        }
                                        var dathang = AccountController.GetByID(DathangID);
                                        if (dathang != null)
                                        {
                                            dathangName = dathang.Username;
                                            //double totalPrice = Convert.ToDouble(mo.TotalPriceVND);
                                            double totalPrice = Convert.ToDouble(mo.PriceVND) + Convert.ToDouble(mo.FeeShipCN);
                                            totalPrice = Math.Round(totalPrice, 0);
                                            double totalRealPrice = 0;
                                            if (mo.TotalPriceReal.ToFloat(0) > 0)
                                                totalRealPrice = Math.Round(Convert.ToDouble(mo.TotalPriceReal), 0);
                                            if (totalRealPrice > 0)
                                            {
                                                double totalpriceloi = totalPrice - totalRealPrice;
                                                double income = Math.Round(totalpriceloi * dathangpercent / 100, 0);
                                                //double income = totalpriceloi;

                                                StaffIncomeController.Insert(id, totalpriceloi.ToString(), dathangpercent.ToString(), DathangID, dathangName, 3, 1,
                                                    income.ToString(), false, CreatedDate, createdDate, username);
                                            }
                                            else
                                            {
                                                StaffIncomeController.Insert(id, "0", dathangpercent.ToString(), DathangID, dathangName, 3, 1, "0", false,
                                                CreatedDate, createdDate, username);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                            MainOrderController.UpdateStaff(id, SalerID, DathangID, KhoTQID, khoVNID);
                            #endregion                                                   
                            var quantitymvd = SmallPackageController.GetByMainOrderID(id);
                            if (quantitymvd.Count > 0)
                            {
                                if (quantitymvd != null)
                                {
                                    MainOrderController.UpdateListMVD(id, listmvd, quantitymvd.Count);
                                }
                            }

                            Response.Redirect("/manager/OrderDetail.aspx?id=" + id + "");
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Mã vận đơn " + listmvd_ne + " đã tồn tại.", "e", false, Page);
                        }
                    }
                }
            }
        }
        protected void btnStaffUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            DateTime currentDate = DateTime.Now;
            int SalerID = ddlSaler.SelectedValue.ToString().ToInt(0);
            int DathangID = ddlDatHang.SelectedValue.ToString().ToInt(0);
            int CSKHID = ddlCSKH.SelectedValue.ToString().ToInt(0);
            int KhoTQID = ddlKhoTQ.SelectedValue.ToString().ToInt(0);
            int khoVNID = ddlKhoVN.SelectedValue.ToString().ToInt(0);
            int ID = ViewState["MOID"].ToString().ToInt();
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 2)
                {
                    var mo = MainOrderController.GetAllByID(ID);
                    if (mo != null)
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

                        int UID = 0;
                        UID = mo.UID.Value;

                        int salerID_old = Convert.ToInt32(mo.SalerID);
                        int dathangID_old = Convert.ToInt32(mo.DathangID);
                        int cskh_old = Convert.ToInt32(mo.CSID);

                        #region Saler
                        if (SalerID > 0)
                        {
                            if (SalerID == salerID_old)
                            {
                                var staff = StaffIncomeController.GetByMainOrderIDUID(ID, salerID_old);
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
                                    var sale = AccountController.GetByID(SalerID);
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
                                                StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                                CreatedDate, currentDate, username);
                                            }
                                            else
                                            {
                                                double per = Math.Round(feebp * salepercent / 100, 0);
                                                StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                                CreatedDate, currentDate, username);
                                            }

                                        }




                                    }
                                }
                            }
                            else
                            {
                                var staff = StaffIncomeController.GetByMainOrderIDUID(ID, salerID_old);
                                var createdDate = DateTime.Now;
                                if (staff != null)
                                {
                                    createdDate = staff.CreatedDate.Value;
                                    StaffIncomeController.Delete(staff.ID);
                                }
                                var sale = AccountController.GetByID(SalerID);
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
                                            StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                            CreatedDate, createdDate, username);
                                        }
                                        else
                                        {
                                            double per = Math.Round(feebp * salepercent / 100, 0);
                                            StaffIncomeController.Insert(ID, per.ToString(), salepercent.ToString(), SalerID, salerName, 6, 1, per.ToString(), false,
                                            CreatedDate, createdDate, username);
                                        }
                                    }




                                }
                            }
                        }
                        #endregion
                        #region Đặt hàng
                        if (DathangID > 0)
                        {
                            if (DathangID == dathangID_old)
                            {
                                var staff = StaffIncomeController.GetByMainOrderIDUID(ID, dathangID_old);
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
                                    var dathang = AccountController.GetByID(DathangID);
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
                                            StaffIncomeController.Insert(ID, totalpriceloi.ToString(), dathangpercent.ToString(), DathangID, dathangName, 3, 1,
                                                income.ToString(), false, CreatedDate, currentDate, username);
                                        }
                                        else
                                        {
                                            StaffIncomeController.Insert(ID, "0", dathangpercent.ToString(), DathangID, dathangName, 3, 1, "0", false,
                                            CreatedDate, currentDate, username);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var createdDate = DateTime.Now;
                                var staff = StaffIncomeController.GetByMainOrderIDUID(ID, dathangID_old);
                                if (staff != null)
                                {
                                    createdDate = staff.CreatedDate.Value;
                                    StaffIncomeController.Delete(staff.ID);
                                }
                                var dathang = AccountController.GetByID(DathangID);
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

                                        StaffIncomeController.Insert(ID, totalpriceloi.ToString(), dathangpercent.ToString(), DathangID, dathangName, 3, 1,
                                            income.ToString(), false, CreatedDate, createdDate, username);
                                    }
                                    else
                                    {
                                        StaffIncomeController.Insert(ID, "0", dathangpercent.ToString(), DathangID, dathangName, 3, 1, "0", false,
                                        CreatedDate, createdDate, username);
                                    }
                                }
                            }
                        }
                        #endregion

                        if (cskh_old != CSKHID)
                        {
                            MainOrderController.UpdateCSKHID(mo.ID, CSKHID);
                        }
                    }
                    MainOrderController.UpdateStaff(ID, SalerID, DathangID, KhoTQID, khoVNID);
                    PJUtils.ShowMsg("Cập nhật nhân viên thành công.", true, Page);
                }
                else
                    PJUtils.ShowMsg("Cập nhật nhân viên thành công.", true, Page);
            }
        }
        protected void btnThanhtoan_Click(object sender, EventArgs e)
        {
            int id = ViewState["MOID"].ToString().ToInt(0);
            //var id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                var o = MainOrderController.GetAllByID(id);
                if (o != null)
                {
                    Response.Redirect("/manager/Pay-Order.aspx?id=" + id);
                }
            }
        }
        #endregion
        #region Ajax
        [WebMethod]
        public static string DeleteSmallPackage(string IDPackage)
        {
            if (HttpContext.Current.Session["userLoginSystem"] == null)
            {
                return null;
            }
            else
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var obj_user = AccountController.GetByUsername(username);
                if (obj_user.RoleID != 6)
                {
                    int ID = IDPackage.ToInt(0);
                    var smallpackage = SmallPackageController.GetByID(ID);
                    if (smallpackage != null)
                    {
                        string kq = SmallPackageController.Delete(ID);
                        return "ok";
                    }
                    else
                    {
                        return "null";
                    }
                }
                else
                {
                    return "null";
                }

            }

        }

        [WebMethod]
        public static string DeleteSupportFee(string IDPackage)
        {
            DateTime currentDate = DateTime.Now;
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 2 || obj_user.RoleID == 6 || obj_user.RoleID == 9 || obj_user.RoleID == 3)
                {
                    int ID = IDPackage.ToInt(0);
                    var supportfee = FeeSupportController.GetByID(ID);
                    if (supportfee != null)
                    {
                        string kq = FeeSupportController.Delete(ID);
                        HistoryOrderChangeController.Insert(supportfee.MainOrderID.Value, obj_user.ID, obj_user.Username, obj_user.Username +
                        " đã xóa tiền phụ phí của đơn hàng ID là: " + supportfee.MainOrderID + ", Tên phụ phí: " + supportfee.SupportName + ", Số tiền: "
                        + string.Format("{0:N0}", Convert.ToDouble(supportfee.SupportInfoVND)) + "", 10, currentDate);

                        double TotalPriceVND = 0;
                        int MainOrderID = Convert.ToInt32(supportfee.MainOrderID);
                        var mo = MainOrderController.GetByID(MainOrderID);
                        if (mo != null)
                        {
                            TotalPriceVND = Convert.ToDouble(mo.TotalPriceVND) - Convert.ToDouble(supportfee.SupportInfoVND);
                            TotalPriceVND = Math.Round(TotalPriceVND, 0);

                            MainOrderController.UpdateTotalPriceVNDSupport(MainOrderID, TotalPriceVND.ToString());
                        }
                        return "ok";
                    }
                    else
                        return "none";
                }
                else
                    return "role";
            }
            else
            {
                return "null";
            }
        }


        [WebMethod]
        public static string DeleteMainOrderCode(int IDCode)
        {
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (HttpContext.Current.Session["userLoginSystem"] == null)
            {
                return null;
            }
            else
            {
                int ID = IDCode;
                if (ac.RoleID != 6)
                {
                    var MainOrderCode = MainOrderCodeController.GetByID(ID);
                    if (MainOrderCode != null)
                    {

                        string kq = MainOrderCodeController.Delete(ID);

                        string ListMVD = "";
                        var a = MainOrderController.GetByID(Convert.ToInt32(MainOrderCode.MainOrderID));
                        var list = MainOrderCodeController.GetAllByMainOrderID(Convert.ToInt32(MainOrderCode.MainOrderID));
                        foreach (var item in list)
                        {
                            ListMVD += item.MainOrderCode + " | ";
                        }
                        MainOrderController.UpdateMainOrderCode_Thang(Convert.ToInt32(MainOrderCode.MainOrderID), Convert.ToInt32(a.UID), ListMVD, list.Count);

                        return kq;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }

        }



        //[WebMethod]
        //public static string DeleteMainOrderCode(int IDCode)
        //{
        //    string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
        //    tbl_Account ac = AccountController.GetByUsername(username_current);
        //    if (HttpContext.Current.Session["userLoginSystem"] == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        int ID = IDCode;
        //        if (ac.RoleID != 6)
        //        {
        //            var MainOrderCode = MainOrderCodeController.GetByID(ID);
        //            if (MainOrderCode != null)
        //            {
        //                string kq = MainOrderCodeController.Delete(ID);
        //                return kq;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }

        //}

        [WebMethod]
        public static string UpdateMainOrderCode(int ID, string MainOrderCode, int MainOrderID)
        {
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                if (ac.RoleID != 6)
                {
                    var o = MainOrderController.GetAllByID(MainOrderID);
                    if (o != null)
                    {
                        var lo = MainOrderCodeController.GetByID(ID);
                        if (!string.IsNullOrEmpty(MainOrderCode))
                        {
                            if (lo == null)
                            {
                                var so = MainOrderCodeController.GetByMainOrderIDANDMainOrderCode(MainOrderID, MainOrderCode);
                                if (so == null)
                                {
                                    var kq = MainOrderCodeController.Insert(MainOrderID, MainOrderCode, DateTime.Now, ac.Username);

                                    string ListMVD = "";
                                    var list = MainOrderCodeController.GetAllByMainOrderID(MainOrderID);
                                    foreach (var item in list)
                                    {
                                        ListMVD += item.MainOrderCode + " | ";
                                    }
                                    MainOrderController.UpdateMainOrderCode_Thang(MainOrderID, Convert.ToInt32(o.UID), ListMVD, list.Count);

                                    return kq;
                                }
                                return null;
                            }
                            else
                            {
                                var so = MainOrderCodeController.GetByMainOrderIDANDMainOrderCode(MainOrderID, MainOrderCode);
                                if (so == null)
                                {
                                    var kq = MainOrderCodeController.UpdateCode(ID, MainOrderCode, DateTime.Now, ac.Username);

                                    string ListMVD = "";
                                    var list = MainOrderCodeController.GetAllByMainOrderID(MainOrderID);
                                    foreach (var item in list)
                                    {
                                        ListMVD += item.MainOrderCode + " | ";
                                    }
                                    MainOrderController.UpdateMainOrderCode_Thang(MainOrderID, Convert.ToInt32(o.UID), ListMVD, list.Count);

                                    return kq;
                                }
                                return null;
                            }


                        }
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return "role";
                }
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public static string CountFeeWeight(int orderID, int receivePlace, int shippingTypeValue, double weight, int WarehouseFrom)
        {
            var order = MainOrderController.GetAllByID(orderID);
            if (order != null)
            {
                double pricePerKg = 0;
                int fromPlace = WarehouseFrom;
                var warehousefee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(
                    fromPlace, receivePlace, shippingTypeValue, false);
                if (warehousefee.Count > 0)
                {
                    foreach (var w in warehousefee)
                    {
                        if (w.WeightFrom < Convert.ToDouble(order.PriceVND) && Convert.ToDouble(order.PriceVND) <= w.WeightTo)
                        {
                            pricePerKg = Convert.ToDouble(w.Price);
                        }
                    }
                }
                int UID = Convert.ToInt32(order.UID);
                var usercreate = AccountController.GetByID(UID);
                if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerWeight))
                {
                    double feeweightuser = 0;
                    if (usercreate.FeeTQVNPerWeight.ToFloat(0) > 0)
                    {
                        feeweightuser = Convert.ToDouble(usercreate.FeeTQVNPerWeight);
                    }
                    pricePerKg = feeweightuser;
                }

                double ckFeeWeight = 0;
                if (usercreate != null)
                {
                    ckFeeWeight = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeWeight.ToString());
                }
                double currency = Convert.ToDouble(order.CurrentCNYVN);
                double totalPriceFeeweightVN = pricePerKg * weight;

                double discountVN = totalPriceFeeweightVN * ckFeeWeight / 100;
                double discountCYN = discountVN / currency;

                double feeoutVN = totalPriceFeeweightVN - discountVN;
                double feeoutCYN = feeoutVN / currency;

                double feebpVN = 0;
                double feebpCYN = 0;
                var fe = FeeBuyProController.GetByTypeAndPrice(shippingTypeValue, Convert.ToDouble(order.PriceVND));
                if (fe != null)
                {
                    var fee = fe.FeePercent;
                    feebpVN = Convert.ToDouble(order.PriceVND) * fee.Value / 100;
                    if (shippingTypeValue == 1 && feebpVN < 10000)
                        feebpVN = 10000;
                    if (shippingTypeValue == 4 && feebpVN < 15000)
                        feebpVN = 15000;
                    feebpCYN = feebpVN / currency;
                }

                FeeWeightObj f = new FeeWeightObj();
                f.FeeWeightCYN = Math.Floor(feeoutCYN);
                f.FeeWeightVND = Math.Floor(feeoutVN);
                f.DiscountFeeWeightCYN = Math.Floor(discountCYN);
                f.DiscountFeeWeightVN = Math.Floor(discountVN);
                //f.FeeBuyProCYN = Math.Floor(feebpCYN);
                //f.FeeBuyProVN = Math.Floor(feebpVN);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(f);
            }
            return "none";
        }

        [WebMethod]
        public static string sendcustomercomment(string comment, int id, string urlIMG, string real)
        {
            var listLink = urlIMG.Split('|').ToList();
            if (listLink.Count > 0)
            {
                listLink.RemoveAt(listLink.Count - 1);
            }
            var listComment = real.Split('|').ToList();
            if (listComment.Count > 0)
            {
                listComment.RemoveAt(listComment.Count - 1);
            }
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (obj_user != null)
            {
                string ret = "";
                var ai = AccountInfoController.GetByUserID(obj_user.ID);
                if (ai != null)
                {
                    ret += ai.FirstName + " " + ai.LastName + "," + ai.IMGUser + "," + string.Format("{0:dd/MM/yyyy HH:mm}", currentDate);
                }
                int uid = obj_user.ID;
                //var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {
                        var setNoti = SendNotiEmailController.GetByID(12);
                        int type = 1;
                        if (type > 0)
                        {
                            for (int i = 0; i < listLink.Count; i++)
                            {
                                string kqq = OrderCommentController.InsertNew(id, listLink[i], listComment[i], true, type, DateTime.Now, uid);
                            }
                            if (!string.IsNullOrEmpty(comment))
                            {
                                string kq = OrderCommentController.Insert(id, comment, true, type, DateTime.Now, uid);
                                if (type == 1)
                                {
                                    if (setNoti != null)
                                    {
                                        if (setNoti.IsSentNotiUser == true)
                                        {
                                            if (o.OrderType == 1)
                                            {
                                                NotificationsController.Inser(Convert.ToInt32(o.UID),
                                       AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem",
                                       12, currentDate, obj_user.Username, true);
                                                string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                                string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                string datalink = "" + strUrl + "chi-tiet-don-hang/" + id;
                                                PJUtils.PushNotiDesktop(Convert.ToInt32(o.UID), "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                            }
                                            else
                                            {
                                                NotificationsController.Inser(Convert.ToInt32(o.UID),
                                       AccountController.GetByID(Convert.ToInt32(o.UID)).Username, id, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem",
                                       13, currentDate, obj_user.Username, true);
                                                string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                                string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                string datalink = "" + strUrl + "chi-tiet-don-hang/" + id;
                                                PJUtils.PushNotiDesktop(Convert.ToInt32(o.UID), "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                            }

                                        }

                                        if (setNoti.IsSendEmailUser == true)
                                        {
                                            try
                                            {
                                                PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy",
                                                    AccountInfoController.GetByUserID(Convert.ToInt32(o.UID)).Email,
                                                    "Thông báo tại NHAPSICHINA.COM.",
                                                    "Đã có đánh giá mới cho đơn hàng #" + id
                                                    + " của bạn. CLick vào để xem", "");
                                            }
                                            catch { }
                                        }
                                    }
                                }
                                ChatHub ch = new ChatHub();
                                ch.SendMessenger(uid, id, comment, listLink, listComment);

                                CustomerComment dataout = new CustomerComment();
                                dataout.UID = uid;
                                dataout.OrderID = id;
                                StringBuilder showIMG = new StringBuilder();
                                for (int i = 0; i < listLink.Count; i++)
                                {
                                    showIMG.Append("<div class=\"chat chat-right\">");
                                    showIMG.Append("    <div class=\"chat-avatar\">");
                                    showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("    <div class=\"chat-body\">");
                                    showIMG.Append("        <div class=\"chat-text\">");
                                    showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                    showIMG.Append("            <div class=\"text-content\">");
                                    showIMG.Append("                <div class=\"content\">");
                                    showIMG.Append("                    <div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                    showIMG.Append("	                    <div class=\"img-block\">");
                                    if (listLink[i].Contains(".doc"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                    }
                                    else if (listLink[i].Contains(".xls"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    else
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    showIMG.Append("	                    </div>");
                                    showIMG.Append("                    </div>");
                                    showIMG.Append("                </div>");
                                    showIMG.Append("            </div>");
                                    showIMG.Append("        </div>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("</div>");
                                }
                                showIMG.Append("<div class=\"chat chat-right\">");
                                showIMG.Append("    <div class=\"chat-avatar\">");
                                showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                showIMG.Append("    </div>");
                                showIMG.Append("    <div class=\"chat-body\">");
                                showIMG.Append("        <div class=\"chat-text\">");
                                showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                showIMG.Append("            <div class=\"text-content\">");
                                showIMG.Append("                <div class=\"content\">");
                                showIMG.Append("                    <p>" + comment + "</p>");
                                showIMG.Append("                </div>");
                                showIMG.Append("            </div>");
                                showIMG.Append("        </div>");
                                showIMG.Append("    </div>");
                                showIMG.Append("</div>");

                                dataout.Comment = showIMG.ToString();
                                return serializer.Serialize(dataout);

                            }
                            else
                            {

                                if (listComment.Count > 0)
                                {
                                    ChatHub ch = new ChatHub();
                                    ch.SendMessenger(uid, id, comment, listLink, listComment);
                                    CustomerComment dataout = new CustomerComment();
                                    StringBuilder showIMG = new StringBuilder();
                                    for (int i = 0; i < listLink.Count; i++)
                                    {

                                        showIMG.Append("<div class=\"chat chat-right\">");
                                        showIMG.Append("<div class=\"chat-avatar\">");
                                        showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("<div class=\"chat-body\">");
                                        showIMG.Append("<div class=\"chat-text\">");
                                        showIMG.Append("<div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                        showIMG.Append("<div class=\"text-content\">");
                                        showIMG.Append("<div class=\"content\">");
                                        showIMG.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        showIMG.Append("<div class=\"img-block\">");
                                        if (listLink[i].Contains(".doc"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (listLink[i].Contains(".xls"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                    }
                                    dataout.UID = uid;
                                    dataout.OrderID = id;
                                    dataout.Comment = showIMG.ToString();
                                    return serializer.Serialize(dataout);
                                }

                            }

                        }
                    }
                }
            }
            return serializer.Serialize(null);
        }
        public partial class CustomerComment
        {
            public int UID { get; set; }
            public int OrderID { get; set; }
            public string Comment { get; set; }
            public List<string> Link { get; set; }
            public List<string> CommentName { get; set; }
        }
        [WebMethod]
        public static string sendstaffcomment(string comment, int id, string urlIMG, string real)
        {
            var listLink = urlIMG.Split('|').ToList();
            if (listLink.Count > 0)
            {
                listLink.RemoveAt(listLink.Count - 1);
            }
            var listComment = real.Split('|').ToList();
            if (listComment.Count > 0)
            {
                listComment.RemoveAt(listComment.Count - 1);
            }
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (obj_user != null)
            {
                string ret = "";
                var ai = AccountInfoController.GetByUserID(obj_user.ID);
                if (ai != null)
                {
                    ret += ai.FirstName + " " + ai.LastName + "," + ai.IMGUser + "," + string.Format("{0:dd/MM/yyyy HH:mm}", currentDate);
                }
                int uid = obj_user.ID;
                //var id = Convert.ToInt32(Request.QueryString["id"]);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByID(id);
                    if (o != null)
                    {

                        int type = 2;
                        if (type > 0)
                        {
                            for (int i = 0; i < listLink.Count; i++)
                            {
                                string kqq = OrderCommentController.InsertNew(id, listLink[i], listComment[i], true, type, DateTime.Now, uid);
                            }
                            if (!string.IsNullOrEmpty(comment))
                            {
                                string kq = OrderCommentController.Insert(id, comment, true, type, DateTime.Now, uid);
                                var sale = AccountController.GetByID(o.SalerID.Value);
                                if (sale != null)
                                {
                                    if (obj_user.ID != sale.ID)
                                    {
                                        NotificationsController.Inser(sale.ID,
                                                                         sale.Username, id,
                                                                         "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                          currentDate, username, false);
                                        string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                        string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                        PJUtils.PushNotiDesktop(sale.ID, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                    }
                                }

                                var dathang = AccountController.GetByID(o.DathangID.Value);
                                if (dathang != null)
                                {
                                    if (obj_user.ID != dathang.ID)
                                    {
                                        NotificationsController.Inser(dathang.ID,
                                                                           dathang.Username, id,
                                                                           "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                            currentDate, username, false);
                                        string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                        string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                        PJUtils.PushNotiDesktop(dathang.ID, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                    }
                                }

                                var admins = AccountController.GetAllByRoleID(0);
                                if (admins.Count > 0)
                                {
                                    foreach (var admin in admins)
                                    {
                                        if (obj_user.ID != admin.ID)
                                        {
                                            NotificationsController.Inser(admin.ID,
                                                                          admin.Username, id,
                                                                          "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                           currentDate, username, false);
                                            string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                            string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                            string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                            PJUtils.PushNotiDesktop(admin.ID, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                        }
                                    }
                                }
                                var managers = AccountController.GetAllByRoleID(2);
                                if (managers.Count > 0)
                                {
                                    foreach (var manager in managers)
                                    {
                                        if (obj_user.ID != manager.ID)
                                        {
                                            NotificationsController.Inser(manager.ID,
                                                                           manager.Username, id,
                                                                           "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", 1,
                                                                          currentDate, username, false);
                                            string strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                                            string strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                            string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                            PJUtils.PushNotiDesktop(manager.ID, "Đã có đánh giá mới cho đơn hàng #" + id + " của bạn. CLick vào để xem", datalink);
                                        }
                                    }
                                }
                                ChatHub ch = new ChatHub();
                                ch.SendMessengerToStaff(uid, id, comment, listLink, listComment);

                                CustomerComment dataout = new CustomerComment();
                                dataout.UID = uid;
                                dataout.OrderID = id;
                                StringBuilder showIMG = new StringBuilder();
                                for (int i = 0; i < listLink.Count; i++)
                                {
                                    showIMG.Append("<div class=\"chat chat-right\">");
                                    showIMG.Append("    <div class=\"chat-avatar\">");
                                    showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("    <div class=\"chat-body\">");
                                    showIMG.Append("        <div class=\"chat-text\">");
                                    showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                    showIMG.Append("            <div class=\"text-content\">");
                                    showIMG.Append("                <div class=\"content\">");
                                    showIMG.Append("                    <div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                    showIMG.Append("	                    <div class=\"img-block\">");
                                    if (listLink[i].Contains(".doc"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                    }
                                    else if (listLink[i].Contains(".xls"))
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    else
                                    {
                                        showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    showIMG.Append("	                    </div>");
                                    showIMG.Append("                    </div>");
                                    showIMG.Append("                </div>");
                                    showIMG.Append("            </div>");
                                    showIMG.Append("        </div>");
                                    showIMG.Append("    </div>");
                                    showIMG.Append("</div>");
                                }
                                showIMG.Append("<div class=\"chat chat-right\">");
                                showIMG.Append("    <div class=\"chat-avatar\">");
                                showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                showIMG.Append("    </div>");
                                showIMG.Append("    <div class=\"chat-body\">");
                                showIMG.Append("        <div class=\"chat-text\">");
                                showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                showIMG.Append("            <div class=\"text-content\">");
                                showIMG.Append("                <div class=\"content\">");
                                showIMG.Append("                    <p>" + comment + "</p>");
                                showIMG.Append("                </div>");
                                showIMG.Append("            </div>");
                                showIMG.Append("        </div>");
                                showIMG.Append("    </div>");
                                showIMG.Append("</div>");


                                dataout.Comment = showIMG.ToString();
                                return serializer.Serialize(dataout);


                            }
                            else
                            {
                                if (listComment.Count > 0)
                                {
                                    ChatHub ch = new ChatHub();
                                    ch.SendMessengerToStaff(uid, id, comment, listLink, listComment);
                                    CustomerComment dataout = new CustomerComment();
                                    StringBuilder showIMG = new StringBuilder();
                                    for (int i = 0; i < listLink.Count; i++)
                                    {

                                        showIMG.Append("<div class=\"chat chat-right\">");
                                        showIMG.Append("<div class=\"chat-avatar\">");
                                        showIMG.Append("    <p class=\"name\">" + AccountController.GetByID(uid).Username + "</p>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("<div class=\"chat-body\">");
                                        showIMG.Append("<div class=\"chat-text\">");
                                        showIMG.Append("<div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                        showIMG.Append("<div class=\"text-content\">");
                                        showIMG.Append("<div class=\"content\">");
                                        showIMG.Append("<div class=\"content-img\" style=\"border-radius: 5px;background-color: #2196f3;\">");
                                        showIMG.Append("<div class=\"img-block\">");
                                        if (listLink[i].Contains(".doc"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                        }
                                        else if (listLink[i].Contains(".xls"))
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/AdminNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        else
                                        {
                                            showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"" + listLink[i] + "\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
                                        }
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                        showIMG.Append("</div>");
                                    }
                                    dataout.UID = uid;
                                    dataout.OrderID = id;
                                    dataout.Comment = showIMG.ToString();
                                    return serializer.Serialize(dataout);
                                }
                            }


                        }
                    }
                }
            }
            return serializer.Serialize(null);
        }

        #endregion
        #region Class
        public class historyCustom
        {
            public int ID { get; set; }
            public string Username { get; set; }
            //public string RoleName { get; set; }
            public string Date { get; set; }
            public string Content { get; set; }
        }
        public class FeeWeightObj
        {
            public double FeeWeightVND { get; set; }
            public double FeeWeightCYN { get; set; }
            public double DiscountFeeWeightCYN { get; set; }
            public double DiscountFeeWeightVN { get; set; }
            public double FeeBuyProVN { get; set; }
            public double FeeBuyProCYN { get; set; }
        }
        #endregion
        protected void r_ItemCommand(object sender, GridCommandEventArgs e)
        {
            var g = e.Item as GridDataItem;
            if (g == null) return;

        }

        protected void gr_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {

        }
        protected void gr_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                var o = MainOrderController.GetAllByID(id);
                if (o != null)
                {
                    var historyorder = HistoryOrderChangeController.GetByMainOrderID(o.ID);
                    if (historyorder.Count > 0)
                    {
                        List<historyCustom> hc = new List<historyCustom>();
                        foreach (var item in historyorder)
                        {
                            string username = item.Username;
                            string rolename = "admin";
                            var acc = AccountController.GetByUsername(username);
                            if (acc != null)
                            {
                                int role = Convert.ToInt32(acc.RoleID);

                                var r = RoleController.GetByID(role);
                                if (r != null)
                                {
                                    rolename = r.RoleDescription;
                                }
                            }
                            historyCustom h = new historyCustom();
                            h.ID = item.ID;
                            h.Username = username;
                            //h.RoleName = rolename;
                            h.Date = string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate);
                            h.Content = item.HistoryContent;
                            hc.Add(h);
                        }
                        gr.DataSource = hc;
                    }
                }
            }
        }
        private string ConvertBoolHistory(bool status, string content)
        {
            if (status)
                return "có " + content;
            else
                return "không " + content;
        }


        [WebMethod]
        public static string checkbefore(string listStr)
        {
            string returns = "";
            if (!string.IsNullOrEmpty(listStr))
            {
                string[] list = listStr.Split('|');
                bool checkConflitCode = false;
                if (list.Length - 1 > 0)
                {
                    for (int i = 0; i < list.Length - 1; i++)
                    {
                        string items = list[i];
                        string[] item = items.Split(',');
                        string code = item[1].ToString().Trim();
                        var getsmallcheck = SmallPackageController.GetByOrderCode(code);
                        if (getsmallcheck.Count > 0)
                        {
                            checkConflitCode = true;
                            returns += code + "; ";
                        }
                    }
                }
                if (checkConflitCode == true)
                {
                    return returns;
                }
                else
                {
                    return "ok";
                }
            }
            return "ok";
        }
    }
}