<%@ Page Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="Session-Detail.aspx.cs" Inherits="NHST.manager.Session_Detail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Quản lý phiên làm việc chi tiết</h4>
                </div>
            </div>
            <div class="list-staff col s12 section">
                <div class="list-table">
                    <div class="row">
                        <div class="col s12 m3">
                            <div class="card-panel">
                                <h6 class="black-text">Thông tin phiên làm việc</h6>
                                <hr class="mb-5" />
                                <div class="row">
                                    <div class="input-field col s12">
                                        <asp:TextBox ID="txtPackageCode" runat="server" type="text" Enabled="false"></asp:TextBox>
                                        <label for="txtPackageCode">Mã phiên làm việc</label>
                                    </div>
                                     <div class="input-field col s12">
                                        <asp:TextBox ID="txtPackageNote" runat="server" type="text"></asp:TextBox>
                                        <label for="txtPackageNote">Ghi chú</label>
                                    </div>
                                    <div class="input-field col s12">
                                        <telerik:RadNumericTextBox runat="server" CssClass="" Skin="MetroTouch"
                                            ID="pWeight" MinValue="0" NumberFormat-DecimalDigits="2" Enabled="false"
                                            NumberFormat-GroupSizes="3" Width="100%" Value="0">
                                        </telerik:RadNumericTextBox>
                                        <label for="pWeight" class="active">Cân (kg)</label>
                                    </div>
                                    <div class="input-field col s12">
                                        <telerik:RadNumericTextBox runat="server" CssClass="" Skin="MetroTouch"
                                            ID="pTotalPackage" MinValue="0" NumberFormat-DecimalDigits="0" Enabled="false"
                                            NumberFormat-GroupSizes="3" Width="100%" Value="0">
                                        </telerik:RadNumericTextBox>
                                        <label for="pTotalPackage" class="active">Tổng số kiện</label>
                                    </div>
                                    <div class="input-field col s12">
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="">
                                            <asp:ListItem Value="1" Text="Đang hoạt động"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Đã kết thúc"></asp:ListItem>
                                        </asp:DropDownList>
                                        <label for="ddlStatus">Trạng thái</label>
                                    </div>
                                    <asp:Panel runat="server" ID="pnAdmin" Visible="false">
                                        <div class="input-field col s12">
                                            <asp:Button ID="btncreateuser" runat="server" Text="Cập nhật" CssClass="btn" OnClick="btncreateuser_Click" UseSubmitBehavior="false" />
                                            <%-- <asp:Button ID="btnBack" runat="server" Text="Trở về" CssClass="btn" OnClick="btnBack_Click" UseSubmitBehavior="false" />
                                        <asp:Literal ID="ltrCreateSmallpackage" runat="server" Visible="false"></asp:Literal>--%>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div class="col s12 m9">
                            <div class="card-panel">
                                <h6 class="black-text">Danh sách mã vận đơn</h6>
                                <hr class="mb-5" />
                                <div class="row">
                                    <div class="col s12 m12 l6">
                                        <div class="search-name input-field no-margin full-width">
                                            <asp:TextBox runat="server" ID="tSearchName" CssClass="validate autocomplete barcode" placeholder="Nhập mã vận đơn"></asp:TextBox>
                                            <span class="bg-barcode"></span>
                                            <span class="material-icons search-action">search</span>
                                            <asp:Button runat="server" ID="btnSearch" Style="display: none" OnClick="btnSearch_Click" />
                                        </div>
                                    </div>
                                    <div class="col s12 mt-2">
                                        <div class="list-package">
                                            <div class="package-item">
                                                <span class="owner">
                                                    <asp:Literal ID="ltrPackageName" runat="server" EnableViewState="false"></asp:Literal></span>
                                                <div class="responsive-tb">
                                                    <table class="table  centered bordered ">
                                                        <thead>
                                                            <tr class="teal darken-4">
                                                                <th>STT</th>
                                                                <th>Username</th>
                                                                <th>Mã đơn hàng</th>
                                                                <th>Mã vận đơn</th>
                                                                <th>Cân nặng (kg)</th>
                                                                <th>Dài (cm)</th>
                                                                <th>Rộng (cm)</th>
                                                                <th>Cao (cm)</th>
                                                                <th>Ghi chú kiểm hàng</th>
                                                                <th>Ghi chú</th>
                                                                <th>Trạng thái</th>
                                                                <th>Ngày tạo</th>
                                                                <th>Action</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Literal ID="ltr" runat="server" EnableViewState="false"></asp:Literal>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="pagi-table float-right mt-2">
                                                    <%this.DisplayHtmlStringPaging1();%>
                                                </div>
                                                <div class="clearfix"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">               
        $('.search-action').click(function () {
            $('#<%=btnSearch.ClientID%>').click();
        })
        function deleteTrade(ID) {
            var r = confirm("Bạn muốn xóa kiện này ra khỏi phiên?");
            if (r == true) {
                $.ajax({
                    type: "POST",
                    url: "/manager/Session-Detail.aspx/UpdateStatus",
                    data: "{ID:'" + ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var ret = msg.d;
                        if (ret == 1) {
                            window.location.reload(true);
                        }
                        else {
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        //alert(errorthrow);
                    }
                });
            }
            else {
            }
        }
    </script>
</asp:Content>
