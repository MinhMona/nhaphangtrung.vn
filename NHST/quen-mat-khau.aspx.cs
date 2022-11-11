using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZLADIPJ.Business;

namespace NHST
{
    public partial class quen_mat_khau2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Loaddata();
            }
        }
        public void Loaddata()
        {
            var confi = ConfigurationController.GetByTop1();
            if (confi != null)
            {
                string hotline = confi.Hotline;
                ltrHotlineCall.Text += "<a href=\"tel:" + hotline + "\" class=\"fancybox\">";
                ltrHotlineCall.Text += "<div class=\"coccoc-alo-phone coccoc-alo-green coccoc-alo-show\" id=\"coccoc-alo-phoneIcon\">";
                ltrHotlineCall.Text += "<div class=\"coccoc-alo-ph-circle\"></div>";
                ltrHotlineCall.Text += "<div class=\"coccoc-alo-ph-circle-fill\"></div>";
                ltrHotlineCall.Text += "<div class=\"coccoc-alo-ph-img-circle\"></div>";
                ltrHotlineCall.Text += "</div>";
                ltrHotlineCall.Text += "</a>";
            }
        }
        protected void btngetpass_Click(object sender, EventArgs e)
        {
            var checkmail = AccountController.GetByEmail(txtEmail.Text.Trim());
            if (checkmail != null)
            {
                string token = PJUtils.RandomStringWithText(15);
                var tk = TokenForgotPassController.Insert(Convert.ToInt32(checkmail.ID), token, checkmail.ID.ToString());
                if (tk != null)
                {
                    string link = "<a href=\"https://nhapsichina.com/mat-khau-moi.aspx?token=" + token + "\">đây</a>";
                    try
                    {
                        PJUtils.SendMailGmail("nslogi.vietnam@gmail.com", "123456", txtEmail.Text.Trim(),
                            "Reset mật khẩu trên hệ thống NHẬP HÀNG TRUNG, ", "Bạn vui lòng nhấn vào: <strong>" + link + "</strong>", "");
                    }
                    catch
                    {

                    }
                    PJUtils.ShowMessageBoxSwAlertBackToLink("Hệ thống đã gửi 1 email mới cho bạn, vui lòng kiểm tra email để khôi phục mật khẩu mới.", "s", true, "/dang-nhap", Page);
                }
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Email không tồn tại trong hệ thống.", "e", false, Page);
            }
           
        }
    }
}