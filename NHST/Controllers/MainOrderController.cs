using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHST.Models;
using NHST.Bussiness;
using System.Data;
using WebUI.Business;
using MB.Extensions;

namespace NHST.Controllers
{
    public class MainOrderController
    {
        #region CRUD
        public static double GetTotalPriceVND(string status, string PriceType, string searchtext, int Type, int orderType, int PTVC, string fd, string td, int nvdh, int nvkd, int nvcs)
        {
            var sql = @"select total=SUM(CAST(" + PriceType + " as float)) ";
            sql += "FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN ";
            sql += " dbo.tbl_AccountInfo AS ui ON mo.UID = ui.UID  LEFT OUTER JOIN  ";
            sql += "dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN ";           
            sql += "(SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage) sm ON sm.MainOrderID = mo.ID and totalSmallPackages = 1 LEFT OUTER JOIN ";
            sql += "(SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1 ";
            sql += "LEFT OUTER JOIN(SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
            sql += "LEFT OUTER JOIN ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.OrderTransactionCode  AS [text()] ";
            sql += "FROM tbl_SmallPackage ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [MaVanDon] FROM tbl_MainOder ST2) se on se.ID=mo.ID ";
            sql += "LEFT OUTER JOIN ";
            sql += " (SELECT DISTINCT ST2.ID, SUBSTRING((SELECT ',' + ST1.MainOrderCode   AS [text()] FROM tbl_MainOrderCode ST1 WHERE ST1.MainOrderID = ST2.ID FOR XML PATH('')), 2, 1000)[MaDonHangString] FROM tbl_MainOder ST2) sz on sz.ID = mo.ID ";
            sql += "LEFT OUTER JOIN ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.title_origin  AS [text()] ";
            sql += "FROM tbl_Order ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [TenSanPham] FROM tbl_MainOder ST2) sp on sp.ID=mo.ID ";
            sql += "Where mo.IsHidden = 0 and  mo.OrderType = '" + orderType + "'";
            if (nvdh > 0)
            {
                sql += " AND mo.DathangID =" + nvdh + "";
            }
            if (nvkd > 0)
            {
                sql += " AND mo.SalerID =" + nvkd + "";
            }
            if (nvcs > 0)
            {
                sql += " AND mo.CSID =" + nvcs + "";
            }
            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 1)
                    sql += " AND mo.ID like N'%" + searchtext + "%'";

                if (Type == 2)
                    sql += " AND mo.MainOrderCode like N'%" + searchtext + "%'";

                if (Type == 3)
                    sql += " AND mo.OrderTransactionCode like N'%" + searchtext + "%'";

                if (Type == 4)
                    sql += "  AND u.Email like N'%" + searchtext + "%'";

                if (Type == 5)
                    sql += " AND u.Username like N'%" + searchtext + "%'";

                if (Type == 6)
                    sql += " AND ui.Phone like N'%" + searchtext + "%'";
            }
            if (PTVC > 0)
            {
                sql += "    AND mo.ShippingType = " + PTVC + " ";
            }
            if (!string.IsNullOrEmpty(status))
            {
                if (status != "-1")
                    sql += " AND mo.Status in (" + status + ")";
            }
            if (status == "2")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DepositDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DepositDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "3")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateShipper >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateShipper <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "4")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuy >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuy <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "5")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuyOK >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuyOK <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "6")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateTQ >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateTQ <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "7")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToVN >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToVN <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "8")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateVN >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateVN <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "9")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.PayDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.PayDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "10")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CompleteDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CompleteDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "11")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToShip >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToShip <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else if (status == "12")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToCancel >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToCancel <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }
            //if (!string.IsNullOrEmpty(mvd))
            //{
            //    sql += " And  mo.OrderTransactionCode like N'%" + mvd + "%'";
            //}
            //if (!string.IsNullOrEmpty(mdh))
            //{
            //    sql += " And mo.MainOrderCode like N'%" + mdh + "%'";
            //}
            //if (!string.IsNullOrEmpty(fd))
            //{
            //    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
            //    sql += "AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            //}
            //if (!string.IsNullOrEmpty(td))
            //{
            //    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
            //    sql += "AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            //}
            //if (!string.IsNullOrEmpty(priceFrom))
            //{
            //    sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            //}
            //if (!string.IsNullOrEmpty(priceTo) && !string.IsNullOrEmpty(priceFrom))
            //{
            //    if (Convert.ToDouble(priceFrom) <= Convert.ToDouble(priceTo))
            //        sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            //}
            //if (isNotCode == true)
            //{
            //    sql += " AND sm.totalSmallPackages is null";
            //}
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            double total = 0;
            while (reader.Read())
            {
                if (reader["total"] != DBNull.Value)
                    total = Convert.ToDouble(reader["total"].ToString());
            }
            reader.Close();
            return total;
        }
        public static string UpdateCYN(int ID, string FeeShipCNRealCYN, string FeeShipCNCYN, string IsCheckProductPriceCYN, string IsPackedPriceCYN)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.FeeShipCNRealCYN = FeeShipCNRealCYN;
                    or.FeeShipCNCYN = FeeShipCNCYN;
                    or.IsCheckProductPriceCYN = IsCheckProductPriceCYN;
                    or.IsPackedPriceCYN = IsPackedPriceCYN;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateCurrentCNYVN(int ID, string CurrentCNYVN)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    var obj_user = AccountController.GetByID(Convert.ToInt32(or.UID));
                    DateTime currentDate = DateTime.Now;
                    if (obj_user != null)
                    {
                        double LessDeposit = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).LessDeposit);
                        if (!string.IsNullOrEmpty(obj_user.Deposit.ToString()))
                        {
                            if (Convert.ToDouble(obj_user.Deposit) > 0)
                            {
                                LessDeposit = Convert.ToDouble(obj_user.Deposit);
                            }
                        }


                        double UL_CKFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeBuyPro);
                        double servicefee = 0;
                        double feebuypropt = 0;
                        //var adminfeebuypro = FeeBuyProController.GetAll();
                        //if (adminfeebuypro.Count > 0)
                        //{
                        //    foreach (var temp in adminfeebuypro)
                        //    {
                        //        if (Convert.ToDouble(or.PriceVND) >= temp.AmountFrom && Convert.ToDouble(or.PriceVND) < temp.AmountTo)
                        //        {
                        //            servicefee = temp.FeePercent.ToString().ToFloat(0) / 100;
                        //            feebuypropt = Convert.ToDouble(temp.FeePercent);
                        //            //servicefeeMoney = Convert.ToDouble(item.FeeMoney);
                        //            break;
                        //        }
                        //    }
                        //}
                        double feebp = 0;



                        var listorder = OrderController.GetByMainOrderID(or.ID);
                        var mainorder = MainOrderController.GetAllByID(or.ID);
                        double pricevnd = 0;
                        double pricecyn = 0;

                        if (mainorder != null)
                        {
                            double current = Convert.ToDouble(CurrentCNYVN);
                            double InsurancePercent = Convert.ToDouble(mainorder.InsurancePercent);
                            if (listorder != null)
                            {
                                if (listorder.Count > 0)
                                {

                                    foreach (var item in listorder)
                                    {
                                        double originprice = Math.Round(Convert.ToDouble(item.price_origin), 2);
                                        double promotionprice = Math.Round(Convert.ToDouble(item.price_promotion), 2);
                                        double oprice = 0;
                                        if (promotionprice > 0)
                                        {
                                            if (promotionprice < originprice)
                                            {
                                                pricecyn += promotionprice * Convert.ToDouble(item.quantity);
                                                oprice = promotionprice * Convert.ToDouble(item.quantity) * current;
                                            }
                                            else
                                            {
                                                pricecyn += originprice * Convert.ToDouble(item.quantity);
                                                oprice = originprice * Convert.ToDouble(item.quantity) * current;
                                            }
                                        }
                                        else
                                        {
                                            pricecyn += originprice * Convert.ToDouble(item.quantity);
                                            oprice = originprice * Convert.ToDouble(item.quantity) * current;
                                        }
                                        pricevnd += oprice;
                                    }
                                    pricevnd = Math.Round(pricevnd, 0);
                                    pricecyn = Math.Round(pricecyn, 2);
                                }
                            }
                        }





                        string money = (Convert.ToDecimal(pricecyn) * CurrentCNYVN.ToDecimal()).ToString();
                        if (Convert.ToDouble(or.PriceVND) != Convert.ToDouble(money))
                        {
                            var fe = FeeBuyProController.GetByTypeAndPrice(or.ShippingType.Value, Convert.ToDouble(money));
                            if (fe != null)
                            {
                                var fee = fe.FeePercent;
                                feebp = Convert.ToDouble(money) * fee.Value / 100;
                                feebuypropt = fee.Value;
                                if (or.ShippingType.Value == 1 && feebp < 10000)
                                    feebp = 10000;
                                if (or.ShippingType.Value == 4 && feebp < 15000)
                                    feebp = 15000;
                            }
                        }

                        double feebpnotdc = 0;
                        string PriceVND = or.PriceVND;
                        //int soluong = OrderTempController.GetTotalProduct(oshops.ID);
                        string FeeBuyProUser = "";
                        if (!string.IsNullOrEmpty(obj_user.FeeBuyPro))
                        {
                            if (obj_user.FeeBuyPro.ToFloat(0) > 0)
                            {
                                feebpnotdc = Convert.ToDouble(or.PriceVND) * Convert.ToDouble(obj_user.FeeBuyPro) / 100;
                                feebuypropt = Convert.ToDouble(obj_user.FeeBuyPro);
                                FeeBuyProUser = obj_user.FeeBuyPro;
                            }
                        }
                        else
                            feebpnotdc = Convert.ToDouble(or.PriceVND) * servicefee;
                        double subfeebp = feebpnotdc * UL_CKFeeBuyPro / 100;
                        feebp = feebpnotdc - subfeebp;
                        feebp = Math.Round(feebp, 0);

                        var fbp = UserLevelController.GetByID(AccountController.GetByID(or.UID.Value).LevelID.Value).FeeBuyPro.Value;
                        or.CurrentCNYVN = CurrentCNYVN;
                        or.PriceVND = (Convert.ToDecimal(pricecyn) * CurrentCNYVN.ToDecimal()).ToString();
                        if (or.TotalPriceRealCYN != null && or.TotalPriceRealCYN.ToDecimal() > 0)
                        {
                            or.TotalPriceReal = (or.TotalPriceRealCYN.ToDecimal() * CurrentCNYVN.ToDecimal()).ToString();
                        }
                        if (or.FeeShipCNCYN != null && or.FeeShipCNCYN.ToDecimal() > 0)
                        {
                            or.FeeShipCN = (or.FeeShipCNCYN.ToDecimal() * CurrentCNYVN.ToDecimal()).ToString();
                        }
                        if (or.FeeShipCNRealCYN != null && or.FeeShipCNRealCYN.ToDecimal() > 0)
                        {
                            or.FeeShipCNReal = (or.FeeShipCNRealCYN.ToDecimal() * CurrentCNYVN.ToDecimal()).ToString();
                        }
                        if (or.IsCheckProductPriceCYN != null && or.IsCheckProductPriceCYN.ToDecimal() > 0)
                        {
                            or.IsCheckProductPrice = (or.IsCheckProductPriceCYN.ToDecimal() * CurrentCNYVN.ToDecimal()).ToString();
                        }
                        //if (or.IsPackedPriceCYN != null && or.IsPackedPriceCYN.ToDecimal() > 0)
                        //{
                        //    or.IsPackedPrice = (or.IsPackedPriceCYN.ToDecimal() * CurrentCNYVN.ToDecimal()).ToString();
                        //}
                        decimal fbp1;
                        if (fbp > 0)
                        {
                            fbp1 = (Convert.ToDecimal(pricecyn) * CurrentCNYVN.ToDecimal() * Convert.ToDecimal(feebuypropt) / 100) * Convert.ToDecimal(100 - fbp) / 100;
                            if (or.ShippingType.Value == 1 && fbp1 < 10000)
                                fbp1 = 10000;
                            if (or.ShippingType.Value == 4 && fbp1 < 15000)
                                fbp1 = 15000;
                        }
                        else
                        {
                            fbp1 = Convert.ToDecimal(pricecyn) * CurrentCNYVN.ToDecimal() * Convert.ToDecimal(feebuypropt) / 100;
                            if (or.ShippingType.Value == 1 && fbp1 < 10000)
                                fbp1 = 10000;
                            if (or.ShippingType.Value == 4 && fbp1 < 15000)
                                fbp1 = 15000;
                        }
                        or.FeeBuyPro = Math.Round(fbp1, 0).ToString();
                        or.AmountDeposit = (((Convert.ToDecimal(pricecyn) * CurrentCNYVN.ToDecimal())) * Convert.ToDecimal(LessDeposit) / 100).ToString();
                        dbe.Configuration.ValidateOnSaveEnabled = false;
                        dbe.SaveChanges();

                    }
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string Insert_Same(int UID, string ShopID, string ShopName, string Site, bool IsForward, string IsForwardPrice, bool IsFastDelivery, string IsFastDeliveryPrice, bool IsCheckProduct,
          string IsCheckProductPrice, bool IsPacked, string IsPackedPrice, bool IsFast, string IsFastPrice, string PriceVND, string PriceCNY, string FeeShipCN, string FeeBuyPro, string FeeWeight,
          string Note, string FullName, string Address, string Email, string Phone, int Status, string Deposit, string CurrentCNYVN, string TotalPriceVND,
          int SalerID, int DathangID, DateTime CreatedDate, int CreatedBy, string AmountDeposit, int OrderType, string FeeShipCNReal, int MaDonTruoc, bool IsCheckSpecial1, bool IsCheckSpecial2)
        {
            using (var dbe = new NHSTEntities())
            {

                tbl_MainOder o = new tbl_MainOder();
                o.UID = UID;
                o.ShopID = ShopID;
                o.ShopName = ShopName;
                o.Site = Site;
                o.IsForward = IsForward;
                o.IsForwardPrice = IsForwardPrice;
                o.IsFastDelivery = IsFastDelivery;
                o.IsFastDeliveryPrice = IsFastDeliveryPrice;
                o.MaDonTruoc = MaDonTruoc;
                o.IsCheckProduct = IsCheckProduct;
                o.IsCheckPriceSpecial = "0";
                o.IsCheckSpecial1 = IsCheckSpecial1;
                o.IsCheckSpecial2 = IsCheckSpecial2;
                o.IsCheckProductPrice = IsCheckProductPrice;
                o.IsPacked = IsPacked;
                o.IsPackedPrice = IsPackedPrice;
                o.IsFast = IsFast;
                o.IsFastPrice = IsFastPrice;
                o.PriceVND = PriceVND;
                o.PriceCNY = PriceCNY;
                o.FeeShipCN = FeeShipCN;
                o.FeeBuyPro = FeeBuyPro;
                o.FeeShipCNReal = FeeShipCNReal;
                o.FeeWeight = FeeWeight;
                o.Note = Note;
                o.FullName = FullName;
                o.Address = Address;
                o.Email = Email;
                o.Phone = Phone;
                o.Status = Status;
                o.Deposit = Deposit;
                o.CurrentCNYVN = CurrentCNYVN;
                o.TotalPriceVND = TotalPriceVND;
                o.SalerID = SalerID;
                o.DathangID = DathangID;
                o.KhoTQID = 0;
                o.KhoVNID = 0;
                o.FeeShipCNToVN = "0";
                o.CreatedDate = CreatedDate;
                o.CreatedBy = CreatedBy;
                o.IsHidden = false;
                o.AmountDeposit = AmountDeposit;
                o.OrderType = OrderType;
                dbe.tbl_MainOder.Add(o);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                dbe.SaveChanges();
                string k = o.ID.ToString();
                return k;
            }
        }




        public static string Insert(int UID, string ShopID, string ShopName, string Site, bool IsForward, string IsForwardPrice, bool IsFastDelivery, string IsFastDeliveryPrice, bool IsCheckProduct,
            string IsCheckProductPrice, bool IsPacked, string IsPackedPrice, bool IsFast, string IsFastPrice, string PriceVND, string PriceCNY, string FeeShipCN, string FeeBuyPro, string FeeWeight,
            string Note, string FullName, string Address, string Email, string Phone, int Status, string Deposit, string CurrentCNYVN, string TotalPriceVND,
            int SalerID, int DathangID, DateTime CreatedDate, int CreatedBy, string AmountDeposit, int OrderType)
        {
            using (var dbe = new NHSTEntities())
            {

                tbl_MainOder o = new tbl_MainOder();
                o.UID = UID;
                o.ShopID = ShopID;
                o.ShopName = ShopName;
                o.FeeShipCNReal = "0";
                o.Site = Site;
                o.IsForward = IsForward;
                o.IsForwardPrice = IsForwardPrice;
                o.IsFastDelivery = IsFastDelivery;
                o.IsFastDeliveryPrice = IsFastDeliveryPrice;
                o.IsCheckProduct = IsCheckProduct;
                o.IsCheckProductPrice = IsCheckProductPrice;
                o.IsPacked = IsPacked;
                o.IsPackedPrice = IsPackedPrice;
                o.IsFast = IsFast;
                o.IsFastPrice = IsFastPrice;
                o.IsCheckSpecial1 = false;
                o.IsCheckSpecial2 = false;
                o.IsCheckPriceSpecial = "0";
                o.PriceVND = PriceVND;
                o.PriceCNY = PriceCNY;
                o.FeeShipCN = FeeShipCN;
                o.FeeBuyPro = FeeBuyPro;
                o.FeeWeight = FeeWeight;
                o.Note = Note;
                o.FullName = FullName;
                o.Address = Address;
                o.Email = Email;
                o.Phone = Phone;
                o.Status = Status;
                o.Deposit = Deposit;
                o.CurrentCNYVN = CurrentCNYVN;
                o.TotalPriceVND = TotalPriceVND;
                o.SalerID = SalerID;
                o.DathangID = DathangID;
                o.KhoTQID = 0;
                o.KhoVNID = 0;
                o.FeeShipCNToVN = "0";
                o.CreatedDate = CreatedDate;
                o.CreatedBy = CreatedBy;
                o.IsHidden = false;
                o.IsUpdatePrice = false;
                o.AmountDeposit = AmountDeposit;
                o.OrderType = OrderType;
                dbe.tbl_MainOder.Add(o);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                dbe.SaveChanges();
                string k = o.ID.ToString();
                return k;
            }
        }
        public static string UpdateStaff(int ID, int SalerID, int DathangID, int KhoTQID, int KhoVNID)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.SalerID = SalerID;
                    or.DathangID = DathangID;
                    or.KhoTQID = KhoTQID;
                    or.KhoVNID = KhoVNID;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateCSKHID(int ID, int CSID)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.CSID = CSID;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateStatus(int ID, int UID, int Status)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.Status = Status;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateNoteManager(int ID, string NoteManager)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.NoteManager = NoteManager;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateExpectedDate(int ID, DateTime ExpectedDate)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.ExpectedDate = ExpectedDate;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateYCG(int ID, bool IsGiaoHang)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsGiaohang = IsGiaoHang;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDoneSmallPackage(int ID, bool IsDoneSmallPackage)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsDoneSmallPackage = IsDoneSmallPackage;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateStatusByID(int ID, int Status)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.Status = Status;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateDeposit(int ID, string Deposit)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.Deposit = Deposit;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateOrderWeight(int ID, string OrderWeight)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderWeight = OrderWeight;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateTotalFeeSupport(int ID, string TotalFeeSupport)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TotalFeeSupport = TotalFeeSupport;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateAmountDeposit(int ID, string AmountDeposit)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.AmountDeposit = AmountDeposit;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateProduct(int ID, string AmountDeposit, string Deposit, string FeeBuyPro, string TotalPriceVND)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.AmountDeposit = AmountDeposit;
                    or.Deposit = Deposit;
                    or.FeeBuyPro = FeeBuyPro;
                    or.TotalPriceVND = TotalPriceVND;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateNote(int ID, string Note)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.Note = Note;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateCheckPro(int ID, bool IsCheckProduct)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsCheckProduct = IsCheckProduct;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateIsUpdatePrice(int ID, bool IsUpdatePrice)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsUpdatePrice = IsUpdatePrice;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateBaogia(int ID, bool IsCheckNotiPrice)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsCheckNotiPrice = IsCheckNotiPrice;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateIsGiaohang(int ID, bool IsGiaohang)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsGiaohang = IsGiaohang;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateOrderDone(int ID, bool OrderDone)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderDone = OrderDone;
                    //or.Status = 4;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateOrderPrice(int ID, bool OrderPrice)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderPrice = OrderPrice;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateIsPacked(int ID, bool IsPacked)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsPacked = IsPacked;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateIsSpecial(int ID, bool IsCheckSpecial1, bool IsCheckSpecial2)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsCheckSpecial1 = IsCheckSpecial1;
                    or.IsCheckSpecial2 = IsCheckSpecial2;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateFeeWeightDC(int ID, string Feeweightdc)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.FeeWeightCK = Feeweightdc;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateIsFastDelivery(int ID, bool IsFastDelivery)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsFastDelivery = IsFastDelivery;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }


        public static string UpdateIsInsurrance(int ID, bool IsInsurrance)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsInsurrance = IsInsurrance;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateInsurranceMoney(int ID, string InsuranceMoney, string InsurancePercent)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.InsuranceMoney = InsuranceMoney;
                    or.InsurancePercent = InsurancePercent;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateLinkImage(int ID, string LinkImage)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.LinkImage = LinkImage;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdatePercentDeposit(int ID, string PercentDeposit)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.PercentDeposit = PercentDeposit;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateTotalFeeBuyPro(int ID, string TotalPriceVND, string FeeBuyPro)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TotalPriceVND = TotalPriceVND;
                    or.FeeBuyPro = FeeBuyPro;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateOrderWeightCK(int ID, string OrderWeightCKS)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.FeeWeightCK = OrderWeightCKS;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateOrderTransactionCode(int ID, string OrderTransactionCode, string OrderTransactionCodeweight)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderTransactionCode = OrderTransactionCode;
                    or.OrderTransactionCodeWeight = OrderTransactionCodeweight;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateOrderTransactionCode2(int ID, string OrderTransactionCode2, string OrderTransactionCodeweight2)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderTransactionCode2 = OrderTransactionCode2;
                    or.OrderTransactionCodeWeight2 = OrderTransactionCodeweight2;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateOrderTransactionCode3(int ID, string OrderTransactionCode3, string OrderTransactionCodeweight3)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderTransactionCode3 = OrderTransactionCode3;
                    or.OrderTransactionCodeWeight3 = OrderTransactionCodeweight3;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateOrderTransactionCode4(int ID, string OrderTransactionCode4, string OrderTransactionCodeweight4)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderTransactionCode4 = OrderTransactionCode4;
                    or.OrderTransactionCodeWeight4 = OrderTransactionCodeweight4;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateOrderTransactionCode5(int ID, string OrderTransactionCode5, string OrderTransactionCodeweight5)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderTransactionCode5 = OrderTransactionCode5;
                    or.OrderTransactionCodeWeight5 = OrderTransactionCodeweight5;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateDeposit(int ID, int UID, string Deposit)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.Deposit = Deposit;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateReceivePlace(int ID, int UID, string ReceivePlace, int ShippingType)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.ReceivePlace = ReceivePlace;
                    or.ShippingType = ShippingType;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateFromPlace(int ID, int UID, int FromPlace, int ShippingType)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.FromPlace = FromPlace;
                    or.ShippingType = ShippingType;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateFTS(int ID, int UID, int FromPlace, string ReceivePlace, int ShippingType)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.FromPlace = FromPlace;
                    or.ReceivePlace = ReceivePlace;
                    or.ShippingType = ShippingType;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateIsCheckNotiPrice(int ID, int UID, bool IsCheckNotiPrice)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsCheckNotiPrice = IsCheckNotiPrice;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateOrderType(int ID, int UID, int OrderType)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderType = OrderType;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateTQVNWeight(int ID, int UID, string TQVNWeight)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TQVNWeight = TQVNWeight;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateMainOrderCode(int ID, int UID, string MainOrderCode)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.MainOrderCode = MainOrderCode;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateTotalPriceReal(int ID, string TotalPriceReal, string TotalPriceRealCYN)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TotalPriceReal = TotalPriceReal;
                    or.TotalPriceRealCYN = TotalPriceRealCYN;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateTotalPriceVNDSupport(int ID, string TotalPriceVND)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TotalPriceVND = TotalPriceVND;
                    or.TotalFeeSupport = "0";
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateTimeline(int ID, string Timeline)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TimeLine = Timeline;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDepositDate(int ID, DateTime DepositDate)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DepostiDate = DepositDate;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDateShipper(int ID, DateTime DateShipper)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DateShipper = DateShipper;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDateBuy(int ID, DateTime DateBuy)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DateBuy = DateBuy;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDateBuyOK(int ID, DateTime DateBuyok)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DateBuyOK = DateBuyok;
                    or.OrderPrice = true;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDateTQ(int ID, DateTime DateTQ)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DateTQ = DateTQ;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDateVN(int ID, DateTime DateVN)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DateVN = DateVN;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDateToVN(int ID, DateTime DateToVN)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DateToVN = DateToVN;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDateToShip(int ID, DateTime DateToShip)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DateToShip = DateToShip;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string Remove(int ID)
        {
            using (var db = new NHSTEntities())
            {
                var p = db.tbl_MainOder.Where(x => x.ID == ID).SingleOrDefault();
                if (p != null)
                {
                    db.tbl_MainOder.Remove(p);
                    db.SaveChanges();
                    return "ok";
                }
                return null;
            }
        }
        public static string UpdatePayDate(int ID, DateTime PayDate)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.PayDate = PayDate;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateIsDebt(int ID, bool IsDebt)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsDebt = IsDebt;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateCompleteDate(int ID, DateTime CompleteDate)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.CompleteDate = CompleteDate;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDateToFis(int ID, DateTime Date)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DateToFis = Date;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateDateToCancel(int ID, DateTime Date)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.DateToCancel = Date;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateEditCurrency(int ID, string CurrentCNYVN, string AmountDeposit, string Deposit, string FeeShipCN,
        string FeeShipCNReal, string FeeBuyPro, string PriceVND, string PriceCNY, string TotalPriceVND, string TotalPriceReal)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.CurrentCNYVN = CurrentCNYVN;
                    or.AmountDeposit = AmountDeposit;
                    or.Deposit = Deposit;
                    or.FeeShipCN = FeeShipCN;
                    or.FeeShipCNReal = FeeShipCNReal;
                    or.FeeBuyPro = FeeBuyPro;
                    or.PriceVND = PriceVND;
                    or.PriceCNY = PriceCNY;
                    or.TotalPriceVND = TotalPriceVND;
                    or.TotalPriceReal = TotalPriceReal;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateFee(int ID, string Deposit, string FeeShipCN, string FeeBuyPro, string FeeWeight,
           string IsCheckProductPrice, string IsPackedPrice, string IsFastDeliveryPrice, string TotalPriceVND, string IsCheckPriceSpecial)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.Deposit = Deposit;
                    or.FeeShipCN = FeeShipCN;
                    or.FeeBuyPro = FeeBuyPro;
                    or.FeeWeight = FeeWeight;
                    or.IsCheckProductPrice = IsCheckProductPrice;
                    or.IsPackedPrice = IsPackedPrice;
                    or.IsFastDeliveryPrice = IsFastDeliveryPrice;
                    or.TotalPriceVND = TotalPriceVND;
                    or.IsCheckPriceSpecial = IsCheckPriceSpecial;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateFeeInclueFeeSupport(int ID, string Deposit, string FeeShipCN, string FeeBuyPro, string FeeWeight,
            string IsPackedPrice, string FeeSupport, string TotalPriceVND, string IsCheckPriceSpecial)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.Deposit = Deposit;
                    or.FeeShipCN = FeeShipCN;
                    or.FeeBuyPro = FeeBuyPro;
                    or.FeeWeight = FeeWeight;
                    or.IsPackedPrice = IsPackedPrice;
                    or.TotalFeeSupport = FeeSupport;
                    or.TotalPriceVND = TotalPriceVND;
                    or.IsCheckPriceSpecial = IsCheckPriceSpecial;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateFee_OrderDetail(int ID, string Deposit, string FeeShipCN, string FeeBuyPro, string FeeWeight,
           string IsCheckProductPrice, string IsPackedPrice, string IsFastDeliveryPrice, string TotalPriceVND, string FeeShipCNReal, string IsCheckPriceSpecial)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    //or.Deposit = Deposit;
                    or.FeeShipCN = FeeShipCN;
                    or.FeeBuyPro = FeeBuyPro;
                    or.FeeShipCNReal = FeeShipCNReal;
                    or.FeeWeight = FeeWeight;
                    or.IsCheckPriceSpecial = IsCheckPriceSpecial;
                    or.IsCheckProductPrice = IsCheckProductPrice;
                    or.IsPackedPrice = IsPackedPrice;
                    or.IsFastDeliveryPrice = IsFastDeliveryPrice;
                    or.TotalPriceVND = TotalPriceVND;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateFeeWeightCK(int ID, string FeeWeightCK)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(x => x.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.FeeWeightCK = FeeWeightCK;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateTotalWeight(int ID, string TotalWeight, string OrderWeight)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TQVNWeight = TotalWeight;
                    or.OrderWeight = OrderWeight;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateTotalWeightandTongCanNang(int ID, string TotalWeight, string OrderWeight, string TongCanNang)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TQVNWeight = TotalWeight;
                    or.OrderWeight = OrderWeight;
                    or.TongCanNang = TongCanNang;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static string UpdateVolume(int ID, string TongKhoi)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TQVNVolume = TongKhoi;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateWeightStatusTQ(int ID, string TotalWeight, string OrderWeight, int Status, DateTime DateTQ)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TQVNWeight = TotalWeight;
                    or.OrderWeight = OrderWeight;
                    or.Status = Status;
                    or.DateTQ = DateTQ;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateWeightStatusVN(int ID, string TotalWeight, string OrderWeight, int Status, DateTime DateVN)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.TQVNWeight = TotalWeight;
                    or.OrderWeight = OrderWeight;
                    or.Status = Status;
                    or.DateVN = DateVN;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateFeeShipTQVN(int ID, string FeeShipTQVN)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.FeeShipCNToVN = FeeShipTQVN;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateFeeWarehouse(int ID, double FeeInWareHouse)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.FeeInWareHouse = FeeInWareHouse;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdatePriceNotFee(int ID, string PriceVND)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.PriceVND = PriceVND;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdatePriceCYN(int ID, string PriceCNY)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.PriceCNY = PriceCNY;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        public static string UpdateIsHiddenTrue(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.IsHidden = true;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }
        #endregion

        #region GetAll
        public static List<View_OrderListFilterWithStatusString> GetByUserInViewFilterWithStatusString(int RoleID, int OrderType, int StaffID,
            string searchtext, int Type)
        {
            using (var dbe = new NHSTEntities())
            {
                if (RoleID != 1)
                {
                    List<View_OrderListFilterWithStatusString> lo = new List<View_OrderListFilterWithStatusString>();
                    List<View_OrderListFilterWithStatusString> losearch = new List<View_OrderListFilterWithStatusString>();
                    if (RoleID == 0)
                    {
                        lo = dbe.View_OrderListFilterWithStatusString.Where(l => l.OrderType == OrderType).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 2)
                    {
                        lo = dbe.View_OrderListFilterWithStatusString.Where(l => l.OrderType == OrderType).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 3)
                    {
                        lo = dbe.View_OrderListFilterWithStatusString.Where(l => l.Status >= 2 && l.DathangID == StaffID && l.OrderType == OrderType).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 4)
                    {
                        lo = dbe.View_OrderListFilterWithStatusString.Where(l => l.Status >= 5 && l.Status < 7 && l.OrderType == OrderType).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 5)
                    {
                        lo = dbe.View_OrderListFilterWithStatusString.Where(l => l.Status >= 5 && l.Status <= 7 && l.OrderType == OrderType).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 6)
                    {
                        lo = dbe.View_OrderListFilterWithStatusString.Where(l => l.Status != 1 && l.SalerID == StaffID && l.OrderType == OrderType).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 8)
                    {
                        lo = dbe.View_OrderListFilterWithStatusString.Where(l => l.Status >= 9 && l.Status < 10 && l.OrderType == OrderType).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else
                    {
                        lo = dbe.View_OrderListFilterWithStatusString.Where(l => l.Status >= 2 && l.OrderType == OrderType).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    if (lo.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            if (Type == 1)
                            {
                                var listos = GetMainOrderIDBySearch(searchtext);
                                if (listos.Count > 0)
                                {
                                    foreach (var id in listos)
                                    {
                                        var a = lo.Where(o => o.ID == id.ID).FirstOrDefault();
                                        if (a != null)
                                        {
                                            losearch.Add(a);
                                        }
                                    }
                                }
                            }
                            else if (Type == 2)
                            {
                                var listos = GetSmallPackageMainOrderIDBySearch(searchtext);
                                if (listos.Count > 0)
                                {
                                    foreach (var id in listos)
                                    {
                                        var a = lo.Where(o => o.ID == id.ID).FirstOrDefault();
                                        if (a != null)
                                        {
                                            losearch.Add(a);
                                        }
                                    }
                                }
                            }
                            else
                            {

                                losearch = lo.Where(o => o.MainOrderCode == searchtext).ToList();

                            }
                        }
                        else
                        {
                            losearch = lo;
                        }
                    }
                    return losearch;
                }
                return null;
            }
        }

        public static string SelectUIDByIDOrder(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var ot = dbe.tbl_MainOder.Where(o => o.ID == ID && o.IsHidden == false).FirstOrDefault();
                if (ot != null)
                {
                    return ot.UID.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
        public static tbl_MainOder GetByMainOrderCode(string MainOrderCode)
        {
            using (var dbe = new NHSTEntities())
            {
                var ot = dbe.tbl_MainOder.Where(o => o.MainOrderCode == MainOrderCode && o.IsHidden == false).FirstOrDefault();
                if (ot != null)
                {
                    return ot;
                }
                else
                {
                    return null;
                }
            }
        }
        public static tbl_MainOder GetByMainOrderCodeAndID(int MainOrderID, string MainOrderCode)
        {
            using (var dbe = new NHSTEntities())
            {
                var ot = dbe.tbl_MainOder.Where(o => o.ID == MainOrderID && o.MainOrderCode == MainOrderCode && o.IsHidden == false).FirstOrDefault();
                if (ot != null)
                {
                    return ot;
                }
                else
                {
                    return null;
                }
            }
        }


        public static string UpdateMainOrderCode_Thang(int ID, int UID, string MainOrderCode, int QuantityMDH)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.MainOrderCode = MainOrderCode;
                    //or.QuantityMDH = QuantityMDH;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static List<tbl_MainOder> GetListByMainOrderCode(string MainOrderCode)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> ot = new List<tbl_MainOder>();
                ot = dbe.tbl_MainOder.Where(o => o.MainOrderCode == MainOrderCode && o.IsHidden == false).ToList();
                return ot;
            }
        }
        public static List<tbl_MainOder> GetAll()
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.IsHidden == false).OrderByDescending(o => o.ID).ToList();
                if (lo.Count > 0)
                    return lo;
                else
                    return null;
            }
        }
        public static List<tbl_MainOder> GetByRoleID(int RoleID, int StaffID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                if (RoleID != 1)
                {
                    if (RoleID == 3)
                        lo = dbe.tbl_MainOder.Where(l => l.Status >= 2 && l.Status < 5 && l.DathangID == StaffID && l.IsHidden == false).ToList();
                    else if (RoleID == 4)
                        lo = dbe.tbl_MainOder.Where(l => l.Status == 5 && (l.KhoTQID == StaffID || l.KhoTQID == 0) && l.IsHidden == false).ToList();
                    else if (RoleID == 5)
                        lo = dbe.tbl_MainOder.Where(l => l.Status == 6 && (l.KhoVNID == StaffID || l.KhoVNID == 0) && l.IsHidden == false).ToList();
                    else if (RoleID == 6)
                        lo = dbe.tbl_MainOder.Where(l => l.SalerID == StaffID && l.IsHidden == false).ToList();
                    else if (RoleID == 7)
                        lo = dbe.tbl_MainOder.Where(l => l.Status >= 7 && l.IsHidden == false).ToList();
                    else
                    {
                        lo = dbe.tbl_MainOder.Where(o => o.IsHidden == false).ToList();
                    }
                }
                return lo;
            }
        }

        public static string UpdateListMVD(int ID, string OrderTransactionCode, int QuantityMVD)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.OrderTransactionCode = OrderTransactionCode;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }


        public static List<View_OrderList> GetByUserInView(int RoleID, int StaffID, string searchtext, int Type)
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_OrderList> lo = new List<View_OrderList>();
                List<View_OrderList> losearch = new List<View_OrderList>();
                if (RoleID != 1)
                {
                    if (RoleID == 0)
                    {
                        lo = dbe.View_OrderList.Where(o => o.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 2)
                    {
                        lo = dbe.View_OrderList.Where(o => o.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 3)
                    {
                        lo = dbe.View_OrderList.Where(l => l.Status >= 2 && l.DathangID == StaffID && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 4)
                    {
                        lo = dbe.View_OrderList.Where(l => l.Status == 5 && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 5)
                    {
                        lo = dbe.View_OrderList.Where(l => l.Status >= 6 && l.Status < 7 && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 6)
                    {
                        lo = dbe.View_OrderList.Where(l => l.Status != 1 && l.SalerID == StaffID && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 8)
                    {
                        lo = dbe.View_OrderList.Where(l => l.Status >= 9 && l.Status < 10 && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else
                    {
                        lo = dbe.View_OrderList.Where(l => l.Status >= 2 && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    if (lo.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            foreach (var item in lo)
                            {
                                if (Type == 1)
                                {
                                    var pros = OrderController.GetByMainOrderID(item.ID);
                                    if (pros.Count > 0)
                                    {
                                        foreach (var p in pros)
                                        {
                                            if (p.title_origin.Contains(searchtext))
                                            {
                                                losearch.Add(item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        losearch = lo;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(item.OrderTransactionCode))
                                    {
                                        if (item.OrderTransactionCode.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode2))
                                    {
                                        if (item.OrderTransactionCode2.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode3))
                                    {
                                        if (item.OrderTransactionCode3.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode4))
                                    {
                                        if (item.OrderTransactionCode4.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode5))
                                    {
                                        if (item.OrderTransactionCode5.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            losearch = lo;
                        }
                    }
                }
                return losearch;
            }
        }
        public static List<View_OrderListFilter> GetByUserInViewFilter(int RoleID, int StaffID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_OrderListFilter> lo = new List<View_OrderListFilter>();
                List<View_OrderListFilter> losearch = new List<View_OrderListFilter>();
                if (RoleID != 1)
                {
                    if (RoleID == 0)
                    {
                        lo = dbe.View_OrderListFilter.OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 2)
                    {
                        lo = dbe.View_OrderListFilter.OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 3)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 2 && l.DathangID == StaffID).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 4)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 5 && l.Status < 7).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 5)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 5 && l.Status <= 7).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 6)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status != 1 && l.SalerID == StaffID).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 8)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 9 && l.Status < 10).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 2).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }

                }
                return lo;
            }
        }
        public static List<View_OrderListFilter> GetByUserInViewFilter2(int RoleID, int StaffID, string searchtext, int Type)
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_OrderListFilter> lo = new List<View_OrderListFilter>();
                List<View_OrderListFilter> losearch = new List<View_OrderListFilter>();
                if (RoleID != 1)
                {
                    if (RoleID == 0)
                    {
                        lo = dbe.View_OrderListFilter.OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 2)
                    {
                        lo = dbe.View_OrderListFilter.OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 3)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 2 && l.DathangID == StaffID).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 4)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 5 && l.Status < 7).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 5)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 5 && l.Status <= 7).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 6)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status != 1 && l.SalerID == StaffID).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 8)
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 9 && l.Status < 10).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else
                    {
                        lo = dbe.View_OrderListFilter.Where(l => l.Status >= 2).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    if (lo.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            foreach (var item in lo)
                            {
                                if (Type == 1)
                                {
                                    var pros = OrderController.GetByMainOrderID(item.ID);
                                    if (pros.Count > 0)
                                    {
                                        foreach (var p in pros)
                                        {
                                            if (p.title_origin.Contains(searchtext))
                                            {
                                                losearch.Add(item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        losearch = lo;
                                    }
                                }
                                else
                                {
                                    var findpackage = SmallPackageController.GetByMainOrderIDAndCode(item.ID, searchtext);
                                    if (findpackage.Count > 0)
                                    {
                                        losearch.Add(item);
                                    }
                                    //if (!string.IsNullOrEmpty(item.OrderTransactionCode))
                                    //{
                                    //    if (item.OrderTransactionCode.Contains(searchtext))
                                    //    {
                                    //        losearch.Add(item);
                                    //    }
                                    //}
                                    //else if (!string.IsNullOrEmpty(item.OrderTransactionCode2))
                                    //{
                                    //    if (item.OrderTransactionCode2.Contains(searchtext))
                                    //    {
                                    //        losearch.Add(item);
                                    //    }
                                    //}
                                    //else if (!string.IsNullOrEmpty(item.OrderTransactionCode3))
                                    //{
                                    //    if (item.OrderTransactionCode3.Contains(searchtext))
                                    //    {
                                    //        losearch.Add(item);
                                    //    }
                                    //}
                                    //else if (!string.IsNullOrEmpty(item.OrderTransactionCode4))
                                    //{
                                    //    if (item.OrderTransactionCode4.Contains(searchtext))
                                    //    {
                                    //        losearch.Add(item);
                                    //    }
                                    //}
                                    //else if (!string.IsNullOrEmpty(item.OrderTransactionCode5))
                                    //{
                                    //    if (item.OrderTransactionCode5.Contains(searchtext))
                                    //    {
                                    //        losearch.Add(item);
                                    //    }
                                    //}
                                }
                            }
                        }
                        else if (Type == 3)
                        {
                            foreach (var item in lo)
                            {
                                var findpackage = SmallPackageController.GetByMainOrderID(item.ID);
                                if (findpackage.Count == 0)
                                {
                                    losearch.Add(item);
                                }
                            }
                        }
                        else
                        {
                            losearch = lo;
                        }
                    }
                }
                return losearch;
            }
        }
        public static List<View_OrderListDamuahang> GetByUserInViewFilterStatus5()
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_OrderListDamuahang> lo = new List<View_OrderListDamuahang>();
                lo = dbe.View_OrderListDamuahang.OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                return lo;
            }
        }
        public static List<View_OrderListKhoTQ> GetByUserInViewFilterStatus6()
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_OrderListKhoTQ> lo = new List<View_OrderListKhoTQ>();
                lo = dbe.View_OrderListKhoTQ.OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                return lo;
            }
        }
        public static List<View_OrderListKhoVN> GetByUserInViewFilterStatus7()
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_OrderListKhoVN> lo = new List<View_OrderListKhoVN>();
                lo = dbe.View_OrderListKhoVN.OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                return lo;
            }
        }
        public static List<View_Orderlistwithstatus> GetByUserInViewFilterStatus(int status)
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_Orderlistwithstatus> lo = new List<View_Orderlistwithstatus>();
                lo = dbe.View_Orderlistwithstatus.Where(l => l.Status == status).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                return lo;
            }
        }
        public static List<View_OrderListFilterYCGiao> GetByUserInViewFilterYCG(int RoleID, int StaffID, string searchtext, int Type)
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_OrderListFilterYCGiao> lo = new List<View_OrderListFilterYCGiao>();
                List<View_OrderListFilterYCGiao> losearch = new List<View_OrderListFilterYCGiao>();
                if (RoleID != 1)
                {
                    if (RoleID == 0)
                    {
                        lo = dbe.View_OrderListFilterYCGiao.OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 2)
                    {
                        lo = dbe.View_OrderListFilterYCGiao.OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 3)
                    {
                        lo = dbe.View_OrderListFilterYCGiao.Where(l => l.Status >= 2 && l.DathangID == StaffID).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 4)
                    {
                        lo = dbe.View_OrderListFilterYCGiao.Where(l => l.Status == 5).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 5)
                    {
                        lo = dbe.View_OrderListFilterYCGiao.Where(l => l.Status >= 5 && l.Status <= 7).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 6)
                    {
                        lo = dbe.View_OrderListFilterYCGiao.Where(l => l.Status != 1 && l.SalerID == StaffID).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 8)
                    {
                        lo = dbe.View_OrderListFilterYCGiao.Where(l => l.Status >= 9 && l.Status < 10).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else
                    {
                        lo = dbe.View_OrderListFilterYCGiao.Where(l => l.Status >= 2).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    if (lo.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            foreach (var item in lo)
                            {
                                if (Type == 1)
                                {
                                    var pros = OrderController.GetByMainOrderID(item.ID);
                                    if (pros.Count > 0)
                                    {
                                        foreach (var p in pros)
                                        {
                                            if (p.title_origin.Contains(searchtext))
                                            {
                                                losearch.Add(item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        losearch = lo;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(item.OrderTransactionCode))
                                    {
                                        if (item.OrderTransactionCode.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode2))
                                    {
                                        if (item.OrderTransactionCode2.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode3))
                                    {
                                        if (item.OrderTransactionCode3.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode4))
                                    {
                                        if (item.OrderTransactionCode4.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode5))
                                    {
                                        if (item.OrderTransactionCode5.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            losearch = lo;
                        }
                    }
                }
                return losearch;
            }
        }
        public static List<tbl_MainOder> GetByUser(int RoleID, int StaffID, string searchtext, int Type)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                List<tbl_MainOder> losearch = new List<tbl_MainOder>();
                if (RoleID != 1)
                {
                    if (RoleID == 0)
                    {
                        lo = dbe.tbl_MainOder.Where(o => o.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 2)
                    {
                        lo = dbe.tbl_MainOder.Where(o => o.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 3)
                    {
                        lo = dbe.tbl_MainOder.Where(l => l.Status >= 2 && l.DathangID == StaffID && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 4)
                    {
                        lo = dbe.tbl_MainOder.Where(l => l.Status == 5 && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 5)
                    {
                        lo = dbe.tbl_MainOder.Where(l => l.Status >= 6 && l.Status < 7 && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 6)
                    {
                        lo = dbe.tbl_MainOder.Where(l => l.Status != 1 && l.SalerID == StaffID && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else if (RoleID == 8)
                    {
                        lo = dbe.tbl_MainOder.Where(l => l.Status >= 9 && l.Status < 10 && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    else
                    {
                        lo = dbe.tbl_MainOder.Where(l => l.Status >= 2 && l.IsHidden == false).OrderByDescending(l => l.ID).ThenByDescending(l => l.Status).ToList();
                    }
                    if (lo.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            foreach (var item in lo)
                            {
                                if (Type == 1)
                                {
                                    var pros = OrderController.GetByMainOrderID(item.ID);
                                    if (pros.Count > 0)
                                    {
                                        foreach (var p in pros)
                                        {
                                            if (p.title_origin.Contains(searchtext))
                                            {
                                                losearch.Add(item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        losearch = lo;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(item.OrderTransactionCode))
                                    {
                                        if (item.OrderTransactionCode.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode2))
                                    {
                                        if (item.OrderTransactionCode2.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode3))
                                    {
                                        if (item.OrderTransactionCode3.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode4))
                                    {
                                        if (item.OrderTransactionCode4.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(item.OrderTransactionCode5))
                                    {
                                        if (item.OrderTransactionCode5.Contains(searchtext))
                                        {
                                            losearch.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            losearch = lo;
                        }
                    }
                }
                return losearch;
            }
        }
        public static List<tbl_MainOder> GetSuccessByCustomer(int customerID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(l => l.Status == 10 && l.UID == customerID && l.IsHidden == false).ToList();
                return lo;
            }
        }
        public static List<tbl_MainOder> GetFromDateToDate(DateTime from, DateTime to)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();

                var alllist = dbe.tbl_MainOder.Where(o => o.IsHidden == false).OrderByDescending(t => t.CreatedDate).ThenBy(t => t.Status).ToList();
                if (alllist.Count > 0)
                {
                    if (!string.IsNullOrEmpty(from.ToString()) && !string.IsNullOrEmpty(to.ToString()))
                    {
                        lo = alllist.Where(t => t.CreatedDate >= from && t.CreatedDate <= to).OrderByDescending(t => t.CreatedDate).ThenBy(t => t.Status).ToList();
                    }
                    else if (!string.IsNullOrEmpty(from.ToString()) && string.IsNullOrEmpty(to.ToString()))
                    {
                        lo = alllist.Where(t => t.CreatedDate >= from).OrderByDescending(t => t.CreatedDate).ThenBy(t => t.Status).ToList();
                    }
                    else if (string.IsNullOrEmpty(from.ToString()) && !string.IsNullOrEmpty(to.ToString()))
                    {
                        lo = alllist.Where(t => t.CreatedDate <= to).OrderByDescending(t => t.CreatedDate).ThenBy(t => t.Status).ToList();
                    }
                    else
                    {
                        lo = alllist;
                    }
                }

                return lo;
            }
        }
        public static List<tbl_MainOder> GetAllByUID(int UID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.UID == UID && o.IsHidden == false).ToList();
                if (lo.Count > 0)
                    return lo;
                else
                    return null;
            }
        }
        public static List<tbl_MainOder> GetAllByUIDNotHidden(int UID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.UID == UID && o.IsHidden == false).OrderByDescending(t => t.CreatedDate).ThenBy(t => t.Status).ToList();
                if (lo.Count > 0)
                    return lo;
                else
                    return null;
            }
        }
        public static List<mainorder> GetAllByUIDNotHidden_SqlHelper(int UID, int status, string fd, string td, int OrderType)
        {
            var list = new List<mainorder>();
            var sql = @"SELECT mo.ID, mo.TotalPriceVND, mo.Deposit,mo.AmountDeposit, mo.CreatedDate, mo.Status, mo.shopname, mo.site, mo.IsGiaohang, mo.OrderType, mo.IsCheckNotiPrice, o.anhsanpham";
            sql += " FROM dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            //sql += " (SELECT MainOrderID, MIN(image_origin) AS anhsanpham FROM dbo.tbl_Order GROUP BY MainOrderID) AS o ON mo.ID = o.MainOrderID ";
            sql += " (SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER (PARTITION BY MainOrderID ORDER BY (SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1";
            sql += " where IsHidden = 0 and UID = " + UID + " AND mo.OrderType = " + OrderType + " ";

            if (status >= 0)
                sql += " AND Status = " + status;

            if (!string.IsNullOrEmpty(fd))
            {
                var df = Convert.ToDateTime(fd).Date.ToString("yyyy-MM-dd HH:mm:ss");
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = Convert.ToDateTime(td).Date.ToString("yyyy-MM-dd HH:mm:ss");
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113)";
            }
            sql += " Order By ID desc";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int i = 1;
            while (reader.Read())
            {
                var entity = new mainorder();
                entity.STT = i;
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["AmountDeposit"] != DBNull.Value)
                    entity.AmountDeposit = reader["AmountDeposit"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt();
                if (reader["shopname"] != DBNull.Value)
                    entity.ShopName = reader["shopname"].ToString();
                if (reader["site"] != DBNull.Value)
                    entity.Site = reader["site"].ToString();
                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();
                if (reader["IsGiaohang"] != DBNull.Value)
                    entity.IsGiaohang = Convert.ToBoolean(reader["IsGiaohang"].ToString());
                else
                    entity.IsGiaohang = false;
                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"].ToString());
                else
                    entity.IsCheckNotiPrice = false;
                i++;
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<mainorder> GetAllByUIDOrderCodeNotHidden_SqlHelper(int UID, int type)
        {
            var list = new List<mainorder>();
            var sql = @"SELECT mo.ID, mo.TotalPriceVND, mo.Deposit,mo.AmountDeposit, mo.CreatedDate, mo.Status, mo.shopname, mo.site, mo.IsGiaohang, mo.OrderTransactionCode,mo.OrderTransactionCode2,mo.OrderTransactionCode3,mo.OrderTransactionCode4,mo.OrderTransactionCode5, mo.OrderType, mo.IsCheckNotiPrice, o.anhsanpham, o.quantityPro";
            sql += " FROM dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            sql += " (SELECT MainOrderID, MIN(image_origin) AS anhsanpham, sum(CONVERT(float, quantity)) as quantityPro FROM dbo.tbl_Order GROUP BY MainOrderID) AS o ON mo.ID = o.MainOrderID ";
            sql += " where mo.IsHidden = 0 and UID = " + UID + " AND mo.OrderType = " + type + " ";
            //sql += " where UID = " + UID + " and IsHidden = 0 ";
            sql += " Order By ID desc";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int i = 1;
            while (reader.Read())
            {
                var entity = new mainorder();
                entity.STT = i;
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["AmountDeposit"] != DBNull.Value)
                    entity.AmountDeposit = reader["AmountDeposit"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt();
                if (reader["shopname"] != DBNull.Value)
                    entity.ShopName = reader["shopname"].ToString();
                if (reader["site"] != DBNull.Value)
                    entity.Site = reader["site"].ToString();
                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();
                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                if (reader["OrderTransactionCode2"] != DBNull.Value)
                    entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                if (reader["OrderTransactionCode3"] != DBNull.Value)
                    entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                if (reader["OrderTransactionCode4"] != DBNull.Value)
                    entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                if (reader["OrderTransactionCode5"] != DBNull.Value)
                    entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();
                if (reader["quantityPro"] != DBNull.Value)
                    entity.quantityPro = reader["quantityPro"].ToString();
                if (reader["IsGiaohang"] != DBNull.Value)
                    entity.IsGiaohang = Convert.ToBoolean(reader["IsGiaohang"].ToString());
                else
                    entity.IsGiaohang = false;
                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"].ToString());
                else
                    entity.IsCheckNotiPrice = false;
                i++;
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<OrderGetSQL> GetByUserIDInSQLHelper_WithPaging(int userID, int page, int maxrows)
        {
            var list = new List<OrderGetSQL>();
            var sql = @"SELECT  mo.ID, mo.TotalPriceVND, mo.Deposit, mo.CreatedDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, ";
            sql += " CASE mo.Status WHEN 0 THEN N'<span class=\"bg-red\">Chờ Đơn đã cọc</span>' ";
            sql += "                WHEN 1 THEN N'<span class=\"bg-black\">Hủy đơn hàng</span>' ";
            sql += "WHEN 2 THEN N'<span class=\"bg-bronze\">Khách đã Đơn đã cọc</span>' ";
            sql += "WHEN 3 THEN N'<span class=\"bg-green\">Chờ duyệt đơn</span>'";
            sql += "WHEN 4 THEN N'<span class=\"bg-green\">Đã duyệt đơn</span>' ";
            sql += "WHEN 5 THEN N'<span class=\"bg-green\">Đã mua hàng</span>' ";
            sql += "WHEN 6 THEN N'<span class=\"bg-green\">Kho Trung Quốc nhận hàng</span>' ";
            sql += "WHEN 7 THEN N'<span class=\"bg-orange\">Trên đường về Việt Nam</span>'";
            sql += "WHEN 8 THEN N'<span class=\"bg-yellow\">Chờ thanh toán</span>' ";
            sql += "WHEN 9 THEN N'<span class=\"bg-blue\">Khách đã thanh toán</span>' ";
            sql += "ELSE N'<span class=\"bg-blue\">Đã hoàn thành</span>' ";
            sql += "        END AS statusstring, mo.DathangID, ";
            sql += " mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4, ";
            sql += " mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, ";
            sql += " CASE WHEN o.anhsanpham IS NULL THEN '' ELSE '<img alt=\"\" src=\"' + o.anhsanpham + '\" width=\"100%\">' END AS anhsanpham,";
            sql += "  CASE WHEN mo.IsDoneSmallPackage = 1 THEN N'<span class=\"bg-blue\">Đã đủ</span>'  WHEN a.countrow > 0 THEN N'<span class=\"bg-yellow\">Đã nhập</span>' ELSE N'<span class=\"bg-red\">Chưa nhập</span>' END AS hasSmallpackage";
            sql += " FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN";
            sql += " dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN";
            sql += " dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN";
            sql += " dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN";
            //sql += " (SELECT MainOrderID, MIN(image_origin) AS anhsanpham FROM dbo.tbl_Order GROUP BY MainOrderID) AS o ON mo.ID = o.MainOrderID";
            sql += " (SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER (PARTITION BY MainOrderID ORDER BY (SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1";
            sql += " LEFT OUTER JOIN  (SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
            sql += "        Where mo.IsHidden = 0 and UID = " + userID + " ";
            sql += " ORDER BY mo.ID DESC OFFSET (" + page + " * " + maxrows + ") ROWS FETCH NEXT " + maxrows + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                int MainOrderID = reader["ID"].ToString().ToInt(0);
                var entity = new OrderGetSQL();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = MainOrderID;
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = reader["CreatedDate"].ToString();
                if (reader["Status"] != DBNull.Value)
                {
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                }
                if (reader["statusstring"] != DBNull.Value)
                {
                    entity.statusstring = reader["statusstring"].ToString();
                }
                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                if (reader["OrderTransactionCode2"] != DBNull.Value)
                    entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                if (reader["OrderTransactionCode3"] != DBNull.Value)
                    entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                if (reader["OrderTransactionCode4"] != DBNull.Value)
                    entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                if (reader["OrderTransactionCode5"] != DBNull.Value)
                    entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();
                if (reader["Uname"] != DBNull.Value)
                    entity.Uname = reader["Uname"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();
                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();
                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                else
                    entity.IsCheckNotiPrice = false;

                if (reader["hasSmallpackage"] != DBNull.Value)
                    entity.hasSmallpackage = reader["hasSmallpackage"].ToString();

                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<OrderGetSQL> GetByUserIDInSQLHelper_WithNoPaging(int userID)
        {
            var list = new List<OrderGetSQL>();
            var sql = @"SELECT  mo.ID, mo.TotalPriceVND, mo.Deposit, mo.CreatedDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, ";
            sql += " CASE mo.Status WHEN 0 THEN N'<span class=\"bg-red\">Chờ Đơn đã cọc</span>' ";
            sql += "                WHEN 1 THEN N'<span class=\"bg-black\">Hủy đơn hàng</span>' ";
            sql += "WHEN 2 THEN N'<span class=\"bg-bronze\">Khách đã Đơn đã cọc</span>' ";
            sql += "WHEN 3 THEN N'<span class=\"bg-green\">Chờ duyệt đơn</span>'";
            sql += "WHEN 4 THEN N'<span class=\"bg-green\">Đã duyệt đơn</span>' ";
            sql += "WHEN 5 THEN N'<span class=\"bg-green\">Đã mua hàng</span>' ";
            sql += "WHEN 6 THEN N'<span class=\"bg-green\">Kho Trung Quốc nhận hàng</span>' ";
            sql += "WHEN 7 THEN N'<span class=\"bg-orange\">Trên đường về Việt Nam</span>'";
            sql += "WHEN 8 THEN N'<span class=\"bg-yellow\">Chờ thanh toán</span>' ";
            sql += "WHEN 9 THEN N'<span class=\"bg-blue\">Khách đã thanh toán</span>' ";
            sql += "ELSE N'<span class=\"bg-blue\">Đã hoàn thành</span>' ";
            sql += "        END AS statusstring, mo.DathangID, ";
            sql += " mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4, ";
            sql += " mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, ";
            sql += " CASE WHEN o.anhsanpham IS NULL THEN '' ELSE '<img alt=\"\" src=\"' + o.anhsanpham + '\" width=\"100%\">' END AS anhsanpham,";
            sql += "  CASE WHEN mo.IsDoneSmallPackage = 1 THEN N'<span class=\"bg-blue\">Đã đủ</span>'  WHEN a.countrow > 0 THEN N'<span class=\"bg-yellow\">Đã nhập</span>' ELSE N'<span class=\"bg-red\">Chưa nhập</span>' END AS hasSmallpackage";
            sql += " FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN";
            sql += " dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN";
            sql += " dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN";
            sql += " dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN";
            //sql += " (SELECT MainOrderID, MIN(image_origin) AS anhsanpham FROM dbo.tbl_Order GROUP BY MainOrderID) AS o ON mo.ID = o.MainOrderID";
            sql += " (SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER (PARTITION BY MainOrderID ORDER BY (SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1";
            sql += " LEFT OUTER JOIN  (SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
            sql += "        Where mo.IsHidden = 0 and UID = " + userID + " ";
            sql += " ORDER BY mo.ID DESC";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                int MainOrderID = reader["ID"].ToString().ToInt(0);
                var entity = new OrderGetSQL();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = MainOrderID;
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = reader["CreatedDate"].ToString();
                if (reader["Status"] != DBNull.Value)
                {
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                }
                if (reader["statusstring"] != DBNull.Value)
                {
                    entity.statusstring = reader["statusstring"].ToString();
                }
                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                if (reader["OrderTransactionCode2"] != DBNull.Value)
                    entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                if (reader["OrderTransactionCode3"] != DBNull.Value)
                    entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                if (reader["OrderTransactionCode4"] != DBNull.Value)
                    entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                if (reader["OrderTransactionCode5"] != DBNull.Value)
                    entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();
                if (reader["Uname"] != DBNull.Value)
                    entity.Uname = reader["Uname"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();
                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();
                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                else
                    entity.IsCheckNotiPrice = false;

                if (reader["hasSmallpackage"] != DBNull.Value)
                    entity.hasSmallpackage = reader["hasSmallpackage"].ToString();

                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<OrderGetSQL> GetByUserInSQLHelper_nottextnottypeWithstatus(int RoleID, int OrderType, int StaffID, int page, int maxrows)
        {
            var list = new List<OrderGetSQL>();
            if (RoleID != 1)
            {
                var sql = @"SELECT  mo.ID, mo.TotalPriceVND, mo.Deposit, mo.CreatedDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, ";
                sql += " CASE mo.Status WHEN 0 THEN N'<span class=\"bg-red\">Chờ Đơn đã cọc</span>' ";
                sql += "                WHEN 1 THEN N'<span class=\"bg-black\">Hủy đơn hàng</span>' ";
                sql += "WHEN 2 THEN N'<span class=\"bg-bronze\">Khách đã Đơn đã cọc</span>' ";
                sql += "WHEN 3 THEN N'<span class=\"bg-green\">Chờ duyệt đơn</span>'";
                sql += "WHEN 4 THEN N'<span class=\"bg-green\">Đã duyệt đơn</span>' ";
                sql += "WHEN 5 THEN N'<span class=\"bg-green\">Đã mua hàng</span>' ";
                sql += "WHEN 6 THEN N'<span class=\"bg-green\">Kho Trung Quốc nhận hàng</span>' ";
                sql += "WHEN 7 THEN N'<span class=\"bg-orange\">Trên đường về Việt Nam</span>'";
                sql += "WHEN 8 THEN N'<span class=\"bg-yellow\">Chờ thanh toán</span>' ";
                sql += "WHEN 9 THEN N'<span class=\"bg-blue\">Khách đã thanh toán</span>' ";
                sql += "ELSE N'<span class=\"bg-blue\">Đã hoàn thành</span>' ";
                sql += "        END AS statusstring, mo.DathangID, ";
                sql += " mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4, ";
                sql += " mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, ";
                sql += " CASE WHEN o.anhsanpham IS NULL THEN '' ELSE '<img alt=\"\" src=\"' + o.anhsanpham + '\" width=\"100%\">' END AS anhsanpham,";
                sql += "  CASE WHEN mo.IsDoneSmallPackage = 1 THEN N'<span class=\"bg-blue\">Đã đủ</span>'  WHEN a.countrow > 0 THEN N'<span class=\"bg-yellow\">Đã nhập</span>' ELSE N'<span class=\"bg-red\">Chưa nhập</span>' END AS hasSmallpackage";
                sql += " FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN";
                sql += " dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN";
                sql += " dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN";
                sql += " dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN";
                //sql += " (SELECT MainOrderID, MIN(image_origin) AS anhsanpham FROM dbo.tbl_Order GROUP BY MainOrderID) AS o ON mo.ID = o.MainOrderID";
                sql += " (SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER (PARTITION BY MainOrderID ORDER BY (SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1";
                sql += " LEFT OUTER JOIN  (SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
                sql += "        Where mo.IsHidden = 0 and UID > 0 ";
                sql += "    AND mo.OrderType  = " + OrderType + "";
                if (RoleID == 3)
                {
                    sql += "    AND mo.Status >= 2 and mo.DathangID = " + StaffID + "";
                }
                else if (RoleID == 4)
                {
                    sql += "    AND mo.Status >= 5 and mo.Status < 7";
                }
                else if (RoleID == 5)
                {
                    sql += "    AND mo.Status >= 5 and mo.Status <= 7";
                }
                else if (RoleID == 6)
                {
                    sql += "    AND mo.Status != 1 and mo.SalerID = " + StaffID + "";
                }
                else if (RoleID == 8)
                {
                    sql += "    AND mo.Status >= 9 and mo.Status < 10";
                }
                else if (RoleID == 7)
                {
                    sql += "    AND mo.Status >= 2";
                }
                sql += " ORDER BY mo.ID DESC OFFSET (" + page + " * " + maxrows + ") ROWS FETCH NEXT " + maxrows + " ROWS ONLY";
                var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    int MainOrderID = reader["ID"].ToString().ToInt(0);
                    var entity = new OrderGetSQL();
                    if (reader["ID"] != DBNull.Value)
                        entity.ID = MainOrderID;
                    if (reader["TotalPriceVND"] != DBNull.Value)
                        entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                    if (reader["Deposit"] != DBNull.Value)
                        entity.Deposit = reader["Deposit"].ToString();
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.CreatedDate = reader["CreatedDate"].ToString();
                    if (reader["Status"] != DBNull.Value)
                    {
                        entity.Status = Convert.ToInt32(reader["Status"].ToString());
                    }
                    if (reader["statusstring"] != DBNull.Value)
                    {
                        entity.statusstring = reader["statusstring"].ToString();
                    }
                    if (reader["OrderTransactionCode"] != DBNull.Value)
                        entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                    if (reader["OrderTransactionCode2"] != DBNull.Value)
                        entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                    if (reader["OrderTransactionCode3"] != DBNull.Value)
                        entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                    if (reader["OrderTransactionCode4"] != DBNull.Value)
                        entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                    if (reader["OrderTransactionCode5"] != DBNull.Value)
                        entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();
                    if (reader["Uname"] != DBNull.Value)
                        entity.Uname = reader["Uname"].ToString();
                    if (reader["saler"] != DBNull.Value)
                        entity.saler = reader["saler"].ToString();
                    if (reader["dathang"] != DBNull.Value)
                        entity.dathang = reader["dathang"].ToString();
                    if (reader["anhsanpham"] != DBNull.Value)
                        entity.anhsanpham = reader["anhsanpham"].ToString();
                    if (reader["OrderType"] != DBNull.Value)
                        entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                    if (reader["IsCheckNotiPrice"] != DBNull.Value)
                        entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                    else
                        entity.IsCheckNotiPrice = false;

                    if (reader["hasSmallpackage"] != DBNull.Value)
                        entity.hasSmallpackage = reader["hasSmallpackage"].ToString();

                    list.Add(entity);
                }
                reader.Close();
            }
            return list;
        }
        public static List<mainorder> GetAllByUIDNotHidden_SqlHelper1(int UID, int status, string fd, string td)
        {
            var list = new List<mainorder>();
            var sql = @"SELECT mo.ID, mo.TotalPriceVND, mo.Deposit, mo.AmountDeposit, mo.CreatedDate, mo.Status, mo.shopname, mo.site, mo.IsGiaohang, o.anhsanpham";
            sql += " FROM dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            sql += " (SELECT MainOrderID, MIN(image_origin) AS anhsanpham FROM dbo.tbl_Order GROUP BY MainOrderID) AS o ON mo.ID = o.MainOrderID ";
            sql += " where UID = " + UID + " and IsHidden = 0 ";

            if (status >= 0)
                sql += " AND Status = " + status;

            if (!string.IsNullOrEmpty(fd))
            {
                var df = Convert.ToDateTime(fd).Date.ToString("yyyy-MM-dd HH:mm:ss");
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = Convert.ToDateTime(td).Date.ToString("yyyy-MM-dd HH:mm:ss");
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113)";
            }
            sql += " Order By ID desc";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int i = 1;
            while (reader.Read())
            {
                var entity = new mainorder();
                entity.STT = i;
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["AmountDeposit"] != DBNull.Value)
                    entity.AmountDeposit = reader["AmountDeposit"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt();
                if (reader["shopname"] != DBNull.Value)
                    entity.ShopName = reader["shopname"].ToString();
                if (reader["site"] != DBNull.Value)
                    entity.Site = reader["site"].ToString();
                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();
                if (reader["IsGiaohang"] != DBNull.Value)
                    entity.IsGiaohang = Convert.ToBoolean(reader["IsGiaohang"].ToString());
                else
                    entity.IsGiaohang = false;
                i++;
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static tbl_MainOder GetAllByUIDAndID(int UID, int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var lo = dbe.tbl_MainOder.Where(o => o.UID == UID && o.ID == ID && o.IsHidden == false).FirstOrDefault();
                if (lo != null)
                    return lo;
                else
                    return null;
            }
        }
        public static List<tbl_MainOder> GetByUIDAndStatus(int UID, int status)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.UID == UID && o.Status == status && o.IsHidden == false).ToList();
                return lo;

            }
        }

        public static List<tbl_MainOder> GetByUID(int UID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.UID == UID && o.Status > 2 && o.Status <= 9 && o.IsHidden == false).ToList();
                return lo;

            }
        }

        public static tbl_MainOder GetAllByID(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var lo = dbe.tbl_MainOder.Where(o => o.ID == ID && o.IsHidden == false).FirstOrDefault();
                if (lo != null)
                    return lo;
                else
                    return null;
            }
        }
        public static int getOrderByRoleIDStaffID_SQL(int RoleID, int StaffID)
        {
            int Count = 0;
            var sql = @"SELECT COUNT(*) as Total from tbl_MainOder as mo";
            sql += "        Where IsHidden = 0 and UID > 0";
            if (RoleID == 3)
            {
                sql += "    AND mo.Status >= 2 and mo.DathangID = " + StaffID + "";
            }
            else if (RoleID == 4)
            {
                sql += "    AND mo.Status >= 5 and mo.Status < 7";
            }
            else if (RoleID == 5)
            {
                sql += "    AND mo.Status >= 5 and mo.Status <= 7";
            }
            else if (RoleID == 6)
            {
                sql += "    AND mo.Status != 1 and mo.SalerID = " + StaffID + "";
            }
            else if (RoleID == 8)
            {
                sql += "    AND mo.Status >= 9 and mo.Status < 10";
            }
            else if (RoleID == 7)
            {
                sql += "    AND mo.Status >= 2";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }
        public static int getOrderByUID_SQL(int UID)
        {
            int Count = 0;
            var sql = @"SELECT COUNT(*) as Total from tbl_MainOder as mo";
            sql += "        Where IsHidden = 0 and UID = " + UID + "";

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }
        public static List<MainOrderID> GetMainOrderIDBySearch(string search)
        {
            List<MainOrderID> ods = new List<MainOrderID>();
            var sql = @"Select MainOrderID from tbl_order where IsHidden = 0 and title_origin like N'%" + search + "%' GROUP BY MainorderID";

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                MainOrderID os = new MainOrderID();
                if (reader["MainOrderID"] != DBNull.Value)
                    os.ID = reader["MainOrderID"].ToString().ToInt(0);
                ods.Add(os);
            }
            reader.Close();
            return ods;
        }
        public static List<MainOrderID> GetSmallPackageMainOrderIDBySearch(string search)
        {
            List<MainOrderID> ods = new List<MainOrderID>();
            var sql = @"Select MainOrderID from tbl_SmallPackage where IsHidden = 0 and OrderTransactionCode like N'%" + search + "%' GROUP BY MainorderID";

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                MainOrderID os = new MainOrderID();
                if (reader["MainOrderID"] != DBNull.Value)
                    os.ID = reader["MainOrderID"].ToString().ToInt(0);
                ods.Add(os);
            }
            reader.Close();
            return ods;
        }
        public class MainOrderID
        {
            public int ID { get; set; }
        }
        public class mainorder
        {
            public int ID { get; set; }
            public int STT { get; set; }
            public string TotalPriceVND { get; set; }
            public string PriceVND { get; set; }
            public string Deposit { get; set; }
            public string CurrentCNYVN { get; set; }
            public string AmountDeposit { get; set; }
            public DateTime CreatedDate { get; set; }
            public int Status { get; set; }
            public string ShopName { get; set; }
            public string Site { get; set; }
            public string anhsanpham { get; set; }
            public bool IsGiaohang { get; set; }
            public bool IsCheckNotiPrice { get; set; }
            public int OrderType { get; set; }
            public string OrderTransactionCode { get; set; }
            public string OrderTransactionCode2 { get; set; }
            public string OrderTransactionCode3 { get; set; }
            public string OrderTransactionCode4 { get; set; }
            public string OrderTransactionCode5 { get; set; }
            public string quantityPro { get; set; }

            public string Created { get; set; }
            public string DepostiDate { get; set; }
            public string DateBuy { get; set; }
            public string DateTQ { get; set; }
            public string DateShipper { get; set; }
            public string DateVN { get; set; }
            public string DatePay { get; set; }
            public string CompleteDate { get; set; }
            public string DateBuyOK { get; set; }
            public string DateToVN { get; set; }
            public string DateToShip { get; set; }
            public string DateToCancel { get; set; }
            public string FeeInWareHouse { get; set; }

            public int SalerID { get; set; }
            public int DathangID { get; set; }
            public int TotalLink { get; set; }
        }
        public class OrderGetSQL
        {
            public int ID { get; set; }
            public int STT { get; set; }
            public int totalrow { get; set; }
            public string MainOrderCode { get; set; }
            public string TotalPriceReal { get; set; }
            public string anhsanpham { get; set; }
            public string ShopID { get; set; }
            public string ShopName { get; set; }
            public string HoaHongVND { get; set; }
            public string HoaHongCYN { get; set; }
            public string Site { get; set; }
            public string StaffNote { get; set; }
            public string FeeShipCN { get; set; }
            public string TotalPriceVND { get; set; }
            public string PriceVND { get; set; }
            public string Deposit { get; set; }
            public int MaDonTruoc { get; set; }
            public int UID { get; set; }
            public int Status { get; set; }
            public string CreatedDate { get; set; }
            public string statusstring { get; set; }
            public int OrderType { get; set; }
            public bool IsCheckNotiPrice { get; set; }
            public bool OrderDone { get; set; }
            public string OrderTransactionCode { get; set; }
            public string OrderTransactionCode2 { get; set; }
            public string OrderTransactionCode3 { get; set; }
            public string OrderTransactionCode4 { get; set; }
            public string OrderTransactionCode5 { get; set; }

            public string Uname { get; set; }
            public string dathang { get; set; }
            public string saler { get; set; }
            public string khotq { get; set; }
            public string khovn { get; set; }
            public string hasSmallpackage { get; set; }
            public bool IsDoneSmallPackage { get; set; }

            public string Currency { get; set; }

            public List<string> listMainOrderCode { get; set; }

            public string Created { get; set; }
            public string DepostiDate { get; set; }
            public string DateBuy { get; set; }
            public string DateTQ { get; set; }
            public string Cancel { get; set; }
            public string DateShipper { get; set; }
            public string DateVN { get; set; }
            public string DateBuyOK { get; set; }
            public string DateToVN { get; set; }
            public string DateToShip { get; set; }
            public string DateToCancel { get; set; }
            public string DatePay { get; set; }
            public string CompleteDate { get; set; }
            public string CSKHNAME { get; set; }
            public int CSID { get; set; }
            public int SalerID { get; set; }
            public int DathangID { get; set; }
        }
        #endregion

        public static string UpdateBrand(int ID, string StaffNote)
        {
            using (var dbe = new NHSTEntities())
            {
                var or = dbe.tbl_MainOder.Where(o => o.ID == ID).FirstOrDefault();
                if (or != null)
                {
                    or.BankPayment = StaffNote;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    dbe.SaveChanges();
                    return "ok";
                }
                else
                    return null;
            }
        }

        public static List<tbl_MainOder> GetAllByOrderType(int OrderType)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.OrderType == OrderType && o.IsHidden != true).OrderByDescending(o => o.ID).ToList();
                return lo;
            }
        }

        public static List<tbl_MainOder> GetAllByOrderType_SaleID(int OrderType, int SaleID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.OrderType == OrderType && o.IsHidden == false && o.SalerID == SaleID).OrderByDescending(o => o.ID).ToList();
                return lo;
            }
        }

        public static List<tbl_MainOder> GetAllByOrderTypeCSKHID(int OrderType, int CSID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.OrderType == OrderType && o.IsHidden == false && o.CSID == CSID).OrderByDescending(o => o.ID).ToList();
                return lo;
            }
        }

        public static List<tbl_MainOder> GetTop1ByUID(int UID)
        {
            var list = new List<tbl_MainOder>();
            var sql = @"select TOP(1) * ";
            sql += " from tbl_MainOder";
            sql += " where IsHidden = 0 and Status != 1 and UID = " + UID + "";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new tbl_MainOder();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static tbl_MainOder GetByID(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var lo = dbe.tbl_MainOder.Where(x => x.ID == ID && x.IsHidden == false).FirstOrDefault();
                if (lo != null)
                    return lo;
                return null;
            }
        }
        public static List<tbl_MainOder> GetByStatus(int status)
        {
            using (var dbe = new NHSTEntities())
            {
                var lo = dbe.tbl_MainOder.Where(x => x.Status == status && x.IsHidden == false).ToList();
                if (lo != null)
                    return lo;
                return null;
            }
        }
        #region New
        public static List<tbl_MainOder> GetAllByUIDAndOrderType(int UID, int OrderType)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.UID == UID && o.OrderType == OrderType && o.IsHidden != true).ToList();
                return lo;
            }
        }

        public static List<mainorder> GetAllByUIDNotHidden_SqlHelperNew(int UID, string search, int typesearch, int status, string fd, string td, int OrderType, int page)
        {
            var list = new List<mainorder>();
            var sql = @"SELECT mo.ID, mo.TotalPriceVND,mo.PriceVND,mo.DateShipper,mo.CurrentCNYVN, mo.Deposit,mo.AmountDeposit, mo.FeeInWareHouse, mo.CreatedDate, mo.DepostiDate, mo.DateBuy, mo.DateTQ, mo.DateVN,mo.PayDate,mo.CompleteDate, mo.Status, mo.shopname, mo.site, mo.IsGiaohang, mo.OrderType, mo.IsCheckNotiPrice, o.anhsanpham, TotalLink, mo.DateBuyOK, mo.DateToVN, mo.DateToCancel, mo.DateToShip";
            sql += " FROM dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            //sql += " (SELECT MainOrderID, MIN(image_origin) AS anhsanpham FROM dbo.tbl_Order GROUP BY MainOrderID) AS o ON mo.ID = o.MainOrderID ";
            sql += " (SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER (PARTITION BY MainOrderID ORDER BY (SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1";
            sql += "LEFT OUTER JOIN(SELECT count(*) AS TotalLink, MainOrderID  FROM tbl_Order AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
            sql += " where mo.IsHidden = 0 and UID = " + UID + " AND mo.OrderType = " + OrderType + " ";

            if (status >= 0)
                sql += " AND Status = " + status;

            if (typesearch != 0)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    if (typesearch == 1)
                    {
                        sql += " AND ID=" + search + "";
                    }
                    else if (typesearch == 3)
                    {
                        sql += " AND site like N'%" + search + "%'";
                    }
                }

            }

            if (!string.IsNullOrEmpty(fd))
            {
                //var df = Convert.ToDateTime(fd).Date.ToString("dd-MM-yyyy HH:mm:ss");
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                //var dt = Convert.ToDateTime(td).Date.ToString("dd-MM-yyyy HH:mm:ss");
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113)";
            }
            sql += " Order By ID desc OFFSET (" + page + "*15) ROWS FETCH NEXT 15 ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int i = 1;
            while (reader.Read())
            {
                if (typesearch != 0)
                {
                    if (typesearch == 2)
                    {
                        int mainorderID = 0;
                        if (reader["ID"] != DBNull.Value)
                            mainorderID = reader["ID"].ToString().ToInt(0);
                        var orders = OrderController.GetByMainOrderIDAndBrand(mainorderID, search);
                        if (orders.Count > 0)
                        {
                            int Stt = 0;
                            var entity = new mainorder();
                            entity.STT = i;
                            if (reader["ID"] != DBNull.Value)
                                entity.ID = reader["ID"].ToString().ToInt(0);
                            if (reader["TotalLink"] != DBNull.Value)
                                entity.TotalLink = reader["TotalLink"].ToString().ToInt(0);
                            if (reader["TotalPriceVND"] != DBNull.Value)
                                entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                            if (reader["PriceVND"] != DBNull.Value)
                                entity.PriceVND = reader["PriceVND"].ToString();
                            if (reader["Deposit"] != DBNull.Value)
                                entity.Deposit = reader["Deposit"].ToString();
                            if (reader["AmountDeposit"] != DBNull.Value)
                                entity.AmountDeposit = reader["AmountDeposit"].ToString();

                            if (reader["CurrentCNYVN"] != DBNull.Value)
                                entity.CurrentCNYVN = reader["CurrentCNYVN"].ToString();

                            if (reader["CreatedDate"] != DBNull.Value)
                                entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                            if (reader["Status"] != DBNull.Value)
                            {
                                entity.Status = reader["Status"].ToString().ToInt();
                                Stt = Convert.ToInt32(reader["Status"].ToString());
                            }
                            if (reader["shopname"] != DBNull.Value)
                                entity.ShopName = reader["shopname"].ToString();
                            if (reader["site"] != DBNull.Value)
                                entity.Site = reader["site"].ToString();
                            if (reader["anhsanpham"] != DBNull.Value)
                                entity.anhsanpham = reader["anhsanpham"].ToString();
                            if (reader["IsGiaohang"] != DBNull.Value)
                                entity.IsGiaohang = Convert.ToBoolean(reader["IsGiaohang"].ToString());
                            else
                                entity.IsGiaohang = false;
                            if (reader["OrderType"] != DBNull.Value)
                                entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                            if (reader["IsCheckNotiPrice"] != DBNull.Value)
                                entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"].ToString());
                            else
                                entity.IsCheckNotiPrice = false;

                            if (Stt == 0)
                            {
                                if (reader["CreatedDate"] != DBNull.Value)
                                    entity.Created = "<p class=\"s-txt no-wrap red-text\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["CreatedDate"] != DBNull.Value)
                                    entity.Created = "<p class=\"s-txt no-wrap\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                            }
                            if (Stt == 2)
                            {
                                if (reader["DepostiDate"] != DBNull.Value)
                                    entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DepostiDate"] != DBNull.Value)
                                    entity.DepostiDate = "<p class=\"s-txt no-wrap\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                            }
                            if (Stt == 4)
                            {
                                if (reader["DateBuy"] != DBNull.Value)
                                    entity.DateBuy = "<p class=\"s-txt no-wrap red-text\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateBuy"] != DBNull.Value)
                                    entity.DateBuy = "<p class=\"s-txt no-wrap\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                            }
                            if (Stt == 5)
                            {
                                if (reader["DateBuyOK"] != DBNull.Value)
                                    entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateBuyOK"] != DBNull.Value)
                                    entity.DateBuyOK = "<p class=\"s-txt no-wrap\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                            }
                            if (Stt == 3)
                            {
                                if (reader["DateShipper"] != DBNull.Value)
                                    entity.DateShipper = "<p class=\"s-txt no-wrap red-text\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateShipper"] != DBNull.Value)
                                    entity.DateShipper = "<p class=\"s-txt no-wrap\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                            }
                            if (Stt == 6)
                            {
                                if (reader["DateTQ"] != DBNull.Value)
                                    entity.DateTQ = "<p class=\"s-txt no-wrap red-text\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateTQ"] != DBNull.Value)
                                    entity.DateTQ = "<p class=\"s-txt no-wrap\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                            }
                            if (Stt == 7)
                            {
                                if (reader["DateToVN"] != DBNull.Value)
                                    entity.DateToVN = "<p class=\"s-txt no-wrap red-text\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateToVN"] != DBNull.Value)
                                    entity.DateToVN = "<p class=\"s-txt no-wrap\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                            }
                            if (Stt == 8)
                            {
                                if (reader["DateVN"] != DBNull.Value)
                                    entity.DateVN = "<p class=\"s-txt no-wrap red-text\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateVN"] != DBNull.Value)
                                    entity.DateVN = "<p class=\"s-txt no-wrap\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                            }
                            if (Stt == 11)
                            {
                                if (reader["DateToShip"] != DBNull.Value)
                                    entity.DateToShip = "<p class=\"s-txt no-wrap red-text\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateToShip"] != DBNull.Value)
                                    entity.DateToShip = "<p class=\"s-txt no-wrap\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                            }
                            if (Stt == 9)
                            {
                                if (reader["PayDate"] != DBNull.Value)
                                    entity.DatePay = "<p class=\"s-txt no-wrap red-text\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["PayDate"] != DBNull.Value)
                                    entity.DatePay = "<p class=\"s-txt no-wrap\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                            }
                            if (Stt == 10)
                            {
                                if (reader["CompleteDate"] != DBNull.Value)
                                    entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["CompleteDate"] != DBNull.Value)
                                    entity.CompleteDate = "<p class=\"s-txt no-wrap\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                            }
                            if (Stt == 12)
                            {
                                if (reader["DateToCancel"] != DBNull.Value)
                                    entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateToCancel"] != DBNull.Value)
                                    entity.DateToCancel = "<p class=\"s-txt no-wrap\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                            }

                            if (reader["FeeInWareHouse"] != DBNull.Value)
                                entity.FeeInWareHouse = reader["FeeInWareHouse"].ToString();

                            i++;
                            list.Add(entity);
                        }
                    }
                    else
                    {
                        int Stt = 0;
                        var entity = new mainorder();
                        entity.STT = i;
                        if (reader["ID"] != DBNull.Value)
                            entity.ID = reader["ID"].ToString().ToInt(0);
                        if (reader["TotalLink"] != DBNull.Value)
                            entity.TotalLink = reader["TotalLink"].ToString().ToInt(0);
                        if (reader["TotalPriceVND"] != DBNull.Value)
                            entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                        if (reader["PriceVND"] != DBNull.Value)
                            entity.PriceVND = reader["PriceVND"].ToString();
                        if (reader["Deposit"] != DBNull.Value)
                            entity.Deposit = reader["Deposit"].ToString();
                        if (reader["CurrentCNYVN"] != DBNull.Value)
                            entity.CurrentCNYVN = reader["CurrentCNYVN"].ToString();
                        if (reader["AmountDeposit"] != DBNull.Value)
                            entity.AmountDeposit = reader["AmountDeposit"].ToString();
                        if (reader["CreatedDate"] != DBNull.Value)
                            entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                        if (reader["Status"] != DBNull.Value)
                        {
                            entity.Status = reader["Status"].ToString().ToInt();
                            Stt = Convert.ToInt32(reader["Status"].ToString());
                        }
                        if (reader["shopname"] != DBNull.Value)
                            entity.ShopName = reader["shopname"].ToString();
                        if (reader["site"] != DBNull.Value)
                            entity.Site = reader["site"].ToString();
                        if (reader["anhsanpham"] != DBNull.Value)
                            entity.anhsanpham = reader["anhsanpham"].ToString();
                        if (reader["IsGiaohang"] != DBNull.Value)
                            entity.IsGiaohang = Convert.ToBoolean(reader["IsGiaohang"].ToString());
                        else
                            entity.IsGiaohang = false;
                        if (reader["OrderType"] != DBNull.Value)
                            entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                        if (reader["IsCheckNotiPrice"] != DBNull.Value)
                            entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"].ToString());
                        else
                            entity.IsCheckNotiPrice = false;

                        if (Stt == 0)
                        {
                            if (reader["CreatedDate"] != DBNull.Value)
                                entity.Created = "<p class=\"s-txt no-wrap red-text\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["CreatedDate"] != DBNull.Value)
                                entity.Created = "<p class=\"s-txt no-wrap\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                        }
                        if (Stt == 2)
                        {
                            if (reader["DepostiDate"] != DBNull.Value)
                                entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DepostiDate"] != DBNull.Value)
                                entity.DepostiDate = "<p class=\"s-txt no-wrap\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                        }
                        if (Stt == 4)
                        {
                            if (reader["DateBuy"] != DBNull.Value)
                                entity.DateBuy = "<p class=\"s-txt no-wrap red-text\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateBuy"] != DBNull.Value)
                                entity.DateBuy = "<p class=\"s-txt no-wrap\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                        }
                        if (Stt == 5)
                        {
                            if (reader["DateBuyOK"] != DBNull.Value)
                                entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateBuyOK"] != DBNull.Value)
                                entity.DateBuyOK = "<p class=\"s-txt no-wrap\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                        }
                        if (Stt == 3)
                        {
                            if (reader["DateShipper"] != DBNull.Value)
                                entity.DateShipper = "<p class=\"s-txt no-wrap red-text\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateShipper"] != DBNull.Value)
                                entity.DateShipper = "<p class=\"s-txt no-wrap\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                        }
                        if (Stt == 6)
                        {
                            if (reader["DateTQ"] != DBNull.Value)
                                entity.DateTQ = "<p class=\"s-txt no-wrap red-text\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateTQ"] != DBNull.Value)
                                entity.DateTQ = "<p class=\"s-txt no-wrap\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                        }
                        if (Stt == 7)
                        {
                            if (reader["DateToVN"] != DBNull.Value)
                                entity.DateToVN = "<p class=\"s-txt no-wrap red-text\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateToVN"] != DBNull.Value)
                                entity.DateToVN = "<p class=\"s-txt no-wrap\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                        }
                        if (Stt == 8)
                        {
                            if (reader["DateVN"] != DBNull.Value)
                                entity.DateVN = "<p class=\"s-txt no-wrap red-text\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateVN"] != DBNull.Value)
                                entity.DateVN = "<p class=\"s-txt no-wrap\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                        }
                        if (Stt == 11)
                        {
                            if (reader["DateToShip"] != DBNull.Value)
                                entity.DateToShip = "<p class=\"s-txt no-wrap red-text\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateToShip"] != DBNull.Value)
                                entity.DateToShip = "<p class=\"s-txt no-wrap\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                        }
                        if (Stt == 9)
                        {
                            if (reader["PayDate"] != DBNull.Value)
                                entity.DatePay = "<p class=\"s-txt no-wrap red-text\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["PayDate"] != DBNull.Value)
                                entity.DatePay = "<p class=\"s-txt no-wrap\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                        }
                        if (Stt == 10)
                        {
                            if (reader["CompleteDate"] != DBNull.Value)
                                entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["CompleteDate"] != DBNull.Value)
                                entity.CompleteDate = "<p class=\"s-txt no-wrap\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                        }
                        if (Stt == 12)
                        {
                            if (reader["DateToCancel"] != DBNull.Value)
                                entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateToCancel"] != DBNull.Value)
                                entity.DateToCancel = "<p class=\"s-txt no-wrap\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                        }

                        if (reader["FeeInWareHouse"] != DBNull.Value)
                            entity.FeeInWareHouse = reader["FeeInWareHouse"].ToString();

                        i++;
                        list.Add(entity);
                    }
                }
                else
                {
                    int Stt = 0;
                    var entity = new mainorder();
                    entity.STT = i;
                    if (reader["ID"] != DBNull.Value)
                        entity.ID = reader["ID"].ToString().ToInt(0);
                    if (reader["TotalLink"] != DBNull.Value)
                        entity.TotalLink = reader["TotalLink"].ToString().ToInt(0);
                    if (reader["TotalPriceVND"] != DBNull.Value)
                        entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                    if (reader["PriceVND"] != DBNull.Value)
                        entity.PriceVND = reader["PriceVND"].ToString();
                    if (reader["Deposit"] != DBNull.Value)
                        entity.Deposit = reader["Deposit"].ToString();
                    if (reader["AmountDeposit"] != DBNull.Value)
                        entity.AmountDeposit = reader["AmountDeposit"].ToString();
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    if (reader["Status"] != DBNull.Value)
                    {
                        entity.Status = reader["Status"].ToString().ToInt();
                        Stt = Convert.ToInt32(reader["Status"].ToString());
                    }
                    if (reader["CurrentCNYVN"] != DBNull.Value)
                        entity.CurrentCNYVN = reader["CurrentCNYVN"].ToString();
                    if (reader["shopname"] != DBNull.Value)
                        entity.ShopName = reader["shopname"].ToString();
                    if (reader["site"] != DBNull.Value)
                        entity.Site = reader["site"].ToString();
                    if (reader["anhsanpham"] != DBNull.Value)
                        entity.anhsanpham = reader["anhsanpham"].ToString();
                    if (reader["IsGiaohang"] != DBNull.Value)
                        entity.IsGiaohang = Convert.ToBoolean(reader["IsGiaohang"].ToString());
                    else
                        entity.IsGiaohang = false;
                    if (reader["OrderType"] != DBNull.Value)
                        entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                    if (reader["IsCheckNotiPrice"] != DBNull.Value)
                        entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"].ToString());
                    else
                        entity.IsCheckNotiPrice = false;

                    if (Stt == 0)
                    {
                        if (reader["CreatedDate"] != DBNull.Value)
                            entity.Created = "<p class=\"s-txt no-wrap red-text\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["CreatedDate"] != DBNull.Value)
                            entity.Created = "<p class=\"s-txt no-wrap\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                    }
                    if (Stt == 2)
                    {
                        if (reader["DepostiDate"] != DBNull.Value)
                            entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DepostiDate"] != DBNull.Value)
                            entity.DepostiDate = "<p class=\"s-txt no-wrap\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                    }
                    if (Stt == 4)
                    {
                        if (reader["DateBuy"] != DBNull.Value)
                            entity.DateBuy = "<p class=\"s-txt no-wrap red-text\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateBuy"] != DBNull.Value)
                            entity.DateBuy = "<p class=\"s-txt no-wrap\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                    }
                    if (Stt == 5)
                    {
                        if (reader["DateBuyOK"] != DBNull.Value)
                            entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateBuyOK"] != DBNull.Value)
                            entity.DateBuyOK = "<p class=\"s-txt no-wrap\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                    }
                    if (Stt == 3)
                    {
                        if (reader["DateShipper"] != DBNull.Value)
                            entity.DateShipper = "<p class=\"s-txt no-wrap red-text\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateShipper"] != DBNull.Value)
                            entity.DateShipper = "<p class=\"s-txt no-wrap\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                    }
                    if (Stt == 6)
                    {
                        if (reader["DateTQ"] != DBNull.Value)
                            entity.DateTQ = "<p class=\"s-txt no-wrap red-text\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateTQ"] != DBNull.Value)
                            entity.DateTQ = "<p class=\"s-txt no-wrap\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                    }
                    if (Stt == 7)
                    {
                        if (reader["DateToVN"] != DBNull.Value)
                            entity.DateToVN = "<p class=\"s-txt no-wrap red-text\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateToVN"] != DBNull.Value)
                            entity.DateToVN = "<p class=\"s-txt no-wrap\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                    }
                    if (Stt == 8)
                    {
                        if (reader["DateVN"] != DBNull.Value)
                            entity.DateVN = "<p class=\"s-txt no-wrap red-text\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateVN"] != DBNull.Value)
                            entity.DateVN = "<p class=\"s-txt no-wrap\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                    }
                    if (Stt == 11)
                    {
                        if (reader["DateToShip"] != DBNull.Value)
                            entity.DateToShip = "<p class=\"s-txt no-wrap red-text\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateToShip"] != DBNull.Value)
                            entity.DateToShip = "<p class=\"s-txt no-wrap\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                    }
                    if (Stt == 9)
                    {
                        if (reader["PayDate"] != DBNull.Value)
                            entity.DatePay = "<p class=\"s-txt no-wrap red-text\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["PayDate"] != DBNull.Value)
                            entity.DatePay = "<p class=\"s-txt no-wrap\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                    }
                    if (Stt == 10)
                    {
                        if (reader["CompleteDate"] != DBNull.Value)
                            entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["CompleteDate"] != DBNull.Value)
                            entity.CompleteDate = "<p class=\"s-txt no-wrap\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                    }
                    if (Stt == 12)
                    {
                        if (reader["DateToCancel"] != DBNull.Value)
                            entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateToCancel"] != DBNull.Value)
                            entity.DateToCancel = "<p class=\"s-txt no-wrap\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                    }

                    if (reader["FeeInWareHouse"] != DBNull.Value)
                        entity.FeeInWareHouse = reader["FeeInWareHouse"].ToString();

                    i++;
                    list.Add(entity);
                }

            }
            reader.Close();
            return list;
        }



        public static List<mainorder> GetAllByUIDNotHidden_SqlHelperNew_Excel(int UID, string search, int typesearch, int status, string fd, string td, int OrderType)
        {
            var list = new List<mainorder>();
            var sql = @"SELECT mo.ID, mo.TotalPriceVND,mo.PriceVND,mo.DateShipper, mo.Deposit,mo.AmountDeposit, mo.FeeInWareHouse, mo.CreatedDate, mo.DepostiDate, mo.DateBuy, mo.DateTQ, mo.DateVN,mo.PayDate,mo.CompleteDate, mo.Status, mo.shopname, mo.site, mo.IsGiaohang, mo.OrderType, mo.IsCheckNotiPrice, o.anhsanpham, TotalLink, mo.DateBuyOK, mo.DateToVN, mo.DateToCancel, mo.DateToShip";
            sql += " FROM dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            //sql += " (SELECT MainOrderID, MIN(image_origin) AS anhsanpham FROM dbo.tbl_Order GROUP BY MainOrderID) AS o ON mo.ID = o.MainOrderID ";
            sql += " (SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER (PARTITION BY MainOrderID ORDER BY (SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1";
            sql += "LEFT OUTER JOIN(SELECT count(*) AS TotalLink, MainOrderID  FROM tbl_Order AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
            sql += " where mo.IsHidden = 0 and UID = " + UID + " AND mo.OrderType = " + OrderType + " ";

            if (status >= 0)
                sql += " AND Status = " + status;

            if (typesearch != 0)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    if (typesearch == 1)
                    {
                        sql += " AND ID=" + search + "";
                    }
                    else if (typesearch == 3)
                    {
                        sql += " AND site like N'%" + search + "%'";
                    }
                }

            }

            if (!string.IsNullOrEmpty(fd))
            {
                //var df = Convert.ToDateTime(fd).Date.ToString("dd-MM-yyyy HH:mm:ss");
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                //var dt = Convert.ToDateTime(td).Date.ToString("dd-MM-yyyy HH:mm:ss");
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113)";
            }
            sql += " Order By ID desc ";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int i = 1;
            while (reader.Read())
            {
                if (typesearch != 0)
                {
                    if (typesearch == 2)
                    {
                        int mainorderID = 0;
                        if (reader["ID"] != DBNull.Value)
                            mainorderID = reader["ID"].ToString().ToInt(0);
                        var orders = OrderController.GetByMainOrderIDAndBrand(mainorderID, search);
                        if (orders.Count > 0)
                        {
                            int Stt = 0;
                            var entity = new mainorder();
                            entity.STT = i;
                            if (reader["ID"] != DBNull.Value)
                                entity.ID = reader["ID"].ToString().ToInt(0);
                            if (reader["TotalLink"] != DBNull.Value)
                                entity.TotalLink = reader["TotalLink"].ToString().ToInt(0);
                            if (reader["TotalPriceVND"] != DBNull.Value)
                                entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                            if (reader["PriceVND"] != DBNull.Value)
                                entity.PriceVND = reader["PriceVND"].ToString();
                            if (reader["Deposit"] != DBNull.Value)
                                entity.Deposit = reader["Deposit"].ToString();
                            if (reader["AmountDeposit"] != DBNull.Value)
                                entity.AmountDeposit = reader["AmountDeposit"].ToString();
                            if (reader["CreatedDate"] != DBNull.Value)
                                entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                            if (reader["Status"] != DBNull.Value)
                            {
                                entity.Status = reader["Status"].ToString().ToInt();
                                Stt = Convert.ToInt32(reader["Status"].ToString());
                            }
                            if (reader["shopname"] != DBNull.Value)
                                entity.ShopName = reader["shopname"].ToString();
                            if (reader["site"] != DBNull.Value)
                                entity.Site = reader["site"].ToString();
                            if (reader["anhsanpham"] != DBNull.Value)
                                entity.anhsanpham = reader["anhsanpham"].ToString();
                            if (reader["IsGiaohang"] != DBNull.Value)
                                entity.IsGiaohang = Convert.ToBoolean(reader["IsGiaohang"].ToString());
                            else
                                entity.IsGiaohang = false;
                            if (reader["OrderType"] != DBNull.Value)
                                entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                            if (reader["IsCheckNotiPrice"] != DBNull.Value)
                                entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"].ToString());
                            else
                                entity.IsCheckNotiPrice = false;

                            if (Stt == 0)
                            {
                                if (reader["CreatedDate"] != DBNull.Value)
                                    entity.Created = "<p class=\"s-txt no-wrap red-text\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["CreatedDate"] != DBNull.Value)
                                    entity.Created = "<p class=\"s-txt no-wrap\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                            }
                            if (Stt == 2)
                            {
                                if (reader["DepostiDate"] != DBNull.Value)
                                    entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DepostiDate"] != DBNull.Value)
                                    entity.DepostiDate = "<p class=\"s-txt no-wrap\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                            }
                            if (Stt == 4)
                            {
                                if (reader["DateBuy"] != DBNull.Value)
                                    entity.DateBuy = "<p class=\"s-txt no-wrap red-text\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateBuy"] != DBNull.Value)
                                    entity.DateBuy = "<p class=\"s-txt no-wrap\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                            }
                            if (Stt == 5)
                            {
                                if (reader["DateBuyOK"] != DBNull.Value)
                                    entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateBuyOK"] != DBNull.Value)
                                    entity.DateBuyOK = "<p class=\"s-txt no-wrap\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                            }
                            if (Stt == 3)
                            {
                                if (reader["DateShipper"] != DBNull.Value)
                                    entity.DateShipper = "<p class=\"s-txt no-wrap red-text\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateShipper"] != DBNull.Value)
                                    entity.DateShipper = "<p class=\"s-txt no-wrap\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                            }
                            if (Stt == 6)
                            {
                                if (reader["DateTQ"] != DBNull.Value)
                                    entity.DateTQ = "<p class=\"s-txt no-wrap red-text\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateTQ"] != DBNull.Value)
                                    entity.DateTQ = "<p class=\"s-txt no-wrap\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                            }
                            if (Stt == 7)
                            {
                                if (reader["DateToVN"] != DBNull.Value)
                                    entity.DateToVN = "<p class=\"s-txt no-wrap red-text\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateToVN"] != DBNull.Value)
                                    entity.DateToVN = "<p class=\"s-txt no-wrap\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                            }
                            if (Stt == 8)
                            {
                                if (reader["DateVN"] != DBNull.Value)
                                    entity.DateVN = "<p class=\"s-txt no-wrap red-text\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateVN"] != DBNull.Value)
                                    entity.DateVN = "<p class=\"s-txt no-wrap\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                            }
                            if (Stt == 11)
                            {
                                if (reader["DateToShip"] != DBNull.Value)
                                    entity.DateToShip = "<p class=\"s-txt no-wrap red-text\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateToShip"] != DBNull.Value)
                                    entity.DateToShip = "<p class=\"s-txt no-wrap\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                            }
                            if (Stt == 9)
                            {
                                if (reader["PayDate"] != DBNull.Value)
                                    entity.DatePay = "<p class=\"s-txt no-wrap red-text\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["PayDate"] != DBNull.Value)
                                    entity.DatePay = "<p class=\"s-txt no-wrap\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                            }
                            if (Stt == 10)
                            {
                                if (reader["CompleteDate"] != DBNull.Value)
                                    entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["CompleteDate"] != DBNull.Value)
                                    entity.CompleteDate = "<p class=\"s-txt no-wrap\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                            }
                            if (Stt == 12)
                            {
                                if (reader["DateToCancel"] != DBNull.Value)
                                    entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                            }
                            else
                            {
                                if (reader["DateToCancel"] != DBNull.Value)
                                    entity.DateToCancel = "<p class=\"s-txt no-wrap\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                            }

                            if (reader["FeeInWareHouse"] != DBNull.Value)
                                entity.FeeInWareHouse = reader["FeeInWareHouse"].ToString();

                            i++;
                            list.Add(entity);
                        }
                    }
                    else
                    {
                        int Stt = 0;
                        var entity = new mainorder();
                        entity.STT = i;
                        if (reader["ID"] != DBNull.Value)
                            entity.ID = reader["ID"].ToString().ToInt(0);
                        if (reader["TotalLink"] != DBNull.Value)
                            entity.TotalLink = reader["TotalLink"].ToString().ToInt(0);
                        if (reader["TotalPriceVND"] != DBNull.Value)
                            entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                        if (reader["PriceVND"] != DBNull.Value)
                            entity.PriceVND = reader["PriceVND"].ToString();
                        if (reader["Deposit"] != DBNull.Value)
                            entity.Deposit = reader["Deposit"].ToString();
                        if (reader["AmountDeposit"] != DBNull.Value)
                            entity.AmountDeposit = reader["AmountDeposit"].ToString();
                        if (reader["CreatedDate"] != DBNull.Value)
                            entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                        if (reader["Status"] != DBNull.Value)
                        {
                            entity.Status = reader["Status"].ToString().ToInt();
                            Stt = Convert.ToInt32(reader["Status"].ToString());
                        }
                        if (reader["shopname"] != DBNull.Value)
                            entity.ShopName = reader["shopname"].ToString();
                        if (reader["site"] != DBNull.Value)
                            entity.Site = reader["site"].ToString();
                        if (reader["anhsanpham"] != DBNull.Value)
                            entity.anhsanpham = reader["anhsanpham"].ToString();
                        if (reader["IsGiaohang"] != DBNull.Value)
                            entity.IsGiaohang = Convert.ToBoolean(reader["IsGiaohang"].ToString());
                        else
                            entity.IsGiaohang = false;
                        if (reader["OrderType"] != DBNull.Value)
                            entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                        if (reader["IsCheckNotiPrice"] != DBNull.Value)
                            entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"].ToString());
                        else
                            entity.IsCheckNotiPrice = false;

                        if (Stt == 0)
                        {
                            if (reader["CreatedDate"] != DBNull.Value)
                                entity.Created = "<p class=\"s-txt no-wrap red-text\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["CreatedDate"] != DBNull.Value)
                                entity.Created = "<p class=\"s-txt no-wrap\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                        }
                        if (Stt == 2)
                        {
                            if (reader["DepostiDate"] != DBNull.Value)
                                entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DepostiDate"] != DBNull.Value)
                                entity.DepostiDate = "<p class=\"s-txt no-wrap\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                        }
                        if (Stt == 4)
                        {
                            if (reader["DateBuy"] != DBNull.Value)
                                entity.DateBuy = "<p class=\"s-txt no-wrap red-text\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateBuy"] != DBNull.Value)
                                entity.DateBuy = "<p class=\"s-txt no-wrap\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                        }
                        if (Stt == 5)
                        {
                            if (reader["DateBuyOK"] != DBNull.Value)
                                entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateBuyOK"] != DBNull.Value)
                                entity.DateBuyOK = "<p class=\"s-txt no-wrap\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                        }
                        if (Stt == 3)
                        {
                            if (reader["DateShipper"] != DBNull.Value)
                                entity.DateShipper = "<p class=\"s-txt no-wrap red-text\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateShipper"] != DBNull.Value)
                                entity.DateShipper = "<p class=\"s-txt no-wrap\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                        }
                        if (Stt == 6)
                        {
                            if (reader["DateTQ"] != DBNull.Value)
                                entity.DateTQ = "<p class=\"s-txt no-wrap red-text\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateTQ"] != DBNull.Value)
                                entity.DateTQ = "<p class=\"s-txt no-wrap\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                        }
                        if (Stt == 7)
                        {
                            if (reader["DateToVN"] != DBNull.Value)
                                entity.DateToVN = "<p class=\"s-txt no-wrap red-text\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateToVN"] != DBNull.Value)
                                entity.DateToVN = "<p class=\"s-txt no-wrap\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                        }
                        if (Stt == 8)
                        {
                            if (reader["DateVN"] != DBNull.Value)
                                entity.DateVN = "<p class=\"s-txt no-wrap red-text\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateVN"] != DBNull.Value)
                                entity.DateVN = "<p class=\"s-txt no-wrap\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                        }
                        if (Stt == 11)
                        {
                            if (reader["DateToShip"] != DBNull.Value)
                                entity.DateToShip = "<p class=\"s-txt no-wrap red-text\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateToShip"] != DBNull.Value)
                                entity.DateToShip = "<p class=\"s-txt no-wrap\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                        }
                        if (Stt == 9)
                        {
                            if (reader["PayDate"] != DBNull.Value)
                                entity.DatePay = "<p class=\"s-txt no-wrap red-text\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["PayDate"] != DBNull.Value)
                                entity.DatePay = "<p class=\"s-txt no-wrap\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                        }
                        if (Stt == 10)
                        {
                            if (reader["CompleteDate"] != DBNull.Value)
                                entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["CompleteDate"] != DBNull.Value)
                                entity.CompleteDate = "<p class=\"s-txt no-wrap\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                        }
                        if (Stt == 12)
                        {
                            if (reader["DateToCancel"] != DBNull.Value)
                                entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                        }
                        else
                        {
                            if (reader["DateToCancel"] != DBNull.Value)
                                entity.DateToCancel = "<p class=\"s-txt no-wrap\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                        }

                        if (reader["FeeInWareHouse"] != DBNull.Value)
                            entity.FeeInWareHouse = reader["FeeInWareHouse"].ToString();

                        i++;
                        list.Add(entity);
                    }
                }
                else
                {
                    int Stt = 0;
                    var entity = new mainorder();
                    entity.STT = i;
                    if (reader["ID"] != DBNull.Value)
                        entity.ID = reader["ID"].ToString().ToInt(0);
                    if (reader["TotalLink"] != DBNull.Value)
                        entity.TotalLink = reader["TotalLink"].ToString().ToInt(0);
                    if (reader["TotalPriceVND"] != DBNull.Value)
                        entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                    if (reader["PriceVND"] != DBNull.Value)
                        entity.PriceVND = reader["PriceVND"].ToString();
                    if (reader["Deposit"] != DBNull.Value)
                        entity.Deposit = reader["Deposit"].ToString();
                    if (reader["AmountDeposit"] != DBNull.Value)
                        entity.AmountDeposit = reader["AmountDeposit"].ToString();
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    if (reader["Status"] != DBNull.Value)
                    {
                        entity.Status = reader["Status"].ToString().ToInt();
                        Stt = Convert.ToInt32(reader["Status"].ToString());
                    }
                    if (reader["shopname"] != DBNull.Value)
                        entity.ShopName = reader["shopname"].ToString();
                    if (reader["site"] != DBNull.Value)
                        entity.Site = reader["site"].ToString();
                    if (reader["anhsanpham"] != DBNull.Value)
                        entity.anhsanpham = reader["anhsanpham"].ToString();
                    if (reader["IsGiaohang"] != DBNull.Value)
                        entity.IsGiaohang = Convert.ToBoolean(reader["IsGiaohang"].ToString());
                    else
                        entity.IsGiaohang = false;
                    if (reader["OrderType"] != DBNull.Value)
                        entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                    if (reader["IsCheckNotiPrice"] != DBNull.Value)
                        entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"].ToString());
                    else
                        entity.IsCheckNotiPrice = false;

                    if (Stt == 0)
                    {
                        if (reader["CreatedDate"] != DBNull.Value)
                            entity.Created = "<p class=\"s-txt no-wrap red-text\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["CreatedDate"] != DBNull.Value)
                            entity.Created = "<p class=\"s-txt no-wrap\">Đơn mới: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " </p>";

                    }
                    if (Stt == 2)
                    {
                        if (reader["DepostiDate"] != DBNull.Value)
                            entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DepostiDate"] != DBNull.Value)
                            entity.DepostiDate = "<p class=\"s-txt no-wrap\">Đơn đã cọc: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " </p>";

                    }
                    if (Stt == 4)
                    {
                        if (reader["DateBuy"] != DBNull.Value)
                            entity.DateBuy = "<p class=\"s-txt no-wrap red-text\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateBuy"] != DBNull.Value)
                            entity.DateBuy = "<p class=\"s-txt no-wrap\">Đơn chờ mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " </p>";

                    }
                    if (Stt == 5)
                    {
                        if (reader["DateBuyOK"] != DBNull.Value)
                            entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateBuyOK"] != DBNull.Value)
                            entity.DateBuyOK = "<p class=\"s-txt no-wrap\">Đơn đã mua hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " </p>";

                    }
                    if (Stt == 3)
                    {
                        if (reader["DateShipper"] != DBNull.Value)
                            entity.DateShipper = "<p class=\"s-txt no-wrap red-text\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateShipper"] != DBNull.Value)
                            entity.DateShipper = "<p class=\"s-txt no-wrap\">Đơn người bán giao: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " </p>";

                    }
                    if (Stt == 6)
                    {
                        if (reader["DateTQ"] != DBNull.Value)
                            entity.DateTQ = "<p class=\"s-txt no-wrap red-text\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateTQ"] != DBNull.Value)
                            entity.DateTQ = "<p class=\"s-txt no-wrap\">Kho Trung Quốc nhận hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " </p>";

                    }
                    if (Stt == 7)
                    {
                        if (reader["DateToVN"] != DBNull.Value)
                            entity.DateToVN = "<p class=\"s-txt no-wrap red-text\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateToVN"] != DBNull.Value)
                            entity.DateToVN = "<p class=\"s-txt no-wrap\">Trên đường về Việt Nam: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " </p>";

                    }
                    if (Stt == 8)
                    {
                        if (reader["DateVN"] != DBNull.Value)
                            entity.DateVN = "<p class=\"s-txt no-wrap red-text\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateVN"] != DBNull.Value)
                            entity.DateVN = "<p class=\"s-txt no-wrap\">Trong kho Hà Nội: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + " </p>";

                    }
                    if (Stt == 11)
                    {
                        if (reader["DateToShip"] != DBNull.Value)
                            entity.DateToShip = "<p class=\"s-txt no-wrap red-text\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateToShip"] != DBNull.Value)
                            entity.DateToShip = "<p class=\"s-txt no-wrap\">Đang giao hàng: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " </p>";

                    }
                    if (Stt == 9)
                    {
                        if (reader["PayDate"] != DBNull.Value)
                            entity.DatePay = "<p class=\"s-txt no-wrap red-text\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["PayDate"] != DBNull.Value)
                            entity.DatePay = "<p class=\"s-txt no-wrap\">Đã thanh toán: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + " </p>";

                    }
                    if (Stt == 10)
                    {
                        if (reader["CompleteDate"] != DBNull.Value)
                            entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["CompleteDate"] != DBNull.Value)
                            entity.CompleteDate = "<p class=\"s-txt no-wrap\">Đã hoàn thành: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " </p>";

                    }
                    if (Stt == 12)
                    {
                        if (reader["DateToCancel"] != DBNull.Value)
                            entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                    }
                    else
                    {
                        if (reader["DateToCancel"] != DBNull.Value)
                            entity.DateToCancel = "<p class=\"s-txt no-wrap\">Đơn khiếu nại: " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " </p>";

                    }

                    if (reader["FeeInWareHouse"] != DBNull.Value)
                        entity.FeeInWareHouse = reader["FeeInWareHouse"].ToString();

                    i++;
                    list.Add(entity);
                }

            }
            reader.Close();
            return list;
        }

        public static double GetTotalPriceSale(int UID, string PriceType, string fd, string td)
        {
            var sql = @"select total=SUM(CAST(" + PriceType + " as float)) ";
            sql += " from tbl_MainOder ";
            sql += " WHERE SalerID = '" + UID + "' AND Status > 4 ";
            if (!string.IsNullOrEmpty(fd))
            {
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + fd + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + td + "',113)";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            double total = 0;
            while (reader.Read())
            {
                if (reader["total"] != DBNull.Value)
                    total = Convert.ToDouble(reader["total"].ToString());
            }
            reader.Close();
            return total;
        }

        public static double GetTotalPriceSaleCustom(int UID, string PriceType, int Type)
        {
            var sql = @"select total=SUM(CAST(" + PriceType + " as float)) ";
            sql += " from tbl_MainOder as mo ";
            sql += " WHERE SalerID = '" + UID + "' AND Status > 4 ";
            if (Type == 1)
            {
                sql += " AND mo.UID IN (select ID from tbl_Account WHERE TypePerson = 1)";
            }
            else if (Type == 2)
            {
                sql += " AND mo.UID IN (select ID from tbl_Account WHERE TypePerson = 2)";
            }
            else if (Type == 3)
            {
                sql += " AND mo.UID IN (select ID from tbl_Account WHERE TypePerson = 3)";
            }            
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            double total = 0;
            while (reader.Read())
            {
                if (reader["total"] != DBNull.Value)
                    total = Convert.ToDouble(reader["total"].ToString());
            }
            reader.Close();
            return total;
        }

        public static int GetTotalOrderAllSale(int UID, string fd, string td)
        {
            int Count = 0;
            var sql = @" SELECT COUNT(*) as Total from tbl_MainOder ";
            sql += " WHERE SalerID = '" + UID + "' AND Status > 1 ";
            if (!string.IsNullOrEmpty(fd))
            {
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + fd + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + td + "',113)";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }

        public static int GetTotalOrderAllSaleStatus0(int UID, string fd, string td)
        {
            int Count = 0;
            var sql = @" SELECT COUNT(*) as Total from tbl_MainOder ";
            sql += " WHERE SalerID = '" + UID + "' AND Status = 0 ";
            if (!string.IsNullOrEmpty(fd))
            {
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + fd + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + td + "',113)";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }

        public static int GetTotalOrderSaleStatus0(int OrderType, int UID, string fd, string td)
        {
            int Count = 0;
            var sql = @" SELECT COUNT(*) as Total from tbl_MainOder ";
            sql += " WHERE OrderType = '" + OrderType + "' AND SalerID = '" + UID + "' AND Status = 0 ";
            if (!string.IsNullOrEmpty(fd))
            {
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + fd + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + td + "',113)";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }

        public static int GetTotalOrderSale(int OrderType, int UID, string fd, string td)
        {
            int Count = 0;
            var sql = @" SELECT COUNT(*) as Total from tbl_MainOder ";
                sql += " WHERE OrderType = '" + OrderType + "' AND SalerID = '" + UID + "' AND Status > 1 ";
            if (!string.IsNullOrEmpty(fd))
            {                               
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + fd + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {           
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + td + "',113)";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }

        public static int GetTotalItem(int UID, int status, string fd, string td, int OrderType)
        {
            var sql = @"select Count(*) as Total from tbl_MainOder";
            sql += " where IsHidden = 0 and UID = " + UID + " And OrderType= " + OrderType + " ";

            if (status >= 0)
                sql += " AND Status = " + status;

            if (!string.IsNullOrEmpty(fd))
            {
                //var df = Convert.ToDateTime(fd).Date.ToString("yyyy-MM-dd HH:mm:ss");
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                //var dt = Convert.ToDateTime(td).Date.ToString("yyyy-MM-dd HH:mm:ss");
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113)";
            }

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int total = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    total = reader["Total"].ToString().ToInt();
            }
            return total;
        }


        public static List<tbl_MainOder> GetByCustomerAndStatus(int customerID, int status)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(l => l.Status == status && l.UID == customerID && l.IsHidden == false).ToList();
                return lo;
            }
        }


        public static List<OrderGetSQL> GetByUserInSQLHelperWithFilter(int RoleID, int OrderType, int StaffID,
            string searchtext, int Type, string fd, string td, double priceFrom, double priceTo,
            bool isNotCode)
        {
            var list = new List<OrderGetSQL>();
            if (RoleID != 1)
            {
                var sql = @"SELECT  mo.ID, mo.TotalPriceVND, mo.Deposit, mo.CreatedDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, ";
                sql += " CASE mo.Status WHEN 0 THEN N'<span class=\"bg-red\">Chờ Đơn đã cọc</span>' ";
                sql += "                WHEN 1 THEN N'<span class=\"bg-black\">Hủy đơn hàng</span>' ";
                sql += "WHEN 2 THEN N'<span class=\"bg-bronze\">Khách đã Đơn đã cọc</span>' ";
                sql += "WHEN 3 THEN N'<span class=\"bg-green\">Chờ duyệt đơn</span>'";
                sql += "WHEN 4 THEN N'<span class=\"bg-green\">Đã duyệt đơn</span>' ";
                sql += "WHEN 5 THEN N'<span class=\"bg-green\">Đã mua hàng</span>' ";
                sql += "WHEN 6 THEN N'<span class=\"bg-green\">Kho Trung Quốc nhận hàng</span>' ";
                sql += "WHEN 7 THEN N'<span class=\"bg-orange\">Trên đường về Việt Nam</span>'";
                sql += "WHEN 8 THEN N'<span class=\"bg-yellow\">Chờ thanh toán</span>' ";
                sql += "WHEN 9 THEN N'<span class=\"bg-blue\">Khách đã thanh toán</span>' ";
                sql += "ELSE N'<span class=\"bg-blue\">Đã hoàn thành</span>' ";
                sql += "        END AS statusstring, mo.DathangID, ";
                sql += " mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4, ";
                sql += " mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, sm.totalSmallPackages, sm1.totalSmallPackagesWithSearchText, ofi.totalOrderSearch, ";
                sql += " CASE WHEN o.anhsanpham IS NULL THEN '' ELSE '<img alt=\"\" src=\"' + o.anhsanpham + '\" width=\"100%\">' END AS anhsanpham";

                sql += " FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN";
                sql += " dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN";
                sql += " dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN";
                sql += " dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN";
                sql += " (SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackagesWithSearchText FROM tbl_smallpackage where OrderTransactionCode like N'%" + searchtext + "%') sm1 ON sm1.MainOrderID = mo.ID and totalSmallPackagesWithSearchText = 1 LEFT OUTER JOIN";
                sql += " (SELECT MainOrderID, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalOrderSearch FROM tbl_Order where title_origin like N'%" + searchtext + "%') ofi ON ofi.MainOrderID = mo.ID and totalOrderSearch = 1 LEFT OUTER JOIN";
                sql += " (SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage) sm ON sm.MainOrderID = mo.ID and totalSmallPackages=1 LEFT OUTER JOIN";
                sql += " (SELECT image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER (PARTITION BY MainOrderID ORDER BY (SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1";

                sql += "        Where mo.IsHidden = 0 and UID > 0 ";
                sql += "    AND mo.OrderType  = " + OrderType + "";
                if (!string.IsNullOrEmpty(searchtext))
                {
                    if (Type == 3)
                    {
                        sql += "  AND mo.Mainordercode like N'%" + searchtext + "%'";
                    }
                }
                if (RoleID == 3)
                {
                    sql += "    AND mo.Status >= 2 and mo.DathangID = " + StaffID + "";
                }
                else if (RoleID == 4)
                {
                    sql += "    AND mo.Status >= 5 and mo.Status < 7";
                }
                else if (RoleID == 5)
                {
                    sql += "    AND mo.Status >= 5 and mo.Status <= 7";
                }
                else if (RoleID == 6)
                {
                    sql += "    AND mo.Status != 1 and mo.SalerID = " + StaffID + "";
                }
                else if (RoleID == 8)
                {
                    sql += "    AND mo.Status >= 9 and mo.Status < 10";
                }
                else if (RoleID == 7)
                {
                    sql += "    AND mo.Status >= 2";
                }
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = Convert.ToDateTime(fd).Date.ToString("yyyy-MM-dd HH:mm:ss");
                    sql += " AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113)";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = Convert.ToDateTime(td).Date.ToString("yyyy-MM-dd HH:mm:ss");
                    sql += " AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113)";
                }
                if (priceFrom > 0)
                {
                    sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
                }
                if (priceTo > 0)
                {
                    sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
                }
                if (isNotCode == true)
                {
                    sql += " AND totalSmallPackages is null";
                }
                sql += " ORDER BY mo.ID DESC";
                //sql += " ORDER BY mo.ID DESC OFFSET (" + page + " * " + maxrows + ") ROWS FETCH NEXT " + maxrows + " ROWS ONLY";
                var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(searchtext))
                    {
                        int totalOrderSearch = 0;
                        if (reader["totalOrderSearch"] != DBNull.Value)
                            totalOrderSearch = reader["totalOrderSearch"].ToString().ToInt(0);

                        int totalSmallPackagesWithSearchText = 0;
                        if (reader["totalSmallPackagesWithSearchText"] != DBNull.Value)
                            totalSmallPackagesWithSearchText = reader["totalSmallPackagesWithSearchText"].ToString().ToInt(0);

                        if (Type == 1)
                        {
                            if (totalOrderSearch > 0)
                            {
                                int MainOrderID = reader["ID"].ToString().ToInt(0);
                                var entity = new OrderGetSQL();
                                if (reader["ID"] != DBNull.Value)
                                    entity.ID = MainOrderID;
                                if (reader["TotalPriceVND"] != DBNull.Value)
                                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                                if (reader["Deposit"] != DBNull.Value)
                                    entity.Deposit = reader["Deposit"].ToString();
                                if (reader["CreatedDate"] != DBNull.Value)
                                    entity.CreatedDate = reader["CreatedDate"].ToString();
                                if (reader["Status"] != DBNull.Value)
                                {
                                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                                }
                                if (reader["statusstring"] != DBNull.Value)
                                {
                                    entity.statusstring = reader["statusstring"].ToString();
                                }
                                if (reader["OrderTransactionCode"] != DBNull.Value)
                                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                                if (reader["OrderTransactionCode2"] != DBNull.Value)
                                    entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                                if (reader["OrderTransactionCode3"] != DBNull.Value)
                                    entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                                if (reader["OrderTransactionCode4"] != DBNull.Value)
                                    entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                                if (reader["OrderTransactionCode5"] != DBNull.Value)
                                    entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();

                                if (reader["Uname"] != DBNull.Value)
                                    entity.Uname = reader["Uname"].ToString();
                                if (reader["saler"] != DBNull.Value)
                                    entity.saler = reader["saler"].ToString();
                                if (reader["dathang"] != DBNull.Value)
                                    entity.dathang = reader["dathang"].ToString();
                                if (reader["anhsanpham"] != DBNull.Value)
                                    entity.anhsanpham = reader["anhsanpham"].ToString();
                                if (reader["OrderType"] != DBNull.Value)
                                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);

                                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                                else
                                    entity.IsCheckNotiPrice = false;
                                list.Add(entity);
                            }
                        }
                        else if (Type == 2)
                        {
                            if (totalSmallPackagesWithSearchText > 0)
                            {
                                int MainOrderID = reader["ID"].ToString().ToInt(0);
                                var entity = new OrderGetSQL();
                                if (reader["ID"] != DBNull.Value)
                                    entity.ID = MainOrderID;
                                if (reader["TotalPriceVND"] != DBNull.Value)
                                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                                if (reader["Deposit"] != DBNull.Value)
                                    entity.Deposit = reader["Deposit"].ToString();
                                if (reader["CreatedDate"] != DBNull.Value)
                                    entity.CreatedDate = reader["CreatedDate"].ToString();
                                if (reader["Status"] != DBNull.Value)
                                {
                                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                                }
                                if (reader["statusstring"] != DBNull.Value)
                                {
                                    entity.statusstring = reader["statusstring"].ToString();
                                }
                                if (reader["OrderTransactionCode"] != DBNull.Value)
                                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                                if (reader["OrderTransactionCode2"] != DBNull.Value)
                                    entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                                if (reader["OrderTransactionCode3"] != DBNull.Value)
                                    entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                                if (reader["OrderTransactionCode4"] != DBNull.Value)
                                    entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                                if (reader["OrderTransactionCode5"] != DBNull.Value)
                                    entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();

                                if (reader["Uname"] != DBNull.Value)
                                    entity.Uname = reader["Uname"].ToString();
                                if (reader["saler"] != DBNull.Value)
                                    entity.saler = reader["saler"].ToString();
                                if (reader["dathang"] != DBNull.Value)
                                    entity.dathang = reader["dathang"].ToString();
                                if (reader["anhsanpham"] != DBNull.Value)
                                    entity.anhsanpham = reader["anhsanpham"].ToString();
                                if (reader["OrderType"] != DBNull.Value)
                                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);

                                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                                else
                                    entity.IsCheckNotiPrice = false;
                                list.Add(entity);
                            }
                        }
                        else
                        {
                            int MainOrderID = reader["ID"].ToString().ToInt(0);
                            var entity = new OrderGetSQL();
                            if (reader["ID"] != DBNull.Value)
                                entity.ID = MainOrderID;
                            if (reader["TotalPriceVND"] != DBNull.Value)
                                entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                            if (reader["Deposit"] != DBNull.Value)
                                entity.Deposit = reader["Deposit"].ToString();
                            if (reader["CreatedDate"] != DBNull.Value)
                                entity.CreatedDate = reader["CreatedDate"].ToString();
                            if (reader["Status"] != DBNull.Value)
                            {
                                entity.Status = Convert.ToInt32(reader["Status"].ToString());
                            }
                            if (reader["statusstring"] != DBNull.Value)
                            {
                                entity.statusstring = reader["statusstring"].ToString();
                            }
                            if (reader["OrderTransactionCode"] != DBNull.Value)
                                entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                            if (reader["OrderTransactionCode2"] != DBNull.Value)
                                entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                            if (reader["OrderTransactionCode3"] != DBNull.Value)
                                entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                            if (reader["OrderTransactionCode4"] != DBNull.Value)
                                entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                            if (reader["OrderTransactionCode5"] != DBNull.Value)
                                entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();

                            if (reader["Uname"] != DBNull.Value)
                                entity.Uname = reader["Uname"].ToString();
                            if (reader["saler"] != DBNull.Value)
                                entity.saler = reader["saler"].ToString();
                            if (reader["dathang"] != DBNull.Value)
                                entity.dathang = reader["dathang"].ToString();
                            if (reader["anhsanpham"] != DBNull.Value)
                                entity.anhsanpham = reader["anhsanpham"].ToString();
                            if (reader["OrderType"] != DBNull.Value)
                                entity.OrderType = reader["OrderType"].ToString().ToInt(1);

                            if (reader["IsCheckNotiPrice"] != DBNull.Value)
                                entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                            else
                                entity.IsCheckNotiPrice = false;
                            list.Add(entity);
                        }
                    }
                    else
                    {
                        int MainOrderID = reader["ID"].ToString().ToInt(0);
                        var entity = new OrderGetSQL();
                        if (reader["ID"] != DBNull.Value)
                            entity.ID = MainOrderID;
                        if (reader["TotalPriceVND"] != DBNull.Value)
                            entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                        if (reader["Deposit"] != DBNull.Value)
                            entity.Deposit = reader["Deposit"].ToString();
                        if (reader["CreatedDate"] != DBNull.Value)
                            entity.CreatedDate = reader["CreatedDate"].ToString();
                        if (reader["Status"] != DBNull.Value)
                        {
                            entity.Status = Convert.ToInt32(reader["Status"].ToString());
                        }
                        if (reader["statusstring"] != DBNull.Value)
                        {
                            entity.statusstring = reader["statusstring"].ToString();
                        }
                        if (reader["OrderTransactionCode"] != DBNull.Value)
                            entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                        if (reader["OrderTransactionCode2"] != DBNull.Value)
                            entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                        if (reader["OrderTransactionCode3"] != DBNull.Value)
                            entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                        if (reader["OrderTransactionCode4"] != DBNull.Value)
                            entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                        if (reader["OrderTransactionCode5"] != DBNull.Value)
                            entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();

                        if (reader["Uname"] != DBNull.Value)
                            entity.Uname = reader["Uname"].ToString();
                        if (reader["saler"] != DBNull.Value)
                            entity.saler = reader["saler"].ToString();
                        if (reader["dathang"] != DBNull.Value)
                            entity.dathang = reader["dathang"].ToString();
                        if (reader["anhsanpham"] != DBNull.Value)
                            entity.anhsanpham = reader["anhsanpham"].ToString();
                        if (reader["OrderType"] != DBNull.Value)
                            entity.OrderType = reader["OrderType"].ToString().ToInt(1);

                        if (reader["IsCheckNotiPrice"] != DBNull.Value)
                            entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                        else
                            entity.IsCheckNotiPrice = false;
                        list.Add(entity);
                    }
                }
                reader.Close();
            }
            return list;
        }
        public static List<OrderGetSQL> GetByUserIDInSQLHelperWithFilter(int UID,
            string searchtext, int Type, string fd, string td, double priceFrom, double priceTo,
            bool isNotCode)
        {
            var list = new List<OrderGetSQL>();
            var sql = @"SELECT  mo.ID, mo.TotalPriceVND, mo.Deposit, mo.CreatedDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, ";
            sql += " CASE mo.Status WHEN 0 THEN N'<span class=\"bg-red\">Chờ Đơn đã cọc</span>' ";
            sql += "                WHEN 1 THEN N'<span class=\"bg-black\">Hủy đơn hàng</span>' ";
            sql += "WHEN 2 THEN N'<span class=\"bg-bronze\">Khách đã Đơn đã cọc</span>' ";
            sql += "WHEN 3 THEN N'<span class=\"bg-green\">Chờ duyệt đơn</span>'";
            sql += "WHEN 4 THEN N'<span class=\"bg-green\">Đã duyệt đơn</span>' ";
            sql += "WHEN 5 THEN N'<span class=\"bg-green\">Đã mua hàng</span>' ";
            sql += "WHEN 6 THEN N'<span class=\"bg-green\">Kho Trung Quốc nhận hàng</span>' ";
            sql += "WHEN 7 THEN N'<span class=\"bg-orange\">Trên đường về Việt Nam</span>'";
            sql += "WHEN 8 THEN N'<span class=\"bg-yellow\">Chờ thanh toán</span>' ";
            sql += "WHEN 9 THEN N'<span class=\"bg-blue\">Khách đã thanh toán</span>' ";
            sql += "ELSE N'<span class=\"bg-blue\">Đã hoàn thành</span>' ";
            sql += "        END AS statusstring, mo.DathangID, ";
            sql += " mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4, ";
            sql += " mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, sm.totalSmallPackages, sm1.totalSmallPackagesWithSearchText, ofi.totalOrderSearch, ";
            sql += " CASE WHEN o.anhsanpham IS NULL THEN '' ELSE '<img alt=\"\" src=\"' + o.anhsanpham + '\" width=\"100%\">' END AS anhsanpham";

            sql += " FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN";
            sql += " dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN";
            sql += " dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN";
            sql += " dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN";
            sql += " (SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackagesWithSearchText FROM tbl_smallpackage where OrderTransactionCode like N'%" + searchtext + "%') sm1 ON sm1.MainOrderID = mo.ID and totalSmallPackagesWithSearchText = 1 LEFT OUTER JOIN";
            sql += " (SELECT MainOrderID, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalOrderSearch FROM tbl_Order where title_origin like N'%" + searchtext + "%') ofi ON ofi.MainOrderID = mo.ID and totalOrderSearch = 1 LEFT OUTER JOIN";
            sql += " (SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage) sm ON sm.MainOrderID = mo.ID and totalSmallPackages=1 LEFT OUTER JOIN";
            sql += " (SELECT image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER (PARTITION BY MainOrderID ORDER BY (SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1";

            sql += "        Where mo.IsHidden = 0 and UID = " + UID + " ";
            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 3)
                {
                    sql += "  AND mo.Mainordercode like N'%" + searchtext + "%'";
                }
            }

            if (!string.IsNullOrEmpty(fd))
            {
                var df = Convert.ToDateTime(fd).Date.ToString("yyyy-MM-dd HH:mm:ss");
                sql += " AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113)";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = Convert.ToDateTime(td).Date.ToString("yyyy-MM-dd HH:mm:ss");
                sql += " AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113)";
            }
            if (priceFrom > 0)
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            }
            if (priceTo > 0)
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            }
            if (isNotCode == true)
            {
                sql += " AND totalSmallPackages is null";
            }
            sql += " ORDER BY mo.ID DESC";
            //sql += " ORDER BY mo.ID DESC OFFSET (" + page + " * " + maxrows + ") ROWS FETCH NEXT " + maxrows + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (!string.IsNullOrEmpty(searchtext))
                {
                    int totalOrderSearch = 0;
                    if (reader["totalOrderSearch"] != DBNull.Value)
                        totalOrderSearch = reader["totalOrderSearch"].ToString().ToInt(0);

                    int totalSmallPackagesWithSearchText = 0;
                    if (reader["totalSmallPackagesWithSearchText"] != DBNull.Value)
                        totalSmallPackagesWithSearchText = reader["totalSmallPackagesWithSearchText"].ToString().ToInt(0);

                    if (Type == 1)
                    {
                        if (totalOrderSearch > 0)
                        {
                            int MainOrderID = reader["ID"].ToString().ToInt(0);
                            var entity = new OrderGetSQL();
                            if (reader["ID"] != DBNull.Value)
                                entity.ID = MainOrderID;
                            if (reader["TotalPriceVND"] != DBNull.Value)
                                entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                            if (reader["Deposit"] != DBNull.Value)
                                entity.Deposit = reader["Deposit"].ToString();
                            if (reader["CreatedDate"] != DBNull.Value)
                                entity.CreatedDate = reader["CreatedDate"].ToString();
                            if (reader["Status"] != DBNull.Value)
                            {
                                entity.Status = Convert.ToInt32(reader["Status"].ToString());
                            }
                            if (reader["statusstring"] != DBNull.Value)
                            {
                                entity.statusstring = reader["statusstring"].ToString();
                            }
                            if (reader["OrderTransactionCode"] != DBNull.Value)
                                entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                            if (reader["OrderTransactionCode2"] != DBNull.Value)
                                entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                            if (reader["OrderTransactionCode3"] != DBNull.Value)
                                entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                            if (reader["OrderTransactionCode4"] != DBNull.Value)
                                entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                            if (reader["OrderTransactionCode5"] != DBNull.Value)
                                entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();

                            if (reader["Uname"] != DBNull.Value)
                                entity.Uname = reader["Uname"].ToString();
                            if (reader["saler"] != DBNull.Value)
                                entity.saler = reader["saler"].ToString();
                            if (reader["dathang"] != DBNull.Value)
                                entity.dathang = reader["dathang"].ToString();
                            if (reader["anhsanpham"] != DBNull.Value)
                                entity.anhsanpham = reader["anhsanpham"].ToString();
                            if (reader["OrderType"] != DBNull.Value)
                                entity.OrderType = reader["OrderType"].ToString().ToInt(1);

                            if (reader["IsCheckNotiPrice"] != DBNull.Value)
                                entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                            else
                                entity.IsCheckNotiPrice = false;
                            list.Add(entity);
                        }
                    }
                    else if (Type == 2)
                    {
                        if (totalSmallPackagesWithSearchText > 0)
                        {
                            int MainOrderID = reader["ID"].ToString().ToInt(0);
                            var entity = new OrderGetSQL();
                            if (reader["ID"] != DBNull.Value)
                                entity.ID = MainOrderID;
                            if (reader["TotalPriceVND"] != DBNull.Value)
                                entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                            if (reader["Deposit"] != DBNull.Value)
                                entity.Deposit = reader["Deposit"].ToString();
                            if (reader["CreatedDate"] != DBNull.Value)
                                entity.CreatedDate = reader["CreatedDate"].ToString();
                            if (reader["Status"] != DBNull.Value)
                            {
                                entity.Status = Convert.ToInt32(reader["Status"].ToString());
                            }
                            if (reader["statusstring"] != DBNull.Value)
                            {
                                entity.statusstring = reader["statusstring"].ToString();
                            }
                            if (reader["OrderTransactionCode"] != DBNull.Value)
                                entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                            if (reader["OrderTransactionCode2"] != DBNull.Value)
                                entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                            if (reader["OrderTransactionCode3"] != DBNull.Value)
                                entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                            if (reader["OrderTransactionCode4"] != DBNull.Value)
                                entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                            if (reader["OrderTransactionCode5"] != DBNull.Value)
                                entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();
                            if (reader["Uname"] != DBNull.Value)
                                entity.Uname = reader["Uname"].ToString();
                            if (reader["saler"] != DBNull.Value)
                                entity.saler = reader["saler"].ToString();
                            if (reader["dathang"] != DBNull.Value)
                                entity.dathang = reader["dathang"].ToString();
                            if (reader["anhsanpham"] != DBNull.Value)
                                entity.anhsanpham = reader["anhsanpham"].ToString();
                            if (reader["OrderType"] != DBNull.Value)
                                entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                            if (reader["IsCheckNotiPrice"] != DBNull.Value)
                                entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                            else
                                entity.IsCheckNotiPrice = false;
                            list.Add(entity);
                        }
                    }
                    else
                    {
                        int MainOrderID = reader["ID"].ToString().ToInt(0);
                        var entity = new OrderGetSQL();
                        if (reader["ID"] != DBNull.Value)
                            entity.ID = MainOrderID;
                        if (reader["TotalPriceVND"] != DBNull.Value)
                            entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                        if (reader["Deposit"] != DBNull.Value)
                            entity.Deposit = reader["Deposit"].ToString();
                        if (reader["CreatedDate"] != DBNull.Value)
                            entity.CreatedDate = reader["CreatedDate"].ToString();
                        if (reader["Status"] != DBNull.Value)
                        {
                            entity.Status = Convert.ToInt32(reader["Status"].ToString());
                        }
                        if (reader["statusstring"] != DBNull.Value)
                        {
                            entity.statusstring = reader["statusstring"].ToString();
                        }
                        if (reader["OrderTransactionCode"] != DBNull.Value)
                            entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                        if (reader["OrderTransactionCode2"] != DBNull.Value)
                            entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                        if (reader["OrderTransactionCode3"] != DBNull.Value)
                            entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                        if (reader["OrderTransactionCode4"] != DBNull.Value)
                            entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                        if (reader["OrderTransactionCode5"] != DBNull.Value)
                            entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();

                        if (reader["Uname"] != DBNull.Value)
                            entity.Uname = reader["Uname"].ToString();
                        if (reader["saler"] != DBNull.Value)
                            entity.saler = reader["saler"].ToString();
                        if (reader["dathang"] != DBNull.Value)
                            entity.dathang = reader["dathang"].ToString();
                        if (reader["anhsanpham"] != DBNull.Value)
                            entity.anhsanpham = reader["anhsanpham"].ToString();
                        if (reader["OrderType"] != DBNull.Value)
                            entity.OrderType = reader["OrderType"].ToString().ToInt(1);

                        if (reader["IsCheckNotiPrice"] != DBNull.Value)
                            entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                        else
                            entity.IsCheckNotiPrice = false;
                        list.Add(entity);
                    }
                }
                else
                {
                    int MainOrderID = reader["ID"].ToString().ToInt(0);
                    var entity = new OrderGetSQL();
                    if (reader["ID"] != DBNull.Value)
                        entity.ID = MainOrderID;
                    if (reader["TotalPriceVND"] != DBNull.Value)
                        entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                    if (reader["Deposit"] != DBNull.Value)
                        entity.Deposit = reader["Deposit"].ToString();
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.CreatedDate = reader["CreatedDate"].ToString();
                    if (reader["Status"] != DBNull.Value)
                    {
                        entity.Status = Convert.ToInt32(reader["Status"].ToString());
                    }
                    if (reader["statusstring"] != DBNull.Value)
                    {
                        entity.statusstring = reader["statusstring"].ToString();
                    }
                    if (reader["OrderTransactionCode"] != DBNull.Value)
                        entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                    if (reader["OrderTransactionCode2"] != DBNull.Value)
                        entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                    if (reader["OrderTransactionCode3"] != DBNull.Value)
                        entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                    if (reader["OrderTransactionCode4"] != DBNull.Value)
                        entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                    if (reader["OrderTransactionCode5"] != DBNull.Value)
                        entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();

                    if (reader["Uname"] != DBNull.Value)
                        entity.Uname = reader["Uname"].ToString();
                    if (reader["saler"] != DBNull.Value)
                        entity.saler = reader["saler"].ToString();
                    if (reader["dathang"] != DBNull.Value)
                        entity.dathang = reader["dathang"].ToString();
                    if (reader["anhsanpham"] != DBNull.Value)
                        entity.anhsanpham = reader["anhsanpham"].ToString();
                    if (reader["OrderType"] != DBNull.Value)
                        entity.OrderType = reader["OrderType"].ToString().ToInt(1);

                    if (reader["IsCheckNotiPrice"] != DBNull.Value)
                        entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                    else
                        entity.IsCheckNotiPrice = false;
                    list.Add(entity);
                }
            }
            reader.Close();
            return list;
        }
        public static string Report_TotalItem(int pricefrom, int priceto)
        {
            var sql = @"select Count(*) as Total from tbl_Account as ac";
            sql += " left outer join(select Sum(CONVERT(numeric(18, 2), TotalPriceVND)) as Total, UID from tbl_MainOder group by UID) as mo ON mo.UID = ac.ID ";
            sql += " where mo.IsHidden = 0 and ac.RoleID = 1 and mo.Total > " + pricefrom + " and mo.Total < " + priceto + " ";

            string Total = "0";

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    Total = reader["Total"].ToString();
            }

            reader.Close();
            return Total;
        }

        public static double GetTotalPrice(int UID, int status, string PriceType, int orderType)
        {
            var sql = @"select total=SUM(CAST(" + PriceType + " as float)) ";
            sql += "from tbl_MainOder ";
            sql += "where IsHidden = 0 and UID=" + UID.ToString() + " AND STATUS=" + status.ToString() + " And OrderType=" + orderType.ToString();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            double total = 0;
            while (reader.Read())
            {
                if (reader["total"] != DBNull.Value)
                    total = Convert.ToDouble(reader["total"].ToString());
            }
            reader.Close();
            return total;
        }

        public static List<tbl_MainOder> GetAllDateToDateNotPage(string fd, string td)
        {
            var sql = @"select * ";
            sql += "from dbo.tbl_MainOder ";
            sql += "Where IsHidden = 0 and OrderType Like N'%%' ";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<tbl_MainOder> list = new List<tbl_MainOder>();
            while (reader.Read())
            {
                var entity = new tbl_MainOder();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["ShopID"] != DBNull.Value)
                    entity.ShopID = reader["ShopID"].ToString();
                entity.TotalPriceVND = "0";
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();

                entity.TotalPriceReal = "0";
                if (reader["TotalPriceReal"] != DBNull.Value)
                    entity.TotalPriceReal = reader["TotalPriceReal"].ToString();

                entity.Deposit = "0";
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt();

                if (reader["ShopName"] != DBNull.Value)
                    entity.ShopName = reader["ShopName"].ToString();

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString().ToInt(0);

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<tbl_MainOder> GetAllDateToDate(string fd, string td, int pageIndex, int pageSize)
        {
            var sql = @"select * ";
            sql += "from dbo.tbl_MainOder ";
            sql += "Where IsHidden = 0 and OrderType Like N'%%' ";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += " ORDER BY CreatedDate DESC, Status desc OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<tbl_MainOder> list = new List<tbl_MainOder>();
            while (reader.Read())
            {
                var entity = new tbl_MainOder();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["ShopID"] != DBNull.Value)
                    entity.ShopID = reader["ShopID"].ToString();
                entity.TotalPriceVND = "0";
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();

                entity.TotalPriceReal = "0";
                if (reader["TotalPriceReal"] != DBNull.Value)
                    entity.TotalPriceReal = reader["TotalPriceReal"].ToString();

                entity.Deposit = "0";
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt();

                if (reader["ShopName"] != DBNull.Value)
                    entity.ShopName = reader["ShopName"].ToString();

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString().ToInt(0);

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static int GetTotalDateToDate(string fd, string td, string st)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from dbo.tbl_MainOder ";
            sql += "Where IsHidden = 0 and OrderType Like N'%%' ";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            if (!string.IsNullOrEmpty(st))
            {
                sql += "And Status=" + st + " ";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);

            }
            reader.Close();
            return a;
        }

        public static double GetTotalPriceDateToDate(string fd, string td, string col)
        {
            var sql = @"select Total = COUNT(*) ";
            if (!string.IsNullOrEmpty(col))
                sql += ", TotalPrice=Sum(CAST(" + col + " as float)) ";
            sql += "from dbo.tbl_MainOder ";
            sql += "Where IsHidden = 0 and OrderType Like N'%%' ";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            double a = 0;
            while (reader.Read())
            {
                if (reader["TotalPrice"] != DBNull.Value)
                    a = reader["TotalPrice"].ToString().ToFloat(0);
            }
            reader.Close();
            return a;
        }

        public static List<tbl_MainOder> GetBuyProBySQLNotPage(string fd, string td)
        {
            var sql = @"select * ";
            sql += "from dbo.tbl_MainOder ";
            sql += "Where IsHidden = 0 and Status > = 5 ";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += " ORDER BY CreatedDate DESC, Status desc ";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<tbl_MainOder> list = new List<tbl_MainOder>();
            while (reader.Read())
            {
                var entity = new tbl_MainOder();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["ShopID"] != DBNull.Value)
                    entity.ShopID = reader["ShopID"].ToString();
                entity.TotalPriceVND = "0";
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();

                entity.TotalPriceReal = "0";
                if (reader["TotalPriceReal"] != DBNull.Value)
                    entity.TotalPriceReal = reader["TotalPriceReal"].ToString();

                entity.Deposit = "0";
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt();

                if (reader["ShopName"] != DBNull.Value)
                    entity.ShopName = reader["ShopName"].ToString();

                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt();

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString().ToInt(0);

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<tbl_MainOder> GetBuyProBySQL(string fd, string td, int pageIndex, int pageSize)
        {
            var sql = @"select * ";
            sql += "from dbo.tbl_MainOder ";
            sql += "Where IsHidden = 0 and Status > = 5 ";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += " ORDER BY CreatedDate DESC, Status desc OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<tbl_MainOder> list = new List<tbl_MainOder>();
            while (reader.Read())
            {
                var entity = new tbl_MainOder();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["ShopID"] != DBNull.Value)
                    entity.ShopID = reader["ShopID"].ToString();
                entity.TotalPriceVND = "0";
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();

                entity.TotalPriceReal = "0";
                if (reader["TotalPriceReal"] != DBNull.Value)
                    entity.TotalPriceReal = reader["TotalPriceReal"].ToString();

                entity.Deposit = "0";
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();

                if (reader["PriceVND"] != DBNull.Value)
                    entity.PriceVND = reader["PriceVND"].ToString();

                if (reader["FeeShipCN"] != DBNull.Value)
                    entity.FeeShipCN = reader["FeeShipCN"].ToString();

                if (reader["FeeWeight"] != DBNull.Value)
                    entity.FeeWeight = reader["FeeWeight"].ToString();

                if (reader["FeeBuyPro"] != DBNull.Value)
                    entity.FeeBuyPro = reader["FeeBuyPro"].ToString();

                if (reader["IsCheckProductPrice"] != DBNull.Value)
                    entity.IsCheckProductPrice = reader["IsCheckProductPrice"].ToString();

                if (reader["IsPackedPrice"] != DBNull.Value)
                    entity.IsPackedPrice = reader["IsPackedPrice"].ToString();

                if (reader["InsuranceMoney"] != DBNull.Value)
                    entity.InsuranceMoney = reader["InsuranceMoney"].ToString();

                if (reader["FeeInWareHouse"] != DBNull.Value)
                    entity.FeeInWareHouse = Convert.ToDouble(reader["FeeInWareHouse"].ToString());

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt();

                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt(0);

                if (reader["ShopName"] != DBNull.Value)
                    entity.ShopName = reader["ShopName"].ToString();

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString().ToInt(0);

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static int GetTotalBuyProBySQL(string fd, string td)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from dbo.tbl_MainOder ";
            sql += "Where IsHidden = 0 and Status > = 5 ";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {

                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }
        public static double GetTotalPriceBuyProBySQL(string fd, string td, string Col)
        {
            var sql = @"select Total=Count(*) ";
            sql += ", TotalPrice=SUM(Cast(" + Col + " as Float)) ";
            sql += "from dbo.tbl_MainOder ";
            sql += "Where IsHidden = 0 and Status > = 5 ";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            double a = 0;
            while (reader.Read())
            {
                if (reader["TotalPrice"] != DBNull.Value)
                    a = Convert.ToDouble(reader["TotalPrice"].ToString());
            }
            reader.Close();
            return a;
        }
        #endregion

        #region 4.5

        public static tbl_MainOder GetByIDandStatus(int ID, int Status)
        {
            using (var db = new NHSTEntities())
            {
                var mo = db.tbl_MainOder.Where(x => x.ID == ID && x.Status < Status && x.IsHidden == false).FirstOrDefault();
                if (mo != null)
                    return mo;
                else return null;
            }
        }

        public static tbl_MainOder GetByIDAndUID(int ID, int UID)
        {
            using (var db = new NHSTEntities())
            {
                var mo = db.tbl_MainOder.Where(x => x.ID == ID && x.IsHidden == false && x.Status > 2 && x.Status <= 9 && x.UID == UID).FirstOrDefault();
                if (mo != null)
                    return mo;
                else return null;
            }
        }

        public static double GetTotalPriceDebt(string PriceType, string Username, int RoleID, int StaffID)
        {
            int a = 0;
            var sql = @"SELECT Total=SUM(CAST(" + PriceType + " as float)) ";
            sql += "FROM dbo.tbl_MainOder as mo ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS u ON mo.UID = u.ID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS s ON mo.SalerID = s.ID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS d ON mo.DathangID = d.ID ";
            sql += "WHERE mo.IsDebt = 1 AND mo.OrderType > 0 ";
            if (RoleID == 9)
            {
                sql += "    AND mo.Status != 1 and mo.CSID = " + StaffID + "";
            }
            if (!string.IsNullOrEmpty(Username))
            {
                sql += " AND u.Username like N'%" + Username + "%'";
            }
            double total = 0;
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    total = Convert.ToDouble(reader["Total"].ToString());
            }
            reader.Close();
            return total;
        }

        public static int GetTotalOrderListDebt(string Username, int RoleID, int StaffID)
        {
            int a = 0;
            var sql = @"SELECT Total=Count(*) ";
            sql += "FROM dbo.tbl_MainOder as mo ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS u ON mo.UID = u.ID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS s ON mo.SalerID = s.ID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS d ON mo.DathangID = d.ID ";
            sql += "WHERE mo.IsDebt = 1 AND mo.OrderType > 0 ";
            if (RoleID == 9)
            {
                sql += "    AND mo.Status != 1 and mo.CSID = " + StaffID + "";
            }
            if (!string.IsNullOrEmpty(Username))
            {
                sql += " AND u.Username like N'%" + Username + "%'";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            return a;
        }

        public static int GetTotalForOrderListOfDK(int RoleID, int StaffID, int orderType, string searchtext, int Type, string fd, string td, string priceFrom, string priceTo, string st, bool isNotCode, string mvd, string mdh, int PTVC)
        {
            int a = 0;
            var sql = @"select Total=Count(*) ";
            sql += "FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_AccountInfo AS ui ON mo.UID = ui.UID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN ";
            sql += "(SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage) sm ON sm.MainOrderID = mo.ID and totalSmallPackages = 1 LEFT OUTER JOIN ";
            sql += "(SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1 ";
            sql += "LEFT OUTER JOIN(SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.OrderTransactionCode  AS [text()] ";
            sql += "FROM tbl_SmallPackage ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [MaVanDon] FROM tbl_MainOder ST2) se on se.ID=mo.ID ";

            sql += "Left outer join ";
            sql += " (SELECT DISTINCT ST2.ID, SUBSTRING((SELECT ',' + ST1.MainOrderCode   AS [text()] FROM tbl_MainOrderCode ST1 WHERE ST1.MainOrderID = ST2.ID FOR XML PATH('')), 2, 1000)[MaDonHangString] FROM tbl_MainOder ST2) sz on sz.ID = mo.ID ";

            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.title_origin  AS [text()] ";
            sql += "FROM tbl_Order ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [TenSanPham] FROM tbl_MainOder ST2) sp on sp.ID=mo.ID ";
            sql += "Where mo.IsHidden = 0 and mo.OrderType = '" + orderType + "'";

            if (RoleID == 3)
            {
                sql += "    AND mo.Status != 1 and mo.DathangID = " + StaffID + "";
            }
            else if (RoleID == 4)
            {
                sql += "    AND mo.Status >= 5 and mo.Status < 7";
            }
            else if (RoleID == 5)
            {
                sql += "    AND mo.Status >= 5 and mo.Status <= 7";
            }
            else if (RoleID == 6)
            {
                sql += "    AND mo.Status != 1 and mo.SalerID = " + StaffID + "";
            }
            else if (RoleID == 8)
            {
                sql += "    AND mo.Status >= 9 and mo.Status < 10";
            }
            else if (RoleID == 7)
            {
                sql += "    AND mo.Status >= 2";
            }

            if (PTVC > 0)
            {
                sql += "    AND mo.ShippingType = " + PTVC + " ";
            }

            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 1)
                    sql += " And mo.ID like N'%" + searchtext + "%'";

                if (Type == 2)
                    sql += " And sz.MaDonHangString like N'%" + searchtext + "%'";

                if (Type == 3)
                    sql += " And se.MaVanDon like N'%" + searchtext + "%'";

                if (Type == 4)
                    sql += "  AND u.Email like N'%" + searchtext + "%'";

                if (Type == 5)
                    sql += " And u.Username like N'%" + searchtext + "%'";

                if (Type == 6)
                    sql += " And ui.Phone like N'%" + searchtext + "%'";

                if (Type == 7)
                    sql += "  AND sp.TenSanPham like N'%" + searchtext + "%'";
            }
            if (!string.IsNullOrEmpty(st))
            {
                if (st != "-1")
                    sql += " AND mo.Status in (" + st + ")";
            }
            if (!string.IsNullOrEmpty(mvd))
            {
                sql += " And se.MaVanDon like N'%" + mvd + "%'";
            }
            if (!string.IsNullOrEmpty(mdh))
            {
                sql += " And sz.MaDonHangString like N'%" + mdh + "%'";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            if (!string.IsNullOrEmpty(priceFrom))
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            }
            if (!string.IsNullOrEmpty(priceTo) && !string.IsNullOrEmpty(priceFrom))
            {
                if (Convert.ToDouble(priceFrom) <= Convert.ToDouble(priceTo))
                    sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            }
            if (isNotCode == true)
            {
                sql += " AND sm.totalSmallPackages is null";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            return a;
        }

        public static List<OrderGetSQL> GetAllOrderListDebt(int pageIndex, int pageSize, string Username, int RoleID, int StaffID)
        {
            var list = new List<OrderGetSQL>();
            var sql = @"SELECT mo.ID, mo.UID, mo.TotalPriceVND, mo.Deposit, mo.CreatedDate, mo.Status, u.Username AS Uname, s.Username AS saler, d.Username AS dathang,
                               mo.DepostiDate, mo.DateShipper, mo.DateBuy, mo.DateBuyOK, mo.DateTQ, mo.DateToVN, mo.DateVN, mo.PayDate, mo.CompleteDate, mo.DateToShip, mo.DateToCancel ";
            sql += "FROM dbo.tbl_MainOder as mo ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS u ON mo.UID = u.ID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS s ON mo.SalerID = s.ID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS d ON mo.DathangID = d.ID ";
            sql += "WHERE mo.IsDebt=1 AND mo.OrderType > 0 ";
            if (RoleID == 9)
            {
                sql += "    AND mo.Status != 1 AND mo.CSID = " + StaffID + "";
            }
            if (!string.IsNullOrEmpty(Username))
            {
                sql += " AND u.Username like N'%" + Username + "%'";
            }
            sql += "ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new OrderGetSQL();

                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["Uname"] != DBNull.Value)
                    entity.Uname = reader["Uname"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();

                int Status = 0;
                if (reader["Status"] != DBNull.Value)
                    Status = Convert.ToInt32(reader["Status"].ToString());

                entity.statusstring = PJUtils.IntToRequestAdminNew(Status);

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.Created = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Lên đơn:</span><span>" + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + "</span> </p>";

                if (reader["DepostiDate"] != DBNull.Value)
                    entity.DepostiDate = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn đã cọc:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + "</span> </p>";

                if (reader["DateShipper"] != DBNull.Value)
                    entity.DateShipper = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn người bán giao:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + "</span> </p>";

                if (reader["DateBuy"] != DBNull.Value)
                    entity.DateBuy = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn chờ mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + "</span> </p>";

                if (reader["DateBuyOK"] != DBNull.Value)
                    entity.DateBuyOK = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn đã mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + "</span> </p>";

                if (reader["DateTQ"] != DBNull.Value)
                    entity.DateTQ = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Kho Trung Quốc nhận hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + "</span> </p>";

                if (reader["DateToVN"] != DBNull.Value)
                    entity.DateToVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trên đường về Việt Nam:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + "</span> </p>";

                if (reader["DateVN"] != DBNull.Value)
                    entity.DateVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trong kho Hà Nội:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + "</span> </p>";

                if (reader["PayDate"] != DBNull.Value)
                    entity.DatePay = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã thanh toán:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + "</span> </p>";

                if (reader["CompleteDate"] != DBNull.Value)
                    entity.CompleteDate = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã hoàn thành:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + "</span> </p>";

                if (reader["DateToShip"] != DBNull.Value)
                    entity.DateToShip = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đang giao hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + "</span> </p>";

                if (reader["DateToCancel"] != DBNull.Value)
                    entity.DateToCancel = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn khiếu nại:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + "</span> </p>";


                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<OrderGetSQL> GetByUserIDInSQLHelperWithFilterOrderList_PhieuIn(int RoleID, int StaffID, int OrderType, string searchtext, int Type, string fd, string td, string priceFrom, string priceTo, string st, bool isNotCode, int pageIndex, int pageSize, string mvd, string mdh, int sort, int PTVC)
        {
            var list = new List<OrderGetSQL>();
            var sql = @"; with ds as ( ";
            sql += "  select  m.ID, ";
            sql += "count(m.ID) over() as totalrow ";
            sql += " from tbl_MainOder m ";
            sql += ") select ds.ID, ds.totalrow, mo.Site, mo.BankPayment, mo.TotalPriceVND,mo.ShippingType,mo.TotalPriceReal,mo.ShopName,mo.FeeShipCN, mo.MaDonTruoc,mo.OrderTransactionCode, mo.PriceVND, mo.OrderDone,mo.CurrentCNYVN, mo.DathangID, mo.SalerID ,mo.IsDoneSmallPackage, ";
            sql += "mo.MainOrderCode,mo.OrderTransactionCode,  mo.Deposit, mo.CreatedDate,ui.Phone, mo.DepostiDate, mo.DateBuy,mo.DateShipper, mo.DateTQ,  ";
            sql += " mo.DateVN,mo.PayDate,mo.CompleteDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, mo.DateBuyOK, mo.DateToVN, mo.DateToCancel, mo.DateToShip,    ";
            sql += "mo.DathangID,  mo.SalerID, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, sm.totalSmallPackages,  mo.SalerID,  u.Username AS Uname,  ";
            sql += " s.Username AS saler, d.Username AS dathang,   ";
            sql += " CASE WHEN mo.LinkImage IS NULL THEN '' ELSE '<img alt=\"\" src=\"' + mo.LinkImage + '\" width=\"100%\">' END AS anhsanpham,     ";
            sql += "CASE WHEN mo.IsDoneSmallPackage = 1      ";
            sql += "THEN N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã đủ MVĐ</span>'     ";
            sql += "WHEN a.countrow > 0      ";
            sql += " THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đã nhập</span>'  ";
            sql += "ELSE N'<span class=\"badge red darken-2 white-text border-radius-2\">Chưa nhập MVĐ</span>'   ";
            sql += "END AS hasSmallpackage   ";
            sql += "from ds  ";
            sql += "left join tbl_MainOder mo on mo.ID = ds.ID   ";
            sql += "LEFT OUTER JOIN dbo.tbl_AccountInfo AS ui ON mo.UID = ui.UID    ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS u ON mo.UID = u.ID   ";
            sql += " LEFT OUTER JOIN dbo.tbl_Account AS s ON mo.SalerID = s.ID    ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS d ON mo.DathangID = d.ID    ";
            sql += "LEFT OUTER JOIN (SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage)  ";
            sql += "sm ON sm.MainOrderID = mo.ID and totalSmallPackages = 1   ";
            sql += " LEFT OUTER JOIN(SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID  ";
            sql += "Where mo.IsHidden = 0 and mo.Status = 4 and mo.OrderDone = 1  ";
            if (PTVC > 0)
            {
                sql += "    AND mo.ShippingType = " + PTVC + " ";
            }
            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 1)
                    sql += " And mo.ID like N'%" + searchtext + "%'";

                if (Type == 2)
                    sql += " And mo.MainOrderCode like N'%" + searchtext + "%'";

                if (Type == 3)
                    sql += " And mo.OrderTransactionCode like N'%" + searchtext + "%'";

                if (Type == 4)
                    sql += "  AND u.Email like N'%" + searchtext + "%'";

                if (Type == 5)
                    sql += " And u.Username like N'%" + searchtext + "%'";

                if (Type == 6)
                    sql += " And ui.Phone like N'%" + searchtext + "%'";

                //if (Type == 7)
                //    sql += "  AND sp.TenSanPham like N'%" + searchtext + "%'";
            }
            if (!string.IsNullOrEmpty(st))
            {
                if (st != "-1")
                    sql += " AND mo.Status in (" + st + ")";
            }
            if (!string.IsNullOrEmpty(mvd))
            {
                sql += " And  mo.OrderTransactionCode like N'%" + mvd + "%'";
            }
            if (!string.IsNullOrEmpty(mdh))
            {
                sql += " And mo.MainOrderCode like N'%" + mdh + "%'";
            }
            if (!string.IsNullOrEmpty(priceFrom))
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            }
            if (!string.IsNullOrEmpty(priceTo) && !string.IsNullOrEmpty(priceFrom))
            {
                if (Convert.ToDouble(priceFrom) <= Convert.ToDouble(priceTo))
                    sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            }
            if (isNotCode == true)
            {
                sql += " AND sm.totalSmallPackages is null";
            }
            if (st == "2")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DepositDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DepositDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DepositDate ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DepositDate DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "3")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateShipper >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateShipper <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateShipper ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateShipper DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "4")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuy >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuy <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateBuy ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateBuy DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "5")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuyOK >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuyOK <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateBuyOK ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateBuyOK DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "6")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateTQ >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateTQ <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateTQ ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateTQ DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "7")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToVN >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToVN <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateToVN ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateToVN DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "8")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateVN >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateVN <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateVN ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateVN DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "9")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.PayDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.PayDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.PayDate ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.PayDate DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "10")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CompleteDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CompleteDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.CompleteDate ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.CompleteDate DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "11")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToShip >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToShip <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateToShip ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateToShip DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "12")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToCancel >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToCancel <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateToCancel ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateToCancel DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.ID ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            //if (sort == 0)
            //{
            //    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            //}
            //else if (sort == 1)
            //{
            //    sql += " ORDER BY mo.ID ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            //}
            //else if (sort == 2)
            //{
            //    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            //}
            //else if (sort == 3)
            //{
            //    sql += " ORDER BY mo.Status ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            //}
            //else if (sort == 4)
            //{
            //    sql += " ORDER BY mo.Status DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            //}
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                int Status = 0;

                var entity = new OrderGetSQL();

                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();

                if (reader["Site"] != DBNull.Value)
                    entity.Site = reader["Site"].ToString();

                if (reader["ShopName"] != DBNull.Value)
                    entity.ShopName = reader["ShopName"].ToString();

                if (reader["BankPayment"] != DBNull.Value)
                    entity.StaffNote = reader["BankPayment"].ToString();

                entity.Deposit = "0";
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();

                double TiGia = 0;
                if (reader["CurrentCNYVN"] != DBNull.Value)
                    TiGia = Convert.ToDouble(reader["CurrentCNYVN"].ToString());
                entity.Currency = TiGia.ToString();

                double PhiShipTQ = 0;
                if (reader["FeeShipCN"] != DBNull.Value)
                    PhiShipTQ = Convert.ToDouble(reader["FeeShipCN"].ToString());
                entity.FeeShipCN = PhiShipTQ.ToString();

                double TienMuaThat = 0;
                if (reader["TotalPriceReal"] != DBNull.Value)
                    TienMuaThat = Convert.ToDouble(reader["TotalPriceReal"].ToString());
                entity.TotalPriceReal = TienMuaThat.ToString();

                double TienHang = 0;
                if (reader["PriceVND"] != DBNull.Value)
                    TienHang = Convert.ToDouble(reader["PriceVND"].ToString());
                entity.PriceVND = TienHang.ToString();

                double HoaHong = 0;
                double HoaHongCYN = 0;
                if (TienMuaThat > 0)
                {
                    HoaHong = Math.Round(TienHang + PhiShipTQ - TienMuaThat, 0);
                    HoaHongCYN = Math.Round(HoaHong / TiGia, 2);
                }
                entity.HoaHongVND = HoaHong.ToString();
                entity.HoaHongCYN = HoaHongCYN.ToString();

                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]).ToString("dd/MM/yyyy HH:mm");

                if (reader["Status"] != DBNull.Value)
                {
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                    Status = Convert.ToInt32(reader["Status"].ToString());
                }

                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();

                if (reader["Uname"] != DBNull.Value)
                    entity.Uname = reader["Uname"].ToString();

                if (reader["MainOrderCode"] != DBNull.Value)
                    entity.MainOrderCode = reader["MainOrderCode"].ToString();

                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();

                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();

                if (reader["TotalPriceReal"] != DBNull.Value)
                    entity.TotalPriceReal = reader["TotalPriceReal"].ToString();

                if (reader["SalerID"] != DBNull.Value)
                    entity.SalerID = Convert.ToInt32(reader["SalerID"].ToString());

                if (reader["DathangID"] != DBNull.Value)
                    entity.DathangID = Convert.ToInt32(reader["DathangID"].ToString());

                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();

                if (reader["IsDoneSmallPackage"] != DBNull.Value)
                    entity.IsDoneSmallPackage = Convert.ToBoolean(reader["IsDoneSmallPackage"].ToString());

                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);

                if (reader["MaDonTruoc"] != DBNull.Value)
                    entity.MaDonTruoc = reader["MaDonTruoc"].ToString().ToInt();

                if (reader["totalrow"] != DBNull.Value)
                    entity.totalrow = reader["totalrow"].ToString().ToInt();

                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                else
                    entity.IsCheckNotiPrice = false;

                if (reader["OrderDone"] != DBNull.Value)
                    entity.OrderDone = Convert.ToBoolean(reader["OrderDone"]);
                else
                    entity.OrderDone = false;

                if (reader["hasSmallpackage"] != DBNull.Value)
                    entity.hasSmallpackage = reader["hasSmallpackage"].ToString();

                if (Status == 0)
                {
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.Created = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn mới:</span><span>" + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.Created = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn mới:</span><span>" + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + "</span> </p>";
                }
                if (Status == 1)
                {
                    entity.Cancel = "<span class=\"badge black darken-2 white-text border-radius-2\">Hủy đơn hàng</span>";
                }
                if (Status == 2)
                {
                    if (reader["DepostiDate"] != DBNull.Value)
                        entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn đã cọc:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DepostiDate"] != DBNull.Value)
                        entity.DepostiDate = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn đã cọc:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + "</span> </p>";
                }
                if (Status == 3)
                {
                    if (reader["DateShipper"] != DBNull.Value)
                        entity.DateShipper = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn người bán giao:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateShipper"] != DBNull.Value)
                        entity.DateShipper = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn người bán giao:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + "</span> </p>";
                }
                if (Status == 4)
                {
                    if (reader["DateBuy"] != DBNull.Value)
                        entity.DateBuy = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn chờ mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateBuy"] != DBNull.Value)
                        entity.DateBuy = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn chờ mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + "</span> </p>";
                }
                if (Status == 5)
                {
                    if (reader["DateBuyOK"] != DBNull.Value)
                        entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn đã mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateBuyOK"] != DBNull.Value)
                        entity.DateBuyOK = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn đã mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + "</span> </p>";
                }
                if (Status == 6)
                {
                    if (reader["DateTQ"] != DBNull.Value)
                        entity.DateTQ = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Kho Trung Quốc nhận hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateTQ"] != DBNull.Value)
                        entity.DateTQ = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Kho Trung Quốc nhận hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + "</span> </p>";
                }
                if (Status == 7)
                {
                    if (reader["DateToVN"] != DBNull.Value)
                        entity.DateToVN = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Trên đường về Việt Nam:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToVN"] != DBNull.Value)
                        entity.DateToVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trên đường về Việt Nam:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + "</span> </p>";
                }
                if (Status == 8)
                {
                    if (reader["DateVN"] != DBNull.Value)
                        entity.DateVN = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Trong kho Hà Nội:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateVN"] != DBNull.Value)
                        entity.DateVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trong kho Hà Nội:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + "</span> </p>";
                }
                if (Status == 9)
                {
                    if (reader["PayDate"] != DBNull.Value)
                        entity.DatePay = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đã thanh toán:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["PayDate"] != DBNull.Value)
                        entity.DatePay = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã thanh toán:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + "</span> </p>";
                }
                if (Status == 10)
                {
                    if (reader["CompleteDate"] != DBNull.Value)
                        entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đã hoàn thành:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["CompleteDate"] != DBNull.Value)
                        entity.CompleteDate = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã hoàn thành:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + "</span> </p>";
                }

                if (Status == 11)
                {
                    if (reader["DateToShip"] != DBNull.Value)
                        entity.DateToShip = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đang giao hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToShip"] != DBNull.Value)
                        entity.DateToShip = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đang giao hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + "</span> </p>";
                }

                if (Status == 12)
                {
                    if (reader["DateToCancel"] != DBNull.Value)
                        entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn khiếu nại:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToCancel"] != DBNull.Value)
                        entity.DateToCancel = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn khiếu nại:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + "</span> </p>";
                }

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<OrderGetSQL> GetByUserIDInSQLHelperWithFilterOrderList_PhieuIn_excel(int RoleID, int StaffID, int OrderType, string searchtext, int Type, string fd, string td, string priceFrom, string priceTo, string st, bool isNotCode, string mvd, string mdh, int sort, int PTVC)
        {
            var list = new List<OrderGetSQL>();
            var sql = @";with ds as ( ";
            sql += "  select  m.ID, ";
            sql += "count(m.ID) over() as totalrow ";
            sql += " from tbl_MainOder m ";
            sql += ") select ds.ID, ds.totalrow, mo.TotalPriceVND,mo.ShippingType,mo.ShopName,mo.MaDonTruoc,mo.OrderTransactionCode, mo.PriceVND, mo.OrderDone,mo.CurrentCNYVN, mo.DathangID, mo.SalerID ,mo.IsDoneSmallPackage, ";
            sql += "mo.MainOrderCode,mo.OrderTransactionCode,  mo.Deposit, mo.CreatedDate,ui.Phone, mo.DepostiDate, mo.DateBuy,mo.DateShipper, mo.DateTQ,  ";
            sql += " mo.DateVN,mo.PayDate,mo.CompleteDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, mo.DateBuyOK, mo.DateToVN, mo.DateToCancel, mo.DateToShip,    ";
            sql += "mo.DathangID,  mo.SalerID, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, sm.totalSmallPackages,  mo.SalerID,  u.Username AS Uname,  ";
            sql += " s.Username AS saler, d.Username AS dathang,   ";
            sql += " CASE WHEN mo.LinkImage IS NULL THEN '' ELSE '<img alt=\"\" src=\"' + mo.LinkImage + '\" width=\"100%\">' END AS anhsanpham,     ";
            sql += "CASE WHEN mo.IsDoneSmallPackage = 1      ";
            sql += "THEN N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã đủ MVĐ</span>'     ";
            sql += "WHEN a.countrow > 0      ";
            sql += " THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đã nhập</span>'  ";
            sql += "ELSE N'<span class=\"badge red darken-2 white-text border-radius-2\">Chưa nhập MVĐ</span>'   ";
            sql += "END AS hasSmallpackage   ";
            sql += "from ds  ";
            sql += "left join tbl_MainOder mo on mo.ID = ds.ID   ";
            sql += "LEFT OUTER JOIN dbo.tbl_AccountInfo AS ui ON mo.UID = ui.UID    ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS u ON mo.UID = u.ID   ";
            sql += " LEFT OUTER JOIN dbo.tbl_Account AS s ON mo.SalerID = s.ID    ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS d ON mo.DathangID = d.ID    ";
            sql += "LEFT OUTER JOIN (SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage)  ";
            sql += "sm ON sm.MainOrderID = mo.ID and totalSmallPackages = 1   ";
            sql += " LEFT OUTER JOIN(SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID  ";

            sql += "Where mo.IsHidden = 0 and mo.Status = 4 and mo.OrderDone = 1  ";

            //if (RoleID == 3)
            //{
            //    sql += "    AND mo.Status != 1 and mo.DathangID = " + StaffID + "";
            //}
            //else if (RoleID == 4)
            //{
            //    sql += "    AND mo.Status >= 5 and mo.Status < 7";
            //}
            //else if (RoleID == 5)
            //{
            //    sql += "    AND mo.Status >= 5 and mo.Status <= 7";
            //}
            //else if (RoleID == 6)
            //{
            //    sql += "    AND mo.Status != 1 and mo.SalerID = " + StaffID + "";
            //}
            //else if (RoleID == 8)
            //{
            //    sql += "    AND mo.Status >= 9 and mo.Status < 10";
            //}
            //else if (RoleID == 7)
            //{
            //    sql += "    AND mo.Status >= 2";
            //}

            if (PTVC > 0)
            {
                sql += "    AND mo.ShippingType = " + PTVC + " ";
            }

            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 1)
                    sql += " And mo.ID like N'%" + searchtext + "%'";

                if (Type == 2)
                    sql += " And mo.MainOrderCode like N'%" + searchtext + "%'";

                if (Type == 3)
                    sql += " And mo.OrderTransactionCode like N'%" + searchtext + "%'";

                if (Type == 4)
                    sql += "  AND u.Email like N'%" + searchtext + "%'";

                if (Type == 5)
                    sql += " And u.Username like N'%" + searchtext + "%'";

                if (Type == 6)
                    sql += " And ui.Phone like N'%" + searchtext + "%'";

                //if (Type == 7)
                //    sql += "  AND sp.TenSanPham like N'%" + searchtext + "%'";
            }
            if (!string.IsNullOrEmpty(st))
            {
                if (st != "-1")
                    sql += " AND mo.Status in (" + st + ")";
            }
            if (!string.IsNullOrEmpty(mvd))
            {
                sql += " And  mo.OrderTransactionCode like N'%" + mvd + "%'";
            }
            if (!string.IsNullOrEmpty(mdh))
            {
                sql += " And mo.MainOrderCode like N'%" + mdh + "%'";
            }








            if (!string.IsNullOrEmpty(priceFrom))
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            }
            if (!string.IsNullOrEmpty(priceTo) && !string.IsNullOrEmpty(priceFrom))
            {
                if (Convert.ToDouble(priceFrom) <= Convert.ToDouble(priceTo))
                    sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            }
            if (isNotCode == true)
            {
                sql += " AND sm.totalSmallPackages is null";
            }



            if (st == "2")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DepositDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DepositDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }


            }
            else if (st == "3")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateShipper >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateShipper <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }


            }
            else if (st == "4")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuy >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuy <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }


            }
            else if (st == "5")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuyOK >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuyOK <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }


            }
            else if (st == "6")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateTQ >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateTQ <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }


            }
            else if (st == "7")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToVN >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToVN <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }


            }
            else if (st == "8")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateVN >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateVN <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }


            }
            else if (st == "9")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.PayDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.PayDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }


            }
            else if (st == "10")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CompleteDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CompleteDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }


            }
            else if (st == "11")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToShip >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToShip <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

            }
            else if (st == "12")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToCancel >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToCancel <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
            }

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                int Status = 0;
                int MainOrderID = reader["ID"].ToString().ToInt(0);
                var entity = new OrderGetSQL();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = MainOrderID;
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["PriceVND"] != DBNull.Value)
                    entity.PriceVND = reader["PriceVND"].ToString();

                if (reader["ShopName"] != DBNull.Value)
                    entity.ShopName = reader["ShopName"].ToString();
                if (reader["CurrentCNYVN"] != DBNull.Value)
                    entity.Currency = reader["CurrentCNYVN"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]).ToString("dd/MM/yyyy HH:mm");
                if (reader["Status"] != DBNull.Value)
                {
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                    Status = Convert.ToInt32(reader["Status"].ToString());
                }
                //if (reader["statusstring"] != DBNull.Value)
                //{
                //    entity.statusstring = reader["statusstring"].ToString();
                //}
                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                //if (reader["OrderTransactionCode2"] != DBNull.Value)
                //    entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                //if (reader["OrderTransactionCode3"] != DBNull.Value)
                //    entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                //if (reader["OrderTransactionCode4"] != DBNull.Value)
                //    entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                //if (reader["OrderTransactionCode5"] != DBNull.Value)
                //    entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();
                if (reader["Uname"] != DBNull.Value)
                    entity.Uname = reader["Uname"].ToString();
                if (reader["MainOrderCode"] != DBNull.Value)
                    entity.MainOrderCode = reader["MainOrderCode"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();

                if (reader["SalerID"] != DBNull.Value)
                    entity.SalerID = Convert.ToInt32(reader["SalerID"].ToString());
                if (reader["DathangID"] != DBNull.Value)
                    entity.DathangID = Convert.ToInt32(reader["DathangID"].ToString());

                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();

                //if (reader["MaDonHang"] != DBNull.Value)
                //    entity.listMainOrderCode = reader["MaDonHang"].ToString().Split(',').ToList();

                if (reader["IsDoneSmallPackage"] != DBNull.Value)
                    entity.IsDoneSmallPackage = Convert.ToBoolean(reader["IsDoneSmallPackage"].ToString());

                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);


                if (reader["MaDonTruoc"] != DBNull.Value)
                    entity.MaDonTruoc = reader["MaDonTruoc"].ToString().ToInt();

                if (reader["totalrow"] != DBNull.Value)
                    entity.totalrow = reader["totalrow"].ToString().ToInt();

                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                else
                    entity.IsCheckNotiPrice = false;

                if (reader["OrderDone"] != DBNull.Value)
                    entity.OrderDone = Convert.ToBoolean(reader["OrderDone"]);
                else
                    entity.OrderDone = false;

                if (reader["hasSmallpackage"] != DBNull.Value)
                    entity.hasSmallpackage = reader["hasSmallpackage"].ToString();

                if (Status == 0)
                {
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.Created = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn mới:</span><span>" + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.Created = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn mới:</span><span>" + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + "</span> </p>";
                }

                if (Status == 1)
                {
                    entity.Cancel = "<span class=\"badge black darken-2 white-text border-radius-2\">Hủy đơn hàng</span>";
                }



                if (Status == 2)
                {
                    if (reader["DepostiDate"] != DBNull.Value)
                        entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn đã cọc:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DepostiDate"] != DBNull.Value)
                        entity.DepostiDate = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn đã cọc:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + "</span> </p>";
                }

                if (Status == 3)
                {
                    if (reader["DateShipper"] != DBNull.Value)
                        entity.DateShipper = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn người bán giao:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateShipper"] != DBNull.Value)
                        entity.DateShipper = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn người bán giao:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + "</span> </p>";
                }

                if (Status == 4)
                {
                    if (reader["DateBuy"] != DBNull.Value)
                        entity.DateBuy = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn chờ mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateBuy"] != DBNull.Value)
                        entity.DateBuy = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn chờ mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + "</span> </p>";
                }

                if (Status == 5)
                {
                    if (reader["DateBuyOK"] != DBNull.Value)
                        entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn đã mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateBuyOK"] != DBNull.Value)
                        entity.DateBuyOK = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn đã mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + "</span> </p>";
                }

                if (Status == 6)
                {
                    if (reader["DateTQ"] != DBNull.Value)
                        entity.DateTQ = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Kho Trung Quốc nhận hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateTQ"] != DBNull.Value)
                        entity.DateTQ = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Kho Trung Quốc nhận hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + "</span> </p>";
                }

                if (Status == 7)
                {
                    if (reader["DateToVN"] != DBNull.Value)
                        entity.DateToVN = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Trên đường về Việt Nam:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToVN"] != DBNull.Value)
                        entity.DateToVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trên đường về Việt Nam:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + "</span> </p>";
                }

                if (Status == 8)
                {
                    if (reader["DateVN"] != DBNull.Value)
                        entity.DateVN = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Trong kho Hà Nội:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateVN"] != DBNull.Value)
                        entity.DateVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trong kho Hà Nội:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + "</span> </p>";
                }

                if (Status == 9)
                {
                    if (reader["PayDate"] != DBNull.Value)
                        entity.DatePay = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đã thanh toán:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["PayDate"] != DBNull.Value)
                        entity.DatePay = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã thanh toán:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + "</span> </p>";
                }

                if (Status == 10)
                {
                    if (reader["CompleteDate"] != DBNull.Value)
                        entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đã hoàn thành:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["CompleteDate"] != DBNull.Value)
                        entity.CompleteDate = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã hoàn thành:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + "</span> </p>";
                }

                if (Status == 11)
                {
                    if (reader["DateToShip"] != DBNull.Value)
                        entity.DateToShip = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đang giao hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToShip"] != DBNull.Value)
                        entity.DateToShip = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đang giao hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + "</span> </p>";
                }

                if (Status == 12)
                {
                    if (reader["DateToCancel"] != DBNull.Value)
                        entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn khiếu nại:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToCancel"] != DBNull.Value)
                        entity.DateToCancel = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn khiếu nại:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + "</span> </p>";
                }

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<OrderGetSQL> GetByUserIDInSQLHelperWithFilterOrderList(int RoleID, int StaffID, int OrderType, string searchtext, int Type, string fd, string td, string priceFrom, string priceTo, string st, bool isNotCode, int pageIndex, int pageSize, string mvd, string mdh, int sort, int PTVC, int nvdh, int nvkd, int nvcs)
        {
            var list = new List<OrderGetSQL>();
            var sql = @";with ds as ( ";
            sql += "  select  m.ID, ";
            sql += "count(m.ID) over() as totalrow ";
            sql += " from tbl_MainOder m ";
            sql += ") select ds.ID, ds.totalrow,mo.BankPayment,mo.TotalPriceVND,mo.ShippingType,mo.ShopName,mo.MaDonTruoc,mo.OrderTransactionCode,mo.PriceVND,mo.OrderDone,mo.CurrentCNYVN,mo.DathangID,mo.SalerID,mo.CSID,mo.IsDoneSmallPackage, ";
            sql += "mo.MainOrderCode,mo.OrderTransactionCode,  mo.Deposit, mo.CreatedDate,ui.Phone, mo.DepostiDate, mo.DateBuy,mo.DateShipper, mo.DateTQ, mo.Site,  ";
            sql += " mo.DateVN,mo.PayDate,mo.CompleteDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, mo.DateBuyOK, mo.DateToVN, mo.DateToCancel, mo.DateToShip,    ";
            sql += "mo.DathangID,  mo.SalerID, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, cskh.Username AS cskhname, sm.totalSmallPackages,  mo.SalerID,  u.Username AS Uname,  ";
            sql += " s.Username AS saler, d.Username AS dathang,   ";
            sql += " CASE WHEN mo.LinkImage IS NULL THEN '' ELSE '<img alt=\"\" src=\"' + mo.LinkImage + '\" width=\"100%\">' END AS anhsanpham,     ";
            sql += "CASE WHEN mo.IsDoneSmallPackage = 1      ";
            sql += "THEN N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã đủ MVĐ</span>'     ";
            sql += "WHEN a.countrow > 0      ";
            sql += " THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đã nhập</span>'  ";
            sql += "ELSE N'<span class=\"badge red darken-2 white-text border-radius-2\">Chưa nhập MVĐ</span>'   ";
            sql += "END AS hasSmallpackage   ";
            sql += "from ds  ";
            sql += "LEFT JOIN tbl_MainOder mo on mo.ID = ds.ID   ";
            sql += "LEFT OUTER JOIN dbo.tbl_AccountInfo AS ui ON mo.UID = ui.UID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS u ON mo.UID = u.ID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS s ON mo.SalerID = s.ID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS d ON mo.DathangID = d.ID ";
            sql += "LEFT OUTER JOIN dbo.tbl_Account AS cskh ON mo.CSID = cskh.ID ";
            sql += "LEFT OUTER JOIN (SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage)  ";
            sql += "sm ON sm.MainOrderID = mo.ID and totalSmallPackages = 1   ";
            sql += "LEFT OUTER JOIN(SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID  ";
            sql += "Where mo.IsHidden = 0 and mo.OrderType = '" + OrderType + "'";
            if (RoleID == 3)
            {
                sql += "    AND mo.DathangID = " + StaffID + "";
            }
            else if (RoleID == 4)
            {
                sql += "    AND mo.Status >= 5 and mo.Status < 7";
            }
            else if (RoleID == 5)
            {
                sql += "    AND mo.Status >= 5 and mo.Status <= 7";
            }
            else if (RoleID == 6)
            {
                sql += "    AND mo.SalerID = " + StaffID + "";
            }
            else if (RoleID == 8)
            {
                sql += "    AND mo.Status >= 9 and mo.Status < 10";
            }
            else if (RoleID == 7)
            {
                sql += "    AND mo.Status >= 2";
            }
            else if (RoleID == 9)
            {
                sql += "    AND mo.CSID = " + StaffID + "";
            }
            if (PTVC > 0)
            {
                sql += "    AND mo.ShippingType = " + PTVC + " ";
            }
            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 1)
                    sql += " AND mo.ID like N'%" + searchtext + "%'";

                if (Type == 2)
                    sql += " AND mo.MainOrderCode like N'%" + searchtext + "%'";

                if (Type == 3)
                    sql += " AND mo.OrderTransactionCode like N'%" + searchtext + "%'";

                if (Type == 4)
                    sql += " AND u.Email like N'%" + searchtext + "%'";

                if (Type == 5)
                    sql += " AND u.Username like N'%" + searchtext + "%'";

                if (Type == 6)
                    sql += " AND ui.Phone like N'%" + searchtext + "%'";

                if (Type == 7)
                    sql += " AND mo.ShopName like N'%" + searchtext + "%'";
            }
            if (!string.IsNullOrEmpty(st))
            {
                if (st != "-1")
                {
                    if (st == "13")
                    {
                        sql += " AND mo.Status=5 AND DATEDIFF(day,mo.DateBuy,getdate()) > 3 ";
                    }
                    else if (st == "14")
                    {
                        sql += " AND mo.Status=3 AND DATEDIFF(day,mo.DateShipper,getdate()) > 6 ";
                    }
                    else if (st == "15")
                    {
                        sql += " AND mo.Status=6 AND DATEDIFF(day,mo.DateTQ,getdate()) > 3 ";
                    }
                    else if (st == "16")
                    {
                        sql += " AND mo.Status=7 AND DATEDIFF(day,mo.DateToVN,getdate()) > 6 ";
                    }
                    else
                    {
                        sql += " AND mo.Status in (" + st + ")";
                    }
                }
            }
            if (!string.IsNullOrEmpty(mvd))
            {
                sql += " AND  mo.OrderTransactionCode like N'%" + mvd + "%'";
            }
            if (!string.IsNullOrEmpty(mdh))
            {
                sql += " AND mo.MainOrderCode like N'%" + mdh + "%'";
            }
            if (nvdh > 0)
            {
                sql += " AND mo.DathangID =" + nvdh + "";
            }
            if (nvkd > 0)
            {
                sql += " AND mo.SalerID =" + nvkd + "";
            }
            if (nvcs > 0)
            {
                sql += " AND mo.CSID =" + nvcs + "";
            }
            if (!string.IsNullOrEmpty(priceFrom))
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            }
            if (!string.IsNullOrEmpty(priceTo) && !string.IsNullOrEmpty(priceFrom))
            {
                if (Convert.ToDouble(priceFrom) <= Convert.ToDouble(priceTo))
                    sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            }
            if (isNotCode == true)
            {
                sql += " AND sm.totalSmallPackages is null";
            }
            if (st == "2")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DepositDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DepositDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DepositDate ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DepositDate DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "3")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateShipper >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateShipper <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateShipper ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateShipper DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "4")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuy >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuy <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateBuy ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateBuy DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "5")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuyOK >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateBuyOK <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateBuyOK ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateBuyOK DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "6")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateTQ >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateTQ <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateTQ ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateTQ DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "7")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToVN >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToVN <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateToVN ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateToVN DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "8")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateVN >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateVN <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateVN ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateVN DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "9")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.PayDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.PayDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.PayDate ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.PayDate DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "10")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CompleteDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CompleteDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.CompleteDate ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.CompleteDate DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "11")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToShip >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToShip <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateToShip ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateToShip DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else if (st == "12")
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToCancel >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.DateToCancel <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }
                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.DateToCancel ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.DateToCancel DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(fd))
                {
                    var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
                }
                if (!string.IsNullOrEmpty(td))
                {
                    var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                    sql += " AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
                }

                if (sort == 0)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 1)
                {
                    sql += " ORDER BY mo.ID ASC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
                else if (sort == 2)
                {
                    sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
                }
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                int Status = 0;
                int MainOrderID = reader["ID"].ToString().ToInt(0);
                var entity = new OrderGetSQL();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = MainOrderID;
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["PriceVND"] != DBNull.Value)
                    entity.PriceVND = reader["PriceVND"].ToString();
                if (reader["ShopName"] != DBNull.Value)
                    entity.ShopName = reader["ShopName"].ToString();
                if (reader["Site"] != DBNull.Value)
                    entity.Site = reader["Site"].ToString();
                if (reader["CurrentCNYVN"] != DBNull.Value)
                    entity.Currency = reader["CurrentCNYVN"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["BankPayment"] != DBNull.Value)
                    entity.StaffNote = reader["BankPayment"].ToString();

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]).ToString("dd/MM/yyyy HH:mm");

                if (reader["Status"] != DBNull.Value)
                {
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                    Status = Convert.ToInt32(reader["Status"].ToString());
                }

                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();

                if (reader["Uname"] != DBNull.Value)
                    entity.Uname = reader["Uname"].ToString();
                if (reader["MainOrderCode"] != DBNull.Value)
                    entity.MainOrderCode = reader["MainOrderCode"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();
                if (reader["cskhname"] != DBNull.Value)
                    entity.CSKHNAME = reader["cskhname"].ToString();

                if (reader["SalerID"] != DBNull.Value)
                    entity.SalerID = Convert.ToInt32(reader["SalerID"].ToString());
                if (reader["DathangID"] != DBNull.Value)
                    entity.DathangID = Convert.ToInt32(reader["DathangID"].ToString());
                if (reader["CSID"] != DBNull.Value)
                    entity.CSID = Convert.ToInt32(reader["CSID"].ToString());
                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();

                //if (reader["MaDonHang"] != DBNull.Value)
                //    entity.listMainOrderCode = reader["MaDonHang"].ToString().Split(',').ToList();

                if (reader["IsDoneSmallPackage"] != DBNull.Value)
                    entity.IsDoneSmallPackage = Convert.ToBoolean(reader["IsDoneSmallPackage"].ToString());

                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);


                if (reader["MaDonTruoc"] != DBNull.Value)
                    entity.MaDonTruoc = reader["MaDonTruoc"].ToString().ToInt();

                if (reader["totalrow"] != DBNull.Value)
                    entity.totalrow = reader["totalrow"].ToString().ToInt();

                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                else
                    entity.IsCheckNotiPrice = false;

                if (reader["OrderDone"] != DBNull.Value)
                    entity.OrderDone = Convert.ToBoolean(reader["OrderDone"]);
                else
                    entity.OrderDone = false;

                if (reader["hasSmallpackage"] != DBNull.Value)
                    entity.hasSmallpackage = reader["hasSmallpackage"].ToString();

                if (Status == 0)
                {
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.Created = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn mới:</span><span>" + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.Created = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn mới:</span><span>" + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + "</span> </p>";
                }

                if (Status == 1)
                {
                    entity.Cancel = "<span class=\"badge black darken-2 white-text border-radius-2\">Hủy đơn hàng</span>";
                }



                if (Status == 2)
                {
                    if (reader["DepostiDate"] != DBNull.Value)
                        entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn đã cọc:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DepostiDate"] != DBNull.Value)
                        entity.DepostiDate = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn đã cọc:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + "</span> </p>";
                }

                if (Status == 3)
                {
                    if (reader["DateShipper"] != DBNull.Value)
                        entity.DateShipper = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn người bán giao:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateShipper"] != DBNull.Value)
                        entity.DateShipper = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn người bán giao:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + "</span> </p>";
                }

                if (Status == 4)
                {
                    if (reader["DateBuy"] != DBNull.Value)
                        entity.DateBuy = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn chờ mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateBuy"] != DBNull.Value)
                        entity.DateBuy = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn chờ mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + "</span> </p>";
                }

                if (Status == 5)
                {
                    if (reader["DateBuyOK"] != DBNull.Value)
                        entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn đã mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateBuyOK"] != DBNull.Value)
                        entity.DateBuyOK = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn đã mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + "</span> </p>";
                }

                if (Status == 6)
                {
                    if (reader["DateTQ"] != DBNull.Value)
                        entity.DateTQ = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Kho Trung Quốc nhận hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateTQ"] != DBNull.Value)
                        entity.DateTQ = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Kho Trung Quốc nhận hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + "</span> </p>";
                }

                if (Status == 7)
                {
                    if (reader["DateToVN"] != DBNull.Value)
                        entity.DateToVN = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Trên đường về Việt Nam:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToVN"] != DBNull.Value)
                        entity.DateToVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trên đường về Việt Nam:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + "</span> </p>";
                }

                if (Status == 8)
                {
                    if (reader["DateVN"] != DBNull.Value)
                        entity.DateVN = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Trong kho Hà Nội:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateVN"] != DBNull.Value)
                        entity.DateVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trong kho Hà Nội:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + "</span> </p>";
                }

                if (Status == 9)
                {
                    if (reader["PayDate"] != DBNull.Value)
                        entity.DatePay = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đã thanh toán:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["PayDate"] != DBNull.Value)
                        entity.DatePay = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã thanh toán:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + "</span> </p>";
                }

                if (Status == 10)
                {
                    if (reader["CompleteDate"] != DBNull.Value)
                        entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đã hoàn thành:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["CompleteDate"] != DBNull.Value)
                        entity.CompleteDate = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã hoàn thành:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + "</span> </p>";
                }

                if (Status == 11)
                {
                    if (reader["DateToShip"] != DBNull.Value)
                        entity.DateToShip = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đang giao hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToShip"] != DBNull.Value)
                        entity.DateToShip = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đang giao hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + "</span> </p>";
                }

                if (Status == 12)
                {
                    if (reader["DateToCancel"] != DBNull.Value)
                        entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn khiếu nại:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToCancel"] != DBNull.Value)
                        entity.DateToCancel = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn khiếu nại:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + "</span> </p>";
                }

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<OrderGetSQL> GetMoney(int RoleID, int StaffID, int OrderType, string searchtext, int Type, string fd, string td, string priceFrom, string priceTo, string st, bool isNotCode, string mvd, string mdh, int PTVC)
        {
            var list = new List<OrderGetSQL>();
            var sql = @"SELECT mo.ID, mo.TotalPriceVND,mo.ShippingType, mo.PriceVND, mo.OrderDone,mo.CurrentCNYVN, mo.DathangID, mo.SalerID ,mo.IsDoneSmallPackage,sz.MaDonHangString,se.MaVanDon,sw.MaDonHang,sp.TenSanPham, mo.Deposit, mo.CreatedDate, mo.DepostiDate, mo.DateBuy,mo.DateShipper, mo.DateTQ, mo.DateVN,mo.PayDate,mo.CompleteDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, mo.DateBuyOK, mo.DateToVN, mo.DateToCancel, mo.DateToShip, ";
            sql += "CASE mo.Status WHEN 0 THEN N'<span class=\"badge red darken-2 white-text border-radius-2\">Đơn mới</span>' ";
            sql += "WHEN 1 THEN N'<span class=\"badge black darken-2 white-text border-radius-2\">Đơn hàng hủy</span>' ";
            sql += "WHEN 2 THEN N'<span class=\"badge pink darken-2 white-text border-radius-2\">Đơn đã cọc</span>' ";
            sql += "WHEN 3 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đơn người bán giao</span>' ";
            sql += "WHEN 4 THEN N'<span class=\"badge green darken-2 white-text border-radius-2\">Đã duyệt đơn</span>' ";
            sql += "WHEN 5 THEN N'<span class=\"badge green darken-2 white-text border-radius-2\">Đơn đã mua hàng</span>' ";
            sql += "WHEN 6 THEN N'<span class=\"badge green darken-2 white-text border-radius-2\">Kho Trung Quốc nhận hàng</span>' ";
            sql += "WHEN 7 THEN N'<span class=\"badge orange darken-2 white-text border-radius-2\">Trên đường về Việt Nam</span>' ";
            sql += "WHEN 8 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Chờ thanh toán</span>' ";
            sql += "WHEN 9 THEN N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã thanh toán</span>' ";
            sql += "ELSE N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã hoàn thành</span>' ";
            sql += "END AS statusstring, mo.DathangID,  ";
            sql += "mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4,  ";
            sql += "mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, sm.totalSmallPackages,  ";
            sql += "mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4,  ";
            sql += "mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang,  ";
            sql += "CASE WHEN o.anhsanpham IS NULL THEN '' ELSE '<img alt=\"\" style=\"max-width:100px;max-height:100px\" src=\"' + o.anhsanpham + '\">' END AS anhsanpham, ";
            sql += "CASE WHEN mo.IsDoneSmallPackage = 1 THEN N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã đủ MVĐ</span>'  WHEN a.countrow > 0 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đã nhập</span>' ELSE N'<span class=\"badge red darken-2 white-text border-radius-2\">Chưa nhập MVĐ</span>' END AS hasSmallpackage ";
            sql += "FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            sql += "dbo.tbl_AccountInfo AS ui ON mo.UID = ui.UID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN ";
            sql += "(SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage) sm ON sm.MainOrderID = mo.ID and totalSmallPackages = 1 LEFT OUTER JOIN ";
            sql += "(SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1 ";
            sql += "LEFT OUTER JOIN(SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.OrderTransactionCode  AS [text()] ";
            sql += "FROM tbl_SmallPackage ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [MaVanDon] FROM tbl_MainOder ST2) se on se.ID=mo.ID ";
            sql += "Left outer join ";
            sql += " (SELECT DISTINCT ST2.ID, SUBSTRING((SELECT ',' + Cast(ST1.ID as nvarchar(Max))  AS [text()] FROM tbl_MainOrderCode ST1 WHERE ST1.MainOrderID = ST2.ID FOR XML PATH('')), 2, 1000)[MaDonHang] FROM tbl_MainOder ST2) sw on sw.ID = mo.ID ";
            sql += "Left outer join ";
            sql += " (SELECT DISTINCT ST2.ID, SUBSTRING((SELECT ',' + ST1.MainOrderCode   AS [text()] FROM tbl_MainOrderCode ST1 WHERE ST1.MainOrderID = ST2.ID FOR XML PATH('')), 2, 1000)[MaDonHangString] FROM tbl_MainOder ST2) sz on sz.ID = mo.ID ";
            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.title_origin  AS [text()] ";
            sql += "FROM tbl_Order ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [TenSanPham] FROM tbl_MainOder ST2) sp on sp.ID=mo.ID ";
            sql += "Where mo.IsHidden = 0 and mo.OrderType = '" + OrderType + "'";

            if (RoleID == 3)
            {
                sql += "    AND mo.Status != 1 and mo.DathangID = " + StaffID + "";
            }
            else if (RoleID == 4)
            {
                sql += "    AND mo.Status >= 5 and mo.Status < 7";
            }
            else if (RoleID == 5)
            {
                sql += "    AND mo.Status >= 5 and mo.Status <= 7";
            }
            else if (RoleID == 6)
            {
                sql += "    AND mo.Status != 1 and mo.SalerID = " + StaffID + "";
            }
            else if (RoleID == 8)
            {
                sql += "    AND mo.Status >= 9 and mo.Status < 10";
            }
            else if (RoleID == 7)
            {
                sql += "    AND mo.Status >= 2";
            }
            if (PTVC > 0)
            {
                sql += "    AND mo.ShippingType = " + PTVC + " ";
            }
            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 1)
                    sql += " And mo.ID like N'%" + searchtext + "%'";

                if (Type == 2)
                    sql += " And sz.MaDonHangString like N'%" + searchtext + "%'";

                if (Type == 3)
                    sql += " And se.MaVanDon like N'%" + searchtext + "%'";

                if (Type == 4)
                    sql += "  AND u.Email like N'%" + searchtext + "%'";

                if (Type == 5)
                    sql += " And u.Username like N'%" + searchtext + "%'";

                if (Type == 6)
                    sql += " And ui.Phone like N'%" + searchtext + "%'";

                if (Type == 7)
                    sql += "  AND sp.TenSanPham like N'%" + searchtext + "%'";
            }
            if (!string.IsNullOrEmpty(st))
            {
                if (st != "-1")
                    sql += " AND mo.Status in (" + st + ")";
            }
            if (!string.IsNullOrEmpty(mvd))
            {
                sql += " And se.MaVanDon like N'%" + mvd + "%'";
            }
            if (!string.IsNullOrEmpty(mdh))
            {
                sql += " And sz.MaDonHangString like N'%" + mdh + "%'";
            }


            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            if (!string.IsNullOrEmpty(priceFrom))
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            }
            if (!string.IsNullOrEmpty(priceTo) && !string.IsNullOrEmpty(priceFrom))
            {
                if (Convert.ToDouble(priceFrom) <= Convert.ToDouble(priceTo))
                    sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            }
            if (isNotCode == true)
            {
                sql += " AND sm.totalSmallPackages is null";
            }

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                int Status = 0;
                int MainOrderID = reader["ID"].ToString().ToInt(0);
                var entity = new OrderGetSQL();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = MainOrderID;
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["PriceVND"] != DBNull.Value)
                    entity.PriceVND = reader["PriceVND"].ToString();
                if (reader["CurrentCNYVN"] != DBNull.Value)
                    entity.Currency = reader["CurrentCNYVN"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]).ToString("dd/MM/yyyy HH:mm");
                if (reader["Status"] != DBNull.Value)
                {
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                    Status = Convert.ToInt32(reader["Status"].ToString());
                }
                if (reader["statusstring"] != DBNull.Value)
                {
                    entity.statusstring = reader["statusstring"].ToString();
                }
                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                if (reader["OrderTransactionCode2"] != DBNull.Value)
                    entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                if (reader["OrderTransactionCode3"] != DBNull.Value)
                    entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                if (reader["OrderTransactionCode4"] != DBNull.Value)
                    entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                if (reader["OrderTransactionCode5"] != DBNull.Value)
                    entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();
                if (reader["Uname"] != DBNull.Value)
                    entity.Uname = reader["Uname"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();

                if (reader["SalerID"] != DBNull.Value)
                    entity.SalerID = Convert.ToInt32(reader["SalerID"].ToString());
                if (reader["DathangID"] != DBNull.Value)
                    entity.DathangID = Convert.ToInt32(reader["DathangID"].ToString());

                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();

                if (reader["MaDonHang"] != DBNull.Value)
                    entity.listMainOrderCode = reader["MaDonHang"].ToString().Split(',').ToList();

                if (reader["IsDoneSmallPackage"] != DBNull.Value)
                    entity.IsDoneSmallPackage = Convert.ToBoolean(reader["IsDoneSmallPackage"].ToString());

                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                else
                    entity.IsCheckNotiPrice = false;

                if (reader["OrderDone"] != DBNull.Value)
                    entity.OrderDone = Convert.ToBoolean(reader["OrderDone"]);
                else
                    entity.OrderDone = false;

                if (reader["hasSmallpackage"] != DBNull.Value)
                    entity.hasSmallpackage = reader["hasSmallpackage"].ToString();

                if (Status == 0)
                {
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.Created = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn mới:</span><span>" + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["CreatedDate"] != DBNull.Value)
                        entity.Created = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn mới:</span><span>" + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CreatedDate"].ToString())) + "</span> </p>";
                }

                if (Status == 2)
                {
                    if (reader["DepostiDate"] != DBNull.Value)
                        entity.DepostiDate = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn đã cọc:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DepostiDate"] != DBNull.Value)
                        entity.DepostiDate = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn đã cọc:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DepostiDate"].ToString())) + "</span> </p>";
                }

                if (Status == 3)
                {
                    if (reader["DateShipper"] != DBNull.Value)
                        entity.DateShipper = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn người bán giao:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateShipper"] != DBNull.Value)
                        entity.DateShipper = "<p class=\"s-txt no-wrap \"><span class=\"mg\">Đơn người bán giao:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateShipper"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateShipper"].ToString())) + "</span> </p>";
                }

                if (Status == 4)
                {
                    if (reader["DateBuy"] != DBNull.Value)
                        entity.DateBuy = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn chờ mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateBuy"] != DBNull.Value)
                        entity.DateBuy = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn chờ mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuy"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuy"].ToString())) + "</span> </p>";
                }

                if (Status == 5)
                {
                    if (reader["DateBuyOK"] != DBNull.Value)
                        entity.DateBuyOK = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn đã mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateBuyOK"] != DBNull.Value)
                        entity.DateBuyOK = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn đã mua hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateBuyOK"].ToString())) + "</span> </p>";
                }

                if (Status == 6)
                {
                    if (reader["DateTQ"] != DBNull.Value)
                        entity.DateTQ = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Kho Trung Quốc nhận hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateTQ"] != DBNull.Value)
                        entity.DateTQ = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Kho Trung Quốc nhận hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateTQ"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateTQ"].ToString())) + "</span> </p>";
                }

                if (Status == 7)
                {
                    if (reader["DateToVN"] != DBNull.Value)
                        entity.DateToVN = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Trên đường về Việt Nam:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToVN"] != DBNull.Value)
                        entity.DateToVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trên đường về Việt Nam:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToVN"].ToString())) + "</span> </p>";
                }

                if (Status == 8)
                {
                    if (reader["DateVN"] != DBNull.Value)
                        entity.DateVN = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Trong kho Hà Nội:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateVN"] != DBNull.Value)
                        entity.DateVN = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Trong kho Hà Nội:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateVN"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateVN"].ToString())) + "</span> </p>";
                }

                if (Status == 9)
                {
                    if (reader["PayDate"] != DBNull.Value)
                        entity.DatePay = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đã thanh toán:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["PayDate"] != DBNull.Value)
                        entity.DatePay = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã thanh toán:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["PayDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["PayDate"].ToString())) + "</span> </p>";
                }

                if (Status == 10)
                {
                    if (reader["CompleteDate"] != DBNull.Value)
                        entity.CompleteDate = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đã hoàn thành:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["CompleteDate"] != DBNull.Value)
                        entity.CompleteDate = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đã hoàn thành:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["CompleteDate"].ToString())) + "</span> </p>";
                }

                if (Status == 11)
                {
                    if (reader["DateToShip"] != DBNull.Value)
                        entity.DateToShip = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đang giao hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToShip"] != DBNull.Value)
                        entity.DateToShip = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đang giao hàng:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToShip"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToShip"].ToString())) + "</span> </p>";
                }

                if (Status == 12)
                {
                    if (reader["DateToCancel"] != DBNull.Value)
                        entity.DateToCancel = "<p class=\"s-txt no-wrap red-text\"><span class=\"mg\">Đơn khiếu nại:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + "</span> </p>";
                }
                else
                {
                    if (reader["DateToCancel"] != DBNull.Value)
                        entity.DateToCancel = "<p class=\"s-txt no-wrap\"><span class=\"mg\">Đơn khiếu nại:</span><span> " + string.Format("{0:HH:mm}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + " - " + string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(reader["DateToCancel"].ToString())) + "</span> </p>";
                }

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static int GetTotalNewOfDK(int UID, string searchtext, int Type, string fd, string td, string priceFrom, string priceTo, string st, bool isNotCode, int RoleID, int ID)
        {
            int a = 0;
            var sql = @"select Total=Count(*) ";
            sql += "FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN ";
            sql += "(SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage) sm ON sm.MainOrderID = mo.ID and totalSmallPackages = 1 LEFT OUTER JOIN ";
            sql += "(SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1 ";
            sql += "LEFT OUTER JOIN(SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";

            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.OrderTransactionCode  AS [text()] ";
            sql += "FROM tbl_SmallPackage ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [MaVanDon] FROM tbl_MainOder ST2) se on se.ID=mo.ID ";

            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.title_origin  AS [text()] ";
            sql += "FROM tbl_Order ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [TenSanPham] FROM tbl_MainOder ST2) sp on sp.ID=mo.ID ";

            sql += "Where mo.IsHidden = 0 and UID = '" + UID + "'";

            if (RoleID == 6)
            {
                sql += "    AND mo.SalerID = " + ID + "";
            }
            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 3)
                {
                    sql += "  AND sp.TenSanPham like N'%" + searchtext + "%'";
                }
                if (Type == 2)
                    sql += " And se.MaVanDon like N'%" + searchtext + "%'";

                if (Type == 1)
                    sql += " And mo.ID like N'%" + searchtext + "%'";

            }
            if (!string.IsNullOrEmpty(st))
            {
                sql += " AND mo.Status in (" + st + ")";
            }

            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            if (!string.IsNullOrEmpty(priceFrom))
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            }
            if (!string.IsNullOrEmpty(priceTo) && !string.IsNullOrEmpty(priceFrom))
            {
                if (Convert.ToDouble(priceFrom) <= Convert.ToDouble(priceTo))
                    sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            }
            if (isNotCode == true)
            {
                sql += " AND sm.totalSmallPackages is null";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            return a;
        }

        public static List<OrderGetSQL> GetByUserIDInSQLHelperWithFilterNew(int UID, string searchtext, int Type, string fd, string td, string priceFrom, string priceTo, string st, bool isNotCode, int pageIndex, int pageSize, int RoleID, int ID)
        {
            var list = new List<OrderGetSQL>();
            var sql = @"SELECT mo.ID, mo.TotalPriceVND,se.MaVanDon,sp.TenSanPham, mo.Deposit, mo.CreatedDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, ";
            sql += "CASE mo.Status WHEN 0 THEN N'<span class=\"badge red darken-2 white-text border-radius-2\">Đơn mới</span>' ";
            sql += "WHEN 1 THEN N'<span class=\"badge black darken-2 white-text border-radius-2\">Đơn hàng hủy</span>' ";
            sql += "WHEN 2 THEN N'<span class=\"badge pink darken-2 white-text border-radius-2\">Đơn đã cọc</span>' ";
            sql += "WHEN 3 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đơn người bán giao</span>' ";
            sql += "WHEN 4 THEN N'<span class=\"badge green darken-2 white-text border-radius-2\">Đơn chờ mua hàng</span>' ";
            sql += "WHEN 5 THEN N'<span class=\"badge green darken-2 white-text border-radius-2\">Đã mua hàng</span>' ";
            sql += "WHEN 6 THEN N'<span class=\"badge green darken-2 white-text border-radius-2\">Kho Trung Quốc nhận hàng</span>' ";
            sql += "WHEN 7 THEN N'<span class=\"badge orange darken-2 white-text border-radius-2\">Trên đường về Việt Nam</span>' ";
            sql += "WHEN 8 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Trong kho Hà Nội</span>' ";
            sql += "WHEN 9 THEN N'<span class=\"badge blue darken-2 white-text border-radius-2\">Khách đã thanh toán</span>' ";
            sql += "WHEN 11 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đang giao hàng</span>' ";
            sql += "WHEN 12 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đơn khiếu nại</span>' ";
            sql += "ELSE N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã hoàn thành</span>' ";
            sql += "END AS statusstring, mo.DathangID,  ";
            sql += "mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4,  ";
            sql += "mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, sm.totalSmallPackages,  ";
            sql += "mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4,  ";
            sql += "mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang,  ";
            sql += "CASE WHEN o.anhsanpham IS NULL THEN '' ELSE '<img alt=\"\" style=\"max-width:125px;max-height:125px\" src=\"' + o.anhsanpham + '\">' END AS anhsanpham, ";
            sql += "CASE WHEN mo.IsDoneSmallPackage = 1 THEN N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã đủ</span>'  WHEN a.countrow > 0 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đã nhập</span>' ELSE N'<span class=\"badge red darken-2 white-text border-radius-2\">Chưa nhập</span>' END AS hasSmallpackage ";
            sql += "FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN ";
            sql += "(SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage) sm ON sm.MainOrderID = mo.ID and totalSmallPackages = 1 LEFT OUTER JOIN ";
            sql += "(SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1 ";

            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.OrderTransactionCode  AS [text()] ";
            sql += "FROM tbl_SmallPackage ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [MaVanDon] FROM tbl_MainOder ST2) se on se.ID=mo.ID ";

            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.title_origin  AS [text()] ";
            sql += "FROM tbl_Order ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [TenSanPham] FROM tbl_MainOder ST2) sp on sp.ID=mo.ID ";

            sql += "LEFT OUTER JOIN(SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
            sql += "Where mo.IsHidden = 0 and UID = '" + UID + "'";

            if (RoleID == 6)
            {
                sql += "    AND mo.SalerID = " + ID + "";
            }

            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 3)
                {
                    sql += "  AND sp.TenSanPham like N'%" + searchtext + "%'";
                }
                if (Type == 2)
                    sql += " And se.MaVanDon like N'%" + searchtext + "%'";

                if (Type == 1)
                    sql += " And mo.ID like N'%" + searchtext + "%'";

            }
            if (!string.IsNullOrEmpty(st))
            {
                sql += " AND mo.Status in (" + st + ")";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            if (!string.IsNullOrEmpty(priceFrom))
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            }
            if (!string.IsNullOrEmpty(priceTo) && !string.IsNullOrEmpty(priceFrom))
            {
                if (Convert.ToDouble(priceFrom) <= Convert.ToDouble(priceTo))
                    sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            }
            if (isNotCode == true)
            {
                sql += " AND sm.totalSmallPackages is null";
            }
            sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                int MainOrderID = reader["ID"].ToString().ToInt(0);
                var entity = new OrderGetSQL();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = MainOrderID;
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]).ToString("dd/MM/yyyy HH:mm");
                if (reader["Status"] != DBNull.Value)
                {
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                }
                if (reader["statusstring"] != DBNull.Value)
                {
                    entity.statusstring = reader["statusstring"].ToString();
                }
                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                if (reader["OrderTransactionCode2"] != DBNull.Value)
                    entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                if (reader["OrderTransactionCode3"] != DBNull.Value)
                    entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                if (reader["OrderTransactionCode4"] != DBNull.Value)
                    entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                if (reader["OrderTransactionCode5"] != DBNull.Value)
                    entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();
                if (reader["Uname"] != DBNull.Value)
                    entity.Uname = reader["Uname"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();
                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();
                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                else
                    entity.IsCheckNotiPrice = false;
                if (reader["hasSmallpackage"] != DBNull.Value)
                    entity.hasSmallpackage = reader["hasSmallpackage"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }


        public static List<OrderGetSQL> GetByUserIDInSQLHelperWithFilterOrderListNew(int OrderType, string searchtext, int Type, string fd, string td, string priceFrom, string priceTo, string st, bool isNotCode, int pageIndex, int pageSize)
        {
            var list = new List<OrderGetSQL>();
            var sql = @"SELECT mo.ID, mo.TotalPriceVND,mo.IsDoneSmallPackage,sz.MaDonHangString,se.MaVanDon,sw.MaDonHang,sp.TenSanPham, mo.Deposit, mo.CreatedDate, mo.Status, mo.OrderType, mo.IsCheckNotiPrice, ";
            sql += "CASE mo.Status WHEN 0 THEN N'<span class=\"badge red darken-2 white-text border-radius-2\">Chờ Đơn đã cọc</span>' ";
            sql += "WHEN 1 THEN N'<span class=\"badge black darken-2 white-text border-radius-2\">Hủy đơn hàng</span>' ";
            sql += "WHEN 2 THEN N'<span class=\"badge pink darken-2 white-text border-radius-2\">Khách đã Đơn đã cọc</span>' ";
            sql += "WHEN 3 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Chờ duyệt đơn</span>' ";
            sql += "WHEN 4 THEN N'<span class=\"badge green darken-2 white-text border-radius-2\">Đã duyệt đơn</span>' ";
            sql += "WHEN 5 THEN N'<span class=\"badge green darken-2 white-text border-radius-2\">Đã mua hàng</span>' ";
            sql += "WHEN 6 THEN N'<span class=\"badge green darken-2 white-text border-radius-2\">Kho Trung Quốc nhận hàng</span>' ";
            sql += "WHEN 7 THEN N'<span class=\"badge orange darken-2 white-text border-radius-2\">Trên đường về Việt Nam</span>' ";
            sql += "WHEN 8 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Chờ thanh toán</span>' ";
            sql += "WHEN 9 THEN N'<span class=\"badge blue darken-2 white-text border-radius-2\">Khách đã thanh toán</span>' ";
            sql += "ELSE N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã hoàn thành</span>' ";
            sql += "END AS statusstring, mo.DathangID,  ";
            sql += "mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4,  ";
            sql += "mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang, sm.totalSmallPackages,  ";
            sql += "mo.SalerID, mo.OrderTransactionCode, mo.OrderTransactionCode2, mo.OrderTransactionCode3, mo.OrderTransactionCode4,  ";
            sql += "mo.OrderTransactionCode5, u.Username AS Uname, s.Username AS saler, d.Username AS dathang,  ";
            sql += "CASE WHEN o.anhsanpham IS NULL THEN '' ELSE '<img alt=\"\" style=\"max-width:125px;max-height:125px\" src=\"' + o.anhsanpham + '\">' END AS anhsanpham, ";
            sql += "CASE WHEN mo.IsDoneSmallPackage = 1 THEN N'<span class=\"badge blue darken-2 white-text border-radius-2\">Đã đủ</span>'  WHEN a.countrow > 0 THEN N'<span class=\"badge yellow darken-2 white-text border-radius-2\">Đã nhập</span>' ELSE N'<span class=\"badge red darken-2 white-text border-radius-2\">Chưa nhập</span>' END AS hasSmallpackage ";
            sql += "FROM    dbo.tbl_MainOder AS mo LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS u ON mo.UID = u.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS s ON mo.SalerID = s.ID LEFT OUTER JOIN ";
            sql += "dbo.tbl_Account AS d ON mo.DathangID = d.ID LEFT OUTER JOIN ";
            sql += "(SELECT MainOrderID, OrderTransactionCode, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS totalSmallPackages FROM tbl_smallpackage) sm ON sm.MainOrderID = mo.ID and totalSmallPackages = 1 LEFT OUTER JOIN ";
            sql += "(SELECT  image_origin as anhsanpham, MainOrderID, ROW_NUMBER() OVER(PARTITION BY MainOrderID ORDER BY(SELECT NULL)) AS RowNum FROM tbl_Order) o ON o.MainOrderID = mo.ID And RowNum = 1 ";
            sql += "LEFT OUTER JOIN(SELECT count(*) AS countRow, MainOrderID  FROM tbl_SmallPackage AS a  GROUP BY a.MainOrderID) AS a ON a.MainOrderID = mo.ID ";
            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.OrderTransactionCode  AS [text()] ";
            sql += "FROM tbl_SmallPackage ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [MaVanDon] FROM tbl_MainOder ST2) se on se.ID=mo.ID ";
            sql += "Left outer join ";
            sql += " (SELECT DISTINCT ST2.ID, SUBSTRING((SELECT ',' + Cast(ST1.ID as nvarchar(Max))  AS [text()] FROM tbl_MainOrderCode ST1 WHERE ST1.MainOrderID = ST2.ID FOR XML PATH('')), 2, 1000)[MaDonHang] FROM tbl_MainOder ST2) sw on sw.ID = mo.ID ";
            sql += "Left outer join ";
            sql += " (SELECT DISTINCT ST2.ID, SUBSTRING((SELECT ',' + ST1.MainOrderCode   AS [text()] FROM tbl_MainOrderCode ST1 WHERE ST1.MainOrderID = ST2.ID FOR XML PATH('')), 2, 1000)[MaDonHangString] FROM tbl_MainOder ST2) sz on sz.ID = mo.ID ";
            sql += "Left outer join ( ";
            sql += "SELECT DISTINCT ST2.ID,SUBSTRING((SELECT ','+ST1.title_origin  AS [text()] ";
            sql += "FROM tbl_Order ST1 ";
            sql += "WHERE ST1.MainOrderID = ST2.ID ";
            sql += "FOR XML PATH ('') ";
            sql += "), 2, 1000) [TenSanPham] FROM tbl_MainOder ST2) sp on sp.ID=mo.ID ";
            sql += "Where mo.IsHidden = 0 and mo.OrderType = '" + OrderType + "'";
            if (!string.IsNullOrEmpty(searchtext))
            {
                if (Type == 3)
                {
                    sql += "  AND sp.TenSanPham like N'%" + searchtext + "%'";
                }
                if (Type == 2)
                    sql += " And se.MaVanDon like N'%" + searchtext + "%'";

                if (Type == 1)
                    sql += " And sz.MaDonHangString like N'%" + searchtext + "%'";
            }
            if (!string.IsNullOrEmpty(st))
            {
                sql += " AND mo.Status in (" + st + ")";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            if (!string.IsNullOrEmpty(priceFrom))
            {
                sql += " AND CAST(mo.TotalPriceVND AS float)  >= " + priceFrom;
            }
            if (!string.IsNullOrEmpty(priceTo) && !string.IsNullOrEmpty(priceFrom))
            {
                if (Convert.ToDouble(priceFrom) <= Convert.ToDouble(priceTo))
                    sql += " AND CAST(mo.TotalPriceVND AS float)  <= " + priceTo;
            }
            if (isNotCode == true)
            {
                sql += " AND sm.totalSmallPackages is null";
            }
            sql += " ORDER BY mo.ID DESC OFFSET (" + pageIndex + " * " + pageSize + ") ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                int MainOrderID = reader["ID"].ToString().ToInt(0);
                var entity = new OrderGetSQL();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = MainOrderID;
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString();
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]).ToString("dd/MM/yyyy HH:mm");
                if (reader["Status"] != DBNull.Value)
                {
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());
                }
                if (reader["statusstring"] != DBNull.Value)
                {
                    entity.statusstring = reader["statusstring"].ToString();
                }
                if (reader["OrderTransactionCode"] != DBNull.Value)
                    entity.OrderTransactionCode = reader["OrderTransactionCode"].ToString();
                if (reader["OrderTransactionCode2"] != DBNull.Value)
                    entity.OrderTransactionCode2 = reader["OrderTransactionCode2"].ToString();
                if (reader["OrderTransactionCode3"] != DBNull.Value)
                    entity.OrderTransactionCode3 = reader["OrderTransactionCode3"].ToString();
                if (reader["OrderTransactionCode4"] != DBNull.Value)
                    entity.OrderTransactionCode4 = reader["OrderTransactionCode4"].ToString();
                if (reader["OrderTransactionCode5"] != DBNull.Value)
                    entity.OrderTransactionCode5 = reader["OrderTransactionCode5"].ToString();
                if (reader["Uname"] != DBNull.Value)
                    entity.Uname = reader["Uname"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();
                if (reader["anhsanpham"] != DBNull.Value)
                    entity.anhsanpham = reader["anhsanpham"].ToString();

                if (reader["MaDonHang"] != DBNull.Value)
                    entity.listMainOrderCode = reader["MaDonHang"].ToString().Split(',').ToList();

                if (reader["IsDoneSmallPackage"] != DBNull.Value)
                    entity.IsDoneSmallPackage = Convert.ToBoolean(reader["IsDoneSmallPackage"].ToString());

                if (reader["OrderType"] != DBNull.Value)
                    entity.OrderType = reader["OrderType"].ToString().ToInt(1);
                if (reader["IsCheckNotiPrice"] != DBNull.Value)
                    entity.IsCheckNotiPrice = Convert.ToBoolean(reader["IsCheckNotiPrice"]);
                else
                    entity.IsCheckNotiPrice = false;
                if (reader["hasSmallpackage"] != DBNull.Value)
                    entity.hasSmallpackage = reader["hasSmallpackage"].ToString();


                list.Add(entity);
            }
            reader.Close();
            return list;
        }


        public static List<tbl_MainOder> GetAllByUID_Deposit(int UID, int type)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.UID == UID && o.Status == 0 && o.OrderType == type && o.IsHidden == false).OrderBy(t => t.CreatedDate).ToList();
                return lo;
            }
        }

        public static List<tbl_MainOder> GetAllByUID_Payall(int UID, int type)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_MainOder> lo = new List<tbl_MainOder>();
                lo = dbe.tbl_MainOder.Where(o => o.UID == UID && o.Status > 6 && o.OrderType == type && o.IsHidden == false).OrderBy(t => t.CreatedDate).ToList();
                return lo;
            }
        }


        public static List<SQLsumtotal> GetByUsernameInSQLHelper_Sumtotal(int UID)
        {
            var list = new List<SQLsumtotal>();
            var sql = @"select sum(CONVERT(float,TotalPriceVND)) tongtienhang";
            sql += ",sum(convert(float,Deposit )) tongtiencoc";
            sql += " from tbl_MainOder";
            sql += " where IsHidden = 0 and Status != 1 and UID = " + UID + "";

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new SQLsumtotal();
                if (reader["tongtienhang"] != DBNull.Value)
                    entity.tongtienhang = reader["tongtienhang"].ToString();
                if (reader["tongtiencoc"] != DBNull.Value)
                    entity.tongtiencoc = reader["tongtiencoc"].ToString();

                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public class SQLsumtotal
        {
            public int ID { get; set; }
            public int UID { get; set; }
            public string tongtienhang { get; set; }
            public string tongtiencoc { get; set; }
            public string ShopName { get; set; }
            public string TotalPriceVND { get; set; }
        }

        #endregion

        public static List<IncomSaler> GetFromDateToDate_IncomSaler_Thang_total(string fd, string td, string Username, string Saler)
        {
            var list = new List<IncomSaler>();

            var sql = @"select  ac.Username as Saler, ac.ID as SalerID , COUNT(ac.ID) OVER() AS TotalRow ";
            sql += ", Sum(convert(float,mo.TotalPriceVND)) as TotalPriceVND ";
            sql += ", Sum(convert(float,mo.PriceVND)) as PriceVND ";
            sql += ", Sum(convert(float,mo.FeeBuyPro)) as FeeBuyPro ";
            sql += ", Sum(convert(float,mo.FeeShipCN)) as FeeShipCN ";
            sql += ", Sum(convert(float,mo.PriceVND)+(convert(float,mo.FeeShipCN))) as giatridonhang ";
            sql += ", Sum(convert(float,mo.FeeBuyPro)+(convert(float,mo.FeeShipCN))+(convert(float,mo.FeeWeight))) as phidonhang ";
            sql += ", Sum((convert(float,mo.PriceVND) +(convert(float,mo.FeeShipCN)))-(convert(float,mo.TotalPriceReal))) as tienmacca ";
            //sql += ", Sum(convert(float,mo.TotalPriceVND)-((convert(float,mo.FeeShipCN))+(convert(float,mo.TotalPriceReal)))) as tienmacca ";
            sql += ", Sum(convert(float,mo.TotalPriceReal)) as TotalPriceReal ";
            sql += ", Sum(convert(float,mo.TQVNWeight)) as TQVNWeight ";
            sql += ", Sum(convert(float,mo.FeeWeight)) as FeeWeight, Count(mo.ID) as TotalOrder from tbl_MainOder as mo, ";
            sql += "tbl_Account as ac, ";
            sql += "tbl_Account as kh ";
            sql += "where mo.IsHidden = 0 and mo.SalerID = ac.ID and mo.UID = kh.ID and mo.Status > 4 ";

            if (!string.IsNullOrEmpty(Username))
                sql += " AND ac.Username = '" + Username + "'";
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += " group by  ac.Username , ac.ID ";





            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int i = 1;
            while (reader.Read())
            {
                var entity = new IncomSaler();
                #region code mới

                if (reader["SalerID"] != DBNull.Value)
                    entity.SalerID = reader["SalerID"].ToString().ToInt(0);

                if (reader["Saler"] != DBNull.Value)
                    entity.Saler = reader["Saler"].ToString();
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["PriceVND"] != DBNull.Value)
                    entity.PriceVND = reader["PriceVND"].ToString();
                if (reader["FeeBuyPro"] != DBNull.Value)
                    entity.FeeBuyPro = reader["FeeBuyPro"].ToString();
                if (reader["FeeShipCN"] != DBNull.Value)
                    entity.FeeShipCN = reader["FeeShipCN"].ToString();
                if (reader["giatridonhang"] != DBNull.Value)
                    entity.giatridonhang = reader["giatridonhang"].ToString();
                if (reader["phidonhang"] != DBNull.Value)
                    entity.phidonhang = reader["phidonhang"].ToString();
                if (reader["tienmacca"] != DBNull.Value)
                    entity.tienmacca = reader["tienmacca"].ToString();
                if (reader["TotalPriceReal"] != DBNull.Value)
                    entity.TotalPriceReal = reader["TotalPriceReal"].ToString();
                if (reader["TQVNWeight"] != DBNull.Value)
                    entity.TQVNWeight = reader["TQVNWeight"].ToString();
                if (reader["FeeWeight"] != DBNull.Value)
                    entity.FeeWeight = reader["FeeWeight"].ToString();
                if (reader["TotalOrder"] != DBNull.Value)
                    entity.TotalOrder = reader["TotalOrder"].ToString();



                i++;
                list.Add(entity);

                #endregion
            }
            reader.Close();
            return list;
        }

        public class IncomSaler
        {
            public string Username { get; set; }
            public string Saler { get; set; }
            public string Dealer { get; set; }
            public int SalerID { get; set; }
            public int DathangID { get; set; }
            public string TotalPriceVND { get; set; }
            public string PriceVND { get; set; }
            public string FeeBuyPro { get; set; }
            public string TotalPriceRealCYN { get; set; }
            public string FeeShipCN { get; set; }
            public string giatridonhang { get; set; }
            public string phidonhang { get; set; }
            public string tienmacca { get; set; }
            public string TotalPriceReal { get; set; }
            public string TQVNWeight { get; set; }
            public string FeeWeight { get; set; }
            public string TotalOrder { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime DateBuy { get; set; }
            public int TotalRow { get; set; }
        }

        public static List<IncomSaler> GetFromDateToDate_IncomSaler_total(string fd, string td, string Username, string Saler, int pageIndex, int pageSize)
        {
            var list = new List<IncomSaler>();

            var sql = @"select  ac.Username as Saler, ac.ID as SalerID , COUNT(ac.ID) OVER() AS TotalRow ";
            sql += ", Sum(convert(float,mo.TotalPriceVND)) as TotalPriceVND ";
            sql += ", Sum(convert(float,mo.PriceVND)) as PriceVND ";
            sql += ", Sum(convert(float,mo.FeeBuyPro)) as FeeBuyPro ";
            sql += ", Sum(convert(float,mo.FeeShipCN)) as FeeShipCN ";
            sql += ", Sum(convert(float,mo.PriceVND)+(convert(float,mo.FeeShipCN))) as giatridonhang ";
            sql += ", Sum(convert(float,mo.FeeBuyPro)+(convert(float,mo.FeeShipCN))+(convert(float,mo.FeeWeight))) as phidonhang ";
            sql += ", Sum((convert(float,mo.PriceVND) +(convert(float,mo.FeeShipCN)))-(convert(float,mo.TotalPriceReal))) as tienmacca ";
            sql += ", Sum(convert(float,mo.TotalPriceReal)) as TotalPriceReal ";
            sql += ", Sum(convert(float,mo.TQVNWeight)) as TQVNWeight ";
            sql += ", Sum(convert(float,mo.FeeWeight)) as FeeWeight, Count(mo.ID) as TotalOrder from tbl_MainOder as mo, ";
            sql += "tbl_Account as ac, ";
            sql += "tbl_Account as kh ";
            sql += "where mo.IsHidden = 0 and mo.SalerID = ac.ID and mo.UID = kh.ID and mo.Status > 4 ";

            if (!string.IsNullOrEmpty(Username))
                sql += " AND ac.Username = '" + Username + "'";

            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND mo.CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND mo.CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += " group by  ac.Username , ac.ID ";

            sql += " order by ac.ID desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";




            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int i = 1;
            while (reader.Read())
            {
                var entity = new IncomSaler();
                #region code mới

                if (reader["SalerID"] != DBNull.Value)
                    entity.SalerID = reader["SalerID"].ToString().ToInt(0);

                if (reader["Saler"] != DBNull.Value)
                    entity.Saler = reader["Saler"].ToString();
                if (reader["TotalPriceVND"] != DBNull.Value)
                    entity.TotalPriceVND = reader["TotalPriceVND"].ToString();
                if (reader["PriceVND"] != DBNull.Value)
                    entity.PriceVND = reader["PriceVND"].ToString();
                if (reader["FeeBuyPro"] != DBNull.Value)
                    entity.FeeBuyPro = reader["FeeBuyPro"].ToString();
                if (reader["FeeShipCN"] != DBNull.Value)
                    entity.FeeShipCN = reader["FeeShipCN"].ToString();
                if (reader["giatridonhang"] != DBNull.Value)
                    entity.giatridonhang = reader["giatridonhang"].ToString();
                if (reader["phidonhang"] != DBNull.Value)
                    entity.phidonhang = reader["phidonhang"].ToString();
                if (reader["tienmacca"] != DBNull.Value)
                    entity.tienmacca = reader["tienmacca"].ToString();
                if (reader["TotalPriceReal"] != DBNull.Value)
                    entity.TotalPriceReal = reader["TotalPriceReal"].ToString();
                if (reader["TQVNWeight"] != DBNull.Value)
                    entity.TQVNWeight = reader["TQVNWeight"].ToString();
                if (reader["FeeWeight"] != DBNull.Value)
                    entity.FeeWeight = reader["FeeWeight"].ToString();
                if (reader["TotalOrder"] != DBNull.Value)
                    entity.TotalOrder = reader["TotalOrder"].ToString();
                if (reader["TotalRow"] != DBNull.Value)
                    entity.TotalRow = reader["TotalRow"].ToString().ToInt(0);


                i++;
                list.Add(entity);

                #endregion
            }
            reader.Close();
            return list;
        }


        public static int CountOrderSlow(int OrderType, int RoleID, int StaffID)
        {
            int Count = 0;
            var sql = @" SELECT COUNT(*) as Total from tbl_MainOder ";
            sql += " WHERE OrderType = '" + OrderType + "' AND IsHidden=0 AND Status=5 AND DATEDIFF(day,DateBuy,getdate()) > 3 ";
            if (RoleID == 3)
            {
                sql += "    AND DathangID = " + StaffID + "";
            }
            else if (RoleID == 6)
            {
                sql += "    AND SalerID = " + StaffID + "";
            }
            else if (RoleID == 9)
            {
                sql += "    AND CSID = " + StaffID + "";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }
        public static int CountOrderSlowChina(int OrderType, int RoleID, int StaffID)
        {
            int Count = 0;
            var sql = @" SELECT COUNT(*) as Total from tbl_MainOder ";
            sql += " WHERE OrderType = '" + OrderType + "' AND IsHidden=0 AND Status=3 AND DATEDIFF(day,DateShipper,getdate()) > 6 ";
            if (RoleID == 3)
            {
                sql += "    AND DathangID = " + StaffID + "";
            }
            else if (RoleID == 6)
            {
                sql += "    AND SalerID = " + StaffID + "";
            }
            else if (RoleID == 9)
            {
                sql += "    AND CSID = " + StaffID + "";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }
        public static int CountOrderSlowOutChina(int OrderType, int RoleID, int StaffID)
        {
            int Count = 0;
            var sql = @" SELECT COUNT(*) as Total from tbl_MainOder ";
            sql += " WHERE OrderType = '" + OrderType + "' AND IsHidden=0 AND Status=6 AND DATEDIFF(day,DateTQ,getdate()) > 3 ";
            if (RoleID == 3)
            {
                sql += "    AND DathangID = " + StaffID + "";
            }
            else if (RoleID == 6)
            {
                sql += "    AND SalerID = " + StaffID + "";
            }
            else if (RoleID == 9)
            {
                sql += "    AND CSID = " + StaffID + "";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }
        public static int CountOrderSlowVN(int OrderType, int RoleID, int StaffID)
        {
            int Count = 0;
            var sql = @" SELECT COUNT(*) as Total from tbl_MainOder ";
            sql += " WHERE OrderType = '" + OrderType + "' AND IsHidden=0 AND Status=7 AND DATEDIFF(day,DateToVN,getdate()) > 6 ";
            if (RoleID == 3)
            {
                sql += "    AND DathangID = " + StaffID + "";
            }
            else if (RoleID == 6)
            {
                sql += "    AND SalerID = " + StaffID + "";
            }
            else if (RoleID == 9)
            {
                sql += "    AND CSID = " + StaffID + "";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                Count = reader["Total"].ToString().ToInt();
            }
            reader.Close();
            return Count;
        }
    }
}