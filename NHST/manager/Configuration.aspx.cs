using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST.manager
{
    public partial class Configuration : System.Web.UI.Page
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
                    if (ac.RoleID != 0)
                        Response.Redirect("/trang-chu");
                }
                loaddata();
            }
        }
        public void loaddata()
        {
            var c = ConfigurationController.GetByTop1();
            if (c != null)
            {
                txtWebName.Text = c.Websitename;
                txtAddress1.Text = c.Address;
                txtAddress2.Text = c.Address2;
                hdfAbout.Value = c.AboutText;
                hdfAddress1.Value = c.Address;
                hdfAddress2.Value = c.Address2;
                txtEmailSupport.Text = c.EmailSupport;
                txtEmailContact.Text = c.EmailContact;
                txtHotline.Text = c.Hotline;
                txtHotlineSupport.Text = c.HotlineSupport;
                txtHotlineFeedback.Text = c.HotlineFeedback;
                txtFacebook.Text = c.Facebook;
                txtTwitter.Text = c.Twitter;
                txtGooglePlus.Text = c.GooglePlus;
                txtPinterest.Text = c.Pinterest;
                txtInstagram.Text = c.Instagram;
                txtSkype.Text = c.Skype;
                //txtTimeWork.Text = c.TimeWork;
                txtWorkingTime.Text = c.TimeWork;
                txtCNY_VN.Text = c.Currency; ;
                hdfFooterConfig.Value = c.FooterLeft;
                //rFooterPhai.Content = c.FooterRight;
                //pContent.Content = c.InfoContent;
                //rPercent.Value = Convert.ToDouble(c.PercentOrder);
                //rWeightPrice.Value = Convert.ToDouble(c.WeightPrice);
                //rCurrencyIncome.Value = Convert.ToDouble(c.CurrencyIncome);
                txtPriceDefault.Text = c.PricePayHelpDefault;
                //pPriceSendDefaultHN.Value = Convert.ToDouble(c.PriceSendDefaultHN);
                //pPriceSendDefaultSG.Value = Convert.ToDouble(c.PriceSendDefaultSG);
                txtPercentAfter3M.Text = c.SalePercentAfter3Month;
                txtPercentIn3M.Text = c.SalePercent;
                txtPercentStaffOrder.Text = c.DathangPercent;
                hdfContentNotificationPopup.Value = c.NotiPopup;
                txtTitleNotificationPopup.Text = c.NotiPopupTitle;
                txtTitleNotificattion.Text = c.NotiPopupEmail;
                txtLinkYoutube.Text = c.YoutubeLink;
                txtLinkZalo.Text = c.ZaloLink;
                txtLinkWeChat.Text = c.WechatLink;
                txtMetaDescription.Text = c.MetaDescription;
                txtMetaKeyWord.Text = c.MetaKeyword;
                txtGoogleAna.Text = c.GoogleAnalytics;
                txtWebMaster.Text = c.WebmasterTools;
                txtNumberLinkOfOrder.Text = Convert.ToInt32(c.NumberLinkOfOrder).ToString();


                txtKgDau.Text = c.FeeDongGoKgDau;
                txtKgSau.Text = c.FeeDongGoKgSau;
                txtDacbiet1.Text = c.FeeDacBiet1;
                txtDacbiet2.Text = c.FeeDacBiet2;


                txtTitle.Text = c.MetaTitle;
                txtOGTitle.Text = c.OGTitle;
                txtOGDescription.Text = c.OGDescription;
                OGImageBefore.ImageUrl = c.OGImage;

                txtOGFBTitle.Text = c.OGFBTitle;
                txtOGFBDescription.Text = c.OGFBDescription;
                OGFbImageBefore.ImageUrl = c.OGFBImage;

                txtTwitterTitle.Text = c.OGTwitterTitle;
                txtTwitterDescription.Text = c.OGTwitterDescription;
                OGTwitterImageBefore.ImageUrl = c.OGTwitterImage;
                txtInsurancePercent.Text = c.InsurancePercent;

                txtCompanyName.Text = c.CompanyName;
                txtCompanyLongName.Text = c.CompanyLongName;
                txtCompanyShortName.Text = c.CompanyShortName;
                txtTaxCode.Text = c.TaxCode;
                imgLogoBefore.ImageUrl = c.LogoIMG;

                rAgentCurrency.Text = c.AgentCurrency.ToString();
                rPriceCheckOutWareDefault.Text = c.PriceCheckOutWareDefault.ToString();
            }
        }

        protected void btncreateuser_Click(object sender, EventArgs e)
        {
            //if (!Page.IsValid) return;
            //var c = ConfigurationController.GetByTop1();
            //if (c != null)
            //{
            //    string PathIMG = "/Uploads/images/";
            //    string LogoIMG = "";
            //    string BannerIMG = "";
            //    if (rLogo.UploadedFiles.Count > 0)
            //    {
            //        foreach (UploadedFile f in rLogo.UploadedFiles)
            //        {
            //            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
            //            {
            //                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
            //                {
            //                    var o = PathIMG + Guid.NewGuid() + f.GetExtension();
            //                    try
            //                    {
            //                        f.SaveAs(Server.MapPath(o));
            //                        LogoIMG = o;
            //                    }
            //                    catch { }
            //                }
            //            }
            //        }
            //    }
            //    else
            //        LogoIMG = imgLogo.ImageUrl;

            //    if (rBannerIMG.UploadedFiles.Count > 0)
            //    {
            //        foreach (UploadedFile f in rBannerIMG.UploadedFiles)
            //        {
            //            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
            //            {
            //                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
            //                {
            //                    var o = PathIMG + Guid.NewGuid() + f.GetExtension();
            //                    try
            //                    {
            //                        f.SaveAs(Server.MapPath(o));
            //                        BannerIMG = o;
            //                    }
            //                    catch { }
            //                }
            //            }
            //        }
            //    }
            //    else
            //        BannerIMG = imgBannerIMG.ImageUrl;


            //    var kq = ConfigurationController.Update(c.ID, txtWebsitename.Text, txtEmailSupport.Text, txtEmailContact.Text, txtHotline.Text,
            //        rAddress.Content, txtFacebook.Text, txtTwitter.Text, txtGooglePlus.Text,
            //        txtInstagram.Text, txtSkype.Text, rTimeWork.Content, pCurrency.Value.ToString(), rCurrencyIncome.Value.ToString(), rPercent.Value.ToString(),
            //        pContent.Content, LogoIMG, BannerIMG, txtChromeExtensionLink.Text, txtCocCocExtensionLink.Text, rAbount.Content, rAddress2.Content, rAddress3.Content,
            //        rFooterTrai.Content, rFooterPhai.Content, rWeightPrice.Value.ToString(),
            //        pPricePayHelpDefault.Value.ToString(), pPriceSendDefaultHN.Value.ToString(), pPriceSendDefaultSG.Value.ToString(),
            //        rSalePercentAfter3Month.Value.ToString(), rSalePercent.Value.ToString(), rDathangPercent.Value.ToString(), txtHotlineSupport.Text,
            //        txtHotlineFeedback.Text, txtPinterest.Text, rNotiPopup.Content,
            //        txtNotiPopupTitle.Text, txtEmailNoti.Text, txtYoutubeLink.Text, txtZaloLink.Text,
            //        txtWechat.Text, txtMetakeyWord.Text, txtMetaDescription.Text, txtGoogleAnalytics.Text,
            //        txtWebmastertool.Text);
            //    if (kq == "ok")
            //        PJUtils.ShowMsg("Cập nhật thiết lập thành công.", true, Page);
            //}
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            var c = ConfigurationController.GetByTop1();
            if (c != null)
            {

                string IMG = "";
                string LogoFolder = "/Uploads/Images/";
                if (imgLogo.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile f in imgLogo.PostedFiles)
                    {
                        string fileContentType = f.ContentType; // getting ContentType

                        byte[] tempFileBytes = new byte[f.ContentLength];

                        var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                        string fileName = f.FileName; // getting File Name
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                        if (result)
                        {
                            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                            {
                                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                {
                                    //var o = KhieuNaiIMG1 + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                    try
                                    {
                                        //f.SaveAs(Server.MapPath(o));
                                        IMG = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                else
                    IMG = imgLogoBefore.ImageUrl;

                string IMG1 = "";
                string KhieuNaiIMG1 = "/Uploads/Images/";
                if (OGImage.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile f in OGImage.PostedFiles)
                    {
                        string fileContentType = f.ContentType; // getting ContentType

                        byte[] tempFileBytes = new byte[f.ContentLength];

                        var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                        string fileName = f.FileName; // getting File Name
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                        if (result)
                        {
                            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                            {
                                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                {
                                    //var o = KhieuNaiIMG1 + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                    try
                                    {
                                        //f.SaveAs(Server.MapPath(o));
                                        IMG1 = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                            IMG1 = OGImageBefore.ImageUrl;
                    }
                }
                else
                    IMG1 = OGImageBefore.ImageUrl;

                string IMGFacebook = "";
                string KhieuNaiIMGFacebook = "/Uploads/Images/";
                if (OGFbImage.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile f in OGFbImage.PostedFiles)
                    {
                        string fileContentType = f.ContentType; // getting ContentType

                        byte[] tempFileBytes = new byte[f.ContentLength];

                        var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                        string fileName = f.FileName; // getting File Name
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                        if (result)
                        {
                            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                            {
                                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                {
                                    //var o = KhieuNaiIMGFacebook + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                    try
                                    {
                                        //f.SaveAs(Server.MapPath(o));
                                        IMGFacebook = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                else
                    IMGFacebook = OGFbImageBefore.ImageUrl;

                string IMGTwitter = "";
                string KhieuNaiIMGTwitter = "/Uploads/Images/";
                if (OGTwitterImage.PostedFiles.Count > 0)
                {
                    foreach (HttpPostedFile f in OGTwitterImage.PostedFiles)
                    {
                        string fileContentType = f.ContentType; // getting ContentType

                        byte[] tempFileBytes = new byte[f.ContentLength];

                        var data = f.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(f.ContentLength));

                        string fileName = f.FileName; // getting File Name
                        string fileExtension = Path.GetExtension(fileName).ToLower();

                        var result = FileUploadCheck.isValidFile(tempFileBytes, fileExtension, fileContentType); // Validate Header
                        if (result)
                        {
                            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                            {
                                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                {
                                    // var o = KhieuNaiIMGTwitter + Guid.NewGuid() + Path.GetExtension(f.FileName);
                                    try
                                    {
                                        //f.SaveAs(Server.MapPath(o));
                                        IMGTwitter = FileUploadCheck.ConvertToBase64(tempFileBytes);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                else
                    IMGTwitter = OGTwitterImageBefore.ImageUrl;


                var kq = ConfigurationController.UpdateVer2(c.ID, txtWebName.Text, txtEmailSupport.Text, txtEmailContact.Text, txtHotline.Text, txtAddress1.Text, txtFacebook.Text, txtTwitter.Text, txtGooglePlus.Text, txtInstagram.Text, txtSkype.Text,
                    txtWorkingTime.Text, txtCNY_VN.Text, txtPriceDefault.Text, hdfAbout.Value.ToString(), txtAddress2.Text, hdfFooterConfig.Value.ToString(), txtPercentAfter3M.Text, txtPercentIn3M.Text, txtPercentStaffOrder.Text,
                    txtHotlineSupport.Text, txtHotlineFeedback.Text, txtPinterest.Text, hdfContentNotificationPopup.Value.ToString(), txtTitleNotificationPopup.Text, txtTitleNotificattion.Text, txtLinkYoutube.Text, txtLinkZalo.Text, txtLinkWeChat.Text, txtMetaKeyWord.Text, txtMetaDescription.Text, txtGoogleAna.Text, txtWebMaster.Text, Convert.ToInt32(txtNumberLinkOfOrder.Text), txtTitle.Text, txtOGTitle.Text,
                    txtOGDescription.Text, IMG1, txtOGFBTitle.Text, txtOGFBDescription.Text, IMGFacebook, txtTwitterTitle.Text, txtTwitterDescription.Text, IMGTwitter, txtInsurancePercent.Text, txtHeaderScript.Text, txtFooterScript.Text, txtCompanyShortName.Text, txtCompanyLongName.Text, txtCompanyName.Text, IMG, txtTaxCode.Text, rAgentCurrency.Text, rPriceCheckOutWareDefault.Text,
                    txtKgDau.Text,txtKgSau.Text,txtDacbiet1.Text,txtDacbiet2.Text);
                if (kq == "ok")
                    PJUtils.ShowMsg("Cập nhật thiết lập thành công.", true, Page);
            }
        }

    }
}