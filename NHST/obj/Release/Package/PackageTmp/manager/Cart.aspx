<%@ Page Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="NHST.manager.Cart" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div id="main">
        <div class="row">
            <div class="content-wrapper-before blue-grey lighten-5"></div>
            <div class="col s12">
                <div class="container">
                    <div class="all">
                        <div class="card-panel mt-3 no-shadow">
                            <div class="row">
                                <div class="col s12">
                                    <div class="page-title mt-2 center-align">
                                        <h4>GIỎ HÀNG</h4>
                                    </div>
                                </div>
                                <div class="col s12 order-info">
                                    <div class="checkout col s12 m6">
                                        <p class="black-text">
                                            <span class="lbl">Username: </span><span
                                                class="black-text font-weight-700">
                                                <asp:Literal runat="server" ID="ltrUsername"></asp:Literal></span>
                                        </p>
                                    </div>
                                </div>
                                <div class="col s12">
                                    <asp:Literal runat="server" ID="ltrNotProduct"></asp:Literal>
                                    <div class="total-info mb-2">
                                        <asp:Literal runat="server" ID="ltr_sub"></asp:Literal>
                                    </div>
                                    <div class="list-order">
                                        <div class="order-item-wrap">
                                            <asp:Literal ID="ltr_cart" runat="server"></asp:Literal>
                                        </div>
                                        <div class="order-quantity">
                                            <span>Tổng Mua =
                                                <asp:Literal runat="server" ID="ltr_totalproduct"></asp:Literal>
                                                sản phẩm</span>
                                        </div>
                                        <div class="total-checkout">
                                            <asp:Literal runat="server" ID="ltr_total_pay"></asp:Literal>
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

    <asp:Button ID="checkoutallorder" runat="server" Text="chaeckdall" OnClick="checkoutallorder_Click" Style="display: none" CausesValidation="false" />
    <asp:Button ID="checkout1order" runat="server" Text="chaeckdall" OnClick="checkout1order_Click" Style="display: none" CausesValidation="false" />
    <asp:HiddenField ID="hdforderlistall" runat="server" />
    <asp:HiddenField ID="hdfallnote" runat="server" />
    <asp:HiddenField ID="hdfallorderid" runat="server" />
    <asp:HiddenField ID="hdfProductBrand" runat="server" />
    <asp:HiddenField ID="hdfListOrderTempID" runat="server" />
    <script type="text/javascript">

        $(document).ready(function () {
            $('.list-order').on('change', '.select-checkout input[type="checkbox"]', function () {
                var $this = $(this);
                if ($this.prop('checked')) {
                    $this.parents('.order-item').css('border', '1.5px solid #000');
                    $this.next().find('.checked').css('display', 'inline-block');
                }
                else {
                    $this.parents('.order-item').css({
                        'border': 'none'
                    });
                    $this.next().find('.checked').css('display', 'none');
                }

            });
        });

        function updatequantity1(ID, obj, shopid) {
            var quantity = obj.val();
            if (quantity == 0) {
                deleteordertemp(ID, shopid);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/manager/Cart.aspx/UpdateQuantityOrderTemp",
                    data: "{ID:'" + ID + "',quantity:'" + quantity + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "1") {
                            location.reload();
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        // alert('lỗi');
                    }
                });
            }

        }
        function updatequantity(ID, obj, shopid) {
            var quantity = obj.parent().parent().find(".quantitiofpro").val();
            var brand = obj.parent().parent().parent().find(".notebrand").val();
            if (quantity == 0) {
                deleteordertemp(ID, shopid);
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/manager/Cart.aspx/UpdateQuantityOrderTemp",
                    data: "{ID:'" + ID + "',quantity:'" + quantity + "',brand:'" + brand + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "1") {
                            location.reload();
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        // alert('lỗi');
                    }
                });
            }

        }
        function deleteordertemp(ID, shopid) {
            var r = confirm("Bạn muốn xóa sản phẩm này?");
            if (r == true) {
                $.ajax({
                    type: "POST",
                    url: "/manager/Cart.aspx/deleteOrderTemp",
                    data: "{ID:'" + ID + "',shopID:'" + shopid + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "1") {
                            location.reload();
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        //alert('lỗi');
                    }
                });
            }
            else {
            }
        }
        function deleteordershoptemp(ID) {
            var r = confirm("Bạn muốn xóa cửa hàng này và tất cả sản phẩm hiện có ?");
            if (r == true) {
                $.ajax({
                    type: "POST",
                    url: "/manager/Cart.aspx/deleteOrderShopTemp",
                    data: "{ID:'" + ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "1") {
                            location.reload();
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        //alert('lỗi');
                    }
                });
            }
            else {
            }
        }

        function updatecheck(obj, ID, fieldUpdate) {
            $("body").append("<div class=\"loading\"></div>");
            var chk = obj.is(':checked');
            $.ajax({
                type: "POST",
                url: "/manager/Cart.aspx/UpdateField",
                data: "{ID:'" + ID + "',chk:'" + chk + "',field:'" + fieldUpdate + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var ret = msg.d;
                    if (ret == 1) {

                    }
                    else if (ret == "wronguser") {
                        alert("Không đúng tài khoản.");
                    }
                    else if (ret == "null") {
                        alert("Không tồn tại.");
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    //alert(errorthrow);
                }
            });
            $(".loading").remove();
        }

        function checkoutAll(listorderid) {
            $("#<%=hdfListOrderTempID.ClientID%>").val(listorderid);
            var striplist = listorderid.split('|');
            var html = "";
            var brandpro = "";
            if (striplist.length > 0) {
                for (var i = 0; i < striplist.length - 1; i++) {

                    var ID = striplist[i];
                    var objo = $("#" + ID + "_checkproductselect");
                    //updatecheck(objo, ID, "IsCheckProduct");
                    //var chk_isfast = $("#chk_fast_" + ID + "").is(':checked');

                    var note = $("#order_temp_" + ID + "").val();
                    var priceVND = $("#priceVND_" + ID + "").attr("data-price");

                    if (note == null || note == "") {
                        note = "";
                    }
                    if (isEmpty(priceVND)) {
                        alert('lỗi priceVND');
                        return;
                    }
                    html += ID + "," + note + "," + priceVND + "|";
                }
                $(".brand-name-product").each(function () {
                    var idpro = $(this).find(".notebrand").attr("data-item-id");
                    var brandtext = $(this).find(".notebrand").val();
                    brandpro += idpro + "," + brandtext + "|";
                });
                $("#<%= hdfProductBrand.ClientID %>").val(brandpro);
                $("#<%= hdforderlistall.ClientID %>").val(html);
                $("#<%= checkoutallorder.ClientID %>").click();
                //window.location.href = "/thanh-toan/all";
            }
        }
        function checkoutAllSelect() {
            var listorderid = $("#<%=hdfListOrderTempID.ClientID%>").val();
            var striplist = listorderid.split('|');
            var html = "";
            var brandpro = "";
            if (striplist.length - 1 > 0) {
                for (var i = 0; i < striplist.length - 1; i++) {

                    var ID = striplist[i];
                    var objo = $("#" + ID + "_checkproductselect");
                    //updatecheck(objo, ID, "IsCheckProduct");
                    //var chk_isfast = $("#chk_fast_" + ID + "").is(':checked');

                    var note = $("#order_temp_" + ID + "").val();
                    var priceVND = $("#priceVND_" + ID + "").attr("data-price");

                    if (note == null || note == "") {
                        note = "";
                    }
                    if (isEmpty(priceVND)) {
                        alert('lỗi priceVND');
                        return;
                    }
                    html += ID + "," + note + "," + priceVND + "|";
                }

                $(".brand-name-product").each(function () {
                    var idpro = $(this).find(".notebrand").attr("data-item-id");
                    var brandtext = $(this).find(".notebrand").val();
                    brandpro += idpro + "," + brandtext + "|";
                });
                $("#<%= hdfProductBrand.ClientID %>").val(brandpro);
                $("#<%= hdforderlistall.ClientID %>").val(html);
                $("#<%= checkoutallorder.ClientID %>").click();
            }
        }

        function isEmpty(str) {
            return !str.replace(/^\s+/g, '').length; // boolean (`true` if field is empty)
        }

        function getCartToSuccess(obj) {

            var countOrderCheck = 0;
            var list = "";
            if (obj.is(":checked")) {
                obj.parent().parent().addClass("select");
            }
            else {
                obj.parent().parent().removeClass("select");
            }
            $(".shop-cart").each(function () {
                if ($(this).hasClass("select")) {
                    countOrderCheck += 1;
                    list += $(this).attr("data-item-id") + "|";
                }
            });
            $("#<%=hdfListOrderTempID.ClientID%>").val(list);
            if (countOrderCheck > 0) {
                $(".getallOrder").attr("style", "display:inline-block;").find(".numofOrder").html(countOrderCheck);
            }
            else {
                $(".getallOrder").removeAttr("style").hide().find(".numofOrder").html(countOrderCheck);
            }
        }

        $(document).ready(function () {
            var countOrderCheck = 0;
            var list = "";
            $(".shop-cart").each(function () {
                if ($(this).hasClass("select")) {
                    countOrderCheck += 1;
                    list += $(this).attr("data-item-id") + "|";
                }
            });
            $("#<%=hdfListOrderTempID.ClientID%>").val(list);
            if (countOrderCheck > 0) {
                $(".getallOrder").attr("style", "display:inline-block;").find(".numofOrder").html(countOrderCheck);
            }
            else {
                $(".getallOrder").removeAttr("style").hide().find(".numofOrder").html(countOrderCheck);
            }
        });

        function checkout(ID) {
            var infor = "";
            var brandpro = "";
            var chk_isfast = $("#chk_fast_" + ID + "").is(':checked');
            var note = $("#order_temp_" + ID + "").val();
            if (note == null || note == "") {
                note = "";
            }

            var priceVND = $("#priceVND_" + ID + "").attr("data-price");
            var objo = $("#" + ID + "_checkproductselect");
            infor += ID + "," + note + "," + priceVND + "|";

            $(".brand-name-product").each(function () {
                var parent = $(this).attr("data-parent");
                if (parent == ID) {
                    var idpro = $(this).find(".notebrand").attr("data-item-id");
                    var brandtext = $(this).find(".notebrand").val();
                    brandpro += idpro + "," + brandtext + "|";
                }
            });

            $("#<%= hdforderlistall.ClientID %>").val(infor);
            $("#<%= hdfProductBrand.ClientID %>").val(brandpro);
            $("#<%= checkout1order.ClientID %>").click();

        }
    </script>
    <style>
        .all {
            width: 1200px;
            max-width: 100%;
            margin: 0 auto;
            position: relative;
            overflow: hidden;
        }

        .order-quantity {
            text-align: center;
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 20px;
            color: #000;
        }

        .quantity {
            font-weight: bold;
            font-size: 16px;
        }

        .checkout {
            font-size: 16px !important;
        }
    </style>
</asp:Content>
