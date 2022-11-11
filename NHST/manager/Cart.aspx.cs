using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userLoginSystem"] == null)
            {
                Response.Redirect("/trang-chu");
            }
            else
            {
                string username_current = Session["userLoginSystem"].ToString();
                tbl_Account ac = AccountController.GetByUsername(username_current);
                if (ac.RoleID != 0 && ac.RoleID != 2 && ac.RoleID != 6 && ac.RoleID != 9 && ac.RoleID != 11 && ac.RoleID != 12 && ac.RoleID != 13)
                    Response.Redirect("/trang-chu");
                LoadData();
            }
        }
        public void LoadData()
        {
            //ltrTitle.Text = ConfigurationController.GetByTop1().Websitename;
            var UID = Request.QueryString["i"].ToInt(0);
            if (UID > 0)
            {
                var acc = AccountController.GetByID(UID);
                if (acc != null)
                {
                    int id = acc.ID;
                    ViewState["UID"] = id;
                    ltrUsername.Text = acc.Username;
                }
            }
            var obj_user = AccountController.GetByID(UID);
            double pricecurrency = Convert.ToDouble(ConfigurationController.GetByTop1().Currency);
            if (!string.IsNullOrEmpty(obj_user.Currency.ToString()))
            {
                if (Convert.ToDouble(obj_user.Currency) > 0)
                {
                    pricecurrency = Convert.ToDouble(obj_user.Currency);
                }
            }

            List<tbl_OrderTemp> os = OrderTempController.GetAllByUID(UID);
            //DateTime checkdate = DateTime.Now;
            //foreach (var item in os)
            //{
            //    DateTime CreatedDate = Convert.ToDateTime(item.CreatedDate);
            //    TimeSpan span = checkdate.Subtract(CreatedDate);
            //    if (span.Days > 30)
            //    {
            //        OrderTempController.RemoveCartOver15days(item.ID);
            //    }
            //}
            StringBuilder html = new StringBuilder();
            double pricefullcart = 0;
            int totalproduct = 0;
            string kq = "";
            int countshop = 0;
            int tongsoluongsp = 0;
            int i = 1;
            var oshop = OrderShopTempController.GetByUID(UID);
            if (oshop != null)
            {
                //DateTime checkdate1 = DateTime.Now;
                //foreach (var item in oshop)
                //{
                //    DateTime CreatedDate = Convert.ToDateTime(item.CreatedDate);
                //    TimeSpan span = checkdate1.Subtract(CreatedDate);
                //    if (span.Days > 30)
                //    {
                //        kq = OrderShopTempController.RemoveCartOver15days(item.ID);
                //    }
                //}
                //if (kq == "ok1")
                //{
                //    PJUtils.ShowMessageBoxSwAlert("Những sản phẩm trong giỏ hàng quá 30 ngày đã bị xóa.", "s", true, Page);
                //}
                if (oshop.Count > 0)
                {
                    countshop = oshop.Count();

                    string listorderid = "";
                    foreach (var shop in oshop)
                    {
                        int totalquantity = 0;
                        double TotalPriceProductInShop = 0;
                        double TotalPriceShop = 0;
                        listorderid += shop.ID + "|";

                        #region New
                        html.Append(" <div class=\"order-item\">");
                        html.Append("<div class=\"left-info\">");
                        html.Append("<div class=\"order-hd\">");
                        html.Append("<div class=\"shop-cart\" data-item-id=\"" + shop.ID + "\">");
                        html.Append("<label class=\"select-checkout tooltipped\" data-position=\"top\" data-tooltip=\"Chọn đặt đơn hàng này \">");
                        html.Append("<input type=\"checkbox\" onclick=\"getCartToSuccess($(this))\" class=\"filled-in\" />");
                        html.Append("<span class=\"shop-name\">" + shop.ShopName + " <span class=\"green-text checked hide-on-small-and-down\">Đã thêm vào danh sách đặt hàng</span></span>");
                        html.Append("</label>");
                        html.Append("</div>");
                        html.Append("<div class=\"delete\">");
                        html.Append("<a href=\"javascript:;\" onclick=\"deleteordershoptemp('" + shop.ID + "')\" class=\"btn-delete tooltipped\" data-position=\"top\" data-tooltip=\"Xóa toàn bộ sản phẩm của cửa hàng này\"><i class=\"material-icons\">delete</i></a>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("<div class=\"order-main\">");

                        List<tbl_OrderTemp> ors = OrderTempController.GetAllByOrderShopTempIDAndUID(UID, shop.ID);
                        if (ors != null)
                        {
                            if (ors.Count > 0)
                            {
                                foreach (var item in ors)
                                {
                                    totalproduct += 1;
                                    int ID = item.ID;
                                    string linkproduct = item.link_origin;
                                    string productname = item.title_origin;
                                    string brand = item.brand;
                                    string image = item.image_origin;
                                    int quantity = Convert.ToInt32(item.quantity);
                                    totalquantity += quantity;
                                    double originprice = Math.Round(Convert.ToDouble(item.price_origin), 2);
                                    double promotionprice = Math.Round(Convert.ToDouble(item.price_promotion), 2);
                                    double u_pricecbuy = 0;
                                    double u_pricevn = 0;
                                    double e_pricebuy = 0;
                                    double e_pricevn = 0;
                                    double e_pricetemp = 0;
                                    double e_totalproduct = 0;
                                    if (promotionprice > 0)
                                    {
                                        if (promotionprice < originprice)
                                        {
                                            u_pricecbuy = promotionprice;
                                            u_pricevn = promotionprice * pricecurrency;
                                        }
                                        else
                                        {
                                            u_pricecbuy = originprice;
                                            u_pricevn = originprice * pricecurrency;
                                        }
                                    }
                                    else
                                    {
                                        u_pricecbuy = originprice;
                                        u_pricevn = originprice * pricecurrency;
                                    }
                                    e_pricebuy = u_pricecbuy * quantity;
                                    e_pricevn = u_pricevn * quantity;

                                    e_totalproduct = e_pricevn + e_pricetemp;

                                    TotalPriceProductInShop += Math.Round(e_pricevn, 0);
                                    TotalPriceShop += Math.Round(e_totalproduct, 0);

                                    //pricefullcart += e_totalproduct;

                                    if (image.Contains("%2F"))
                                    {
                                        image = image.Replace("%2F", "/");
                                    }
                                    if (image.Contains("%3A"))
                                    {
                                        image = image.Replace("%3A", ":");
                                    }

                                    html.Append("<div class=\"item-wrap\">");
                                    html.Append("<div class=\"item-name\">");
                                    html.Append("<div class=\"number\">");
                                    html.Append("<span class=\"count\">" + i + "</span>");
                                    html.Append("</div>");
                                    html.Append("<div class=\"name\">");
                                    html.Append("<span class=\"item-img\">");
                                    html.Append("<img src=\"" + image + "\" alt=\"image\">");
                                    html.Append("</span>");
                                    html.Append("<div class=\"caption\">");
                                    html.Append("<a href=\"" + linkproduct + "\" target=\"_blank\" class=\"title black-text\">" + productname + "</a>");
                                    html.Append("<div class=\"item-price mt-1\">");
                                    html.Append("<span class=\"pr-2 black-text font-weight-600\">" + item.property + "</span>");
                                    html.Append("</div>");
                                    html.Append("<div class=\"note\">");
                                    html.Append("<span class=\"black-text font-weight-500\">Ghi chú: </span>");
                                    html.Append("<div class=\"input-field inline\">");
                                    html.Append("<input type=\"text\" id=\"note_" + ID + "\" value=\"" + brand + "\" data-item-id=\"" + ID + "\" class=\"validate notebrand\">");
                                    html.Append("</div>");
                                    html.Append("</div>");
                                    html.Append("</div>");
                                    html.Append("</div>");
                                    html.Append("</div>");
                                    html.Append("<div class=\"item-info\">");
                                    html.Append("<div class=\"separator\"></div>");
                                    html.Append("<div class=\"item-num column\">");
                                    html.Append("<span class=\"black-text\"><strong>Số lượng</strong></span>");
                                    html.Append("<div class=\"up-downControl form-style form-control\" data-step=\"1\" data-min=\"0\">");
                                    html.Append("<a href=\"javascript:;\" class=\"btn minus\">-</a>");
                                    html.Append("<div class=\"value-group fcontrol\">");
                                    html.Append("<input type=\"text\"  placeholder=\"0\" value=\"" + quantity + "\" min=\"1\" class=\"value quantitiofpro\">");
                                    html.Append("</div>");
                                    html.Append("<a href=\"javascript:;\" class=\"btn plus\">+</a>");
                                    html.Append("</div>");
                                    html.Append("<p></p>");
                                    html.Append("</div>");
                                    html.Append("<div class=\"separator\"></div>");
                                    html.Append("<div class=\"item-price column\">");
                                    html.Append("<span class=\"black-text\"><strong>Đơn giá</strong></span>");
                                    html.Append("<p class=\"red-text\">¥" + u_pricecbuy + "</p>");
                                    html.Append("<p class=\"red-text\">" + string.Format("{0:N0}", u_pricevn) + " VNĐ</p>");
                                    html.Append("</div>");
                                    html.Append("<div class=\"separator\"></div>");
                                    html.Append("<div class=\"item-price column\">");
                                    html.Append("<span class=\"black-text\"><strong>Tiền hàng</strong></span>");
                                    html.Append("<p class=\"red-text\">¥" + e_pricebuy + "</p>");
                                    html.Append("<p class=\"red-text\">" + string.Format("{0:N0}", e_pricevn) + " VNĐ</p>");
                                    html.Append("</div>");
                                    html.Append("<div class=\"separator\"></div>");
                                    html.Append("<div class=\"delete\">");
                                    html.Append("<a href=\"javascript:;\" onclick=\"updatequantity('" + ID + "',$(this),'" + shop.ID + "')\" class=\"btn-update tooltipped\" data-position=\"top\" data-tooltip=\"Cập nhật sản phẩm này\"><i class=\"material-icons\">sync</i></a>");
                                    html.Append("<a href=\"javascript:;\" onclick=\"deleteordertemp('" + ID + "','" + shop.ID + "')\" class=\"btn-delete tooltipped\" data-position=\"top\" data-tooltip=\"Xóa sản phẩm này\"><i class=\"material-icons \">delete</i></a>");
                                    html.Append("</div>");
                                    html.Append("</div>");
                                    html.Append("</div>");

                                    tongsoluongsp += Convert.ToInt32(item.quantity);

                                    i++;
                                }
                            }
                        }

                        ltr_totalproduct.Text = tongsoluongsp.ToString();
                        hdfallorderid.Value = listorderid;

                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("<div class=\"right-info\">");
                        html.Append("<div class=\"order-hd\">");
                        html.Append("<div class=\"shop-cart\">");
                        html.Append("<span class=\"shop-name\">ĐẶT HÀNG</span>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("<div class=\"order-main\">");
                        html.Append("<div class=\"order-note input-field\">");
                        if (!string.IsNullOrEmpty(shop.Note))
                        {
                            html.Append("<textarea id=\"order_temp_" + shop.ID + "\" class=\"materialize-textarea\">" + shop.Note + "</textarea>");
                        }
                        else
                        {
                            html.Append("<textarea id=\"order_temp_" + shop.ID + "\" class=\"materialize-textarea\"></textarea>");
                        }
                        html.Append("<label for=\"textarea2\">Ghi chú đơn hàng</label>");
                        html.Append("</div>");
                        html.Append("<div>");
                        html.Append("<div class=\"total-all\">");
                        html.Append("<strong class=\"black-text left\">Tiền hàng:</strong>");
                        html.Append("<strong class=\"price-normal red-text\">" + string.Format("{0:N0}", TotalPriceProductInShop) + " VNĐ</strong>");
                        html.Append("</div>");
                        html.Append("<div class=\"total-all\">");
                        html.Append("<strong class=\"black-text left\">Tổng tính:</strong>");
                        html.Append("<strong class=\"price-normal red-text\" id=\"priceVND_" + shop.ID + "\" data-price=\"" + TotalPriceShop + "\">" + string.Format("{0:N0}", TotalPriceShop) + " VNĐ</strong>");
                        html.Append("</div>");

                        //html.Append("<div class=\"checkout\">");
                        //html.Append("<a class=\"btn\"onclick=\"checkout('" + shop.ID + "')\" href=\"javascript:;\">Đặt hàng</a>");
                        //html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("<div class=\"select-option\">");
                        html.Append("<label>");
                        if (shop.IsFastDelivery == true)
                        {
                            html.Append("<input type=\"checkbox\" class=\"\" onclick=\"updatecheck($(this),'" + shop.ID + "','IsFastDelivery')\"  checked=\"checked\" />");
                        }
                        else
                        {
                            html.Append("<input type=\"checkbox\" class=\"\" onclick=\"updatecheck($(this),'" + shop.ID + "','IsFastDelivery')\" />");
                        }
                        html.Append("<span>Giao tận nhà</span>");
                        html.Append("</label>");
                        html.Append("<label>");
                        if (shop.IsCheckProduct == true)
                        {
                            html.Append("<input type=\"checkbox\" id=\"" + shop.ID + "_checkproductselect\" onclick=\"updatecheck($(this),'" + shop.ID + "','IsCheckProduct')\" class=\"\" checked=\"checked\" />");
                        }
                        else
                        {
                            html.Append("<input type=\"checkbox\" id=\"" + shop.ID + "_checkproductselect\" onclick=\"updatecheck($(this),'" + shop.ID + "','IsCheckProduct')\" class=\"\" />");
                        }
                        html.Append("<span>Kiểm hàng</span>");
                        html.Append("</label>");
                        html.Append("<label>");
                        if (shop.IsPacked == true)
                        {
                            html.Append("<input type=\"checkbox\" class=\"\" onclick=\"updatecheck($(this),'" + shop.ID + "','IsPacked')\" checked=\"checked\" />");
                        }
                        else
                        {
                            html.Append("<input type=\"checkbox\" onclick=\"updatecheck($(this),'" + shop.ID + "','IsPacked')\" class=\"\" />");
                        }
                        html.Append("<span>Đóng gỗ</span>");
                        html.Append("</label>");

                        html.Append("<label>");
                        if (shop.IsInsurrance == true)
                        {
                            html.Append("<input type=\"checkbox\" class=\"\" onclick=\"updatecheck($(this),'" + shop.ID + "','IsInsurrance')\" checked=\"checked\" />");
                        }
                        else
                        {
                            html.Append("<input type=\"checkbox\" onclick=\"updatecheck($(this),'" + shop.ID + "','IsInsurrance')\" class=\"\" />");
                        }
                        html.Append("<span>Bảo hiểm</span>");
                        html.Append("</label>");
                        html.Append("<label>");
                        html.Append("<span class=\"quantity\">Tổng số lượng = " + totalquantity + " sản phẩm</span>");
                        html.Append("</label>");
                        html.Append("</div>");
                        html.Append("</div>");

                        #endregion

                        pricefullcart += TotalPriceShop;
                    }
                    ltr_cart.Text = html.ToString();
                    ltr_sub.Text = "<p><span class=\"teal-text text-darken-4 font-weight-700\">" + countshop + "</span> Shop / <span class=\"teal-text text-darken-4 font-weight-700\">" + totalproduct + "</span> Sản phẩm / <span class=\"teal-text text-darken-4 font-weight-700\">" + string.Format("{0:N0}", pricefullcart) + " VNĐ</span> Tiền hàng</p>";

                    ltr_total_pay.Text += "<div class=\"total-wrap right-align\">";
                    ltr_total_pay.Text += "<div class=\"total-all\">";
                    ltr_total_pay.Text += "<strong class=\"black-text\">Tổng tiền tất cả đơn hàng: </strong>";
                    ltr_total_pay.Text += "<strong class=\"price-all red-text\">" + string.Format("{0:N0}", pricefullcart) + " VNĐ</strong>";
                    ltr_total_pay.Text += " </div>";
                    //ltr_total_pay.Text += "<div class=\"checkout-all\">";
                    //ltr_total_pay.Text += " <a href=\"javascript:;\"  style=\"display:none\" class=\"btn checkout-select mr-1 getallOrder\" onclick=\"checkoutAllSelect()\" >Đặt hàng đơn đã chọn (<span class=\"numofOrder\">" + totalproduct + "</span>)</a>";
                    //ltr_total_pay.Text += "<a href=\"javascript:;\" class=\"btn checkout-all \" onclick=\"checkoutAll('" + listorderid + "')\">Đặt hàng tất cả đơn</a>";
                    //ltr_total_pay.Text += "</div>";
                    ltr_total_pay.Text += " </div>";

                }
                else
                {
                    ltrNotProduct.Text = "<p class=\"center-align red-text\">Hiện tại không có sản phẩm nào trong giỏ hàng của bạn.</p>";
                }
            }
            else
            {
                ltrNotProduct.Text = "<p class=\"center-align red-text\">Hiện tại không có sản phẩm nào trong giỏ hàng của bạn.</p>";
            }
        }
        protected void checkoutallorder_Click(object sender, EventArgs e)
        {
            string lo = hdforderlistall.Value;
            string brandtext = hdfProductBrand.Value;
            if (!string.IsNullOrEmpty(brandtext))
            {
                string[] bps = brandtext.Split('|');
                for (int i = 0; i < bps.Length - 1; i++)
                {
                    string bt = bps[i];
                    string[] item = bt.Split(',');
                    int IDpro = item[0].ToInt(0);
                    string bra = item[1];
                    OrderTempController.UpdateBrand(IDpro, bra);
                }
            }
            Guid temp = Guid.NewGuid();
            List<string> orderId = hdfListOrderTempID.Value.Split('|').Where(n => !string.IsNullOrEmpty(n)).ToList();
            if (!string.IsNullOrEmpty(lo))
            {
                string[] orders = lo.Split('|');
                for (int i = 0; i < orders.Length - 1; i++)
                {
                    string order = orders[i];
                    string[] items = order.Split(',');
                    int ID = items[0].ToInt(0);
                    string note = items[1];
                    string pricevnd = items[2];
                    string kq = OrderShopTempController.UpdateNoteFastPriceVND(ID, note, pricevnd);
                }
                Session["PayAllTempOrder"] = orderId;
                Response.Redirect("/manager/Payment");
            }
        }

        protected void checkout1order_Click(object sender, EventArgs e)
        {
            string lo = hdforderlistall.Value;
            string brandtext = hdfProductBrand.Value;
            if (!string.IsNullOrEmpty(brandtext))
            {
                string[] bps = brandtext.Split('|');
                for (int i = 0; i < bps.Length - 1; i++)
                {
                    string bt = bps[i];
                    string[] item = bt.Split(',');
                    int IDpro = item[0].ToInt(0);
                    string bra = item[1];
                    OrderTempController.UpdateBrand(IDpro, bra);
                }
            }
            if (!string.IsNullOrEmpty(lo))
            {
                int ID = 0;
                string[] orders = lo.Split('|');

                for (int i = 0; i < orders.Length - 1; i++)
                {
                    string order = orders[i];
                    string[] items = order.Split(',');
                    ID = items[0].ToInt(0);
                    string note = items[1];
                    string pricevnd = items[2];
                    string kq = OrderShopTempController.UpdateNoteFastPriceVND(ID, note, pricevnd);
                }
                List<string> orderId = new List<string>();
                orderId.Add(ID.ToString());
                Session["PayAllTempOrder"] = orderId;
                Response.Redirect("/manager/Payment/" + ID);
            }
        }

        #region Webservice
        [WebMethod]
        public static string deleteOrderShopTemp(string ID)
        {
            string kq = OrderShopTempController.Delete(Convert.ToInt32(ID));
            return kq;
        }
        [WebMethod]
        public static string deleteOrderTemp(string ID, string shopID)
        {
            string kq = "0";
            var ordertemp = OrderTempController.GetByID(Convert.ToInt32(ID));
            if (ordertemp != null)
            {
                string pricestep = ordertemp.stepprice;
                int UID = Convert.ToInt32(ordertemp.UID);
                string itemid = ordertemp.item_id;
                kq = OrderTempController.Delete(Convert.ToInt32(ID));
                OrderTempController.UpdatePriceInsert(UID, pricestep, itemid);
                int IDS = Convert.ToInt32(shopID);
                var ordert = OrderTempController.GetAllByOrderShopTempID(IDS);
                if (ordert.Count == 0)
                {
                    OrderShopTempController.Delete(IDS);
                }
            }
            return kq;
        }
        [WebMethod]
        public static string UpdateQuantityOrderTemp(string ID, int quantity, string brand)
        {
            OrderTempController.UpdateBrand(Convert.ToInt32(ID), brand);
            string kq = OrderTempController.UpdateQuantity(Convert.ToInt32(ID), quantity);
            return kq;
        }
        [WebMethod]
        public static string UpdateNoteFastPriceVND(int ID, string note, string priceVND)
        {
            string kq = OrderShopTempController.UpdateNoteFastPriceVND(ID, note, priceVND);
            if (kq == "1")
                return kq;
            else
                return "fail";
        }
        [WebMethod]
        public static string UpdateField(int ID, bool chk, string field)
        {
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user.RoleID == 0 || obj_user.RoleID == 2)
            {
                string UID = OrderShopTempController.SelectUIDByIDOrder(ID);
                if (UID != null)
                {
                    if (field == "IsCheckProduct")
                    {
                        if (chk == true)
                        {
                            var os = OrderShopTempController.GetByUIDAndID(UID.ToInt(), ID);
                            if (os != null)
                            {
                                double total = 0;
                                var listpro = OrderTempController.GetAllByOrderShopTempID(os.ID);
                                double counpros = 0;
                                if (listpro.Count > 0)
                                {
                                    foreach (var item in listpro)
                                    {
                                        double countProduct = item.quantity.ToInt(1);
                                        counpros += item.quantity.ToInt(1);
                                    }
                                    if (counpros > 0)
                                    {
                                        if (counpros >= 1 && counpros <= 2)
                                        {
                                            total = total + (5000 * counpros);
                                        }
                                        else if (counpros >= 3 && counpros <= 10)
                                        {
                                            total = total + (3500 * counpros);
                                        }
                                        else if (counpros >= 11 && counpros <= 100)
                                        {
                                            total = total + (2000 * counpros);
                                        }
                                        else if (counpros >= 101 && counpros <= 500)
                                        {
                                            total = total + (1500 * counpros);
                                        }
                                        else if (counpros >= 501 && counpros <= 999999)
                                        {
                                            total = total + (1000 * counpros);
                                        }
                                    }
                                }
                                total = Math.Round(total, 0);
                                OrderShopTempController.UpdateCheckProductPrice(ID, total.ToString());
                            }
                        }
                        else
                        {
                            OrderShopTempController.UpdateCheckProductPrice(ID, "0");
                        }
                    }
                    string kq = OrderShopTempController.Update4Field(ID, chk, field);
                    return kq;
                }
                return "null";
            }
            return "null";
        }
        #endregion
    }
}