<%@ Page Language="C#" Title="Cấu hình phí đơn hàng" AutoEventWireup="true" MasterPageFile="~/manager/adminMasterNew.Master" CodeBehind="FeeOrder.aspx.cs" Inherits="NHST.manager.FeeOrder" %>

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
                    <h4 class="title no-margin" style="display: inline-block;">CẤU HÌNH PHÍ ĐƠN HÀNG NHẬP HÀNG TRUNG</h4>
                </div>
            </div>
            <div class="col s12 m12 l8 section">
                <div class="list-table card-panel">
                    <div class="responsive-tb">
                        <table class="table highlight   bordered">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Tài khoản</th>
                                    <th>Phí mua hàng (%)</th>
                                    <th>Phần trăm cọc (%)</th>
                                    <th>Hà Nội (đ/kg)</th>
                                    <th>TP.HCM (đ/kg)</th>
                                    <th>Thao tác</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal ID="ltr" runat="server" EnableViewState="false"></asp:Literal>
                            </tbody>
                        </table>
                        <span style="font-weight : bold; font-size: 18px; color:#366136;">Lưu ý: Nếu đơn hàng có tiền hàng trên web trên 10.000.000 (VNĐ) tương ứng cấp Kim Cương</span>
                    </div>
                    <div class="pagi-table float-right mt-2">
                        <%this.DisplayHtmlStringPaging1();%>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>

    </div>
    <div id="modalEditFee" class="modal modal-fixed-footer">
        <div class="modal-hd">
            <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
            <h4 class="no-margin center-align">CHỈNH SỬA PHÍ ĐƠN HÀNG#<asp:Label runat="server" ID="lbID"></asp:Label></h4>
        </div>
        <div class="modal-bd">
            <div class="row">
                <div class="input-field col s12 m12">
                    <asp:TextBox runat="server" ID="txtLevelName" Enabled="false" class="validate"></asp:TextBox>                      
                </div>
                <div class="input-field col s12 m6">
                    <asp:TextBox value="0" runat="server" ID="txtFeeService" type="number"></asp:TextBox>
                    <label class="active" for="txtFeeService">Phần trăm phí mua hàng</label>                   
                </div>
                <div class="input-field col s12 m6">
                    <asp:TextBox value="0" runat="server" ID="txtFeeDeposit" type="number"></asp:TextBox>
                    <label class="active" for="txtFeeDeposit">Phần trăm đặt cọc</label>                   
                </div>
                <div class="input-field col s12 m6">
                    <asp:TextBox value="0" runat="server" ID="txtFeeWeightHN" type="number" class="validate"></asp:TextBox>
                    <label class="active" for="txtFeeWeightHN">Phí vận chuyển kho HN</label>                   
                </div>
                <div class="input-field col s12 m6">
                    <asp:TextBox value="0" runat="server" ID="txtFeeWeightSG" type="number" class="validate"></asp:TextBox>
                    <label class="active" for="txtFeeWeightSG">Phí vận chuyển kho SG</label>                   
                </div>               
            </div>
        </div>
        <div class="modal-ft">
            <div class="ft-wrap center-align">
                <asp:Button runat="server" ID="btnUpdate" OnClick="btnUpdate_Click" UseSubmitBehavior="false" class=" btn white-text mr-2" Text="Cập nhật"></asp:Button>
                <a href="#!" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfID" runat="server" />
    <script>
        function EditFunction(ID) {
            $.ajax({
                type: "POST",
                url: "/manager/FeeOrder.aspx/loadinfo",
                data: '{ID: "' + ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data != null) {
                        $('#<%=hdfID.ClientID%>').val(ID);
                        $('#<%=lbID.ClientID%>').text(ID);                        
                        $('#<%=txtLevelName.ClientID%>').val(data.LevelName);
                        $('#<%=txtFeeService.ClientID%>').val(data.PercentService);
                        $('#<%=txtFeeDeposit.ClientID%>').val(data.PercentDeposit);
                        $('#<%=txtFeeWeightHN.ClientID%>').val(data.FeeWeightHN);
                        $('#<%=txtFeeWeightSG.ClientID%>').val(data.FeeWeightSG);                      
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
            height: auto;
        }
    </style>
</asp:Content>
