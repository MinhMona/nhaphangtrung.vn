using MB.Extensions;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class WorkingSession : System.Web.UI.Page
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
                    if (ac.RoleID != 0 && ac.RoleID != 2 && ac.RoleID != 5)
                        Response.Redirect("/trang-chu");
                    loadFilter();
                }
            }
        }

        public void loadFilter()
        {
            var session = WorkingSessionController.GetAllByStatus(1);
            ddlSession.Items.Clear();
            ddlSession.Items.Insert(0, new ListItem("Chọn phiên làm việc", "0"));
            if (session.Count > 0)
            {
                ddlSession.DataSource = session;
                ddlSession.DataBind();
            }
        }

        [WebMethod]
        public static string AddBigPackage(string PackageCode, string PackageNote)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user = AccountController.GetByUsername(username);
                if (user != null)
                {
                    var check = WorkingSessionController.GetByNameSession(PackageCode);
                    if (check != null)
                    {
                        return "existCode";
                    }
                    else
                    {
                        double volume = 0;
                        double weight = 0;

                        string kq = WorkingSessionController.Insert(PackageCode, weight, volume, 1, DateTime.Now, username, PackageNote);

                        if (kq.ToInt(0) > 0)
                            return kq;
                        else
                            return null;
                    }
                }
                else
                    return null;
            }
            else
                return null;
        }

        public class smallpackageitem
        {
            public int ID { get; set; }
            public string OrderType { get; set; }
            public int MainorderID { get; set; }
            public int TransportationID { get; set; }
            public int UID { get; set; }
            public string Username { get; set; }
            public double Wallet { get; set; }
            public string OrderShopCode { get; set; }
            public string BarCode { get; set; }
            public double Weight { get; set; }
            public string Kiemdem { get; set; }
            public string PDB1 { get; set; }
            public string PDB2 { get; set; }
            public string Donggo { get; set; }
            public string Soloaisanpham { get; set; }
            public string Soluongsanpham { get; set; }
            public string Baohiem { get; set; }
            public string NVKiemdem { get; set; }
            public string Loaisanpham { get; set; }
            public string Khachghichu { get; set; }
            public int Status { get; set; }
            public int BigPackageID { get; set; }
            public bool IsTemp { get; set; }
            public bool IsThatlac { get; set; }
            public bool IsVCH { get; set; }
            public string Fullname { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Description { get; set; }
            public double dai { get; set; }
            public double rong { get; set; }
            public double cao { get; set; }
            public string IMG { get; set; }
            public string Note { get; set; }
            public int OrderTypeInt { get; set; }
            public string DateShop { get; set; }
            public string NameReceivePlace { get; set; }
            public string NameShippingType { get; set; }
        }

        public class BigPackageItem
        {
            public int BigpackageID { get; set; }
            public string BigpackageCode { get; set; }
            public int BigpackageSmallPackageCount { get; set; }
            public int BigpackageType { get; set; }
            public List<smallpackageitem> Smallpackages { get; set; }
        }

        [WebMethod]
        public static string GetListPackage(string barcode, int session)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var user = AccountController.GetByUsername(username);
                if (user != null)
                {
                    DateTime currentDate = DateTime.Now;
                    int userRole = Convert.ToInt32(user.RoleID);
                    if (userRole == 0 || userRole == 2 || userRole == 5)
                    {
                        var smallpackages = SmallPackageController.GetListByOrderTransactionCodeKhoVN(barcode, session);
                        if (smallpackages.Count > 0)
                        {
                            BigPackageItem bi = new BigPackageItem();
                            List<smallpackageitem> sis = new List<smallpackageitem>();
                            foreach (var item in smallpackages)
                            {
                                smallpackageitem si = new smallpackageitem();

                                int mID = Convert.ToInt32(item.MainOrderID);
                                int tID = Convert.ToInt32(item.TransportationOrderID);

                                si.ID = item.ID;
                                si.IMG = item.ListIMG;
                                si.Note = item.Description;
                                si.DateShop = "";
                                si.NameReceivePlace = "";
                                si.NameShippingType = "";

                                if (mID > 0)
                                {
                                    var mainorder = MainOrderController.GetAllByID(mID);
                                    if (mainorder != null)
                                    {
                                        if (mainorder.DateShipper != null)
                                        {
                                            si.DateShop = string.Format("{0:dd/MM/yyyy HH:mm}", mainorder.DateShipper);
                                        }    

                                        if (Convert.ToInt32(mainorder.ReceivePlace) == 1)
                                        {
                                            si.NameReceivePlace = "Kho Hà Nội";
                                        }   
                                        else if (Convert.ToInt32(mainorder.ReceivePlace) == 3)
                                        {
                                            si.NameReceivePlace = "Kho Hồ Chí Minh";
                                        }    
                                        else
                                        {
                                            si.NameReceivePlace = "Không xác định";
                                        }

                                        if (Convert.ToInt32(mainorder.ShippingType) == 1)
                                        {
                                            si.NameShippingType = "Vận chuyển nhanh";
                                        }
                                        else if (Convert.ToInt32(mainorder.ShippingType) == 4)
                                        {
                                            si.NameShippingType = "Thương mại điện tử";
                                        }
                                        else if (Convert.ToInt32(mainorder.ShippingType) == 5)
                                        {
                                            si.NameShippingType = "Line biển - đặc biệt";
                                        }
                                        else
                                        {
                                            si.NameShippingType = "Không xác định";
                                        }

                                        int UID = Convert.ToInt32(mainorder.UID);
                                        si.UID = UID;
                                        var acc = AccountController.GetByID(UID);
                                        if (acc != null)
                                        {
                                            si.Username = acc.Username;
                                            si.Wallet = Convert.ToDouble(acc.Wallet);
                                            si.OrderShopCode = mainorder.MainOrderCode;

                                            if (mainorder.IsCheckProduct == true)
                                                si.Kiemdem = "Có";
                                            else
                                                si.Kiemdem = "Không";

                                            if (mainorder.IsPacked == true)
                                                si.Donggo = "Có";
                                            else
                                                si.Donggo = "Không";

                                            if (mainorder.IsCheckSpecial1 == true)
                                                si.PDB1 = "Có";
                                            else
                                                si.PDB1 = "Không";

                                            if (mainorder.IsCheckSpecial2 == true)
                                                si.PDB2 = "Có";
                                            else
                                                si.PDB2 = "Không";

                                            var orders = OrderController.GetByMainOrderID(mID);
                                            double totalProductQuantity = 0;
                                            if (orders.Count > 0)
                                            {
                                                foreach (var p in orders)
                                                {
                                                    totalProductQuantity += Convert.ToDouble(p.quantity);
                                                }
                                            }
                                            si.Soluongsanpham = totalProductQuantity.ToString();
                                            string Phone = "";
                                            var ai = AccountInfoController.GetByUserID(acc.ID);
                                            if (ai != null)
                                            {
                                                si.Fullname = ai.FirstName + " " + ai.LastName;
                                                si.Email = acc.Email;
                                                Phone = ai.Phone;
                                                si.Address = ai.Address;
                                            }
                                            si.Phone = Phone;
                                        }
                                    }

                                    bi.BigpackageID = Convert.ToInt32(item.BigPackageID);
                                    string PackageCode = "";
                                    if (Convert.ToInt32(item.BigPackageID) > 0)
                                    {
                                        var big = BigPackageController.GetByID(Convert.ToInt32(item.BigPackageID));
                                        if (big != null)
                                        {
                                            PackageCode = big.PackageCode;
                                        }
                                    }
                                    bi.BigpackageCode = PackageCode;
                                    bi.BigpackageType = 1;

                                    si.OrderType = "Mua hộ";
                                    si.MainorderID = mID;
                                    si.TransportationID = 0;
                                    si.OrderTypeInt = 1;

                                    si.Weight = Convert.ToDouble(item.Weight);
                                    si.BarCode = item.OrderTransactionCode;
                                    si.Status = 3;

                                    if (!string.IsNullOrEmpty(item.Description))
                                        si.Description = item.Description;
                                    else
                                        si.Description = string.Empty;

                                    if (!string.IsNullOrEmpty(item.UserNote))
                                        si.Khachghichu = item.UserNote;
                                    else
                                        si.Khachghichu = string.Empty;

                                    si.BigPackageID = Convert.ToInt32(item.BigPackageID);

                                    si.IsTemp = Convert.ToBoolean(item.IsTemp);

                                    if (item.IsLost != null)
                                        si.IsThatlac = Convert.ToBoolean(item.IsLost);
                                    else
                                        si.IsThatlac = false;

                                    if (item.IsHelpMoving != null)
                                        si.IsVCH = Convert.ToBoolean(item.IsHelpMoving);
                                    else
                                        si.IsVCH = false;

                                    double dai = 0;
                                    double rong = 0;
                                    double cao = 0;
                                    if (item.Length.ToString().ToFloat(0) > 0)
                                    {
                                        dai = Convert.ToDouble(item.Length);
                                    }
                                    if (item.Width.ToString().ToFloat(0) > 0)
                                    {
                                        rong = Convert.ToDouble(item.Width);
                                    }
                                    if (item.Height.ToString().ToFloat(0) > 0)
                                    {
                                        cao = Convert.ToDouble(item.Height);
                                    }

                                    si.dai = dai;
                                    si.rong = rong;
                                    si.cao = cao;
                                    sis.Add(si);

                                    bi.BigpackageSmallPackageCount = smallpackages.Count;
                                    bi.Smallpackages = sis;
                                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                                    return serializer.Serialize(bi);
                                }
                            }
                            return "ok";
                        }
                        else
                            return "none";
                    }
                    else
                        return "permission";
                }
                else
                    return "none";
            }
            else
                return "none";
        }


        [WebMethod]
        public static string UpdateQuantityNew(string barcode, string quantity, int status, int BigPackageID, int packageID,
        double dai, double rong, double cao, string base64, string note, string nvkiemdem, string khachghichu, string loaisanpham, int sessionID)
        {
            string username_current = HttpContext.Current.Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;            

            var package = SmallPackageController.GetByID(packageID);
            if (package != null)
            {
                double weight = 0;
                if (quantity.ToFloat(0) > 0)
                    weight = Math.Round(Convert.ToDouble(quantity), 5);
                SmallPackageController.UpdateWeightStatusAndDateInLasteWareHouseIsLost(package.ID, weight, status, currentDate, false, dai, rong, cao);
                SmallPackageController.UpdateStaffNoteCustdescproducttype(package.ID, nvkiemdem, khachghichu, loaisanpham);
                SmallPackageController.UpdateNote(package.ID, note);
                SmallPackageController.UpdateSessionID(package.ID, sessionID);

                int bID = Convert.ToInt32(package.BigPackageID);
                if (bID > 0)
                {
                    var big = BigPackageController.GetByID(bID);
                    if (big != null)
                    {
                        bool checkIschua = false;
                        var smalls = SmallPackageController.GetBuyBigPackageID(bID, "");
                        if (smalls.Count > 0)
                        {
                            foreach (var s in smalls)
                            {
                                if (s.Status < 3)
                                    checkIschua = true;
                            }
                            if (checkIschua == false)
                            {
                                BigPackageController.UpdateStatus(bID, 2, currentDate, username_current);
                            }
                        }
                    }
                }

                int maiorderID = Convert.ToInt32(package.MainOrderID);
                if (maiorderID > 0)
                {
                    var mainorder = MainOrderController.GetAllByID(maiorderID);
                    if (mainorder != null)
                    {
                        int orderID = mainorder.ID;
                        int warehouse = mainorder.ReceivePlace.ToInt(1);
                        int shipping = Convert.ToInt32(mainorder.ShippingType);
                        int warehouseFrom = Convert.ToInt32(mainorder.FromPlace);

                        var usercreate = AccountController.GetByID(Convert.ToInt32(mainorder.UID));
                        double ckFeeWeight = Convert.ToDouble(UserLevelController.GetByID(usercreate.LevelID.ToString().ToInt()).FeeWeight.ToString());

                        int LevelID = 0;
                        if (Convert.ToDouble(mainorder.PriceVND) > 10000000)
                            LevelID = 4;
                        else
                            LevelID = Convert.ToInt32(usercreate.LevelID);
                        
                        double totalweight = weight;
                        double FeeWeight = 0;
                        double FeeWeightDiscount = 0;
                        double returnprice = 0;
                        double pricePerWeight = 0;
                        double cannangdonggo = 0;
                        double TongCanNang = 0;

                        var smallpackage = SmallPackageController.GetByMainOrderID(orderID);
                        if (smallpackage.Count > 0)
                        {
                            double totalWeight = 0;
                            foreach (var item in smallpackage)
                            {
                                double compareSize = 0;
                                double weightcn = Convert.ToDouble(item.Weight);

                                double pDai = Convert.ToDouble(item.Length);
                                double pRong = Convert.ToDouble(item.Width);
                                double pCao = Convert.ToDouble(item.Height);
                                if (pDai > 0 && pRong > 0 && pCao > 0)
                                {
                                    compareSize = (pDai * pRong * pCao) / 6000;
                                }

                                if (weightcn >= compareSize)
                                {
                                    totalWeight += Math.Round(weightcn, 5);
                                }
                                else
                                {
                                    totalWeight += Math.Round(compareSize, 5);
                                }
                            }
                            totalweight = Math.Round(totalWeight, 5);
                            if (!string.IsNullOrEmpty(usercreate.FeeTQVNPerWeight))
                            {
                                double feetqvn = 0;
                                if (usercreate.FeeTQVNPerWeight.ToFloat(0) > 0)
                                {
                                    feetqvn = Convert.ToDouble(usercreate.FeeTQVNPerWeight);
                                    pricePerWeight = feetqvn;
                                }
                                returnprice = totalweight * feetqvn;
                            }
                            else
                            {
                                var fee = FeeBuyProController.GetFeeLevelID(LevelID);
                                if (fee != null)
                                {
                                    if (warehouse == 1)
                                    {
                                        pricePerWeight = Convert.ToDouble(fee.FeeWeightHN);
                                    }
                                    else
                                    {
                                        pricePerWeight = Convert.ToDouble(fee.FeeWeightSG);
                                    }
                                    returnprice = totalWeight * pricePerWeight;
                                }
                            }

                            cannangdonggo = totalWeight;
                            TongCanNang = totalWeight;
                          
                            foreach (var item in smallpackage)
                            {

                                double compareSize = 0;
                                double weightcn = Convert.ToDouble(item.Weight);

                                double pDai = Convert.ToDouble(item.Length);
                                double pRong = Convert.ToDouble(item.Width);
                                double pCao = Convert.ToDouble(item.Height);
                                if (pDai > 0 && pRong > 0 && pCao > 0)
                                {
                                    compareSize = (pDai * pRong * pCao) / 6000;
                                }
                                if (weight >= compareSize)
                                {
                                    double TotalPriceCN = weightcn * pricePerWeight;
                                    TotalPriceCN = Math.Round(TotalPriceCN, 0);
                                    SmallPackageController.UpdateTotalPrice(item.ID, TotalPriceCN);
                                }
                                else
                                {
                                    double TotalPriceTT = compareSize * pricePerWeight;
                                    TotalPriceTT = Math.Round(TotalPriceTT, 0);
                                    SmallPackageController.UpdateTotalPrice(item.ID, TotalPriceTT);
                                }
                            }
                        }

                        FeeWeight = Math.Round(returnprice, 0);
                        FeeWeightDiscount = FeeWeight * ckFeeWeight / 100;
                        FeeWeightDiscount = Math.Round(FeeWeightDiscount, 0);
                        FeeWeight = FeeWeight - FeeWeightDiscount;
                        FeeWeight = Math.Round(FeeWeight, 0);

                        double currency = Convert.ToDouble(mainorder.CurrentCNYVN);
                        double FeeShipCN = Math.Round(Convert.ToDouble(mainorder.FeeShipCN), 0);
                        double FeeBuyPro = Math.Round(Convert.ToDouble(mainorder.FeeBuyPro), 0);
                        double IsCheckProductPrice = Math.Round(Convert.ToDouble(mainorder.IsCheckProductPrice), 0);
                        double IsPackedPrice = Math.Round(Convert.ToDouble(mainorder.IsPackedPrice), 0);
                        double IsPriceSepcial = Math.Round(Convert.ToDouble(mainorder.IsCheckPriceSpecial), 0);

                        var conf = ConfigurationController.GetByTop1();
                        cannangdonggo = Math.Round(cannangdonggo, 5);
                        TongCanNang = Math.Round(TongCanNang, 5);
                        if (mainorder.IsPacked == true)
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
                                    cannangdonggo = Math.Round(cannangdonggo, 5);
                                    IsPackedPrice = Convert.ToDouble(conf.FeeDongGoKgDau) + (cannangdonggo * Convert.ToDouble(conf.FeeDongGoKgSau));
                                }
                            }
                        }

                        if (mainorder.IsCheckSpecial1 == true)
                        {
                            if (TongCanNang > 0)
                            {
                                IsPriceSepcial = (TongCanNang * Convert.ToDouble(conf.FeeDacBiet1));
                            }
                        }
                        if (mainorder.IsCheckSpecial2 == true)
                        {
                            if (TongCanNang > 0)
                            {
                                IsPriceSepcial = (TongCanNang * Convert.ToDouble(conf.FeeDacBiet2));
                            }
                        }
                        if (mainorder.IsCheckSpecial1 == true && mainorder.IsCheckSpecial2 == true)
                        {
                            if (TongCanNang > 0)
                            {
                                IsPriceSepcial = (TongCanNang * (Convert.ToDouble(conf.FeeDacBiet1) + Convert.ToDouble(conf.FeeDacBiet2)));
                            }
                        }

                        IsPackedPrice = Math.Round(IsPackedPrice, 0);
                        IsPriceSepcial = Math.Round(IsPriceSepcial, 0);
                        double IsFastDeliveryPrice = Math.Round(Convert.ToDouble(mainorder.IsFastDeliveryPrice), 0);
                        double TotalFeeSupport = Math.Round(Convert.ToDouble(mainorder.TotalFeeSupport), 0);
                        double InsuranceMoney = Math.Round(Convert.ToDouble(mainorder.InsuranceMoney), 0);

                        double PriceVND = 0;
                        if (mainorder.PriceVND.ToFloat(0) > 0)
                            PriceVND = Math.Round(Convert.ToDouble(mainorder.PriceVND), 0);

                        double Deposit = Math.Round(Convert.ToDouble(mainorder.Deposit), 0);

                        double TotalPriceVND = FeeShipCN + FeeBuyPro + FeeWeight + IsCheckProductPrice + IsPackedPrice
                                  + IsFastDeliveryPrice + PriceVND + TotalFeeSupport + InsuranceMoney + IsPriceSepcial;

                        TotalPriceVND = Math.Round(TotalPriceVND, 0);

                        MainOrderController.UpdateFee(mainorder.ID, Deposit.ToString(), FeeShipCN.ToString(), FeeBuyPro.ToString(), FeeWeight.ToString(),
                        IsCheckProductPrice.ToString(), IsPackedPrice.ToString(), IsFastDeliveryPrice.ToString(), TotalPriceVND.ToString(), IsPriceSepcial.ToString());
                        MainOrderController.UpdateFeeWeightCK(mainorder.ID, FeeWeightDiscount.ToString());
                        MainOrderController.UpdateTotalWeightandTongCanNang(mainorder.ID, totalweight.ToString(), totalweight.ToString(), TongCanNang.ToString());
                        var accChangeData = AccountController.GetByUsername(username_current);
                        if (accChangeData != null)
                        {
                            if (status == 3)
                            {
                                HistoryOrderChangeController.Insert(mainorder.ID, accChangeData.ID, accChangeData.Username, accChangeData.Username +
                                " đã đổi trạng thái của mã vận đơn: <strong>" + barcode
                                + "</strong> của đơn hàng ID là: " + mainorder.ID + ", là: Đã về kho VN", 8, currentDate);
                            }

                            if (Convert.ToDouble(package.Weight) != Convert.ToDouble(quantity))
                            {
                                HistoryOrderChangeController.Insert(mainorder.ID, accChangeData.ID, accChangeData.Username, accChangeData.Username +
                                           " đã đổi cân nặng của mã vận đơn: <strong>" + barcode
                                           + "</strong> của đơn hàng ID là: " + mainorder.ID + ", từ: " + package.Weight + " , sang: " + quantity + "", 8, currentDate);
                            }

                            int MainorderID = mainorder.ID;
                            if (mainorder.Status < 9)
                            {
                                if (mainorder.DateVN == null)                                
                                    MainOrderController.UpdateDateVN(mainorder.ID, currentDate);                                
                                MainOrderController.UpdateStatus(MainorderID, Convert.ToInt32(mainorder.UID), 8);

                                var setNoti = SendNotiEmailController.GetByID(9);
                                if (setNoti != null)
                                {
                                    var acc = AccountController.GetByID(mainorder.UID.Value);
                                    if (acc != null)
                                    {
                                        if (setNoti.IsSentNotiUser == true)
                                        {
                                            if (mainorder.OrderType == 1)
                                            {
                                                NotificationsController.Inser(acc.ID,
                                                 acc.Username, MainorderID,
                                                 "Hàng của đơn hàng " + MainorderID + " đã về kho VN.", 1,
                                                 currentDate, username_current, true);
                                            }
                                            else
                                            {
                                                NotificationsController.Inser(acc.ID,
                                                 acc.Username, MainorderID,
                                                 "Hàng của đơn hàng TMĐT " + MainorderID + " đã về kho VN.", 11,
                                                 currentDate, username_current, true);
                                            }
                                        }
                                    }
                                }
                            }
                            HistoryOrderChangeController.Insert(mainorder.ID, accChangeData.ID, accChangeData.Username, accChangeData.Username +
                                               " đã đổi trạng thái đơn hàng ID " + mainorder.ID + " đã về kho Hà Nội", 8, currentDate);
                        }

                        return "1";
                    }
                    else
                        return "none";
                }
                else
                    return "none";
            }
            else
                return "none";
        }

        protected void btnCompleted_Click(object sender, EventArgs e)
        {
            var username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int sessionID = ddlSession.SelectedValue.ToInt();
                if (sessionID > 0)
                {
                    var ss = WorkingSessionController.GetByID(sessionID);
                    if (ss != null)
                    {
                        string kq = WorkingSessionController.UpdateStatus(sessionID, 2, DateTime.Now, obj_user.Username);                         
                        Response.Redirect("/manager/WorkingSession.aspx");
                    }    
                }    
            }    
        }

    }
}