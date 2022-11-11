<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSForward.aspx.cs" MasterPageFile="~/manager/adminMasterNew.Master" Inherits="NHST.manager.SMSForward" %>

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
                    <h4 class="title no-margin" style="display: inline-block;">Truy vấn nạp tiền tự động</h4>
                    <div class="right-action">
                        <a href="#" class="btn" id="filter-btn">Bộ lọc</a>
                    </div>
                    <div class="clearfix"></div>
                    <div class="filter-wrap">
                        <div class="row mt-2 pt-2">
                            <div class="search-name input-field col s12 m6">
                                <asp:TextBox ID="txtSearchName" name="txtSearchName" type="text" onkeypress="myFunction()" runat="server" />
                                <label for="search_name"><span>Username</span></label>
                            </div>
                            <div class="input-field col s12 m6">
                                <asp:ListBox runat="server" placeholder="" ID="ddlStatus">
                                    <asp:ListItem Value="0" Text="Tất cả" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Chờ duyệt"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Đã xử lý"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Hủy"></asp:ListItem>
                                </asp:ListBox>
                                <label>Trạng thái</label>
                            </div>
                            <div class="input-field col s6 l6">
                                <asp:TextBox runat="server" ID="rFD" type="text" class="datetimepicker from-date"></asp:TextBox>
                                <label>Từ ngày</label>
                            </div>
                            <div class="input-field col s6 l6">
                                <asp:TextBox runat="server" ID="rTD" type="text" class="datetimepicker to-date"></asp:TextBox>
                                <label>Đến ngày</label>
                                <span class="helper-text" data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                            </div>
                            <div class="col s12 right-align">
                                <span class="search-action btn">Lọc kết quả</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="list-donate-money col s12 section">
                <div class="list-table card-panel">
                    <div class="responsive-tb mt-2">
                        <table class="table responsive-table  bordered highlight">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Ngân hàng</th>
                                    <th>Username</th>
                                    <th>Số tiền</th>
                                    <th>Thời gian nhận tiền</th>
                                    <th>Trạng thái</th>
                                    <th>Nội dung</th>
                                    <th>Thời gian hệ thống</th>
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
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <!-- Edit mode -->
        <div class="detail-fixed  col s12 m5 l5 xl4 section" id="draw-detail">
            <div class="rp-detail card-panel row">
                <div class="col s12">
                    <div class="page-title">
                        <h5>Thông tin nạp tiền #<asp:Label runat="server" ID="lbID"></asp:Label></h5>
                        <a href="#!" class="close-editmode top-right valign-wrapper"><i
                            class="material-icons">close</i>Close</a>
                    </div>
                </div>
                <div class="col s12">
                    <div class="row pb-2 border-bottom-1 ">
                        <div class="input-field col s12">
                            <asp:TextBox runat="server" placeholder="" ID="BankName" type="text" class="validate" Enabled="false"></asp:TextBox>
                            <label for="rp_username">Tên ngân hàng</label>
                        </div>
                        <div class="input-field col s12">
                            <asp:TextBox runat="server" placeholder="" ID="Amount" type="text" class="validate" Enabled="false"></asp:TextBox>
                            <label for="rp_vnd">Số tiền (VNĐ)</label>
                        </div>
                        <div class="input-field col s12">
                            <asp:TextBox runat="server" placeholder="" TextMode="MultiLine" Enabled="false" ID="pContent" class="materialize-textarea">
                            </asp:TextBox>
                            <label for="rp_textarea">Nội dung</label>
                        </div>
                        <div class="input-field col s12">
                            <asp:TextBox runat="server" placeholder="" ID="pUsername" type="text" class="validate"></asp:TextBox>
                            <label for="rp_vnd">Username</label>
                        </div>
                    </div>
                    <div class="row section mt-2">
                        <div class="col s12">
                            <a href="javascript:;" onclick="btnSave()" class="btn">Duyệt</a>
                            <a href="#" class="btn close-editmode">Trở về</a>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <!-- END : Edit mode -->
    </div>
    <asp:HiddenField runat="server" ID="hdfIDWR" />
    <asp:Button ID="btnSaveEdit" runat="server" OnClick="btncreateuser_Click" Style="display: none" UseSubmitBehavior="false" />
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
    <script type="text/javascript">           
        function btnSave() {
            $('#<%=btnSaveEdit.ClientID%>').click();
        }
        $('.search-action').click(function () {
            $('#<%=btnSearch.ClientID%>').click();
        })
        function addCodeTemp(ID) {
            $.ajax({
                type: "POST",
                url: "/manager/SMSForward.aspx/GetData",
                data: '{ID: "' + ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data != null) {
                        $('#<%=lbID.ClientID%>').text(ID);
                        $('#<%=hdfIDWR.ClientID%>').val(ID);
                        $('#<%=BankName.ClientID%>').val(data.ten_bank);
                        $('#<%=Amount.ClientID%>').val(data.so_tien);
                        $('#<%=pContent.ClientID%>').val(data.noi_dung);
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

        function CancelNaptien(ID) {
            var c = confirm("Bạn muốn hủy yêu cầu?");
            if (c) {
                debugger;
                $.ajax({
                    type: "POST",
                    url: "/manager/SMSForward.aspx/Cancel1",
                    data: "{ID:'" + ID + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var ret = msg.d;
                        if (ret != "none") {
                            if (ret == "notrightSMS") {
                                alert('Mã đơn không đúng, vui lòng kiểm tra lại');
                            }
                            else {
                                alert('Hủy thành công');
                                location.reload();
                            }
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        alert(errorthrow);
                    }
                });
            }
        }

    </script>
</asp:Content>
