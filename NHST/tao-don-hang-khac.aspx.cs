using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class tao_don_hang_khac1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                loadPrefix();
                loaddata();
            }
        }

        public void loadPrefix()
        {
            var khotq = WarehouseFromController.GetAllWithIsHidden(false);
            if (khotq.Count > 0)
            {
                foreach (var item in khotq)
                {
                    ListItem us = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlKhoTQ.Items.Add(us);
                }
            }
            ListItem sleTT = new ListItem("Chọn kho TQ", "0");
            ddlKhoTQ.Items.Insert(0, sleTT);



            var khovn = WarehouseController.GetAllWithIsHidden(false);
            if (khovn.Count > 0)
            {
                foreach (var item in khovn)
                {
                    ListItem us = new ListItem(item.WareHouseName, item.ID.ToString());
                    ddlKhoVN.Items.Add(us);
                }
            }
            ListItem sleTT1 = new ListItem("Chọn kho VN", "0");
            ddlKhoVN.Items.Insert(0, sleTT1);

            var shipping = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shipping.Count > 0)
            {
                foreach (var item in shipping)
                {
                    ListItem us = new ListItem(item.ShippingTypeName, item.ID.ToString());
                    ddlShipping.Items.Add(us);
                }
            }
            ListItem sleTT2 = new ListItem("Chọn phương thức", "0");
            ddlShipping.Items.Insert(0, sleTT2);
        }

        public void loaddata()
        {
            string username = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                int id = obj_user.ID;
                ViewState["UID"] = id;
            }
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            string product = hdfProductList.Value;
            int UIDCreate = 0;
            string userBuy = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(userBuy);
            if (obj_user != null)
            {
                double currency = Convert.ToDouble(ConfigurationController.GetByTop1().Currency);
                if (!string.IsNullOrEmpty(obj_user.Currency.ToString()))
                {
                    if (Convert.ToDouble(obj_user.Currency) > 0)
                    {
                        currency = Convert.ToDouble(obj_user.Currency);
                    }
                }

                string fullname = "";
                string address = "";
                string email = "";
                string phone = "";

                UIDCreate = obj_user.ID;
                var ai = AccountInfoController.GetByUserID(UIDCreate);
                if (ai != null)
                {
                    fullname = ai.FirstName + " " + ai.LastName;
                    address = ai.Address;
                    email = ai.Email;
                    phone = ai.Phone;
                }

                int salerID = Convert.ToInt32(obj_user.SaleID);
                int dathangID = Convert.ToInt32(obj_user.DathangID);
                int cskhID = obj_user.CSID.ToString().ToInt(0);
                int UID = obj_user.ID;

                //double UL_CKFeeBuyPro = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeBuyPro);
                //double UL_CKFeeWeight = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).FeeWeight);
                //double LessDeposit = Convert.ToDouble(UserLevelController.GetByID(obj_user.LevelID.ToString().ToInt()).LessDeposit);
                //if (!string.IsNullOrEmpty(obj_user.Deposit.ToString()))
                //{
                //    if (obj_user.Deposit > 0)
                //    {
                //        LessDeposit = Convert.ToDouble(obj_user.Deposit);
                //    }
                //}

                double priceCYN = 0;
                double priceVND = 0;
                string[] products = product.Split('|');
                if (products.Length - 1 > 0)
                {
                    for (int i = 0; i < products.Length - 1; i++)
                    {
                        string[] item = products[i].Split(']');

                        string productlink = item[0];
                        string productname = item[1];
                        double productpriceCNY = 0;
                        if (item[2].ToFloat(0) > 0)
                        {
                            productpriceCNY = Convert.ToDouble(item[2]);
                        }
                        string productvariable = item[3];
                        double productquantity = 0;
                        if (item[4].ToFloat(0) > 0)
                            productquantity = Convert.ToDouble(item[4]);
                        var productnote = item[5];
                        string productimage = item[6];
                        double productprice = 0;
                        double productpromotionprice = 0;
                        double pricetoPay = 0;

                        if (productpromotionprice <= productprice)
                        {
                            pricetoPay = productpromotionprice;
                        }
                        else
                        {
                            pricetoPay = productprice;
                        }
                        priceCYN += (productpriceCNY * productquantity);
                    }
                }
                priceVND = priceCYN * currency;

                double userbuypro = 0;
                double userdeposit = 0;
                double phantramdichvu = 0;
                double phantramcoc = 0;

                if (!string.IsNullOrEmpty(obj_user.Deposit.ToString()))
                {
                    userdeposit = Convert.ToDouble(obj_user.Deposit);
                }
                if (!string.IsNullOrEmpty(obj_user.FeeBuyPro))
                {
                    userbuypro = Convert.ToDouble(obj_user.FeeBuyPro);
                }

                int Percent = ddlPercent.SelectedValue.ToString().ToInt();
                if (userdeposit > 0)
                {
                    if (userbuypro > 0)
                    {
                        phantramdichvu = userbuypro;
                    }
                    else if (priceVND < 6000000)
                    {
                        phantramdichvu = 3;
                    }
                    else
                    {
                        phantramdichvu = 1.8;
                    }
                    phantramcoc = userdeposit;
                }
                else if (Percent == 1)
                {
                    if (userbuypro > 0)
                    {
                        phantramdichvu = userbuypro;
                    }
                    else if (priceVND < 6000000)
                    {
                        phantramdichvu = 2;
                    }
                    else
                    {
                        phantramdichvu = 0.9;
                    }
                    phantramcoc = 100;
                }
                else
                {
                    if (userbuypro > 0)
                    {
                        phantramdichvu = userbuypro;
                    }
                    else if (priceVND < 6000000)
                    {
                        phantramdichvu = 3;
                    }
                    else
                    {
                        phantramdichvu = 1.8;
                    }
                    phantramcoc = 80;
                }

                double feebp = priceVND * phantramdichvu / 100;
                if (feebp < 20000)
                {
                    feebp = 20000;
                }

                double TotalPriceVND = Math.Round(priceVND + feebp, 0);
                string AmountDeposit = (priceVND * phantramcoc / 100).ToString();
                string Deposit = "0";
                string kq = MainOrderController.Insert(UID, "", "", "", false, "0", false, "0", false, "0", false, "0", false, "0",
                priceVND.ToString(), priceCYN.ToString(), "0", feebp.ToString(), "0", "", fullname, address, email, phone, 0, Deposit,
                currency.ToString(), TotalPriceVND.ToString(), salerID, dathangID, currentDate, UIDCreate, AmountDeposit, 3);
                int idkq = Convert.ToInt32(kq);
                string linkimage = "";
                if (idkq > 0)
                {
                    for (int i = 0; i < products.Length - 1; i++)
                    {
                        string[] item = products[i].Split(']');

                        string productlink = item[0];
                        string productname = item[1];
                        double productpriceCNY = 0;
                        double CNYPrice = 0;
                        if (item[2].ToFloat(0) > 0)
                        {
                            CNYPrice = Convert.ToDouble(item[2]);
                            productpriceCNY = Convert.ToDouble(item[2]);
                        }
                        string productvariable = item[3];
                        double productquantity = 0;
                        if (item[4].ToFloat(0) > 0)
                            productquantity = Convert.ToDouble(item[4]);
                        var productnote = item[5];
                        string productimage = item[6];

                        double productprice = 0;
                        double productpromotionprice = 0;
                        double pricetoPay = 0;

                        if (productpromotionprice <= productprice)
                        {
                            pricetoPay = productpromotionprice;
                        }
                        else
                        {
                            pricetoPay = productprice;
                        }
                        priceCYN += (pricetoPay * productquantity);

                        int quantity = item[4].ToInt(0);

                        double originprice = 0;
                        double promotionprice = 0;
                        double u_pricecbuy = 0;
                        double u_pricevn = 0;
                        double e_pricebuy = 0;
                        double e_pricevn = 0;
                        if (promotionprice < originprice)
                        {
                            productpriceCNY = promotionprice;
                            u_pricecbuy = promotionprice * currency;
                        }
                        else
                        {
                            productpriceCNY = originprice;
                            u_pricecbuy = originprice * currency;
                        }

                        e_pricebuy = productpriceCNY * productquantity;
                        e_pricevn = e_pricebuy * currency;

                        string image = productimage;
                        if (image.Contains("%2F"))
                        {
                            image = image.Replace("%2F", "/");
                        }
                        if (image.Contains("%3A"))
                        {
                            image = image.Replace("%3A", ":");
                        }
                        HttpPostedFile postedFile = Request.Files["" + productimage + ""];
                        string imageinput = "";
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {

                            string fileContentType = postedFile.ContentType; // getting ContentType

                            byte[] tempFileBytes = new byte[postedFile.ContentLength];

                            var data = postedFile.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(postedFile.ContentLength));

                            string fileName = postedFile.FileName; // getting File Name
                            string fileExtension = Path.GetExtension(fileName).ToLower();

                            var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                            if (result)
                            {

                                if (postedFile.FileName.ToLower().Contains(".jpg") || postedFile.FileName.ToLower().Contains(".png") || postedFile.FileName.ToLower().Contains(".jpeg"))
                                {
                                    if (postedFile.ContentType == "image/png" || postedFile.ContentType == "image/jpeg" || postedFile.ContentType == "image/jpg")
                                    {
                                        //var o = "/Uploads/Images/" + Guid.NewGuid() + System.IO.Path.GetExtension(postedFile.FileName);
                                        //postedFile.SaveAs(Server.MapPath(o));
                                        imageinput = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                }
                            }
                        }
                        string imagein = "";
                        if (!string.IsNullOrEmpty(imageinput))
                        {
                            imagein = imageinput;
                        }
                        else if (!string.IsNullOrEmpty(image))
                        {
                            imagein = image;
                        }
                        linkimage = imagein;
                        string ret = OrderController.Insert(UID, productname, productname, CNYPrice.ToString(), promotionprice.ToString(), productvariable,
                        productvariable, productvariable, imagein, imagein, "", "", "", "", quantity.ToString(),
                        "", "", "", "", "", productlink, "", "", "", "", "", productnote,
                        "", "0", "Web", "", false, false, "0",
                        false, "0", false, "0", false, "0", false,
                        "0", e_pricevn.ToString(), e_pricebuy.ToString(), productnote, fullname, address, email,
                        phone, 0, "0", currency.ToString(), TotalPriceVND.ToString(), idkq, DateTime.Now, UIDCreate);
                    }

                    MainOrderController.UpdateReceivePlace(idkq, UID, ddlKhoVN.SelectedValue, Convert.ToInt32(ddlShipping.SelectedValue));
                    MainOrderController.UpdateFromPlace(idkq, UID, Convert.ToInt32(ddlKhoTQ.SelectedValue), Convert.ToInt32(ddlShipping.SelectedValue));
                    MainOrderController.UpdateIsCheckNotiPrice(idkq, UID, true);
                    MainOrderController.UpdateLinkImage(idkq, linkimage);
                    MainOrderController.UpdateCSKHID(idkq, cskhID);

                    var setNoti = SendNotiEmailController.GetByID(5);
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
                                                                       "Có đơn hàng TMĐT mới ID là: " + idkq,
                                                                       1, currentDate, userBuy, false);
                                    string strPathAndQuery = Request.Url.PathAndQuery;
                                    string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                    PJUtils.PushNotiDesktop(admin.ID, "Có đơn hàng TMĐT mới ID là: " + idkq, datalink);
                                }
                            }

                            var managers = AccountController.GetAllByRoleID(2);
                            if (managers.Count > 0)
                            {
                                foreach (var manager in managers)
                                {
                                    NotificationsController.Inser(manager.ID,
                                                                       manager.Username, idkq,
                                                                       "Có đơn hàng TMĐT mới ID là: " + idkq,
                                                                       1, currentDate, userBuy, false);
                                    string strPathAndQuery = Request.Url.PathAndQuery;
                                    string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    string datalink = "" + strUrl + "manager/OrderDetail/" + idkq;
                                    PJUtils.PushNotiDesktop(manager.ID, "Có đơn hàng TMĐT mới ID là: " + idkq, datalink);
                                }
                            }
                        }
                    }
                }
                PJUtils.ShowMessageBoxSwAlert("Tạo đơn hàng thành công", "s", true, Page);
            }
        }
    }
}