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
    public partial class NhapSiChinaMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMenu();
                LoadData();
            }
        }

        public void LoadMenu()
        {
            string html = "";
            var categories = MenuController.GetByLevel(0);
            if (categories != null)
            {
                html += "<ul class=\"main-menu-nav\">";
                foreach (var c in categories)
                {
                    var categories2 = MenuController.GetByLevel(c.ID);
                    if (categories2 != null)
                    {
                        html += " <li>";

                        if (!string.IsNullOrEmpty(c.MenuLink))
                        {
                            if (Convert.ToBoolean(c.Target))
                                html += "<a target=\"_blank\" href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                            else
                                html += "<a href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                        }
                        else
                        {
                            html += "<a href=\"javascript:;\">" + c.MenuName + "</a>";
                        }

                        html += "<ul class=\"sub-menu\">";
                        foreach (var item in categories2)
                        {
                            html += " <li>";
                            if (Convert.ToBoolean(c.Target))
                                html += "   <a target=\"_blank\" href =\"" + item.MenuLink + "\">" + item.MenuName + "</a>";
                            else
                                html += "   <a href =\"" + item.MenuLink + "\">" + item.MenuName + "</a>";
                            html += "</li>";
                        }
                        html += " </ul>";
                    }
                    else
                    {
                        html += " <li>";
                        if (Convert.ToBoolean(c.Target))
                            html += "<a target=\"_blank\" href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                        else
                            html += "<a href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                        html += "</li>";
                    }
                }
                html += "</ul>";
                ltrMenu.Text = html;
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

        #region Translate And Run
        public string TranslateText(string input, string languagePair)
        {
            try
            {
                string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", input, languagePair);
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                request.Method = "GET";
                var content = String.Empty;
                HttpStatusCode statusCode;
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    var contentType = response.ContentType;
                    Encoding encoding = null;
                    if (contentType != null)
                    {
                        var match = Regex.Match(contentType, @"(?<=charset\=).*");
                        if (match.Success)
                            encoding = Encoding.GetEncoding(match.ToString());
                    }

                    encoding = encoding ?? Encoding.UTF8;

                    statusCode = ((HttpWebResponse)response).StatusCode;
                    using (var reader = new StreamReader(stream, encoding))
                        content = reader.ReadToEnd();
                }
                var doc = Dcsoup.Parse(content);
                var scoreDiv = doc.Select("html").Select("span[id=result_box]").Html;
                return scoreDiv;
            }
            catch
            {
                return "";
            }

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
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "redirect('" + linkgo + "')", true);
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "redirect('" + linkgo + "');", true);
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
        #endregion
        public void LoadData()
        {
            var confi = ConfigurationController.GetByTop1();
            if (confi != null)
            {
                #region lấy meta
                HtmlHead objHeader = (HtmlHead)Page.Header;
                HtmlMeta meta = new HtmlMeta();
                meta = new HtmlMeta();
                meta.Attributes.Add("name", "description");
                meta.Content = confi.MetaDescription;
                objHeader.Controls.Add(meta);

                meta = new HtmlMeta();
                meta.Attributes.Add("name", "keyword");
                meta.Content = confi.MetaKeyword;
                objHeader.Controls.Add(meta);
                ltrSEO.Text += "<script>" + confi.GoogleAnalytics + "</script>";
                ltrSEO.Text += "<script>" + confi.WebmasterTools + "</script>";

                ltrHeader.Text += "<script>" + confi.HeaderScriptCode + "</script>";
                //ltrFooterScript.Text += "<script>" + confi.FooterScriptCode + "</script>";
                #endregion

                string email = confi.EmailSupport;
                string hotline = confi.Hotline;
                string timework = confi.TimeWork;

                //ltrFooter.Text = confi.FooterLeft;
              
                ltrTopLeft.Text += "<p class=\"text\"> <span class=\"font-bold\">Tỉ giá: </span>   1¥ = " + string.Format("{0:N0}", Convert.ToDouble(confi.Currency)) + " Đồng</p>";

                ltrLogo.Text = "<a href=\"/\"><img  src=\"" + confi.LogoIMG + "\" alt=\"\"></a>";
                ltrLogoFooter.Text = "<a href=\"/\"><img  src=\"" + confi.LogoIMG + "\" alt=\"\"></a>";

                //ltrLogofooter.Text = "<img class=\"logo-cus\" width=\"100px\" src=\"" + confi.LogoIMG + "\" alt=\"\">";

                ltrTime.Text = "<a href=\"#\">Giờ hoạt động <span>"+timework+"</span></a>";
                ltrTopLeftEmail.Text = "<a href=\""+email + "\"><span class=\"font-bold\">Email: </span>"+email+"</a>";

                ltrEmailFooter.Text = "<a href=\"" + email + "\"><span class=\"font-bold\">Email: </span>" + email + "</a>";

                ltrHotLine.Text += "<a href=\"tel:" + hotline + "\"> Hotline: <span>" + hotline + "</a></span>";
                ltrHotlineFooter.Text += "<a href=\"tel:" + hotline + "\"> <span class=\"font-bold\">Hotline: " + hotline + "</a></span>";

                ltrAddFooter.Text += "<span class=\"font-bold\">Địa chỉ: </span> <br /> "+confi.Address+"";

                ltrSocial.Text += "<li><a href=\"" + confi.Facebook + "\" target=\"_blank\"><i class=\"fa fa-facebook\" aria-hidden=\"true\"></i></a></li>";
                ltrSocial.Text += "<li><a href=\"" + confi.Twitter + "\" target=\"_blank\"><i class=\"fa fa-twitter\" aria-hidden=\"true\"></i></a></li>";
                ltrSocial.Text += "<li><a href=\"" + confi.YoutubeLink + "\" target=\"_blank\"><i class=\"fa fa-youtube-play\" aria-hidden=\"true\"></i></a></li>";
                ltrSocial.Text += "<li><a href=\"" + confi.Instagram + "\" target=\"_blank\"><i class=\"fa fa-instagram\" aria-hidden=\"true\"></i></a></li>";
                //ltrSocial.Text += "<li><a href=\"" + confi.GooglePlus + "\" target=\"_blank\"><i class=\"fa fa-google-plus\" aria-hidden=\"true\"></i></a></li>";

                string infocontent = confi.InfoContent;
                if (Session["infoclose"] == null)
                {
                    if (!string.IsNullOrEmpty(infocontent))
                    {
                        ltr_infor.Text += "<div class=\"sec webinfo\">";
                        ltr_infor.Text += "<div class=\"all-info\">";
                        ltr_infor.Text += "<div class=\"main-info\">";
                        ltr_infor.Text += "<div class=\"textcontent\">";
                        ltr_infor.Text += "<span>" + infocontent + "</span>";
                        ltr_infor.Text += "<a href=\"javascript:;\" onclick=\"closewebinfo()\" class=\"icon-close-info\"><i class=\"fa fa-times\"></i></a>";
                        ltr_infor.Text += "</div></div></div></div>";
                    }
                }
            }
            if (Session["userLoginSystem"] != null)
            {
                string username = Session["userLoginSystem"].ToString();
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    var ordershoptemp = OrderShopTempController.GetByUID(acc.ID);
                    int count = 0;
                    if (ordershoptemp.Count > 0)
                        count = ordershoptemp.Count;
                    #region phần thông báo
                    decimal levelID = Convert.ToDecimal(acc.LevelID);
                    int levelID1 = Convert.ToInt32(acc.LevelID);
                    string level = "1 Vương Miện";
                    var userLevel = UserLevelController.GetByID(levelID1);
                    if (userLevel != null)
                    {
                        level = userLevel.LevelName;
                    }
                    string userIMG = "/App_Themes/CIQOrder/images/user-icon.png";
                    var ai = AccountInfoController.GetByUserID(acc.ID);
                    if (ai != null)
                    {
                        if (!string.IsNullOrEmpty(ai.IMGUser))
                            userIMG = ai.IMGUser;
                    }

                    decimal countLevel = UserLevelController.GetAll("").Count();
                    decimal te = levelID / countLevel;
                    te = Math.Round(te, 2, MidpointRounding.AwayFromZero);
                    decimal tile = te * 100;
                    string levelIconList = "";
                    string levelIconSingle = "";
                    var userLevels = UserLevelController.GetAll("");
                    if (userLevels.Count > 0)
                    {
                        foreach (var item in userLevels)
                        {
                            if (item.ID <= levelID)
                            {
                                levelIconList += "<img style=\"margin-right:5px;width:15%\" src=\"/App_Themes/StarExpress/images/vm-active.png\">";
                                //levelIconSingle += "<img src=\"/App_Themes/CIQOrder/images/vm-active.png\">";
                            }
                            else
                            {
                                levelIconList += "<img style=\"margin-right:5px;width:15%\" src=\"/App_Themes/StarExpress/images/vm-inactive.png\">";
                            }
                        }
                    }
                    #endregion

                    #region New
                    ltrLogin.Text += "<div class=\"acc-info\">";
                    ltrLogin.Text += "<a class=\"acc-info-btn\" href=\"/thong-tin-nguoi-dung\"><i class=\"icon fa fa-user\"></i><span>" + username + "</span></a>";
                    ltrLogin.Text += "<div class=\"status-desktop\">";
                    ltrLogin.Text += "<div class=\"status-wrap\">";
                    ltrLogin.Text += "<div class=\"status__header\">";
                    ltrLogin.Text += "<h4>" + level + "</h4>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"status__body\">";
                    ltrLogin.Text += "<div class=\"level\">";
                    ltrLogin.Text += "<div class=\"level__info\">";
                    ltrLogin.Text += "<p>Level</p>";
                    ltrLogin.Text += "<p class=\"rank\">" + level + "</p>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"level__process\"><span style=\"width: " + tile + "%\"></span></div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"balance\">";
                    ltrLogin.Text += "<p>Số dư:</p>";
                    ltrLogin.Text += "<div class=\"balance__number\"><p class=\"vnd\">" + string.Format("{0:N0}", acc.Wallet) + " VNĐ</p></div>";
                    ltrLogin.Text += "</div>";
                    if (acc.RoleID != 1)
                        ltrLogin.Text += "<div class=\"links\"><a href=\"/manager/login\">Quản trị<i class=\"fa fa-caret-right\"></i></a></div>";
                    ltrLogin.Text += "<div class=\"links\"><a href=\"/thong-tin-nguoi-dung\">Thông tin tài khoản<i class=\"fa fa-caret-right\"></i></a></div>";
                    ltrLogin.Text += "<div class=\"links\"><a href=\"/danh-sach-don-hang?t=1\">Đơn hàng của bạn<i class=\"fa fa-caret-right\"></i></a></div>";
                    ltrLogin.Text += "<div class=\"links\"><a href=\"/lich-su-giao-dich\">Lịch sử giao dịch<i class=\"fa fa-caret-right\"></i></a></div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"status__footer\"><a href=\"/dang-xuat\" class=\"ft-btn\">ĐĂNG XUẤT</a></div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"status-mobile\">";
                    ltrLogin.Text += "<a href=\"/thong-tin-nguoi-dung\" class=\"user-menu-logo\"><img src=\"" + userIMG + "\" alt=\"\"></a>";
                    ltrLogin.Text += "<h3 class=\"username\">" + username + "</h3>";
                    ltrLogin.Text += "<div class=\"user-info\">Số tiền: <span class=\"money\">" + string.Format("{0:N0}", acc.Wallet) + "</span> VNĐ | Level <span class=\"vip\">" + level + "</span></div>";
                    ltrLogin.Text += "<div class=\"nav-percent\">";
                    ltrLogin.Text += "<div class=\"nav-percent-ok\" style=\"width: " + tile + "%\"></div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"profile-bottom\">";
                    ltrLogin.Text += "<ul class=\"menu-in-profile\">";
                    ltrLogin.Text += "<li><a href=\"/\"><i class=\"fa fa-home\"></i>TRANG CHỦ</a></li>";
                    ltrLogin.Text += "<li><a href=\"/theo-doi-mvd\"><i class=\"fa fa-search\"></i>TRACKING</a></li>";
                    ltrLogin.Text += "<li><a href=\"/danh-sach-don-hang?t=1\"><i class=\"fa fa-home\"></i>MUA HÀNG HỘ</a></li>";
                    ltrLogin.Text += "<li><a href=\"/lich-su-giao-dich\"><i class=\"fa fa-money\"></i>TÀI CHÍNH</a></li>";
                    ltrLogin.Text += "<li><a href=\"/khieu-nai\"><i class=\"fa fa-exclamation\"></i>KHIẾU NẠI</a></li>";
                    ltrLogin.Text += "<li><a href=\"/thong-tin-nguoi-dung\"><i class=\"fa fa-user\"></i>QUẢN LÝ TÀI KHOẢN</a></li>";
                    ltrLogin.Text += "</ul>";
                    ltrLogin.Text += "</div><a href=\"/dang-xuat\" class=\"main-btn\">Đăng xuất</a></div>";
                    ltrLogin.Text += "<div class=\"overlay-status-mobile\"></div>";
                    ltrLogin.Text += "</div>";
                    #endregion
                }
            }
            else
            {
                ltrLogin.Text += "<div class=\"login-register\">";
                ltrLogin.Text += "       <a href=\"/dang-ky\" ><i class=\"fa fa-sign-out-alt\" aria-hidden=\"true\"></i>Đăng ký</a>";
                ltrLogin.Text += "          <span>|</span> <a href=\"/dang-nhap\" ><i class=\"fa fa-user-alt\" aria-hidden=\"true\"></i>Đăng nhập</a>";
                ltrLogin.Text += "</div>";
            }
        }
        protected void BtnContact_Click(object sender, EventArgs e)
        {
            string FullName = txtFullNameContact.Text.Trim();
            string Email = txtEmailContact.Text.Trim();
            string Note = txtNoteContact.Text.Trim();
            var rs = ContactCustomerController.InsertNew(FullName, "", Email, Note);
            if (rs != null)
            {
                PJUtils.ShowMessageBoxSwAlert("Gửi yêu cầu thành công.", "s", true, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình gửi. Vui lòng thử lại.", "e", true, Page);
            }
        }
    }
}