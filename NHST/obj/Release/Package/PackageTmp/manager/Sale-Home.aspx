<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sale-Home.aspx.cs" MasterPageFile="~/manager/adminMasterNew.Master" Inherits="NHST.manager.Sale_Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/AdminNew45/assets/vendors/chartist-js/chartist.min.css">
    <link rel="stylesheet" type="text/css" href="/App_Themes/AdminNew45/assets/vendors/chartist-js/chartist-plugin-tooltip.css">
    <link rel="stylesheet" type="text/css" href="/App_Themes/AdminNew45/assets/css/pages/dashboard-modern.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="col s12">
                <div class="container">
                    <div class="row mt-4">
                        <div class="col s12">
                            <!-- Tổng đơn hàng trong tuần -->
                            <div class="card user-statistics-card animate fadeLeft">
                                <div class="card-content">
                                    <h4 class="card-title mb-0">Số lượng đơn hàng trong tháng: <span style="color: blue; font-size: 20px; font-weight: bold;">
                                        <asp:Literal runat="server" ID="ltrTotalOrder"></asp:Literal>
                                        Đơn</span></h4>

                                    <h4 class="card-title mb-0">Số lượng đơn hàng mới trong tháng: <span style="color: blue; font-size: 20px; font-weight: bold;">
                                        <asp:Literal runat="server" ID="ltrTotalOrderStatus0"></asp:Literal>
                                        Đơn</span></h4>
                                    <div class="row">
                                        <div class="col s12 m3">
                                            <ul class="collection border-none mb-0">
                                                <li class="collection-item avatar">
                                                    <i class="material-icons circle pink accent-2">library_books</i>
                                                    <p class="medium-small">Mua hàng hộ</p>
                                                    <h5 class="mt-0 mb-0 total-muahangho">
                                                        <asp:Literal runat="server" ID="ltrOrder1"></asp:Literal></h5>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="col s12 m3">
                                            <ul class="collection border-none mb-0">
                                                <li class="collection-item avatar">
                                                    <i class="material-icons circle red accent-2" style="background-color: #d17905 !important;">library_books</i>
                                                    <p class="medium-small">Mua hàng hộ khác</p>
                                                    <h5 class="mt-0 mb-0 total-muahanghokhac">
                                                        <asp:Literal runat="server" ID="ltrOrder3"></asp:Literal></h5>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="col s12 m3">
                                            <ul class="collection border-none mb-0">
                                                <li class="collection-item avatar">
                                                    <i class="material-icons circle pink accent-2">library_books</i>
                                                    <p class="medium-small">Mua hàng hộ đơn mới</p>
                                                    <h5 class="mt-0 mb-0 total-muahangho">
                                                        <asp:Literal runat="server" ID="ltrOrder1Satus0"></asp:Literal></h5>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="col s12 m3">
                                            <ul class="collection border-none mb-0">
                                                <li class="collection-item avatar">
                                                    <i class="material-icons circle red accent-2" style="background-color: #d17905 !important;">library_books</i>
                                                    <p class="medium-small">Mua hàng hộ khác đơn mới</p>
                                                    <h5 class="mt-0 mb-0 total-muahanghokhac">
                                                        <asp:Literal runat="server" ID="ltrOrder3Satus0"></asp:Literal></h5>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <%-- <div class="user-statistics-container mt-2">
                                        <div id="user-statistics-bar-chart" class="user-statistics-shadow"></div>
                                    </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col s12 m12 l6">
                            <div class="card-panel animate fadeRight">
                                <div class="card-content pb-1">
                                    <h6 class="card-title mb-2 font-weight-400">Tổng doanh thu trong tháng của bạn: <span style="color: blue; font-size: 20px; font-weight: bold;">
                                        <asp:Literal runat="server" ID="ltrTotalPrice"></asp:Literal></span></h6>
                                    <hr />
                                </div>
                                <div class="responsive-tb">
                                    <table class=" table highlight striped">
                                        <thead>
                                            <tr>
                                                <th>STT</th>
                                                <th>Username</th>
                                                <th>Doanh thu</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Literal runat="server" ID="ltrListTotalPriceSale"></asp:Literal>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div class="col s12 m12 l6">
                            <div class="card-panel animate fadeRight">
                                <div class="card-content pb-1">
                                    <h6 class="card-title mb-2 font-weight-400">Tổng doanh thu khách hàng từ 0 - 60 ngày: <span style="color: blue; font-size: 20px; font-weight: bold;">
                                        <asp:Literal runat="server" ID="ltrTotalPrice0"></asp:Literal></span></h6>
                                    <hr />
                                </div>
                                <div class="card-content pb-1">
                                    <h6 class="card-title mb-2 font-weight-400">Tổng doanh thu khách hàng từ 61 - 240 ngày: <span style="color: blue; font-size: 20px; font-weight: bold;">
                                        <asp:Literal runat="server" ID="ltrTotalPrice60"></asp:Literal></span></h6>
                                    <hr />
                                </div>
                                <div class="card-content pb-1">
                                    <h6 class="card-title mb-2 font-weight-400">Tổng doanh thu khách hàng trên 240 ngày: <span style="color: blue; font-size: 20px; font-weight: bold;">
                                        <asp:Literal runat="server" ID="ltrTotalPrice240"></asp:Literal></span></h6>
                                    <hr />
                                </div>
                                <div class="card-content pb-1">
                                    <h6 class="card-title mb-2 font-weight-400">Top 10 khách hàng của bạn có nhiều đơn hàng trong tháng</h6>
                                    <hr />
                                </div>
                                <div class="responsive-tb">
                                    <table class="  table    highlight striped">
                                        <thead>
                                            <tr>
                                                <th>STT</th>
                                                <th>Username</th>
                                                <th>Số dư</th>
                                                <th>Tổng đơn</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Literal runat="server" ID="ltrTop10UserHasAlotOrder"></asp:Literal>
                                        </tbody>
                                    </table>
                                </div>

                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col s12 m12 l12">
                            <div class="card-panel animate fadeRight">
                                <div class="card-content pb-1">
                                    <h6 class="card-title mb-2 font-weight-400">Top 10 khách hàng của bạn mới nạp tiền</h6>
                                    <hr />
                                </div>
                                <div class="responsive-tb">
                                    <table class=" table highlight striped">
                                        <thead>
                                            <tr>
                                                <th>STT</th>
                                                <th>Username</th>
                                                <th>Số tiền nạp</th>
                                                <th>Ngày giờ</th>
                                                <th>Trạng thái</th>                                               
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Literal runat="server" ID="ltrUserAddNewWallet"></asp:Literal>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdfTotalOrderWeek" />
    <asp:HiddenField runat="server" ID="hdfTotalWalletInWeek" />
    <asp:HiddenField runat="server" ID="hdfRationBuyPro" />
    <script>
        // Dashboard - Modern
        //----------------------
        window.onload = function () {
            (function (window, document, $) {
                //format Money
                function formatMoney(
                    amount,
                    decimalCount = 2,
                    decimal = ".",
                    thousands = ","
                ) {
                    try {
                        decimalCount = Math.abs(decimalCount);
                        decimalCount = isNaN(decimalCount) ? 2 : decimalCount;

                        const negativeSign = amount < 0 ? "-" : "";

                        let i = parseInt(
                            (amount = Math.abs(Number(amount) || 0).toFixed(decimalCount))
                        ).toString();
                        let j = i.length > 3 ? i.length % 3 : 0;

                        return (
                            negativeSign +
                            (j ? i.substr(0, j) + thousands : "") +
                            i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands)
                        );
                    } catch (e) {
                        console.log(e);
                    }
                }

                // Tỉ lệ đơn hàng mua hộ
                // -----------
                var sum = function (a, b) {
                    return a + b;
                };
                var options = {
                    labelInterpolationFnc: function (value, idx) {
                        return value[0];
                    },
                    labelPosition: "outside",
                    plugins: [
                        Chartist.plugins.tooltip({
                            tooltipFnc: function (meta, value) {
                                return value + " Đơn hàng";
                            },
                            appendToBody: true,
                            class: "pie-tooltip"
                        })
                    ],
                    labelOffset: 10,
                    labelDirection: "explode",
                    labelInterpolationFnc: function (value, idx) {
                        var datas = MuaHangHoChartPie.data.series;
                        function getSum(total, num) {
                            return total + num;
                        }
                        var percentage =
                            (datas[idx] / datas.reduce(getSum) * 100).toFixed(2) + "%";
                        return value + " / " + percentage;
                    },
                    chartPadding: 35
                };

                var responsiveOptions = [
                    [
                        "screen and (min-width: 1200px)",
                        {
                            labelOffset: 10,
                            labelDirection: "explode",
                            labelInterpolationFnc: function (value, idx) {
                                var datas = MuaHangHoChartPie.data.series;
                                function getSum(total, num) {
                                    return total + num;
                                }
                                var percentage =
                                    (datas[idx] / datas.reduce(getSum) * 100).toFixed(2) + "%";
                                return value + " / " + percentage;
                            },
                            chartPadding: 35
                        }
                    ],
                    [
                        "screen and (max-width: 480px)",
                        {
                            labelOffset: 10,
                            labelDirection: "explode",
                            labelInterpolationFnc: function (value, idx) {
                                var datas = MuaHangHoChartPie.data.series;
                                function getSum(total, num) {
                                    return total + num;
                                }
                                var percentage =
                                    Math.round((datas[idx] / datas.reduce(getSum)) * 100) + "%";
                                var valueSub = value.substring(0, value.indexOf(" "));
                                console.log(valueSub);
                                return value[0] + value[1] + value[2] + " / " + percentage;
                            },
                            chartPadding: 60
                        }
                    ]
                ];
                var labelsRatioBuyPro = JSON.parse($('#<%=hdfRationBuyPro.ClientID%>').val()).label;
                var seriesRatioBuyPro = JSON.parse($('#<%=hdfRationBuyPro.ClientID%>').val()).data;
                var MuaHangHoChartPie = new Chartist.Pie(
                    "#muahangho-ratio-pie-chart",
                    {
                        labels: labelsRatioBuyPro,
                        series: seriesRatioBuyPro
                    },
                    options,
                    responsiveOptions
                );

                var array;
                var array1 = $('#<%=hdfTotalOrderWeek.ClientID%>').val();
                console.log(array1);
                if (array1 != null || array1 != "") {
                    var array2 = JSON.parse($('#<%=hdfTotalOrderWeek.ClientID%>').val());
                    console.log(array2.data);
                    array = array2.data;

                }


                var TotalOrderWeekBarChart = new Chartist.Bar(
                    "#user-statistics-bar-chart",
                    {
                        labels: [
                            "Monday",
                            "Tuesday",
                            "Wednesday",
                            "Thursday",
                            "Friday",
                            "Saturday",
                            "Sunday"
                        ],
                        series: array
                    },
                    {
                        // Default mobile configuration
                        stackBars: true,
                        chartPadding: 0,
                        axisX: {
                            showGrid: false
                        },
                        axisY: {
                            showGrid: true,
                            labelInterpolationFnc: function (value) {
                                return value;
                            },
                            scaleMinSpace: 20
                        },
                        plugins: [
                            Chartist.plugins.tooltip({
                                class: "user-statistics-tooltip",
                                appendToBody: true
                            })
                        ]
                    },
                    [
                        // Options override for media > 800px
                        [
                            "screen and (min-width: 800px)",
                            {
                                stackBars: false,
                                seriesBarDistance: 10
                            }
                        ],
                        // Options override for media > 1000px
                        [
                            "screen and (min-width: 1000px)",
                            {
                                reverseData: false,
                                horizontalBars: false,
                                seriesBarDistance: 15
                            }
                        ]
                    ]
                );

                TotalOrderWeekBarChart.on("draw", function (data) {
                    var dataMHH = TotalOrderWeekBarChart.data.series[0];
                    var dataVCH = TotalOrderWeekBarChart.data.series[1];
                    var dataTTH = TotalOrderWeekBarChart.data.series[2];
                    var dataMHHK = TotalOrderWeekBarChart.data.series[3];
                    var totalMHH = dataMHH.reduce((average, data, index, dataMHH) => {
                        return (average += data);
                    }, 0);
                    var totalMHHK = dataMHHK.reduce((average, data, index, dataMHHK) => {
                        return (average += data);
                    }, 0);
                    var totalVCH = dataVCH.reduce((average, data, index, dataVCH) => {
                        return (average += data);
                    }, 0);
                    var totalTTH = dataTTH.reduce((average, data, index, dataTTH) => {
                        return (average += data);
                    }, 0);
                    $(".total-muahangho").text(Math.floor(totalMHH));
                    $(".total-vanchuyenho").text(Math.floor(totalVCH));
                    $(".total-thanhtoanho").text(Math.floor(totalTTH));
                    $(".total-muahanghokhac").text(Math.floor(totalMHHK));
                    if (data.type === "bar") {
                        data.element.attr({
                            style: "stroke-width: 16px",
                            x1: data.x1 + 0.001
                        });

                        data.element.animate({
                            y2: {
                                begin: 500,
                                dur: 500,
                                from: data.y1,
                                to: data.y2
                            }
                        });
                    }
                });

                TotalOrderWeekBarChart.on("created", function (data) {
                    var defs = data.svg.querySelector("defs") || data.svg.elem("defs");
                    defs
                        .elem("linearGradient", {
                            id: "barGradient1",
                            x1: 0,
                            y1: 0,
                            x2: 0,
                            y2: 1
                        })
                        .elem("stop", {
                            offset: 0,
                            "stop-color": "rgba(255,75,172,1)"
                        })
                        .parent()
                        .elem("stop", {
                            offset: 1,
                            "stop-color": "rgba(255,75,172, 0.6)"
                        });

                    defs
                        .elem("linearGradient", {
                            id: "barGradient2",
                            x1: 0,
                            y1: 0,
                            x2: 0,
                            y2: 1
                        })
                        .elem("stop", {
                            offset: 0,
                            "stop-color": "rgba(129,51,255,1)"
                        })
                        .parent()
                        .elem("stop", {
                            offset: 1,
                            "stop-color": "rgba(129,51,255, 0.6)"
                        });
                    return defs;
                });

             <%--   var ArrayGuestDonate = JSON.parse($('#<%=hdfTotalOrderWeek.ClientID%>').val()).data;
                var TotalGuestDonate = new Chartist.Bar(
                    "#total-guest-donate-bar-chart",
                    {
                        labels: [
                            "Monday",
                            "Tuesday",
                            "Wednesday",
                            "Thursday",
                            "Friday",
                            "Saturday",
                            "Sunday"
                        ],
                        series: [ArrayGuestDonate]
                    },--%>

                var ArrayGuestDonate = JSON.parse($('#<%=hdfTotalWalletInWeek.ClientID%>').val()).data;
                console.log(ArrayGuestDonate);
                //Tổng tiền khách nạp  trong tuần
                var TotalGuestDonate = new Chartist.Bar(
                    "#total-guest-donate-bar-chart",
                    {
                        labels: [
                            "Monday",
                            "Tuesday",
                            "Wednesday",
                            "Thursday",
                            "Friday",
                            "Saturday",
                            "Sunday"
                        ],
                        series: [ArrayGuestDonate]
                    },
                    {
                        // Default mobile configuration
                        stackBars: true,
                        chartPadding: 15,
                        axisX: {
                            showGrid: false
                        },
                        axisY: {
                            showGrid: true,
                            labelInterpolationFnc: function (value) {
                                return value;
                            },
                            scaleMinSpace: 10,
                            onlyInteger: true
                        },
                        plugins: [
                            Chartist.plugins.tooltip({
                                class: "user-statistics-tooltip",
                                appendToBody: true
                            })
                        ]
                    },
                    [
                        // Options override for media > 800px
                        [
                            "screen and (min-width: 800px)",
                            {
                                stackBars: false,
                                seriesBarDistance: 10
                            }
                        ],
                        // Options override for media > 1000px
                        [
                            "screen and (min-width: 1000px)",
                            {
                                reverseData: false,
                                horizontalBars: false,
                                seriesBarDistance: 15
                            }
                        ]
                    ]
                );

                TotalGuestDonate.on("draw", function (data) {
                    var dataDonate = TotalGuestDonate.data.series[0];
                    var totalDonate = dataDonate.reduce((average, data, index, dataDonate) => {
                        return (average += data);
                    }, 0);
                    $(".total-guest-donate").text(formatMoney(Math.floor(totalDonate)) + " đ");
                    if (data.type === "bar") {
                        data.element.attr({
                            style: "stroke-width: 2rem",
                            x1: data.x1 + 0.001
                        });
                        data.element.animate({
                            y2: {
                                begin: 500,
                                dur: 500,
                                from: data.y1,
                                to: data.y2
                            }
                        });
                    }
                });

                TotalGuestDonate.on("created", function (data) {
                    var defs = data.svg.querySelector("defs") || data.svg.elem("defs");
                    defs
                        .elem("linearGradient", {
                            id: "barGradient1",
                            x1: 0,
                            y1: 0,
                            x2: 0,
                            y2: 1
                        })
                        .elem("stop", {
                            offset: 0,
                            "stop-color": "rgba(255,75,172,1)"
                        })
                        .parent()
                        .elem("stop", {
                            offset: 1,
                            "stop-color": "rgba(255,75,172, 0.6)"
                        });

                    defs
                        .elem("linearGradient", {
                            id: "barGradient2",
                            x1: 0,
                            y1: 0,
                            x2: 0,
                            y2: 1
                        })
                        .elem("stop", {
                            offset: 0,
                            "stop-color": "rgba(129,51,255,1)"
                        })
                        .parent()
                        .elem("stop", {
                            offset: 1,
                            "stop-color": "rgba(129,51,255, 0.6)"
                        });
                    return defs;
                });
            })(window, document, jQuery);
        }

    </script>
    <script src="/App_Themes/AdminNew45/assets/vendors/chartjs/chart.min.js" type="text/javascript"></script>
    <script src="/App_Themes/AdminNew45/assets/vendors/chartist-js/chartist.min.js" type="text/javascript"></script>
    <script src="/App_Themes/AdminNew45/assets/vendors/chartist-js/chartist-plugin-tooltip.js" type="text/javascript"></script>
    <script src="/App_Themes/AdminNew45/assets/vendors/chartist-js/chartist-plugin-fill-donut.min.js" type="text/javascript"></script>
    <script src="/App_Themes/AdminNew45/assets/vendors/chartist-js/chartist-plugin-legend.js" type="text/javascript"></script>
</asp:Content>
