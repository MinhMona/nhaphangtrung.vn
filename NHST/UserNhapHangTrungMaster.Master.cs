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
    public partial class UserNhapHangTrungMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/dang-nhap.aspx");
                }
                else
                {
                    string username_current = Session["userLoginSystem"].ToString();
                    var obj_user = AccountController.GetByUsername(username_current);
                    if (obj_user != null)
                    {
                        //if (!string.IsNullOrEmpty(obj_user.LoginStatus))
                        //{
                        //    if (Session["StateLogin"].ToString() == obj_user.LoginStatus)
                        //    {

                        //        hdfMainLoginID.Value = obj_user.ID.ToString();
                        //        hdfMainLoginStatus.Value = obj_user.LoginStatus;
                        //    }
                        //    else
                        //    {
                        //        Session.Abandon();
                        //        Response.Redirect("/");
                        //    }
                        //}
                        //else
                        //{
                        //    hdfMainLoginID.Value = obj_user.ID.ToString();
                        //    hdfMainLoginStatus.Value = "1";
                        //}
                    }
                    LoadNotification();                    
                }
            }
        }

        public void LoadNotification()
        {
            string username_current = Session["userLoginSystem"].ToString();
            var acc = AccountController.GetByUsername(username_current);
            if (acc != null)
            {
                ltrUsername.Text = acc.Username;              

                ltrProfile.Text += "<ul class=\"list\">";
                ltrProfile.Text += "<li class=\"name-mobile name\">" + acc.Username + "</li>";
                ltrProfile.Text += "<li class=\"item\"><a href=\"/thong-tin-nguoi-dung\">Thông tin tài khoản</a></li>";
                ltrProfile.Text += "<li class=\"item\"><a href=\"/danh-sach-don-hang\">Danh sách đơn hàng</a></li>";
                if (acc.RoleID != 1)                
                    ltrProfile.Text += "<li class=\"item\"><a href=\"/manager/orderlist\">Quản trị</a></li>";
                ltrProfile.Text += "<li class=\"item\"><a href=\"/dang-xuat\">Đăng xuất</a></li>";
                ltrProfile.Text += "</ul>";

                int levelID1 = Convert.ToInt32(acc.LevelID);
                string VIP = "Bạc";
                var userLevel = UserLevelController.GetByID(levelID1);
                if (userLevel != null)
                    VIP = userLevel.LevelName;                

                ltrHeaderLeft.Text += "<div class=\"user-name\">" + acc.Username + "</div>";
                ltrHeaderLeft.Text += "<div class=\"wallet\">Số dư: <span>" + string.Format("{0:N0} đ", Convert.ToDouble(acc.Wallet)) + "<span></div>";
                ltrHeaderLeft.Text += "<div class=\"rating-wrapper\">";
                ltrHeaderLeft.Text += "<div class=\"rating\">Cấp độ: " + VIP + "</div>";
                if (levelID1 == 1)
                {
                    ltrHeaderLeft.Text += "<div class=\"rating-img\"><img src=\"/App_Themes/USERMASTER/images/rank-silver.png\" alt=\"\"></div>";
                }
                else if (levelID1 == 2)
                {
                    ltrHeaderLeft.Text += "<div class=\"rating-img\"><img src=\"/App_Themes/USERMASTER/images/rank-gold.png\" alt=\"\"></div>";
                }
                else if (levelID1 == 3)
                {
                    ltrHeaderLeft.Text += "<div class=\"rating-img\"><img src=\"/App_Themes/USERMASTER/images/rank-platinum.png\" alt=\"\"></div>";
                }
                else if (levelID1 == 4)
                {
                    ltrHeaderLeft.Text += "<div class=\"rating-img\"><img src=\"/App_Themes/USERMASTER/images/rank-diamond.png\" alt=\"\"></div>";
                }
                ltrHeaderLeft.Text += "</div>";
            }    
        }
    }
}