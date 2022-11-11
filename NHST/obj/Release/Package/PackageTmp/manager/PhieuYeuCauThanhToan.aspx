<%@ Page Title="Phiếu yêu cầu thanh toán" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="PhieuYeuCauThanhToan.aspx.cs" Inherits="NHST.manager.PhieuYeuCauThanhToan" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .input-mainOderCode {
            padding: 2px 2px;
            width: 100%;
            border-radius: 5px;
            display: inline-block;
            font-size: 15px;
            border: solid 1px #000000;
            margin: 1px 1px 1px 1px;
        }

        .div-mainOrderCode {
            width: 100%;
            display: flex;
            align-items: center;
            padding: 5px;
            /* justify-content: space-between;*/
        }

        .div-label {
            padding: 2px 2px;
            font-size: 15px;
            width: 160px;
            /* border-radius: 5px; */
            text-align: left;
        }

        .total-price {
            display: inline-block;
            padding-bottom: 5px;
            font-weight: bold;
            width: 100%;
        }

        .rTotal {
            display: grid;
        }

        .fl-width {
            float: left;
            width: 30%;
            display: inline-table;
            text-align: left;
        }

        .s-txt {
            text-align: left;
            width: 100%;
            display: flex;
            line-height: 10px;
            justify-content: center;
        }

        span.mg {
            min-width: 130px;
            display: flex;
        }

        span.total {
            min-width: 75px;
            display: flex;
        }

        .order-status {
            width: calc(16.6% - 10px);
            height: 35px;
            line-height: 20px;
            padding: 5px 10px;
            background: #fff;
            outline: 0 !important;
            color: #000;
            border: 1px solid #d0bcbc;
            margin-right: 10px;
            transition: 0.2s ease;
            margin-bottom: 10px;
        }

            .order-status:hover {
                background: #366136;
                color: #fff;
                border: 1px solid #366136;
            }

        .pane-shadow {
            display: flex !important;
            justify-content: space-between;
            flex-wrap: wrap;
        }

        p.s-txt.no-wrap.red-text {
            font-weight: bold;
        }

        .margin-left {
            margin-left: auto;
            margin-right: 10px;
        }

        .order-status.active {
            background: #366136;
            color: #fff;
            border: 1px solid #366136;
        }

        @media only screen and (max-width: 768px) {
            .order-status {
                width: calc(33.33% - 10px);
            }
        }

        .gicungduoc {
            line-height: 2;
            border: 0 !important;
            color: #366136 !important;
            font-weight: bold;
        }

        .pane-shadow .order-status {
            width: calc(16.6& - 5px);
        }

        @media only screen and (max-width: 768px) {
            .pane-shadow .order-status {
                width: calc(33.33% - 5px);
                font-size: 10px;
            }

            .pane-shadow .order-status {
                font-size: 10px;
            }
        }

        @media only screen and (min-width: 769px) and (max-width: 1326px) {
            .pane-shadow .order-status {
                width: calc(25.16% - 5px);
            }
        }

        .text-align {
            text-align: right !important;
        }

        .pane-shadow {
            justify-content: flex-start;
        }

        .extra-btn {
            width: 100%;
            display: flex;
            flex-wrap: wrap;
        }

        .extra-all {
            width: 20%;
            margin-right: 5%;
        }



            .extra-all .order-status {
                width: 100%;
            }

        .order-status {
            width: calc(20% - 10px);
        }

        [type="checkbox"].filled-in:disabled:checked + span.checkColor:not(.lever):after {
            border: 2px solid #F64302;
            background-color: #F64302;
        }




        table {
            font-size: 12px !important;
        }

        .table img {
            max-width: 100%
        }

        .s-txt {
            line-height: 15px !important;
        }


        @media screen and (max-width: 1600px) {
            .pane-shadow .order-status {
                margin-right: 3px
            }
        }
    </style>
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="col s12 page-title">
                <div class="card-panel">
                    <div class="title-flex">
                        <h4 class="title no-margin">Phiếu yêu cầu thanh toán</h4>
                    </div>
                </div>
            </div>

            <div class="list-staff col s12 section" style="display: none">

                <div class="list-table card-panel">
                    <div class="row section">
                        <div class="col s12">

                            <div class="top-table-filter">
                                <div class="sort-tb-wrap">
                                    <div class="filter-link select-sort">
                                        <span>Sắp xếp theo</span>
                                        <asp:DropDownList runat="server" ID="ddlSortType" onchange="SearchSort();">
                                            <asp:ListItem Value="0" Text="--Sắp xếp--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Thời gian tăng"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Thời gian giảm"></asp:ListItem>
                                            <%-- <asp:ListItem Value="3" Text="Trạng thái đơn hàng tăng"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Trạng thái đơn hàng giảm"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="filter-link">
                                        <a href="#" class="btn-icon btn" id="filter-btn"><i class="material-icons">filter_list</i><span>Bộ lọc nâng cao</span></a>
                                    </div>
                                </div>

                                <div class="filter-wrap" style="display: block">
                                    <div class="row">
                                        <div class="input-field col s12 l3">
                                            <asp:DropDownList runat="server" ID="ddlType">
                                                <asp:ListItem Value="0" Selected="True">Tất cả</asp:ListItem>
                                                <asp:ListItem Value="1">ID đơn</asp:ListItem>
                                                <asp:ListItem Value="2">Mã đơn hàng</asp:ListItem>
                                                <asp:ListItem Value="3">Mã vận đơn</asp:ListItem>
                                                <asp:ListItem Value="4">Email khách</asp:ListItem>
                                                <asp:ListItem Value="5">Username</asp:ListItem>
                                                <asp:ListItem Value="6">Số điện thoại khách</asp:ListItem>
                                                <%--  <asp:ListItem Value="7">Tên sản phẩm</asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <label for="select_by">Tìm kiếm theo</label>
                                        </div>
                                        <div class="input-field col s12 l9">
                                            <asp:TextBox runat="server" placeholder="" ID="tSearchName" type="text" onkeypress="myFunction()" class="validate"></asp:TextBox>
                                            <label for="search_name"><span>Nhập Id đơn/ mã đơn/ mã vẫn đơn/ email/ username/ số điện thoại</span></label>
                                        </div>

                                        <div class="input-field col s6 l3">
                                            <asp:TextBox ID="rFD" runat="server" placeholder="" Type="text" class="datetimepicker from-date"></asp:TextBox>
                                            <label>Từ ngày</label>
                                        </div>
                                        <div class="input-field col s6 l3">
                                            <asp:TextBox runat="server" Type="text" placeholder="" ID="rTD" class="datetimepicker to-date"></asp:TextBox>
                                            <label>Đến ngày</label>
                                            <span class="helper-text" data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                                        </div>



                                        <div class="input-field col s6 l3" style="display: none;">
                                            <asp:TextBox runat="server" ID="rPriceFrom" placeholder="" type="number" class="validate from-price" min="0"></asp:TextBox>
                                            <label for="from_price">Giá từ</label>
                                        </div>
                                        <div class="input-field col s6 l3" style="display: none;">
                                            <asp:TextBox runat="server" ID="rPriceTo" placeholder="" type="number" class="validate to-price" min="0"></asp:TextBox>
                                            <label for="to_price" data-error="wrong">Giá đến</label>
                                            <span class="helper-text"
                                                data-error="Vui lòng chọn giá trị lớn hơn giá bắt đầu"></span>
                                        </div>
                                        <div class="input-field col s12 l3">
                                            <asp:ListBox runat="server" SelectionMode="Multiple" class="select_all" ID="ddlStatus">
                                                <asp:ListItem Value="-1">Tất cả</asp:ListItem>
                                                <asp:ListItem Value="0">Đơn mới</asp:ListItem>
                                                <asp:ListItem Value="2">Đơn đã cọc</asp:ListItem>
                                                <asp:ListItem Value="4">Đơn chờ mua hàng</asp:ListItem>
                                                <asp:ListItem Value="5">Đơn đã mua hàng</asp:ListItem>
                                                <asp:ListItem Value="3">Đơn người bán giao</asp:ListItem>
                                                <asp:ListItem Value="6">Kho Trung Quốc nhận hàng</asp:ListItem>
                                                <asp:ListItem Value="7">Trên đường về Việt Nam</asp:ListItem>
                                                <asp:ListItem Value="8">Trong kho Hà Nội</asp:ListItem>
                                                <asp:ListItem Value="11">Đang giao hàng</asp:ListItem>
                                                <asp:ListItem Value="9">Đã thanh toán</asp:ListItem>
                                                <asp:ListItem Value="10">Đã hoàn thành</asp:ListItem>
                                                <asp:ListItem Value="12">Đơn khiếu nại</asp:ListItem>
                                                <asp:ListItem Value="1">Đơn hàng hủy</asp:ListItem>
                                            </asp:ListBox>
                                            <label for="status">Trạng thái</label>
                                        </div>
                                        <div class="input-field col s12 l3">
                                            <asp:DropDownList runat="server" ID="ddlPTVC">
                                                <asp:ListItem Value="-1">Tất cả</asp:ListItem>
                                                <asp:ListItem Value="1">Nhanh - TK</asp:ListItem>
                                                <asp:ListItem Value="4">Nhanh - QC</asp:ListItem>
                                            </asp:DropDownList>
                                            <label>PTVC</label>
                                        </div>

                                        <div class="input-field col s12 l9">
                                        </div>
                                        <div class="input-field col s12 l3">
                                            <label>
                                                <asp:TextBox Enabled="true" ID="cbMaVanDon" unchecked runat="server" type="checkbox" /><span id="lbCheckBox">Đơn không có mã vận đơn</span></label>
                                            <asp:HiddenField runat="server" ID="hdfCheckBox" Value="0" />
                                        </div>

                                        <div class="input-field col s12 l6">
                                        </div>
                                        <div class="col s12 input-field mb-0">
                                            <a class="btnSort btn ">Lọc kết quả</a>
                                        </div>



                                    </div>
                                </div>
                            </div>


                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="list-staff col s12 section">
                <asp:Panel runat="server" ID="pnStaff" Visible="false">
                    <div class="list-table card-panel" style="display: none">
                        <div class="row section">

                            <div class="input-field col s12 l3">
                                <asp:DropDownList runat="server" ID="ddlStaffType" onchange="ChangeStaff($(this))">
                                    <asp:ListItem Value="0" Selected="True">--Chọn loại nhân viên--</asp:ListItem>
                                    <asp:ListItem Value="1">Nhân viên đặt hàng</asp:ListItem>
                                    <asp:ListItem Value="2">Nhân viên kinh doanh</asp:ListItem>
                                </asp:DropDownList>
                                <label for="select_by">Chọn loại nhân viên</label>
                            </div>

                            <div class="hide" id="pnListStaff">

                                <div class="input-field col s12 l3 hide" id="staffdh">
                                    <asp:DropDownList runat="server" ID="ddlStaffDH" AppendDataBoundItems="true"
                                        DataValueField="ID" DataTextField="Username">
                                    </asp:DropDownList>
                                    <label for="select_by">Chọn nhân viên đặt hàng</label>
                                </div>

                                <div class="input-field col s12 l3 hide" id="staffsaler">
                                    <asp:DropDownList runat="server" ID="ddlStaffSaler" AppendDataBoundItems="true"
                                        DataValueField="ID" DataTextField="Username">
                                    </asp:DropDownList>
                                    <label for="select_by">Chọn loại nhân viên</label>
                                </div>

                                <div class="input-field col s12 l3">
                                    <a href="javascript:;" onclick="UpdateStaff($(this))" class="btn">Cập nhật</a>
                                </div>

                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="list-table card-panel">
                    <div class="responsive-tb">
                        <div class="input-field col s12 m4 l2">
                            <a href="javascript:;" class="btn" onclick="btnExcel_Click()">Yêu cầu thanh toán</a>
                        </div>
                        <table class="table bordered highlight striped ">
                            <thead>
                                <tr>
                                    <th>Mã đơn</th>
                                    <th>Website</th>
                                    <th>Mã đơn hàng China</th>
                                    <th>Tiền trên web</th>
                                    <th>Tiền thanh toán thực</th>
                                    <th>Tiền đã cọc</th>
                                    <th>Tiền hoa hồng</th>
                                    <th>Tài khoản thanh toán</th>
                                    <th>Thông tin khách</th>
                                    <th style="min-width: 100px;">Nhân viên
                                        <br />
                                        đặt hàng</th>
                                    <th style="min-width: 50px;">Trạng thái xử lý</th>
                                    <th style="min-width: 100px;">Thao tác</th>
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
    <asp:HiddenField ID="hdfOrderID" runat="server" />
    <asp:HiddenField ID="hdfStatus" runat="server" Value="-1" />
    <asp:HiddenField ID="hdfStaffID" runat="server" />
    <asp:HiddenField ID="hdfType" runat="server" />
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
    <asp:Button ID="buttonExport" runat="server" Style="display: none" Text="Thanh toán tất cả" CssClass="btn primary-btn" OnClick="btnExcel_Click" UseSubmitBehavior="false" />
    <script type="text/javascript">
        function updateNote(obj, ID) {
            var Note = obj.parent().find(".txtNote").val();
            $.ajax({
                type: "POST",
                url: "/manager/PhieuYeuCauThanhToan.aspx/UpdateBrand",
                data: "{ID:'" + ID + "',Note:'" + Note + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var ret = msg.d;
                    if (ret == "ok") {
                        obj.parent().find(".update-info").show();
                    }
                    else {
                        obj.parent().find(".update-info").hide();
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    //alert('lỗi checkend');
                }
            });
        }
        function btnExcel_Click() {
            var c = confirm('Bạn muốn thanh toán ?');
            if (c == true) {
                $(<%=buttonExport.ClientID%>).click();
            }
        }      
        function myFunction() {
            if (event.which == 13 || event.keyCode == 13) {

                $('#<%=btnSearch.ClientID%>').click();
            }
        }
        function SearchSort() {
            $('#<%=btnSearch.ClientID%>').click();
        }    
        $(document).ready(function () {

            if ($('#<%=hdfCheckBox.ClientID%>').val() == 0) {

                $('#<%=cbMaVanDon.ClientID%>').prop("checked", false);
            } else {
                $('#<%=cbMaVanDon.ClientID%>').prop("checked", true);
            }
        });
        $('.btnSort').click(function () {
            $('#<%=btnSearch.ClientID%>').click();
        })
        function ChooseDathang(OrderID, obj) {
            var dathangID = obj.val();
            $.ajax({
                type: "POST",
                url: "/manager/PhieuYeuCauThanhToan.aspx/UpdateStaff",
                data: "{OrderID:'" + OrderID + "',StaffID:'" + dathangID + "',Type:'2'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    if (data != "null") {
                        if (data != "notpermission") {
                            location.reload();
                        }
                        else {
                            alert('Bạn không có quyền');
                        }
                    }
                    else {
                        alert('Vui lòng đăng nhập lại.');
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi checkend');
                }
            });
        }
        function ChooseSaler(OrderID, obj) {
            var SalerID = obj.val();
            $.ajax({
                type: "POST",
                url: "/manager/PhieuYeuCauThanhToan.aspx/UpdateStaff",
                data: "{OrderID:'" + OrderID + "',StaffID:'" + SalerID + "',Type:'1'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    if (data != "null") {
                        if (data != "notpermission") {
                            location.reload();
                        }
                        else {
                            alert('Bạn không có quyền');
                        }
                    }
                    else {
                        alert('Vui lòng đăng nhập lại.');
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi checkend');
                }
            });
        }
        function ChangeStaff(obj) {
            var id = obj.val();
            if (id == 1) {
                $("#pnListStaff").removeClass('hide');
                $("#staffsaler").addClass('hide');
                $("#staffdh").removeClass('hide');
            }
            else if (id == 2) {
                $("#pnListStaff").removeClass('hide');
                $("#staffdh").addClass('hide');
                $("#staffsaler").removeClass('hide');
            }
            else {
                $("#pnListStaff").addClass('hide');
                $("#staffdh").addClass('hide');
                $("#staffsaler").addClass('hide');
            }
        }      
        function CheckStaff(ID) {
            $.ajax({
                type: "POST",
                url: "/manager/PhieuYeuCauThanhToan.aspx/CheckStaff",
                data: "{MainOrderID:'" + ID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert(errorthrow);
                }
            });
        }
    </script>
    <style>
        .fix-width {
            width: 300px;
            line-height: 20px;
            white-space: normal !important;
        }
    </style>
</asp:Content>
