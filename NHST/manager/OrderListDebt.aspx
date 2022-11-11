<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderListDebt.aspx.cs" MasterPageFile="~/manager/adminMasterNew.Master" Inherits="NHST.manager.OrderListDebt" %>

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
            <div class="col s12 page-title">
                <div class="card-panel">
                    <div class="title-flex">
                        <h4 class="title no-margin">Đơn hàng mua hộ công nợ</h4>
                    </div>
                </div>
            </div>
            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="row section">
                        <div class="input-field col s12 l9">
                            <asp:TextBox runat="server" placeholder="" ID="tSearchName" type="text" onkeypress="myFunction()" class="validate"></asp:TextBox>
                            <label for="search_name"><span>Username</span></label>
                        </div>
                        <div class="input-field col s12 l3">
                            <a class="btnSort btn ">Lọc kết quả</a>
                        </div>
                        <div class="clearfix"></div>
                        <div class="input-field col s12 l4">
                            <p style="font-size: 16px; font-weight: bold;">
                                Tổng tiền còn thiếu: <span style="color: #F64302;">
                                    <asp:Literal runat="server" ID="ltrNotPay"></asp:Literal>
                                    đ</span>
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="responsive-tb">
                        <table class="table bordered highlight">
                            <thead>
                                <tr>
                                    <th>ID đơn</th>
                                    <th>UserName</th>
                                    <th>Tổng tiền</th>
                                    <th>Đã trả</th>
                                    <th>Còn lại</th>
                                    <th>NV Sales</th>
                                    <th>NV Đặt hàng</th>
                                    <th>Trạng thái</th>
                                    <th>Ngày đơn về</th>
                                    <th>Thao tác</th>
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
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
    <script type="text/javascript">
        $('.btnSort').click(function () {
            $('#<%=btnSearch.ClientID%>').click();
        })
    </script>
</asp:Content>
