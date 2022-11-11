<%@ Page Language="C#" MasterPageFile="~/NhapSiChinaMaster.Master" AutoEventWireup="true" CodeBehind="Default8.aspx.cs" Inherits="NHST.Default8" %>

<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <main class="main-wrap">
        <div class="main-banner">
            <div class="decor-fly plane-decor">
                <img src="/App_Themes/nhapsichina/images/decor-fly.png" alt="" id="air-plane">
            </div>
            <div class="container">
                <div class="content-banner">
                    <h4 class="title-small animation-text">
                    </h4>
                    <h3 class="title-big animation-title animated" data-in-effect="rollIn" data-out-effect="fadeOutDown" data-out-shuffle="true" data-wow-delay="0.5s">NHẬP SỈ CHINA<br> NHANH CHÓNG VÀ CHUYÊN NGHIỆP </h3>
                    <p class="desc">
                    </p>
                    <p class="tools">
                        Công cụ đặt hàng:
                    </p>
                    <div class="box-btn-tool">
                        <a href="https://chrome.google.com/webstore/detail/c%C3%B4ng-c%E1%BB%A5-%C4%91%E1%BA%B7t-h%C3%A0ng-c%E1%BB%A7a-nh%E1%BA%ADp/elfmiplccfnohmbbpdehiflablbfeijh" class="btn btn-tool bg-red "><img src="/App_Themes/nhapsichina/images/chrome.png" alt=""> <span>Chrome</span></a>
                        <a href="https://chrome.google.com/webstore/detail/c%C3%B4ng-c%E1%BB%A5-%C4%91%E1%BA%B7t-h%C3%A0ng-c%E1%BB%A7a-nh%E1%BA%ADp/elfmiplccfnohmbbpdehiflablbfeijh" class="btn btn-tool bg-green m-r-10"><img src="/App_Themes/nhapsichina/images/coccoc.png" alt=""> <span>Cốc cốc</span></a>
                    </div>
                </div>
            </div>
        </div>
        <section class="main-service">
            <div class="table-main-service">
                <div class="columns">
                     <asp:Literal ID="ltrService" runat="server"></asp:Literal>
                    <div class="colum">
                        <div class="box-main-ser">
                            <div class="main-title">
                                <h3 class="title">
                                    DỊCH VỤ CHÍNH
                                </h3>
                                <p class="desc">
                                  Chúng tôi đưa đến giải pháp kết nối con người / doanh nghiệp Việt Nam với nguồn hàng tận gốc (không qua bất kỳ trung gian nào) và trở thành người đồng hành lâu dài nhất của họ trên con đường cùng nhau tìm thấy mục tiêu.
                                </p>
                                <a href="#" class="btn btn-tool bg-green">Read more</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section class="sec sec-about">
            <div class="container">
                <div class="main-title">
                    <h3 class="title">
                        Dịch vụ của chúng tôi
                    </h3>
                </div>
                <div class="table-about">
                    <div class="columns">
                        <asp:Literal ID="ltrServiceCus" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </section>
        <section class="sec sec-procedure">
            <div class="container">
                <div class="main-title text-center  width-550">
                    <h3 class="title">
                        HƯỚNG DẪN ĐĂNG KÝ
                    </h3>
                </div>
                <div class="tab-swrap wow zoomIn" data-wow-delay=".4s" data-wow-duration="1s">
                    <nav class="c-tab__nav">
                        <div class="line-tab"></div>
                        <ul>
                             <asp:Literal ID="ltrStep1" runat="server"></asp:Literal>
                        </ul>
                    </nav>
                      <asp:Literal ID="ltrStep2" runat="server"></asp:Literal>
                </div>
            </div>
        </section>
        <section class="sec sec-customer">
            <div class="container">
                <div class="main-title text-center  width-550 width-700">
                    <h3 class="title">
                        QUYỀN LỢI KHÁCH HÀNG
                    </h3>
                </div>
                <div class="table-customer">
                    <div class="img-thung-hang">
                        <img src="/App_Themes/nhapsichina/images/thung-hang.png" alt="">
                    </div>
                    <div class="columns">
                        <asp:Literal ID="ltrBenefit" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </section>
        <section class="sec sec-number">
            <div class="container">
                <div class="table-number">
                    <div class="main-title w-40 wow fadeInUp" data-wow-delay=".2s" data-wow-duration="1s" style="visibility: visible; animation-duration: 1s; animation-delay: 0.2s; animation-name: fadeInUp;">
                        <h3 class="title">
                            CHÚNG TÔI CÓ</br>
                            5 NĂM </br> KINH NGHIỆM
                        </h3>
                    </div>
                    <div class="columns w-60">
                        <div class="colum">
                            <div class="content-number wow fadeInUp" data-wow-delay=".2s" data-wow-duration="1s" style="visibility: visible; animation-duration: 1s; animation-delay: 0.2s; animation-name: fadeInUp;">
                                <div class="icon">
                                    <img src="/App_Themes/nhapsichina/images/number-1.png" alt="">
                                </div>
                                <div class="number counting" data-count="05">05</div>
                                <div class="text-number">
                                    <p class="txt">Kho hàng</p>
                                </div>
                            </div>
                        </div>
                        <div class="colum">
                            <div class="content-number wow fadeInUp" data-wow-delay=".4s" data-wow-duration="1s" style="visibility: visible; animation-duration: 1s; animation-delay: 0.4s; animation-name: fadeInUp;">
                                <div class="icon">
                                    <img src="/App_Themes/nhapsichina/images/number-3.png" alt="">
                                </div>
                                <div class="number counting" data-count="560">560</div>
                                <div class="text-number">
                                    <p class="txt">Đơn hàng</p>
                                </div>
                            </div>
                        </div>
                        <div class="colum">
                            <div class="content-number wow fadeInUp" data-wow-delay=".6s" data-wow-duration="1s" style="visibility: visible; animation-duration: 1s; animation-delay: 0.6s; animation-name: fadeInUp;">
                                <div class="icon">
                                    <img src="/App_Themes/nhapsichina/images/number-2.png" alt="">
                                </div>
                                <div class="number counting" data-count="2400">2400</div>
                                <div class="text-number">
                                    <p class="txt">Khách hàng</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-map-contact">
                    <div class="container">
                        <div class="columns">
                            <div class="colum">
                                <div class="contact-li">
                                    <div class="icon">
                                        <img src="/App_Themes/nhapsichina/images/ft-mail.png" alt="">
                                    </div>
                                    <div class="text">
                                        <div class="txt-big">
                                            EMAIL CONTACT
                                        </div>
                                        <div class="txt-small">
                                             <asp:Literal runat="server" ID="ltrTopLeftEmail"></asp:Literal>
                                            <%--<a href="mailto:chinanhapsi@gmail.com">chinanhapsi@gmail.com</a>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="colum">
                                <div class="contact-li">
                                    <div class="icon">
                                        <img src="/App_Themes/nhapsichina/images/ft-hour.png" alt="">
                                    </div>
                                    <div class="text">
                                        <div class="txt-big">
                                            GIỜ HOẠT ĐỘNG
                                        </div>
                                        <div class="txt-small">
                                             <asp:Literal runat="server" ID="ltrTime"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="colum">
                                <div class="contact-li">
                                    <div class="icon">
                                        <img src="/App_Themes/nhapsichina/images/ft-phone.png" alt="">
                                    </div>
                                    <div class="text">
                                        <div class="txt-big">
                                            HOTLINE
                                        </div>
                                        <div class="txt-small">
                                            <asp:Literal runat="server" ID="ltrHotlineFooter"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section class="sec-map">
            <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d1565.768094579167!2d105.78489468170106!3d21.030880683779444!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3135ab4c78507d3f%3A0x8dcf93d729932f22!2zMjQgUC4gRHV5IFTDom4sIEThu4tjaCBW4buNbmcgSOG6rXUsIEPhuqd1IEdp4bqleSwgSMOgIE7hu5lpLCBWaeG7h3QgTmFt!5e0!3m2!1svi!2s!4v1661829896297!5m2!1svi!2s" width="100%" height="500" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
        </section>
    </main>
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
                        fr += "<div class=\"popup_header\" style=\"display: flex;justify-content: space-between;\">";
                        fr += "<div>" + title + "</div>";
                        fr += "<div><a style='cursor:pointer;right:5px;color:red;' onclick='close_popup_ms()' class='close_message'>x</a></div>";
                        fr += "</div>";
                        fr += "     <div class=\"changeavatar\">";
                        fr += "         <div class=\"content1\" style=\"height: 500px;\">";
                        fr += content;
                        fr += "         </div>";
                        //fr += "         <div class=\"content2\">";
                        //fr += "<a href=\"javascript:;\" class=\"btn btn-close-full\" onclick='closeandnotshow()'>Đóng & không hiện thông báo</a>";
                        //fr += "<a href=\"javascript:;\" class=\"btn btn-close\" onclick='close_popup_ms()'>Đóng</a>";
                        //fr += "         </div>";
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
            background: #366136;
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
            width:8%;
        }

            .btn.btn-close:hover {
                background: #366136;
            }

        .btn.btn-close-full {
            float: right;
            background: #7bb1c7;
            color: #fff;
            margin: 10px 5px;
            text-transform: none;
            padding: 10px 20px;
            width:25%;
        }

            .btn.btn-close-full:hover {
                background: #366136;
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