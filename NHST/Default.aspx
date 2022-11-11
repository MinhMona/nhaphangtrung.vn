<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/NHAPHANGTRUNGMASTER.Master" CodeBehind="Default.aspx.cs" Inherits="NHST.Default5" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <main>
        <section class="wrapper-banner">
            <div class="main-banner">
                <div class="container">
                    <div class="content-banner">
                        <div class="content-banner-left">
                            <div class="title-banner">
                                <h2>dịch vụ nhập hàng Trung Quốc</h2>
                            </div>
                            <div class="search-form-wrap">
                                <div id="search" class="search-form tab-content">
                                    <div class="select-form">
                                        <select class="fcontrol" id="brand-source">
                                            <option value="taobao" data-image="/App_Themes/NHAPHANGTRUNG/images/hdsearch-item-taobao.png">Taobao</option>
                                            <option value="tmall" data-image="/App_Themes/NHAPHANGTRUNG/images/hdsearch-item-tmall.png">Tmall.com</option>
                                            <option value="1688" data-image="/App_Themes/NHAPHANGTRUNG/images/hdsearch-item-1688.png">1688</option>
                                        </select>
                                        <span class="icon">
                                            <i class="fa fa-sort" aria-hidden="true"></i>
                                        </span>
                                    </div>
                                    <div class="input-form">
                                        <asp:TextBox type="text" class="fcontrol f-input" runat="server" ID="txtSearch" placeholder="Nhập tên sản phẩm tìm kiếm"></asp:TextBox>
                                    </div>
                                    <a href="javascript:" onclick="searchProduct()" class="main-btn">
                                        <img src="/App_Themes/NHAPHANGTRUNG/images/search-ic.png" alt="">
                                    </a>
                                    <asp:Button ID="btnSearch" runat="server"
                                        OnClick="btnSearch_Click" Style="display: none"
                                        OnClientClick="document.forms[0].target = '_blank';" UseSubmitBehavior="false" />
                                </div>
                            </div>
                        </div>
                        <div class="content-banner-right">
                            <div class="title-banner">
                                <h2>Công cụ đặt hàng Trung Quốc</h2>
                            </div>
                            <div class="desc-p">
                                <p>
                                    (Lưu ý chỉ sử dụng trên máy tính)
                                </p>
                            </div>
                            <div class="box-plg">
                                <a href="" class="browser-down">
                                    <img src="/App_Themes/NHAPHANGTRUNG/images/gg-ic.png" alt="">
                                    <div class="text-browser">
                                        <p>Tải về cho trình duyệt</p>
                                        <span>Google Chrome</span>
                                    </div>
                                </a>
                                <a href="" class="browser-down">
                                    <img src="/App_Themes/NHAPHANGTRUNG/images/coc-ic.png" alt="">
                                    <div class="text-browser">
                                        <p>Tải về cho trình duyệt</p>
                                        <span>Cốc Cốc</span>
                                    </div>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section class="sec wrapper-2 ">
            <div class="container">
                <div class="title-sec">
                    <h2>CAM KẾT CỦA CHÚNG TÔI</h2>
                </div>
                <div class="content-2">
                    <div class="desc-sec">
                        <p>
                            <span>NHẬP HÀNG TRUNG</span>
                            Cam kết đền bù 100% giá trị tiền hàng đã cọc trong quá trình vận chuyển Trung Việt khi không may hàng hóa xảy ra thất lạc ….
                        </p>
                    </div>
                </div>
                <div class="wrapper-card">
                    <div class="cols">
                        <asp:Literal runat="server" ID="ltrCamKet"></asp:Literal>
                    </div>
                </div>
            </div>
        </section>
        <section class=" wrapper-3">
            <div class="bg-wrapper-3">
                <div class="container">
                    <div class="title-sec">
                        <h2>bảng giá dịch vụ order hàng NHẬP HÀNG TRUNG</h2>
                    </div>
                    <div class="content-3">
                        <div class="cols">
                            <div class="col-4">
                                <div class="left-wrapper-3">
                                    <div class="img-wrapper">
                                        <img src="/App_Themes/NHAPHANGTRUNG/images/air.png" alt="">
                                    </div>
                                </div>
                            </div>
                            <div class="col-8">
                                <div class="right-wrapper-3">
                                    <div class="banggia-wrapper">
                                        <div class="cols">
                                            <div class="col-6">
                                                <div class="banggia">
                                                    <div class="circle-banggia">
                                                        <div class="circle-inline">
                                                            <h4>01</h4>
                                                        </div>
                                                    </div>
                                                    <div class="text-banggia">
                                                        <h3>tiền hàng trên web</h3>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="banggia">
                                                    <div class="circle-banggia">
                                                        <div class="circle-inline">
                                                            <h4>02</h4>
                                                        </div>
                                                    </div>
                                                    <div class="text-banggia">
                                                        <h3>phí ship NHẬP HÀNG TRUNG</h3>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="banggia">
                                                    <div class="circle-banggia">
                                                        <div class="circle-inline">
                                                            <h4>03</h4>
                                                        </div>
                                                    </div>
                                                    <div class="text-banggia">
                                                        <h3>tiền hàng trên web</h3>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="banggia">
                                                    <div class="circle-banggia">
                                                        <div class="circle-inline">
                                                            <h4>04</h4>
                                                        </div>
                                                    </div>
                                                    <div class="text-banggia">
                                                        <h3>cước vận chuyển</h3>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="btn-web">
                                        <a href="/chuyen-muc/bang-gia" class="btn-click">Chi tiết bảng giá</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section class="wrapper-4 pd-not-top">
            <div class="bg-wrapper-4">
                <div class="container">
                    <div class="title-sec">
                        <h2>DỊCH VỤ CỦA NHẬP HÀNG TRUNG</h2>
                    </div>
                    <div class="desc-sec">
                        <p><span>NHẬP HÀNG TRUNG</span> luôn luôn cố gắng không ngừng nghỉ nhằm mang lại cho khách hàng dịch vụ nhập hàng tốt nhất, uy tín nhất, mang lại sự hài lòng khi khách hàng sử dụng dịch vụ nhập hàng của chúng tôi.</p>
                    </div>
                    <div class="content-4">
                        <div class="cols">
                            <asp:Literal runat="server" ID="ltrServices"></asp:Literal>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section class="sec wrapper-1">
            <div class="container">
                <div class="title-sec">
                    <h2>Hướng dẫn đặt hàng</h2>
                </div>
            </div>
            <div class="content-1">
                <asp:Literal runat="server" ID="ltrStep1"></asp:Literal>
            </div>
        </section>
        <section class="sec wrapper-5">
            <div class="container">
                <div class="title-sec">
                    <h2>Câu hỏi thường gặp</h2>
                </div>
                <div class="desc-sec">
                    <p>Danh mục giải đáp các vấn đề thường xuyên xảy ra đối với khách hàng trong quá trình NHẬP HÀNG TRUNG, order hàng quảng châu. Nếu khách hàng thắc mắc hay gặp những vấn đề khi nhập hàng hãy gửi email hoặc liên hệ ngay với NHẬP HÀNG TRUNG. Chúng tôi sẽ giải đáp toàn bộ vấn đề mà quý khách gặp phải.</p>
                </div>
                <div class="content-5">
                    <div class="cols">
                        <div class="col-6">
                            <div class="left-5">
                                <asp:Literal runat="server" ID="ltrQuestions"></asp:Literal>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="right-5">
                                <img src="/App_Themes/NHAPHANGTRUNG/images/51374.png" alt="">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section class="sec wrapper-6">
            <div class="container">
                <div class="content-6">
                    <div class="cols">
                        <div class="col-8">
                            <div class="left-6">
                                <div class="title-sec">
                                    <h2>Ý kiến khách hàng</h2>
                                </div>
                                <div class="box-quotes">
                                    <p>
                                        Khi bắt đầu khởi nghiệp tôi rất mơ hồ về nguồn hàng, không biết tìm nguồn hàng ở đâu rẻ và đẹp để mở shop. Rất may mắn khi thấy bài quảng cáo của NHẬP HÀNG TRUNG trên Facebook và cũng nhờ nhân viên của công ty tư vấn nhiệt tình mà tôi đã tìm được đúng nguồn hàng mà mình mong muốn cho Shop của mình
                                    </p>
                                </div>
                                <div class="info-person">
                                    <div class="avatar">
                                        <img src="/App_Themes/NHAPHANGTRUNG/images/chi-nguyen-thuy-van.png" alt="">
                                    </div>
                                    <div class="name-person">
                                        <h3>Chị Nguyễn Thúy Vân</h3>
                                        <p>Chủ shop thời trang</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="right-6">
                                <div class="title-sec">
                                    <h2>hỗ trợ khách hàng</h2>
                                </div>
                                <div class="support-cus">
                                    <div class="cols">
                                        <div class="col-6">
                                            <div class="box-support">
                                                <div class="img-zalo">
                                                    <img src="/App_Themes/NHAPHANGTRUNG/images/ic-zalo.png" alt="">
                                                </div>
                                                <div class="admin">
                                                    <h3>CSKH 1</h3>
                                                    <a href="tel:+0965276680">096.527.6680</a>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="box-support">
                                                <div class="img-zalo">
                                                    <img src="/App_Themes/NHAPHANGTRUNG/images/ic-zalo.png" alt="">
                                                </div>
                                                <div class="admin">
                                                    <h3>CSKH 2</h3>
                                                    <a href="tel:+0387171688">038.717.1688</a>
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
        </section>
        <section class="sec wrapper-7 pd-not-top">
            <div class="container">
                <div class="title-sec">
                    <h2>tin mới nhất</h2>
                </div>
                <div class="content-7">
                    <div class="cols">
                        <asp:Literal runat="server" ID="ltrNews"></asp:Literal>
                    </div>
                </div>
            </div>
        </section>
        <section class="wrapper-8">
            <div class="bg-wrapper">
                <div class="container">
                    <div class="title-sec">
                        <h2>thông tin ngân hàng</h2>
                    </div>
                    <div class="content-8">
                        <div class="cols">
                            <asp:Literal runat="server" ID="ltrBank"></asp:Literal>
                        </div>
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
        function keyclose_ms(e) {
            if (e.keyCode == 27) {
                close_popup_ms();
            }
        }
        function searchProduct() {
            var web = $("#brand-source").val();
            $("#<%=hdfWebsearch.ClientID%>").val(web);
            $("#<%=btnSearch.ClientID%>").click();
        }
    </script>
    <script type="text/javascript">        
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
                background: url("/App_Themes/Camthach/images/close_button.png") no-repeat scroll 0 0 rgba(0, 0, 0, 0);
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
            top: 17%;
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
            background: #34a8ec;
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
            background: #d00f0f;
            color: #fff;
            margin-top: 20px;
            text-transform: none;
            padding: 10px 20px;
            width: unset;
        }

            .btn.btn-close:hover {
                background: #5f0d0d;
            }

        .btn.btn-close-full {
            float: right;
            background: #7bb1c7;
            color: #fff;
            margin: 20px 5px;
            text-transform: none;
            padding: 10px 20px;
            width: unset;
        }

            .btn.btn-close-full:hover {
                background: #435156;
            }
    </style>
</asp:Content>
