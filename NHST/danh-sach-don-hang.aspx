<%@ Page Language="C#" MasterPageFile="~/UserNhapHangTrungMaster.Master" AutoEventWireup="true" CodeBehind="danh-sach-don-hang.aspx.cs" Inherits="NHST.danh_sach_don_hang" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <style>
        .yellow-gold.darken-2 {
            background-color: #e87e04 !important;
        }

        .bronze.darken-2 {
            background: #e6cb78;
        }

        .table .action-table > a {
            margin: 5px 3px;
            width: auto;
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
            transition: 0.2s ease;
            margin-bottom: 10px;
        }

            .order-status:hover {
                background: #fdba24;
                color: #fff;
                border: 1px solid #fdba24;
            }

            .order-status.active {
                background: #fdba24;
                color: #fff;
                border: 1px solid #fdba24;
            }

        @media only screen and (max-width: 768px) {
            .order-status {
                width: calc(33.33% - 10px);
            }
        }

        .pane-shadow {
            padding-top: 10px;
            display: flex !important;
            justify-content: space-between;
            flex-wrap: wrap;
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

        .pane-shadow {
            justify-content: flex-start;
        }

        .extra-btn {
            width: 100%;
            display: flex;
            justify-content: space-between;
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

        .collapase_header + .table-info {
            display: block;
        }

        button, html [type="button"], [type="reset"], [type="submit"] {
            border-left: 3px solid var(--color);
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="main">
        <div class="content">
            <div class="row">               
                <div class="col s12">
                    <div class="container">
                        <div class="all-pqd">
                            <div class="card-panel no-shadow">
                                <div class="row">
                                    <div class="col s12">
                                        <div class="page-title center-align">
                                            <h4>Danh sách đơn hàng</h4>
                                        </div>
                                    </div>
                                    <div class="col s12">
                                        <div class="order-list-info">
                                            <div class="total-info mb-2 pb-10">
                                                <div class="row section">
                                                    <div class="col s12 m6">
                                                        <div class="title-flex collapase_header">
                                                            <span class="font-weight-700 table-title">THÔNG TIN ĐƠN HÀNG</span>
                                                            <a href="javascript:;" class="collapse"><i class="material-icons">arrow_drop_down_circle</i></a>
                                                        </div>
                                                        <div class="table table-info   ">
                                                            <table>
                                                                <tbody>
                                                                    <%-- <tr>
                                                                    <td colspan="2"
                                                                        class="white font-weight-700 table-title">THÔNG TIN
                                                                    ĐƠN HÀNG</td>
                                                                </tr>--%>
                                                                    <tr>
                                                                        <td>Tổng số đơn hàng</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrAllOrderCount" runat="server"></asp:Literal></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Đơn hàng chưa đặt cọc</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrOrderStatus0" runat="server"></asp:Literal></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Đơn hàng đã đặt cọc</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrOrderStatus2" runat="server"></asp:Literal></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Đơn hàng đã đặt hàng</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrOrderStatus5" runat="server"></asp:Literal></span>
                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td>Đơn hàng đã có tại kho TQ</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrOrderStatus6" runat="server"></asp:Literal></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Đơn hàng đã có hàng tại VN</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrOrderStatus7" runat="server"></asp:Literal></span>
                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td>Đơn hàng đã nhận</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrOrderStatus10" runat="server"></asp:Literal></span>
                                                                        </td>
                                                                    </tr>

                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                    <div class="col s12 m6">
                                                        <div class="title-flex collapase_header">
                                                            <span class="font-weight-700 table-title">THÔNG TIN TIỀN HÀNG</span>
                                                            <a href="javascript:;" class="collapse"><i class="material-icons">arrow_drop_down_circle</i></a>
                                                        </div>
                                                        <div class="table table-info   ">
                                                            <table>
                                                                <tbody>
                                                                    <%-- <tr>
                                                                    <td colspan="2"
                                                                        class="white font-weight-700 table-title">THÔNG TIN
                                                                    TIỀN HÀNG</td>
                                                                </tr>--%>
                                                                    <tr>
                                                                        <td>Tổng tiền hàng chưa giao</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrTongtienhangchuagiao" runat="server"></asp:Literal>
                                                                            VNĐ</span></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Tổng tiền hàng cần đặt cọc</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrTongtienhangcandatcoc" runat="server"></asp:Literal>
                                                                            VNĐ</span></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Tổng tiền hàng (đơn hàng cần đặt cọc)</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrTongtienhangdatcoc" runat="server"></asp:Literal>
                                                                            VNĐ</span></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Tổng tiền hàng chờ về kho TQ</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrTongtienhangchovekhotq" runat="server"></asp:Literal>
                                                                            VNĐ</span></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Tổng tiền hàng đã về kho TQ</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrTongtienhangdavekhotq" runat="server"></asp:Literal>
                                                                            VNĐ</span></td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td>Tổng tiền hàng đang ở kho đích</td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrTongtienhangdangokhovn" runat="server"></asp:Literal>
                                                                            VNĐ</span></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Tổng tiền cần thanh toán để lấy hàng trong kho
                                                                        </td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrTongtienhangcanthanhtoandelayhang" runat="server"></asp:Literal>
                                                                            VNĐ</span></td>

                                                                    </tr>

                                                                    <tr>
                                                                        <td>Tổng tiền hàng đã nạp ví
                                                                        </td>
                                                                        <td><span
                                                                            class="teal-text text-darken-4 font-weight-700">
                                                                            <asp:Literal ID="ltrTongtienhangdanapvi" runat="server"></asp:Literal>
                                                                            VNĐ</span></td>

                                                                    </tr>


                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row section mt-1">
                                                    <div class="col s12">
                                                        <a href="javascript:;" class="btn" id="filter-btn">Bộ lọc</a>

                                                        <a href="javascript:;" onclick="depositAllOrder()" class="btn">Đặt cọc tất cả</a>

                                                        <a href="javascript:;" class="btn" id="btnExport">Xuất thống kê</a>
                                                        <%--<a href="javascript:;" onclick="payall()" class="btn">Thanh toán tất cả</a>--%>

                                                        <div class="filter-wrap mb-2" style="display: block;">
                                                            <div class="row">

                                                                <div class="input-field col s12 l2">
                                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                                                                        <asp:ListItem Value="0" Text="Chọn loại tìm kiếm"></asp:ListItem>
                                                                        <asp:ListItem Value="1" Text="ID đơn hàng"></asp:ListItem>
                                                                        <asp:ListItem Value="2" Text="Ghi chú sản phẩm"></asp:ListItem>
                                                                        <asp:ListItem Value="3" Text="Website"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <label for="select_by">Tìm kiếm theo</label>
                                                                </div>

                                                                <div class="input-field col s12 l4">
                                                                    <%--  <input id="search_name" type="text" class="validate">--%>
                                                                    <asp:TextBox ID="txtSearhc" placeholder="" runat="server" CssClass="search_name"></asp:TextBox>
                                                                    <label for="search_name">
                                                                        <span>Nhập ID / tên shop / tên
                                                                    website</span></label>
                                                                </div>
                                                                <div class="input-field col s12 l6">
                                                                    <asp:DropDownList runat="server" ID="ddlStatus">
                                                                        <asp:ListItem Value="-1" Text="Tất cả trạng thái"></asp:ListItem>
                                                                        <asp:ListItem Value="0" Text="Chưa đặt cọc"></asp:ListItem>
                                                                        <asp:ListItem Value="1" Text="Hủy đơn hàng"></asp:ListItem>
                                                                        <asp:ListItem Value="2" Text="Đã đặt cọc"></asp:ListItem>
                                                                        <asp:ListItem Value="4" Text="Đơn chờ mua hàng"></asp:ListItem>
                                                                        <asp:ListItem Value="5" Text="Đã mua hàng"></asp:ListItem>
                                                                        <asp:ListItem Value="3" Text="Đơn người bán giao"></asp:ListItem>
                                                                        <asp:ListItem Value="6" Text="Đã về kho TQ"></asp:ListItem>
                                                                        <asp:ListItem Value="7" Text="Trên đường về VN"></asp:ListItem>
                                                                        <asp:ListItem Value="8" Text="Đã về kho VN"></asp:ListItem>
                                                                        <asp:ListItem Value="11" Text="Đã giao hàng"></asp:ListItem>
                                                                        <asp:ListItem Value="9" Text="Khách đã thanh toán"></asp:ListItem>
                                                                        <asp:ListItem Value="10" Text="Đã hoàn thành"></asp:ListItem>
                                                                        <asp:ListItem Value="12" Text="Đơn khiếu nại"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <label for="status">Trạng thái</label>
                                                                </div>
                                                                <div class="input-field col s6 l6">
                                                                    <%--   <telerik:RadDateTimePicker RenderMode="Lightweight" ID="rFD" ShowPopupOnFocus="true" Width="100%" runat="server"
                                                                    DateInput-CssClass="radPreventDecorate" placeholder="Từ ngày" CssClass="date" DateInput-EmptyMessage="Từ ngày">
                                                                    <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" runat="server">
                                                                    </DateInput>
                                                                </telerik:RadDateTimePicker>--%>
                                                                    <%--  <input type="text" class="datepicker from-date">--%>
                                                                    <asp:TextBox runat="server" ID="FD" placeholder="" CssClass="datetimepicker from-date"></asp:TextBox>
                                                                    <label>Từ ngày</label>
                                                                </div>
                                                                <div class="input-field col s6 l6">
                                                                    <%-- <telerik:RadDateTimePicker RenderMode="Lightweight" ID="rTD" ShowPopupOnFocus="true" Width="100%" runat="server"
                                                                    DateInput-CssClass="radPreventDecorate" placeholder="Đến ngày" CssClass="date" DateInput-EmptyMessage="Đến ngày">
                                                                    <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" runat="server">
                                                                    </DateInput>
                                                                </telerik:RadDateTimePicker>--%>
                                                                    <%-- <input type="text" class="datepicker to-date">--%>
                                                                    <asp:TextBox runat="server" placeholder="" ID="TD" CssClass="datetimepicker to-date"></asp:TextBox>
                                                                    <label>Đến ngày</label>
                                                                    <span class="helper-text"
                                                                        data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                                                                </div>


                                                                <div class="col s12 right-align">
                                                                    <asp:Button ID="btnSear" runat="server"
                                                                        CssClass="btn" OnClick="btnSear_Click" Text="TÌM KIẾM" />

                                                                </div>



                                                            </div>
                                                        </div>

                                                        <div class="responsive-tb">
                                                            <div class="pane-shadow filter-form" id="filter-form1" style="display: block">
                                                                <%--<div class="colum left">
                                   
                                </div>--%>
                                                                <%--  <div class="colum right">--%>

                                                                <div class="extra-btn">
                                                                    <asp:Button ID="bttnAll" runat="server" Style="--color: red" CssClass="order-status btn-1" OnClick="btnAll_Click" />
                                                                    <asp:Button ID="btn0" runat="server" Style="--color: Violet" CssClass="order-status btn0" OnClick="btn0_Click" />
                                                                    <asp:Button ID="btn1" runat="server" Style="--color: black" CssClass="order-status btn1" OnClick="btn1_Click" />
                                                                    <asp:Button ID="btn2" runat="server" Style="--color: Indigo" CssClass="order-status btn2" OnClick="btn2_Click" />
                                                                    <asp:Button ID="btn4" runat="server" Style="--color: Green" CssClass="order-status btn4" OnClick="btn4_Click" />
                                                                    <asp:Button ID="btn5" runat="server" Style="--color: Teal" CssClass="order-status btn5" OnClick="btn5_Click" />
                                                                    <asp:Button ID="btn3" runat="server" Style="--color: Aqua" CssClass="order-status btn3" OnClick="btn3_Click" />
                                                                    <asp:Button ID="btn6" runat="server" Style="--color: SteelBlue" CssClass="order-status btn6" OnClick="btn6_Click" />
                                                                    <asp:Button ID="btn7" runat="server" Style="--color: Navy" CssClass="order-status btn7" OnClick="btn7_Click" />
                                                                    <asp:Button ID="btn8" runat="server" Style="--color: GoldenRod" CssClass="order-status btn8" OnClick="btn8_Click" />
                                                                    <asp:Button ID="btn11" runat="server" Style="--color: Chocolate" CssClass="order-status btn11" OnClick="btn11_Click" />
                                                                    <asp:Button ID="btn9" runat="server" Style="--color: RosyBrown" CssClass="order-status btn9" OnClick="btn9_Click" />
                                                                    <asp:Button ID="btn10" runat="server" Style="--color: blue" CssClass="order-status btn10" OnClick="btn10_Click" />
                                                                    <asp:Button ID="btn12" runat="server" Style="--color: SaddleBrown" CssClass="order-status btn12" OnClick="btn12_Click" />
                                                                    <%--   </div>--%>
                                                                </div>
                                                            </div>
                                                            <%-- <div class="input-field col s12 m4 l2">
                                                            <a href="javascript:;" class="btn" id="btnExport">Xuất thống kê</a>
                                                        </div>--%>
                                                            <table class="table   highlight bordered  centered bordered mt-2">
                                                                <thead>
                                                                    <tr>
                                                                        <th class="center-checkbox">
                                                                            <%-- <label>
                                                                            <input type="checkbox" class="selected-all">
                                                                            <span></span>
                                                                        </label>--%>
                                                                        </th>
                                                                        <th>ID</th>
                                                                        <th>Ảnh
                                                                        <br />
                                                                            sản phẩm</th>
                                                                        <%-- <th>Tổng link</th>--%>
                                                                        <th>Website</th>
                                                                        <th>Tỷ giá</th>
                                                                        <th>Tổng tiền</th>
                                                                        <th>Cọc (%)</th>
                                                                        <th class="tb-date">Số tiền
                                                                        <br />
                                                                            phải cọc</th>
                                                                        <th class="tb-date">Số tiền
                                                                        <br />
                                                                            đã cọc</th>
                                                                        <th class="tb-date">Ngày đặt</th>
                                                                        <th>Trạng thái</th>
                                                                        <th>Thao tác</th>
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
                                                        <div class="result-select">
                                                            <div class="all">
                                                                <div class="fixed-noti">
                                                                    <div class="hide-noti"></div>
                                                                    <div class="noti-wrap">

                                                                        <div class="noti-item GiaoHang" style="display: none">
                                                                            <div class="info">
                                                                                <div class="noti-txt col">
                                                                                    <p>
                                                                                        <span class="count"></span>
                                                                                        đơn hàng <span class="type">yêu cầu giao</span>.
                                                                                    </p>
                                                                                </div>
                                                                            </div>

                                                                            <div class="noti-action col">
                                                                                <a href="#addPackage" class="btn modal-trigger"><span
                                                                                    class="type">Yêu</span> cầu giao</a>
                                                                            </div>
                                                                        </div>

                                                                        <div class="noti-item checkout" style="display: none">
                                                                            <div class="info">
                                                                                <div class="noti-txt col">
                                                                                    <p>
                                                                                        <span class="count">
                                                                                            <asp:Label ID="lblPayordercount" runat="server"></asp:Label></span>
                                                                                        đơn hàng <span class="type">thanh
                                                                                    toán</span>.
                                                                                    </p>
                                                                                </div>
                                                                                <div class="noti-total col">
                                                                                    <p>
                                                                                        Tổng tiền: <span
                                                                                            class="total totalpayselected">12,000,000
                                                                                    VNĐ</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>

                                                                            <div class="noti-action col">
                                                                                <a href="javascript:;" style="display: none" onclick="paySelected()" class="btn"><span
                                                                                    class="type">thanh toán</span> tất
                                                                            cả</a>
                                                                            </div>
                                                                        </div>

                                                                        <div class="noti-item deposit" style="display: none">
                                                                            <div class="info">
                                                                                <div class="noti-txt col">
                                                                                    <p>
                                                                                        <span class="count">
                                                                                            <asp:Label ID="lblDepositOrderCount" runat="server"></asp:Label></span>
                                                                                        đơn hàng <span class="type">đặt
                                                                                    cọc</span>.
                                                                                    </p>
                                                                                </div>
                                                                                <div class="noti-total col">
                                                                                    <p>
                                                                                        Tổng tiền: <span
                                                                                            class="total totaldepositselected">
                                                                                            <asp:Literal runat="server" ID="ltrTotalPriceDeposit"></asp:Literal>
                                                                                            VNĐ</span>
                                                                                    </p>
                                                                                </div>
                                                                            </div>

                                                                            <div class="noti-action col">
                                                                                <a href="javascript:;" onclick="depositSelected()" class="btn"><span class="type">đặt
                                                                                cọc</span> tất cả</a>
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
                                </div>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="addPackage" class="modal modal-small add-package">
        <div class="modal-hd">
            <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
            <h4 class="no-margin center-align">Yêu cầu giao hàng</h4>
        </div>
        <div class="modal-bd">
            <div class="row">
                <div class="input-field col s12">
                    <asp:TextBox runat="server" ID="txtFullName" placeholder="" class="validate" data-type="text-only"></asp:TextBox>
                    <label for="txtFullName">Họ tên</label>
                </div>
                <div class="input-field col s12 m6">
                    <asp:TextBox runat="server" ID="txtPhone" placeholder="" class="validate" data-type="text-only"></asp:TextBox>
                    <label for="txtPhone" class="active">Số điện thoại</label>
                </div>
                <div class="input-field col s12 m6">
                    <asp:TextBox ID="txtAddress" runat="server" class="validate" placeholder="" data-type="text-only"></asp:TextBox>
                    <label for="txtAddress">Địa chỉ</label>
                </div>

                <div class="input-field col s12 m12">
                    <asp:TextBox ID="txtNote" runat="server" class="validate" placeholder="" data-type="text-only"></asp:TextBox>
                    <label for="txtNote">Ghi chú</label>
                </div>
            </div>
        </div>
        <div class="modal-ft">
            <div class="ft-wrap center-align">
                <a href="javascript:;" onclick="YCGSelected()" class="modal-action btn modal-close waves-effect waves-green mr-2 submit-button">Yêu cầu</a>
                <a href="javascript:;" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
            </div>

        </div>
    </div>

    <asp:HiddenField ID="hdfOrderID" runat="server" />
    <asp:HiddenField ID="hdfListOrderID" runat="server" />
    <asp:HiddenField runat="server" ID="hdfShowDep" Value="0" />
    <asp:HiddenField runat="server" ID="hdfShowPay" Value="0" />
    <asp:HiddenField runat="server" ID="hdfShowYCG" Value="0" />
    <asp:Button ID="btnOrderSame" runat="server" OnClick="btnOrderSame_Click" Style="display: none" />
    <asp:Button ID="btnDeposit" runat="server" OnClick="btnDeposit_Click" Style="display: none" />
    <asp:Button ID="btnDeposit100" runat="server" OnClick="btnDeposit100_Click" Style="display: none" />
    <asp:Button ID="btnDeposit80" runat="server" OnClick="btnDeposit80_Click" Style="display: none" />
    <asp:Button ID="btnDepositExtra" runat="server" OnClick="btnDepositExtra_Click" Style="display: none" />
    <asp:Button ID="btnPay" runat="server" OnClick="btnPay_Click" Style="display: none" />
    <asp:Button ID="btnDepositSelected1" runat="server" OnClick="btnDepositSelected1_Click" Style="display: none" UseSubmitBehavior="false" />
    <asp:Button ID="btnPayAlllSelected" runat="server" OnClick="btnPayAlllSelected_Click" Style="display: none" UseSubmitBehavior="false" />
    <asp:Button runat="server" ID="btnYCG" OnClick="btnYCG_Click" UseSubmitBehavior="false" Style="display: none" />
    <asp:Button runat="server" ID="btnDepositAll" OnClick="btnDepositAll_Click" Style="display: none" UseSubmitBehavior="false" />
    <asp:Button runat="server" ID="btnPayAll" OnClick="btnPayAll_Click" Style="display: none" UseSubmitBehavior="false" />
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="buttonExport" runat="server" OnClick="btnExcel_Click" />
    <script>
        $('#btnExport').click(function () {
            $(<%=buttonExport.ClientID%>).click();
        });
    </script>
    <script>
        $(document).ready(function () {
            var table = $('.order-list-info .table');
            $('tr[data-action="other"] td label').hide();
            var listCb = table.find('tbody tr td label input[type="checkbox"]');

            var showDep = $("#<%=hdfShowDep.ClientID%>").val();
            if (showDep != 0) {
                var data = JSON.parse(showDep);
                var totalDep = 0;
                for (var i = 0; i < data.length; i++) {
                    totalDep += data[i].TotalDeposit;
                }
                $('.noti-item.deposit').find('.count').text(data.length);
                $(".totaldepositselected").html(formatThousands(totalDep) + " VNĐ")
                $('body').find('.result-select').addClass('show');
                $('.noti-item.deposit').show();
            }

            var showPay = $("#<%=hdfShowPay.ClientID%>").val();
            if (showPay != 0) {
                var data = JSON.parse(showPay);
                var totalPay = 0;
                for (var i = 0; i < data.length; i++) {
                    totalPay += data[i].TotalPricePay;
                }
                $('.noti-item.checkout').find('.count').text(data.length);
                $(".totalpayselected").html(formatThousands(totalPay) + " VNĐ")
                $('body').find('.result-select').addClass('show');
                $('.noti-item.checkout').show();
            }

            var showYCG = $("#<%=hdfShowYCG.ClientID%>").val();
            if (showYCG != 0) {
                var data = JSON.parse(showYCG);
                $('.noti-item.GiaoHang').find('.count').text(data.length);
                $('body').find('.result-select').addClass('show');
                $('.noti-item.GiaoHang').show();
            }


            //table.on('change', 'tbody input[type="checkbox"]', function (e) {
            //    var checkedList = table.find('tbody tr td label input[type="checkbox"]:checked');
            //    var type = $(this).parents('tr').attr('data-action');
            //    if (checkedList.length < 1) {
            //        $('body').find('.result-select').removeClass('show');

            //    } else {
            //        $('body').find('.result-select').addClass('show');
            //    }
            //    var listDep = table.find(
            //        'tbody tr[data-action="deposit"] td label input[type="checkbox"]:checked');
            //    var listCheckout = table.find(
            //        'tbody tr[data-action="checkout"] td label input[type="checkbox"]:checked');
            //    $('.noti-item.deposit').find('.count').text(listDep.length);
            //    $('.noti-item.checkout').find('.count').text(listCheckout.length);
            //    if (listDep.length < 1) {
            //        $('.noti-item.deposit').hide();
            //    } else {
            //        var amount = 0;
            //        listDep.each(function () {
            //            amount += parseFloat($(this).attr('data-value'));
            //        })
            //        $(".totaldepositselected").html(formatThousands(amount) + " VNĐ")
            //        $('.noti-item.deposit').show();
            //    }
            //    if (listCheckout.length < 1) {
            //        $('.noti-item.checkout').hide();
            //    } else {
            //        var amount = 0;
            //        listCheckout.each(function () {
            //            amount += parseFloat($(this).attr('data-value'));
            //        })
            //        $(".totalpayselected").html(formatThousands(amount) + " VNĐ")
            //        $('.noti-item.checkout').show();
            //    }
            //})
            //$('body').on('click', '.selected-all', function () {
            //    var checkStatus = this.checked;
            //    var listCb = table.find(
            //        'tbody tr:not([data-action="other"]) td label input[type="checkbox"]');
            //    $(listCb).each(function () {
            //        $(this).prop('checked', checkStatus);
            //        $(this).trigger('change');
            //    });
            //})

            $('.collapase_header .collapse').on('click', function () {
                $(this).toggleClass('active');
                $(this).parent().next().slideToggle();

            });

        });

        function OrderSame(orderID) {
            var c = confirm('Bạn muốn đặt đơn hàng tương tự đơn : ' + orderID);
            if (c == true) {
                $("#<%=hdfOrderID.ClientID%>").val(orderID);
                $("#<%=btnOrderSame.ClientID%>").click();
            }
        }


        function isEmpty(str) {
            return !str.replace(/^\s+/g, '').length; // boolean (`true` if field is empty)
        }

        var formatThousands = function (n, dp) {
            var s = '' + (Math.floor(n)), d = n % 1, i = s.length, r = '';
            while ((i -= 3) > 0) { r = ',' + s.substr(i, 3) + r; }
            return s.substr(0, i + 3) + r + (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
        };

        function depositOrder(orderID, obj) {
            var c = confirm('Bạn muốn đặt cọc đơn: ' + orderID);
            if (c == true) {
                obj.removeAttr("onclick");
                $("#<%=hdfOrderID.ClientID%>").val(orderID);
                $("#<%=btnDeposit.ClientID%>").click();
            }
        }

        function depositOrder100(orderID, obj) {
            var c = confirm('Bạn muốn đặt cọc 100% đơn: ' + orderID);
            if (c == true) {
                obj.removeAttr("onclick");
                $("#<%=hdfOrderID.ClientID%>").val(orderID);
                $("#<%=btnDeposit100.ClientID%>").click();
            }
        }

        function depositOrder80(orderID, obj) {
            var c = confirm('Bạn muốn đặt cọc 70% đơn: ' + orderID);
            if (c == true) {
                obj.removeAttr("onclick");
                $("#<%=hdfOrderID.ClientID%>").val(orderID);
                $("#<%=btnDeposit80.ClientID%>").click();
            }
        }

        function depositOrderExtra(orderID, obj) {
            var c = confirm('Bạn muốn đặt cọc thêm cho đơn: ' + orderID);
            if (c == true) {
                obj.removeAttr("onclick");
                $("#<%=hdfOrderID.ClientID%>").val(orderID);
                $("#<%=btnDepositExtra.ClientID%>").click();
            }
        }

        function payallorder(orderID) {
            var r = confirm("Bạn muốn thanh toán đơn hàng này?");
            if (r == true) {
                $("#<%=hdfOrderID.ClientID%>").val(orderID);
                $("#<%= btnPay.ClientID%>").click();
            }
            else {
            }
        }

        function depositAllOrder() {
            var c = confirm('Bạn muốn đặt cọc đơn tất cả?');
            if (c == true) {
                $("#<%=btnDepositAll.ClientID%>").click();
            }
        }

        function payall() {
            var r = confirm("Bạn muốn thanh toán tất cả?");
            if (r == true) {
                $("#<%= btnPayAll.ClientID%>").click();
            }
            else {
            }
        }

        function CheckDepAll(ID, TotalPrice) {
            $.ajax({
                type: "POST",
                url: "/danh-sach-don-hang.aspx/CheckDepAll",
                data: "{MainOrderID:'" + ID + "',TotalPrice:'" + TotalPrice + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data.length > 0) {
                        var totalDep = 0;
                        for (var i = 0; i < data.length; i++) {
                            totalDep += data[i].TotalDeposit;
                        }
                        $('.noti-item.deposit').find('.count').text(data.length);
                        $(".totaldepositselected").html(formatThousands(totalDep) + " VNĐ")
                        $('body').find('.result-select').addClass('show');
                        $('.noti-item.deposit').show();
                    }
                    else {
                        var count = $('.noti-item.checkout').find('.count').text();
                        var count1 = $('.noti-item.GiaoHang').find('.count').text();
                        $('.noti-item.deposit').find('.count').text("0");
                        $(".totaldepositselected").html("0 VNĐ")
                        if (count == 0 && count1 == 0) {
                            $('body').find('.result-select').removeClass('show');
                        }
                        $('.noti-item.deposit').hide();
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    //alert(errorthrow);
                }
            });
        }

        function CheckPayAll(ID, TotalPricePay) {
            console.log(ID);
            console.log(TotalPricePay);
            $.ajax({
                type: "POST",
                url: "/danh-sach-don-hang.aspx/CheckPayAll",
                data: "{MainOrderID:'" + ID + "',TotalPricePay:'" + TotalPricePay + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data.length > 0) {
                        var totalPay = 0;
                        for (var i = 0; i < data.length; i++) {
                            totalPay += data[i].TotalPricePay;
                        }
                        $('.noti-item.checkout').find('.count').text(data.length);
                        $(".totalpayselected").html(formatThousands(totalPay) + " VNĐ")
                        $('body').find('.result-select').addClass('show');
                        $('.noti-item.checkout').show();
                    }
                    else {
                        var count = $('.noti-item.deposit').find('.count').text();
                        var count1 = $('.noti-item.GiaoHang').find('.count').text();
                        $('.noti-item.checkout').find('.count').text("0");
                        $(".totalpayselected").html("0 VNĐ")
                        if (count == 0 && count1 == 0) {
                            $('body').find('.result-select').removeClass('show');
                        }

                        $('.noti-item.checkout').hide();
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    //alert(errorthrow);
                }
            });
        }

        function CheckYCG(ID) {
            $.ajax({
                type: "POST",
                url: "/danh-sach-don-hang.aspx/CheckYCGAll",
                data: "{MainOrderID:'" + ID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data.length > 0) {
                        var totalPay = 0;
                        for (var i = 0; i < data.length; i++) {
                            totalPay += data[i].TotalPricePay;
                        }
                        $('.noti-item.GiaoHang').find('.count').text(data.length);
                        $('body').find('.result-select').addClass('show');
                        $('.noti-item.GiaoHang').show();
                    }
                    else {
                        var count = $('.noti-item.deposit').find('.count').text();
                        var count1 = $('.noti-item.checkout').find('.count').text();
                        $('.noti-item.GiaoHang').find('.count').text("0");
                        $(".totalpayselected").html("0 VNĐ")
                        if (count == 0 && count1 == 0) {
                            $('body').find('.result-select').removeClass('show');
                        }
                        $('.noti-item.checkout').hide();
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    //alert(errorthrow);
                }
            });
        }


        function depositSelected() {
            var c = confirm('Bạn muốn đặt cọc tất cả đơn hàng đã chọn?');
            if (c == true) {
                $("#<%=btnDepositSelected1.ClientID%>").click();
            }
        }

        function paySelected() {
            var c = confirm('Bạn muốn thanh toán tất cả đơn hàng đã chọn?');
            if (c == true) {
                $("#<%=btnPayAlllSelected.ClientID%>").click();
            }
        }

        function YCGSelected() {
            var c = confirm('Bạn muốn yêu cầu giao hàng tất cả đơn hàng đã chọn?');
            if (c == true) {
                var fname = $("#<%=txtFullName.ClientID%>").val();
                var phone = $("#<%=txtPhone.ClientID%>").val();
                var address = $("#<%=txtAddress.ClientID%>").val();
                if (!isEmpty(fname) && !isEmpty(phone) && !isEmpty(address)) {
                    $("#<%=btnYCG.ClientID%>").click();
                }
                else {
                    alert('Vui lòng nhập đầy đủ thông tin');
                }
            }
        }

    </script>
</asp:Content>
