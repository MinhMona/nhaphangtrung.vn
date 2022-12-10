using MB.Extensions;
using Microsoft.AspNet.SignalR;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Hubs;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Telerik.Web.UI.OrgChartStyles;

namespace NHST
{
    public partial class chi_tiet_don_hang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] != null)
                {
                    // UpdatePrice();
                    loaddata();
                }
                else
                {
                    Response.Redirect("/trang-chu");
                }
            }
        }
        //public void UpdatePrice()
        //{
        //    string username_current = Session["userLoginSystem"].ToString();
        //    var obj_user = AccountController.GetByUsername(username_current);
        //    if (obj_user != null)
        //    {
        //        DateTime currentDate = DateTime.Now;
        //        var MainOrderID = RouteData.Values["id"].ToString().ToInt(0); ;
        //        if (MainOrderID > 0)
        //        {
        //            var mainorder = MainOrderController.GetAllByUIDAndID(obj_user.ID, MainOrderID);
        //            if (mainorder != null)
        //            {
        //                var listorder = OrderController.GetByMainOrderID(MainOrderID);
        //                bool IsUpdatePrice = true;
        //                if (mainorder.IsUpdatePrice != null)
        //                    IsUpdatePrice = Convert.ToBoolean(mainorder.IsUpdatePrice);
        //                if (IsUpdatePrice == false)
        //                {
        //                    double current = Convert.ToDouble(mainorder.CurrentCNYVN);
        //                    if (listorder != null)
        //                    {
        //                        if (listorder.Count > 0)
        //                        {
        //                            double pricevnd = 0;
        //                            double pricecyn = 0;
        //                            foreach (var item in listorder)
        //                            {
        //                                double originprice = Convert.ToDouble(item.price_origin);
        //                                double promotionprice = Convert.ToDouble(item.price_promotion);
        //                                double oprice = 0;
        //                                if (promotionprice > 0)
        //                                {
        //                                    if (promotionprice < originprice)
        //                                    {
        //                                        pricecyn += promotionprice;
        //                                        oprice = promotionprice * Convert.ToDouble(item.quantity) * current;
        //                                    }
        //                                    else
        //                                    {
        //                                        pricecyn += originprice;
        //                                        oprice = originprice * Convert.ToDouble(item.quantity) * current;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    pricecyn += originprice;
        //                                    oprice = originprice * Convert.ToDouble(item.quantity) * current;
        //                                }
        //                                //var oprice = Convert.ToDouble(item.price_origin) * Convert.ToDouble(item.quantity) * Convert.ToDouble(item.CurrentCNYVN) + Convert.ToDouble(item.PriceChange);

        //                                //pricecyn += item.price_origin.ToFloat();
        //                                //var oprice = Convert.ToDouble(item.price_origin) * Convert.ToDouble(item.quantity) * current;
        //                                pricevnd += oprice;
        //                            }
        //                            pricevnd = Math.Round(pricevnd, 0);
        //                            pricecyn = Math.Round(pricecyn, 2);
        //                            MainOrderController.UpdatePriceNotFee(MainOrderID, pricevnd.ToString());
        //                            MainOrderController.UpdatePriceCYN(MainOrderID, pricecyn.ToString());
        //                            double Deposit = Math.Round(Convert.ToDouble(mainorder.Deposit), 0);
        //                            double FeeShipCN = Math.Round(Convert.ToDouble(mainorder.FeeShipCN), 0);
        //                            double FeeBuyPro = Math.Round(Convert.ToDouble(mainorder.FeeBuyPro), 0);
        //                            double FeeWeight = Math.Round(Convert.ToDouble(mainorder.FeeWeight), 0);
        //                            //double FeeShipCNToVN = Convert.ToDouble(mainorder.FeeShipCNToVN);

        //                            double IsCheckProductPrice = 0;
        //                            if (mainorder.IsCheckProduct == true)
        //                            {
        //                                double total = 0;
        //                                double counpros = 0;
        //                                if (listorder.Count > 0)
        //                                {
        //                                    foreach (var item in listorder)
        //                                    {
        //                                        counpros += item.quantity.ToInt(1);
        //                                    }
        //                                }
        //                                //var count = listpro.Count;
        //                                if (counpros >= 1 && counpros <= 2)
        //                                {
        //                                    total = total + (5000 * counpros);
        //                                }
        //                                else if (counpros > 2 && counpros <= 10)
        //                                {
        //                                    total = total + (3500 * counpros);
        //                                }
        //                                else if (counpros > 10 && counpros <= 100)
        //                                {
        //                                    total = total + (2000 * counpros);
        //                                }
        //                                else if (counpros > 100 && counpros <= 500)
        //                                {
        //                                    total = total + (1500 * counpros);
        //                                }
        //                                else if (counpros > 500)
        //                                {
        //                                    total = total + (1000 * counpros);
        //                                }
        //                                IsCheckProductPrice = Math.Round(total, 0);
        //                            }
        //                            else
        //                                IsCheckProductPrice = Math.Round(Convert.ToDouble(mainorder.IsCheckProductPrice), 0);

        //                            double IsPackedPrice = 0;
        //                            IsPackedPrice = Math.Round(Convert.ToDouble(mainorder.IsPackedPrice), 0);

        //                            double IsFastDeliveryPrice = 0;
        //                            IsFastDeliveryPrice = Math.Round(Convert.ToDouble(mainorder.IsFastDeliveryPrice), 0);


        //                            double TotalPriceVND = FeeShipCN + FeeBuyPro
        //                                                    + FeeWeight + IsCheckProductPrice
        //                                                    + IsPackedPrice + IsFastDeliveryPrice
        //                                                    + Math.Round(Convert.ToDouble(mainorder.IsFastPrice), 0) + pricevnd;
        //                            TotalPriceVND = Math.Round(TotalPriceVND, 0);
        //                            double newdeposit = 0;


        //                            #region phần chỉnh sửa giá
        //                            double totalo = 0;
        //                            var ui = AccountController.GetByID(mainorder.UID.ToString().ToInt());
        //                            double UL_CKFeeBuyPro = 0;
        //                            double UL_CKFeeWeight = 0;
        //                            double LessDeposito = 0;
        //                            if (ui != null)
        //                            {
        //                                UL_CKFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(ui.LevelID.ToString().ToInt()).FeeBuyPro);
        //                                UL_CKFeeWeight = Convert.ToDouble(UserLevelController.GetByID(ui.LevelID.ToString().ToInt()).FeeWeight);
        //                                LessDeposito = Convert.ToDouble(UserLevelController.GetByID(ui.LevelID.ToString().ToInt()).LessDeposit);
        //                            }
        //                            double fastprice = 0;
        //                            double pricepro = pricevnd;
        //                            double servicefee = 0;
        //                            var adminfeebuypro = FeeBuyProController.GetAll();
        //                            if (adminfeebuypro.Count > 0)
        //                            {
        //                                foreach (var item in adminfeebuypro)
        //                                {
        //                                    if (pricepro >= item.AmountFrom && pricepro < item.AmountTo)
        //                                    {
        //                                        servicefee = Convert.ToDouble(item.FeePercent.ToString()) / 100;
        //                                    }
        //                                }
        //                            }

        //                            double feebpnotdc = pricepro * servicefee;
        //                            double subfeebp = feebpnotdc * UL_CKFeeBuyPro / 100;
        //                            //double feebp = feebpnotdc - subfeebp;
        //                            //feebp = Math.Round(feebp, 0);
        //                            double feebp = 0;
        //                            double feebuyproUser = 0;
        //                            if (!string.IsNullOrEmpty(obj_user.FeeBuyPro))
        //                            {
        //                                if (obj_user.FeeBuyPro.ToFloat(0) > 0)
        //                                {
        //                                    feebuyproUser = Convert.ToDouble(obj_user.FeeBuyPro);
        //                                }
        //                                feebp = feebuyproUser;
        //                            }
        //                            else
        //                            {
        //                                feebp = feebpnotdc - subfeebp; ;
        //                            }
        //                            feebp = Math.Round(feebp, 0);
        //                            if (mainorder.IsFast == true)
        //                            {
        //                                fastprice = Math.Round((pricepro * 5 / 100), 0);
        //                            }
        //                            totalo = Math.Round(fastprice + pricepro, 0);
        //                            double FeeCNShip = Math.Round(FeeShipCN, 0);
        //                            double FeeBuyPros = feebp;
        //                            double FeeCheck = Math.Round(IsCheckProductPrice, 0);
        //                            //totalo = totalo + FeeCNShip + FeeBuyPros + FeeCheck;
        //                            double totalNotWeight = fastprice + pricepro + FeeCNShip + FeeBuyPros + FeeCheck + IsFastDeliveryPrice;
        //                            totalo = fastprice + pricepro + FeeCNShip + FeeBuyPros + FeeCheck + FeeWeight
        //                                + IsFastDeliveryPrice;
        //                            totalo = Math.Round(totalo, 0);
        //                            //double AmountDeposit = Math.Round((totalo * LessDeposito) / 100, 0);
        //                            double AmountDeposit = Math.Round((totalNotWeight * LessDeposito) / 100, 0);

        //                            //cập nhật lại giá phải deposit của đơn hàng
        //                            MainOrderController.UpdateAmountDeposit(MainOrderID, AmountDeposit.ToString());

        //                            //giá hỏa tốc, giá sản phẩm, phí mua sản phẩm, phí ship cn, phí kiểm tra hàng
        //                            newdeposit = AmountDeposit;

        //                            //nếu đã đặt cọc rồi thì trả phí lại cho người ta
        //                            if (Deposit > 0)
        //                            {
        //                                if (Deposit > newdeposit)
        //                                {
        //                                    double drefund = Math.Round(Deposit - newdeposit, 0);
        //                                    double userwallet = 0;
        //                                    if (ui.Wallet.ToString() != null)
        //                                        userwallet = Math.Round(Convert.ToDouble(ui.Wallet.ToString()), 0);

        //                                    double wallet = Math.Round(userwallet + drefund, 0);
        //                                    AccountController.updateWallet(ui.ID, wallet, currentDate, obj_user.Username);
        //                                    PayOrderHistoryController.Insert(MainOrderID, obj_user.ID, 12, drefund, 2, currentDate, obj_user.Username);
        //                                    // HistoryOrderChangeController.Insert(mainorder.ID, obj_user.ID, username, username +
        //                                    //" đã đổi trạng thái của đơn hàng ID là: " + o.ID + ", từ: Chờ thanh toán, sang: Đã xong.", 1, currentDate);
        //                                    HistoryPayWalletController.Insert(ui.ID, ui.Username, mainorder.ID, drefund, "Sản phẩm đơn hàng: " + mainorder.ID + " giảm giá hoặc hết hàng.", wallet, 2, 2, currentDate, obj_user.Username);

        //                                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        //                                    hubContext.Clients.All.addNewMessageToPage("", "");
        //                                    MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 2);
        //                                }
        //                                else
        //                                {
        //                                    if (Deposit < newdeposit)
        //                                    {
        //                                        MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 0);
        //                                    }
        //                                    else if (Deposit == newdeposit)
        //                                    {
        //                                        MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 2);
        //                                    }
        //                                    newdeposit = Deposit;

        //                                }
        //                            }
        //                            else
        //                            {
        //                                MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 0);
        //                                newdeposit = 0;
        //                            }
        //                            if (totalo == 0)
        //                            {
        //                                MainOrderController.UpdateStatus(mainorder.ID, ui.ID, 0);
        //                            }
        //                            #endregion


        //                            MainOrderController.UpdateFee(MainOrderID, newdeposit.ToString(), FeeCNShip.ToString(), FeeBuyPros.ToString(), FeeWeight.ToString(),
        //                                FeeCheck.ToString(), IsPackedPrice.ToString(), IsFastDeliveryPrice.ToString(), totalo.ToString());
        //                        }
        //                    }
        //                    MainOrderController.UpdateIsUpdatePrice(MainOrderID, true);
        //                }
        //            }
        //        }
        //    }
        //}
        public void loaddata()
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                var conf = ConfigurationController.GetByTop1();
                int uid = obj_user.ID;
                hdfID.Value = obj_user.ID.ToString();
                #region Update Trước
                var id = RouteData.Values["id"].ToString().ToInt(0);
                if (id > 0)
                {

                    var o = MainOrderController.GetAllByUIDAndID(uid, id);
                    if (o != null)
                    {
                        double totalprice = Math.Round(Convert.ToDouble(o.TotalPriceVND), 0);
                        double feeinwarehouse = 0;
                        if (o.FeeInWareHouse != null)
                            feeinwarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);

                        double totalPay = totalprice + feeinwarehouse;
                        totalPay = Math.Round(totalPay, 0);
                        double deposited = Math.Round(Convert.ToDouble(o.Deposit), 0);
                        double leftpay = totalPay - deposited;

                        if (leftpay > 0)
                        {
                            //ddlStatus.Items.Add(new ListItem("Chờ đặt cọc", "0"));
                            //ddlStatus.Items.Add(new ListItem("Đã đặt cọc", "2"));
                            //ddlStatus.Items.Add(new ListItem("Đã mua hàng", "5"));
                            //ddlStatus.Items.Add(new ListItem("Đang về kho đích", "6"));
                            //ddlStatus.Items.Add(new ListItem("Đã nhận hàng tại kho đích", "7"));
                            ////ddlStatus.Items.Add(new ListItem("Chờ thanh toán", "8"));
                            //ddlStatus.Items.Add(new ListItem("Khách đã thanh toán", "9"));
                            //ddlStatus.Items.Add(new ListItem("Đã hoàn thành", "10"));
                            if (o.Status > 7)
                            {
                                //MainOrderController.UpdateStatus(o.ID, uid, 7);
                            }
                        }
                    }
                }
                #endregion


                //if (obj_user.RoleID == 0)
                //    ltr_currentUserImg.Text = "<img src=\"/App_Themes/NHST/images/icon.png\" width=\"100%\" />";
                //else
                //    ltr_currentUserImg.Text = "<img src=\"/App_Themes/NHST/images/user-icon.png\" width=\"100%\" />";


                double UL_CKFeeBuyPro = 0;
                double UL_CKFeeWeight = 0;

                if (UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeBuyPro.ToString().ToFloat(0) > 0)
                    UL_CKFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeBuyPro.ToString());

                if (UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeWeight.ToString().ToFloat(0) > 0)
                    UL_CKFeeWeight = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeWeight.ToString());

                //UL_CKFeeBuyPro = UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeBuyPro.ToString().ToFloat(0);
                //UL_CKFeeWeight = UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeWeight.ToString().ToFloat(0);

                if (id > 0)
                {
                    var o = MainOrderController.GetAllByUIDAndID(uid, id);
                    if (o != null)
                    {
                        hdfOrderID.Value = o.ID.ToString();
                        //txtMainOrderCode.Text = o.MainOrderCode;

                        ltrMainOrderID.Text += "Chi tiết đơn hàng #" + o.ID + "";
                        ltrChat.Text = "Hỗ trợ đơn hàng #" + o.ID + "";
                        //var config = ConfigurationController.GetByTop1();
                        double currency = 0;
                        double currency1 = 0;
                        if (!string.IsNullOrEmpty(obj_user.Currency.ToString()))
                        {
                            if (Convert.ToDouble(obj_user.Currency) > 0)
                                currency = Convert.ToDouble(obj_user.Currency);
                        }
                        else
                        {
                            var config = ConfigurationController.GetByTop1();
                            if (config != null)
                                currency = Convert.ToDouble(config.Currency);
                        }
                        currency = Convert.ToDouble(o.CurrentCNYVN);
                        currency1 = currency;
                        ViewState["OID"] = id;

                        if (!string.IsNullOrEmpty(o.ExpectedDate.ToString()))
                        {
                            ltrExpectedDate.Text += "<div class=\"arrival-info\">";
                            ltrExpectedDate.Text += "<div class=\"arrival-note\"><p>Dự kiến tới nơi</p></div>";
                            ltrExpectedDate.Text += "<div class=\"arrival-date\"><span class=\"time\">" + string.Format("{0:dd/MM/yyyy}", o.ExpectedDate) + "</span></div>";
                            ltrExpectedDate.Text += "</div>";
                        }


                        #region Lịch sử thanh toán
                        var PayorderHistory = PayOrderHistoryController.GetAllByMainOrderID(o.ID);
                        if (PayorderHistory.Count > 0)
                        {
                            rptPayment.DataSource = PayorderHistory;
                            rptPayment.DataBind();
                        }
                        else
                        {
                            ltrHistoryPay.Text = "<tr class=\"noti\"><td class=\"red-text\" colspan=\"4\">Chưa có lịch sử thanh toán nào.</td></tr>";
                        }
                        #endregion
                        StringBuilder shtml = new StringBuilder();
                        #region Load Mã vận đơn 
                        var lmvd = SmallPackageController.GetByMainOrderID(o.ID);
                        if (lmvd.Count > 0)
                        {
                            var SmallNew = lmvd.Where(x => x.Status == 1).ToList();
                            if (SmallNew.Count > 0)
                            {
                                string mvd = "";
                                foreach (var item in SmallNew)
                                {
                                    mvd += item.OrderTransactionCode + ";";
                                }
                                shtml.Append("<div class=\"info-top\">");
                                shtml.Append("<span class=\"bill\">Mã vận đơn: <span class=\"bold black-text code\">" + mvd + "</span></span>");
                                shtml.Append("<span class=\"status incoming\">Mới đặt</span>");
                                shtml.Append("</div>");
                            }

                            var SmallInTQ = lmvd.Where(x => x.Status == 2).ToList();
                            if (SmallInTQ.Count > 0)
                            {
                                string mvd = "";
                                foreach (var item in SmallInTQ)
                                {
                                    mvd += item.OrderTransactionCode + ";";
                                }
                                shtml.Append("<div class=\"info-top\">");
                                shtml.Append("<span class=\"bill\">Mã vận đơn: <span class=\"bold black-text code\">" + mvd + "</span></span>");
                                shtml.Append("<span class=\"status incoming\">Đã về kho TQ</span>");
                                shtml.Append("</div>");
                            }

                            var SmallInVN = lmvd.Where(x => x.Status == 3).ToList();
                            if (SmallInVN.Count > 0)
                            {
                                string mvd = "";
                                foreach (var item in SmallInVN)
                                {
                                    mvd += item.OrderTransactionCode + ";";
                                }
                                shtml.Append("<div class=\"info-top\">");
                                shtml.Append("<span class=\"bill\">Mã vận đơn: <span class=\"bold black-text code\">" + mvd + "</span></span>");
                                shtml.Append("<span class=\"status incoming\">Đã về kho VN</span>");
                                shtml.Append("</div>");
                            }
                        }
                        else
                        {
                            shtml.Append("<div class=\"info-top\">");
                            shtml.Append("<span class=\"bill\">Mã vận đơn: <span class=\"bold black-text code\"></span></span>");
                            shtml.Append("<span class=\"status incoming\">Chờ đặt hàng</span>");
                            shtml.Append("</div>");
                        }
                        ltrSmallInfo.Text = shtml.ToString();
                        #endregion

                        #region load Map
                        List<warehouses> lwh = new List<warehouses>();
                        var khoTQ = WarehouseFromController.GetByID(o.FromPlace.Value);
                        if (khoTQ != null)
                        {
                            warehouses wh = new warehouses();
                            wh.name = khoTQ.WareHouseName;
                            wh.lat = khoTQ.Latitude;
                            wh.lng = khoTQ.Longitude;

                            ltrTQ.Text = "<div class=\"from\"><span class=\"lb position\" data-lat=\"" + khoTQ.Latitude + "\" data-lng=\"" + khoTQ.Longitude + "\" id=\"js-map-from\">" + khoTQ.WareHouseName + "</span></div>";

                            var lsmall = SmallPackageController.GetByMainOrderID(o.ID);
                            if (lsmall.Count > 0)
                            {
                                var inTQ = lsmall.Where(x => Convert.ToInt32(x.CurrentPlaceID) == khoTQ.ID && x.Status == 2).ToList();
                                if (inTQ.Count > 0)
                                {
                                    List<package> lpc = new List<package>();
                                    foreach (var item in inTQ)
                                    {
                                        package pk = new package();
                                        pk.code = item.OrderTransactionCode;
                                        pk.status = "Đang vận chuyển";
                                        pk.classColor = "being-transport";
                                        lpc.Add(pk);
                                    }
                                    wh.package = lpc;
                                }
                            }
                            lwh.Add(wh);
                        }

                        var khoVN = WarehouseController.GetByID(Convert.ToInt32(o.ReceivePlace));
                        if (khoVN != null)
                        {
                            warehouses wh = new warehouses();
                            wh.name = khoVN.WareHouseName;
                            wh.lat = khoVN.Latitude;
                            wh.lng = khoVN.Longitude;

                            ltrVN.Text = "<div class=\"to\"><span class=\"lb position\" data-lat=\"" + khoVN.Latitude + "\" data-lng=\"" + khoVN.Longitude + "\" id=\"js-map-to\">" + khoVN.WareHouseName + "</span></div>";

                            var lsmall = SmallPackageController.GetByMainOrderID(o.ID);
                            if (lsmall.Count > 0)
                            {
                                var inVN = lsmall.Where(x => Convert.ToInt32(x.CurrentPlaceID) == khoVN.ID && x.Status == 3).ToList();
                                if (inVN.Count > 0)
                                {
                                    List<package> lpc = new List<package>();
                                    foreach (var item in inVN)
                                    {
                                        package pk = new package();
                                        pk.code = item.OrderTransactionCode;
                                        pk.status = "Đã về kho đích";
                                        pk.classColor = "transported";
                                        lpc.Add(pk);
                                    }
                                    wh.package = lpc;
                                }
                            }
                            lwh.Add(wh);
                        }


                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        hdfLoadMap.Value = serializer.Serialize(lwh);

                        #endregion

                        #region Lịch sử thay đổi
                        //var OrderChange = HistoryOrderChangeController.GetByMainOrderID(o.ID);
                        //if (OrderChange.Count > 0)
                        //{
                        //    rptHistoryOrderChange.DataSource = OrderChange;
                        //    rptHistoryOrderChange.DataBind();
                        //}
                        //else
                        //{
                        //    ltrHistory.Text = "<tr class=\"noti\"><td class=\"red-text\" colspan=\"4\">Chưa có lịch sử thay đổi nào.</td></tr>";
                        //}
                        #endregion


                        double pricepro = Math.Round(Convert.ToDouble(o.PriceVND), 0);
                        double servicefee = 0;
                        var adminfeebuypro = FeeBuyProController.GetAll();
                        if (adminfeebuypro.Count > 0)
                        {
                            foreach (var item in adminfeebuypro)
                            {
                                if (pricepro >= item.AmountFrom && pricepro < item.AmountTo)
                                {
                                    double feePercent = 0;
                                    if (item.FeePercent.ToString().ToFloat(0) > 0)
                                        feePercent = Convert.ToDouble(item.FeePercent);
                                    servicefee = feePercent / 100;
                                }
                            }
                        }
                        double feebpnotdc = 0;
                        if (pricepro >= 1000000)
                        {
                            feebpnotdc = pricepro * servicefee;
                        }
                        else
                        {
                            feebpnotdc = 30000;
                        }
                        double subfeebp = feebpnotdc * UL_CKFeeBuyPro / 100;
                        double userdadeposit = 0;
                        if (o.Deposit != null)
                            userdadeposit = Math.Round(Convert.ToDouble(o.Deposit), 0);

                        //Hiển thị nút thanh toán
                        double feewarehouse = 0;
                        if (o.FeeInWareHouse != null)
                            feewarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);
                        double totalPrice = Math.Round(Convert.ToDouble(o.TotalPriceVND), 0);
                        double totalPay = totalPrice + feewarehouse;
                        double totalleft = totalPay - userdadeposit;
                        if (totalleft > 0)
                        {
                            if (obj_user.Wallet >= totalleft)
                            {
                                if (o.Status > 6)
                                {
                                    if (o.IsGiaohang != true)
                                        ltrYCG.Text = " <a href=\"#addPackage\" style=\"display:none\" class=\"btn modal-trigger btn-complain\">Yêu cầu giao hàng</a>";
                                    pnthanhtoan.Visible = true;
                                }
                            }
                        }

                        //Hiển thị nút đặt cọc và hủy đơn hàng

                        if (o.OrderType == 3)
                        {
                            if (o.IsCheckNotiPrice == true)
                            {
                                ltrCancel.Text += "   <a href=\"javascript:;\" class=\"btn btn-cancel orange accent-4\" onclick=\"cancelOrder()\">Xóa đơn hàng</a>";
                            }
                            else
                            {
                                if (o.Status == 0 && Convert.ToDouble(o.Deposit) < Convert.ToDouble(o.AmountDeposit) && Convert.ToDouble(o.TotalPriceVND) > 0)
                                {
                                    ltrbtndeposit.Text += "     <a href=\"javascript:;\" class=\"btn btn-deposit\" onclick=\"depositOrder($(this))\">Đặt cọc</a>";
                                    ltrCancel.Text += "   <a href=\"javascript:;\" class=\"btn btn-cancel orange accent-4\" onclick=\"cancelOrder()\">Xóa đơn hàng</a>";
                                }
                            }
                        }
                        else
                        {
                            if (o.Status == 0 && Convert.ToDouble(o.Deposit) < Convert.ToDouble(o.AmountDeposit) && Convert.ToDouble(o.TotalPriceVND) > 0)
                            {
                                ltrbtndeposit.Text += "   <a href=\"javascript:;\" class=\"btn btn-deposit\" onclick=\"depositOrder($(this))\">Đặt cọc</a>";
                                ltrCancel.Text += "   <a href=\"javascript:;\" class=\"btn btn-cancel orange accent-4\" onclick=\"cancelOrder()\">Xóa đơn hàng</a>";
                            }
                        }
                        if (o.Status == 1)
                        {
                            ltrbtndeposit.Text += "   <a href=\"#modalOrder\" id=\"Order\" onclick=\"Order(" + o.ID + ")\" class=\"btn btn-deposit modal-trigger\" data-position=\"top\">Đặt lại đơn hàng</a>";
                            loadinfoSP(o.ID.ToString());
                        }


                        #region lấy tất cả kiện
                        var smallpackages = SmallPackageController.GetByMainOrderID(o.ID);
                        if (smallpackages.Count > 0)
                        {
                            foreach (var s in smallpackages)
                            {
                                double sokhoi = 0;
                                if (s.Height > 0 && s.Width > 0 && s.Length > 0)
                                    sokhoi = Convert.ToDouble(s.Height) * Convert.ToDouble(s.Width) * Convert.ToDouble(s.Length) / 1000000;

                                var usercreate = AccountController.GetByID(uid);

                                int LevelID = 0;
                                if (Convert.ToDouble(o.PriceVND) > 10000000)
                                    LevelID = 4;
                                else
                                    LevelID = Convert.ToInt32(usercreate.LevelID);

                                double pricePerWeight = 0;
                                double pricePerVolume = 0;
                                int warehouse = o.ReceivePlace.ToInt(1);
                                //Phí vận chuyển /kg và /m3
                                var fee = FeeBuyProController.GetFeeLevelID(LevelID);
                                if (fee != null)
                                {
                                    if (warehouse == 1)
                                    {
                                        pricePerWeight = Convert.ToDouble(fee.FeeWeightHN);
                                        pricePerVolume = Convert.ToDouble(fee.FeeVolumeHN);
                                    }
                                    else
                                    {
                                        pricePerWeight = Convert.ToDouble(fee.FeeWeightSG);
                                        pricePerVolume = Convert.ToDouble(fee.FeeVolumeSG);
                                    }
                                }
                                if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerWeight))
                                {
                                    if (usercreate.FeeTQVNPerWeight.ToFloat(0) > 0)
                                    {
                                        pricePerWeight = Convert.ToDouble(usercreate.FeeTQVNPerWeight);
                                    }
                                }

                                double smPriceWeight = (s.Weight ?? 0) * pricePerWeight;
                                double smPriceVolume = sokhoi * pricePerVolume;
                                double smPriceTT = 0;

                                ltrSmallPackages.Text += "<tr class=\"slide-up\">";
                                ltrSmallPackages.Text += "<td>" + s.OrderTransactionCode + "</td>";
                                ltrSmallPackages.Text += "<td>" + s.Length + " x " + s.Width + " x " + s.Height + "</td>";
                                if (smPriceWeight > smPriceVolume)
                                {
                                    ltrSmallPackages.Text += "<td>" + Math.Round(sokhoi, 5) + "</td>";
                                    ltrSmallPackages.Text += "<td style=\" font-weight: bold; \">" + Math.Round(Convert.ToDouble(s.Weight), 1) + "</td>";
                                }
                                else
                                {
                                    ltrSmallPackages.Text += "<td style=\" font-weight: bold; \">" + Math.Round(sokhoi, 5) + "</td>";
                                    ltrSmallPackages.Text += "<td>" + Math.Round(Convert.ToDouble(s.Weight), 1) + "</td>";
                                }

                                ltrSmallPackages.Text += "<td>" + string.Format("{0:N0}", s.TotalPrice) + "</td>";
                                ltrSmallPackages.Text += "<td><span>" + s.Description + "</span></td>";
                                ltrSmallPackages.Text += "<td>" + PJUtils.IntToStringStatusSmallPackageWithBGNew(Convert.ToInt32(s.Status)) + "</td>";
                                ltrSmallPackages.Text += "</tr>";
                            }
                        }
                        #endregion
                        #region lấy tất cả phụ phí
                        var listsp = FeeSupportController.GetAllByMainOrderID(o.ID);
                        if (listsp.Count > 0)
                        {
                            foreach (var item in listsp)
                            {
                                ltrFeeSupport.Text += "<tr>";
                                ltrFeeSupport.Text += "<td>" + item.SupportName + "</td>";
                                ltrFeeSupport.Text += "<td>" + item.SupportInfoVND + "</td>";
                                //ltrFeeSupport.Text += "<td class=\"\"><a href=\"javascript:;\" onclick=\"deleteSupportFee($(this))\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons valign-center\">remove_circle</i></a></td>";
                                ltrFeeSupport.Text += "</tr>";
                            }
                        }
                        #endregion
                        #region Lấy thông tin người đặt
                        var ui = AccountInfoController.GetByUserID(uid);
                        if (ui != null)
                        {
                            ltrBuyerInfo.Text += "<tr>";
                            ltrBuyerInfo.Text += "<td>Tên</td>";
                            ltrBuyerInfo.Text += "<td>" + ui.FirstName + " " + ui.LastName + "</td>";
                            ltrBuyerInfo.Text += "</tr>";
                            ltrBuyerInfo.Text += "<tr>";
                            ltrBuyerInfo.Text += "<td>Địa chỉ</td>";
                            ltrBuyerInfo.Text += "<td>" + ui.Address + "</td>";
                            ltrBuyerInfo.Text += "</tr>";
                            ltrBuyerInfo.Text += "<tr>";
                            ltrBuyerInfo.Text += "<td>Email</td>";
                            ltrBuyerInfo.Text += "<td> " + ui.Email + " </td>";
                            ltrBuyerInfo.Text += "</tr>";
                            ltrBuyerInfo.Text += "<tr>";
                            ltrBuyerInfo.Text += "<td>Số ĐT</td>";
                            ltrBuyerInfo.Text += "<td>" + ui.Phone + "</td>";
                            ltrBuyerInfo.Text += "</tr>";
                            ltrBuyerInfo.Text += "<tr>";
                            ltrBuyerInfo.Text += "<td>Ghi chú</td>";
                            ltrBuyerInfo.Text += "<td>" + o.Note + "</td>";
                            ltrBuyerInfo.Text += "</tr>";
                        }
                        #endregion

                        int totalproduct = 0;
                        #region Lấy sản phẩm
                        List<tbl_Order> lo = new List<tbl_Order>();
                        lo = OrderController.GetByMainOrderID(o.ID);
                        if (lo.Count > 0)
                        {
                            int stt = 1;
                            foreach (var item in lo)
                            {
                                double currentcyt = 0;
                                if (item.CurrentCNYVN.ToFloat(0) > 0)
                                    currentcyt = Convert.ToDouble(item.CurrentCNYVN);

                                double price = 0;
                                double pricepromotion = 0;
                                if (item.price_promotion.ToFloat(0) > 0)
                                    pricepromotion = Convert.ToDouble(item.price_promotion);

                                double priceorigin = 0;
                                if (item.price_origin.ToFloat(0) > 0)
                                    priceorigin = Convert.ToDouble(item.price_origin);

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

                                ltrProducts.Text += "<div class=\"item-wrap\">";
                                ltrProducts.Text += "<div class=\"item-name\">";
                                ltrProducts.Text += "<div class=\"number\"><span class=\"count\">" + stt + "</span></div>";
                                ltrProducts.Text += "<div class=\"name\"><span class=\"item-img\"><img src=\"" + item.image_origin + "\"alt=\"image\"></span>";
                                ltrProducts.Text += "<div class=\"caption\"><a href=\"" + item.link_origin + "\" class=\"title black-text\">" + item.title_origin + "</a>";
                                ltrProducts.Text += "<div class=\"item-price mt-1\"><span class=\"pr-2 black-text font-weight-600\">" + item.property + "</span>";
                                ltrProducts.Text += "</div>";
                                ltrProducts.Text += "<div class=\"note\"><span class=\"black-text font-weight-500\">Ghi chú: </span>";
                                ltrProducts.Text += "<div class=\"input-field inline\"><input type=\"text\" value=\"" + item.brand + "\" class=\"validate\" id=\"note_" + item.ID + "\"></div>";
                                ltrProducts.Text += "</div>";
                                //if (o.Status > 1)
                                //{
                                //    ltrProducts.Text += "<div class=\"capnhat\" style=\"display: flex; justify-content: center;\">";
                                //    ltrProducts.Text += "<a href=\"javascript:;\" onclick=\"updateNote('" + item.ID + "')\" style=\"\" class=\"btn-update tooltipped position-ab\" data-position=\"top\" data-tooltip=\"Cập nhật sản phẩm này\">Cập nhật ghi chú  <i class=\"material-icons\" style=\"position:absolute;\">sync</i></a>";
                                //    ltrProducts.Text += "</div>";
                                //}                               
                                ltrProducts.Text += "</div>";
                                ltrProducts.Text += "</div>";
                                ltrProducts.Text += "</div>";
                                ltrProducts.Text += "<div class=\"item-info\">";
                                ltrProducts.Text += "<div class=\"item-num column\"><span class=\"black-text\"><strong>Số lượng</strong></span><p>" + item.quantity + "</p><p></p></div>";
                                ltrProducts.Text += "<div class=\"item-price column\"><span class=\"black-text\"><strong>Đơn giá</strong></span>";
                                ltrProducts.Text += "<p class=\"grey-text font-weight-500\">¥" + string.Format("{0:0.##}", price) + "</p>";
                                ltrProducts.Text += "<p class=\"grey-text font-weight-500\">" + string.Format("{0:N0}", vndprice) + " VNĐ</p>";
                                ltrProducts.Text += "</div>";
                                ltrProducts.Text += "<div class=\"item-num column\"><span class=\"black-text\"><strong>Tổng tệ</strong></span><p>" + string.Format("{0:N0}", price * Convert.ToInt32(item.quantity)) + " ¥</p><p></p></div>";
                                ltrProducts.Text += "<div class=\"item-num column\"><span class=\"black-text\"><strong>Tổng tiền</strong></span><p>" + string.Format("{0:N0}", vndprice * Convert.ToInt32(item.quantity)) + " VNĐ</p><p></p></div>";
                                ltrProducts.Text += "<div class=\"item-status column\"><span class=\"black-text\"><strong>Trạng thái</strong></span>";
                                if (!string.IsNullOrEmpty(item.ProductStatus.ToString()))
                                {
                                    if (item.ProductStatus == 1)
                                        ltrProducts.Text += "<p class=\"green-text\">Còn hàng</p>";
                                    else
                                        ltrProducts.Text += "<p class=\"red-text\">Hết hàng</p>";
                                }
                                else
                                {
                                    ltrProducts.Text += "<p class=\"green-text\">Còn hàng</p>";
                                }

                                ltrProducts.Text += "</div>";
                                ltrProducts.Text += "</div>";
                                ltrProducts.Text += "</div>";

                                totalproduct += Convert.ToInt32(item.quantity);

                                stt++;
                            }
                        }
                        ltrTotalProduct.Text = totalproduct.ToString();
                        double currencyOrder = 0;
                        if (o.CurrentCNYVN.ToFloat(0) > 0)
                            currencyOrder = Convert.ToDouble(o.CurrentCNYVN);

                        ltrService.Text += "<li><span class=\"lbl\">Tiền hàng</span><span class=\"value\">" + string.Format("{0:N0}", Convert.ToDouble(o.PriceVND)) + " VNĐ ~ (<i class=\"fa fa-yen\"></i>" + string.Format("{0:#.##}", Convert.ToDouble(o.PriceVND) / o.CurrentCNYVN.ToFloat()) + ")</span></li>";
                        if (!string.IsNullOrEmpty(o.FeeBuyPro))
                        {
                            double bp = Convert.ToDouble(o.FeeBuyPro);
                            if (bp > 0)
                            {
                                if (UL_CKFeeBuyPro > 0)
                                    ltrService.Text += "<li><span class=\"lbl\">Phí mua hàng (Đã CK " + UL_CKFeeBuyPro + "% : " + string.Format("{0:N0}", subfeebp) + " VNĐ)</span><span class=\"value\">" + string.Format("{0:N0}", bp) + " VNĐ</span></li>";
                                else
                                    ltrService.Text += "<li><span class=\"lbl\">Phí mua hàng</span><span class=\"value\">" + string.Format("{0:N0}", bp) + " VNĐ</span></li>";
                            }
                            else
                                ltrService.Text += "<li><span class=\"lbl\">Phí mua hàng</span><span class=\"value\">Đang cập nhật</span></li>";
                        }
                        else
                            ltrService.Text += "<li><span class=\"lbl\">Phí mua hàng</span><span class=\"value\">Đang cập nhật</span></li>";


                        ltrService.Text += "<li style=\"display:none\"><span class=\"lbl\">Phí kiểm đếm</span><span class=\"value\">" + string.Format("{0:N0}", o.IsCheckProductPrice.ToFloat(0)) + " VNĐ</span></li>";
                        //ltrService.Text += "<li><span class=\"lbl\">Phí bảo hiểm </span><span class=\"value\">5,220,000 VNĐ</span></li>";
                        ltrService.Text += "<li><span class=\"lbl\">Phí đóng gỗ</span><span class=\"value\">" + string.Format("{0:N0}", Convert.ToDouble(o.IsPackedPrice)) + " VNĐ</span></li>";

                        ltrService.Text += "<li><span class=\"lbl\">Phí đặc biệt</span><span class=\"value\">" + string.Format("{0:N0}", Convert.ToDouble(o.IsCheckPriceSpecial)) + " VNĐ</span></li>";

                        ltrService.Text += "<li style=\"display:none\"><span class=\"lbl\">Phí bảo hiểm</span><span class=\"value\">" + string.Format("{0:N0}", Convert.ToDouble(o.InsuranceMoney)) + " VNĐ</span></li>";

                        if (!string.IsNullOrEmpty(o.FeeShipCN))
                        {
                            double fscn = Math.Floor(Convert.ToDouble(o.FeeShipCN));
                            double phhinoidiate = fscn / currency1;
                            ltrService.Text += "<li><span class=\"lbl\">Phí ship nội địa TQ</span><span class=\"value\">" + string.Format("{0:N0}", Convert.ToDouble(o.FeeShipCN)) + " VNĐ (¥ " + phhinoidiate + ")</span></li>";
                        }
                        else
                            ltrService.Text += "<li><span class=\"lbl\">Phí ship nội địa TQ</span><span class=\"value\">Đang cập nhật</span></li>";

                        ltrService.Text += "<li style=\" display: none; \"><span class=\"lbl\">Cân nặng</span><span class=\"value\">" + o.TQVNWeight + " kg</span></li>";
                        ltrService.Text += "<li style=\" display: none; \"><span class=\"lbl\">Số khối</span><span class=\"value\">" + o.TQVNVolume + " khối</span></li>";

                        //if (UL_CKFeeWeight > 0)
                        //{
                        //    ltrService.Text += "<li><span class=\"lbl\">Phí cân nặng (Đã CK " + UL_CKFeeWeight + "% : " + string.Format("{0:N0}", o.FeeWeightCK.ToFloat(0) > 0 ? Convert.ToDouble(o.FeeWeightCK) : 0) + " VNĐ)</span><span class=\"value\">" + o.TQVNWeight + " kg - " + string.Format("{0:N0}", o.FeeWeight.ToFloat(0)) + " VNĐ</span></li>";
                        //}
                        //else
                        //{
                        //    ltrService.Text += "<li><span class=\"lbl\">Phí cân nặng</span><span class=\"value\">" + o.TQVNWeight + " kg - " + string.Format("{0:N0}", o.FeeWeight.ToFloat(0)) + " VNĐ</span></li>";
                        //}
                        ltrService.Text += "<li><span class=\"lbl\">Phí vận chuyển TQVN</span><span class=\"value\">" + string.Format("{0:N0}", o.FeeWeight.ToFloat(0)) + " VNĐ</span></li>";

                        double feeinwarehouse = 0;
                        if (o.FeeInWareHouse != null)
                        {
                            feeinwarehouse = Convert.ToDouble(o.FeeInWareHouse);
                        }

                        if (feeinwarehouse > 0)
                        {
                            ltrService.Text += "<li><span class=\"lbl\">Phí lưu kho</span><span class=\"value\">" + string.Format("{0:N0}", feeinwarehouse) + " VNĐ</span></li>";
                        }
                        #endregion

                        #region Tổng tiền
                        ltrTotal.Text += "<li class=\"\"><span class=\"lbl\">Tổng tiền đơn hàng</span><span class=\"value \">" + string.Format("{0:N0}", Convert.ToDouble(o.TotalPriceVND) + feeinwarehouse) + " VNĐ</span></li>";
                        if (!string.IsNullOrEmpty(o.AmountDeposit))
                            ltrTotal.Text += "<li class=\"\"><span class=\"lbl\">Số tiền phải cọc</span><span class=\"value \">" + string.Format("{0:N0}", Convert.ToDouble(o.AmountDeposit)) + " VNĐ</span></li>";

                        double deposit = 0;
                        if (!string.IsNullOrEmpty(o.Deposit))
                        {
                            deposit = Convert.ToDouble(o.Deposit);
                            ltrTotal.Text += "<li class=\"\"><span class=\"lbl\">Đã thanh toán</span><span class=\"value \">" + string.Format("{0:N0}", deposit) + " VNĐ</span></li>";
                        }
                        else
                            ltrTotal.Text += "<li class=\"\"><span class=\"lbl\">Đã thanh toán</span><span class=\"value \">Chưa đặt cọc</span></li>";

                        ltrTotal.Text += "<li class=\"\"><span class=\"lbl\">Còn lại</span><span class=\"value red-text font-weight-700\">" + string.Format("{0:N0}", Convert.ToDouble(o.TotalPriceVND) + feeinwarehouse - deposit) + " VNĐ</span></li>";
                        #endregion

                        #region Tổng quan
                        ltrOverView.Text += "<div class=\"col s12 m6\">";
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Trạng thái đơn hàng: </span></div>";
                        if (o.OrderType == 3)
                        {
                            if (o.IsCheckNotiPrice == true)
                            {
                                ltrOverView.Text += "<div class=\"right-content\"><span class=\"badge yellow-gold darken-2 left m-0\">Chờ báo giá</span></div>";
                            }
                            else
                            {
                                ltrOverView.Text += "<div class=\"right-content\">" + PJUtils.IntToRequestClientNew(Convert.ToInt32(o.Status)) + "</div>";
                            }
                        }
                        else
                        {
                            ltrOverView.Text += "<div class=\"right-content\">" + PJUtils.IntToRequestClientNew(Convert.ToInt32(o.Status)) + "</div>";
                        }

                        ltrOverView.Text += "</div>";
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Tổng tiền đơn hàng: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + string.Format("{0:N0}", Convert.ToDouble(o.TotalPriceVND) + feeinwarehouse) + " VNĐ</span></div>";
                        ltrOverView.Text += "</div>";
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Số tiền phải cọc: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + string.Format("{0:N0}", Convert.ToDouble(o.AmountDeposit)) + " VNĐ</span></div>";
                        ltrOverView.Text += "</div>";
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Đã thanh toán: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + string.Format("{0:N0}", Convert.ToDouble(o.Deposit)) + " VNĐ</span></div>";
                        ltrOverView.Text += "</div>";
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Còn lại: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"red-text font-weight-700\">" + string.Format("{0:N0}", Convert.ToDouble(o.TotalPriceVND) + feeinwarehouse - deposit) + " VNĐ</span></div>";
                        ltrOverView.Text += "</div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"col s12 m6\">";
                        //ltrOverView.Text += "<div class=\"order-row\">";
                        //ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Kho TQ: </span></div>";
                        //ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + WarehouseFromController.GetByID(Convert.ToInt32(o.FromPlace)).WareHouseName + "</span></div>";
                        //ltrOverView.Text += "</div>";
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Kho nhận: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + WarehouseController.GetByID(Convert.ToInt32(o.ReceivePlace)).WareHouseName + "</span></div>";
                        ltrOverView.Text += "</div>";
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Phương thức VC: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + ShippingTypeToWareHouseController.GetByID(Convert.ToInt32(o.ShippingType)).ShippingTypeName + "</span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\" style=\"display:none\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Kiểm đếm: </span></div>";
                        if (o.IsCheckProduct == true)
                            ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\"><i class=\"material-icons green-text left\">check_circle</i></span></div>";
                        else
                            ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\"><i class=\"material-icons red-text left\">clear</i></span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Đóng gỗ: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\">";
                        if (o.IsPacked == true)
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons green-text left\">check_circle</i></span></div>";
                        else
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons red-text left\">clear</i></span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        if (o.IsCheckSpecial1 == true)
                            ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Phí đặc biệt 1 (" + string.Format("{0:N0}", Convert.ToDouble(conf.FeeDacBiet1)) + "đ/kg): </span></div>";
                        else
                            ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Phí đặc biệt 1: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\">";
                        if (o.IsCheckSpecial1 == true)
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons green-text left\">check_circle</i></span></div>";
                        else
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons red-text left\">clear</i></span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        if (o.IsCheckSpecial2 == true)
                            ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Phí đặc biệt 2 (" + string.Format("{0:N0}", Convert.ToDouble(conf.FeeDacBiet2)) + "đ/kg): </span></div>";
                        else
                            ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Phí đặc biệt 2: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\">";
                        if (o.IsCheckSpecial2 == true)
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons green-text left\">check_circle</i></span></div>";
                        else
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons red-text left\">clear</i></span></div>";
                        ltrOverView.Text += "</div>";



                        ltrOverView.Text += "<div class=\"order-row\" style=\"display:none\">";
                        ltrOverView.Text += "<div class=\"left-fixed\" style=\"display: none;\"><span class=\"lb\">Bảo hiểm: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\" style=\"display: none;\">";
                        if (o.IsInsurrance == true)
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons green-text left\">check_circle</i></span></div>";
                        else
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons red-text left\">clear</i></span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\" style=\"display: none;\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Giao hàng tận nhà: </span></div>";
                        if (o.IsFastDelivery == true)
                            ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\"><i class=\"material-icons green-text left\">check_circle</i></span></div>";
                        else
                            ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\"><i class=\"material-icons red-text left\">clear</i></span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "</div>";
                        #endregion
                        #region Lấy bình luận
                        ltrComment.Text += "<div class=\"comment mar-bot2\">";
                        ltrComment.Text += "     <div class=\"comment_content\" seller=\"" + o.ShopID + "\" order=\"" + o.ID + "\" >";
                        var shopcomments = OrderCommentController.GetByOrderIDAndType(o.ID, 1);
                        StringBuilder chathtml = new StringBuilder();
                        if (shopcomments.Count > 0)
                        {
                            foreach (var item in shopcomments)
                            {
                                string fullname = "";
                                int role = 0;
                                var user = AccountController.GetByID(Convert.ToInt32(item.CreatedBy));
                                var userinfo = AccountInfoController.GetByUserID(user.ID);
                                if (user != null)
                                {
                                    role = Convert.ToInt32(user.RoleID);

                                    if (userinfo != null)
                                    {
                                        fullname = userinfo.FirstName + " " + userinfo.LastName;
                                    }
                                }
                                if (role == 1)
                                {
                                    chathtml.Append("<div class=\"chat chat-right\">");
                                }
                                else
                                {
                                    chathtml.Append("<div class=\"chat\">");
                                }
                                chathtml.Append("<div class=\"chat-avatar\">");
                                chathtml.Append("    <p class=\"name\">" + fullname + "</p>");
                                chathtml.Append("</div>");
                                chathtml.Append("<div class=\"chat-body\">");
                                chathtml.Append("        <div class=\"chat-text\">");
                                chathtml.Append("                <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</div>");
                                chathtml.Append("                <div class=\"text-content\">");
                                chathtml.Append("                    <div class=\"content\">");
                                if (!string.IsNullOrEmpty(item.Link))
                                {
                                    chathtml.Append("<div class=\"content-img\">");
                                    chathtml.Append("<div class=\"img-block\">");
                                    if (item.Link.Contains(".doc"))
                                    {
                                        chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/UserNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");

                                    }
                                    else if (item.Link.Contains(".xls"))
                                    {
                                        chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"/App_Themes/UserNew45/assets/images/icon/file.png\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    else
                                    {
                                        chathtml.Append("<a href=\"" + item.Link + "\" target=\"_blank\"><img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/></a>");
                                    }
                                    //chathtml.Append("<img src=\"" + item.Link + "\" title=\"" + item.Comment + "\"  class=\"\" height=\"50\"/>");
                                    chathtml.Append("</div>");
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
                        ltrComment.Text = chathtml.ToString();
                        //ltrComment.Text += "     </div>";
                        //ltrComment.Text += "     <div class=\"comment_action\" style=\"padding-bottom: 4px; padding-top: 4px;\">";
                        //ltrComment.Text += "         <input shop_code=\"" + o.ID + "\" type=\"text\" class=\"comment-text\" order=\"188083\" seller=\"" + o.ShopID + "\" placeholder=\"Nội dung\">";
                        ////ltrComment.Text += "         <a id=\"sendnotecomment\" onclick=\"postcomment($(this))\" order=\"" + o.ID + "\" class=\"btn pill-btn primary-btn main-btn hover\" href=\"javascript:;\" style=\"min-width:10px;\">Gửi</a>";
                        //ltrComment.Text += "         <a id=\"sendnotecomment\" order=\"" + o.ID + "\" class=\"btn pill-btn primary-btn main-btn hover\" href=\"javascript:;\" style=\"min-width:10px;\">Gửi</a>";
                        //ltrComment.Text += "     </div>";
                        //ltrComment.Text += "</div>";








                        //var cs = OrderCommentController.GetByOrderIDAndType(o.ID, 1);
                        //if (cs != null)
                        //{
                        //    if (cs.Count > 0)
                        //    {
                        //        foreach (var item in cs)
                        //        {
                        //            string fullname = "";
                        //            int role = 0;
                        //            var user = AccountController.GetByID(Convert.ToInt32(item.CreatedBy));
                        //            if (user != null)
                        //            {
                        //                role = Convert.ToInt32(user.RoleID);
                        //                var userinfo = AccountInfoController.GetByUserID(user.ID);
                        //                if (userinfo != null)
                        //                {
                        //                    fullname = userinfo.FirstName + " " + userinfo.LastName;
                        //                }
                        //            }
                        //            ltr_comment.Text += "<li class=\"item\">";
                        //            ltr_comment.Text += "   <div class=\"item-left\">";
                        //            if (role == 0)
                        //            {
                        //                ltr_comment.Text += "       <span class=\"avata circle\"><img src=\"/App_Themes/NHST/images/icon.png\" width=\"100%\" /></span>";
                        //            }
                        //            else
                        //            {
                        //                ltr_comment.Text += "       <span class=\"avata circle\"><img src=\"/App_Themes/NHST/images/user-icon.png\" width=\"100%\" /></span>";
                        //            }
                        //            ltr_comment.Text += "   </div>";
                        //            ltr_comment.Text += "   <div class=\"item-right\">";
                        //            ltr_comment.Text += "       <strong class=\"item-username\">" + fullname + "</strong>";
                        //            ltr_comment.Text += "       <span class=\"item-date\">" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</span>";
                        //            ltr_comment.Text += "       <p class=\"item-comment\">";
                        //            ltr_comment.Text += item.Comment;
                        //            ltr_comment.Text += "       </p>";
                        //            ltr_comment.Text += "   </div>";
                        //            ltr_comment.Text += "</li>";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        ltr_comment.Text += "Hiện chưa có đánh giá nào.";
                        //    }
                        //}
                        //else
                        //{
                        //    ltr_comment.Text += "Hiện chưa có đánh giá nào.";
                        //}
                        #endregion
                    }
                    else
                    {
                        Response.Redirect("/trang-chu");
                    }
                }
            }
        }

        public class warehouses
        {
            public string name { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public List<package> package { get; set; }
        }

        public class package
        {
            public string code { get; set; }
            public string status { get; set; }
            public string classColor { get; set; }
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int uid = obj_user.ID;
                var id = RouteData.Values["id"].ToString().ToInt(0);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByUIDAndID(uid, id);
                    if (o != null)
                    {
                        if (o.Status == 0)
                        {
                            double wallet = 0;
                            if (obj_user.Wallet.ToString().ToFloat(0) > 0)
                                wallet = Convert.ToDouble(obj_user.Wallet);
                            wallet = wallet + Convert.ToDouble(o.Deposit);
                            AccountController.updateWallet(obj_user.ID, wallet, currentDate, obj_user.Username);
                            MainOrderController.UpdateDeposit(o.ID, obj_user.ID, "0");
                            int statusOOld = Convert.ToInt32(o.Status);
                            int statusONew = 1;

                            HistoryOrderChangeController.Insert(o.ID, uid, username, username +
            " đã xóa đơn hàng ID là: " + o.ID + ".", 1, currentDate);

                            string kq = MainOrderController.UpdateIsHiddenTrue(id);
                            //if (kq == "ok")
                            //    Page.Response.Redirect(Page.Request.Url.ToString(), true);
                            PJUtils.ShowMessageBoxSwAlertBackToLink("Xóa đơn hàng thành công.", "s", true, "/danh-sach-don-hang", Page);
                        }
                    }
                }
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int uid = obj_user.ID;
                var id = RouteData.Values["id"].ToString().ToInt(0);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByUIDAndID(uid, id);
                    if (o != null)
                    {
                        //string kq = OrderCommentController.Insert(id, txtComment.Text, true, 1, DateTime.Now, uid);
                        //if (Convert.ToInt32(kq) > 0)
                        //    Page.Response.Redirect(Page.Request.Url.ToString(), true);
                    }
                }
            }
        }

        protected void btnPayAll_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int uid = obj_user.ID;
                var id = ViewState["OID"].ToString().ToInt(0);
                DateTime currentDate = DateTime.Now;
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByUIDAndID(uid, id);
                    if (o != null)
                    {
                        double deposit = 0;
                        if (o.Deposit.ToFloat(0) > 0)
                            deposit = Math.Round(Convert.ToDouble(o.Deposit), 0);

                        double wallet = 0;
                        if (obj_user.Wallet.ToString().ToFloat(0) > 0)
                            wallet = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);

                        double feewarehouse = 0;
                        if (o.FeeInWareHouse.ToString().ToFloat(0) > 0)
                            feewarehouse = Math.Round(Convert.ToDouble(o.FeeInWareHouse), 0);

                        double totalPriceVND = 0;
                        if (o.TotalPriceVND.ToFloat(0) > 0)
                            totalPriceVND = Math.Round(Convert.ToDouble(o.TotalPriceVND), 0);
                        double moneyleft = Math.Round((totalPriceVND + feewarehouse) - deposit, 0);

                        if (wallet >= moneyleft)
                        {
                            double walletLeft = Math.Round(wallet - moneyleft, 0);
                            double payalll = Math.Round(deposit + moneyleft, 0);

                            #region Cập nhật ví và đơn hàng
                            //                MainOrderController.UpdateStatus(o.ID, uid, 9);
                            //                MainOrderController.UpdatePayDate(o.ID, currentDate);
                            //                int statusOOld = Convert.ToInt32(o.Status);
                            //                int statusONew = 9;
                            //                //if (statusONew != statusOOld)
                            //                //{
                            //                //    StatusChangeHistoryController.Insert(o.ID, statusOOld, statusONew, currentDate, obj_user.Username);
                            //                //}
                            //                AccountController.updateWallet(uid, walletLeft, currentDate, username);
                            //                HistoryOrderChangeController.Insert(o.ID, uid, username, username +
                            //" đã đổi trạng thái của đơn hàng ID là: " + o.ID + ", từ: Chờ thanh toán, sang: Khách đã thanh toán.", 1, currentDate);
                            //                HistoryPayWalletController.Insert(uid, username, o.ID, moneyleft, username + " đã thanh toán đơn hàng: " + o.ID + ".", walletLeft, 1, 3, currentDate, username);
                            //                MainOrderController.UpdateDeposit(id, uid, payalll.ToString());
                            //                PayOrderHistoryController.Insert(id, uid, 9, moneyleft, 2, currentDate, username);
                            #endregion

                            int st = TransactionController.PayAll(o.ID, wallet, o.Status.ToString().ToInt(0), uid, currentDate, username, deposit, 1, moneyleft, 1, 3, 2);
                            if (st == 1)
                            {
                                var setNoti = SendNotiEmailController.GetByID(11);
                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiAdmin == true)
                                    {
                                        var admins = AccountController.GetAllByRoleID(0);
                                        if (admins.Count > 0)
                                        {
                                            foreach (var admin in admins)
                                            {
                                                NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã được thanh toán.",
1, currentDate, obj_user.Username, false);
                                                string strPathAndQuery = Request.Url.PathAndQuery;
                                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã được thanh toán.", datalink);
                                            }
                                        }

                                        var managers = AccountController.GetAllByRoleID(2);
                                        if (managers.Count > 0)
                                        {
                                            foreach (var manager in managers)
                                            {
                                                NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã được thanh toán.",
1, currentDate, obj_user.Username, false);
                                                string strPathAndQuery = Request.Url.PathAndQuery;
                                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + " đã được thanh toán.", datalink);
                                            }
                                        }
                                    }

                                    if (setNoti.IsSentEmailAdmin == true)
                                    {
                                        var admins = AccountController.GetAllByRoleID(0);
                                        if (admins.Count > 0)
                                        {
                                            foreach (var admin in admins)
                                            {
                                                try
                                                {
                                                    PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                                        "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được thanh toán.", "");
                                                }
                                                catch { }
                                            }
                                        }

                                        var managers = AccountController.GetAllByRoleID(2);
                                        if (managers.Count > 0)
                                        {
                                            foreach (var manager in managers)
                                            {
                                                try
                                                {
                                                    PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                                        "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được thanh toán.", "");
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                }

                                PJUtils.ShowMessageBoxSwAlert("Thanh toán thành công.", "s", true, Page);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý, vui lòng thử lại sau.", "e", true, Page);
                            }
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Số tiền trong tài khoản của bạn không đủ để thanh toán đơn hàng.", "e", true, Page);
                        }
                    }
                }
            }
        }

        protected void btnDeposit_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                if (obj_user.Wallet > 0)
                {
                    int OID = ViewState["OID"].ToString().ToInt();
                    if (OID > 0)
                    {
                        var o = MainOrderController.GetAllByID(OID);
                        if (o != null)
                        {
                            double orderdeposited = 0;
                            double amountdeposit = 0;
                            double userwallet = 0;

                            if (o.Deposit.ToFloat(0) > 0)
                                orderdeposited = Math.Round(Convert.ToDouble(o.Deposit), 0);
                            if (o.AmountDeposit.ToFloat(0) > 0)
                                amountdeposit = Math.Round(Convert.ToDouble(o.AmountDeposit), 0);
                            if (obj_user.Wallet.ToString().ToFloat(0) > 0)
                                userwallet = Math.Round(Convert.ToDouble(obj_user.Wallet), 0);
                            double custDeposit = amountdeposit - orderdeposited;
                            if (userwallet > 0)
                            {
                                if (userwallet >= custDeposit)
                                {
                                    var setNoti = SendNotiEmailController.GetByID(6);

                                    double wallet = userwallet - custDeposit;
                                    wallet = Math.Round(wallet, 0);

                                    #region cập nhật ví khách hàng và đơn hàng
                                    //AccountController.updateWallet(obj_user.ID, wallet, currentDate, obj_user.Username);
                                    ////Cập nhật lại MainOrder                                
                                    //MainOrderController.UpdateStatus(o.ID, obj_user.ID, 2);
                                    //int statusOOld = Convert.ToInt32(o.Status);
                                    //int statusONew = 2;
                                    ////if (statusONew != statusOOld)
                                    ////{
                                    ////    StatusChangeHistoryController.Insert(o.ID, statusOOld, statusONew, currentDate, obj_user.Username);
                                    ////}
                                    //MainOrderController.UpdateDeposit(o.ID, obj_user.ID, amountdeposit.ToString());
                                    //MainOrderController.UpdateDepositDate(o.ID, currentDate);
                                    //HistoryOrderChangeController.Insert(o.ID, obj_user.ID, obj_user.Username, obj_user.Username +
                                    //   " đã đổi trạng thái của đơn hàng ID là: " + o.ID + ", từ: Chờ đặt cọc, sang: Đã đặt cọc.", 1, currentDate);
                                    //HistoryPayWalletController.Insert(obj_user.ID, obj_user.Username, o.ID, custDeposit,
                                    //    obj_user.Username + " đã đặt cọc đơn hàng: " + o.ID + ".", wallet, 1, 1, currentDate,
                                    //    obj_user.Username);
                                    //PayOrderHistoryController.Insert(o.ID, obj_user.ID, 2, custDeposit, 2, currentDate, obj_user.Username);
                                    #endregion

                                    int st = TransactionController.DepositAll(obj_user.ID, wallet, currentDate, obj_user.Username, o.ID, 2, o.Status.Value, amountdeposit.ToString(), custDeposit, obj_user.Username + " đã đặt cọc đơn hàng: " + o.ID, 1, 1, 2);
                                    if (st == 1)
                                    {
                                        var wh = WarehouseController.GetByID(Convert.ToInt32(o.ReceivePlace));
                                        if (wh != null)
                                        {
                                            var ExpectedDate = currentDate.AddDays(Convert.ToInt32(wh.ExpectedDate));
                                            MainOrderController.UpdateExpectedDate(o.ID, ExpectedDate);
                                        }
                                        if (setNoti != null)
                                        {
                                            if (setNoti.IsSentNotiAdmin == true)
                                            {

                                                var admins = AccountController.GetAllByRoleID(0);
                                                if (admins.Count > 0)
                                                {
                                                    foreach (var admin in admins)
                                                    {
                                                        NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", 1, currentDate, obj_user.Username, false);
                                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                        string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                        PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.", datalink);
                                                    }
                                                }

                                                var managers = AccountController.GetAllByRoleID(2);
                                                if (managers.Count > 0)
                                                {
                                                    foreach (var manager in managers)
                                                    {


                                                        NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã được đặt cọc.",
                                                        1, currentDate, obj_user.Username, false);
                                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                        string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                        PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + " đã được được đặt cọc.", datalink);
                                                    }
                                                }
                                            }

                                            if (setNoti.IsSentEmailAdmin == true)
                                            {
                                                var admins = AccountController.GetAllByRoleID(0);
                                                if (admins.Count > 0)
                                                {
                                                    foreach (var admin in admins)
                                                    {
                                                        try
                                                        {
                                                            PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                                                "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được đặt cọc.", "");
                                                        }
                                                        catch { }
                                                    }
                                                }

                                                var managers = AccountController.GetAllByRoleID(2);
                                                if (managers.Count > 0)
                                                {
                                                    foreach (var manager in managers)
                                                    {
                                                        try
                                                        {
                                                            PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                                                "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã được đặt cọc.", "");
                                                        }
                                                        catch { }
                                                    }
                                                }

                                            }
                                        }
                                        PJUtils.ShowMessageBoxSwAlert("Đặt cọc đơn hàng thành công.", "s", true, Page);
                                    }
                                    else
                                    {
                                        PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình xử lý.", "e", true, Page);
                                    }
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlertBackToLink("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.", "e", true, "/chuyen-muc/huong-dan/nap-tien", Page);
                                }
                                //Page.Response.Redirect(Page.Request.Url.ToString(), true);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlertBackToLink("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.", "e", true, "/chuyen-muc/huong-dan/nap-tien", Page);
                            }
                        }
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlertBackToLink("Số dư trong tài khoản của quý khách không đủ để đặt cọc đơn hàng này. Quý khách vui lòng nạp thêm tiền để tiến hành đặt cọc.", "e", true, "/chuyen-muc/huong-dan/nap-tien", Page);
                }
            }
        }
        [WebMethod]
        public static string PostComment(string commentext, string shopid, string urlIMG, string real)
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
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int uid = obj_user.ID;
                //var id = RouteData.Values["id"].ToString().ToInt(0);
                int id = shopid.ToInt(0);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByUIDAndID(uid, id);
                    if (o != null)
                    {
                        string message = "Khách hàng vừa gửi tin nhắn trong đơn hàng #" + id + ". CLick vào để xem";
                        int salerID = Convert.ToInt32(o.SalerID);
                        int dathangID = Convert.ToInt32(o.DathangID);
                        int khotqID = Convert.ToInt32(o.KhoTQID);
                        int khovnID = Convert.ToInt32(o.KhoVNID);
                        int cskhID = Convert.ToInt32(o.CSID);

                        var setNoti = SendNotiEmailController.GetByID(12);
                        if (setNoti != null)
                        {
                            if (setNoti.IsSentNotiAdmin == true)
                            {
                                if (salerID > 0)
                                {
                                    var saler = AccountController.GetByID(salerID);
                                    if (saler != null)
                                    {
                                        NotificationsController.Inser(salerID,
                                            saler.Username, id,
                                            message, 7,
                                            currentDate, obj_user.Username, false);
                                    }
                                }
                                if (dathangID > 0)
                                {
                                    var dathang = AccountController.GetByID(dathangID);
                                    if (dathang != null)
                                    {
                                        NotificationsController.Inser(dathangID,
                                            dathang.Username, id,
                                            message, 7,
                                            currentDate, obj_user.Username, false);
                                    }
                                }
                                if (cskhID > 0)
                                {
                                    var cskh = AccountController.GetByID(cskhID);
                                    if (cskh != null)
                                    {
                                        NotificationsController.Inser(dathangID,
                                            cskh.Username, id,
                                            message, 7,
                                            currentDate, obj_user.Username, false);
                                    }
                                }
                                //if (khotqID > 0)
                                //{
                                //    var khotq = AccountController.GetByID(khotqID);
                                //    if (khotq != null)
                                //    {
                                //        NotificationsController.Inser(khotqID,
                                //            khotq.Username, id,
                                //            message, 7,
                                //            currentDate, obj_user.Username, false);
                                //    }
                                //}
                                //if (khovnID > 0)
                                //{
                                //    var khovn = AccountController.GetByID(khovnID);
                                //    if (khovn != null)
                                //    {
                                //        NotificationsController.Inser(khovnID,
                                //            khovn.Username, id,
                                //            message, 7,
                                //            currentDate, obj_user.Username, false);
                                //    }
                                //}

                                var admins = AccountController.GetAllByRoleID(0);
                                if (admins.Count > 0)
                                {
                                    foreach (var admin in admins)
                                    {
                                        NotificationsController.Inser(admin.ID,
                                                                           admin.Username, id,
                                                                           message, 7,
                                                                           currentDate, obj_user.Username, false);
                                    }
                                }

                                var managers = AccountController.GetAllByRoleID(2);
                                if (managers.Count > 0)
                                {
                                    foreach (var manager in managers)
                                    {
                                        NotificationsController.Inser(manager.ID,
                                                                           manager.Username, id,
                                                                           message, 7,
                                                                           currentDate, obj_user.Username, false);
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < listLink.Count; i++)
                        {
                            string kqq = OrderCommentController.InsertNew(id, listLink[i], listComment[i], true, 1, DateTime.Now, uid);
                        }
                        if (!string.IsNullOrEmpty(commentext))
                        {
                            string kq = OrderCommentController.Insert(id, commentext, true, 1, currentDate, uid);
                            ChatHub ch = new ChatHub();
                            ch.SendMessenger(uid, id, commentext, listLink, listComment);
                            CustomerComment dataout = new CustomerComment();
                            dataout.UID = uid;
                            dataout.OrderID = id;
                            StringBuilder showIMG = new StringBuilder();
                            for (int i = 0; i < listLink.Count; i++)
                            {
                                showIMG.Append("<div class=\"chat chat-right\">");
                                showIMG.Append("    <div class=\"chat-avatar\">");
                                showIMG.Append("    <p class=\"name\">" + AccountInfoController.GetByUserID(uid).FirstName + " " + AccountInfoController.GetByUserID(uid).LastName + "</p>");
                                showIMG.Append("    </div>");
                                showIMG.Append("    <div class=\"chat-body\">");
                                showIMG.Append("        <div class=\"chat-text\">");
                                showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                showIMG.Append("            <div class=\"text-content\">");
                                showIMG.Append("                <div class=\"content\">");
                                showIMG.Append("                    <div class=\"content-img\">");
                                showIMG.Append("	                    <div class=\"img-block\">");
                                if (listLink[i].Contains(".doc"))
                                {
                                    showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/UserNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");

                                }
                                else if (listLink[i].Contains(".xls"))
                                {
                                    showIMG.Append("<a href=\"" + listLink[i] + "\" target=\"_blank\"><img src=\"/App_Themes/UserNew45/assets/images/icon/file.png\" title=\"" + listComment[i] + "\"  class=\"\" height=\"50\"/></a>");
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
                            showIMG.Append("    <p class=\"name\">" + AccountInfoController.GetByUserID(uid).FirstName + " " + AccountInfoController.GetByUserID(uid).LastName + "</p>");
                            showIMG.Append("    </div>");
                            showIMG.Append("    <div class=\"chat-body\">");
                            showIMG.Append("        <div class=\"chat-text\">");
                            showIMG.Append("            <div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                            showIMG.Append("            <div class=\"text-content\">");
                            showIMG.Append("                <div class=\"content\">");
                            showIMG.Append("                    <p>" + commentext + "</p>");
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
                                ch.SendMessenger(uid, id, commentext, listLink, listComment);
                                CustomerComment dataout = new CustomerComment();
                                StringBuilder showIMG = new StringBuilder();
                                for (int i = 0; i < listLink.Count; i++)
                                {

                                    showIMG.Append("<div class=\"chat chat-right\">");
                                    showIMG.Append("<div class=\"chat-avatar\">");
                                    showIMG.Append("    <p class=\"name\">" + AccountInfoController.GetByUserID(uid).FirstName + " " + AccountInfoController.GetByUserID(uid).LastName + "</p>");
                                    showIMG.Append("</div>");
                                    showIMG.Append("<div class=\"chat-body\">");
                                    showIMG.Append("<div class=\"chat-text\">");
                                    showIMG.Append("<div class=\"date-time center-align\">" + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now) + "</div>");
                                    showIMG.Append("<div class=\"text-content\">");
                                    showIMG.Append("<div class=\"content\">");
                                    showIMG.Append("<div class=\"content-img\">");
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

        protected void btnPostComment_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                int uid = obj_user.ID;
                //var id = RouteData.Values["id"].ToString().ToInt(0);
                int id = hdfShopID.Value.ToInt(0);
                if (id > 0)
                {
                    var o = MainOrderController.GetAllByUIDAndID(uid, id);
                    if (o != null)
                    {
                        int salerID = Convert.ToInt32(o.SalerID);
                        int dathangID = Convert.ToInt32(o.DathangID);
                        int khotqID = Convert.ToInt32(o.KhoTQID);
                        int khovnID = Convert.ToInt32(o.KhoVNID);

                        if (salerID > 0)
                        {
                            var saler = AccountController.GetByID(salerID);
                            if (saler != null)
                            {
                                NotificationsController.Inser(salerID,
                                    saler.Username, id,
                                    "Đã có đánh giá mới cho đơn hàng #" + id + ". CLick vào để xem", id,
                                    currentDate, obj_user.Username, false);
                                string strPathAndQuery = Request.Url.PathAndQuery;
                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                PJUtils.PushNotiDesktop(salerID, "Đơn hàng " + id + " có thông báo mới.", datalink);
                            }
                        }
                        if (dathangID > 0)
                        {
                            var dathang = AccountController.GetByID(dathangID);
                            if (dathang != null)
                            {
                                NotificationsController.Inser(dathangID,
                                    dathang.Username, id,
                                    "Đã có đánh giá mới cho đơn hàng #" + id + ". CLick vào để xem", id,
                                    currentDate, obj_user.Username, false);
                                string strPathAndQuery = Request.Url.PathAndQuery;
                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                PJUtils.PushNotiDesktop(dathangID, "Đơn hàng " + id + " có thông báo mới.", datalink);
                            }
                        }
                        if (khotqID > 0)
                        {
                            var khotq = AccountController.GetByID(khotqID);
                            if (khotq != null)
                            {
                                NotificationsController.Inser(khotqID,
                                    khotq.Username, id,
                                    "Đã có đánh giá mới cho đơn hàng #" + id + ". CLick vào để xem", id,
                                    currentDate, obj_user.Username, false);
                                string strPathAndQuery = Request.Url.PathAndQuery;
                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                PJUtils.PushNotiDesktop(khotqID, "Đơn hàng " + id + " có thông báo mới.", datalink);
                            }
                        }
                        if (khovnID > 0)
                        {
                            var khovn = AccountController.GetByID(khovnID);
                            if (khovn != null)
                            {
                                NotificationsController.Inser(khovnID,
                                    khovn.Username, id,
                                    "Đã có đánh giá mới cho đơn hàng #" + id + ". CLick vào để xem", id,
                                    currentDate, obj_user.Username, false);
                                string strPathAndQuery = Request.Url.PathAndQuery;
                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                PJUtils.PushNotiDesktop(khovnID, "Đơn hàng " + id + " có thông báo mới.", datalink);
                            }
                        }

                        var admins = AccountController.GetAllByRoleID(0);
                        if (admins.Count > 0)
                        {
                            foreach (var admin in admins)
                            {
                                NotificationsController.Inser(admin.ID,
                                                                   admin.Username, id,
                                                                   "Đã có đánh giá mới cho đơn hàng #" + id + ". CLick vào để xem", id,
                                                                   currentDate, obj_user.Username, false);
                                string strPathAndQuery = Request.Url.PathAndQuery;
                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                PJUtils.PushNotiDesktop(admin.ID, "Đã có đánh giá mới cho đơn hàng #" + id + ". CLick vào để xem", datalink);
                            }
                        }

                        var managers = AccountController.GetAllByRoleID(2);
                        if (managers.Count > 0)
                        {
                            foreach (var manager in managers)
                            {
                                NotificationsController.Inser(manager.ID,
                                                                   manager.Username, id,
                                                                   "Đã có đánh giá mới cho đơn hàng #" + id + ". CLick vào để xem", id,
                                                                   currentDate, obj_user.Username, false);
                                string strPathAndQuery = Request.Url.PathAndQuery;
                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                string datalink = "" + strUrl + "manager/OrderDetail/" + id;
                                PJUtils.PushNotiDesktop(manager.ID, "Đã có đánh giá mới cho đơn hàng #" + id + ". CLick vào để xem", datalink);
                            }
                        }


                        string comment = hdfCommentText.Value;
                        string kq = OrderCommentController.Insert(id, comment, true, 1, currentDate, uid);
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                        hubContext.Clients.All.addNewMessageToPage("", "");
                        PJUtils.ShowMessageBoxSwAlert("Gửi nội dung thành công", "s", true, Page);
                    }
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var u = AccountController.GetByUsername(username);
            DateTime currentDate = DateTime.Now;
            if (u != null)
            {
                int UID = u.ID;
                int ID = ViewState["OID"].ToString().ToInt(0);
                string orderCodeshop = Request.QueryString["ordershopcode"];
                var s = MainOrderController.GetAllByUIDAndID(UID, ID);
                if (s != null)
                {
                    //MainOrderController.UpdateNote(s.ID, txt_DNote.Text);
                    PJUtils.ShowMessageBoxSwAlert("Cập nhật ghi chú thành công", "s", true, Page);
                }
            }
        }

        protected void btnYCG_Click(object sender, EventArgs e)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            if (obj_user != null)
            {
                if (obj_user.Wallet > 0)
                {
                    int OID = ViewState["OID"].ToString().ToInt();
                    if (OID > 0)
                    {
                        var o = MainOrderController.GetAllByID(OID);
                        if (o != null)
                        {
                            var check = YCGController.GetByMainOrderID(o.ID);
                            if (check == null)
                            {
                                YCGController.Insert(o.ID, txtFullName.Text, txtPhone.Text, txtAddress.Text, txtNote.Text, username_current, currentDate);
                                MainOrderController.UpdateYCG(o.ID, true);
                                var setNoti = SendNotiEmailController.GetByID(6);
                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiAdmin == true)
                                    {

                                        var admins = AccountController.GetAllByRoleID(0);
                                        if (admins.Count > 0)
                                        {
                                            foreach (var admin in admins)
                                            {
                                                NotificationsController.Inser(admin.ID, admin.Username, o.ID, "Đơn hàng " + o.ID + " đã yêu cầu giao hàng.", 1, currentDate, obj_user.Username, false);
                                                string strPathAndQuery = Request.Url.PathAndQuery;
                                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                PJUtils.PushNotiDesktop(admin.ID, "Đơn hàng " + o.ID + " đã yêu cầu giao hàng.", datalink);
                                            }
                                        }

                                        var managers = AccountController.GetAllByRoleID(2);
                                        if (managers.Count > 0)
                                        {
                                            foreach (var manager in managers)
                                            {


                                                NotificationsController.Inser(manager.ID, manager.Username, o.ID, "Đơn hàng " + o.ID + " đã yêu cầu giao hàng.",
                                                1, currentDate, obj_user.Username, false);
                                                string strPathAndQuery = Request.Url.PathAndQuery;
                                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                                string datalink = "" + strUrl + "manager/OrderDetail/" + o.ID;
                                                PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng " + o.ID + "đã yêu cầu giao hàng.", datalink);
                                            }
                                        }
                                    }

                                    if (setNoti.IsSentEmailAdmin == true)
                                    {
                                        var admins = AccountController.GetAllByRoleID(0);
                                        if (admins.Count > 0)
                                        {
                                            foreach (var admin in admins)
                                            {
                                                try
                                                {
                                                    PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", admin.Email,
                                                        "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã yêu cầu giao hàng.", "");
                                                }
                                                catch { }
                                            }
                                        }

                                        var managers = AccountController.GetAllByRoleID(2);
                                        if (managers.Count > 0)
                                        {
                                            foreach (var manager in managers)
                                            {
                                                try
                                                {
                                                    PJUtils.SendMailGmail("cskh.thuonghai@gmail.com.vn.net", "jrbhnznozmlrmwvy", manager.Email,
                                                        "Thông báo tại NHAPSICHINA.COM.", "Đơn hàng " + o.ID + " đã yêu cầu giao hàng.", "");
                                                }
                                                catch { }
                                            }
                                        }

                                    }
                                }

                                PJUtils.ShowMessageBoxSwAlert("Tạo yêu cầu giao hàng thành công.", "s", true, Page);
                            }
                        }
                    }
                }
            }
        }
        //[WebMethod]
        public void loadinfoSP(string ID)
        {
            var i = 0;
            var o = MainOrderController.GetByID(ID.ToInt(0));
            var lo = OrderController.GetByMainOrderID(ID.ToInt(0));
            hdfNumbersp.Value = lo.Count().ToString();
            hdfOrderIDsp.Value = ID;
            StringBuilder showsp = new StringBuilder();
            foreach (var item in lo)
            {
                showsp.Append("<div class=\"input-field col s6\">");
                showsp.Append("<img src=\"" + item.image_origin + "\" alt=\"image\">");
                showsp.Append("</div>");
                showsp.Append("<div class=\"input-field col s6\">");
                showsp.Append("<span>Số lượng</span>");
                showsp.Append("<input id=\"sp_" + i + "\" type=\"number\" value=\"" + item.quantity.ToInt(0) + "\">");
                showsp.Append("</div>");

                i++;
            }
            showsp.Append("<div class=\"input-field col s12\">");
            showsp.Append("<select name=\"\" id=\"\" class=\"warehosefromselect\">");
            showsp.Append("<option value=\"0\">Chọn kho TQ</option>");
            var warehouseTQ = WarehouseFromController.GetAllWithIsHidden(false);
            if (warehouseTQ.Count > 0)
            {
                foreach (var w in warehouseTQ)
                {
                    if (o.FromPlace == w.ID)
                        showsp.Append("<option value=\"" + w.ID + "\" selected>" + w.WareHouseName + "</option>");
                    else
                        showsp.Append("<option value=\"" + w.ID + "\">" + w.WareHouseName + "</option>");
                }
            }
            showsp.Append("</select>");
            showsp.Append("<label for=\"\">Chọn kho TQ</label>");
            showsp.Append("</div>");

            showsp.Append("<div class=\"input-field col s12\">");
            showsp.Append("<select name=\"\" id=\"\" class=\"warehoseselect\">");
            showsp.Append("<option value=\"0\">Chọn kho VN</option>");
            var warehouseVN = WarehouseController.GetAllWithIsHidden(false);
            if (warehouseVN.Count > 0)
            {
                foreach (var w in warehouseVN)
                {
                    if (o.ReceivePlace.ToInt(0) == w.ID)
                        showsp.Append("<option value=\"" + w.ID + "\" selected>" + w.WareHouseName + "</option>");
                    else
                        showsp.Append("<option value=\"" + w.ID + "\">" + w.WareHouseName + "</option>");
                }
            }
            showsp.Append("</select>");
            showsp.Append("<label for=\"\">Chọn kho VN</label>");
            showsp.Append("</div>");

            showsp.Append("<div class=\"input-field col s12\">");
            showsp.Append("<select name=\"\" id=\"\" class=\"shippingtypesselect\">");
            showsp.Append("<option value=\"0\">Chọn phương thức vận chuyển</option>");
            var shippingType = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shippingType.Count > 0)
            {
                foreach (var w in shippingType)
                {
                    if (o.ShippingType == w.ID)
                        showsp.Append("<option value=\"" + w.ID + "\" selected>" + w.ShippingTypeName + "</option>");
                    else
                        showsp.Append("<option value=\"" + w.ID + "\">" + w.ShippingTypeName + "</option>");
                }
            }
            showsp.Append("</select>");
            showsp.Append("<label for=\"\">Chọn phương thức vận chuyển</label>");
            showsp.Append("</div>");

            ltrListSP.Text = showsp.ToString();

        }

        protected void btn_saveOrder_Click(object sender, EventArgs e)
        {
            var ID = hdfOrderIDsp.Value.ToInt(0);
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                double current = Convert.ToDouble(ConfigurationController.GetByTop1().Currency);
                double InsurancePercent = Convert.ToDouble(ConfigurationController.GetByTop1().InsurancePercent);
                if (!string.IsNullOrEmpty(obj_user.Currency.ToString()))
                {
                    if (Convert.ToDouble(obj_user.Currency) > 0)
                    {
                        current = Convert.ToDouble(obj_user.Currency);
                    }
                }
                var o = MainOrderController.GetByID(ID);
                int salerID = 0;
                int dathangID = 0;

                if (o.SalerID != null)
                {
                    salerID = Convert.ToInt32(o.SalerID);
                }
                if (o.DathangID != null)
                {
                    dathangID = Convert.ToInt32(o.DathangID);
                }

                int UID = obj_user.ID;
                var setNoti = SendNotiEmailController.GetByID(5);
                //double percent_User = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).LevelPercent);
                double UL_CKFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeBuyPro);
                double UL_CKFeeWeight = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeWeight);
                double LessDeposit = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).LessDeposit);
                if (!string.IsNullOrEmpty(obj_user.Deposit.ToString()))
                {
                    if (obj_user.Deposit > 0)
                    {
                        LessDeposit = Convert.ToDouble(obj_user.Deposit);
                    }
                }
                string wareship = hdfWarehouse.Value;
                string strQuantity = hdfSPinCount.Value;
                string[] qtt = strQuantity.Split('-');
                string[] w = wareship.Split('-');
                int warehouseFromID = w[2].ToInt(0);
                int warehouseID = w[0].ToInt(0);
                int w_shippingType = w[1].ToInt(0);

                double total = 0;
                double totaldeposit = 0;
                double fastprice = 0;
                double pricepro = 0;
                double priceproCYN = 0;
                var oder = OrderController.GetByMainOrderID(o.ID);
                if (oder.Count > 0)
                {
                    int i = 0;
                    foreach (var item in oder)
                    {

                        int quantity = Convert.ToInt32(qtt[i]);
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
                        i++;
                    }
                }
                double feecnship = 0;
                if (o.IsFast == true)
                {
                    fastprice = Math.Round((pricepro * 5 / 100), 0);
                }
                //total = fastprice + pricepro + feebp + feecnship;
                string ShopID = o.ShopID;
                string ShopName = o.ShopName;
                string Site = o.Site;
                bool IsForward = Convert.ToBoolean(o.IsForward);
                double isForwardPrice = 0;
                if (o.IsForwardPrice.ToFloat(0) > 0)
                    isForwardPrice = Math.Round(Convert.ToDouble(o.IsForwardPrice), 0);
                string IsForwardPrice = isForwardPrice.ToString();
                bool IsFastDelivery = Convert.ToBoolean(o.IsFastDelivery);
                double isFastDeliveryPrice = 0;
                if (o.IsFastDeliveryPrice.ToFloat(0) > 0)
                    isFastDeliveryPrice = Math.Round(Convert.ToDouble(o.IsFastDeliveryPrice), 0);
                string IsFastDeliveryPrice = isFastDeliveryPrice.ToString();
                bool IsCheckProduct = Convert.ToBoolean(o.IsCheckProduct);
                string IsCheckProductPrice = o.IsCheckProductPrice;
                bool IsPacked = Convert.ToBoolean(o.IsPacked);
                double ispackagePrice = 0;
                if (o.IsPackedPrice.ToFloat(0) > 0)
                    ispackagePrice = Math.Round(Convert.ToDouble(o.IsPackedPrice), 0);
                string IsPackedPrice = ispackagePrice.ToString();
                bool IsFast = Convert.ToBoolean(o.IsFast);
                string IsFastPrice = fastprice.ToString();
                double pricecynallproduct = 0;

                double isCheckProductPrice = 0;
                if (o.IsCheckProductPrice.ToFloat(0) > 0)
                    isCheckProductPrice = Convert.ToDouble(o.IsCheckProductPrice);

                fastprice = Math.Round(fastprice, 0);
                IsFastPrice = fastprice.ToString();
                isCheckProductPrice = Math.Round(isCheckProductPrice, 0);
                IsCheckProductPrice = isCheckProductPrice.ToString();
                double totalFee_CountFee = fastprice + pricepro + feecnship
                    + isCheckProductPrice;

                double servicefee = 0;
                double servicefeeMoney = 0;

                double feebpnotdc = 0;
                var adminfeebuypro = FeeBuyProController.GetAll();
                if (adminfeebuypro.Count > 0)
                {
                    foreach (var item in adminfeebuypro)
                    {
                        if (pricepro >= item.AmountFrom && pricepro < item.AmountTo)
                        {
                            double feepercent = 0;
                            if (item.FeePercent.ToString().ToFloat(0) > 0)
                                feepercent = Convert.ToDouble(item.FeePercent);
                            servicefee = feepercent / 100;
                            //serviceFeeMoney = Convert.ToDouble(item.FeeMoney);
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(obj_user.FeeBuyPro))
                {
                    if (obj_user.FeeBuyPro.ToFloat(0) > 0)
                    {
                        feebpnotdc = pricepro * Convert.ToDouble(obj_user.FeeBuyPro) / 100;
                    }
                }
                else
                    feebpnotdc = pricepro * servicefee;


                double subfeebp = feebpnotdc * UL_CKFeeBuyPro / 100;


                double feebp = feebpnotdc - subfeebp;
                feebp = Math.Round(feebp, 0);
                if (feebp < 10000)
                    feebp = 10000;

                //if (feebp < 10000)
                //    feebp = 10000;
                //double feebp = totalFee_CountFee * UL_CKFeeBuyPro / 100;
                if (w_shippingType == 4)
                {
                    var fe = FeeBuyProController.GetByTypeAndPrice(4, Convert.ToDouble(pricepro));
                    if (fe != null)
                    {
                        var fee = fe.FeePercent;
                        feebp = Convert.ToDouble(pricepro) * fee.Value / 100;
                        if (feebp < 15000)
                            feebp = 15000;
                    }
                    else if (feebp < 15000)
                        feebp = 15000;
                }
                double ischeckproductprice = 0;
                if (o.IsCheckProductPrice.ToFloat(0) > 0)
                    ischeckproductprice = Convert.ToDouble(o.IsCheckProductPrice);
                ischeckproductprice = Math.Round(ischeckproductprice, 0);

                double InsuranceMoney = 0;
                if (o.IsInsurrance == true)
                    InsuranceMoney = pricepro * (InsurancePercent / 100);

                total = fastprice + pricepro + feebp + feecnship + ischeckproductprice + InsuranceMoney;
                totaldeposit = pricepro;
                //Lấy ra từng ordertemp trong shop

                double priceVND = 0;
                double priceCYN = 0;
                var proOrdertemp = OrderController.GetByMainOrderID(o.ID);
                if (proOrdertemp != null)
                {
                    if (proOrdertemp.Count > 0)
                    {
                        int i = 0;
                        foreach (var item in proOrdertemp)
                        {
                            int quantity = Convert.ToInt32(qtt[i]);
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

                            priceVND += e_pricevn;
                            priceCYN += e_pricebuy;

                            pricecynallproduct += e_pricebuy;
                            i++;
                        }
                    }
                }
                priceVND = Math.Round(priceVND, 0);
                priceCYN = Math.Round(priceCYN, 2);
                string PriceVND = priceVND.ToString();

                pricecynallproduct = Math.Round(pricecynallproduct, 2);
                string PriceCNY = priceCYN.ToString();
                //string FeeShipCN = (10 * current).ToString();
                string FeeShipCN = Math.Round(feecnship, 0).ToString();
                string FeeBuyPro = Math.Round(feebp, 0).ToString();
                double feeWeight = 0;
                if (o.FeeWeight.ToFloat(0) > 0)
                    feeWeight = Math.Round(Convert.ToDouble(o.FeeWeight), 0);

                string FeeWeight = feeWeight.ToString();
                string Note = o.Note;
                string FullName = o.FullName;
                string Address = o.Address;
                string Email = o.Email;
                string Phone = o.Phone;
                int Status = 0;
                string Deposit = "0";
                string CurrentCNYVN = current.ToString();
                string TotalPriceVND = Math.Round(total, 0).ToString();
                string AmountDeposit = Math.Round((totaldeposit * LessDeposit / 100)).ToString();
                DateTime CreatedDate = DateTime.Now;
                string kq = MainOrderController.Insert(UID, ShopID, ShopName, Site, IsForward, IsForwardPrice, IsFastDelivery,
                    IsFastDeliveryPrice, IsCheckProduct, IsCheckProductPrice, IsPacked, IsPackedPrice, IsFast, IsFastPrice,
                    PriceVND, PriceCNY, FeeShipCN, FeeBuyPro, FeeWeight, Note, FullName, Address, Email, Phone, Status,
                    Deposit, CurrentCNYVN, TotalPriceVND, salerID, dathangID, CreatedDate, UID, AmountDeposit, 1);
                int idkq = Convert.ToInt32(kq);
                if (idkq > 0)
                {
                    int i = 0;
                    foreach (var item in proOrdertemp)
                    {
                        int quantity = Convert.ToInt32(qtt[i]);
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

                        pricecynallproduct += e_pricebuy;

                        string image = item.image_origin;
                        if (image.Contains("%2F"))
                        {
                            image = image.Replace("%2F", "/");
                        }
                        if (image.Contains("%3A"))
                        {
                            image = image.Replace("%3A", ":");
                        }
                        string ret = OrderController.Insert(UID, item.title_origin, item.title_translated, item.price_origin, item.price_promotion, item.property_translated,
                        item.property, item.data_value, image, image, item.shop_id, item.shop_name, item.seller_id, item.wangwang, item.quantity,
                        item.stock, item.location_sale, item.site, item.comment, item.item_id, item.link_origin, item.outer_id, item.error, item.weight, item.step, item.stepprice, item.brand,
                        item.category_name, item.category_id, item.tool, item.version, Convert.ToBoolean(item.is_translate), Convert.ToBoolean(item.IsForward), "0",
                        Convert.ToBoolean(item.IsFastDelivery), "0", Convert.ToBoolean(item.IsCheckProduct), "0", Convert.ToBoolean(item.IsPacked), "0", Convert.ToBoolean(item.IsFast),
                        fastprice.ToString(), pricepro.ToString(), PriceCNY, item.Note, o.FullName, o.Address, o.Email,
                        o.Phone, 0, "0", current.ToString(), total.ToString(), idkq, DateTime.Now, UID);

                        if (item.price_promotion.ToFloat(0) > 0)
                            OrderController.UpdatePricePriceReal(ret.ToInt(0), item.price_origin, item.price_promotion);
                        else
                            OrderController.UpdatePricePriceReal(ret.ToInt(0), item.price_origin, item.price_origin);
                        i++;
                    }
                    MainOrderController.UpdateReceivePlace(idkq, UID, warehouseID.ToString(), w_shippingType);
                    MainOrderController.UpdateFromPlace(idkq, UID, warehouseFromID, w_shippingType);
                    MainOrderController.UpdateIsInsurrance(idkq, Convert.ToBoolean(o.IsInsurrance));
                    MainOrderController.UpdateInsurranceMoney(idkq, InsuranceMoney.ToString(), InsurancePercent.ToString());

                    if (setNoti != null)
                    {
                        if (setNoti.IsSentNotiAdmin == true)
                        {
                            var admins = AccountController.GetAllByRoleID(0);
                            if (admins.Count > 0)
                            {
                                foreach (var admin in admins)
                                {
                                    NotificationsController.Inser(admin.ID,
                                                                       admin.Username, idkq,
                                                                       "Có đơn hàng mới ID là: " + idkq, 1,
                                                                        CreatedDate, username, false);
                                    string strPathAndQuery = Request.Url.PathAndQuery;
                                    string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                    PJUtils.PushNotiDesktop(admin.ID, "Có đơn hàng mới ID là: " + idkq, datalink);
                                }
                            }

                            var managers = AccountController.GetAllByRoleID(2);
                            if (managers.Count > 0)
                            {
                                foreach (var manager in managers)
                                {
                                    NotificationsController.Inser(manager.ID,
                                                                       manager.Username, idkq,
                                                                       "Có đơn hàng mới ID là: " + idkq, 1,
                                                                      CreatedDate, username, false);
                                    string strPathAndQuery = Request.Url.PathAndQuery;
                                    string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                    PJUtils.PushNotiDesktop(manager.ID, "Có đơn hàng mới ID là: " + idkq, datalink);
                                }
                            }
                        }
                    }

                }

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

                if (salerID > 0)
                {
                    var sale = AccountController.GetByID(salerID);
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
                                double per = feebp * salepercentaf3m / 100;
                                per = Math.Round(per, 0);
                                StaffIncomeController.Insert(idkq, "0", salepercent.ToString(), salerID, salerName, 6, 1, per.ToString(), false,
                                CreatedDate, CreatedDate, username);
                            }
                            else
                            {
                                double per = feebp * salepercent / 100;
                                per = Math.Round(per, 0);
                                StaffIncomeController.Insert(idkq, "0", salepercent.ToString(), salerID, salerName, 6, 1, per.ToString(), false,
                                CreatedDate, CreatedDate, username);
                            }
                        }



                    }
                }
                if (dathangID > 0)
                {
                    var dathang = AccountController.GetByID(dathangID);
                    if (dathang != null)
                    {
                        dathangName = dathang.Username;
                        StaffIncomeController.Insert(idkq, "0", dathangpercent.ToString(), dathangID, dathangName, 3, 1, "0", false,
                            CreatedDate, CreatedDate, username);
                        if (setNoti != null)
                        {
                            if (setNoti.IsSentNotiAdmin == true)
                            {
                                NotificationsController.Inser(dathang.ID,
                                                       dathang.Username, idkq,
                                                       "Có đơn hàng mới ID là: " + idkq, 1,
                                                        CreatedDate, username, false);
                                string strPathAndQuery = Request.Url.PathAndQuery;
                                string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                PJUtils.PushNotiDesktop(dathang.ID, "Có đơn hàng mới ID là: " + idkq, datalink);
                            }
                        }
                    }
                }
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                hubContext.Clients.All.addNewMessageToPage("", "");
                Response.Redirect("/danh-sach-don-hang?t=1");
            }
            else
            {
                Response.Redirect("/trang-chu");
            }
        }
        [WebMethod]
        public static string UpdateNoteOrder(string ID, string brand)
        {
            string kq = OrderController.UpdateBrand(Convert.ToInt32(ID), brand);
            return kq;
        }
    }
}