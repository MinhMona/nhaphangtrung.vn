<%@ Page Language="C#" Title="Ý KIẾN KHÁCH HÀNG" AutoEventWireup="true" MasterPageFile="~/manager/adminMasterNew.Master" CodeBehind="ContactCustomer.aspx.cs" Inherits="NHST.manager.ContactCustomer" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">DANH SÁCH Ý KIẾN KHÁCH HÀNG</h4>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="filter">
                        <div class="row">
                            <div class="input-field col s6 m4 l2">
                                <asp:TextBox runat="server" type="text" ID="search_name" placeholder="" class=""></asp:TextBox>
                                <label>Họ Tên</label>
                            </div>
                            <div class="input-field col s6 m4 l2" style="display:none">
                                <asp:TextBox runat="server" type="text" ID="search_phone" placeholder="" class=""></asp:TextBox>
                                <label>Số điện thoại</label>
                            </div>
                            <div class="input-field col s6 m4 l2" style="display:none">
                                <asp:DropDownList runat="server" ID="ddlStatus">
                                    <asp:ListItem Value="0" Text="Tất cả"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Chờ duyệt"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Đã xử lý"></asp:ListItem>
                                </asp:DropDownList>
                                <label>Trạng thái</label>
                            </div>
                            <div class="input-field col s12 m4 l2">
                                <a href="javascript:;" onclick="myFunction()" class="btn btnsearch">Lọc</a>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <table class="table responsive-table bordered highlight">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Tên khách hàng</th>
                                <th>Email / Số điện thoại</th> 
                                <th>Ý kiến khách hàng</th>
                                <th>Ngày gửi</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Literal ID="ltr" runat="server" EnableViewState="false"></asp:Literal>
                        </tbody>
                    </table>
                    <div class="pagi-table float-right mt-2">
                        <%this.DisplayHtmlStringPaging1();%>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfID" runat="server" />
    <asp:HiddenField ID="hdfFullName" runat="server" />
    <script>
        function UpdateStatus(ID,obj) {

            $.ajax({
                type: "POST",
                url: "/manager/ContactCustomer.aspx/UpdateStatus",
                data: '{ID: "' + ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data =msg.d;
                    if (data == "ok") {
                        obj.parent().parent().parent().find('.checkstatus').attr('checked', 'true');
                    }
                    else
                        swal("Error", "Không thành công", "error");
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    swal("Error", "Fail updateInfoAcc", "error");
                }
            });
        }

        function myFunction() {
            $('#<%=btnSearch.ClientID%>').click();
        }

        $('.search-action').click(function () {
            console.log('dkm ngon');
            console.log($('#<%=search_name.ClientID%>').val());
            $('#<%=btnSearch.ClientID%>').click();
        })
        $(document).ready(function () {
            $('#search_name').autocomplete({
                data: {
                    "Apple": null,
                    "Microsoft": null,
                    "Google": 'https://placehold.it/250x250',
                    "Asgard": null
                },
            });
        });
    </script>
</asp:Content>
