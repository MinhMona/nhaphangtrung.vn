using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class push_noti_app : System.Web.UI.Page
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
                    string Username = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(Username);
                    if (ac.RoleID == 0 || ac.RoleID == 2)
                    {

                    }
                    else
                    {
                        Response.Redirect("/trang-chu");
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();

            DateTime currentDate = DateTime.Now;
            string backlink = "/manager/Noti-app-list.aspx";
            var kq = AppPushNotiController.Insert(txtTitle.Text, txtMessage.Text, currentDate, username);
            if (kq != null)
            {
                PJUtils.ShowMessageBoxSwAlertBackToLink("Thông báo thành công.", "s", true, backlink, Page);
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Có lỗi trong quá trình tạo thông báo. Vui lòng thử lại.", "e", true, Page);
            }
        }
    }
}