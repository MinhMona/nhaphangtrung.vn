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
    public partial class Default5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            var ck = CustomerBenefitsController.GetAllByItemType(2);
            if (ck.Count > 0)
            {
                foreach (var item in ck)
                {
                    ltrCamKet.Text += "<div class=\"col-4\">";
                    ltrCamKet.Text += "<div class=\"box-card\">";
                    ltrCamKet.Text += "<div class=\"content-card\">";
                    ltrCamKet.Text += "<div class=\"img-card\"><img src=\"" + item.Icon + "\" alt=\"\"></div>";
                    ltrCamKet.Text += "<div class=\"text-card\">";
                    ltrCamKet.Text += "<div class=\"title-card\">";
                    ltrCamKet.Text += "<h3>" + item.CustomerBenefitName + "</h3>";
                    ltrCamKet.Text += "</div>";
                    ltrCamKet.Text += "<div class=\"desc-card\">";
                    ltrCamKet.Text += "<p>" + item.CustomerBenefitDescription + "</p>";
                    ltrCamKet.Text += "</div>";
                    ltrCamKet.Text += "</div>";
                    ltrCamKet.Text += "</div>";
                    ltrCamKet.Text += "</div>";
                    ltrCamKet.Text += "</div>";
                }
            }
            var sv = ServiceController.GetAll();
            if (sv.Count > 0)
            {
                foreach (var item in sv)
                {
                    ltrServices.Text += "<div class=\"col-6\" style=\"margin-bottom: 20px\">";
                    ltrServices.Text += "<div class=\"class\"box-wrapper-4\">";
                    ltrServices.Text += "<div class=\"title\"><h4>" + item.ServiceName + "</h4></div>";
                    ltrServices.Text += "<div class=\"desc\"><p>" + item.ServiceContent + "</p></div>";
                    ltrServices.Text += "</div>";
                    ltrServices.Text += "</div>";
                }
            }
            var bank = BankController.GetAllNotIsHidden();
            if (bank.Count > 0)
            {
                foreach (var item in bank)
                {
                    ltrBank.Text += "<div class=\"col-6\">";
                    ltrBank.Text += "<div class=\"box-content8\">";
                    ltrBank.Text += "<div class=\"col-6\">";
                    ltrBank.Text += "<div class=\"img-nganhang\"><img src=\"" + item.IMG + "\" alt=\"\"></div>";
                    ltrBank.Text += "</div>";
                    ltrBank.Text += "<div class=\"col-6\" style=\"padding: 0;\">";
                    ltrBank.Text += "<div class=\"name\">";
                    ltrBank.Text += "<h3>" + item.AccountHolder + "</h3>";
                    ltrBank.Text += "<p>Ngân hàng: " + item.BankName + "</p>";
                    ltrBank.Text += "<p>STK: " + item.BankNumber + "</p>";
                    ltrBank.Text += "<p>Chi nhánh: " + item.Branch + "</p>";
                    ltrBank.Text += "</div>";
                    ltrBank.Text += "</div>";
                    ltrBank.Text += "</div>";
                    ltrBank.Text += "</div>";
                }
            }
            var steps = StepController.GetAll("");
            if (steps.Count > 0)
            {
                foreach (var item in steps)
                {
                    ltrStep1.Text += "<div class=\"item-1\">";
                    ltrStep1.Text += "<div class=\"box-item-1\">";
                    ltrStep1.Text += "<div class=\"img-item-1\"><img src=\"" + item.StepIMG + "\" alt=\"\"></div>";
                    ltrStep1.Text += "<div class=\"title-item-1\">";
                    ltrStep1.Text += "<p>" + item.StepName + "</p>";
                    ltrStep1.Text += "</div>";
                    ltrStep1.Text += "</div>";
                    ltrStep1.Text += "</div>";
                }
            }
            var quest = QuestionController.GetAllNotHiden();
            if (quest.Count > 0)
            {
                foreach (var item in quest)
                {
                    ltrQuestions.Text += "<div class=\"box-i5\">";
                    ltrQuestions.Text += "<div class=\"question\">";
                    ltrQuestions.Text += "<p>" + item.QuesTitle + "</p>";
                    ltrQuestions.Text += "</div>";
                    ltrQuestions.Text += "<div class=\"answer\">";
                    ltrQuestions.Text += "<p>" + item.QuesDesc + "</p>";
                    ltrQuestions.Text += "</div>";
                    ltrQuestions.Text += "</div>";
                }
            }
            var news = NewsController.GetAllNotHiddenType(1);
            if (news.Count > 0)
            {
                foreach (var item in news)
                {
                    ltrNews.Text += "<div class=\"col-4\">";
                    ltrNews.Text += "<div class=\"box-news\">";
                    ltrNews.Text += "<div class=\"img-news\"><img src=\"" + item.NewsIMG + "\" alt=\"\"></div>";
                    ltrNews.Text += "<div class=\"title-news\">";
                    ltrNews.Text += "<a href =\"" + item.NewsLink + "\">" + item.NewsTitle + "</a>";
                    ltrNews.Text += "</div>";
                    ltrNews.Text += "<div class=\"desc-news\">";
                    ltrNews.Text += "<p>" + item.NewsDesc + "</p>";
                    ltrNews.Text += "</div>";
                    if (!string.IsNullOrEmpty(item.NewsLink))
                    {
                        ltrNews.Text += "<div class=\"btn-web\"><a href =\"" + item.NewsLink + "\" class=\"btn-click\">Xem chi tiết</a></div>";
                    }
                    ltrNews.Text += "</div>";
                    ltrNews.Text += "</div>";
                }
            }

            var confi = ConfigurationController.GetByTop1();
            try
            {
                string weblink = "https://nhaphangtrung.vn/";
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

                meta = new HtmlMeta();
                meta.Attributes.Add("name", "keyword");
                meta.Content = confi.MetaKeyword;

                objHeader.Controls.Add(meta);
                this.Title = confi.MetaTitle;

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

                HtmlLink canonicalTag = new HtmlLink();
                canonicalTag.Attributes["rel"] = "canonical";
                canonicalTag.Href = weblink;
                Page.Header.Controls.Add(canonicalTag);
            }
            catch
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string text = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    string a = PJUtils.TranslateTextNew(text, "vi", "zh");
                    a = a.Replace("[", "").Replace("]", "").Replace("\"", "");
                    string[] ass = a.Split(',');
                    string page = hdfWebsearch.Value;
                    SearchPage(page, PJUtils.RemoveHTMLTags(ass[0]));
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA1.Create();
            byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(inputString);
            return bytes;
        }
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append("%" + b.ToString("X2"));

            return sb.ToString();
        }
        public void SearchPage(string page, string text)
        {
            string linkgo = "";
            if (page == "tmall")
            {
                string a = text;
                string textsearch_tmall = GetHashString(a);
                //string fullLinkSearch_tmall = "https://list.tmall.com/search_product.htm?q=" + textsearch_tmall + "&type=p&vmarket=&spm=875.7931836%2FB.a2227oh.d100&from=mallfp..pc_1_searchbutton";
                linkgo = "https://list.tmall.com/search_product.htm?q=" + textsearch_tmall + "&type=p&vmarket=&spm=875.7931836%2FB.a2227oh.d100&from=mallfp..pc_1_searchbutton";
            }
            else if (page == "taobao")
            {
                string a = text;
                string textsearch_taobao = GetHashString(a);
                //string fullLinkSearch_taobao = "https://world.taobao.com/search/search.htm?q=" + textsearch_taobao + "&navigator=all&_input_charset=&spm=a21bp.7806943.20151106.1";
                linkgo = "https://world.taobao.com/search/search.htm?q=" + textsearch_taobao + "&navigator=all&_input_charset=&spm=a21bp.7806943.20151106.1";
                //https://world.taobao.com/search/search.htm?q=%B9%AB%BC%A6&navigator=all&_input_charset=&spm=a21bp.7806943.20151106.1
            }
            else if (page == "1688")
            {
                string a = text;
                string textsearch_1688 = GetHashString(a);
                //string fullLinkSearch_1688 = "https://s.1688.com/selloffer/offer_search.htm?keywords=" + textsearch_1688 + "&button_click=top&earseDirect=false&n=y";
                linkgo = "https://s.1688.com/selloffer/offer_search.htm?keywords=" + textsearch_1688 + "&button_click=top&earseDirect=false&n=y";
            }
            Response.Redirect(linkgo);
        }

        [WebMethod]
        public static string getPopup()
        {
            if (HttpContext.Current.Session["notshowpopup"] == null)
            {
                var conf = ConfigurationController.GetByTop1();
                string popup = conf.NotiPopupTitle;
                if (!string.IsNullOrEmpty(popup))
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
        [WebMethod]
        public static string closewebinfo()
        {
            HttpContext.Current.Session["infoclose"] = "ok";
            return "ok";
        }
        public class NotiInfo
        {
            public string NotiTitle { get; set; }
            public string NotiContent { get; set; }
            public string NotiEmail { get; set; }
        }

    }
}