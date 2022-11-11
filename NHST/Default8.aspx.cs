using NHST.Bussiness;
using NHST.Controllers;
using Supremes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class Default8 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("http://nhapsichina.vn/");
            //LoadData();
        }

        public void LoadData()
        {

            //lấy thông tin dịch vụ
            var services = ServiceController.GetAll().OrderBy(x => x.Position).ToList();
            if (services.Count > 0)
            {
                foreach (var item in services)
                {

                    ltrService.Text += " <div class=\"colum\">";
                    ltrService.Text += "<div class=\"box-content\">";
                    ltrService.Text += "<div class=\"box-img\">";
                    ltrService.Text += "<img src=\"" + item.ServiceIMG + "\" alt=\"\">";
                    ltrService.Text += "</div>";
                    ltrService.Text += "<div class=\"box-guide\">";
                    ltrService.Text += "<h4 class=\"title\">";
                    ltrService.Text += "<img src=\"" + item.ServiceIcon + "\" alt=\"\"><span>" + item.ServiceName + "</span>";
                    ltrService.Text += "</h4>";
                    ltrService.Text += "<p class=\"desc\">" + item.ServiceContent + "</p>";
                    ltrService.Text += "</div>";
                    ltrService.Text += "</div>";
                    ltrService.Text += "</div>";
                }
            }

            //lấy thông tin dịch vụ của chúng tôi
            var servicescus = ServiceCustomerController.GetAll().OrderBy(x => x.Position).ToList();
            if (servicescus.Count > 0)
            {
                foreach (var item in servicescus)
                {

                    ltrServiceCus.Text += " <div class=\"colum\">";
                    ltrServiceCus.Text += "<div class=\"box-guide\">";
                    ltrServiceCus.Text += "<div class=\"bg-gradien\"></div>";
                    ltrServiceCus.Text += "<div class=\"box-icon\">";
                    ltrServiceCus.Text += "<div class=\"icon\">";
                    ltrServiceCus.Text += "<img src=\"" + item.ServiceOfIMG + "\" alt=\"\">";
                    ltrServiceCus.Text += "</div>";
                    ltrServiceCus.Text += "</div>";
                    ltrServiceCus.Text += "<div class=\"box-text\">";
                    ltrServiceCus.Text += "<h4 class=\"title\">";
                    ltrServiceCus.Text += "<a>" + item.ServiceOfName + "</a>";
                    ltrServiceCus.Text += "</h4>";
                    ltrServiceCus.Text += "<p class=\"desc\">" + item.ServiceOfContent + "</p>";
                    ltrServiceCus.Text += "</div>";
                    ltrServiceCus.Text += "</div>";
                    ltrServiceCus.Text += "</div>";
                }
            }

            var benefits = BenefitsController.GetAll("").ToList();
            if (benefits.Count > 0)
            {
                foreach (var item in benefits)
                {

                    ltrBenefit.Text += " <div class=\"colum\">";
                    ltrBenefit.Text += "<div class=\"content wow fadeInUp\" data-wow-delay=\".4s\" data-wow-duration=\"1s\">";
                    ltrBenefit.Text += "<div class=\"bg-gradien\"></div>";
                    ltrBenefit.Text += "<div class=\"icon\">";
                    ltrBenefit.Text += "<img src=\"" + item.BenefitIMG + "\" alt=\"\">";
                    ltrBenefit.Text += "</div>";
                    ltrBenefit.Text += "<h3 class=\"title\">";
                    ltrBenefit.Text += "<a>" + item.BenefitName + "</a>";
                    ltrBenefit.Text += "</h3>";
                    ltrBenefit.Text += "<p class=\"desc\">" + item.BenefitDescription + "</p>";
                    ltrBenefit.Text += "</div>";
                    ltrBenefit.Text += "</div>";
                }
            }

            //Quy trình đặt hàng

            var steps = StepController.GetAll("");
            if (steps.Count > 0)
            {
                int count = 1;
                foreach (var s in steps)
                {
                    string name = s.StepName;
                    string namenotdau = LeoUtils.ConvertToUnSign(name);

                    if (count == 1)
                    {
                        ltrStep1.Text += "<li class=\"active\" data-tab=\"js-" + count + "\">";
                    }
                    else
                    {
                        ltrStep1.Text += "<li data-tab=\"js-" + count + "\">";
                    }

                    ltrStep1.Text += "<div class=\"box-item\">";
                    ltrStep1.Text += "<div class=\"icon\">";
                    ltrStep1.Text += "<img src=\"" + s.StepIMG + "\" alt=\"\">";
                    ltrStep1.Text += "</div>";
                    ltrStep1.Text += "<div class=\"text\"> " + s.StepName + "";
                    ltrStep1.Text += "</div>";
                    ltrStep1.Text += "</div>";
                    ltrStep1.Text += "</li>";

                    if (count == 1)
                    {
                        ltrStep2.Text += "<div class=\"c-tab__content js-"+count+" active\">";
                    }
                    else
                    {
                        ltrStep2.Text += "<div class=\"c-tab__content js-"+count+"\">";
                    }
                    ltrStep2.Text += "          <div class=\"table-content\">";
                    ltrStep2.Text += "                  <div class=\"columns\">";
                    ltrStep2.Text += "                  <div class=\"colum\">";
                    ltrStep2.Text += "                  <div class=\"box-img\">";
                    ltrStep2.Text += "                          <div class=\"box-img\"><img src =\"/App_Themes/nhapsichina/images/dk.png\" alt=\"\"></div>";
                    ltrStep2.Text += "          </div>";
                    ltrStep2.Text += "          </div>";
                    ltrStep2.Text += "                  <div class=\"colum\">";
                    ltrStep2.Text += "                  <div class=\"box-guide\">";
                    ltrStep2.Text += "                          <h3 class=\"title\">" + name + "</h3>";
                    ltrStep2.Text += "                          <p class=\"desc\">" + s.StepContent + "</p>";
                    ltrStep2.Text += "                  </div>";
                    ltrStep2.Text += "                  </div>";
                    ltrStep2.Text += "          </div>";
                    ltrStep2.Text += "</div>";
                    ltrStep2.Text += "</div>";

                    count++;
                }
            }



            var confi = ConfigurationController.GetByTop1();
            if (confi != null)
            {
                string email = confi.EmailSupport;
                string hotline = confi.Hotline;
                string timework = confi.TimeWork;
                ltrTopLeftEmail.Text = "<a href=\"" + email + "\"><span class=\"font-bold\"></span>" + email + "</a>";
                ltrTime.Text = "<a href=\"#\"><span>" + timework + "</span></a>";
                ltrHotlineFooter.Text += "<a href=\"tel:" + hotline + "\"> <span>" + hotline + "</a></span>";
                //ltrAddress.Text += "<p>" + confi.Address2 + "</p>";
                //ltrHotline.Text += "<a href=\"tel:" + hotline + "\"><span class=\"call-me\">Gọi cho chúng tôi khi cần</span><br/>" + hotline + "</a>";
                //ltrEmail.Text += "<p><a href=\"mailto:" + email + "\">" + email + "</a></p>";
                //ltrTimeWork.Text += "<p>" + timework + "</p>";
            }





            try
            {
                string weblink = "https://nhapsichina.com";
                string url = HttpContext.Current.Request.Url.AbsoluteUri;

                HtmlHead objHeader = (HtmlHead)Page.Header;

                //we add meta description
                HtmlMeta objMetaFacebook = new HtmlMeta();

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "fb:app_id");
                objMetaFacebook.Content = "676758839172144";
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:url");
                objMetaFacebook.Content = url;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:type");
                objMetaFacebook.Content = "website";
                objHeader.Controls.Add(objMetaFacebook);


                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:title");
                objMetaFacebook.Content = confi.OGTitle;
                objHeader.Controls.Add(objMetaFacebook);


                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:description");
                objMetaFacebook.Content = confi.OGDescription;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:image");
                objMetaFacebook.Content = weblink + confi.OGImage;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:image:width");
                objMetaFacebook.Content = "200";
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:image:height");
                objMetaFacebook.Content = "500";
                objHeader.Controls.Add(objMetaFacebook);

                HtmlMeta meta = new HtmlMeta();
                meta = new HtmlMeta();
                meta.Attributes.Add("name", "description");
                meta.Content = confi.MetaDescription;

                objHeader.Controls.Add(meta);

                this.Title = confi.MetaTitle;
                //meta = new HtmlMeta();
                //meta.Attributes.Add("name", "title");
                //meta.Content = "Võ Minh Thiên Logistics";
                //objHeader.Controls.Add(meta);


                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "og:title");
                objMetaFacebook.Content = confi.OGTitle;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "twitter:title");
                objMetaFacebook.Content = confi.OGTwitterTitle;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "twitter:description");
                objMetaFacebook.Content = confi.OGTwitterDescription;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "twitter:image");
                objMetaFacebook.Content = weblink + confi.OGTwitterImage;
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "twitter:image:width");
                objMetaFacebook.Content = "200";
                objHeader.Controls.Add(objMetaFacebook);

                objMetaFacebook = new HtmlMeta();
                objMetaFacebook.Attributes.Add("property", "twitter:image:height");
                objMetaFacebook.Content = "500";
                objHeader.Controls.Add(objMetaFacebook);

            }
            catch
            {

            }

        }

        [WebMethod]
        public static string closewebinfo()
        {
            HttpContext.Current.Session["infoclose"] = "ok";
            return "ok";
        }
        [WebMethod]
        public static string getPopup()
        {
            if (HttpContext.Current.Session["notshowpopup"] == null)
            {
                var conf = ConfigurationController.GetByTop1();
                string popup = conf.NotiPopup;
                if (popup != "<p><br data-mce-bogus=\"1\"></p>")
                {
                    NotiInfo n = new NotiInfo();
                    n.NotiTitle = conf.NotiPopupTitle;
                    n.NotiEmail = conf.NotiPopupEmail;
                    n.NotiContent = conf.NotiPopup;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    return serializer.Serialize(n);
                }
                else
                    return "null";
            }
            else
                return null;

        }
        [WebMethod]
        public static void setNotshow()
        {
            HttpContext.Current.Session["notshowpopup"] = "1";
        }
        public class NotiInfo
        {
            public string NotiTitle { get; set; }
            public string NotiContent { get; set; }
            public string NotiEmail { get; set; }
        }

        [WebMethod]
        public static string getInfo(string ordecode)
        {
            string returnString = "";
            var smallpackage = SmallPackageController.GetByOrderTransactionCode(ordecode);
            if (smallpackage != null)
            {
                int mID = 0;
                int tID = 0;
                if (smallpackage.MainOrderID != null)
                {
                    if (smallpackage.MainOrderID > 0)
                    {
                        mID = Convert.ToInt32(smallpackage.MainOrderID);
                    }
                    else if (smallpackage.TransportationOrderID != null)
                    {
                        if (smallpackage.TransportationOrderID > 0)
                        {
                            tID = Convert.ToInt32(smallpackage.TransportationOrderID);
                        }
                    }
                }
                else if (smallpackage.TransportationOrderID != null)
                {
                    if (smallpackage.TransportationOrderID > 0)
                    {
                        tID = Convert.ToInt32(smallpackage.TransportationOrderID);
                    }
                }
                string ordertype = "Chưa xác định";
                if (tID > 0)
                {
                    ordertype = "Đơn hàng vận chuyển hộ";
                }
                else if (mID > 0)
                {
                    ordertype = "Đơn hàng mua hộ";
                }

                returnString += "<aside class=\"side trk-info fr\"><table>";
                returnString += "   <tbody>";
                returnString += "       <tr>";
                returnString += "           <th style=\"width:50%\">Mã vận đơn:</th>";
                returnString += "           <td class=\"m-color\">" + ordecode + "</td>";
                returnString += "       </tr>";
                returnString += "       <tr>";
                returnString += "           <th style=\"width:50%\">ID đơn hàng:</th>";
                if (mID > 0)
                    returnString += "           <td class=\"m-color\">" + mID + "</td>";
                else if (tID > 0)
                    returnString += "           <td class=\"m-color\">" + tID + "</td>";
                else
                    returnString += "           <td class=\"m-color\">Chưa xác định</td>";
                returnString += "       </tr>";
                returnString += "       <tr>";
                returnString += "           <th style=\"width:50%\">Loại đơn hàng:</th>";
                returnString += "           <td class=\"m-color\">" + ordertype + "</td>";
                returnString += "       </tr>";
                returnString += "   </tbody>";
                returnString += "</table></aside>";
                returnString += "<aside class=\"side trk-history fl\"><ul class=\"list\">";
                if (smallpackage.Status == 0)
                {
                    returnString += "<li class=\"clear\">";
                    returnString += "  Mã vận đơn đã bị hủy";
                    returnString += "</li>";
                }
                else if (smallpackage.Status == 1)
                {
                    returnString += "<li class=\"clear\">";
                    returnString += "  Mã vận đơn chưa về kho TQ";
                    returnString += "</li>";
                }
                else if (smallpackage.Status == 2)
                {
                    returnString += "<li class=\"it clear\">";
                    if (smallpackage.DateInTQWarehouse != null)
                        returnString += "  <div class=\"date-time grey89\"><p>" + string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInTQWarehouse) + "</p></div>";
                    else
                        returnString += "  <div class=\"date-time grey89\"><p>Chưa xác định</p></div>";
                    returnString += "  <div class=\"statuss ok\">";
                    returnString += "      <i class=\"ico fa fa-check\"></i>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">Trạng thái:</span><span class=\"m-color\"> Đã về kho TQ</span></p>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">NV Xử lý:</span> <span class=\"m-color\">" + smallpackage.StaffTQWarehouse + "</span></p>";
                    returnString += "  </div>";
                    returnString += "</li>";
                }
                else if (smallpackage.Status == 3)
                {
                    returnString += "<li class=\"it clear\">";
                    if (smallpackage.DateInTQWarehouse != null)
                        returnString += "  <div class=\"date-time grey89\"><p>" + string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInTQWarehouse) + "</p></div>";
                    else
                        returnString += "  <div class=\"date-time grey89\"><p>Chưa xác định</p></div>";
                    returnString += "  <div class=\"statuss ok\">";
                    returnString += "      <i class=\"ico fa fa-check\"></i>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">Trạng thái:</span><span class=\"m-color\"> Đã về kho TQ</span></p>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">NV Xử lý:</span> <span class=\"m-color\">" + smallpackage.StaffTQWarehouse + "</span></p>";
                    returnString += "  </div>";
                    returnString += "</li>";
                    returnString += "<li class=\"it clear\">";
                    if (smallpackage.DateInLasteWareHouse != null)
                        returnString += "  <div class=\"date-time grey89\"><p>" + string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse) + "</p></div>";
                    else
                        returnString += "  <div class=\"date-time grey89\"><p>Chưa xác định</p></div>";
                    returnString += "  <div class=\"statuss ok\">";
                    returnString += "      <i class=\"ico fa fa-check\"></i>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">Trạng thái:</span><span class=\"m-color\"> Đã về kho đích</span></p>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">NV Xử lý:</span> <span class=\"m-color\">" + smallpackage.StaffVNWarehouse + "</span></p>";
                    returnString += "  </div>";
                    returnString += "</li>";
                }
                else if (smallpackage.Status == 4)
                {
                    returnString += "<li class=\"it clear\">";
                    if (smallpackage.DateInTQWarehouse != null)
                        returnString += "  <div class=\"date-time grey89\"><p>" + string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInTQWarehouse) + "</p></div>";
                    else
                        returnString += "  <div class=\"date-time grey89\"><p>Chưa xác định</p></div>";
                    returnString += "  <div class=\"statuss ok\">";
                    returnString += "      <i class=\"ico fa fa-check\"></i>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">Trạng thái:</span><span class=\"m-color\"> Đã về kho TQ</span></p>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">NV Xử lý:</span> <span class=\"m-color\">" + smallpackage.StaffTQWarehouse + "</span></p>";
                    returnString += "  </div>";
                    returnString += "</li>";
                    returnString += "<li class=\"it clear\">";
                    if (smallpackage.DateInLasteWareHouse != null)
                        returnString += "  <div class=\"date-time grey89\"><p>" + string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateInLasteWareHouse) + "</p></div>";
                    else
                        returnString += "  <div class=\"date-time grey89\"><p>Chưa xác định</p></div>";
                    returnString += "  <div class=\"statuss ok\">";
                    returnString += "      <i class=\"ico fa fa-check\"></i>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">Trạng thái:</span><span class=\"m-color\"> Đã về kho đích</span></p>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">NV Xử lý:</span> <span class=\"m-color\">" + smallpackage.StaffVNWarehouse + "</span></p>";
                    returnString += "  </div>";
                    returnString += "</li>";
                    returnString += "<li class=\"it clear\">";
                    if (smallpackage.DateOutWarehouse != null)
                        returnString += "  <div class=\"date-time grey89\"><p>" + string.Format("{0:dd/MM/yyyy HH:mm}", smallpackage.DateOutWarehouse) + "</p></div>";
                    else
                        returnString += "  <div class=\"date-time grey89\"><p>Chưa xác định</p></div>";
                    returnString += "  <div class=\"statuss ok\">";
                    returnString += "      <i class=\"ico fa fa-check\"></i>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">Trạng thái:</span><span class=\"m-color\"> Đã giao khách</span></p>";
                    returnString += "      <p class=\"tit\"><span class=\"grey89\">NV Xử lý:</span> <span class=\"m-color\">" + smallpackage.StaffVNOutWarehouse + "</span></p>";
                    returnString += "  </div>";
                    returnString += "</li>";
                }
                returnString += "</ul></aside>";
            }
            else
            {
                returnString += "Không tồn tại mã vận đơn trong hệ thống";
            }
            return returnString;
        }
    }
}