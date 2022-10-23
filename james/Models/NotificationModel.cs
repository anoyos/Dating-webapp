using james.Helpers.Custom;
using james.Helpers.Custom.Api;
using james.Helpers.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace james.Models
{
    public class NotificationModel
    {
        public void PushNotificationToAndroid(List<EnDeviceToken> regIDs, string Title, string message, EnNotificatoinPayload payload)
        {
            //Task mytask = Task.Run(() =>
            //{
            try
            {
                var serverkey = "AAAAKKFZ5SU:APA91bHBT6qiITa49xeolrgurLYTV8AlZjB49B8dYT9EtLafWDhaJwZEqlEIK56jKcODvcGzhzWAv-54MY5JkteGWlGtv0cVDknHOfRQ23N7MyvDi0_fuYSYwgF3hFX7pl0zhnodssl5";
                var SENDER_ID = "174505714981";


                {
                    foreach (var g in regIDs)
                    {
                        NotificationMessageData notificationdata = new NotificationMessageData
                        {
                            NotificationType = payload.notificationType,
                            ReferenceID = payload.referenceId,
                            MessageType = payload.notificationType,
                            SenderName = payload.senderName,
                            SenderPhoto = payload.senderPhoto,
                            isNotificationFlag = g.isNotificationFlag,
                            agoraToken = payload.agoraToken,
                        };

                        var NotificationData = new
                        {
                            notification = new
                            {
                                title = Title.Replace("_", " "),
                                body = message.Replace("  ", " "),
                            },
                            priority = "high",
                            data = notificationdata,
                            registration_ids = new List<string> { g.TokenID },
                        };

                        string postData = Common.Serialize(NotificationData);

                        WebRequest tRequest;

                        tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                        tRequest.Method = "post";

                        tRequest.ContentType = "application/json";
                        tRequest.Headers.Add(string.Format("Authorization: key={0}", serverkey));

                        tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));


                        Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                        tRequest.ContentLength = byteArray.Length;

                        Stream dataStream = tRequest.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                        WebResponse tResponse = tRequest.GetResponse();

                        dataStream = tResponse.GetResponseStream();

                        StreamReader tReader = new StreamReader(dataStream);

                        String sResponseFromServer = tReader.ReadToEnd();

                        tReader.Close();
                        dataStream.Close();
                        tResponse.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //});
        }
    }
}
