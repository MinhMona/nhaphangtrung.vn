using NHST.Bussiness;
using NHST.Controllers;
using NHST.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] != null)
                {
                    string username_current = Session["userLoginSystem"].ToString();
                    var obj_user = AccountController.GetByUsername(username_current);
                    if (obj_user != null)
                    {
                        int role = Convert.ToInt32(obj_user.RoleID);

                        if (role == 3 || role == 9)
                            Response.Redirect("/manager/OrderList.aspx");
                        else if (role == 4)
                            Response.Redirect("/manager/TQWareHouse-DHH.aspx");
                        else if (role == 5)
                            Response.Redirect("/manager/VNWarehouse-DHH.aspx");
                        else if (role == 6)
                            Response.Redirect("/manager/Sale-Home.aspx");
                        else
                            Response.Redirect("/manager/home.aspx");
                    }
                }               
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                if (password.Length > 2)
                {
                    var ac = AccountController.Login(username.Trim().ToLower(), password.Trim());
                    var check = hdfCB.Value;
                    if (ac != null)
                    {
                        ChatHub ch = new ChatHub();
                        if (!string.IsNullOrEmpty(ac.LoginStatus))
                        {
                            Session["StateLogin"] = ac.LoginStatus;
                        }
                        else
                        {
                            Session["StateLogin"] = "1";
                        }
                        if (ac.Status == 2)
                        {
                            Session["StateLogin"] = TokenSession.CreateAndStoreSessionToken(txtUsername.Text);
                            ac = AccountController.GetByID(ac.ID);
                            ch.Login(ac.ID.ToString(), ac.LoginStatus);

                            int role = Convert.ToInt32(ac.RoleID);
                            if (role != 1)
                            {
                                Session["userLoginSystem"] = username;
                                if (check == "1")
                                {
                                    Response.Cookies["Username"].Expires = DateTime.UtcNow.AddHours(7).AddDays(30);
                                    Response.Cookies["Password"].Expires = DateTime.UtcNow.AddHours(7).AddDays(30);
                                }
                                Response.Cookies["Username"].Value = username;
                                Response.Cookies["Password"].Value = password;

                                if (role == 3 || role == 9)
                                    Response.Redirect("/manager/OrderList.aspx");
                                else if (role == 4)
                                    Response.Redirect("/manager/TQWareHouse-DHH.aspx");
                                else if (role == 5)
                                    Response.Redirect("/manager/VNWarehouse-DHH.aspx");
                                else if (role == 6)
                                    Response.Redirect("/manager/OrderList.aspx");
                                else
                                    Response.Redirect("/manager/home.aspx");
                                //PJUtils.ShowMessageBoxSwAlert("Vui lòng liên hệ admin để đăng nhập.", "e", false, Page);
                            }
                            else
                            {
                                //Session["userLoginSystem"] = username;
                                //if (check == "1")
                                //{
                                //    Response.Cookies["Username"].Expires = DateTime.Now.AddDays(30);
                                //    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                                //}
                                //Response.Cookies["Username"].Value = username;
                                //Response.Cookies["Password"].Value = password;
                                //Response.Redirect("/danh-sach-don-hang?t=1");
                                PJUtils.ShowMessageBoxSwAlert("Tài khoản của bạn không đủ quyền đăng nhập.", "e", false, Page);
                            }
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Tài khoản của bạn đã bị khóa.", "e", false, Page);
                        }
                    }
                    else
                    {
                        PJUtils.ShowMessageBoxSwAlert("Sai Username hoặc Password, vui lòng kiểm tra lại.", "e", false, Page);
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Sai Username hoặc Password, vui lòng kiểm tra lại.", "e", false, Page);
                }
            }
        }
    }
}