using MB.Extensions;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class PushNotiAuto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PushNotiJob();
                PushNotiAPP();
            }
        }

        public class SmsResponse
        {
            public string MessageCount { get; set; }
            public List<Message> Messages { get; set; }
        }

        public class Message
        {
            public string To { get; set; }
            public string MessageId { get; set; }
            public string Status { get; set; }
            public string RemainingBalance { get; set; }
            public string MessagePrice { get; set; }
            public string Network { get; set; }
            public string From { get; set; }
        }

        protected void PushNotiAPP()
        {
            var temp = AppPushNotiController.GetNoti();
            if (temp != null)
            {
                string link = "";
                AppPushNotiController.UpdateSent(temp.ID, "SystemAPI");
                var l = DeviceTokenController.GetAllDevice();
                if (l != null)
                {
                    foreach (var item in l)
                    {
                        Data dt = new Data();
                        dt.AppNotiTitle = temp.AppNotiTitle;
                        dt.AppNotiMessage = temp.AppNotiMessage;
                        dt.Device = item.Device;
                        dt.Type = Convert.ToInt32(item.Type);
                        dt.link = link;

                        Thread t = new Thread(Push);

                        t.Start(dt);

                    }
                }
            }
        }

        public void Push(object ob)
        {
            Data dt = (Data)ob;
            PushAndroidiOS(dt.AppNotiTitle, dt.Device, Convert.ToInt32(dt.Type), dt.AppNotiMessage, dt.link);
        }


        public class Data
        {
            public string AppNotiTitle { get; set; }
            public string Device { get; set; }
            public int Type { get; set; }
            public string AppNotiMessage { get; set; }
            public string link { get; set; }
        }


        //protected void PushStatus()
        //{
        //    var o = MainOrderController.GetByStatus(9);
        //    if (o != null)
        //    {
        //        foreach (var item in o)
        //        {
        //            int dem = 0;
        //            var smalls = SmallPackageController.GetByMainOrderID(item.ID);
        //            if (smalls != null)
        //            {
        //                dem = smalls.Where(x => x.Status != 6).ToList().Count();
        //            }
        //            if (item.DateToFis != null && dem == 0)
        //            {
        //                var date = DateTime.Now;
        //                if (item.ReceivePlace.ToInt(0) == 1)
        //                {
        //                    if ((date - item.DateToFis.Value).Days > 4)
        //                    {
        //                        MainOrderController.UpdateStatusByID(item.ID, 10);
        //                        if (item.CompleteDate == null)
        //                        {
        //                            MainOrderController.UpdateCompleteDate(item.ID, date);
        //                        }
        //                    }
        //                }
        //                else if (item.ReceivePlace.ToInt(0) == 3)
        //                {
        //                    if ((date - item.DateToFis.Value).Days > 8)
        //                    {
        //                        MainOrderController.UpdateStatusByID(item.ID, 10);
        //                        if (item.CompleteDate == null)
        //                        {
        //                            MainOrderController.UpdateCompleteDate(item.ID, date);
        //                        }
        //                    }
        //                }


        //                Data dt = new Data();
        //                dt.AppNotiTitle = title;
        //                dt.AppNotiMessage = mess;
        //                dt.Device = temp.Device;
        //                dt.Type = Convert.ToInt32(temp.Type);
        //                dt.link = link;

        //                Thread t = new Thread(Push);

        //                t.Start(dt);

        //            }
        //        }
        //    }
        //}


        protected void PushNotiJob()
        {
            var lpush = NotificationsController.GetAllPush();
            if (lpush != null)
            {
                if (lpush.Count > 0)
                {
                    foreach (var item in lpush)
                    {
                        string link = "";// thông báo
                        string title = "";

                        if (item != null)
                        {
                            if (item.PushNotiApp == true)
                            {
                                NotificationsController.UpdateSent(item.ID, "SystemAPI");
                                var list = DeviceTokenController.GetAllByUIDandIsHide(item.ReceivedID.Value);
                                if (list != null)
                                {
                                    DateTime cre = Convert.ToDateTime(item.CreatedDate);
                                    //string mess = cre.Hour + ":" + cre.Minute + " " + cre.Day + "/" + cre.Month + " " + item.Message;
                                    string mess = cre.ToString("HH:mm dd/MM") + " " + item.Message;
                                    foreach (var temp in list)
                                    {
                                        string Key = temp.UserToken;
                                        if (item.NotifType == 1)
                                        {
                                            link = "https://NHAPSICHINA.COM/chi-tiet-don-hang-app.aspx?UID=" + item.ReceivedID + "&OrderID=" + item.OrderID + "&Key=" + Key + "";  //đơn hàng mua hộ
                                            title = "ĐƠN HÀNG MUA HỘ";
                                        }
                                        if (item.NotifType == 2)
                                        {
                                            link = "https://NHAPSICHINA.COM/lich-su-giao-dich-app.aspx?UID=" + item.ReceivedID + "&Key=" + Key + "";  //Nạp tiền
                                            title = "NẠP TIỀN";
                                        }
                                        if (item.NotifType == 3)
                                        {
                                            link = "https://NHAPSICHINA.COM/lich-su-giao-dich-app.aspx?UID=" + item.ReceivedID + "&Key=" + Key + ""; //Rút tiền
                                            title = "RÚT TIỀN";
                                        }
                                        if (item.NotifType == 5)
                                        {
                                            link = "https://NHAPSICHINA.COM/khieu-nai-app.aspx?UID=" + item.ReceivedID + "&Key=" + Key + ""; //Khiếu nại
                                            title = "KHIẾU NẠI";
                                        }
                                        if (item.NotifType == 8)
                                        {
                                            link = "https://NHAPSICHINA.COM/thanh-toan-ho-app.aspx?UID=" + item.ReceivedID + "&Key=" + Key + ""; //Thanh toán hộ
                                            title = "THANH TOÁN HỘ";
                                        }
                                        if (item.NotifType == 9)
                                        {
                                            link = "https://NHAPSICHINA.COM/lich-su-giao-dich-tien-te-app.aspx?UID=" + item.ReceivedID + "&Key=" + Key + ""; //Hoàn tệ
                                            title = "HOÀN TỆ";
                                        }
                                        if (item.NotifType == 10)
                                        {
                                            link = "https://NHAPSICHINA.COM/danh-sach-kien-ky-gui-app.aspx?UID=" + item.ReceivedID + "&Key=" + Key + ""; //Vận chuyển hộ
                                            title = "VẬN CHUYỂN HỘ";
                                        }
                                        if (item.NotifType == 11)
                                        {
                                            link = "https://NHAPSICHINA.COM/chi-tiet-don-hang-khac-app.aspx?UID=" + item.ReceivedID + "&OrderID=" + item.OrderID + "&Key=" + Key + ""; //đơn hàng TMĐT
                                            title = "ĐƠN HÀNG TMĐT";
                                        }
                                        if (item.NotifType == 12)
                                        {
                                            link = "https://NHAPSICHINA.COM/chi-tiet-don-hang-app.aspx?UID=" + item.ReceivedID + "&OrderID=" + item.OrderID + "&Key=" + Key + "";  //tin nhắn đơn hàng mua hộ
                                            title = "TIN NHẮN ĐƠN HÀNG MUA HỘ";
                                        }
                                        if (item.NotifType == 13)
                                        {
                                            link = "https://NHAPSICHINA.COM/chi-tiet-don-hang-khac-app.aspx?UID=" + item.ReceivedID + "&OrderID=" + item.OrderID + "&Key=" + Key + ""; //tin nhắn đơn hàng TMĐT
                                            title = "TIN NHẮN ĐƠN HÀNG TMĐT";
                                        }

                                        Data dt = new Data();
                                        dt.AppNotiTitle = title;
                                        dt.AppNotiMessage = mess;
                                        dt.Device = temp.Device;
                                        dt.Type = Convert.ToInt32(temp.Type);
                                        dt.link = link;

                                        Thread t = new Thread(Push);

                                        t.Start(dt);

                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        public class FCMResponse
        {
            public long multicast_id { get; set; }
            public int success { get; set; }
            public int failure { get; set; }
            public int canonical_ids { get; set; }
            public List<FCMResult> results { get; set; }
        }
        public class FCMResult
        {
            public string message_id { get; set; }
        }
        protected void PushAndroidiOS(string title, string DeviceToken, int TypeDevice, string Noti, string link)
        {
            string android_firebase_key = "AAAAlH56jiw:APA91bHCufI-LmxW-8FMTrU21Nfcw-pfQL0hP_Rg6ZZLK_-Z7H7SrmkYjI4KvwOuHpQkOv8TbwkLdq5IQ16iOtkUH3hyAF8_RYgU5YbLd0HwLoaHoS652DqQMUl7pF1OeAsuJkqcsu7b";
            string SenderIdAndroid = "637777120812";

            string ios_firebase_key = "AAAAlH56jiw:APA91bHCufI-LmxW-8FMTrU21Nfcw-pfQL0hP_Rg6ZZLK_-Z7H7SrmkYjI4KvwOuHpQkOv8TbwkLdq5IQ16iOtkUH3hyAF8_RYgU5YbLd0HwLoaHoS652DqQMUl7pF1OeAsuJkqcsu7b";
            string SenderIdiOS = "637777120812";


            var SENDER_ID = "";
            var FirebaseKey = "";

            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var objNotificationiOS = new
            {
                to = DeviceToken,
                collapse_key = "type_a",
                notification = new
                {
                    body = Noti,
                    title = title
                },
                data = new
                {
                    title = title,
                    message = Noti,
                    image = "http://vominhthien.com/App_Themes/vominhthien/images/main-logo.png",
                    link = link

                }
            };
            var objNotificationAndroid = new
            {
                to = DeviceToken,
                data = new
                {
                    title = title,
                    message = Noti,
                    image = "http://vominhthien.com/App_Themes/vominhthien/images/main-logo.png",
                    link = link
                }
            };


            string jsonNotificationFormat = "";
            if (TypeDevice == 1) //1: Android; 2: IOS
            {
                SENDER_ID = SenderIdAndroid;
                FirebaseKey = android_firebase_key;
                jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotificationAndroid);
            }
            else
            {
                SENDER_ID = SenderIdiOS;
                FirebaseKey = ios_firebase_key;
                jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotificationiOS);
            }

            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", FirebaseKey));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String responseFromFirebaseServer = tReader.ReadToEnd();

                            FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            if (response.success == 1)
                            {
                                // thành công
                            }
                            else if (response.failure == 1)
                            {
                                //thất bại
                            }
                        }
                    }

                }
            }
        }
    }
}