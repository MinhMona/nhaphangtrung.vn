<%@ Page Title="Khiếu nại" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="ComplainList.aspx.cs" Inherits="NHST.manager.ComplainList1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Khiếu nại</h4>
                    <div class="right-action">
                        <a href="#addStaff" class="btn  modal-trigger waves-effect">Thêm khiếu nại</a>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="filter">
                        <div class="row">
                            <div class="input-field col s6 m4 l3">
                                <asp:TextBox runat="server" type="text" ID="search_name" placeholder="" class=""></asp:TextBox>
                                <label>Username</label>
                            </div>
                            <div class="input-field col s6 m4 l3">
                                <asp:DropDownList runat="server" ID="ddlStatus">
                                    <asp:ListItem Value="-1" Text="Tất cả"></asp:ListItem>
                                    <asp:ListItem Value="0">Đã hủy</asp:ListItem>
                                    <asp:ListItem Value="1">Khiếu nại mới</asp:ListItem>
                                    <asp:ListItem Value="2">Đang xử lý</asp:ListItem>
                                    <asp:ListItem Value="4">Gửi admin duyệt</asp:ListItem>
                                    <asp:ListItem Value="3">Đã hoàn thành</asp:ListItem>
                                </asp:DropDownList>
                                <label>Trạng thái</label>
                            </div>
                            <div class="input-field col s12 l3">
                                <asp:DropDownList runat="server" ID="searchNVDH" AppendDataBoundItems="true"
                                    DataValueField="ID" DataTextField="Username">
                                </asp:DropDownList>
                                <label>Đặt hàng</label>
                            </div>
                            <div class="input-field col s12 l3">
                                <asp:DropDownList runat="server" ID="searchCSKH" AppendDataBoundItems="true"
                                    DataValueField="ID" DataTextField="Username">
                                </asp:DropDownList>
                                <label>Chăm sóc khách hàng</label>
                            </div>
                            <div class="input-field col s6 m4 l3">
                                <asp:TextBox runat="server" type="text" ID="rdatefrom" placeholder="" class="datetimepicker from-date"></asp:TextBox>
                                <label>Từ ngày</label>
                            </div>
                            <div class="input-field col s6 m4 l3">
                                <asp:TextBox runat="server" ID="rdateto" type="text" placeholder="" class="datetimepicker to-date"></asp:TextBox>
                                <label>Đến ngày</label>
                                <span class="helper-text"
                                    data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                            </div>
                            <div class="input-field col s12 m4 l3">
                                <a href="javascript:;" onclick="myFunction()" class="btn btnsearch">Tìm kiếm</a>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <table class="table responsive-table bordered highlight">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Username</th>
                                <th>Mã đơn hàng</th>
                                <th>Số lần khiếu nại</th>
                                <th>Nhân viên </br> đặt hàng</th>
                                <th>Nhân viên </br> CSKH</th>
                                <th>Loại khiếu nại</th>
                                <th>Số tiền hoàn</th>
                                <th style="width: 500px !important">Nội dung</th>
                                <th>Trạng thái</th>
                                <th>Ngày tạo</th>
                                <th>Thao tác</th>
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
        <div class="row">
            <div class="bg-overlay"></div>
            <!-- Edit mode -->
            <div class="detail-fixed col s12 m12 l8 xl8 section">
                <div class="rp-detail card-panel row">
                    <div class="col s12">
                        <div class="page-title">
                            <h5>Chi tiết khiếu nại #<asp:Label runat="server" ID="labelID" Text=""></asp:Label></h5>
                            <a href="#!" class="close-editmode top-right valign-wrapper"><i
                                class="material-icons">close</i>Close</a>
                        </div>
                    </div>
                    <div class="col s12">
                        <div class="row pb-2 border-bottom-1 ">
                            <div class="input-field col s12 m4">
                                <asp:TextBox runat="server" ID="txtUserName" BackColor="LightGray" type="text" class="validate" value="chipcop106" Enabled="false"></asp:TextBox>
                                <label for="rp_username">Username</label>
                            </div>
                            <div class="input-field col s12 m4">
                                <asp:TextBox runat="server" ID="txtShopID" BackColor="LightGray" type="text" value="1006" class="validate" Enabled="false"></asp:TextBox>
                                <label for="rp_shopid">Mã đơn hàng</label>
                            </div>
                            <div class="input-field col s12 m4">
                                <asp:TextBox runat="server" BackColor="LightGray" ID="txtCurrence" type="text" class="validate" value="3.506" Enabled="false"></asp:TextBox>
                                <label for="rp_exchange">Tỉ giá</label>
                            </div>
                            <%--<div class="input-field col s12 m6">
                                <asp:TextBox runat="server" ID="txtAmountCYN" type="text" class="validate" onkeyup="CountRealPrice()"></asp:TextBox>
                                <label for="rp_yuan">Số tiền (¥)</label>
                            </div> --%>
                            <div class="input-field col s12 m12">
                                <asp:TextBox runat="server" ID="txtAmountVND" type="text" class="validate"></asp:TextBox>
                                <label for="rp_vnd">Số tiền (VNĐ)</label>
                            </div>
                            <div class="input-field col s12">
                                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtComplainText" placeholder=""
                                    CssClass="materialize-textarea"></asp:TextBox>
                                <label class="active">Nội dung khiếu nại của khách hàng</label>
                            </div>
                            <div class="input-field col s12">
                                <asp:ListBox runat="server" ID="lbStatus">
                                    <asp:ListItem Value="0">Đã hủy</asp:ListItem>
                                    <asp:ListItem Value="1">Khiếu nại mới</asp:ListItem>
                                    <asp:ListItem Value="2">Đang xử lý</asp:ListItem>
                                    <asp:ListItem Value="4">Gửi admin duyệt</asp:ListItem>
                                    <asp:ListItem Value="3">Đã hoàn thành</asp:ListItem>
                                </asp:ListBox>
                                <label>Trạng thái</label>
                            </div>
                            <div class="input-field col s12">
                                <p>Ảnh sản phẩm:</p>
                                <div class="list-img">
                                </div>
                            </div>

                        </div>
                        <div class="row section mt-2">
                            <div class="col s12">
                                <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" UseSubmitBehavior="false" class="btn" Text="Cập nhật"></asp:Button>
                                <a href="#" class="btn close-editmode">Trở về</a>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div id="addStaff" class="modal">
            <div class="modal-hd">
                <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
                <h4 class="no-margin center-align">Thêm khiếu nại mới</h4>
            </div>
            <div class="modal-bd">
                <div class="row">
                    <div class="input-field col s12 m6">
                        <asp:TextBox runat="server" ID="txtOrderID" type="number"></asp:TextBox>
                        <label for="full_name">
                            Mã đơn hàng<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtOrderID" SetFocusOnError="true"
                                ValidationGroup="add" ErrorMessage="(*)" ForeColor="Red"></asp:RequiredFieldValidator></label>
                    </div>
                    <div class="input-field col s12 m6">
                        <asp:TextBox runat="server" ID="txtMoney" type="number"></asp:TextBox>
                        <label for="full_name">
                            Số tiền<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtMoney" SetFocusOnError="true"
                                ValidationGroup="add" ErrorMessage="(*)" ForeColor="Red"></asp:RequiredFieldValidator></label>
                    </div>
                    <div class="input-field col s12 m12">
                        <asp:TextBox runat="server" ID="txtContent" TextMode="MultiLine" type="text" CssClass="materialize-textarea"></asp:TextBox>
                        <label for="full_name">
                            Nội dung khiếu nại<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtContent" SetFocusOnError="true"
                                ValidationGroup="add" ErrorMessage="(*)" ForeColor="Red"></asp:RequiredFieldValidator></label>
                    </div>
                    <div class="input-field col s12 m12">
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control select2">
                            <asp:ListItem Value="0" Text="Chọn hình thức khiếu nại"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Khiếu nại chiết khấu cân nặng"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Khiếu nại hàng bị vỡ, ướt, bẩn"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Khiếu nại chất lượng dịch vụ"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Khiếu nại hàng thiếu, nhầm size, nhầm hàng"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Khiếu nại hàng về chậm"></asp:ListItem>
                        </asp:DropDownList>
                        <label>Lựa chọn khiếu nại</label>
                    </div>
                    <div class="input-field col s12 m12">
                        <span class="black-text">Hình ảnh</span>
                        <div style="display: inline-block; margin-left: 15px;">
                            <input class="upload-img" type="file" onchange="previewFiles(this);" multiple title="">
                            <button type="button" class="btn-upload">Upload</button>
                        </div>
                        <div class="preview-img">
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-ft">
                <div class="ft-wrap center-align">
                    <a href="#!" class="modal-action btn modal-close waves-effect waves-green mr-2" onclick="AddComplain()">Thêm</a>
                    <a class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
                </div>
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hdfID" runat="server" />
    <asp:HiddenField ID="hdfUserName" runat="server" />
    <asp:HiddenField runat="server" ID="hdfListIMG" />
    <asp:Button runat="server" ID="btnSave" Text="Lưu" CssClass="btn primary-btn" ValidationGroup="add" OnClick="btnSaveAdd_Click" UseSubmitBehavior="false" Style="display: none" />
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
    <script>
        function AddComplain() {
            var status = $("#<%=ddlType.ClientID%>").val();
            if (status == 0) {
                alert('Vui lòng chọn hình thức khiếu nại');
                return;
            }
            var base64 = "";
            $(".preview-img img").each(function () {
                base64 += $(this).attr('src') + "|";
            })
            $("#<%=hdfListIMG.ClientID%>").val(base64);
            $("#<%=btnSave.ClientID%>").click();
        }
        function CancelOrder(orderID, obj) {
            var c = confirm('Bạn muốn xóa khiếu nại này: ' + orderID);
            if (c == true) {
                obj.removeAttr("onclick");
                $("#<%=hdfID.ClientID%>").val(orderID);
                $("#<%=btnDelete.ClientID%>").click();
            }
        }
        function Complain(ID) {
            $('.list-img').empty();
            $.ajax({
                type: "POST",
                url: "/manager/ComplainList.aspx/loadinfoComplain",
                data: '{ID: "' + ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data != null) {
                        $('#<%=labelID.ClientID%>').text(ID);
                        $('#<%=txtUserName.ClientID%>').val(data.UserName);
                        $('#<%=hdfUserName.ClientID%>').val(data.UserName);
                        $('#<%=txtShopID.ClientID%>').val(data.ShopID);
                        $('#<%=txtAmountVND.ClientID%>').val(data.AmountVND);
                       <%-- $('#<%=txtAmountCYN.ClientID%>').val(data.AmountCNY);--%>
                        $('#<%=txtCurrence.ClientID%>').val(data.TiGia);
                        $('#<%=txtComplainText.ClientID%>').val(data.ComplainText);
                        $('#<%=hdfID.ClientID%>').val(ID);
                        $('#<%=lbStatus.ClientID%>').val(data.Status);
                        var listIMG = data.ListIMG;
                        if (listIMG != null) {
                            for (var i = 0; i < listIMG.length; i++) {
                                console.log(listIMG[i]);
                                if (listIMG[i] != "") {
                                    var a = "<div class=\"img-block\" style><img class=\"materialboxed\" src =\"" + listIMG[i] + "\" width =\"200\"></div>";
                                    $(".list-img").append(a);
                                }
                            }
                        }

                        $(".materialboxed").materialbox({
                            inDuration: 150,
                            onOpenStart: function (element) {
                                $(element).parents('.material-placeholder').attr('style', 'overflow:visible !important;');
                            },
                            onCloseStart: function (element) {
                                $(element).parents('.material-placeholder').attr('style', '');
                            }
                        });
                        $('select').formSelect();

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
