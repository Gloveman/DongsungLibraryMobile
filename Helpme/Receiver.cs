using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.Widget;
using System.Xml;
using Android.OS;

namespace DongsungLibraryMobile
{
    [BroadcastReceiver]
    [IntentFilter(new[] { "android.intent.action.BOOT_COMPLETED", "android.intent.action.LOCKED_BOOT_COMPLETED" })]
    public class BootingReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == "android.intent.action.BOOT_COMPLETED"||intent.Action== "android.intent.action.LOCKED_BOOT_COMPLETED")
            {
                Intent bintent = new Intent("DongsungLibrary.Update");
                context.SendBroadcast(bintent);
            }
        }
    }
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {

          
        public override void OnReceive(Context context, Intent intent)
        {
            NotificationManager notificationManager =
              context.GetSystemService(Context.NotificationService) as NotificationManager;
            if (intent.GetStringExtra("interval")=="8시간")
            {
                 Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle("도서 반납 안내")
                .SetContentText("도서 대출일이 "+intent.GetStringExtra("duedate")+"일 남았습니다. 책을 반납해 주세요.")
                .SetContentIntent(PendingIntent.GetActivity(context, 0, new Intent(context, typeof(CheckBookActivity)), PendingIntentFlags.UpdateCurrent))
                .SetSmallIcon(Resource.Drawable.ico)
                .SetTicker("도서 반납 안내")
                .SetAutoCancel(true);
                
                Notification notification = builder.Build();
                notificationManager.Notify(8, notification);

                Toast.MakeText(context, "책을 반납하세요!!", ToastLength.Long).Show();

            }
            else if(intent.GetStringExtra("interval")=="2시간")
            {
                 Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle("도서 반납 안내")
                .SetContentText("도서 대출일이 "+ intent.GetStringExtra("duedate") + "일밖에 남지 않았습니다. 책을 반납해 주세요.")
                .SetContentIntent(PendingIntent.GetActivity(context, 0, new Intent(context, typeof(CheckBookActivity)), PendingIntentFlags.UpdateCurrent))
                .SetSmallIcon(Resource.Drawable.ico)
                .SetTicker("도서 반납 안내")
                .SetAutoCancel(true);
                
                Notification notification = builder.Build();
                notificationManager.Notify(2, notification);


                Toast.MakeText(context, "책을 반납하세요!!", ToastLength.Long).Show();
            }
            

            
        }
        
     }
    [BroadcastReceiver]
    [IntentFilter(new[] {"DongsungLibrary.Update" })]
    public class UpdateReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //목록 띄우기 + 알림 설정의 대 통합!
            DateTime tempdate = DateTime.Now.ToLocalTime().Date;
            DateTime today = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day);

            string Datafilepath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "booksData.xml");
            if (File.Exists(Datafilepath))
            {
                string viewtext = "";
                int minday = 365;
                bool isoverdue = false;
                List<BookDat> bookcollection = LoadXMLtoBookList(Datafilepath);
                foreach (BookDat book in bookcollection)
                {
                    viewtext += "책제목 : ";
                    viewtext += book.Title;
                    viewtext += "\n";
                    DateTime returnday = DateTime.Parse(book.Returndate.Replace('-', '/'));
                    TimeSpan remaintime = returnday - today;
                    string remaindate = Math.Abs(remaintime.Days).ToString();
                    if (remaintime.Ticks < 0)
                    {
                        isoverdue = true;
                        remaindate += " 일 지남";
                    }
                    else
                    {
                        if (remaintime.Days < minday)
                        {
                            minday = remaintime.Days;
                        }
                        remaindate += " 일 남음";
                    }

                    viewtext += remaindate;
                    viewtext += "\n";
                }
                //대출목록 알림
                Notification.Builder builder = new Notification.Builder(context)
                     .SetContentTitle("대출 목록 안내")
                     //.SetContentText(viewtext)
                     .SetStyle(new Notification.BigTextStyle().BigText(viewtext))
                     .SetSmallIcon(Resource.Drawable.ico)
                     .SetContentIntent(PendingIntent.GetActivity(context, 0, new Intent(context, typeof(CheckBookActivity)), PendingIntentFlags.UpdateCurrent))
                     .AddAction(Resource.Drawable.ico_refresh, "갱신", PendingIntent.GetBroadcast(context, 0, new Intent(context, typeof(UpdateReceiver)), PendingIntentFlags.UpdateCurrent))
                     .SetOngoing(true);

                Notification notification = builder.Build();
                NotificationManager notificationManager =
                    context.GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager.Notify(0, notification);
                Toast.MakeText(context, "대출기록이 갱신되었습니다", ToastLength.Short).Show();

                //반납독촉 알림

                //모든알림취소코드
                notificationManager.Cancel(2);
                notificationManager.Cancel(1);
                Intent BSalarmIntent = new Intent(context, typeof(AlarmReceiver));
                BSalarmIntent.PutExtra("interval", "8시간");
                BSalarmIntent.PutExtra("duedate", minday.ToString());
                Intent BTalarmIntent = new Intent(context, typeof(AlarmReceiver));
                BTalarmIntent.PutExtra("interval", "2시간");
                BTalarmIntent.PutExtra("duedate", minday.ToString());
                PendingIntent BeforeSeven = PendingIntent.GetBroadcast(context, 8, BSalarmIntent, PendingIntentFlags.UpdateCurrent);
                PendingIntent BeforeThree = PendingIntent.GetBroadcast(context, 2, BTalarmIntent, PendingIntentFlags.UpdateCurrent);
                AlarmManager alarmManager = context.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Cancel(BeforeSeven);
                alarmManager.Cancel(BeforeThree);

                //연체
                if (isoverdue)
                {
                    RemoteViews view = new RemoteViews(context.PackageName, Resource.Layout.customnotification);

                    builder = new Notification.Builder(context)
                            .SetContent(view)
                           .SetContentIntent(PendingIntent.GetActivity(context, 0, new Intent(context, typeof(CheckBookActivity)), PendingIntentFlags.UpdateCurrent))
                           .SetSmallIcon(Resource.Drawable.ico)
                           .SetOngoing(true);
                    notification = builder.Build();
                    notificationManager.Notify(2, notification);


                }

                if(minday<=7)
                {
                    if(minday<=3)
                    {
                        if(minday==1)
                        {
                            //항상알림
                            builder = new Notification.Builder(context)
                            .SetContentTitle("도서 반납 1일 전")
                            .SetContentText("도서 대출기한이 하루 남았습니다. 빨리 책을 반납해 주세요.")
                            .SetContentIntent(PendingIntent.GetActivity(context, 0, new Intent(context, typeof(CheckBookActivity)), PendingIntentFlags.UpdateCurrent))
                            .SetSmallIcon(Resource.Drawable.ico)
                            .SetOngoing(true);
                            notification = builder.Build();
                            notificationManager.Notify(1, notification);
                        }
                        else
                        {

                        alarmManager.SetRepeating(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 1 * 1000, 7200 * 1000, BeforeThree);
                        
                        }

                    }
                    else
                    {
                        alarmManager.SetRepeating(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 1 * 1000, 28800 * 1000, BeforeSeven);


                    }
                }
            }
            else
            {
                Toast.MakeText(context, "대출기록이 없습니다", ToastLength.Short).Show();
            }
        }
        List<BookDat> LoadXMLtoBookList(string path)
        {
            List<BookDat> Templist = new List<BookDat>();
            
            XmlDocument xd = new XmlDocument();
            xd.Load(path);

            XmlNode Rootnode = xd.SelectSingleNode("loanbooks");
            XmlNodeList books = Rootnode.SelectNodes("book");
            foreach (XmlNode book in books)
            {
                BookDat tempbook = new BookDat();
                tempbook.Title= book.SelectSingleNode("title").InnerText;
                tempbook.Returndate = book.SelectSingleNode("returndate").InnerText;
                Templist.Add(tempbook);
            }
            return Templist;
        }
    }
}
