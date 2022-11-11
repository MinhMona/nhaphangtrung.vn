<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionList.aspx.cs" MasterPageFile="~/manager/adminMasterNew.Master" Inherits="NHST.manager.SessionList" %>

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
                    <h4 class="title no-margin" style="display: inline-block;">Quản lý Phiên làm việc</h4>
                </div>
            </div>
            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="row">
                        <div class="search-name col s6 m4 l3 input-field">
                            <asp:TextBox runat="server" placeholder="Nhập mã phiên" ID="search_name" type="text"></asp:TextBox>
                        </div>
                        <div class="input-field col s6 m4 l3">
                            <asp:DropDownList runat="server" ID="ddlStatus">
                                <asp:ListItem Value="0" Text="Tất cả"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Đang hoạt động"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Đã kết thúc"></asp:ListItem>
                            </asp:DropDownList>
                            <label>Trạng thái</label>
                        </div>
                        <div class="input-field col s6 m4 l3">
                            <span class="search-action btn">Tìm kiếm</span>
                            <asp:Button runat="server" ID="btnSearch" Style="display: none" OnClick="btnSearch_Click" />
                        </div>
                    </div>
                    <div class="list-package-wrap  mt-2">
                        <div class="package-wrap accent-2">
                            <div class="row">
                                <div class="col s12">
                                    <div class="list-bag">
                                        <div class="responsive-tb">
                                            <table class="table highlight bordered ">
                                                <thead>
                                                    <tr>
                                                        <th>ID</th>
                                                        <th>Mã Phiên</th>
                                                        <th>Ghi chú</th>
                                                        <th>Cân nặng (kg)</th>
                                                        <th>Tổng số kiện</th>
                                                        <th>Trạng thái</th>
                                                        <th>Người tạo</th>
                                                        <th>Ngày tạo</th>
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
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>     
        function myFunction() {
            if (event.which == 13 || event.keyCode == 13) {
                console.log($('#<%=search_name.ClientID%>').val());
                $('#<%=btnSearch.ClientID%>').click();
            }
        }
        $('.search-action').click(function () {
            console.log('dkm ngon');
            console.log($('#<%=search_name.ClientID%>').val());
            $('#<%=btnSearch.ClientID%>').click();
        })
    </script>
</asp:Content>
