<%@ Page Title="Xuất kho" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="OutStock.aspx.cs" Inherits="NHST.manager.OutStock1" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <link rel="stylesheet" type="text/css" href="/App_Themes/AdminNew45/assets/js/lightgallery/css/lightgallery.min.css">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Xuất kho</h4>
                </div>
            </div>
            <div class="list-staff col s12 m12 l12 section">
                <div class="list-table card-panel">
                    <div class="filter">
                        <div class="search-name input-field no-margin">
                            <input placeholder="Nhập username" id="username" type="text" class="validate autocomplete">
                        </div>
                        <div class="select-bao">
                            <div class="input-field inline">
                                <input placeholder="Nhập mã đơn hàng" id="txtOrderID" type="text"
                                    class="validate autocomplete no-margin">
                            </div>
                            <div class="input-field inline" style="display: none;">
                                <select class="ordertypeget">
                                    <option value="0">Tất cả đơn</option>
                                    <option value="1">ĐH mua hộ</option>
                                    <option value="2">ĐH vận chuyển hộ</option>
                                </select>
                            </div>
                            <div class="input-field inline">
                                <select class="Waretypeget">
                                    <option value="0">Tất cả kho</option>
                                    <option value="1">Kho Hà Nội</option>
                                    <option value="3">Kho Hồ Chí Minh</option>
                                </select>
                            </div>
                            <a href="javascript:;" class="btn modal-trigger waves-effect" onclick="getpackagebyoID()">Lấy kiện</a>
                        </div>
                        <div class="search-name input-field no-margin mg-bt-1">
                            <input placeholder="Nhập mã vận đơn" id="barcode-input" type="text"
                                class="validate autocomplete barcode">
                            <div class="bg-barcode"></div>
                            <%-- <span class="material-icons search-action">search</span>--%>
                        </div>
                        <a href="javascript:;" id="xuatkhotatca" style="display: none" onclick="xuatkhotatcakien();" class="btn waves-effect modal-trigger mt-1">Xuất kho tất cả kiện</a>

                    </div>
                    <div class="row">
                        <div class="input-field col s12 l3">
                            <p style="font-size: 16px; font-weight: bold;">
                                Tổng kiện đã về kho đích: <span style="color: #F64302;">
                                    <span ID="ltrPackage">0</span>
                                </span>
                            </p>
                        </div>
                        <div class="input-field col s12 l3">
                            <p style="font-size: 16px; font-weight: bold;">
                                Số kiện đã scan: <span style="color: #F64302;">
                                    <span ID="ltrPackageScan">0</span>
                                </span>
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="input-field col s12 l3">
                            <p style="font-size: 16px; font-weight: bold;">
                                Tổng tiền cần thanh toán: <span style="color: #F64302;">
                                    <span ID="ltrMoneys">0</span>
                                đ</span>
                            </p>
                        </div>
                        <div class="input-field col s12 l3">
                            <p style="font-size: 16px; font-weight: bold;">
                                Số dư của khách: <span style="color: #F64302;">
                                    <span ID="ltrMoneyUser">0</span>
                                đ</span>
                            </p>
                        </div>
                        <div class="input-field col s12 l3">
                            <p style="font-size: 16px; font-weight: bold;">
                                Số tiền cần nạp thêm: <span style="color: #F64302;">
                                    <span ID="ltrMoneyAppro">0</span>
                                    đ</span>
                            </p>
                        </div>
                    </div>
                    <div class="list-package">
                        <div class="package-wrap accent-2">
                            <div class="row">
                                <div class="col s12 m12 l12 xl9">
                                    <div class="list-package export" id="listpackage">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:TextBox runat="server" Style="display: none" ID="txtFullname" CssClass="form-control has-validate"
        placeholder="Họ tên người nhận">
    </asp:TextBox>
    <asp:TextBox runat="server" Style="display: none" ID="txtPhone" CssClass="form-control has-validate"
        placeholder="Số điện thoại">
    </asp:TextBox>
    <asp:Button ID="btnAllOutstock" runat="server" UseSubmitBehavior="false" OnClick="btnAllOutstock_Click" Style="display: none" OnClientClick="document.forms[0].target = '_blank';" />
    <asp:HiddenField ID="hdfListPID" runat="server" />
    <asp:HiddenField ID="hdfUsername" runat="server" />

    <asp:HiddenField ID="hdfListBarcode" runat="server" />
    <script src="/App_Themes/AdminNew45/assets/js/lightgallery/js/lightgallery-all.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#barcode-input').focus();
            $('#barcode-input').keydown(function (e) {
                if (e.key === 'Enter') {
                    //getCodeNew
                    getCodeNew($(this));
                    e.preventDefault();
                    return false;
                }
            });
        });

        function isEmpty(str) {
            return !str.replace(/^\s+/g, '').length; // boolean (`true` if field is empty)
        }

        var odID = 0;

        function getCodeNew(obj) {
            var bc = obj.val();
            var username = $("#username").val();
            var waretype = $(".Waretypeget option:selected").val();
            if (isEmpty(bc)) {
                alert('Vui lòng không để trống barcode');
            }
            else if (isEmpty(username)) {
                alert('Vui lòng không để trống username');
            }
            else {
                var listbarcode = $("#<%=hdfListBarcode.ClientID%>").val();
                $.ajax({
                    type: "POST",
                    url: "/manager/outstock.aspx/getpackages",
                    data: "{barcode:'" + bc + "',username:'" + username + "',Ware:'" + waretype + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var ret = msg.d;
                        //alert(bc);
                        if (ret != "none") {
                            if (ret == "notexistuser") {
                                alert('Không tồn tại user trong hệ thống');
                            }
                            else {
                                var listp = JSON.parse(msg.d);
                                if (listp.length > 0) {
                                    for (var i = 0; i < listp.length; i++) {
                                        var p = listp[i];
                                        var html = '';

                                        var pID = p.pID;
                                        var UID = p.uID;
                                        var uname = p.username;
                                        var mID = p.mID;
                                        var tID = p.tID;

                                        var orderid = 0;
                                        if (mID > 0) {
                                            orderid = mID;
                                        }
                                        else if (tID > 0) {
                                            orderid = tID;
                                        }

                                        var weight = p.weight;
                                        var status = p.status;
                                        var getbarcode = p.barcode;
                                        var dIWH = p.dateInWarehouse;
                                        var kiemdem = p.kiemdem;
                                        var donggo = p.donggo;
                                        var baohiem = p.baohiem;
                                        var ordertype = parseFloat(p.OrderType);
                                        var ordertypeString = p.OrderTypeString;
                                        var totalDaysInWare = p.TotalDayInWarehouse


                                        var isExist = false;
                                        if ($(".package-row").length > 0) {
                                            $(".package-row").each(function () {
                                                var dt_packageID = $(this).attr("data-packageID");
                                                if (pID == dt_packageID) {
                                                    isExist = true;
                                                }
                                            });
                                        }


                                        var check = false;
                                        $(".package-item").each(function () {
                                            if ($(this).attr("data-uid") == UID) {
                                                check = true;
                                            }
                                        })

                                        if (isExist == false) {
                                            if (check == false) {
                                                html += "<div class=\"package-item pb-2\" id=\"" + UID + "\" data-uid=\"" + UID + "\">";
                                                html += "<div class=\"responsive-tb\">";
                                                html += "<table class=\"table table-inside centered  table-warehouse\">";
                                                html += "<thead>";
                                                html += "<tr class=\"teal darken-4\">";
                                                html += "<th style=\"min-width: 50px;\">Order ID</th>";
                                                html += "<th style=\"min-width: 50px;\">Loại ĐH</th>";
                                                html += "<th style=\"min-width: 110px;\">Đơn hàng</th>";
                                                html += "<th>Mã vận đơn</th>";
                                                html += "<th style=\"min-width: auto;\">Cân nặng<br />(kg)</th>";
                                                html += "<th class=\"size-th\">Kích thước</th>";
                                                html += "<th style=\"min-width: 100px\">Tổng ngày</br>lưu kho</th>";
                                                html += "<th style=\"min-width: 150px\">Trạng thái</th>";
                                                html += "<th style=\"min-width: 80px;\">Action</th></tr>";
                                                html += "</thead>";
                                                html += "<tbody class=\"orderid" + UID + " dh" + orderid + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";

                                                if (ordertype != 3) {
                                                    html += "<tr><td rowspan=\"10\" class=\"grey lighten-2\">" + orderid + "</td></tr>";
                                                }

                                                //Xét trạng thái đã về kho VN
                                                if (ordertype != 3) {
                                                    if (status == 3)
                                                        html += "<tr class=\"package-row lighten-4 order-id small" + UID + " blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                    else
                                                        html += "<tr class=\"package-row lighten-4 order-id small" + UID + "\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                }
                                                else {
                                                    html += "<tr class=\"package-row lighten-4 order-id small" + UID + "\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                }

                                                if (ordertype == 1) {
                                                    html += "<td><span>" + ordertypeString + "</span></td>";
                                                }
                                                else if (ordertype == 2) {
                                                    html += "<td><span>" + ordertypeString + "</span></td>";
                                                }
                                                else {
                                                    html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                    html += "<input type=\"text\" value=\"\" class=\"tooltipped packageorderID\" data-tooltip=\"\">";
                                                    html += "</td>";

                                                    html += "<td>";
                                                    html += "<div class=\"input-field\">";
                                                    html += "<select class=\"package-status-select packageOrderType\">";
                                                    html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                    html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                    html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                    html += "</select>";
                                                    html += "</div>";
                                                    html += "</td>";
                                                }

                                                html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                html += "<p class=\"black-text\">KD</p>";
                                                if (kiemdem == "Có") {
                                                    html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                }
                                                else {
                                                    html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                }
                                                html += "</div>";

                                                html += "<div class=\"tb-block\">";
                                                html += "<p class=\"black-text\">ĐG</p>";
                                                if (donggo == "Có") {
                                                    html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                }
                                                else {
                                                    html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                }
                                                html += "</div>";

                                                html += "<div class=\"tb-block\ style=\"display: none;\">";
                                                html += "<p class=\"black-text\">BH</p>";
                                                if (baohiem == "Có") {
                                                    html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                }
                                                else {
                                                    html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                }
                                                html += "</div>";

                                                html += "</td>";
                                                html += "<td><span>" + getbarcode + "</span></td>";
                                                html += "<td><span>" + weight + "</span></td>";
                                                html += "<td class=\"size\">";
                                                html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                html += "</p>";
                                                html += "</td>";
                                                html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                if (status == 1) {
                                                    html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                }
                                                else if (status == 2) {
                                                    html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                }
                                                else if (status == 3) {
                                                    html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                }
                                                else if (status == 4) {
                                                    html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                }
                                                else if (status == 5) {
                                                    html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                }
                                                else {
                                                    html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                }

                                                html += "<td>";
                                                html += "<div class=\"action-table\"> ";
                                                if (ordertype == 3)
                                                    html += "<a href=\"#!\" onclick=\"updateOrderType('" + getbarcode + "',$(this),'" + pID + "'," + UID + ", " + orderid + ")\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                html += "</div>";
                                                html += "</td>";
                                                html += "</tr>";
                                                html += "</tbody>";
                                                html += "</table>";
                                                html += "</div>";
                                                html += "</div>";

                                                $("#listpackage").append(html);

                                            }
                                            else {
                                                var MainID = $(".orderid" + UID + "").attr('data-orderid');
                                                if (MainID == orderid) {
                                                    var otype = $(".orderid" + UID + "").attr('data-ordertype');
                                                    if (otype == ordertype) {

                                                        //Xét trạng thái đã về kho VN
                                                        if (ordertype != 3) {
                                                            if (status == 3)
                                                                html += "<tr class=\"package-row lighten-4 order-id small" + UID + " blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                            else
                                                                html += "<tr class=\"package-row lighten-4 order-id small" + UID + "\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                        }
                                                        else {
                                                            html += "<tbody class=\"orderid" + UID + " dh" + odID + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";
                                                            html += "<tr class=\"package-row lighten-4 order-id small" + UID + "\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";

                                                        }

                                                        if (ordertype == 1) {
                                                            //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                            html += "<td><span>" + ordertypeString + "</span></td>";
                                                        }
                                                        else if (ordertype == 2) {
                                                            //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                            html += "<td><span>" + ordertypeString + "</span></td>";
                                                        }
                                                        else {
                                                            html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                            html += "<input type=\"text\" value=\"\" class=\"tooltipped packageorderID\" data-tooltip=\"\">";
                                                            html += "</td>";

                                                            html += "<td>";
                                                            html += "<div class=\"input-field\">";
                                                            html += "<select class=\"package-status-select packageOrderType\">";
                                                            html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                            html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                            html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                            html += "</select>";
                                                            html += "</div>";
                                                            html += "</td>";
                                                        }

                                                        html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                        html += "<p class=\"black-text\">KD</p>";
                                                        if (kiemdem == "Có") {
                                                            html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                        }
                                                        else {
                                                            html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                        }
                                                        html += "</div>";

                                                        html += "<div class=\"tb-block\">";
                                                        html += "<p class=\"black-text\">ĐG</p>";
                                                        if (donggo == "Có") {
                                                            html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                        }
                                                        else {
                                                            html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                        }
                                                        html += "</div>";

                                                        html += "<div class=\"tb-block\" style=\"display: none;\">";
                                                        html += "<p class=\"black-text\">BH</p>";
                                                        if (baohiem == "Có") {
                                                            html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                        }
                                                        else {
                                                            html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                        }
                                                        html += "</div>";

                                                        html += "</div>";
                                                        html += "</td>";
                                                        html += "<td><span>" + getbarcode + "</span></td>";
                                                        html += "<td><span>" + weight + "</span></td>";
                                                        html += "<td class=\"size\">";
                                                        html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                        html += "</p>";
                                                        html += "</td>";
                                                        html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                        if (status == 1) {
                                                            html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                        }
                                                        else if (status == 2) {
                                                            html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                        }
                                                        else if (status == 3) {
                                                            html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                        }
                                                        else if (status == 4) {
                                                            html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                        }
                                                        else if (status == 5) {
                                                            html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                        }
                                                        else {
                                                            html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                        }

                                                        html += "<td>";
                                                        html += "<div class=\"action-table\"> ";
                                                        if (ordertype == 3)
                                                            html += "<a href=\"#!\" onclick=\"updateOrderType('" + getbarcode + "',$(this),'" + pID + "'," + UID + ", " + odID + ")\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                        html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + odID + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                        html += "</div>";
                                                        html += "</td>";
                                                        html += "</tr>";
                                                        if (ordertype == 3) {
                                                            html += "</tbody>";
                                                        }
                                                        $(".orderid" + UID + "").parent().append(html);
                                                        odID++;
                                                    }
                                                    else {
                                                        html += "<tbody class=\"orderid" + UID + " dh" + orderid + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";
                                                        if (ordertype != 3) {
                                                            html += "<tr><td rowspan=\"10\" class=\"grey lighten-2\">" + orderid + "</td></tr>";
                                                        }

                                                        //Xét trạng thái đã về kho VN
                                                        if (ordertype != 3) {
                                                            if (status == 3)
                                                                html += "<tr class=\"package-row lighten-4 order-id small" + UID + " blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                            else
                                                                html += "<tr class=\"package-row lighten-4 order-id small" + UID + "\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                        }
                                                        else {
                                                            html += "<tr class=\"package-row lighten-4 order-id small" + UID + "\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                        }

                                                        if (ordertype == 1) {
                                                            html += "<td><span>" + ordertypeString + "</span></td>";
                                                        }
                                                        else if (ordertype == 2) {
                                                            html += "<td><span>" + ordertypeString + "</span></td>";
                                                        }
                                                        else {
                                                            html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                            html += "<input type=\"text\" value=\"\" class=\"tooltipped packageorderID\" data-tooltip=\"\">";
                                                            html += "</td>";

                                                            html += "<td>";
                                                            html += "<div class=\"input-field\">";
                                                            html += "<select class=\"package-status-select packageOrderType\">";
                                                            html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                            html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                            html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                            html += "</select>";
                                                            html += "</div>";
                                                            html += "</td>";
                                                        }

                                                        html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                        html += "<p class=\"black-text\">KD</p>";
                                                        if (kiemdem == "Có") {
                                                            html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                        }
                                                        else {
                                                            html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                        }

                                                        html += "</div>";
                                                        html += "<div class=\"tb-block\">";
                                                        html += "<p class=\"black-text\">ĐG</p>";
                                                        if (donggo == "Có") {
                                                            html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                        }
                                                        else {
                                                            html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                        }
                                                        html += "</div>";

                                                        html += "<div class=\"tb-block\" style=\"display: none;\">";
                                                        html += "<p class=\"black-text\">BH</p>";
                                                        if (baohiem == "Có") {
                                                            html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                        }
                                                        else {
                                                            html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                        }
                                                        html += "</div>";

                                                        html += "</div>";
                                                        html += "</td>";
                                                        html += "<td><span>" + getbarcode + "</span></td>";
                                                        html += "<td><span>" + weight + "</span></td>";
                                                        html += "<td class=\"size\">";
                                                        html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                        html += "</p>";
                                                        html += "</td>";
                                                        html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                        if (status == 1) {
                                                            html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                        }
                                                        else if (status == 2) {
                                                            html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                        }
                                                        else if (status == 3) {
                                                            html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                        }
                                                        else if (status == 4) {
                                                            html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                        }
                                                        else if (status == 5) {
                                                            html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                        }
                                                        else {
                                                            html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                        }

                                                        html += "<td>";
                                                        html += "<div class=\"action-table\"> ";
                                                        if (ordertype == 3)
                                                            html += "<a href=\"#!\" onclick=\"updateOrderType('" + getbarcode + "',$(this),'" + pID + "'," + UID + ", " + orderid + ")\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                        html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                        html += "</div>";
                                                        html += "</td>";
                                                        html += "</tr>";
                                                        html += "</tbody>";

                                                        $(".orderid" + UID + "").parent().prepend(html);
                                                    }
                                                }
                                                else {
                                                    html += "<tbody class=\"orderid" + UID + " dh" + orderid + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";
                                                    if (ordertype != 3) {
                                                        html += "<tr><td rowspan=\"10\" class=\"grey lighten-2\">" + orderid + "</td></tr>";
                                                    }

                                                    //Xét trạng thái đã về kho VN
                                                    if (ordertype != 3) {
                                                        if (status == 3)
                                                            html += "<tr class=\"package-row lighten-4 order-id small" + UID + " blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                        else
                                                            html += "<tr class=\"package-row lighten-4 order-id small" + UID + "\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                    }
                                                    else {
                                                        html += "<tr class=\"package-row lighten-4 order-id small" + UID + "\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                    }

                                                    if (ordertype == 1) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else if (ordertype == 2) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else {
                                                        html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                        html += "<input type=\"text\" value=\"\" class=\"tooltipped packageorderID\" data-tooltip=\"\">";
                                                        html += "</td>";

                                                        html += "<td>";
                                                        html += "<div class=\"input-field\">";
                                                        html += "<select class=\"package-status-select packageOrderType\">";
                                                        html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                        html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                        html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                        html += "</select>";
                                                        html += "</div>";
                                                        html += "</td>";
                                                    }

                                                    html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">KD</p>";
                                                    if (kiemdem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }

                                                    html += "</div>";
                                                    html += "<div class=\"tb-block\">";
                                                    html += "<p class=\"black-text\">ĐG</p>";
                                                    if (donggo == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "<div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">BH</p>";
                                                    if (baohiem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "<td><span>" + getbarcode + "</span></td>";
                                                    html += "<td><span>" + weight + "</span></td>";
                                                    html += "<td class=\"size\">";
                                                    html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                    html += "</p>";
                                                    html += "</td>";
                                                    html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                    if (status == 1) {
                                                        html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                    }
                                                    else if (status == 2) {
                                                        html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                    }
                                                    else if (status == 3) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                    }
                                                    else if (status == 4) {
                                                        html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                    }
                                                    else if (status == 5) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                    }
                                                    else {
                                                        html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                    }
                                                    html += "<td>";
                                                    html += "<div class=\"action-table\"> ";
                                                    if (ordertype == 3)
                                                        html += "<a href=\"#!\" onclick=\"updateOrderType('" + getbarcode + "',$(this),'" + pID + "'," + UID + ", " + orderid + ")\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                    html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "</tr>";
                                                    html += "</tbody>";

                                                    $(".orderid" + UID + "").parent().prepend(html);
                                                }
                                            }
                                        }
                                        else {
                                            if ($(".package-row").length > 0) {
                                                $(".package-row").each(function () {
                                                    var dt_packageID = $(this).attr("data-packageID");
                                                    if (pID == dt_packageID) {
                                                        var status = $(this).attr("data-status");
                                                        if (status > 2)
                                                            $(this).addClass("blue");
                                                    }
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                            obj.val("");
                            countOutPackage();
                            $('select').formSelect();
                        }
                        else {
                            alert('Không tìm thấy kiện này');
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        //alert('lỗi checkend');
                    }
                });
            }
        }


        function getbycode(barco) {
            var bc = barco;
            var username = $("#username").val();
            var waretype = $(".Waretypeget option:selected").val();
            if (isEmpty(bc)) {
                alert('Vui lòng không để trống barcode');
            }
            else if (isEmpty(username)) {
                alert('Vui lòng không để trống username');
            }
            else {
                var listbarcode = $("#<%=hdfListBarcode.ClientID%>").val();
                $.ajax({
                    type: "POST",
                    url: "/manager/outstock.aspx/getpackages",
                    data: "{barcode:'" + bc + "',username:'" + username + "',Ware:'" + waretype + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var ret = msg.d;
                        if (ret != "none") {
                            var listp = JSON.parse(msg.d);
                            if (listp.length > 0) {
                                for (var i = 0; i < listp.length; i++) {
                                    var p = listp[i];
                                    var html = '';
                                    var pID = p.pID;
                                    var UID = p.uID;
                                    var uname = p.username;
                                    var mID = p.mID;
                                    var tID = p.tID;
                                    var weight = p.weight;
                                    var status = p.status;
                                    var getbarcode = p.barcode;
                                    var dIWH = p.dateInWarehouse;
                                    var kiemdem = p.kiemdem;
                                    var donggo = p.donggo;
                                    var baohiem = p.baohiem;
                                    var ordertype = parseFloat(p.OrderType);
                                    var ordertypeString = p.OrderTypeString;
                                    var totalDaysInWare = p.TotalDayInWarehouse

                                    var orderid = 0;
                                    if (mID > 0) {
                                        orderid = mID;
                                    }
                                    else if (tID > 0) {
                                        orderid = tID;
                                    }

                                    var isExist = false;
                                    if ($(".package-row").length > 0) {
                                        $(".package-row").each(function () {
                                            var dt_packageID = $(this).attr("data-packageID");
                                            if (pID == dt_packageID) {
                                                isExist = true;
                                            }
                                        });
                                    }

                                    var check = false;
                                    $(".package-item").each(function () {
                                        if ($(this).attr("data-uid") == UID) {
                                            check = true;
                                        }
                                    })

                                    if (isExist == false) {
                                        if (check == false) {
                                            html += "<div class=\"package-item pb-2\" data-uid=\"" + UID + "\">";
                                            html += "<div class=\"responsive-tb\">";
                                            html += "<table class=\"table table-inside centered  table-warehouse\">";
                                            html += "<thead>";
                                            html += "<tr class=\"teal darken-4\">";
                                            html += "<th style=\"min-width: 50px;\">Order ID</th>";
                                            html += "<th style=\"min-width: 50px;\">Loại ĐH</th>";
                                            html += "<th style=\"min-width: 110px;\">Đơn hàng</th>";
                                            html += "<th>Mã vận đơn</th>";
                                            html += "<th style=\"min-width: auto;\">Cân nặng<br />(kg)</th>";
                                            html += "<th class=\"size-th\">Kích thước</th>";
                                            html += "<th style=\"min-width: 100px\">Tổng ngày</br>lưu kho</th>";
                                            html += "<th style=\"min-width: 150px\">Trạng thái</th>";
                                            html += "<th style=\"min-width: 80px;\">Action</th></tr>";
                                            html += "</thead>";
                                            html += "<tbody class=\"orderid" + UID + " dh" + orderid + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";

                                            if (ordertype != 3) {
                                                html += "<tr><td rowspan=\"10\" class=\"grey lighten-2\">" + orderid + "</td></tr>";
                                            }

                                            html += "<tr class=\"package-row lighten-4 order-id blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                            if (ordertype == 1) {
                                                //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                html += "<td><span>" + ordertypeString + "</span></td>";
                                            }
                                            else if (ordertype == 2) {
                                                //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                html += "<td><span>" + ordertypeString + "</span></td>";
                                            }
                                            else {
                                                html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                html += "<input type=\"text\" value=\"\" class=\"tooltipped\" data-tooltip=\"\">";
                                                html += "</td>";

                                                html += "<td>";
                                                html += "<div class=\"input-field\">";
                                                html += "<select class=\"package-status-select packageOrderType\">";
                                                html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                html += "</select>";
                                                html += "</div>";
                                                html += "</td>";
                                            }

                                            html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                            html += "<p class=\"black-text\">KD</p>";
                                            if (kiemdem == "Có") {
                                                html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                            }
                                            else {
                                                html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                            }
                                            html += "</div>";

                                            html += "<div class=\"tb-block\">";
                                            html += "<p class=\"black-text\">ĐG</p>";
                                            if (donggo == "Có") {
                                                html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                            }
                                            else {
                                                html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                            }
                                            html += "</div>";

                                            html += "<div class=\"tb-block\" style=\"display: none;\">";
                                            html += "<p class=\"black-text\">BH</p>";
                                            if (baohiem == "Có") {
                                                html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                            }
                                            else {
                                                html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                            }
                                            html += "</div>";

                                            html += "</td>";
                                            html += "<td><span>" + getbarcode + "</span></td>";
                                            html += "<td><span>" + weight + "</span></td>";
                                            html += "<td class=\"size\">";
                                            html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                            html += "</p>";
                                            html += "</td>";
                                            html += "<td><span>" + totalDaysInWare + "</span></td>";
                                            if (status == 1) {
                                                html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                            }
                                            else if (status == 2) {
                                                html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                            }
                                            else if (status == 3) {
                                                html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                            }
                                            else if (status == 4) {
                                                html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                            }
                                            else if (status == 5) {
                                                html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                            }
                                            else {
                                                html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                            }

                                            html += "<td>";
                                            html += "<div class=\"action-table\"> ";
                                            if (ordertype == 3)
                                                html += "<a href=\"#!\" onclick=\"updateWeightNew($(this))\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                            html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                            html += "</div>";
                                            html += "</td>";
                                            html += "</tr>";
                                            html += "</tbody>";
                                            html += "</table>";
                                            html += "</div>";
                                            html += "</div>";

                                            $("#listpackage").append(html);

                                        }
                                        else {
                                            var MainID = $(".orderid" + UID + "").attr('data-orderid');
                                            if (MainID == orderid) {
                                                var otype = $(".orderid" + UID + "").attr('data-ordertype');
                                                if (otype == ordertype) {
                                                    html += "<tr class=\"package-row lighten-4 order-id blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                    if (ordertype == 1) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else if (ordertype == 2) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else {
                                                        html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                        html += "<input type=\"text\" value=\"\" class=\"tooltipped\" data-tooltip=\"\">";
                                                        html += "</td>";

                                                        html += "<td>";
                                                        html += "<div class=\"input-field\">";
                                                        html += "<select class=\"package-status-select packageOrderType\">";
                                                        html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                        html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                        html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                        html += "</select>";
                                                        html += "</div>";
                                                        html += "</td>";
                                                    }

                                                    html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">KD</p>";
                                                    if (kiemdem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "<div class=\"tb-block\">";
                                                    html += "<p class=\"black-text\">ĐG</p>";
                                                    if (donggo == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "<div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">BH</p>";
                                                    if (baohiem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "<td><span>" + getbarcode + "</span></td>";
                                                    html += "<td><span>" + weight + "</span></td>";
                                                    html += "<td class=\"size\">";
                                                    html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                    html += "</p>";
                                                    html += "</td>";
                                                    html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                    if (status == 1) {
                                                        html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                    }
                                                    else if (status == 2) {
                                                        html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                    }
                                                    else if (status == 3) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                    }
                                                    else if (status == 4) {
                                                        html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                    }
                                                    else if (status == 5) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                    }
                                                    else {
                                                        html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                    }

                                                    html += "<td>";
                                                    html += "<div class=\"action-table\"> ";
                                                    if (ordertype == 3)
                                                        html += "<a href=\"#!\" onclick=\"updateWeightNew($(this))\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                    html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "</tr>";
                                                    $(".orderid" + UID + "").parent().append(html);

                                                }
                                                else {
                                                    html += "<tbody class=\"orderid" + UID + " dh" + orderid + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";
                                                    if (ordertype != 3) {
                                                        html += "<tr><td rowspan=\"10\" class=\"grey lighten-2\">" + orderid + "</td></tr>";
                                                    }
                                                    html += "<tr class=\"package-row lighten-4 order-id blue\" data-packageID=\"" + pID + "\">";
                                                    if (ordertype == 1) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else if (ordertype == 2) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else {
                                                        html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                        html += "<input type=\"text\" value=\"" + data.NVKiemdem + "\" class=\"tooltipped\" data-tooltip=\"\">";
                                                        html += "</td>";

                                                        html += "<td>";
                                                        html += "<div class=\"input-field\">";
                                                        html += "<select class=\"package-status-select packageOrderType\">";
                                                        html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                        html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                        html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                        html += "</select>";
                                                        html += "</div>";
                                                        html += "</td>";
                                                    }

                                                    html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">KD</p>";
                                                    if (kiemdem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }

                                                    html += "</div>";
                                                    html += "<div class=\"tb-block\">";
                                                    html += "<p class=\"black-text\">ĐG</p>";
                                                    if (donggo == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "<div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">BH</p>";
                                                    if (baohiem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "<td><span>" + getbarcode + "</span></td>";
                                                    html += "<td><span>" + weight + "</span></td>";
                                                    html += "<td class=\"size\">";
                                                    html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                    html += "</p>";
                                                    html += "</td>";
                                                    html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                    if (status == 1) {
                                                        html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                    }
                                                    else if (status == 2) {
                                                        html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                    }
                                                    else if (status == 3) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                    }
                                                    else if (status == 4) {
                                                        html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                    }
                                                    else if (status == 5) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                    }
                                                    else {
                                                        html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                    }

                                                    html += "<td>";
                                                    html += "<div class=\"action-table\"> ";
                                                    if (ordertype == 3)
                                                        html += "<a href=\"#!\" onclick=\"updateWeightNew($(this))\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                    html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "</tr>";
                                                    html += "</tbody>";

                                                    $(".orderid" + UID + "").parent().prepend(html);
                                                }
                                            }
                                            else {
                                                html += "<tbody class=\"orderid" + UID + " dh" + orderid + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";
                                                if (ordertype != 3) {
                                                    html += "<tr><td rowspan=\"10\" class=\"grey lighten-2\">" + orderid + "</td></tr>";
                                                }
                                                html += "<tr class=\"package-row lighten-4 order-id blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                if (ordertype == 1) {
                                                    //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                    html += "<td><span>" + ordertypeString + "</span></td>";
                                                }
                                                else if (ordertype == 2) {
                                                    //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                    html += "<td><span>" + ordertypeString + "</span></td>";
                                                }
                                                else {
                                                    html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                    html += "<input type=\"text\" value=\"\" class=\"tooltipped\" data-tooltip=\"\">";
                                                    html += "</td>";

                                                    html += "<td>";
                                                    html += "<div class=\"input-field\">";
                                                    html += "<select class=\"package-status-select packageOrderType\">";
                                                    html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                    html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                    html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                    html += "</select>";
                                                    html += "</div>";
                                                    html += "</td>";
                                                }

                                                html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                html += "<p class=\"black-text\">KD</p>";
                                                if (kiemdem == "Có") {
                                                    html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                }
                                                else {
                                                    html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                }

                                                html += "</div>";
                                                html += "<div class=\"tb-block\">";
                                                html += "<p class=\"black-text\">ĐG</p>";
                                                if (donggo == "Có") {
                                                    html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                }
                                                else {
                                                    html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                }
                                                html += "</div>";

                                                html += "<div class=\"tb-block\" style=\"display: none;\">";
                                                html += "<p class=\"black-text\">BH</p>";
                                                if (baohiem == "Có") {
                                                    html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                }
                                                else {
                                                    html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                }
                                                html += "</div>";

                                                html += "</div>";
                                                html += "</td>";
                                                html += "<td><span>" + getbarcode + "</span></td>";
                                                html += "<td><span>" + weight + "</span></td>";
                                                html += "<td class=\"size\">";
                                                html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                html += "</p>";
                                                html += "</td>";
                                                html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                if (status == 1) {
                                                    html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                }
                                                else if (status == 2) {
                                                    html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                }
                                                else if (status == 3) {
                                                    html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                }
                                                else if (status == 4) {
                                                    html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                }
                                                else if (status == 5) {
                                                    html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                }
                                                else {
                                                    html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                }

                                                html += "<td>";
                                                html += "<div class=\"action-table\"> ";
                                                if (ordertype == 3)
                                                    html += "<a href=\"#!\" onclick=\"updateWeightNew($(this))\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                html += "</div>";
                                                html += "</td>";
                                                html += "</tr>";
                                                html += "</tbody>";

                                                $(".orderid" + UID + "").parent().prepend(html);
                                            }
                                        }
                                    }
                                    else {

                                    }

                                }
                            }
                            totalPrice();
                            obj.val("");
                        }
                        else {
                            alert('Không tìm thấy kiện');
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        //alert('lỗi checkend');
                    }
                });
            }
        }


        function getpackagebyoID() {
            var orderid = 0;
            if (!isEmpty($("#txtOrderID").val()))
                orderid = $("#txtOrderID").val();
            //var orderid = $("#txtOrderID").val();
            var ordertype = $(".ordertypeget option:selected").val();
            var waretype = $(".Waretypeget option:selected").val();
            var username = $("#username").val();
            if (isEmpty(username)) {
                alert('Vui lòng nhập username.');
            }
            else {
                var listbarcode = $("#<%=hdfListBarcode.ClientID%>").val();
                $.ajax({
                    type: "POST",
                    url: "/manager/outstock.aspx/getpackagesbyo",
                    data: "{orderID:'" + orderid + "',username:'" + username + "',type:'" + ordertype + "',Ware:'" + waretype + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var ret = msg.d;
                        if (ret != "none") {
                            var listp = JSON.parse(msg.d);
                            if (listp.length > 0) {
                                for (var i = 0; i < listp.length; i++) {
                                    var p = listp[i];
                                    var html = '';
                                    var pID = p.pID;
                                    var UID = p.uID;
                                    var uname = p.username;
                                    var mID = p.mID;
                                    var tID = p.tID;
                                    var weight = p.weight;
                                    var status = p.status;
                                    var getbarcode = p.barcode;
                                    var dIWH = p.dateInWarehouse;
                                    var kiemdem = p.kiemdem;
                                    var donggo = p.donggo;
                                    var baohiem = p.baohiem;
                                    var ordertype = parseFloat(p.OrderType);
                                    var ordertypeString = p.OrderTypeString;
                                    var totalDaysInWare = p.TotalDayInWarehouse

                                    var orderid = 0;
                                    if (mID > 0) {
                                        orderid = mID;
                                    }
                                    else if (tID > 0) {
                                        orderid = tID;
                                    }

                                    var isExist = false;
                                    if ($(".package-row").length > 0) {
                                        $(".package-row").each(function () {
                                            var dt_packageID = $(this).attr("data-packageID");
                                            if (pID == dt_packageID) {
                                                isExist = true;
                                            }
                                        });
                                    }

                                    var check = false;
                                    $(".package-item").each(function () {
                                        if ($(this).attr("data-uid") == UID) {
                                            check = true;
                                        }
                                    })

                                    if (isExist == false) {
                                        if (check == false) {
                                            html += "<div class=\"package-item pb-2\" data-uid=\"" + UID + "\">";
                                            html += "<div class=\"responsive-tb\">";
                                            html += "<table class=\"table table-inside centered  table-warehouse\">";
                                            html += "<thead>";
                                            html += "<tr class=\"teal darken-4\">";
                                            html += "<th style=\"min-width: 50px;\">Order ID</th>";
                                            html += "<th style=\"min-width: 50px;\">Loại ĐH</th>";
                                            html += "<th style=\"min-width: 110px;\">Đơn hàng</th>";
                                            html += "<th>Mã vận đơn</th>";
                                            html += "<th style=\"min-width: auto;\">Cân nặng<br />(kg)</th>";
                                            html += "<th class=\"size-th\">Kích thước</th>";
                                            html += "<th style=\"min-width: 100px\">Tổng ngày</br>lưu kho</th>";
                                            html += "<th style=\"min-width: 150px\">Trạng thái</th>";
                                            html += "<th style=\"min-width: 80px;\">Action</th></tr>";
                                            html += "</thead>";
                                            html += "<tbody class=\"orderid" + UID + " dh" + orderid + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";

                                            if (ordertype != 3) {
                                                html += "<tr><td rowspan=\"100\" class=\"grey lighten-2\">" + orderid + "</td></tr>";
                                            }

                                            html += "<tr class=\"package-row lighten-4 order-id blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                            if (ordertype == 1) {
                                                //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                html += "<td><span>" + ordertypeString + "</span></td>";
                                            }
                                            else if (ordertype == 2) {
                                                //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                html += "<td><span>" + ordertypeString + "</span></td>";
                                            }
                                            else {
                                                html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                html += "<input type=\"text\" value=\"\" class=\"tooltipped\" data-tooltip=\"\">";
                                                html += "</td>";

                                                html += "<td>";
                                                html += "<div class=\"input-field\">";
                                                html += "<select class=\"package-status-select packageOrderType\">";
                                                html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                html += "</select>";
                                                html += "</div>";
                                                html += "</td>";
                                            }

                                            html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                            html += "<p class=\"black-text\">KD</p>";
                                            if (kiemdem == "Có") {
                                                html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                            }
                                            else {
                                                html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                            }
                                            html += "</div>";

                                            html += "<div class=\"tb-block\">";
                                            html += "<p class=\"black-text\">ĐG</p>";
                                            if (donggo == "Có") {
                                                html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                            }
                                            else {
                                                html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                            }
                                            html += "</div>";

                                            html += "<div class=\"tb-block\" style=\"display: none;\">";
                                            html += "<p class=\"black-text\">BH</p>";
                                            if (baohiem == "Có") {
                                                html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                            }
                                            else {
                                                html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                            }
                                            html += "</div>";

                                            html += "</td>";
                                            html += "<td><span>" + getbarcode + "</span></td>";
                                            html += "<td><span>" + weight + "</span></td>";
                                            html += "<td class=\"size\">";
                                            html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                            html += "</p>";
                                            html += "</td>";
                                            html += "<td><span>" + totalDaysInWare + "</span></td>";
                                            if (status == 1) {
                                                html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                            }
                                            else if (status == 2) {
                                                html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                            }
                                            else if (status == 3) {
                                                html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                            }
                                            else if (status == 4) {
                                                html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                            }
                                            else if (status == 5) {
                                                html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                            }
                                            else {
                                                html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                            }

                                            html += "<td>";
                                            html += "<div class=\"action-table\"> ";
                                            if (ordertype == 3)
                                                html += "<a href=\"#!\" onclick=\"updateWeightNew($(this))\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                            html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                            html += "</div>";
                                            html += "</td>";
                                            html += "</tr>";
                                            html += "</tbody>";
                                            html += "</table>";
                                            html += "</div>";
                                            html += "</div>";

                                            $("#listpackage").append(html);

                                        }
                                        else {
                                            var MainID = $(".orderid" + UID + "").attr('data-orderid');
                                            if (MainID == orderid) {
                                                var otype = $(".orderid" + UID + "").attr('data-ordertype');
                                                if (otype == ordertype) {
                                                    html += "<tr class=\"package-row lighten-4 order-id blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                    if (ordertype == 1) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else if (ordertype == 2) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else {
                                                        html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                        html += "<input type=\"text\" value=\"\" class=\"tooltipped\" data-tooltip=\"\">";
                                                        html += "</td>";

                                                        html += "<td>";
                                                        html += "<div class=\"input-field\">";
                                                        html += "<select class=\"package-status-select packageOrderType\">";
                                                        html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                        html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                        html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                        html += "</select>";
                                                        html += "</div>";
                                                        html += "</td>";
                                                    }

                                                    html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">KD</p>";
                                                    if (kiemdem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "<div class=\"tb-block\">";
                                                    html += "<p class=\"black-text\">ĐG</p>";
                                                    if (donggo == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "<div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">BH</p>";
                                                    if (baohiem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "<td><span>" + getbarcode + "</span></td>";
                                                    html += "<td><span>" + weight + "</span></td>";
                                                    html += "<td class=\"size\">";
                                                    html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                    html += "</p>";
                                                    html += "</td>";
                                                    html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                    if (status == 1) {
                                                        html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                    }
                                                    else if (status == 2) {
                                                        html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                    }
                                                    else if (status == 3) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                    }
                                                    else if (status == 4) {
                                                        html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                    }
                                                    else if (status == 5) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                    }
                                                    else {
                                                        html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                    }

                                                    html += "<td>";
                                                    html += "<div class=\"action-table\"> ";
                                                    if (ordertype == 3)
                                                        html += "<a href=\"#!\" onclick=\"updateWeightNew($(this))\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                    html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "</tr>";
                                                    $(".orderid" + UID + "").parent().append(html);

                                                }
                                                else {
                                                    html += "<tbody class=\"orderid" + UID + " dh" + orderid + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";
                                                    if (ordertype != 3) {
                                                        html += "<tr><td rowspan=\"100\" class=\"grey lighten-2\">" + orderid + "</td></tr>";
                                                    }
                                                    html += "<tr class=\"package-row lighten-4 order-id blue\" data-packageID=\"" + pID + "\">";
                                                    if (ordertype == 1) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else if (ordertype == 2) {
                                                        //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                        html += "<td><span>" + ordertypeString + "</span></td>";
                                                    }
                                                    else {
                                                        html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                        html += "<input type=\"text\" value=\"" + data.NVKiemdem + "\" class=\"tooltipped\" data-tooltip=\"\">";
                                                        html += "</td>";

                                                        html += "<td>";
                                                        html += "<div class=\"input-field\">";
                                                        html += "<select class=\"package-status-select packageOrderType\">";
                                                        html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                        html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                        html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                        html += "</select>";
                                                        html += "</div>";
                                                        html += "</td>";
                                                    }

                                                    html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">KD</p>";
                                                    if (kiemdem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }

                                                    html += "</div>";
                                                    html += "<div class=\"tb-block\">";
                                                    html += "<p class=\"black-text\">ĐG</p>";
                                                    if (donggo == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "<div class=\"tb-block\" style=\"display: none;\">";
                                                    html += "<p class=\"black-text\">BH</p>";
                                                    if (baohiem == "Có") {
                                                        html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                    }
                                                    else {
                                                        html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                    }
                                                    html += "</div>";

                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "<td><span>" + getbarcode + "</span></td>";
                                                    html += "<td><span>" + weight + "</span></td>";
                                                    html += "<td class=\"size\">";
                                                    html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                    html += "</p>";
                                                    html += "</td>";
                                                    html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                    if (status == 1) {
                                                        html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                    }
                                                    else if (status == 2) {
                                                        html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                    }
                                                    else if (status == 3) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                    }
                                                    else if (status == 4) {
                                                        html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                    }
                                                    else if (status == 5) {
                                                        html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                    }
                                                    else {
                                                        html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                    }

                                                    html += "<td>";
                                                    html += "<div class=\"action-table\"> ";
                                                    if (ordertype == 3)
                                                        html += "<a href=\"#!\" onclick=\"updateWeightNew($(this))\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                    html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                    html += "</div>";
                                                    html += "</td>";
                                                    html += "</tr>";
                                                    html += "</tbody>";

                                                    $(".orderid" + UID + "").parent().prepend(html);
                                                }
                                            }
                                            else {
                                                html += "<tbody class=\"orderid" + UID + " dh" + orderid + "\" data-orderid=\"" + orderid + "\"  data-ordertype=\"" + ordertype + "\">";
                                                if (ordertype != 3) {
                                                    html += "<tr><td rowspan=\"100\" class=\"grey lighten-2\">" + orderid + "</td></tr>";
                                                }
                                                html += "<tr class=\"package-row lighten-4 order-id blue\" data-status=\"" + status + "\" data-packageID=\"" + pID + "\">";
                                                if (ordertype == 1) {
                                                    //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + mID + "</span></td>";
                                                    html += "<td><span>" + ordertypeString + "</span></td>";
                                                }
                                                else if (ordertype == 2) {
                                                    //html += "<td rowspan=\"10\" class=\"grey lighten-2\"><span>" + tID + "</span></td>";
                                                    html += "<td><span>" + ordertypeString + "</span></td>";
                                                }
                                                else {
                                                    html += "<td rowspan=\"10\" class=\"grey lighten-2\">";
                                                    html += "<input type=\"text\" value=\"\" class=\"tooltipped\" data-tooltip=\"\">";
                                                    html += "</td>";

                                                    html += "<td>";
                                                    html += "<div class=\"input-field\">";
                                                    html += "<select class=\"package-status-select packageOrderType\">";
                                                    html += "<option value=\"\" disabled>Loại Đơn Hàng</option>";
                                                    html += "                   <option value=\"1\">Đơn hàng mua hộ</option>";
                                                    html += "                   <option value=\"2\">Đơn hàng VC hộ</option>";
                                                    html += "</select>";
                                                    html += "</div>";
                                                    html += "</td>";
                                                }

                                                html += "<td><div class=\"tb-block\" style=\"display: none;\">";
                                                html += "<p class=\"black-text\">KD</p>";
                                                if (kiemdem == "Có") {
                                                    html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                }
                                                else {
                                                    html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                }

                                                html += "</div>";
                                                html += "<div class=\"tb-block\">";
                                                html += "<p class=\"black-text\">ĐG</p>";
                                                if (donggo == "Có") {
                                                    html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                }
                                                else {
                                                    html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                }
                                                html += "</div>";

                                                html += "<div class=\"tb-block\" style=\"display: none;\">";
                                                html += "<p class=\"black-text\">BH</p>";
                                                if (baohiem == "Có") {
                                                    html += "<p><i class=\"material-icons green-text\">check_circle</i></p>";
                                                }
                                                else {
                                                    html += "<p><i class=\"material-icons grey-text\">check_circle</i></p>";
                                                }
                                                html += "</div>";

                                                html += "</div>";
                                                html += "</td>";
                                                html += "<td><span>" + getbarcode + "</span></td>";
                                                html += "<td><span>" + weight + "</span></td>";
                                                html += "<td class=\"size\">";
                                                html += "<p><span>d: " + p.dai + "</span> <b>x</b> <span>r: " + p.rong + "</span><b>x</b> <span>c: " + p.cao + "</span>";
                                                html += "</p>";
                                                html += "</td>";
                                                html += "<td><span>" + totalDaysInWare + "</span></td>";
                                                if (status == 1) {
                                                    html += "<td><span class=\"white-text badge red darken-2\">Chưa về kho TQ</span></td>";
                                                }
                                                else if (status == 2) {
                                                    html += "<td><span class=\"white-text badge orange darken-2\">Đã về kho TQ</span></td>";
                                                }
                                                else if (status == 3) {
                                                    html += "<td><span class=\"white-text badge green darken-2\">Đã về kho VN</span></td>";
                                                }
                                                else if (status == 4) {
                                                    html += "<td><span class=\"white-text badge dungmona darken-2\">Đang giao cho khách</span></td>";
                                                }
                                                else if (status == 5) {
                                                    html += "<td><span class=\"white-text badge green darken-2\">Đang về kho VN</span></td>";
                                                }
                                                else {
                                                    html += "<td><span class=\"white-text badge blue darken-2\">Đã thanh toán</span></td>";
                                                }

                                                html += "<td>";
                                                html += "<div class=\"action-table\"> ";
                                                if (ordertype == 3)
                                                    html += "<a href=\"#!\" onclick=\"updateWeightNew($(this))\" class=\"tooltipped updatebutton\" data-position=\"top\" data-tooltip=\"Cập nhật thay đổi\"><i class=\"material-icons\">sync</i></a>";
                                                html += "<a href=\"#!\" onclick=\"huyxuatkho($(this)," + UID + ", " + orderid + ")\" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Ẩn đi\"><i class=\"material-icons\">visibility_off</i></a>";
                                                html += "</div>";
                                                html += "</td>";
                                                html += "</tr>";
                                                html += "</tbody>";

                                                $(".orderid" + UID + "").parent().prepend(html);
                                            }
                                        }
                                    }
                                    else {

                                    }

                                }
                            }
                            countOutPackage();
                            $('select').formSelect();
                            obj.val("");
                        }
                        else {
                            alert('Không tìm thấy kiện');
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        //alert('lỗi checkend');
                    }
                });
            }
        }

        function huyxuatkhoNew(barcode, obj) {
            var r = confirm("Bạn muốn tắt kiện này?");
            if (r == true) {
                var id = barcode + "|";
                var listbarcode = $("#<%=hdfListBarcode.ClientID%>").val();
                listbarcode = listbarcode.replace(id, "");
                $("#<%=hdfListBarcode.ClientID%>").val(listbarcode);
                obj.parent().parent().parent().remove();
                if ($(".package-item").length == 0) {
                    $("#outall-package").hide();
                    $("#xuatkhotatca").hide();
                }
                countOrder();

                totalPrice();
            } else {

            }
        }

        function updateOrderType(bc, obj, packageID, uid, mainorderid) {
            var root = obj.parent().parent().parent();
            var mordertype = root.find(".packageOrderType option:selected").val();
            var morderID = root.find(".packageorderID").val();
            var musername = $("#username").val();
            $.ajax({
                type: "POST",
                url: "/manager/OutStock.aspx/addpackagetoprder",
                data: "{ordertype:'" + mordertype + "',username:'" + musername + "',orderid:'" + morderID + "',pID:'" + packageID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var ret = JSON.parse(msg.d);
                    if (ret != "none") {
                        var p = ret;
                        var pID = p.pID;
                        var code = p.barcode;

                        obj.parent().parent().parent().remove();

                        if ($(".dh" + mainorderid + " tr").length == 1) {
                            $(".dh" + mainorderid + "").remove();
                        }

                        if ($(".small" + uid + "").length == 0) {
                            $("#" + uid + "").remove();
                        }

                        getbycode(code);
                    }
                    else {
                        alert('Có lỗi trong quá trình cập nhật, vui lòng thử lại sau');
                    }
                    obj.val("");
                }, error: function (xmlhttprequest, textstatus, errorthrow) {
                    //alert('lỗi checkend');
                }
            });
        }
        function xuatkhotatcakien() {
            var checkout = true;
            var username = $("#username").val();
            $(".package-row").each(function () {
                if (!$(this).hasClass("blue")) {
                    checkout = false;
                }
            });
            if (checkout == false) {
                alert('Chưa có kiện nào để xuất. Bạn phải scan kiện hàng trong danh sách bên dưới để xuất!');
            }
            else {
                var listpackid = "";
                $(".package-row").each(function () {
                    listpackid += $(this).attr("data-packageid") + "|";
                });
                $("#<%=hdfListPID.ClientID%>").val(listpackid);
                $("#<%=hdfUsername.ClientID%>").val(username);
                $("#<%=btnAllOutstock.ClientID%>").click();
            }
        }

        function countOutPackage() {
            if ($(".blue").length > 0) {
                $("#xuatkhotatca").show();
            }
            else
                $("#xuatkhotatca").hide();

            totalPrice();
        }


        function totalPrice() {
            var listpackid = "";
            var username = $("#username").val();
            $(".package-row").each(function () {
                listpackid += $(this).attr("data-packageid") + "|";
            });
            $.ajax({
                type: "POST",
                url: "/manager/OutStock.aspx/TotalPrice",
                data: "{ListpackId:'" + listpackid + "',username:'" + username + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var ret = JSON.parse(msg.d);
                    debugger
                    if (ret != "none") {
                        $("#ltrMoneys").text(ret.totalMoney);
                        $("#ltrMoneyUser").text(ret.MoneyUser);
                        $("#ltrMoneyAppro").text(ret.Moneyset);
                        $("#ltrPackage").text(ret.totalPackage);
                        $("#ltrPackageScan").text(ret.totalPackageScan);
                    }
                    else {
                        $("#ltrMoneys").text(0);
                        $("#ltrMoneyUser").text(0);
                        $("#ltrMoneyAppro").text(0);
                        $("#ltrPackage").text(0);
                        $("#ltrPackageScan").text(0);
                    }
                    obj.val("");
                }, error: function (xmlhttprequest, textstatus, errorthrow) {
                    //alert('lỗi checkend');
                }
            });

        }
        function VoucherSourcetoPrint(source) {
            var r = "<html><head><link rel=\"stylesheet\" href=\"/App_Themes/AdminNew/css/style.css\" type=\"text/css\"/><link rel=\"stylesheet\" href=\"/App_Themes/AdminNew/css/style-p.css\" type=\"text/css\"/><script>function step1(){\n" +
                "setTimeout('step2()', 10);}\n" +
                "function step2(){window.print();window.close()}\n" +
                "</scri" + "pt></head><body onload='step1()'>\n" +
                "" + source + "</body></html>";
            return r;
        }
        function VoucherPrint(source) {
            Pagelink = "about:blank";
            var pwa = window.open(Pagelink, "_new");
            pwa.document.open();
            pwa.document.write(VoucherSourcetoPrint(source));
            pwa.document.close();
        }
        function huyxuatkho(obj, uid, mainorderid) {
            var r = confirm("Bạn muốn tắt package này?");
            if (r == true) {

                obj.parent().parent().parent().remove();

                if ($(".dh" + mainorderid + " tr").length == 1) {
                    $(".dh" + mainorderid + "").remove();
                }

                if ($(".small" + uid + "").length == 0) {
                    $("#" + uid + "").remove();
                }

                countOutPackage();
                totalPrice();
            } else {

            }
        }
    </script>
    <style>
        .dungmona.darken-2 {
            background-color: #5ec728;
        }
    </style>
</asp:Content>

