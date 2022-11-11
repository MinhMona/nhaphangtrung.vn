<%@ Page Language="C#" MasterPageFile="~/MrPhucMaster.Master" AutoEventWireup="true" CodeBehind="Default7.aspx.cs" Inherits="NHST.Default4" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <main>
        <section class="bg-full">
            <div class="container">
                <div class="main-text wow fadeInRight" data-wow-delay="0.2s" data-wow-duration="1s">
                    <h2 class="texth1">Giao dịch Trung - Việt
                        <br />
                        Mua niềm tin - bán uy tín
                    </h2>
                    <div class="main-text-install wow fadeInRight" data-wow-delay=".4s" data-wow-duration="1s">
                        <div class="btn">
                            <a id="chrome" href="https://chrome.google.com/webstore/detail/c%C3%B4ng-c%E1%BB%A5-%C4%91%E1%BA%B7t-h%C3%A0ng-c%E1%BB%A7a-apex/oookokalellhhdeeihaahcnihepgomon/related"><i class="fa fa-chrome"></i>Chrome </a>
                            <a id="coccoc" href="https://chrome.google.com/webstore/detail/c%C3%B4ng-c%E1%BB%A5-%C4%91%E1%BA%B7t-h%C3%A0ng-c%E1%BB%A7a-apex/oookokalellhhdeeihaahcnihepgomon/related"><i class="fa fa-chrome"></i>Cốc Cốc</a>
                        </div>
                    </div>
                </div>
                <div class="main-search">
                    <div class="search wow fadeInRight" data-wow-delay=".6s" data-wow-duration="1s">
                        <div class="search-text">
                            <div class="search-maintext">
                                <h2 class="main-title-cus search-form active" data-tab="01">Nhập link sản phẩm
                                </h2>
                                <h2 class="main-title-cus tracking-form" data-tab="02">Tra mã nhận đơn</h2>
                            </div>
                            <div id="search" class="form-search f-control 01 active">
                                <div class="form-search-sel-inp">
                                    <div class="form-search-sel-inp-item">
                                        <select name="select" id="brand-source">
                                            <option value="taobao">TAOBAO</option>
                                            <option value="tmall">TMALL</option>
                                            <option value="1688">1688</option>
                                        </select>
                                        <div class="form-search-sel-inp-item-input">
                                            <%--<input type="text" placeholder="Tìm kiếm sản phẩm" class="search-product" />--%>
                                            <asp:TextBox runat="server" ID="txtSearch" CssClass="input f-control txtsearchfield" placeholder="Nhập sản phẩm cần tìm"></asp:TextBox>
                                            <i class="fa fa-times-circle"></i>
                                        </div>
                                    </div>
                                    <%-- <a href=" " class="btn-search">Tìm Kiếm</a>--%>
                                    <a href="javascript:;" onclick="searchProduct()" class="btn-search">Tìm kiếm</a>
                                    <asp:Button ID="btnSearch" runat="server"
                                        OnClick="btnSearch_Click" Style="display: none"
                                        OnClientClick="document.forms[0].target = '_blank';" UseSubmitBehavior="false" />
                                </div>
                            </div>
                            <div id="tracking" class="form-search f-control hiden 02">
                                <%-- <input type="text" name="" id="" placeholder="Tracking" class="fcontrol" />
                                <a href=" " class="btn-search">Tìm Kiếm</a>--%>
                                <input id="txtMVD" class="fcontrol" type="text" placeholder="Nhập mã vận đơn">
                                <a href="javascript:;" onclick="searchCode()" class="btn-search">Tìm kiếm</a>
                            </div>
                        </div>
                    </div>
                    <div class="tab wow fadeInRight" data-wow-delay=".8s" data-wow-duration="1s">
                        <div class="tab-order">
                            <h3 class="texth3">Quy trình đặt hàng</h3>
                            <p>
                                CÔNG CỤ ĐẶT HÀNG MUA HỘ CHUYÊN NGHIỆP
                            </p>
                        </div>
                        <div class="tab-order-hotline">
                            <h3 class="texth3">HOTLINE</h3>
                            <p class="biggertext">
                                <%--<a href="tel:09084554455"><span class="call-me">Gọi cho chúng tôi khi cần</span><br />
                                    0908 - 455 - 4455</a>--%>
                                <asp:Literal runat="server" ID="ltrHotline"></asp:Literal>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <div class="service-tab-wrapper tab-wrapper">
            <div class="tab-top">
                <div class="container">
                    <h3 class="main-title wow fadeInRight" data-wow-delay=".2s" data-wow-duration="1s">Quy trình đặt hàng</h3>
                    <ul class="service-tab-nav wow zoomIn" data-wow-delay=".4s" data-wow-duration="1s">
                        <asp:Literal ID="ltrStep1" runat="server"></asp:Literal>
                    </ul>
                </div>
            </div>
            <div class="sec-service-tab">
                <div class="container">
                    <div class="service-tab-content-wrapper wow zoomIn" data-wow-delay=".6s" data-wow-duration="1s">
                        <asp:Literal ID="ltrStep2" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <section class="sec-detail">
            <div class="table-detail">
                <div class="swiper-wrapper wow zoomIn" data-wow-delay=".4s" data-wow-duration="1s">
                    <div class="service-detail-form swiper-slide" id="1">
                        <div class="container">
                            <div class="service-detail-form-sum">
                                <div class="service-detail-form-text">
                                    <h3 class="texth3">Chi tiết dịch vụ</h3>
                                    <h2 class="texth2">Chuyển tiền, nạp tiền AlPay, Wechat</h2>
                                </div>
                                <div class="service-detail-form-text-flex">
                                    <p class="bold-text">
                                        Thay vì tới tận nơi mua hàng trực tiếp chọn lựa vất vả, giờ đây ngay tại nhà bạn cũng có thể mua sắm thả ga trực tuyến, chọn cho mình những sản phẩm phù hợp ưng ý nhất, và dùng thẻ alipay thanh toán. Việc nạp tiền alipay không những quy đổi tiền thật vào ví điện tử để thuận tiện thực hiện các giao dịch trực tuyến mà đem lại nhiều tiện ích, tiện lợi cho người dùng.
                                    </p>
                                    <div class="service-detail-form-text-flex-detail">
                                        <p>
                                            Để đáp ứng nhu cầu mua sắm trực tuyến thông qua mang Internet tại các website thương mại điện tử của Trung Quốc bằng các tài khoản alipay, ngày nay ra đời các dịch vụ nạp tiền alipay phục lợi lợi ích con người thông qua thẻ. Nếu bạn không tin, hãy cùng tôi tham khảo bài viết sau đây để nắm bắt được mọi thông tin, tiện ích của dịch vụ này nhé!
                                        </p>
                                        <a href="#">Xem chi tiết</a>
                                    </div>
                                </div>
                                <div class="main-value">
                                    <div class="value-truck-item">
                                        <div class="value-truck-item-cus">
                                            <p class="counter" data-counter="05">05</p>
                                            <p>năm kinh nghiệm</p>
                                        </div>
                                    </div>
                                    <div class="value-truck-item">
                                        <div class="value-truck-item-cus">
                                            <p class="counter" data-counter="4343">4343</p>
                                            <p>khách hàng</p>
                                        </div>
                                    </div>
                                    <div class="value-truck-item">
                                        <div class="value-truck-item-cus">
                                            <p class="counter" data-counter="85302">85302</p>
                                            <p>đơn hàng</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="img">
                                <img src="/App_Themes/MrPhuc/img/bg-detail.png" alt="" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section class="sec sec-benifit">
            <div class="benifit">
                <div class="container">
                    <div class="benifit-form">
                        <h3 class="texth3 wow fadeInRight" data-wow-delay=".2s" data-wow-duration="1s">Quyền lợi khách hàng</h3>
                        <h2 class="texth2 wow fadeInRight" data-wow-delay=".4s" data-wow-duration="1s">Khách hàng được gì khi sự dụng dịch vụ từ chúng tôi
                        </h2>
                    </div>
                    <div class="benifit-form-main">
                        <asp:Literal ID="ltrQL1" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <div class="container">
                <div class="sologan wow fadeInUp" data-wow-delay=".3s" data-wow-duration="1s">
                    <h2 class="texth2">Chính sách ưu đãi <strong>đặc biệt</strong> ngoài bảng giá, luôn
                        dành cho những ai <strong>sẵn lòng hợp tác</strong>
                    </h2>
                </div>
            </div>
        </section>
        <section>
            <div class="new-customer">
                <div class="container">
                    <div class="new-customer-form wow zoomInLeft" data-wow-delay=".3s" data-wow-duration="1s">
                        <h2 class="new-customer-form-biggertext">Dành cho khách hàng mới
                        </h2>
                        <%--<div class="new-customer-form-cus">
                            <input type="text" placeholder="Họ tên khách hàng" />
                            <input type="text" placeholder="Số điện thoại" />
                            <input type="text" placeholder="Ngành nghề" />
                            <input type="text" placeholder="Lời nhắn" />
                            <p class="text-center">
                                <a href="#" class="btn-main btn-1">Gửi tin</a>
                            </p>
                        </div>--%>
                        <div class="contact-form">
                            <div class="f-group">
                                <asp:TextBox runat="server" class="f-control" placeholder="Họ tên khách hàng" ID="txtFullNameContact" data-type="text-only" type="text"></asp:TextBox>
                                <span class="helper-text">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Contact" runat="server" ControlToValidate="txtFullNameContact"
                                        ForeColor="Red" Display="Dynamic" ErrorMessage="Không được để trống."></asp:RequiredFieldValidator>
                                </span>
                            </div>
                            <div class="f-group">
                                <asp:TextBox runat="server" class="f-control" placeholder="Số điện thoại" ID="txtPhoneContact" onkeypress='return event.charCode >= 48 && event.charCode <= 57' MaxLength="11" data-type="phone-number" type="text"></asp:TextBox>
                                <span class="helper-text">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Contact" runat="server" ControlToValidate="txtPhoneContact"
                                        ForeColor="Red" Display="Dynamic" ErrorMessage="Không được để trống."></asp:RequiredFieldValidator>
                                </span>
                            </div>
                            <div class="f-group">
                                <asp:TextBox runat="server" class="f-control" placeholder="Email" ID="txtEmailContact" type="email"></asp:TextBox>
                                <span class="helper-text">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Contact" runat="server" ControlToValidate="txtEmailContact"
                                        ForeColor="Red" Display="Dynamic" ErrorMessage="Không được để trống."></asp:RequiredFieldValidator>
                                </span>
                                <span class="helper-text">
                                    <asp:RegularExpressionValidator runat="server" ValidationGroup="Contact" ID="RegularExpressionValidator4" ForeColor="Red" Display="Dynamic" ControlToValidate="txtEmailContact"
                                        ErrorMessage="Sai định dạng Email" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" />
                                </span>
                            </div>
                            <div class="f-group">
                                <asp:TextBox runat="server" TextMode="MultiLine" class="f-control" placeholder="Lời nhắn" ID="txtNoteContact" type="text"></asp:TextBox>
                                <span class="helper-text">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="Contact" runat="server" ControlToValidate="txtNoteContact"
                                        ForeColor="Red" Display="Dynamic" ErrorMessage="Không được để trống."></asp:RequiredFieldValidator>
                                </span>
                            </div>
                            <div class="f-group text-right">
                                <asp:Button ID="BtnContact" ValidationGroup="Contact" runat="server" class="btn-1 main-btn-form" Text="Gửi tin" UseSubmitBehavior="false" OnClick="BtnContact_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="new-customer-form-text wow zoomInRight" data-wow-delay=".3s" data-wow-duration="1s">
                        <asp:Literal ID="ltrQL2" runat="server"></asp:Literal>
                        <%-- <div class="new-customer-form-text-item">
                            <h3 class="li-h3">Khách hàng order</h3>
                            <p>
                                Branding is no longer simply about visual appeal (or the
                                cherry Lorem ipsum dolor sit amet consectetur adipisicing
                                elit
                            </p>
                            <a href="">Xem chi tiết</a>
                        </div>
                        <div class="new-customer-form-text-item">
                            <h3 class="li-h3">Chú ý</h3>
                            <p>
                                Branding is no longer simply about visual appeal (or the
                                cherry Lorem ipsum dolor sit amet consectetur adipisicing
                                elit
                            </p>
                            <a href="">Xem chi tiết</a>
                        </div>
                        <div class="new-customer-form-text-item">
                            <h3 class="li-h3">Điều khoản sử dụng dịch vụ</h3>
                            <p>
                                Branding is no longer simply about visual appeal (or the
                                cherry Lorem ipsum dolor sit amet consectetur adipisicing
                                elit
                            </p>
                            <a href="">Xem chi tiết</a>
                        </div>--%>
                    </div>
                </div>
            </div>
        </section>
    </main>

    <asp:HiddenField ID="hdfWebsearch" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('.txtSearch').on("keypress", function (e) {
                if (e.keyCode == 13) {
                    searchProduct();
                }
            });
        });

        function searchProduct() {
            var web = $("#brand-source").val();
            $("#<%=hdfWebsearch.ClientID%>").val(web);
            $("#<%=btnSearch.ClientID%>").click();
        }
    </script>
    <script type="text/javascript">
        function keyclose_ms(e) {
            if (e.keyCode == 27) {
                close_popup_ms();
            }
        }
        function close_popup_ms() {
            $("#pupip_home").animate({ "opacity": 0 }, 400);
            $("#bg_popup_home").animate({ "opacity": 0 }, 400);
            setTimeout(function () {
                $("#pupip_home").remove();
                $(".zoomContainer").remove();
                $("#bg_popup_home").remove();
                $('body').css('overflow', 'auto').attr('onkeydown', '');
            }, 500);
        }
        function closeandnotshow() {
            $.ajax({
                type: "POST",
                url: "/Default.aspx/setNotshow",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    close_popup_ms();
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi');
                }
            });
        }
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                url: "/Default.aspx/getPopup",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d != "null") {
                        var data = JSON.parse(msg.d);
                        var title = data.NotiTitle;
                        var content = data.NotiContent;
                        var email = data.NotiEmail;
                        var obj = $('form');
                        $(obj).css('overflow', 'hidden');
                        $(obj).attr('onkeydown', 'keyclose_ms(event)');
                        var bg = "<div id='bg_popup_home'></div>";
                        var fr = "<div id='pupip_home' class=\"columns-container1\">" +
                            "  <div class=\"center_column col-xs-12 col-sm-5\" id=\"popup_content_home\">";
                        fr += "<div class=\"popup_header\">";
                        fr += title;
                        fr += "<a style='cursor:pointer;right:5px;' onclick='close_popup_ms()' class='close_message'></a>";
                        fr += "</div>";
                        fr += "     <div class=\"changeavatar\">";
                        fr += "         <div class=\"content1\">";
                        fr += content;
                        fr += "         </div>";
                        fr += "         <div class=\"content2\">";
                        fr += "<a href=\"javascript:;\" class=\"btn btn-close-full\" onclick='closeandnotshow()'>Đóng & không hiện thông báo</a>";
                        fr += "<a href=\"javascript:;\" class=\"btn btn-close\" onclick='close_popup_ms()'>Đóng</a>";
                        fr += "         </div>";
                        fr += "     </div>";
                        fr += "<div class=\"popup_footer\">";
                        fr += "<span class=\"float-right\">" + email + "</span>";
                        fr += "</div>";
                        fr += "   </div>";
                        fr += "</div>";
                        $(bg).appendTo($(obj)).show().animate({ "opacity": 0.7 }, 800);
                        $(fr).appendTo($(obj));
                        setTimeout(function () {
                            $('#pupip').show().animate({ "opacity": 1, "top": 20 + "%" }, 200);
                            $("#bg_popup").attr("onclick", "close_popup_ms()");
                        }, 1000);
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi');
                }
            });
        });
        function searchCode() {
            var code = $("#txtMVD").val();
            if (isEmpty(code)) {
                alert('Vui lòng nhập mã vận đơn.');
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/Default.aspx/getInfo",
                    data: "{ordecode:'" + code + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d != "null") {
                            //var data = JSON.parse(msg.d);
                            var title = "Thông tin mã vận đơn";
                            var content = msg.d;
                            var email = "";
                            var obj = $('form');
                            $(obj).css('overflow', 'hidden');
                            $(obj).attr('onkeydown', 'keyclose_ms(event)');
                            var bg = "<div id='bg_popup_home'></div>";
                            var fr = "<div id='pupip_home' class=\"columns-container1\">" +
                                "  <div class=\"center_column col-xs-12 col-sm-5\" id=\"popup_content_home\">";
                            fr += "<div class=\"popup_header\">";
                            fr += title;
                            fr += "<a style='cursor:pointer;right:5px;' onclick='close_popup_ms()' class='close_message'></a>";
                            fr += "</div>";
                            fr += "     <div class=\"changeavatar\">";
                            fr += "         <div class=\"content1\" style=\"width:75%;margin-left:11%\">";
                            fr += content;
                            fr += "         </div>";
                            fr += "         <div class=\"content2\">";
                            fr += "             <a href=\"javascript:;\" class=\"btn btn-close\" onclick='close_popup_ms()'>Đóng</a>";
                            fr += "         </div>";
                            fr += "     </div>";
                            fr += "<div class=\"popup_footer\">";
                            fr += "<span class=\"float-right\">" + email + "</span>";
                            fr += "</div>";
                            fr += "   </div>";
                            fr += "</div>";
                            $(bg).appendTo($(obj)).show().animate({ "opacity": 0.7 }, 800);
                            $(fr).appendTo($(obj));
                            setTimeout(function () {
                                $('#pupip').show().animate({ "opacity": 1, "top": 20 + "%" }, 200);
                                $("#bg_popup").attr("onclick", "close_popup_ms()");
                            }, 1000);
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                        alert('lỗi');
                    }
                });
            }

        }
    </script>


    <script>
        jQuery(document).ready(function () {
            new WOW().init();

            var iniTab = $('.list-guide-nav li.active .tabswap-btn').attr('src-navtab');
            if (!!iniTab) {
                $('.guide-ct-nav ' + iniTab).show().siblings().hide();
                $('.list-guide-nav').on('click', '.tabswap-btn', function (e) {
                    e.preventDefault();
                    var srcTab = $(this).attr('src-navtab');
                    $(this).parent().addClass('active').siblings().removeClass('active');
                    $('.guide-ct-nav ' + srcTab).fadeIn().siblings().hide();
                })
            }
            $('.banner-home').slick({
                infinite: true,
                speed: 300,
                slidesToShow: 1,
                adaptiveHeight: true,
                arrows: false
            });

        });
    </script>
    <script>
            $(".acc-info-btn").on('click', function (e) {
                e.preventDefault();
                $(".status-mobile").addClass("open");
                $(".overlay-status-mobile").show();
            });

            $(".overlay-status-mobile").on('click', function () {
                $(".status-mobile").removeClass("open");
                $(this).hide();
            });
    </script>
    <style>
        #bg_popup_home {
            position: fixed;
            width: 100%;
            height: 100%;
            background-color: #333;
            opacity: 0.7;
            filter: alpha(opacity=70);
            left: 0px;
            top: 0px;
            z-index: 999999999;
            opacity: 0;
            filter: alpha(opacity=0);
        }

        #popup_ms_home {
            background: #fff;
            border-radius: 0px;
            box-shadow: 0px 2px 10px #fff;
            float: left;
            position: fixed;
            width: 735px;
            z-index: 10000;
            left: 50%;
            margin-left: -370px;
            top: 200px;
            opacity: 0;
            filter: alpha(opacity=0);
            height: 360px;
        }

            #popup_ms_home .popup_body {
                border-radius: 0px;
                float: left;
                position: relative;
                width: 735px;
            }

            #popup_ms_home .content {
                /*background-color: #487175;     border-radius: 10px;*/
                margin: 12px;
                padding: 15px;
                float: left;
            }

            #popup_ms_home .title_popup {
                /*background: url("../images/img_giaoduc1.png") no-repeat scroll -200px 0 rgba(0, 0, 0, 0);*/
                color: #ffffff;
                font-family: Arial;
                font-size: 24px;
                font-weight: bold;
                height: 35px;
                margin-left: 0;
                margin-top: -5px;
                padding-left: 40px;
                padding-top: 0;
                text-align: center;
            }

            #popup_ms_home .text_popup {
                color: #fff;
                font-size: 14px;
                margin-top: 20px;
                margin-bottom: 20px;
                line-height: 20px;
            }

                #popup_ms_home .text_popup a.quen_mk, #popup_ms_home .text_popup a.dangky {
                    color: #FFFFFF;
                    display: block;
                    float: left;
                    font-style: italic;
                    list-style: -moz-hangul outside none;
                    margin-bottom: 5px;
                    margin-left: 110px;
                    -webkit-transition-duration: 0.3s;
                    -moz-transition-duration: 0.3s;
                    transition-duration: 0.3s;
                }

                    #popup_ms_home .text_popup a.quen_mk:hover, #popup_ms_home .text_popup a.dangky:hover {
                        color: #8cd8fd;
                    }

            #popup_ms_home .close_popup {
                /*background: url("/App_Themes/bee-order/images/close_button.png") no-repeat scroll 0 0 rgba(0, 0, 0, 0);*/
                display: block;
                height: 28px;
                position: absolute;
                right: 0px;
                top: 5px;
                width: 26px;
                cursor: pointer;
                z-index: 10;
            }

        #popup_content_home {
            height: auto;
            position: fixed;
            background-color: #fff;
            top: 15%;
            z-index: 999999999;
            left: 25%;
            border-radius: 10px;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            width: 50%;
            padding: 20px;
        }

        #popup_content_home {
            padding: 0;
        }

        .popup_header, .popup_footer {
            float: left;
            width: 100%;
            background: #ea8c51;
            padding: 10px;
            position: relative;
            color: #fff;
        }

        .popup_header {
            font-weight: bold;
            font-size: 16px;
            text-transform: uppercase;
        }

        .close_message {
            top: 10px;
        }

        .changeavatar {
            padding: 10px;
            margin: 5px 0;
            float: left;
            width: 100%;
        }

        .float-right {
            float: right;
        }

        .content1 {
            float: left;
            width: 100%;
        }

        .content2 {
            float: left;
            width: 100%;
            border-top: 1px solid #eee;
            clear: both;
            margin-top: 10px;
        }

        .btn.btn-close {
            float: right;
            background: #dc4d88;
            color: #fff;
            margin: 10px 5px;
            text-transform: none;
            padding: 10px 20px;
        }

            .btn.btn-close:hover {
                background: #752d4b;
            }

        .btn.btn-close-full {
            float: right;
            background: #7bb1c7;
            color: #fff;
            margin: 10px 5px;
            text-transform: none;
            padding: 10px 20px;
        }

            .btn.btn-close-full:hover {
                background: #ea8c51;
            }


        @media screen and (max-width: 768px) {
            #popup_content_home {
                left: 10%;
                width: 80%;
            }

            .content1 {
                overflow: auto;
                height: 300px;
            }
        }
    </style>
</asp:Content>
