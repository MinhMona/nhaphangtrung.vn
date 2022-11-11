<%@ Page Language="C#" MasterPageFile="~/UserNhapHangTrungMaster.Master" AutoEventWireup="true" CodeBehind="nap-tien.aspx.cs" Inherits="NHST.nap_tien" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="main">
        <div class="content">
            <div class="row">
                <div class="content-wrapper-before blue-grey lighten-5"></div>
                <div class="col s12">
                    <div class="container">
                        <div class="all">
                            <div class="card-panel no-shadow">
                                <div class="row">
                                    <div class="col s12">
                                        <div class="page-title center-align">
                                            <h4>NẠP TIỀN VNĐ</h4>
                                        </div>
                                    </div>
                                    <div class="col s12">
                                        <div class="order-list-info">
                                            <div class="total-info mb-2">
                                                <div class="row section">
                                                    <div class="col s12">
                                                        <p class="center-align">
                                                            Tổng tiền đã nạp: <span
                                                                class="teal-text text-darken-4 font-weight-700">
                                                                <asp:Literal runat="server" ID="lblTotalIncome"></asp:Literal>
                                                                VNĐ</span> <span class="black-text divi">|</span> Số dư hiện tại: <span class="teal-text text-darken-4 font-weight-700"> <asp:Literal runat="server" ID="lblAccount"></asp:Literal></span> VNĐ
                                                        </p>

                                                        <div class="donate-guide">
                                                            <h6 class="font-weight-700 mb-2 mt-2">QUY ĐỊNH HÌNH THỨC THANH TOÁN</h6>
                                                            <p class="pl-1">
                                                            Để kết thúc quá trình đặt hàng, quý khách
                                                            <strong>thanh toán</strong> một khoản tiền <strong>đặt
                                                            cọc trước cho NHẬP HÀNG TRUNG </strong>để chúng tôi thực hiện
                                                            giao dịch mua hàng theo yêu cầu trên đơn hàng.
                                                            </p>
                                                            <p class="pl-2">
                                                                - Số tiền <strong>đặt cọc</strong> trước bao gồm:
                                                            </p>
                                                            <p class="pl-3">
                                                                + <strong>Tiền hàng</strong>: giá sản phẩm trên
                                                            website đặt hàng Trung Quốc, số tiền này <strong>NHẬP HÀNG TRUNG thu hộ cho </strong>
                                                                nhà cung cấp.
                                                            </p>
                                                            <p class="pl-3">
                                                                + <strong>Phí dịch vụ</strong>: là phí khách hàng
                                                            trả cho <strong>NHẬP HÀNG TRUNG </strong>để tiến hành thu mua theo đơn hàng đã đặt.
                                                            </p>
                                                            <h6 class="font-weight-700">CÓ 2 HÌNH THỨC THANH TOÁN:</h6>
                                                            <p class="pl-1">1. Thanh toán trực tiếp.</p>
                                                            <p class="pl-2">
                                                                - Khách hàng có thể đặt cọc trực tiếp tại địa chỉ:
                                                            </p>

                                                            <p class="pl-3 fix"><strong>Khu vực miền Bắc</strong></p>
                                                            <p class="pl-3 fix"><strong>Địa chỉ: Số 66 - Ngõ 66 P. Dịch Vọng Hậu, Dịch Vọng Hậu, Cầu Giấy, Hà Nội</strong></p>                                                                                                           

                                                            <p class="pl-3 fix"><strong>Khu vực miền Nam</strong></p>
                                                            <p class="pl-3 fix"><strong>Địa chỉ : Trường Sơn, Phường 2, Tân Bình, Hồ Chí Minh</strong></p>                                                           

                                                            <p class="pl-1">2. Chuyển khoản trực tiếp.</p>
                                                            <p class="pl-2">- Nội dung chuyển khoản theo cú pháp:</p>
                                                            <p class="pl-3" style="font-size: 16px;">+ <strong>NAP tentaikhoan sodienthoai</strong></p>
                                                            <h6 class="font-weight-700 mb-2 mt-2">THÔNG TIN NGÂN HÀNG:</h6>

                                                            <div class="row">
                                                                <asp:Literal runat="server" ID="ltrBank"></asp:Literal>
                                                            </div>

                                                        </div>
                                                        <div class="card-panel donate-information mt-3">
                                                            <h5>Xác nhận chuyển khoản</h5>
                                                            <hr />
                                                            <div class="order-row">
                                                                <div class="left-fixed">
                                                                    <span>Đã chuyển vào tài khoản:</span>
                                                                </div>
                                                                <div class="right-content">
                                                                    <div class="row">
                                                                        <div class=" col s12">
                                                                            <asp:DropDownList ID="ddlBank" runat="server" DataTextField="BankInfo" CssClass="icons"
                                                                                DataValueField="ID">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="order-row">
                                                                <div class="left-fixed">
                                                                    <span>Số tiền:</span>
                                                                </div>
                                                                <div class="right-content">
                                                                    <div class="row">
                                                                        <div class=" col s12">
                                                                            <telerik:RadNumericTextBox runat="server" Skin="MetroTouch"
                                                                                ID="pAmount" NumberFormat-DecimalDigits="0" Value="0"
                                                                                NumberFormat-GroupSizes="3" Width="100%">
                                                                            </telerik:RadNumericTextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="order-row">
                                                                <div class="left-fixed">
                                                                    <span>Nội dung:</span>
                                                                </div>
                                                                <div class="right-content">
                                                                    <div class="row">
                                                                        <div class=" col s12">
                                                                            <asp:TextBox ID="txtNote" runat="server" CssClass="materialize-textarea" TextMode="MultiLine"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="order-row">
                                                                <div class="left-fixed">
                                                                    <span>Hình ảnh</span>
                                                                </div>
                                                                <div class="right-content">
                                                                    <input class="upload-img" type="file" onchange="previewFiles(this);" multiple title="">
                                                                    <button type="button" class="btn-upload">Upload</button>
                                                                </div>
                                                                <div class="preview-img">
                                                                </div>
                                                            </div>
                                                            <a class="btn right mt-3" href="javascript:;" onclick="AddWallet($(this))">Gửi xác nhận</a>
                                                            <asp:Button ID="btnSend" runat="server" UseSubmitBehavior="false" Style="display: none" Text="Gửi xác nhận"
                                                                CssClass="btn right mt-3" OnClick="btnSend_Click" ValidationGroup="naptien" />
                                                            <div class="clearfix"></div>
                                                        </div>
                                                        <div class="card-panel">
                                                            <h5 class=" mb-2 mt-2">Lịch sử nạp gần đây</h5>
                                                            <hr />
                                                            <div class="responsive-tb mt-3">
                                                                <table class="table    highlight bordered  centered striped">
                                                                    <thead>
                                                                        <tr>
                                                                            <th class="tb-date">Ngày nạp</th>
                                                                            <th class="tb-date">Số tiền</th>
                                                                            <th class="tb-date">Nội dung</th>
                                                                            <th class="tb-date">Trạng thái</th>
                                                                            <th class="tb-date">Hình ảnh</th>
                                                                            <th class="tb-date"></th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <asp:Literal runat="server" ID="ltr"></asp:Literal>
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
            </div>
        </div>
    </div>

    <div id="modalEditDK" class="modal modal-fixed-footer">
        <div class="modal-hd">
            <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
            <h4 class="no-margin center-align">Hình ảnh</h4>
        </div>
        <div class="modal-bd">
            <div class="row">
                <div class="input-field col s12">
                    <p>Hình ảnh:</p>
                    <div class="list-img">
                    </div>
                </div>
                <div class="input-field col s12" style="display: none">
                    <div class="switch status-func">
                        <span class="mr-2">Trạng thái duyệt đơn:  </span>
                        <label>
                            Không duyệt<asp:TextBox ID="EditDKStatus" runat="server" type="checkbox" onclick="StatusDKFunction()"></asp:TextBox><span class="lever"></span>
                            Duyệt
                        </label>

                    </div>
                </div>
            </div>
        </div>
        <div class="modal-ft">
            <div class="ft-wrap center-align">
                <a id="BtnUpDK" onclick="btnUpDK()" style="display: none" class="modal-action btn modal-close waves-effect waves-green mr-2">Cập nhật</a>
                <a href="#!" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
            </div>
        </div>
    </div>
    <asp:Button ID="btnclear" runat="server" UseSubmitBehavior="false" OnClick="btnclear_Click" Style="display: none;" />
    <asp:HiddenField ID="hdfTradeID" runat="server" />
    <asp:HiddenField runat="server" ID="hdfListIMG" />
    <asp:HiddenField ID="hdfUserName" runat="server" />
    <asp:HiddenField ID="hdfDKID" runat="server" Value="0" />
    <asp:HiddenField ID="hdfDKStatus" runat="server" Value="0" />
    <asp:Button ID="buttonUpdateDK" runat="server" OnClick="BtnUpDK_Click" Style="display: none" />
    <telerik:RadScriptBlock ID="sc" runat="server">
        <script type="text/javascript">
            function deleteTrade(ID) {
                var r = confirm("Bạn muốn hủy yêu cầu?");
                if (r == true) {
                    $("#<%= hdfTradeID.ClientID %>").val(ID);
                    $("#<%= btnclear.ClientID %>").click();
                } else {
                }
            }
           <%-- function AddWallet(obj) {
                var r = confirm("Xác nhận nạp tiền!");
                if (r == true) {
                    obj.removeAttr("onclick");
                    $("#<%=btnSend.ClientID%>").click();
                }
          }--%>
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
            function AddWallet(obj) {
                var r = confirm("Xác nhận nạp tiền!");
                var base64 = "";
                if (r == true) {
                    obj.removeAttr("onclick");
                    $(".preview-img img").each(function () {
                        base64 += $(this).attr('src') + "|";
                    })
                    $("#<%=hdfListIMG.ClientID%>").val(base64);
                    $("#<%=btnSend.ClientID%>").click();
                }
            }
            function btnUpDK() {
                $('#<%=buttonUpdateDK.ClientID%>').click();
            }
            function StatusDKFunction() {

                var a = $('#<%=hdfDKStatus.ClientID%>').val();
                if (a == '0') {

                    $('#<%=hdfDKStatus.ClientID%>').val('1');
                }
                else {

                    $('#<%=hdfDKStatus.ClientID%>').val('0');
                }
                console.log($('#<%=hdfDKStatus.ClientID%>').val());
            }
            function EditDK(ID) {
                $('.list-img').empty();
                $.ajax({
                    type: "POST",
                    url: "/nap-tien.aspx/LoadInfor",
                    data: '{ID: "' + ID + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = JSON.parse(msg.d);
                        if (data != null) {
                            var a = data.IsDuyet;
                            if (a == false) {
                                $('#<%=EditDKStatus.ClientID%>').prop('checked', false);
                                $('#<%=hdfDKStatus.ClientID%>').val('0');
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
                            }
                            else {
                                $('#<%=EditDKStatus.ClientID%>').prop('checked', true);
                                $('#<%=hdfDKStatus.ClientID%>').val('1');
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
                            }
                            $('#<%=hdfDKID.ClientID%>').val(data.ID);
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
        </script>
        <style>
            .modal.modal-fixed-footer {
                height: 40%;
            }

            .bank-wrap .icons {
                width: 100px !important;
                height: 55px !important;
                float: right !important;
                vertical-align: middle !important;
                margin-right: 30px !important;
            }

            .fix {
                font-size: 20px;
            }
        </style>
    </telerik:RadScriptBlock>
</asp:Content>
