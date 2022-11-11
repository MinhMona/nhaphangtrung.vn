<%@ Page Title="Tổng tiền giao dịch" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="Report-User-Transaction.aspx.cs" Inherits="NHST.manager.Report_User_Transaction" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<%@ Import Namespace="MB.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Thống kê khách hàng</h4>
                    <%--<div class="right-action" style="display:none;">
                        <a href="#" class="btn" id="filter-btn">Bộ lọc</a>
                    </div>
                    <div class="clearfix"></div>
                    <div class="filter-wrap">
                        <div class="row mt-2 pt-2">
                            <div class="input-field col s12 l4" >
                                <asp:TextBox runat="server" ID="search_name" type="text" class="validate autocomplete"></asp:TextBox>
                                <label for="search_name">Username</label>
                            </div>

                            <div class="input-field col s6 l4">
                                <asp:TextBox ID="rFD" runat="server" placeholder="" Type="text" class="datetimepicker from-date"></asp:TextBox>
                                <label>Từ ngày</label>
                            </div>
                            <div class="input-field col s6 l4">
                                <asp:TextBox runat="server" Type="text" placeholder="" ID="rTD" class="datetimepicker to-date"></asp:TextBox>
                                <label>Đến ngày</label>
                                <span class="helper-text" data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                            </div>
                            <div class="col s12 right-align">
                                <asp:Button runat="server" class="btn" ID="search" OnClick="search_Click" Text="Lọc kết quả"></asp:Button>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>
            <div class="list-commission col s12 section">
                <div class="list-table card-panel">
                    <div class="table-info row center-align-xs">
                        <div class="search-name input-field">
                            <asp:TextBox ID="search_name" placeholder="" name="txtSearchName" type="text" onkeypress="myFunction()" runat="server" />
                            <label for="search_name"><span>Username khách hàng</span></label>
                            <span class="material-icons search-action">search</span>
                        </div>
                        <%--<div class="checkout col s12 m6">
                            <div class="row-p">
                                <div class="cot-p">
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng giá trị đơn hàng: 
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTonggiatridonhang"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng tiền hàng: 
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTongtienhang"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng phí đơn hàng: 
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTongphidonhang"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="cot-p">
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng phí mua hàng:
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTongphimuahang"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng vận chuyển TQ-VN: 
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTongvanchuyenqt"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng vận chuyển nội địa:
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTongvanchuyennoidia"></asp:Literal>
                                        </div>
                                    </div>
                                </div>

                                <div class="cot-p">
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng mặc cả: 
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTongmacca"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng cân nặng:
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTongcannang"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng số đơn hàng: 
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTongsodonhang"></asp:Literal>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>--%>
                    </div>

                    <div class="responsive-tb mt-2">
                        <a href="javascript:;" class="btn" style="margin-bottom: 10px" id="btnExport">Xuất thống kê</a>
                        <table class="table bordered highlight  ">
                            <thead>
                                <tr>
                                    <th style="min-width: 100px;">ID</th>
                                    <th style="min-width: 100px;">UserName</th>
                                    <th style="min-width: 100px;">Họ Tên</th>
                                    <th style="min-width: 100px;">Số điện thoại</th>
                                    <th style="min-width: 100px;">Tiền nạp</th>
                                    <th style="min-width: 100px;">Tiền hoàn trả</th>
                                    <th style="min-width: 100px;">Tiền tiêu</th>
                                    <th style="min-width: 100px;">Tiền rút</th>
                                    <th style="min-width: 100px;">Tiền hiện còn</th>
                                    <%--<th style="min-width: 100px;">Số đơn hàng</th>
                                    <th style="min-width: 100px;">Số khách hàng</th>--%>
                                   <%--  <th style="min-width: 100px;">Xem khách hàng</th>--%>
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
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="buttonExport" runat="server" OnClick="btnExport_Click" />
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
    <script>

        $('#btnExport').click(function () {
            $(<%=buttonExport.ClientID%>).click();
        });

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

        function myFunction() {
            if (event.which == 13 || event.keyCode == 13) {
                console.log($('#<%=search_name.ClientID%>').val());
                $('#<%=btnSearch.ClientID%>').click();
            }
        }
        $('.search-action').click(function () {
            console.log('dkm ngon');
            console.log($('#<%=search_name.ClientID%>').val());
            $('#<%=btnSearch.ClientID%>').click();
        })
    </script>

    <style>
        .row-p {
            display: flex;
            flex-wrap: wrap;
            margin: 0 -15px;
            justify-content: space-around;
        }

        @media screen and (max-width: 768px ) {
            .row-p {
                justify-content: space-between;
            }
        }

        .col .m6 {
            width: 100% !important;
        }

        .cot-p {
            width: calc( 3 / 12 * 100%);
            padding: 0 15px;
            margin-bottom: 30px !important;
        }

        .cot-p {
            margin-bottom: 30px;
        }

        .list-commission .checkout p {
            margin-bottom: 5px;
            font-size: 20px;
        }

            .list-commission .checkout p span {
                font-weight: bold;
                font-size: 16px;
                margin-right: 30px !important;
            }

        .cot-ngang {
            display: flex;
            justify-content: space-between;
        }

            .cot-ngang .text-bold {
                font-weight: bold;
                font-size: 16px;
            }

            .cot-ngang .price {
                font-size: 18px
            }
    </style>
</asp:Content>
