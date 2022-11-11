<%@ Page Title="Chi tiết đơn hàng" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="NHST.manager.OrderDetail" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<%@ Import Namespace="MB.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .chat-fixed.left-chat {
            left: 275px !important;
        }

        .chat-fixed {
            position: fixed;
            bottom: 0;
            left: 533px !important;
            width: 265px;
            z-index: 2;
        }

        .chat-wrapper .chat-area .chats .chat .chat-avatar {
            flex: none !important;
        }

        .chat-wrapper .chat-area {
            height: 300px !important;
            max-height: none !important;
            min-height: none !important;
        }

        td.rgPagerCell.NextPrevAndNumeric {
            float: right;
            width: auto;
            border: hidden !important;
        }

        div.rgWrap.rgAdvPart {
            display: none;
        }

        div.rgWrap.rgArrPart1 {
            float: left;
        }

        div.rgWrap.rgArrPart2 {
            width: auto;
            float: left;
        }

        div.rgWrap.rgNumPart {
            float: left;
        }

        input.rgPageFirst {
            display: none;
        }

        input.rgPagePrev {
            display: none;
        }

        input.rgPageNext {
            display: none;
        }

        input.rgPageLast {
            display: none;
        }

        div.rgWrap.rgNumPart a {
            margin-left: 10px;
            margin-right: 10px;
        }

        a.rgCurrentPage {
            background: #062A34;
            color: #fff;
            padding: 5px 10px;
            cursor: pointer;
            border-radius: 3px;
        }

        .orderid {
            width: 35px;
            padding-right: 50px;
            padding-top: 23px;
            color: blue;
        }
    </style>
    <div id="main" class="">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Chi tiết đơn hàng</h4>
                    <div class="right-action">
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="order-detail-wrap col s12 section">
                <div class="row">
                    <div class="col s12 m12 l4 sticky-wrap">
                        <div class="card-panel">
                            <div class="order-stick-detail">
                                <div class="order-stick order-owner">
                                    <table class="table   ">
                                        <tbody>
                                            <tr>
                                                <td class="tb-date">Order ID</td>
                                                <td>
                                                    <asp:Literal ID="ltr_OrderID" runat="server"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Loại đơn hàng</td>
                                                <td>
                                                    <span class="bold">Mua hàng hộ</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Trạng thái</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <%--   <asp:Literal ID="ltrStatus1" runat="server"></asp:Literal>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Kho TQ</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlWarehouseFrom" runat="server" CssClass="form-control" onchange="returnWeightFee()" Enabled="false"
                                                        DataValueField="ID" DataTextField="WareHouseName">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <asp:Panel runat="server" ID="nhanhangtai">
                                                    <td class="tb-date">Nhận hàng tại</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlReceivePlace" runat="server" CssClass="form-control" onchange="returnWeightFee()" Enabled="false"
                                                            DataValueField="ID" DataTextField="WareHouseName">
                                                        </asp:DropDownList>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <asp:Panel runat="server" ID="hhvc">
                                                    <td class="tb-date">Phương thức vận chuyển</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="form-control" onchange="returnWeightFee()" Enabled="false"
                                                            DataValueField="ID" DataTextField="ShippingTypeName">
                                                        </asp:DropDownList>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Tổng tiền</td>
                                                <td>
                                                    <asp:Literal ID="ltrlblAllTotal1" runat="server"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Đã trả</td>
                                                <td>
                                                    <asp:Literal ID="lblDeposit1" runat="server"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Còn lại</td>
                                                <td class="red-text bold">
                                                    <asp:Literal ID="lblLeftPay1" runat="server"></asp:Literal></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                                <div class="action-btn center-align ">
                                    <asp:Literal ID="ltrBtnUpdate" runat="server"></asp:Literal>
                                    <asp:Button ID="btnUpdate" runat="server" CssClass="btn pill-btn primary-btn admin-btn full-width"
                                        UseSubmitBehavior="false" Style="display: none" Text="CẬP NHẬT" OnClick="btnUpdate_Click" />
                                    <asp:Button ID="btnThanhtoan" runat="server" CssClass="btn mt-2" Text="THANH TOÁN"
                                        UseSubmitBehavior="false" OnClick="btnThanhtoan_Click" Visible="false" />
                                    <%-- <a href="javascript:;" class="btn mt-2">Cập nhật</a>
                                    <a href="javascript:;" class="btn mt-2">Thanh toán</a>--%>
                                </div>
                            </div>
                        </div>
                        <%--<div class="hide-on-small-only card-panel">
                            <ul class="table-of-contents ">
                                <li><a href="#order-list">Danh sách sản phẩm</a></li>
                                <li><a href="#mvc-list">Danh sách mã vận đơn</a></li>
                                <li><a href="#fee-order">Chi phí đơn hàng</a></li>                               
                                <li><a href="#order-own">Thông tin người đặt hàng - nhận hàng</a></li>                                
                                <li><a href="#history-list">Lịch sử thanh toán - thay đổi</a></li>
                            </ul>
                        </div>--%>
                        <asp:Panel ID="pnadminmanager" runat="server" Visible="false">
                            <div class="card-panel" style="box-shadow: rgba(0, 0, 0, 0.14) 0px 8px 17px 2px, rgba(0, 0, 0, 0.12) 0px 3px 14px 2px, rgba(0, 0, 0, 0.2) 0px 5px 5px -3px; border: 1px solid rgb(0, 0, 0);">
                                <article class="pane-primary">
                                    <div class="title-header bg-dark-gradient">
                                        <h6 class="white-text ">Nhân viên xử lý</h6>
                                    </div>
                                    <div class="cont">
                                        <div class="inner" style="height: auto;">
                                            <div class="grid-row">
                                                <div class="grid-col-100 form-row">
                                                    <p class="lb">Nhân viên saler</p>
                                                    <div class="control-with-suffix fw">
                                                        <asp:DropDownList ID="ddlSaler" runat="server" CssClass="form-control" AppendDataBoundItems="true"
                                                            DataTextField="Username" DataValueField="ID">
                                                        </asp:DropDownList>
                                                        <span class="suffix hl-txt"><i class="fa fa-sort"></i></span>
                                                    </div>
                                                </div>
                                                <div class="grid-col-100 form-row">
                                                    <p class="lb">Nhân viên đặt hàng</p>
                                                    <div class="control-with-suffix fw">
                                                        <asp:DropDownList ID="ddlDatHang" runat="server" CssClass="form-control" AppendDataBoundItems="true"
                                                            DataTextField="Username" DataValueField="ID">
                                                        </asp:DropDownList>
                                                        <span class="suffix hl-txt"><i class="fa fa-sort"></i></span>
                                                    </div>
                                                </div>
                                                <div class="grid-col-100 form-row">
                                                    <p class="lb">Nhân viên chăm sóc khách hàng</p>
                                                    <div class="control-with-suffix fw">
                                                        <asp:DropDownList ID="ddlCSKH" runat="server" CssClass="form-control" AppendDataBoundItems="true"
                                                            DataTextField="Username" DataValueField="ID">
                                                        </asp:DropDownList>
                                                        <span class="suffix hl-txt"><i class="fa fa-sort"></i></span>
                                                    </div>
                                                </div>
                                                <div class="grid-col-100 form-row" style="display: none;">
                                                    <asp:DropDownList ID="ddlKhoTQ" runat="server" CssClass="form-control" AppendDataBoundItems="true"
                                                        DataTextField="Username" DataValueField="ID">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlKhoVN" runat="server" CssClass="form-control" AppendDataBoundItems="true"
                                                        DataTextField="Username" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="grid-col-100 center-txt" style="margin-top: 20px">
                                                    <asp:Button ID="btnStaffUpdate" runat="server" UseSubmitBehavior="false"
                                                        CssClass="btn primary-btn" Text="CẬP NHẬT" OnClick="btnStaffUpdate_Click" />
                                                    <%--<a href="#" class="btn primary-btn">Cập nhật</a>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </article>
                                <article class="pane-primary" style="display: none;">
                                    <div class="heading">
                                        <h3 class="lb">Nhân viên xử lý</h3>
                                    </div>
                                    <div class="cont">
                                        <div class="inner">
                                            <dl class="dl">
                                                <asp:Literal ID="ltr_OrderFee_UserInfo2" runat="server"></asp:Literal>
                                            </dl>
                                        </div>
                                    </div>
                                </article>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="col s12 m12 l8">
                        <div class="card-panel">
                            <div id="order-own" class="section scrollspy order-own">
                                <div class="row">
                                    <div class="col s12 m12">
                                        <div class="title-header bg-dark-gradient">
                                            <h6 class="white-text ">Thông tin người đặt hàng</h6>
                                        </div>
                                        <div class="order-owner">
                                            <asp:Literal ID="ltr_OrderFee_UserInfo" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="col s12 m6" style="display: none;">
                                        <div class="title-header bg-dark-gradient">
                                            <h6 class="white-text ">Thông tin người nhận hàng</h6>
                                        </div>
                                        <div class="order-owner">
                                            <asp:Literal ID="ltr_AddressReceive" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="phituychon">
                            <div class="card-panel">
                                <div class="child-fee">
                                    <div class="title-subheader grey lighten-2 mt-2">
                                        <p class="black-text no-margin font-weight-500">Phí tùy chọn</p>
                                    </div>
                                    <div class="content-panel">
                                        <div class="order-row" style="display: none">
                                            <div class="left-fixed">
                                                <p class="txt">Kiểm đếm</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m2">
                                                        <label>
                                                            <input type="checkbox" class="filled-in chk-check-option" id="cbchkCheck" data-id="3" />
                                                            <span></span>
                                                        </label>
                                                        <asp:HiddenField ID="chkCheck" runat="server" />
                                                    </div>

                                                    <div class="input-field col s12 m5">
                                                        <asp:TextBox runat="server" ID="pCheckNDT" placeholder="0" type="text" onkeyup="CountCheckPro('cny')" value=""></asp:TextBox>
                                                        <label>Tệ (¥)</label>
                                                    </div>
                                                    <div class="input-field col s12 m5">
                                                        <asp:TextBox runat="server" ID="pCheck" placeholder="0" type="text" onkeyup="CountCheckPro('vnd')" value=""></asp:TextBox>
                                                        <label>Việt Nam Đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Đóng gỗ</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m2">
                                                        <label>
                                                            <input type="checkbox" class="filled-in chk-check-option" data-id="4" id="cbchkPackage" />
                                                            <span></span>
                                                        </label>
                                                        <asp:HiddenField runat="server" ID="chkPackage" />
                                                    </div>
                                                    <div class="input-field col s12 m5" style="display: none">
                                                        <asp:TextBox runat="server" ID="pPackedNDT" placeholder="0" type="text" onkeyup="CountFeePackage('ndt')" value=""></asp:TextBox>
                                                        <label>Tệ (¥)</label>
                                                    </div>
                                                    <div class="input-field col s12 m10">
                                                        <asp:TextBox runat="server" ID="pPacked" placeholder="0" type="text" onkeyup="CountFeePackage('vnd')" value=""></asp:TextBox>
                                                        <label>Việt Nam Đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Phí đặc biệt 1</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m2">
                                                        <label>
                                                            <input type="checkbox" class="filled-in chk-check-option" data-id="10" id="cbchkSpecial1" />
                                                            <span></span>
                                                        </label>
                                                        <asp:HiddenField runat="server" ID="chkSpecial1" />
                                                    </div>
                                                    <div style="opacity: 0; visibility: hidden;">
                                                        <div class="input-field col s12 m5" style="display: none">
                                                            <asp:TextBox runat="server" ID="TextBox1" placeholder="0" type="text" onkeyup="CountFeePackage('ndt')" value=""></asp:TextBox>
                                                            <label>Tệ (¥)</label>
                                                        </div>
                                                        <div class="input-field col s12 m10">
                                                            <asp:TextBox runat="server" ID="TextBox2" placeholder="0" type="text" onkeyup="CountFeePackage('vnd')" value=""></asp:TextBox>
                                                            <label>Việt Nam Đồng (VNĐ)</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="order-row" style="transform: translateY(-20px);">
                                            <div class="left-fixed">
                                                <p class="txt">Phí đặc biệt 2</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m2">
                                                        <label>
                                                            <input type="checkbox" class="filled-in chk-check-option" data-id="11" id="cbchkSpecial2" />
                                                            <span></span>
                                                        </label>
                                                        <asp:HiddenField runat="server" ID="chkSpecial2" />
                                                    </div>
                                                    <div class="input-field col s12 m5" style="display: none">
                                                        <asp:TextBox runat="server" ID="TextBox3" placeholder="0" type="text" onkeyup="CountFeePackage('ndt')" value=""></asp:TextBox>
                                                        <label>Tệ (¥)</label>
                                                    </div>
                                                    <div class="input-field col s12 m10" style="transform: translateY(-20px);">
                                                        <asp:TextBox runat="server" ID="txtFeeSpecial" placeholder="0" type="text" onkeyup="CountFeePackage('vnd')" value=""></asp:TextBox>
                                                        <label>Việt Nam Đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="order-row" style="display: none">
                                            <div class="left-fixed">
                                                <p class="txt">Bảo hiểm</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m2">
                                                        <label>
                                                            <input type="checkbox" class="filled-in chk-check-option" data-id="7" id="cbchkIsInsurrance" />
                                                            <span></span>
                                                        </label>
                                                        <asp:HiddenField runat="server" ID="hdfIsInsurrance" />
                                                    </div>
                                                    <div class="input-field col s12 m10">
                                                        <asp:TextBox runat="server" ID="txtInsuranceMoney" placeholder="0" type="text" Enabled="false" value=""></asp:TextBox>
                                                        <label>Việt Nam Đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row" style="display: none">
                                            <div class="left-fixed">
                                                <p class="txt">Giao hàng tận nhà</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m2">
                                                        <label>
                                                            <input type="checkbox" class="filled-in chk-check-option" data-id="5" id="cbchkShiphome" />
                                                            <span></span>
                                                        </label>
                                                        <asp:HiddenField runat="server" ID="chkShiphome" />
                                                    </div>
                                                    <div class="input-field col s12 m5">
                                                        <asp:TextBox runat="server" ID="pShipHome" placeholder="0" type="text" value=""></asp:TextBox>
                                                        <label>Việt Nam Đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Yêu cầu giao hàng</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m2">
                                                        <label style="top: -15px;">
                                                            <input type="checkbox" class="filled-in chk-check-option" data-id="6" id="cbchkIsGiaohang" />
                                                            <span></span>
                                                        </label>
                                                        <asp:HiddenField ID="chkIsGiaohang" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="card-panel">

                            <%--<div id="client-chat" class="chat-fixed left-chat">--%>
                            <div class="row">
                                <div class="col s12 m6 chat-wp">
                                    <div class="title-header bg-dark-gradient">
                                        <h6 class="white-text ">Liên lạc khách hàng <span
                                            class="material-icons right">expand_more</span></h6>
                                    </div>
                                    <div class="customer-chat">
                                        <div class="chat-wrapper">
                                            <div class="chat-content-area animate fadeUp">
                                                <!-- Chat content area -->
                                                <div class="chat-area chat-customer" onscroll="myScroll()">
                                                    <div class="chats">
                                                        <div class="chats" id="ContactCustomer">
                                                            <asp:Literal ID="ltrOutComment" runat="server"></asp:Literal>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!--/ Chat content area -->

                                                <!-- Chat footer <-->
                                                <div class="chat-footer">
                                                    <div class="preview-upload" id="imgUpToCustomer">
                                                    </div>
                                                    <div class="chat-input">
                                                        <input id="txtComment1" type="text" placeholder="Type message here.."
                                                            class="message" style="width: 60%;" />
                                                        <%--         <input class="upload-img" type="file"
                                                                onchange="chatFileUploaded(this)" title="" multiple>--%>
                                                        <asp:FileUpload runat="server" ID="ImgUpLoadToCustomer" class="upload-img" type="file" onchange="chatFileUploaded(this)" AllowMultiple="true" title=""></asp:FileUpload>
                                                        <a href="javascript:;" class="upload-file-chat">
                                                            <i class="material-icons">attach_file</i></a>
                                                        <a id="btnsendcomment" href="javascript:;" class="btn waves-effect waves-light send" onclick="SentMessageToCustomer()">Send</a>
                                                        <asp:Button ID="btnSend1" runat="server" Text="Gửi" ValidationGroup="1n" Style="display: none" CssClass="btn" OnClick="btnSend1_Click" UseSubmitBehavior="false" />
                                                    </div>
                                                    <div class="writebox">
                                                        <span class="info-show"></span>
                                                    </div>
                                                </div>
                                                <!--/ Chat footer -->
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="col s12 m6 local-chat chat-wp">
                                    <div class="title-header bg-dark-gradient">
                                        <h6 class="white-text ">Liên lạc nội bộ <span
                                            class="material-icons right">expand_more</span></h6>
                                    </div>
                                    <div class="customer-chat">
                                        <div class="chat-wrapper">
                                            <div class="chat-content-area animate fadeUp">
                                                <!-- Chat content area -->
                                                <div class="chat-area chat-local">
                                                    <div class="chats">
                                                        <div class="chats" id="ContactLocal">
                                                            <asp:Literal ID="ltrInComment" runat="server"></asp:Literal>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!--/ Chat content area -->

                                                <!-- Chat footer <-->
                                                <div class="chat-footer">
                                                    <div class="preview-upload" id="imgUpToLocal">
                                                    </div>
                                                    <div class="chat-input">
                                                        <input id="txtComment" type="text" placeholder="Type message here.."
                                                            class="message" />
                                                        <asp:FileUpload runat="server" ID="IMGUpLoadToLocal" class="upload-img" type="file" onchange="chatFileUploaded(this)" AllowMultiple="true" title=""></asp:FileUpload>

                                                        <a href="javascript:;" class="upload-file-chat">
                                                            <i class="material-icons">attach_file</i></a>
                                                        <a id="btnsendcommentstaff" href="javascript:;" class="btn send" onclick="SentMessageToLocal()">Send</a>
                                                        <asp:Button ID="btnSend" runat="server" Text="Gửi" ValidationGroup="n" Style="display: none" CssClass="btn" OnClick="btnSend_Click" UseSubmitBehavior="false" />
                                                    </div>
                                                    <div class="writebox">
                                                        <span class="info-show-staff"></span>
                                                    </div>
                                                </div>
                                                <!--/ Chat footer -->
                                            </div>
                                        </div>

                                    </div>
                                </div>

                            </div>
                            <%-- </div>--%>
                            <%--<div id="local-chat" class="chat-fixed">--%>
                            <%--<div class="row">
                                        
                                    <%--</div>--%>
                        </div>

                        <div class="card-panel">
                            <div id="order-code" class="section scrollspy mvc-list">
                                <div class="title-header bg-dark-gradient">
                                    <h6 class="white-text ">Mã đơn hàng</h6>
                                </div>
                                <div class="order-row top-justify">
                                    <div class="right-content">
                                        <div class="list-order list-mdh">
                                            <asp:Literal runat="server" ID="lrtMainOrderCode"></asp:Literal>
                                        </div>
                                        <div class="list-order">
                                            <div class="row order-wrap">
                                                <div class="input-field col m4 s12 MainOrderInPut">
                                                    <input class="col m12 MainOrderCode validate valid" onkeypress="myFunction($(this))" data-ordercodeid="0" placeholder="Mã đơn hàng" type="text" value="">
                                                    <span class="helper-text hide" style="position: absolute; margin-top: 0px">
                                                        <label style="color: red">Mã đơn hàng đã tồn tại</label>
                                                    </span>
                                                </div>
                                                <div class="input-field col s12 m4">
                                                    <a style="margin-left: 20px" class="btn" onclick="AddMDH($(this))">Thêm mã đơn hàng</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="display: none">
                                    <div class="col s12 input-field">
                                        <table>
                                            <tbody>
                                                <asp:TextBox Style="display: none" ID="txtMainOrderCode" runat="server" placeholder="Mã đơn hàng"></asp:TextBox>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>

                        <div class="card-panel">
                            <div id="mvc-list" class="section scrollspy mvc-list">
                                <div class="title-header bg-dark-gradient">
                                    <h6 class="white-text ">Danh sách mã vận đơn</h6>
                                </div>
                                <div class="responsive-tb">
                                    <table class="table  highlight ">
                                        <thead>
                                            <tr>
                                                <th>Mã vận đơn</th>
                                                <th>Cân nặng</th>
                                                <th>Cân quy đổi</th>
                                                <th>Mã đơn hàng</th>
                                                <th>Trạng thái</th>
                                                <th class="tb-date">Ghi chú</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody class="list-product">
                                            <asp:Literal ID="ltrCodeList" runat="server"></asp:Literal>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="order-row top-justify">
                                    <div class="right-content">
                                        <div class="list-order">
                                            <div class="row order-wrap">
                                                <asp:Panel runat="server" ID="mvd">
                                                    <div class="input-field col s12 m4">
                                                        <a href="javascript:;" class="btn add-product" style="display: inline-flex; height: 100%; align-items: center"><i class="material-icons">add</i><span>Mã vận đơn</span></a>
                                                    </div>
                                                </asp:Panel>
                                                <div class=" input-field col m4 s12">
                                                    <div style="display: inline-flex; align-items: center;">
                                                        <div class="">
                                                            <p class="txt">Đủ mã vận đơn</p>
                                                        </div>
                                                        <div class="right-content">
                                                            <div class="row">
                                                                <div class="center-checkbox col s12">
                                                                    <label>
                                                                        <input type="checkbox" id="cbchkIsDoneSmallPackage" class="chk-check-option" data-id="1" />
                                                                        <span></span>
                                                                    </label>
                                                                    <asp:HiddenField ID="chkIsDoneSmallPackage" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class=" input-field col m4 s12" style="display: none">
                                                    <asp:DropDownList runat="server" ID="ddlMainOrderCode">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>

                        <div class="card-panel">
                            <div id="order-list" class="section scrollspy order-list">
                                <div class="title-header bg-dark-gradient">
                                    <h6 class="white-text ">Danh sách sản phẩm (<span><asp:Literal runat="server" ID="ltrTotalProduct"></asp:Literal></span>)</h6>
                                </div>
                                <div class="order-item">
                                    <div class="left-info">
                                        <div class="order-main">
                                            <asp:Literal ID="ltrProducts" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card-panel">
                            <div id="FeeSupport-list" class="section scrollspy fsupport-list">
                                <div class="title-header bg-dark-gradient">
                                    <h6 class="white-text ">Danh sách phụ phí</h6>
                                </div>
                                <div class="responsive-tb">
                                    <table class="table  highlight ">
                                        <thead>
                                            <tr>
                                                <th>Tên phụ phí</th>
                                                <th>Số tiền (VNĐ)</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody class="list-product">
                                            <asp:Literal ID="ltrFeeSupport" runat="server"></asp:Literal>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="order-row top-justify">
                                    <div class="right-content">
                                        <div class="list-order">
                                            <div class="row order-wrap">
                                                <div class="input-field col s12 m4">
                                                    <asp:Panel runat="server" ID="phuphi">
                                                        <a href="javascript:;" class="btn add-product" style="display: inline-flex; height: 100%; align-items: center"><i class="material-icons">add</i><span>Phụ phí</span></a>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>

                        <asp:Panel ID="pnAnhet" runat="server">
                            <div class="card-panel">
                                <div id="fee-order" class="section scrollspy fee-order">
                                    <div class="title-header bg-dark-gradient">
                                        <h6 class="white-text ">Chi phí đơn hàng</h6>
                                    </div>
                                    <div class="child-fee">
                                        <div class="title-subheader grey lighten-2">
                                            <p class="black-text no-margin font-weight-500">Phí cố định</p>
                                        </div>
                                        <div class="content-panel">
                                            <div class="order-row">
                                                <div class="left-fixed">
                                                    <p class="txt">Tiền hàng trên web</p>
                                                </div>
                                                <div class="right-content">
                                                    <div class="row">
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox ID="lblTotalMoneyCNY1" runat="server" Enabled="false"></asp:TextBox>
                                                            <label>Tệ (¥)</label>
                                                        </div>
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox ID="lblTotalMoneyVND1" runat="server" Enabled="false"></asp:TextBox>
                                                            <label>Việt Nam Đồng (VNĐ)</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:Panel runat="server" ID="pricereal">
                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Tổng số tiền mua thật</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="rTotalPriceRealCYN" onkeyup="CountRealPrice()" placeholder="0" type="text" value=""></asp:TextBox>
                                                                <label>Tệ (¥)</label>
                                                            </div>
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="rTotalPriceReal" onkeyup="CountRealPrice1()" placeholder="0" type="text" value=""></asp:TextBox>
                                                                <label>Việt Nam Đồng (VNĐ)</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div class="order-row">
                                                <div class="left-fixed">
                                                    <p class="txt">Phí ship Trung Quốc</p>
                                                </div>
                                                <div class="right-content">
                                                    <div class="row">
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox runat="server" ID="pCNShipFeeNDT" onkeyup="CountShipFee('ndt')" placeholder="0" type="text" value=""></asp:TextBox>
                                                            <label>Tệ (¥)</label>
                                                        </div>
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox runat="server" ID="pCNShipFee" onkeyup="CountShipFee('vnd')" placeholder="0" type="text" value=""></asp:TextBox>
                                                            <label>Việt Nam Đồng (VNĐ)</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="order-row">
                                                <div class="left-fixed">
                                                    <p class="txt">Phí ship thực tế TQ</p>
                                                </div>
                                                <div class="right-content">
                                                    <div class="row">
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox runat="server" ID="pCNShipFeeNDTReal" onkeyup="CountShipFeeReal('ndt')" placeholder="0" type="text" value=""></asp:TextBox>
                                                            <label>Tệ (¥)</label>
                                                        </div>
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox runat="server" ID="pCNShipFeeReal" onkeyup="CountShipFeeReal('vnd')" placeholder="0" type="text" value=""></asp:TextBox>
                                                            <label>Việt Nam Đồng (VNĐ)</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:Panel runat="server" ID="tienhh">
                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Tiền hoa hồng</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="pHHCYN" placeholder="0" type="text" value="" Enabled="false"></asp:TextBox>
                                                                <label>Tệ (¥)</label>
                                                            </div>
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="pHHVND" placeholder="0" type="text" value="" Enabled="false"></asp:TextBox>
                                                                <label>Việt Nam Đồng (VNĐ)</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div class="order-row">
                                                <div class="left-fixed">
                                                    <p class="txt">Phí mua hàng CK(<asp:Label ID="lblCKFeebuypro" runat="server"></asp:Label>%)</p>
                                                </div>
                                                <div class="right-content">
                                                    <div class="row">
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox ID="pBuyNDT" runat="server" onkeyup="CountFeeBuyPro()" placeholder="0" type="text" data-type="number" value=""></asp:TextBox>
                                                            <label>Tệ (¥)</label>
                                                        </div>
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox ID="pBuy" runat="server" placeholder="0" type="text" data-type="number" value=""></asp:TextBox>
                                                            <label>Việt Nam Đồng (VNĐ)</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="order-row" style="display: none">
                                                <div class="left-fixed">
                                                    <p class="txt">Tổng cân nặng</p>
                                                </div>
                                                <div class="right-content">
                                                    <div class="row">
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox runat="server" ID="txtOrderWeight" placeholder="0" type="text" value=""></asp:TextBox>
                                                            <label>Kg</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="order-row">
                                                <div class="left-fixed">
                                                    <p class="txt">
                                                        Phí vận chuyển TQ-VN (CK
                                                    <asp:Label ID="lblCKFeeWeight" runat="server"></asp:Label>%: 
                                                        <asp:Label ID="lblCKFeeweightPrice" runat="server"></asp:Label>)
                                                    </p>
                                                </div>
                                                <div class="right-content">
                                                    <div class="row">
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox runat="server" ID="pWeightNDT" placeholder="0" type="text" onkeyup="returnWeightFee()" value=""></asp:TextBox>
                                                            <label>Cân nặng (Kg)</label>
                                                        </div>
                                                        <div class="input-field col s12 m6">
                                                            <asp:TextBox runat="server" ID="pWeight" placeholder="0" type="text" onkeyup="returnWeightFee()" value=""></asp:TextBox>
                                                            <label>Việt Nam Đồng (VNĐ)</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="order-row" style="display: none">
                                                <div class="left-fixed">
                                                    <p class="txt">Phí lưu kho</p>
                                                </div>
                                                <div class="right-content">
                                                    <div class="row">
                                                        <div class="input-field col s12">
                                                            <asp:TextBox runat="server" ID="rFeeWarehouse" placeholder="0" Enabled="false" type="text" onkeyup="returnWeightFee()" value=""></asp:TextBox>
                                                            <label>Việt Nam Đồng (VNĐ)</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <asp:Panel ID="pnOrderDone" runat="server">
                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">ĐH đã xử lý xong</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m2">
                                                                <label style="top: -15px;">
                                                                    <input type="checkbox" class="filled-in chk-check-option" data-id="8" id="cbchkOrderDone" />
                                                                    <span></span>
                                                                </label>
                                                                <asp:HiddenField ID="chkOrderDone" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="pnOrderPrice" runat="server" Enabled="false">
                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Đã mua hàng</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m2">
                                                                <label style="top: -15px;">
                                                                    <input type="checkbox" class="filled-in chk-check-option" data-id="9" id="cbchkOrderPrice" />
                                                                    <span></span>
                                                                </label>
                                                                <asp:HiddenField ID="chkOrderPrice" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="pnbaogia" runat="server">
                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Báo giá</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="center-checkbox col s12">
                                                                <label>
                                                                    <input type="checkbox" id="cbchkIsCheckPrice" class="chk-check-option" data-id="2" />
                                                                    <span></span>
                                                                </label>
                                                            </div>
                                                            <asp:HiddenField ID="chkIsCheckPrice" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <asp:HiddenField runat="server" ID="hdfBaoGiaVisible" />

                                        </div>
                                    </div>

                                    <asp:Panel runat="server" ID="pnCurrentCNYVN">
                                        <div class="child-fee">
                                            <div class="title-subheader grey lighten-2 mt-2">
                                                <p class="black-text no-margin font-weight-500">Điều chỉnh phí</p>
                                            </div>
                                            <div class="content-panel">
                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Tỉ giá</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="tbCurrentCNYVN" placeholder="0" type="number"></asp:TextBox>
                                                                <label>Việt Nam Đồng (VNĐ)</label>
                                                            </div>
                                                            <div class="input-field col s12 m6">
                                                                <a href="javascript:;" class="btn btnCurrency">Cập nhật tỷ giá</a>
                                                                <asp:Button runat="server" ID="btnCurrency" Style="display: none" UseSubmitBehavior="false" OnClick="btnCurrency_Click" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="card-panel">
                            <div class="child-fee">
                                <div class="title-subheader grey lighten-2 mt-2">
                                    <p class="black-text no-margin font-weight-500">Thanh toán</p>
                                </div>
                                <div class="content-panel">
                                    <asp:Panel runat="server" ID="pnAmount">
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Số tiền phải cọc (VNĐ)</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12">
                                                        <%--<asp:TextBox runat="server" ID="pAmountDeposit" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Việt Nam Đồng (VNĐ)</label>--%>
                                                        <telerik:RadNumericTextBox runat="server" Skin="MetroTouch" BorderStyle="Solid" BorderColor="Black"
                                                            ID="pAmountDeposit" Value="0" MinValue="0" NumberFormat-DecimalDigits="0"
                                                            NumberFormat-GroupSizes="3" Width="100%">
                                                        </telerik:RadNumericTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnAdmin">
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Số tiền đã trả (VNĐ)</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12">
                                                        <%--<asp:TextBox runat="server" ID="pDeposit" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Việt Nam Đồng (VNĐ)</label>--%>
                                                        <telerik:RadNumericTextBox runat="server" Skin="MetroTouch" BorderStyle="Solid" BorderColor="Black"
                                                            ID="pDeposit" Value="0" MinValue="0" NumberFormat-DecimalDigits="0"
                                                            NumberFormat-GroupSizes="3" Width="100%">
                                                        </telerik:RadNumericTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>

                        <div class="card-panel" style="display: none">
                            <div id="fee-total" class="section scrollspy fee-total">
                                <div class="title-header bg-dark-gradient">
                                    <h6 class="white-text ">Chi phí khách cần thanh toán</h6>
                                </div>
                                <div class="fee-wrap">
                                    <ul class="list-total">
                                        <li><span class="lbl">Tiền hàng</span><span class="value"><asp:Label ID="lblMoneyNotFee" runat="server"></asp:Label>
                                            VNĐ</span>
                                        </li>
                                        <li><span class="lbl">Ship nội địa</span><span class="value"><asp:Label ID="lblShipTQ" runat="server" Text="0"></asp:Label>
                                            VNĐ</span>
                                        </li>
                                        <li><span class="lbl">Ship nội địa thực tệ</span><span class="value"><asp:Label ID="lblShipTQReal" runat="server" Text="0"></asp:Label>
                                            VNĐ</span>
                                        </li>
                                        <li><span class="lbl">Phí mua hàng</span><span class="value"><asp:Label ID="lblFeeBuyProduct" runat="server" Text="0"></asp:Label>
                                            VNĐ</span>
                                        </li>
                                        <li><span class="lbl">Phí tùy chọn</span><span class="value"><asp:Label ID="lblAllFee" runat="server" Text="0"></asp:Label>
                                            VNĐ</span></li>
                                        <li><span class="lbl">Phí vận chuyển TQ - VN</span><span class="value"><asp:Label ID="lblFeeTQVN" runat="server" Text="0"></asp:Label>
                                            VNĐ</span></li>
                                        <li><span class="lbl">Tổng chi phí</span><span class="value"><asp:Label ID="lblAllTotal" runat="server" Text="0"></asp:Label>
                                            VNĐ</span>
                                        </li>
                                        <li><span class="lbl">Đã thanh toán</span><span class="value"><asp:Label ID="lblDeposit" runat="server" Text="0"></asp:Label>
                                            VNĐ</span></li>
                                        <li class="grey lighten-2"><span class="lbl">Cần thanh toán</span><span
                                            class="value red-text font-weight-700"><asp:Label ID="lblLeftPay" runat="server" Text="0"></asp:Label>
                                            VNĐ</span></li>
                                    </ul>
                                </div>
                            </div>
                        </div>

                        <div class="card-panel">
                            <div id="history-list" class="section scrollspy history-list">
                                <div class="title-header bg-dark-gradient">
                                    <h6 class="white-text ">Lịch sử</h6>
                                </div>
                                <div class="child-fee">
                                    <div class="title-subheader grey lighten-2">
                                        <p class="black-text no-margin font-weight-500">Lịch sử thanh toán</p>
                                    </div>
                                    <div class="content-panel">
                                        <div class="responsive-tb">
                                            <table class="table  highlight">
                                                <thead>
                                                    <tr>
                                                        <th class="tb-date">Ngày thanh toán</th>
                                                        <th>Loại thanh toán</th>
                                                        <th>Hình thức thanh toán</th>
                                                        <th class="tb-date">Số tiền</th>
                                                    </tr>
                                                </thead>
                                                <tbody class="list-product">
                                                    <asp:Literal ID="ltrpa" runat="server"></asp:Literal>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="child-fee mt-2">
                                    <div class="title-subheader grey lighten-2">
                                        <p class="black-text no-margin font-weight-500">Lịch sử thay đổi</p>
                                    </div>
                                    <div class="content-panel">
                                        <div class="responsive-tb">
                                            <%--<table class="table   highlight   ">
                                                <thead>
                                                    <tr>
                                                        <th class="tb-date">Ngày thay đổi</th>
                                                        <th class="no-wrap">Người thay đổi</th>
                                                        <th class="no-wrap">Quyền hạn</th>
                                                        <th class="tb-date">Nội dung</th>
                                                    </tr>
                                                </thead>
                                                <tbody class="list-product">
                                                    <asp:Literal runat="server" ID="lrtHistoryChange"></asp:Literal>
                                                </tbody>
                                            </table>--%>
                                            <telerik:RadGrid runat="server" ID="gr" OnNeedDataSource="gr_NeedDataSource" AutoGenerateColumns="False"
                                                AllowPaging="True" PageSize="10" EnableEmbeddedSkins="False" EnableEmbeddedBaseStylesheet="False"
                                                AllowAutomaticUpdates="True" OnItemCommand="r_ItemCommand" OnPageIndexChanged="gr_PageIndexChanged">
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView CssClass="table   highlight" DataKeyNames="ID">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="ID" HeaderText="ID" HeaderStyle-Width="2%" FilterControlWidth="50px" AutoPostBackOnFilter="false"
                                                            CurrentFilterFunction="Contains" FilterDelay="2000" ShowFilterIcon="false" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Date" HeaderText="Ngày thay đổi" HeaderStyle-Width="2%"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" FilterDelay="2000" ShowFilterIcon="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Username" HeaderText="Người thay đổi" HeaderStyle-Width="2%"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" FilterDelay="2000" ShowFilterIcon="false">
                                                        </telerik:GridBoundColumn>
                                                        <%--<telerik:GridBoundColumn DataField="RoleName" HeaderText="Quyền hạn" HeaderStyle-Width="2%"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" FilterDelay="2000" ShowFilterIcon="false">
                                                        </telerik:GridBoundColumn>--%>
                                                        <telerik:GridBoundColumn DataField="Content" HeaderText="Nội dung" HeaderStyle-Width="10%"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" FilterDelay="2000" ShowFilterIcon="false">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                    <PagerStyle ShowPagerText="False" Mode="NextPrevAndNumeric" NextPageText="Next →"
                                                        PrevPageText="← Previous" />
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </div>
                                    </div>
                                </div>
                                <div class="child-fee" style="display: none">
                                    <div class="title-subheader grey lighten-2">
                                        <p class="black-text no-margin font-weight-500">Danh sách vvận đơn</p>
                                    </div>
                                    <div class="content-panel">
                                        <div class="responsive-tb">
                                            <table class="table highlight   ">
                                                <thead>
                                                    <tr>
                                                        <th>Mã vận đơn</th>
                                                        <th>Cân nặng</th>
                                                        <th>Trạng thái</th>
                                                    </tr>
                                                </thead>
                                                <tbody class="list-product">
                                                    <asp:Literal ID="ltrMavandon" runat="server"></asp:Literal>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="child-fee" style="display: none">
                                    <div class="title-subheader grey lighten-2">
                                        <p class="black-text no-margin font-weight-500">Danh sách sản phẩm</p>
                                    </div>
                                    <div class="content-panel">
                                        <div class="responsive-tb">
                                            <table class="table   highlight">
                                                <tr>
                                                    <th class="pro">ID</th>
                                                    <th class="pro">Sản phẩm</th>
                                                    <th class="pro">Thuộc tính</th>
                                                    <th class="qty">Số lượng</th>
                                                    <th class="price">Đơn giá</th>
                                                    <th class="price">Giá sản phẩm CNY</th>
                                                    <th class="price">Ghi chú riêng sản phẩm</th>
                                                    <th class="price">Trạng thái</th>
                                                </tr>
                                                <asp:Literal ID="ltrProductPrint" runat="server"></asp:Literal>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="order-panels print6" style="display: none;">
                                    <span style="font-size: 30px; text-align: center; text-transform: uppercase; float: left; width: 100%; margin-bottom: 40px;">Thông tin đơn hàng</span>
                                </div>
                                <div class="order-panels print6" style="display: none;">
                                    <asp:Literal ID="ltr_OrderCode" runat="server"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <asp:HiddenField ID="hdfcurrent" runat="server" />
    <asp:HiddenField ID="hdfFeeBuyProDiscount" runat="server" />
    <asp:HiddenField ID="hdfFeeWeightDiscount" runat="server" />
    <asp:HiddenField ID="hdfFeeweightPriceDiscount" runat="server" />
    <asp:HiddenField ID="hdfOrderID" runat="server" />
    <asp:HiddenField ID="hdforderamount" runat="server" />
    <asp:HiddenField ID="hdfoc2" runat="server" />
    <asp:HiddenField ID="hdfoc3" runat="server" />
    <asp:HiddenField ID="hdfoc4" runat="server" />
    <asp:HiddenField ID="hdfoc5" runat="server" />
    <asp:HiddenField ID="hdfFromPlace" runat="server" />
    <asp:HiddenField ID="hdfReceivePlace" runat="server" />
    <asp:HiddenField ID="hdfShippingType" runat="server" />
    <asp:HiddenField ID="hdfFeeTQVNHN" runat="server" />
    <asp:HiddenField ID="hdfCodeTransactionListMVD" runat="server" />
    <asp:HiddenField ID="hdfFeeTQVNHCM" runat="server" />
    <asp:HiddenField ID="hdfCodeTransactionList" runat="server" />
    <asp:HiddenField ID="hdfListFeeSupport" runat="server" />
    <asp:HiddenField ID="hdfListCheckBox" runat="server" />
    <asp:HiddenField ID="hdfID" runat="server" />
    <asp:HiddenField ID="hdfListMainOrderCode" runat="server" />
    <telerik:RadAjaxManager ID="rAjax" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="gr">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gr" LoadingPanelID="rxLoading"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnUpdate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pEdit" LoadingPanelID="rxLoading"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSend">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pEdit" LoadingPanelID="rxLoading"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gr" LoadingPanelID="rxLoading"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <!-- END: Page Main-->
    <telerik:RadScriptBlock ID="rsc" runat="server">
        <script src="/App_Themes/AdminNew45/assets/js/custom/chat.js"></script>
        <script>
            function myScroll() {
                console.log("A");
            }
            $(document).ready(function () {
                $('#txtComment').on("keypress", function (e) {
                    if (e.keyCode == 13) {
                        SentMessageToLocal();
                    }
                });
            });
            $('.btnCurrency').click(function () {
                var r = confirm("Bạn muốn thay đổi tỷ giá?");
                if (r == true) {
                    $('#<%=btnCurrency.ClientID%>').click();
                }
            });
            $(document).ready(function () {
                $('#txtComment1').on("keypress", function (e) {
                    if (e.keyCode == 13) {
                        SentMessageToCustomer();
                    }
                });
                if ($(".MainOrderInPut").length > 0) {
                    var i = 0;
                    var mainordercodeid = 0;
                    var value = 0;
                    $(".MainOrderInPut").each(function () {
                        if (i == 0) {
                            mainordercodeid = $(this).find(".MainOrderCode").attr('data-ordercodeid');
                            value = $(this).find(".MainOrderCode").attr('value');
                        }
                        i++;
                    });

                    $("#<%=ddlMainOrderCode.ClientID%>").find('option').each(function () {
                        if ($(this).val() == mainordercodeid) {
                            $(this).attr("selected", "selected");
                            $('select').formSelect();
                        }
                    });
                    $('select').formSelect();
                }

            });

            $(function () {
                var chat = $.connection.chatHub;
                chat.client.broadcastMessage = function (uid, id, message) {
                    var u = $("#<%= hdfID.ClientID%>").val();
                    if (uid != u) {
                        var OrderID = $("#<%= hdfOrderID.ClientID%>").val();
                        if (id == OrderID) {
                            $("#ContactCustomer").append(message);
                            loadchatkh();
                            if ($("#client-chat").hasClass("hidden")) {
                                let $noti = $('<div class="toast-noti-fixed teal darken-1" ><p><span>Bạn có 1 tin nhắn mới từ <span>Khách hàng</span></span><a href="javascript:;" class="view-message" data-mess-id="#client-chat">Xem</a></p></div>');
                                $('body').append($noti);
                                setTimeout(function () {
                                    $noti.fadeOut('slow', function () {
                                        $(this).remove();
                                    })
                                }, 2000);
                                setTimeout(function () {
                                    $('.toast-noti-fixed').addClass('show');
                                }, 100);
                            }

                        }
                    }
                };
                chat.client.broadcastMessageForLocal = function (uid, id, message) {
                    var u = $("#<%= hdfID.ClientID%>").val();
                    if (uid != u) {
                        var OrderID = $("#<%= hdfOrderID.ClientID%>").val();
                        if (id == OrderID) {
                            $("#ContactLocal").append(message);
                            loadchat();
                            if ($("#local-chat").hasClass("hidden")) {
                                let $noti = $('<div class="toast-noti-fixed teal darken-1" ><p><span>Bạn có 1 tin nhắn mới từ <span>Nội bộ</span></span><a href="javascript:;" class="view-message" data-mess-id="#local-chat">Xem</a></p></div>');
                                $('body').append($noti);
                                setTimeout(function () {
                                    $noti.fadeOut('slow', function () {
                                        $(this).remove();
                                    })
                                }, 2000);
                                setTimeout(function () {
                                    $('.toast-noti-fixed').addClass('show');
                                }, 100);
                            }

                        }
                    }
                };
                $.connection.hub.start().done(function () {

                });
            });

            function isEmpty(str) {
                return !str.replace(/^\s+/g, '').length; // boolean (`true` if field is empty)
            }
            var currency = $("#<%=hdfcurrent.ClientID%>").val();
            function CountShipFee(type) {
                var shipfeendt = $("#<%= pCNShipFeeNDT.ClientID%>").val();
                var shipfeevnd = $("#<%= pCNShipFee.ClientID%>").val();
                if (type == "vnd") {
                    if ((shipfeevnd) != null || shipfeevnd != "") {
                        var ndt = shipfeevnd / currency;
                        $("#<%= pCNShipFeeNDT.ClientID%>").val(ndt);
                    }
                    else {
                        $("#<%= pCNShipFee.ClientID%>").val(0);
                        $("#<%= pCNShipFeeNDT.ClientID%>").val(0);
                    }
                }
                else {
                    if ((shipfeendt) != null || shipfeendt != "") {

                        var vnd = shipfeendt * currency;
                        $("#<%= pCNShipFee.ClientID%>").val(vnd.toLocaleString("en-US"));
                    }
                    else {
                        $("#<%= pCNShipFee.ClientID%>").val(0);
                        $("#<%= pCNShipFeeNDT.ClientID%>").val(0);
                    }
                }
            }

            function CountShipFeeReal(type) {
                var shipfeendt = $("#<%= pCNShipFeeNDTReal.ClientID%>").val();
                var shipfeevnd = $("#<%= pCNShipFeeReal.ClientID%>").val();
                if (type == "vnd") {
                    if ((shipfeevnd) != null || shipfeevnd != "") {
                        var ndt = shipfeevnd / currency;
                        $("#<%= pCNShipFeeNDTReal.ClientID%>").val(ndt);
                    }
                    else {
                        $("#<%= pCNShipFeeReal.ClientID%>").val(0);
                        $("#<%= pCNShipFeeNDTReal.ClientID%>").val(0);
                    }
                }
                else {
                    if ((shipfeendt) != null || shipfeendt != "") {

                        var vnd = shipfeendt * currency;
                        $("#<%= pCNShipFeeReal.ClientID%>").val(vnd.toLocaleString("en-US"));
                    }
                    else {
                        $("#<%= pCNShipFeeReal.ClientID%>").val(0);
                        $("#<%= pCNShipFeeNDTReal.ClientID%>").val(0);
                    }
                }
            }

            <%--function CountFeePackage(type) {
                var pPackedNDT = $("#<%= pPackedNDT.ClientID%>").val();
                var pPackedVND = $("#<%= pPacked.ClientID%>").val();
                if (type == "vnd") {
                    if ((pPackedVND) != null || pPackedVND != "") {
                        var ndt = pPackedVND / currency;
                        $("#<%= pPackedNDT.ClientID%>").val(ndt);
                    }
                    else {
                        $("#<%= pPackedNDT.ClientID%>").val(0);
                        $("#<%= pPacked.ClientID%>").val(0);
                    }
                }
                else {
                    if ((pPackedNDT) != null || pPackedNDT != "") {
                        var vnd = pPackedNDT * currency;
                        $("#<%= pPacked.ClientID%>").val(vnd.toLocaleString("en-US"));
                    }
                    else {
                        $("#<%= pPackedNDT.ClientID%>").val(0);
                        $("#<%= pPacked.ClientID%>").val(0);
                    }
                }
            }--%>
            function CountCheckPro(type) {
                var pCheckNDT = $("#<%= pCheckNDT.ClientID%>").val();
                var pCheckVND = $("#<%= pCheck.ClientID%>").val();
                if (type == 'vnd') {
                    if ((pCheckVND) != null || pCheckVND != "") {
                        var ndt = pCheckVND / currency;
                        $("#<%= pCheckNDT.ClientID%>").val(ndt);
                    }
                    else {
                        $("#<%= pCheckNDT.ClientID%>").val(0);
                        $("#<%= pCheck.ClientID%>").val(0);
                    }
                }
                else {
                    if ((pCheckNDT) != null || pCheckNDT != "") {
                        var vnd = pCheckNDT * currency;
                        $("#<%= pCheck.ClientID%>").val(vnd.toLocaleString("en-US"));
                    }
                    else {
                        $("#<%= pCheckNDT.ClientID%>").val(0);
                        $("#<%= pCheck.ClientID%>").val(0);
                    }
                }
            }

            function deleteOrderCode(obj) {
                var r = confirm("Bạn muốn xóa mã vận đơn này?");
                if (r == true) {
                    var id = obj.parent().parent().attr("data-packageID");
                    if (id > 0) {
                        addLoading();
                        $.ajax({
                            type: "POST",
                            url: "/manager/OrderDetail.aspx/DeleteSmallPackage",
                            data: "{IDPackage:'" + id + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var data = JSON.parse(msg.d);
                                if (data != null) {
                                    obj.parent().parent().remove();
                                    returnWeightFee2();
                                    //gettotalweight();
                                    //UpdateOrder();
                                    removeLoading();
                                }
                                else {
                                    alert('Bạn không có quyền xóa mã vận đơn');
                                    removeLoading();
                                }
                            },
                            error: function (xmlhttprequest, textstatus, errorthrow) {
                                alert('Lỗi');
                                removeLoading();
                            }
                        });
                    }
                    else {
                        obj.parent().parent().remove();
                        returnWeightFee();
                    }

                }
                else {

                }
            }

            function deleteSupportFee(obj) {
                var r = confirm("Bạn muốn xóa phụ phí này?");
                if (r == true) {
                    var id = obj.parent().parent().attr("data-feesupportid");
                    if (id > 0) {
                        $.ajax({
                            type: "POST",
                            url: "/manager/OrderDetail.aspx/DeleteSupportFee",
                            data: "{IDPackage:'" + id + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                var ret = msg.d;
                                if (ret == "ok") {

                                    obj.parent().parent().remove();
                                    window.location.reload();
                                }
                                else if (ret == "role") {
                                    alert('Bạn không có quyền thực hiện chức năng này.');
                                }
                                else
                                    alert('Lỗi');

                            },
                            error: function (xmlhttprequest, textstatus, errorthrow) {
                                alert('Lỗi');
                            }
                        });
                    }
                    else {
                        obj.parent().parent().remove();
                    }

                }
                else {

                }
            }

            //function deleteSupportFee(obj) {
            //    var r = confirm("Bạn muốn xóa phụ phí này?");
            //    if (r == true) {
            //        var id = obj.parent().parent().attr("data-feesupportid");
            //        if (id > 0) {
            //            $.ajax({
            //                type: "POST",
            //                url: "/manager/OrderDetail.aspx/DeleteSupportFee",
            //                data: "{IDPackage:'" + id + "'}",
            //                contentType: "application/json; charset=utf-8",
            //                dataType: "json",
            //                success: function (msg) {
            //                    var data = JSON.parse(msg.d);
            //                    if (data != null) {
            //                        obj.parent().parent().remove();
            //                        window.location.reload();
            //                    }
            //                    else if (data == "role") {
            //                        alert('Bạn không có quyền thực hiện chức năng này.');
            //                    }
            //                    else {
            //                        alert('Lỗi');
            //                    }
            //                },
            //                error: function (xmlhttprequest, textstatus, errorthrow) {
            //                    alert('Lỗi');
            //                }
            //            });
            //        }
            //        else {
            //            obj.parent().parent().remove();
            //        }
            //    }
            //    else {

            //    }
            //}

            function gettotalweight() {
                //txtOrderWeight, txtOrdertransactionCodeWeight
                var totalweight = 0;
                $(".transactionWeight").each(function () {
                    totalweight += parseFloat($(this).val());
                });
                var receiveplace = $("#<%= hdfReceivePlace.ClientID%>").val();
                var shippingtype = $("#<%= hdfShippingType.ClientID%>").val();
                <%--var hnfee = $("#<%= hdfFeeTQVNHN.ClientID%>").val();
                var hcmfee = $("#<%= hdfFeeTQVNHCM.ClientID%>").val();
                var countfeearea = "";
                if (receiveplace == "1") {
                    countfeearea = hnfee;
                }
                else {
                    countfeearea = hcmfee;
                }--%>
                var currency = $("#<%=hdfcurrent.ClientID%>").val();
                var firstfeeweight = 0;
                var firstfeepacked = 20;
                var leftweight = totalweight;
                var totalfeeweight = 0;

                var steps = countfeearea.split('|');
                if (steps.length > 0) {
                    for (var i = 0; i < steps.length - 1; i++) {
                        var step = steps[i];
                        var itemstep = step.split(',');
                        var wf = parseFloat(itemstep[1]);
                        var wt = parseFloat(itemstep[2]);
                        var amount = parseFloat(itemstep[3]);
                        if (totalweight >= wf && totalweight <= wt) {
                            totalfeeweight = firstfeeweight + (leftweight * amount);
                        }
                    }
                }

                var feepackedndt = leftweight * 1 + 20;
                var feepackedvnd = feepackedndt * currency;

                var pweightndt = totalfeeweight / currency;
                $("#<%= pWeightNDT.ClientID %>").val(totalweight);

                $("#<%= txtOrderWeight.ClientID %>").val(totalweight);
                CountFeeWeight("kg");
            }
            function SentMessageToCustomer() {
                var data = new FormData();
                var orderID = $("#<%=hdfOrderID.ClientID%>").val();
                var comment = $("#txtComment1").val();
                var files = $("#<%=ImgUpLoadToCustomer.ClientID%>").get(0).files;
                var url = "";
                var real = "";
                if (files.length > 0) {
                    for (var i = 0; i < files.length; i++) {
                        data.append(files[i].name, files[i]);
                    }
                    data.append("comment", comment);
                    data.append("orderID", orderID);
                    $.ajax({
                        url: '/HandlerCS.ashx',
                        type: 'POST',
                        data: data,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (file) {

                            if (file.length > 0) {
                                file.forEach(function (data, index) {
                                    url += data.name + "|";
                                    real += data.realname + "|";
                                });
                                $("#<%=ImgUpLoadToCustomer.ClientID%>").replaceWith($("#<%=ImgUpLoadToCustomer.ClientID%>").val('').clone(true));
                                sendCustomerComment(orderID, comment, url, real);

                            }
                        },
                        error: function (e) {
                            console.log(e)
                        }
                    });

                }
                else {
                    sendCustomerComment(orderID, comment, url, real);
                }


            }
            function sendCustomerComment(orderID, comment, url, real) {
                if (isEmpty(comment) && url == "") {
                    $(".info-show").html("Vui lòng điền nội dung.").attr("style", "color:red");
                }
                else {
                    $(".info-show").html("Đang cập nhật...").attr("style", "color:blue");
                    $.ajax({
                        type: "POST",
                        url: "/manager/OrderDetail.aspx/sendcustomercomment",
                        data: "{comment:'" + comment + "',id:'" + orderID + "',urlIMG:'" + url + "',real:'" + real + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var data = JSON.parse(msg.d);
                            if (data != null) {
                                var dataComment = data.Comment;
                                $("#ContactCustomer").append(dataComment);
                                $(".materialboxed").materialbox({
                                    inDuration: 200,
                                    onOpenStart: function (element) {
                                        $(element).parents('.chat-area.ps').attr('style', 'overflow:visible !important;');
                                        $('.inside-chat').hide();
                                    },
                                    onCloseStart: function (element) {
                                        $(element).parents('.chat-area.ps').attr('style', '');
                                        $('.inside-chat').show();
                                    }
                                });
                                $("#imgUpToCustomer").html("");

                                $(".info-show").html("");
                                $("#txtComment1").val("");
                                $('select').formSelect();
                                loadchatkh();
                            }
                            else {
                                $("#imgUpToCustomer").html("");
                                $(".info-show").html("Có lỗi trong quá trình gửi, vui lòng thử lại sau.").attr("style", "color:red");
                            }
                        },
                        error: function (xmlhttprequest, textstatus, errorthrow) {
                            console.log('lỗi checkend');
                        }
                    });
                }

            }

            function SentMessageToLocal() {
                var data = new FormData();
                var orderID = $("#<%=hdfOrderID.ClientID%>").val();
                var comment = $("#txtComment").val();
                var files = $("#<%=IMGUpLoadToLocal.ClientID%>").get(0).files;
                var url = "";
                var real = "";
                if (files.length > 0) {
                    for (var i = 0; i < files.length; i++) {
                        data.append(files[i].name, files[i]);
                    }
                    data.append("comment", comment);
                    data.append("orderID", orderID);
                    $.ajax({
                        url: '/HandlerCS.ashx',
                        type: 'POST',
                        data: data,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (file) {

                            if (file.length > 0) {
                                file.forEach(function (data, index) {
                                    url += data.name + "|";
                                    real += data.realname + "|";
                                });
                                $("#<%=IMGUpLoadToLocal.ClientID%>").replaceWith($("#<%=IMGUpLoadToLocal.ClientID%>").val('').clone(true));
                                sendStaffComment(orderID, comment, url, real);

                            }
                        },
                        error: function (e) {
                            console.log(e)
                        }
                    });

                }
                else {
                    sendStaffComment(orderID, comment, url, real);
                }


            }
            function sendStaffComment(orderID, comment, url, real) {

                if (isEmpty(comment) && url == "") {
                    $(".info-show-staff").html("Vui lòng điền nội dung.").attr("style", "color:red");
                }
                else {
                    $(".info-show-staff").html("Đang cập nhật...").attr("style", "color:blue");
                    $.ajax({
                        type: "POST",
                        url: "/manager/OrderDetail.aspx/sendstaffcomment",
                        data: "{comment:'" + comment + "',id:'" + orderID + "',urlIMG:'" + url + "',real:'" + real + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var data = JSON.parse(msg.d);
                            if (data != null) {
                                var dataComment = data.Comment;
                                $("#ContactLocal").append(dataComment);
                                $(".materialboxed").materialbox({
                                    inDuration: 200,
                                    onOpenStart: function (element) {
                                        $(element).parents('.chat-area.ps').attr('style', 'overflow:visible !important;');
                                    },
                                    onCloseStart: function (element) {
                                        $(element).parents('.chat-area.ps').attr('style', '');
                                    }
                                });
                                $("#imgUpToLocal").html("");

                                $(".info-show-staff").html("");
                                $("#txtComment").val("");

                                $('select').formSelect();

                            }
                            else {
                                $("#imgUpToLocal").html("");
                                $(".info-show-staff").html("Có lỗi trong quá trình gửi, vui lòng thử lại sau.").attr("style", "color:red");
                            }
                        },
                        error: function (xmlhttprequest, textstatus, errorthrow) {
                            console.log('lỗi checkend');
                        }
                    });
                }

            }
            function UpdateOrder() {
                //btnUpdate
                var list = "";
                var listmvd = "";

                var check = true;
                $(".order-versionnew").each(function () {
                    var id = $(this).attr("data-packageID");
                    var code = $(this).find(".transactionCode").val();
                    var mainOrderCode = $(this).find(".transactionCodeMainOrderCode").val();
                    if (mainOrderCode == "0") {
                        check = false;
                    }
                    var weight = $(this).find(".transactionWeight").val();
                    var status = $(this).find(".transactionCodeStatus").val();
                    var note = $(this).find(".transactionDescription").val();
                    list += id + "," + code + "," + weight + "," + status + "," + note + "," + mainOrderCode + "|";
                    listmvd += code + "|";
                });
                var listFee = "";
                $(".fee-versionnew").each(function () {
                    var id = $(this).attr("data-feesupportid");
                    var sname = $(this).find(".feesupportname").val();
                    var money = $(this).find(".feesupportvnd").val();
                    listFee += id + "," + sname + "," + money + "|";
                });


                var chuoi = "";
                $('.chk-check-option').each(function () {
                    var id = $(this).attr('data-id');

                    if ($(this).prop("checked") == true) {
                        chuoi += id + "," + "1|";
                    }
                    else {
                        chuoi += id + "," + "0|";
                    }
                });

                if (check) {
                    $("#<%=hdfListCheckBox.ClientID%>").val(chuoi);
                    $("#<%=hdfCodeTransactionList.ClientID%>").val(list);
                    $("#<%=hdfCodeTransactionListMVD.ClientID%>").val(listmvd);
                    $("#<%=hdfListFeeSupport.ClientID%>").val(listFee);
                    console.log($("#<%=hdfFeeweightPriceDiscount.ClientID%>").val());
                    $("#<%=btnUpdate.ClientID%>").click();
                }
                else
                    alert("Vui lòng chọn mã đơn hàng");
            }
            function CountRealPrice() {
                var rTotalPriceRealCYN = $("#<%= rTotalPriceRealCYN.ClientID%>").val();
                var rTotalPriceReal = $("#<%= rTotalPriceReal.ClientID%>").val();
                var newpriuce = rTotalPriceRealCYN * currency;
                $("#<%= rTotalPriceReal.ClientID%>").val(newpriuce.toLocaleString("en-US"));

            }
            function CountRealPrice1() {
                var rTotalPriceRealCYN = $("#<%= rTotalPriceRealCYN.ClientID%>").val();
                var rTotalPriceReal = $("#<%= rTotalPriceReal.ClientID%>").val();
                var newpriuce = rTotalPriceReal / currency;
                $("#<%= rTotalPriceRealCYN.ClientID%>").val(newpriuce);

            }
            function CountFeeBuyPro() {
                var pBuyNotDC = $("#<%= pBuyNDT.ClientID%>").val();
                var pBuyDC = $("#<%= pBuy.ClientID%>").val();

                var discountper = $("#<%= hdfFeeBuyProDiscount.ClientID%>").val();
                var subfee = pBuyNotDC * discountper / 100;
                var vnd = (pBuyNotDC - subfee) * currency;
                $("#<%= pBuy.ClientID%>").val(vnd.toLocaleString("en-US"));
            }
            function returnWeightFee() {
                var orderID = $("#<%= hdfOrderID.ClientID%>").val();
                var WarehouseFrom = $("#<%= ddlWarehouseFrom.ClientID%>").val();
                var receiveValue = $("#<%= ddlReceivePlace.ClientID%>").val();
                var shippingTypeValue = $("#<%= ddlShippingType.ClientID%>").val();
                var currency = $("#<%=hdfcurrent.ClientID%>").val();
                var totalweight = 0;
                $(".transactionWeight").each(function () {
                    totalweight += parseFloat($(this).val());
                });
                $.ajax({
                    type: "POST",
                    url: "/manager/orderdetail.aspx/CountFeeWeight",
                    data: "{orderID:'" + orderID + "',receivePlace:'" + receiveValue + "',shippingTypeValue:'" + shippingTypeValue + "',weight:'" + totalweight + "',WarehouseFrom:'" + WarehouseFrom + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = JSON.parse(msg.d);
                        if (data != "none") {
                            var FeeWeightVND = data.FeeWeightVND;
                            var FeeWeightCYN = data.FeeWeightCYN;
                            var DiscountFeeWeightCYN = data.DiscountFeeWeightCYN;
                            var DiscountFeeWeightVN = data.DiscountFeeWeightVN;
                            var FeeBuyProCYN = data.FeeBuyProCYN;
                            var FeeBuyProVN = data.FeeBuyProVN;

                            //alert(FeeWeightVND + " - " + FeeWeightCYN + " - " + DiscountFeeWeightCYN + " - " + DiscountFeeWeightVN);
                            $("#<%=pWeightNDT.ClientID%>").val(totalweight);
                            $("#<%=pWeight.ClientID%>").val(FeeWeightVND);
                            //$("#<%=pBuy.ClientID%>").val(FeeBuyProVN);
                            $("#<%=lblCKFeeweightPrice.ClientID%>").text(parseFloat(DiscountFeeWeightVN));
                            $("#<%=hdfFeeweightPriceDiscount.ClientID%>").val(parseFloat(DiscountFeeWeightVN));
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        //alert('lỗi checkend');
                    }
                });
                //alert(receiveValue + " - " + shippingTypeValue + " - " + currency + " - " + totalweight);
            }
            function returnWeightFee2() {
                var orderID = $("#<%= hdfOrderID.ClientID%>").val();
                var WarehouseFrom = $("#<%= ddlWarehouseFrom.ClientID%>").val();
                var receiveValue = $("#<%= ddlReceivePlace.ClientID%>").val();
                var shippingTypeValue = $("#<%= ddlShippingType.ClientID%>").val();
                var currency = $("#<%=hdfcurrent.ClientID%>").val();
                var totalweight = 0;
                $(".transactionWeight").each(function () {
                    totalweight += parseFloat($(this).val());
                });
                $.ajax({
                    type: "POST",
                    url: "/manager/orderdetail.aspx/CountFeeWeight",
                    data: "{orderID:'" + orderID + "',receivePlace:'" + receiveValue + "',shippingTypeValue:'" + shippingTypeValue + "',weight:'" + totalweight + "',WarehouseFrom:'" + WarehouseFrom + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = JSON.parse(msg.d);
                        if (data != "none") {
                            var FeeWeightVND = data.FeeWeightVND;
                            var FeeWeightCYN = data.FeeWeightCYN;
                            var DiscountFeeWeightCYN = data.DiscountFeeWeightCYN;
                            var DiscountFeeWeightVN = data.DiscountFeeWeightVN;
                            //alert(FeeWeightVND + " - " + FeeWeightCYN + " - " + DiscountFeeWeightCYN + " - " + DiscountFeeWeightVN);
                            $("#<%=pWeightNDT.ClientID%>").val(totalweight);
                            $("#<%=pWeight.ClientID%>").val(FeeWeightVND);
                            $("#<%= lblCKFeeweightPrice.ClientID%>").text(parseFloat(DiscountFeeWeightVN));
                            $("#<%= hdfFeeweightPriceDiscount.ClientID%>").val(parseFloat(DiscountFeeWeightVN));
                            UpdateOrder();
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        //alert('lỗi checkend');
                    }
                });
                //alert(receiveValue + " - " + shippingTypeValue + " - " + currency + " - " + totalweight);
            }


            $(window).load(function () {
                var window_height = $('.chat-area').height();
                var document_height = $('.chats').height();
                $('.chat-area').animate({
                    scrollTop: window_height + document_height
                }, 1);
                $(".materialboxed").materialbox({
                    inDuration: 200,
                    onOpenStart: function (element) {
                        $(element).parents('.chat-area.ps').attr('style', 'overflow:visible !important;');
                    },
                    onCloseStart: function (element) {
                        $(element).parents('.chat-area.ps').attr('style', '');
                    }
                });
            });

            function loadchatkh() {
                var window_height = $('.chat-area').height();
                var document_height = $('.chats').height();
                $('.chat-area.chat-customer').animate({
                    scrollTop: window_height + document_height
                }, 1);
                $(".materialboxed").materialbox({
                    inDuration: 200,
                    onOpenStart: function (element) {
                        $(element).parents('.chat-area.ps').attr('style', 'overflow:visible !important;');
                    },
                    onCloseStart: function (element) {
                        $(element).parents('.chat-area.ps').attr('style', '');
                    }
                });
            }


            function loadchat() {
                var window_height = $('.chat-area').height();
                var document_height = $('.chats').height();
                $('.chat-area.chat-local').animate({
                    scrollTop: window_height + document_height
                }, 1);
                $(".materialboxed").materialbox({
                    inDuration: 200,
                    onOpenStart: function (element) {
                        $(element).parents('.chat-area.ps').attr('style', 'overflow:visible !important;');
                    },
                    onCloseStart: function (element) {
                        $(element).parents('.chat-area.ps').attr('style', '');
                    }
                });
            }
            function deleteMVD(obj) {
                var r = confirm("Bạn muốn xóa mã đơn hàng này?");
                if (r == true) {
                    var id = obj.parent().find(".MainOrderInPut").find(".MainOrderCode").attr("data-orderCodeID");
                    $.ajax({
                        type: "POST",
                        url: "/manager/OrderDetail.aspx/DeleteMainOrderCode",
                        data: "{IDCode:'" + id + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var data = JSON.parse(msg.d);
                            if (data != null) {
                                obj.parent().remove();
                                $(".transactionCodeMainOrderCode").children().each(function () {
                                    if ($(this).val() == "" + id + "") {
                                        $(this).remove();
                                    }
                                    $('select').formSelect();
                                });
                                $("#<%=ddlMainOrderCode.ClientID%>").find('option').each(function () {
                                    if ($(this).val() == "" + id + "")
                                        $(this).remove();
                                    $('select').formSelect();
                                });
                            }
                            else {
                                alert('Bạn không có quyền xóa mã đơn hàng');
                            }


                        },
                        error: function (xmlhttprequest, textstatus, errorthrow) {
                            alert('Lỗi');
                        }
                    });
                }
            }

            function loadMainOrderCode(Value) {
                if ($(".MainOrderInPut").length > 0) {
                    alert(1);
                }
            }

            function AddMDH(obj) {
                var id = obj.parent().parent().find(".MainOrderInPut").find(".MainOrderCode").attr("data-orderCodeID");
                var MainOrderCode = obj.parent().parent().find(".MainOrderInPut").find(".MainOrderCode").val();
                var MainOrderID = $("#<%= hdfOrderID.ClientID%>").val();
                $.ajax({
                    type: "POST",
                    url: "/manager/orderdetail.aspx/UpdateMainOrderCode",
                    data: "{ID:'" + id + "',MainOrderCode:'" + MainOrderCode + "',MainOrderID:'" + MainOrderID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = JSON.parse(msg.d);
                        if (data != null) {
                            if (data != id) {
                                var tableHTML = $('.right-content .list-mdh');
                                var html = "<div class=\"row order-wrap slide-up\">";
                                html += "<div class=\"input-field col s10 m11 MainOrderInPut\">";
                                html += "<input type=\"text\"  value=\"" + MainOrderCode + "\" class=\"MainOrderCode\"  data-orderCodeID=\"" + id + "\"  onkeypress=\"myFunction($(this))\" >";
                                html += "<span class=\"helper-text hide\" style=\"position:absolute;\">";
                                html += "<label style=\"color:green\">Đã cập nhật</label>";
                                html += "</span>";
                                html += "</div>";
                                html += "<a href='javascript:;' style=\"line-height:80px;position:absolute\" class=\"remove-order tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class=\"material-icons\">remove_circle</i></a>";
                                html += "</div>";
                                tableHTML.append(html);
                                obj.parent().parent().find(".MainOrderInPut").find(".MainOrderCode").val("");
                                $("#<%=ddlMainOrderCode.ClientID%>").append('<option value="' + data + '" selected>' + MainOrderCode + '</option>');
                                $(".transactionCodeMainOrderCode").each(function () {
                                    $(this).append('<option value="' + data + '" selected>' + MainOrderCode + '</option>');
                                    $('select').formSelect();
                                });

                                $('.tooltipped')
                                    .tooltip({
                                        trigger: 'manual'
                                    })
                                    .tooltip('show');

                                $('select').formSelect();
                            }
                            else {
                                obj.val(MainOrderCode);
                                obj.removeClass("slide-up").addClass("slide-up");
                            }
                        } else {
                            obj.parent().parent().find(".MainOrderInPut").find(".helper-text").removeClass("hide");
                            setTimeout(function () {
                                obj.parent().parent().find(".MainOrderInPut").find(".helper-text").addClass("hide");
                            }, 3000);
                            console.log("@@@@@@@@@");
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        alert('lỗi checkend');
                    }
                });

            }



            function myFunction(obj) {
                if (event.which == 13 || event.keyCode == 13) {
                    var id = obj.attr("data-OrderCodeID");
                    var MainOrderCode = obj.val();
                    console.log(id + "," + MainOrderCode);
                    var MainOrderID = $("#<%= hdfOrderID.ClientID%>").val();
                    addLoading();
                    $.ajax({
                        type: "POST",
                        url: "/manager/orderdetail.aspx/UpdateMainOrderCode",
                        data: "{ID:'" + id + "',MainOrderCode:'" + MainOrderCode + "',MainOrderID:'" + MainOrderID + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            debugger;
                            var data = JSON.parse(msg.d);
                            if (data != null) {
                                if (data != id) {
                                    var tableHTML = $('.right-content .list-mdh');
                                    var html = "<div class=\"row order-wrap slide-up\">";
                                    html += "<div class=\"input-field col s10 m11 MainOrderInPut\">";
                                    html += "<input type=\"text\"  value=\"" + MainOrderCode + "\" class=\"MainOrderCode\"  data-orderCodeID=\"" + data + "\"  onkeypress=\"myFunction($(this))\" >";
                                    html += "<span class=\"helper-text hide\" style=\"position:absolute;\">";
                                    html += "<label style=\"color:green\">Đã cập nhật</label>";
                                    html += "</span>";
                                    html += "</div>";
                                    html += "<a href='javascript:;' onclick=\"deleteMVD($(this))\" style=\"line-height:80px;position:absolute\" class=\"remove-order\"><i class=\"material-icons\">remove_circle</i></a>";
                                    html += "</div>";
                                    tableHTML.append(html);
                                    obj.val("");
                                    $("#<%=ddlMainOrderCode.ClientID%>").append('<option value="' + data + '" selected>' + MainOrderCode + '</option>');
                                    $(".transactionCodeMainOrderCode").each(function () {
                                        $(this).append('<option value="' + data + '">' + MainOrderCode + '</option>');
                                        $('select').formSelect();
                                    });
                                    $('select').formSelect();
                                }
                                else {
                                    $(".transactionCodeMainOrderCode").children().each(function () {
                                        if ($(this).val() == "" + id + "") {
                                            $(this).text(MainOrderCode);
                                        }
                                        $('select').formSelect();
                                    });
                                    $("#<%=ddlMainOrderCode.ClientID%>").find('option').each(function () {
                                        if ($(this).val() == "" + id + "")
                                            $(this).text(MainOrderCode);
                                        $('select').formSelect();
                                    });
                                    obj.parent().parent().find(".MainOrderInPut").find(".helper-text").children().text("Đã cập nhật");
                                    obj.parent().parent().find(".MainOrderInPut").find(".helper-text").children().css("color", "green");
                                    obj.parent().parent().find(".MainOrderInPut").find(".helper-text").removeClass("hide");
                                    setTimeout(function () {
                                        obj.parent().parent().find(".MainOrderInPut").find(".helper-text").addClass("hide");
                                    }, 3000);
                                    //obj.removeClass("slide-up").addClass("slide-up");
                                }

                            } else {
                                obj.parent().parent().find(".MainOrderInPut").find(".helper-text").children().text("Mã đơn hàng đã tồn tại");
                                obj.parent().parent().find(".MainOrderInPut").find(".helper-text").children().css("color", "red");
                                obj.parent().parent().find(".MainOrderInPut").find(".helper-text").removeClass("hide");
                                setTimeout(function () {
                                    obj.parent().parent().find(".MainOrderInPut").find(".helper-text").addClass("hide");
                                }, 3000);
                            }
                            removeLoading();
                        },
                        error: function (xmlhttprequest, textstatus, errorthrow) {
                            alert('lỗi checkend');
                            removeLoading();
                        }
                    });
                }
            }

            function loadMVD(obj) {
                var selectedCountry = obj.children("option:selected").val();
                console.log(selectedCountry);

            };
            function addSelect() {
                $(".transactionCodeMainOrderCode").children().each(function () {
                    if ($(this).val() == '96') {
                        $(this).remove();
                    }
                    $('select').formSelect();
                });


            }
            function toastNoti(timeout) {

            }
            $('body').on('click', '.view-message', function () {
                let $this = $(this);
                let chatId = $this.data('mess-id');
                $(chatId).removeClass('hidden');

            });



            $(document).ready(function () {

                var cb1 = $("#<%= chkIsDoneSmallPackage.ClientID%>").val();
                var cb2 = $("#<%= chkIsCheckPrice.ClientID%>").val();
                var checkcb2 = $("#<%= hdfBaoGiaVisible.ClientID%>").val();
                var cb3 = $("#<%= chkCheck.ClientID%>").val();
                var cb4 = $("#<%= chkPackage.ClientID%>").val();
                var cb5 = $("#<%= chkShiphome.ClientID%>").val();
                var cb6 = $("#<%= chkIsGiaohang.ClientID%>").val();
                var cb7 = $("#<%=hdfIsInsurrance.ClientID %>").val();
                var cb8 = $("#<%=chkOrderDone.ClientID %>").val();
                var cb9 = $("#<%=chkOrderPrice.ClientID %>").val();
                var cb10 = $("#<%=chkSpecial1.ClientID %>").val();
                var cb11 = $("#<%=chkSpecial2.ClientID %>").val();

                $('#cbchkIsDoneSmallPackage').prop("checked", (/true/i).test(cb1.toLowerCase()))
                if (checkcb2 == "1") {

                    $('#cbchkIsCheckPrice').prop("checked", (/true/i).test(cb2.toLowerCase()))
                }
                $('#cbchkCheck').prop("checked", (/true/i).test(cb3.toLowerCase()))

                $('#cbchkPackage').prop("checked", (/true/i).test(cb4.toLowerCase()))

                $('#cbchkShiphome').prop("checked", (/true/i).test(cb5.toLowerCase()))

                $('#cbchkIsGiaohang').prop("checked", (/true/i).test(cb6.toLowerCase()))

                $('#cbchkIsInsurrance').prop("checked", (/true/i).test(cb7.toLowerCase()))
                debugger
                $('#cbchkOrderDone').prop("checked", (/true/i).test(cb8.toLowerCase()))

                $('#cbchkOrderPrice').prop("checked", (/true/i).test(cb9.toLowerCase()))

                $('#cbchkSpecial1').prop("checked", (/true/i).test(cb10.toLowerCase()))

                $('#cbchkSpecial2').prop("checked", (/true/i).test(cb11.toLowerCase()))

                $('.mvc-list .table').on('click', '.select-trigger', function () {
                    var content = $(this).parent().find('.dropdown-content');
                    var dropDownTop = $(this).offset().top + $(this).outerHeight();
                    content.css('top', dropDownTop + 'px');
                    content.css('left', $(this).offset().left + 'px');
                });
                $('.mvc-list .add-product').on('click', function () {
                    debugger
                    var tableHTML = $('.mvc-list table .list-product');
                   <%-- var transactionCodeMainOrderCodeHTML = $("#<%=ddlMainOrderCode.ClientID%>").html().replace('selected', '');--%>
                    var transactionCodeMainOrderCodeHTML = $("#<%=ddlMainOrderCode.ClientID%>").html();
                    console.log(transactionCodeMainOrderCodeHTML);
                    var html = ` <tr class="ordercode order-versionnew" data-packageID="0">
                              <td><input class="transactionCode" type="text" value=""></td>
                              <td><input class="transactionWeight" onkeyup="returnWeightFee()" data-type="text" type="text" value="0"></td>
                              <td><input   data-type="text" type="text" value="0"></td>`;

                    html += `<td>
                            <div class="input-field">
                                 <select class="transactionCodeMainOrderCode"">`+ transactionCodeMainOrderCodeHTML + `;
                                </select>
                              </div>
                               </td>`;
                    html += `<td>
                              <div class="input-field">
                                 <select class="transactionCodeStatus">
                                    <option value="1">Chưa về kho TQ</option>
                                    <option value="2">Đã về kho TQ</option>
                                    <option value="3">Đã về kho đích</option>
                                    <option value="4">Đã giao khách hàng</option>
                                    <option value="0">Đã hủy</option>
                                 </select>
                              </div>
                              </td>

                              <td><input class="transactionDescription" type="text" value=""></td>
                              </td>
                              <td class="">
                              <!-- Dropdown Trigger -->
                              <a href='javascript:;' onclick="deleteOrderCode($(this))" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class="material-icons valign-center">remove_circle</i></a>
                              
                              </td>
                           </tr>`;
                    tableHTML.append(html);

                    //$('.tooltipped')
                    //    .tooltip({
                    //        trigger: 'manual'
                    //    })
                    //    .tooltip('show');

                    $('select').formSelect();
                });

                $('.mvc-list').on('click', '.remove-product', function () {
                    $(this).parent().parent().fadeOut(function () {
                        $(this).remove();
                        returnWeightFee();
                    });
                });


                //Add fee support
                $('.fsupport-list .add-product').on('click', function () {
                    var tableHTML = $('.fsupport-list table .list-product');
                    var html = ` <tr class="feesupport fee-versionnew" data-FeeSupportID="0">
                              <td><input class="feesupportname" type="text" value=""></td>
                              <td><input class="feesupportvnd" type="text" value="0"></td>`;
                    html += `                              <td class="">
                              <!-- Dropdown Trigger -->
                              <a href='javascript:;' onclick="deleteSupportFee($(this))" class=\"tooltipped\" data-position=\"top\" data-tooltip=\"Xóa\"><i class="material-icons valign-center">remove_circle</i></a>
                              
                              </td>
                           </tr>`;
                    tableHTML.append(html);
                    $('.tooltipped')
                        .tooltip({
                            trigger: 'manual'
                        })
                        .tooltip('show');
                });



                // Pefectscrollbar for sidebar and chat area

                if ($(".customer-chat .chat-area").length > 0) {
                    var ps_chat_area = new PerfectScrollbar('.customer-chat .chat-area', {
                        theme: "dark"
                    });

                }

                if ($(".local-chat .chat-area").length > 0) {
                    var ps_chat_area = new PerfectScrollbar('.local-chat .chat-area', {
                        theme: "dark"
                    });

                }

                if ($(".inside-chat .chat-area").length > 0) {
                    var ps_chat_area = new PerfectScrollbar('.inside-chat .chat-area', {
                        theme: "dark"
                    });

                }
                $('.upload-file-chat').on('click', function () {
                    $(this).siblings('.upload-img').trigger('click');
                });

                $('.dou-chat .title-header').on('click', function () {
                    $(this).closest('.chat-fixed').toggleClass('hidden');
                });


                $(window).scroll(function () {
                    var id = $('.table-of-contents li a.active').attr('href');

                    $('.scrollspy').each(function () {
                        var itemId = $(this).attr('id');
                        if (('#' + itemId) == id) {
                            $(this).parent().css({
                                'box-shadow': '0 8px 17px 2px rgba(0, 0, 0, 0.14), 0 3px 14px 2px rgba(0, 0, 0, 0.12), 0 5px 5px -3px rgba(0, 0, 0, 0.2)',
                                'border': '1px solid #000'
                            });
                            $('.scrollspy').not(this).parent().css({
                                'box-shadow': 'rgba(0, 0, 0, 0.14) 0px 2px 2px 0px, rgba(0, 0, 0, 0.12) 0px 3px 1px -2px, rgba(0, 0, 0, 0.2) 0px 1px 5px 0px',
                                'border': '0'
                            });
                        }

                    });

                });
                $('#edit-status').on('click', function () {
                    $(this).parent().addClass('show');
                });

            });

        </script>
    </telerik:RadScriptBlock>
</asp:Content>
