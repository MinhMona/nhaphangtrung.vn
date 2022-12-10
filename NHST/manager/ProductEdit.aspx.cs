﻿using MB.Extensions;
using Microsoft.AspNet.SignalR;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Hubs;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static NHST.manager.OutStock1;

namespace NHST.manager
{
    public partial class ProductEdit : System.Web.UI.Page
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
                    Loaddata();
                }

            }

        }

        public void Loaddata()
        {
            var id = Request.QueryString["id"].ToInt(0);
            if (id > 0)
            {
                var o = OrderController.GetAllByID(id);
                if (o != null)
                {
                    var mainorder = MainOrderController.GetAllByID(o.MainOrderID.ToString().ToInt());
                    var config = ConfigurationController.GetByTop1();
                    double currency = 0;
                    if (config != null)
                    {
                        hdfcurrent.Value = mainorder.CurrentCNYVN.ToString();
                        currency = Convert.ToDouble(mainorder.CurrentCNYVN);
                    }
                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);
                    if (ac.RoleID != 0 && ac.RoleID != 3 && ac.RoleID != 2 && ac.RoleID != 9)
                    {
                        Response.Redirect("/manager/OrderDetail.aspx?id=" + o.MainOrderID + "");
                    }
                    //else
                    //{
                    //    if (ac.RoleID == 3)
                    //    {
                    //        if (mainorder.Status >= 5)
                    //            btncreateuser.Visible = false;
                    //        pProductPriceOriginal.Enabled = false;
                    //        pRealPrice.Enabled = true;
                    //    }
                    //}
                    if (mainorder.Status >= 5)
                    {
                        if (ac.ID == 1 || ac.ID == 22)
                        {
                            pnAdmin.Enabled = true;
                        }
                        else
                        {
                            pnAdmin.Enabled = false;
                        }
                    }
                    else
                    {
                        pnAdmin.Enabled = true;
                    }

                    txtTitleOrigin.Text = o.title_origin;
                    double price = 0;
                    double pricepromotion = 0;
                    double priceorigin = 0;
                    if (o.price_promotion.ToFloat(0) > 0)
                        pricepromotion = Convert.ToDouble(o.price_promotion);
                    if (o.price_origin.ToFloat(0) > 0)
                        priceorigin = Convert.ToDouble(o.price_origin);

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
                    ViewState["productprice"] = price;
                    pProductPriceOriginal.Text = price.ToString();
                    if (o.quantity.ToFloat(0) > 0)
                        pQuanity.Text = Convert.ToDouble(o.quantity).ToString();
                    else pQuanity.Text = "0";
                    pRealPrice.Text = Convert.ToDouble(o.RealPrice).ToString();
                    txtproducbrand.Text = o.brand;

                    if (!string.IsNullOrEmpty(o.ProductStatus.ToString()))
                        ddlStatus.SelectedValue = o.ProductStatus.ToString();
                    else
                        ddlStatus.SelectedValue = "1";

                    string urlName = Request.UrlReferrer.ToString();
                    ltrback.Text = "<a href=\"" + urlName + "\" class=\"btn primary-btn\">Trở về</a>";
                }
            }
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            int status = ddlStatus.SelectedValue.ToString().ToInt(1);

            //Update lại giá sản phẩm
            var id = Request.QueryString["id"].ToInt(0);
            int MainOrderID = 0;
            if (id > 0)
            {
                var o = OrderController.GetAllByID(id);
                if (o != null)
                {
                    MainOrderID = Convert.ToInt32(o.MainOrderID);
                    double pprice = Convert.ToDouble(ViewState["productprice"].ToString());
                    double price = 0;
                    double pricepromotion = 0;
                    double priceorigin = 0;

                    if (o.price_promotion.ToFloat(0) > 0)
                        pricepromotion = Math.Round(Convert.ToDouble(o.price_promotion), 2);

                    if (o.price_origin.ToFloat(0) > 0)
                        priceorigin = Math.Round(Convert.ToDouble(o.price_origin), 2);

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
                    price = Math.Round(price, 2);

                    double quantity = 0;
                    if (status == 2)
                    {
                        price = 0;
                        quantity = 0;
                        var od = MainOrderController.GetAllByID(MainOrderID);
                        if (od != null)
                        {
                            var setNoti = SendNotiEmailController.GetByID(19);
                            if (setNoti != null)
                            {
                                if (setNoti.IsSentNotiUser == true)
                                {
                                    int userdathangID = Convert.ToInt32(od.UID);
                                    var userdathang = AccountController.GetByID(userdathangID);
                                    if (userdathang != null)
                                    {
                                        NotificationsController.Inser(userdathang.ID, userdathang.Username, MainOrderID,
                                                               "Đơn hàng: " + MainOrderID + " có sản phẩm bị hết hàng.",
                                                               1, DateTime.Now, obj_user.Username, true);
                                    }
                                }
                                if (setNoti.IsSendEmailUser == true)
                                {
                                    try
                                    {
                                        PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", AccountController.GetByID(Convert.ToInt32(od.UID)).Email,
                                            "Thông báo tại NHAPSICHINA.COM", "Đơn hàng: " + MainOrderID + " có sản phẩm bị hết hàng.", "");
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                        if (price.ToString() != pProductPriceOriginal.Text.ToString())
                        {
                            HistoryOrderChangeController.Insert(MainOrderID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi giá sản phẩm của Sản phẩm ID là: " + o.ID + ", của đơn hàng ID là: " + MainOrderID + ", từ: " + price.ToString() + ", sang: "
                                            + pProductPriceOriginal.Text.ToString() + "", 1, currentDate);
                        }
                        if (o.quantity != pQuanity.Text.ToString())
                        {
                            HistoryOrderChangeController.Insert(MainOrderID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi số lượng sản phẩm của Sản phẩm ID là: " + o.ID + ", của đơn hàng ID là: " + MainOrderID + ", từ: " + o.quantity + ", sang: "
                                            + pQuanity.Text + "", 1, currentDate);
                        }
                        OrderController.UpdateQuantity(id, quantity.ToString());
                        OrderController.UpdateProductStatus(id, status);
                        OrderController.UpdatePricePriceReal(id, "0", "0");
                        OrderController.UpdatePricePromotion(id, "0");
                    }
                    else
                    {
                        quantity = Convert.ToDouble(pQuanity.Text);
                        if (price.ToString() != pProductPriceOriginal.Text.ToString())
                        {
                            HistoryOrderChangeController.Insert(MainOrderID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi giá sản phẩm của Sản phẩm ID là: " + o.ID + ", của đơn hàng ID là: " + MainOrderID + ", từ: " + price.ToString() + ", sang: "
                                            + pProductPriceOriginal.Text.ToString() + "", 1, currentDate);
                        }
                        if (o.quantity != pQuanity.Text.ToString())
                        {
                            HistoryOrderChangeController.Insert(MainOrderID, obj_user.ID, obj_user.Username, obj_user.Username +
                                            " đã đổi số lượng sản phẩm của Sản phẩm ID là: " + o.ID + ", của đơn hàng ID là: " + MainOrderID + ", từ: " + o.quantity.ToString() + ", sang: "
                                            + pQuanity.Text.ToString() + "", 1, currentDate);
                        }
                        OrderController.UpdateQuantity(id, quantity.ToString());
                        OrderController.UpdateProductStatus(id, status);
                        OrderController.UpdatePricePriceReal(id, Math.Round(Convert.ToDouble(pProductPriceOriginal.Text), 2).ToString(), Math.Round(Convert.ToDouble(pRealPrice.Text), 2).ToString());
                        OrderController.UpdatePricePromotion(id, Math.Round(Convert.ToDouble(pProductPriceOriginal.Text), 2).ToString());
                    }
                    OrderController.UpdateBrand(id, txtproducbrand.Text.Trim());
                }
            }

            //Update lại giá của đơn hàng, lấy từng sản phẩm thuộc đơn hàng để lấy giá xác định rồi tổng lại rồi cộng các phí
            var listorder = OrderController.GetByMainOrderID(MainOrderID);
            var mainorder = MainOrderController.GetAllByID(MainOrderID);
            if (mainorder != null)
            {
                double current = Convert.ToDouble(mainorder.CurrentCNYVN);
                double InsurancePercent = Convert.ToDouble(mainorder.InsurancePercent);
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
                                    oprice = promotionprice * Convert.ToDouble(item.quantity) * current;
                                }
                                else
                                {
                                    pricecyn += originprice;
                                    oprice = originprice * Convert.ToDouble(item.quantity) * current;
                                }
                            }
                            else
                            {
                                pricecyn += originprice;
                                oprice = originprice * Convert.ToDouble(item.quantity) * current;
                            }
                            pricevnd += oprice;
                        }
                        pricevnd = Math.Round(pricevnd, 0);
                        pricecyn = Math.Round(pricecyn, 2);

                        MainOrderController.UpdatePriceNotFee(MainOrderID, pricevnd.ToString());
                        MainOrderController.UpdatePriceCYN(MainOrderID, pricecyn.ToString());

                        double Deposit = Math.Round(Convert.ToDouble(mainorder.Deposit), 0);
                        double FeeShipCN = Math.Round(Convert.ToDouble(mainorder.FeeShipCN), 0);
                        double FeeWeight = Math.Round(Convert.ToDouble(mainorder.FeeWeight), 0);
                        double FeeSupport = Math.Round(Convert.ToDouble(mainorder.TotalFeeSupport), 0);

                        double IsPackedPrice = 0;
                        IsPackedPrice = Math.Round(Convert.ToDouble(mainorder.IsPackedPrice), 0);
                        double IsSpecial = 0;
                        IsSpecial = Math.Round(Convert.ToDouble(mainorder.IsCheckPriceSpecial), 0);


                        var ui = AccountController.GetByID(mainorder.UID.ToString().ToInt());

                        double pricepro = pricevnd;
                        double percentbuypro = 0;
                        double percentdeposit = 0;
                        double feebpnotdc = 0;
                        double NewDeposit = 0;

                        int LevelID = 0;

                        if (pricepro > 10000000)
                            LevelID = 4;
                        else
                            LevelID = Convert.ToInt32(ui.LevelID);

                        var adminfeebuypro = FeeBuyProController.GetFeeLevelID(LevelID);
                        if (adminfeebuypro != null)
                        {
                            percentbuypro = Convert.ToDouble(adminfeebuypro.PercentService);
                            percentdeposit = Convert.ToDouble(adminfeebuypro.PercentDeposit);
                        }
                        if (Convert.ToDouble(ui.Deposit) > 0)
                        {
                            percentdeposit = Convert.ToDouble(ui.Deposit);
                        }
                        if (!string.IsNullOrEmpty(ui.FeeBuyPro))
                        {
                            if (ui.FeeBuyPro.ToFloat(0) > 0)
                            {
                                percentbuypro = Convert.ToDouble(ui.FeeBuyPro);
                                feebpnotdc = pricepro * percentbuypro / 100;
                            }
                            else
                                feebpnotdc = pricepro * percentbuypro / 100;
                        }
                        else
                            feebpnotdc = pricepro * percentbuypro / 100;
                        double feebp = Math.Round(feebpnotdc, 0);
                        if (feebp < 20000)
                            feebp = 20000;

                        double FeeBuyPro = feebp;
                        double AmountDeposit = Math.Round((pricepro * percentdeposit) / 100, 0);

                        //Cập nhật lại số tiền phải cọc
                        MainOrderController.UpdateAmountDeposit(mainorder.ID, AmountDeposit.ToString());

                        double TotalPriceVND = pricepro + FeeBuyPro + FeeWeight + IsPackedPrice + IsSpecial + FeeShipCN + FeeSupport;
                        TotalPriceVND = Math.Round(TotalPriceVND, 0);
                        double newdeposit = AmountDeposit;

                        if (Deposit > 0)
                        {
                            if (Deposit > newdeposit)
                            {
                                #region refund
                                double drefund = Math.Round(Deposit - newdeposit, 0);

                                double userwallet = 0;
                                if (ui.Wallet.ToString() != null)
                                    userwallet = Math.Round(Convert.ToDouble(ui.Wallet.ToString()), 0);

                                double wallet = userwallet + drefund;
                                wallet = Math.Round(wallet, 0);
                                AccountController.updateWallet(ui.ID, wallet, currentDate, obj_user.Username);
                                PayOrderHistoryController.Insert(MainOrderID, obj_user.ID, 12, drefund, 2, currentDate, obj_user.Username);
                                if (status == 2)
                                    HistoryPayWalletController.Insert(ui.ID, ui.Username, mainorder.ID, drefund, "Sản phẩm đơn hàng: " + mainorder.ID + " hết hàng.", wallet, 2, 2, currentDate, obj_user.Username);
                                else
                                    HistoryPayWalletController.Insert(ui.ID, ui.Username, mainorder.ID, drefund, "Sản phẩm đơn hàng: " + mainorder.ID + " giảm giá.", wallet, 2, 2, currentDate, obj_user.Username);
                                #endregion

                                NotificationController.Inser(obj_user.ID, obj_user.Username, Convert.ToInt32(mainorder.UID),
                                AccountController.GetByID(Convert.ToInt32(mainorder.UID)).Username, id,
                                "Đã có cập nhật mới về sản phẩm cho đơn hàng #" + id + " của bạn. CLick vào để xem", 0, 1, currentDate, obj_user.Username, true);

                                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                                hubContext.Clients.All.addNewMessageToPage("", "");

                                var setNoti = SendNotiEmailController.GetByID(19);
                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiUser == true)
                                    {
                                        NotificationsController.Inser(Convert.ToInt32(mainorder.UID),
                                        AccountController.GetByID(Convert.ToInt32(mainorder.UID)).Username, mainorder.ID,
                                        "Đã có cập nhật mới về sản phẩm cho đơn hàng #" + mainorder.ID + " của bạn. CLick vào để xem", 1, currentDate, obj_user.Username, true);
                                    }
                                    //if (setNoti.IsSendEmailUser == true)
                                    //{
                                    //    try
                                    //    {
                                    //        PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net",
                                    //            "jrbhnznozmlrmwvy", AccountInfoController.GetByUserID(Convert.ToInt32(mainorder.UID)).Email,
                                    //            "Thông báo tại NHAPSICHINA.COM",
                                    //            "Đã có cập nhật mới về đơn hàng #" + mainorder.ID
                                    //            + " của bạn. CLick vào để xem", "");
                                    //    }
                                    //    catch
                                    //    {

                                    //    }
                                    //}
                                }
                                //try
                                //{
                                //    PJUtils.SendMailGmail("cskh@1688pgs.vn", "1688pegasus", AccountInfoController.GetByUserID(Convert.ToInt32(mainorder.UID)).Email,
                                //        "Thông báo tại 1688PGS", "Đã có cập nhật mới về cân nặng cho đơn hàng #" + id + " của bạn. CLick vào để xem", "");
                                //}
                                //catch
                                //{

                                //}
                                //newdeposit = Deposit;
                                //MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 2);
                            }
                            else
                            {
                                if (Deposit < newdeposit)
                                {
                                    //MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 0);
                                }
                                else if (Deposit == newdeposit)
                                {
                                    //MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 2);
                                }
                                //newdeposit = Deposit;
                            }
                            //if (Deposit > TotalPriceVND)
                            //{
                            //    double drefund = Math.Round(Deposit - TotalPriceVND, 0);
                            //    double userwallet = 0;
                            //    if (ui.Wallet.ToString() != null)
                            //        userwallet = Math.Round(Convert.ToDouble(ui.Wallet.ToString()), 0);

                            //    double wallet = Math.Round(userwallet + drefund, 0);

                            //    AccountController.updateWallet(ui.ID, wallet, currentDate, obj_user.Username);
                            //    PayOrderHistoryController.Insert(MainOrderID, obj_user.ID, 12, drefund, 2, currentDate, obj_user.Username);

                            //    if (status == 2)
                            //        HistoryPayWalletController.Insert(ui.ID, ui.Username, mainorder.ID, drefund, "Sản phẩm đơn hàng: " + mainorder.ID + " hết hàng.", wallet, 2, 2, currentDate, obj_user.Username);
                            //    else
                            //        HistoryPayWalletController.Insert(ui.ID, ui.Username, mainorder.ID, drefund, "Sản phẩm đơn hàng: " + mainorder.ID + " giảm giá.", wallet, 2, 2, currentDate, obj_user.Username);

                            //    NewDeposit = TotalPriceVND;
                            //}
                            //else
                            //{
                            //    NewDeposit = Deposit;
                            //}    

                            MainOrderController.UpdateProduct(MainOrderID, AmountDeposit.ToString(), NewDeposit.ToString(), FeeBuyPro.ToString(), TotalPriceVND.ToString());
                            MainOrderController.UpdatePercentDeposit(MainOrderID, percentdeposit.ToString());
                        }
                        else
                        {
                            MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 0);
                            newdeposit = 0;
                        }
                        if (TotalPriceVND == 0)
                        {
                            MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 0);
                        }

                         MainOrderController.UpdateFeeInclueFeeSupport(MainOrderID, newdeposit.ToString(), FeeShipCN.ToString(), FeeBuyPro.ToString(), FeeWeight.ToString(),
                        IsPackedPrice.ToString(), FeeSupport.ToString(), TotalPriceVND.ToString(), IsSpecial.ToString());
                    }
                    Response.Redirect("/manager/OrderDetail.aspx?id=" + MainOrderID);
                }
            }
        }
    }
}