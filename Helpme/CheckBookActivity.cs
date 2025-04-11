using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Xml;
using HtmlAgilityPack;
using Android.Content.PM;

namespace DongsungLibraryMobile
{
    [Activity(Label = "대출자료 조회",Theme = "@style/Gloveman", ScreenOrientation = ScreenOrientation.Portrait) ]
    public class CheckBookActivity : Activity
    {
        CookieCollection savecookie;
        List<BookDat> bookcollection;
        string id , pw;

        DateTime today;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //오늘 날짜 가져오기
            DateTime tempdate = DateTime.Now.ToLocalTime().Date;
            today = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day);

            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            SetContentView(Resource.Layout.CheckList);
            Button btnCheck = FindViewById<Button>(Resource.Id.btnCheckBook);
            Button btnLogAgain = FindViewById<Button>(Resource.Id.btnChangePW);
            btnLogAgain.Click += BtnLogAgain_Click;
            btnCheck.Click += BtnCheck_Click;
                string filepath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "prefs.xml");
                ProgressDialog progress = new ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                progress.SetMessage("로그인 중...");
                progress.SetCancelable(false);
                progress.Show();
                new Thread(new ThreadStart(() =>
                {
                   try
                    {
                        XmlDocument xd = new XmlDocument();
                        xd.Load(filepath);

                        XmlNode root = xd.SelectSingleNode("useraccount");
                        id = root.SelectSingleNode("id").InnerText;
                        pw = root.SelectSingleNode("pw").InnerText;
                        if (id != "" && pw != "")
                        {
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://reading.ssem.or.kr/r/reading/member/login.jsp");
                            req.Method = "Post";
                            req.KeepAlive = true;
                            req.Credentials = CredentialCache.DefaultCredentials;
                            req.ContentType = "application/x-www-form-urlencoded";
                            string content = "userid=" + id + "&passwd=" + pw + "&s_id=" + id + "&s_pwd=" + pw;
                            req.CookieContainer = new CookieContainer();
                            StreamWriter w = new StreamWriter(req.GetRequestStream());
                            w.Write(content);
                            w.Close();
                            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                            StreamReader r = new StreamReader(resp.GetResponseStream());
                            string result = r.ReadToEnd();
                            result = result.Substring(result.IndexOf(@"</SCRIPT>"));
                            if (result.Contains("아이디 또는 비밀번호가 맞지 않습니다"))
                            {
                                RunOnUiThread(() =>
                               {
                                   progress.Dismiss();
                                   new AlertDialog.Builder(this)
                                  .SetMessage("아이디 또는 비밀번호가 맞지 않습니다.\n다시 로그인하세요.")
                                  .SetCancelable(false)
                                  .SetPositiveButton("OK", (senderAlert, args) =>
                                  {
                                      Intent intent = new Intent(this, typeof(LoginActivity));
                                      StartActivity(intent);
                                  })
                                  .Show();


                               });
                            }
                            else
                            {
                                savecookie = resp.Cookies;
                                resp.Dispose();

                                string Datafilepath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "booksData.xml");
                                string viewtext = "";
                                if (File.Exists(Datafilepath))
                                {
                                   
                                    XmlDocument xml = new XmlDocument();
                                    xd.Load(Datafilepath);

                                    XmlNode Rootnode = xd.SelectSingleNode("loanbooks");
                                    XmlNodeList books = Rootnode.SelectNodes("book");
                                    foreach (XmlNode book in books)
                                    {
                                        viewtext += "책제목 : ";
                                        viewtext += book.SelectSingleNode("title").InnerText;  
                                        viewtext += "\n";
                                        viewtext += "반납 예정일 : ";
                                        viewtext += book.SelectSingleNode("returndate").InnerText;
                                        viewtext += "\n";

                                        DateTime returnday = DateTime.Parse(book.SelectSingleNode("returndate").InnerText.Replace('-','/'));
                                        TimeSpan remaintime = returnday - today;
                                        string remaindate = "";
                                        remaindate += Math.Abs(remaintime.Days).ToString();
                                        if (remaintime.Ticks<0)
                                        {
                                            remaindate += " 일 지남";
                                        }
                                        else
                                        {
                                            remaindate += " 일 남음";
                                        }

                                        viewtext += remaindate;
                                        viewtext += "\n";
                                    }
                                }
                                RunOnUiThread(() =>
                                {
                                    progress.Dismiss();
                                    TextView viewlist = FindViewById<TextView>(Resource.Id.viewList);
                                    viewlist.Text = viewtext;
                                    Toast.MakeText(this, "로그인 완료", ToastLength.Short).Show();
                                });
                            }
                        }
                        else
                        {
                            RunOnUiThread(() =>
                            {
                                progress.Dismiss();
                                new AlertDialog.Builder(this)
                               .SetMessage("로그인이 필요합니다.")
                               .SetCancelable(false)
                               .SetPositiveButton("OK", (senderAlert, args) =>
                               {
                                   Intent intent = new Intent(this, typeof(LoginActivity));
                                   StartActivity(intent);
                               })
                               .Show();
                            });


                        }
                    }
                    
                    catch (WebException we)
                    {
                        RunOnUiThread(() =>
                        {
                            progress.Dismiss();
                            new AlertDialog.Builder(this)
                           .SetMessage("인터넷이 연결되지 않았습니다. 앱을 종료합니다.")
                           .SetCancelable(false)
                           .SetPositiveButton("OK", (senderAlert, args) =>
                           {
                               FinishAffinity();
                           })
                           .Show();
                        });
                    }
                    
                }
                    )).Start();
           
        }

        private void BtnLogAgain_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                
                new Thread(new ThreadStart(() =>
                {
                   

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://reading.ssem.or.kr/r/reading/mypage/borrowList.jsp");
                    req.Method = "Post";

                    req.ContentType = "application/x-www-form-urlencoded";
                    req.CookieContainer = new CookieContainer();
                    req.CookieContainer.Add(savecookie);
                    req.Referer = "https://reading.ssem.or.kr/r/reading/mypage/borrowReturnHistoryForm.jsp";
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    StreamReader r = new StreamReader(resp.GetResponseStream());
                    string result = r.ReadToEnd();
                    resp.Dispose();

                    bookcollection = GetBookList(result);

                    SaveBookListtoXML(bookcollection);


                    
                    string viewtext = "";

                    foreach (var book in bookcollection)
                    {
                       
                        viewtext += "책제목 : ";
                        viewtext += book.Title;
                        viewtext += "\n";
                        viewtext += "반납 예정일 : ";
                        viewtext += book.Returndate;
                        viewtext += "\n";
                        DateTime returnday = DateTime.Parse(book.Returndate.Replace('-', '/'));
                        TimeSpan remaintime = returnday - today;
                        string remaindate = Math.Abs(remaintime.Days).ToString();
                        if (remaintime.Ticks < 0)
                        {
                            remaindate += " 일 지남";
                        }
                        else
                        {
                            remaindate += " 일 남음";
                        }

                        viewtext += remaindate;
                        viewtext += "\n";
                        
                        Intent bintent = new Intent("DongsungLibrary.Update");
                        SendBroadcast(bintent);


                    }
                    RunOnUiThread(() =>
                    {
                        TextView viewlist = FindViewById<TextView>(Resource.Id.viewList);
                        viewlist.Text = viewtext;
                       
                    });
                })).Start();
            }
            catch (Exception E)
            {
                new AlertDialog.Builder(this)
               .SetTitle("오류 발생")
               .SetMessage(E.Message)
               .Show();
            }
        }
        public override void OnBackPressed()
        {
            new AlertDialog.Builder(this)
              .SetTitle("앱 종료")
              .SetMessage("정말 종료하시겠습니까?")
              .SetPositiveButton("종료",(senderAlert, args) => { FinishAffinity(); })
              .SetNeutralButton("로그인 정보 변경", (senderAlert, args) => {
                  Intent intent = new Intent(this, typeof(LoginActivity));
                  StartActivity(intent);  })
              .SetNegativeButton("취소", (senderAlert, args) => { })
              .Show();
            
        }
        private List<BookDat> GetBookList(string resulthtml)
        {
            BookDat tempbook = new BookDat();
            List<BookDat> templist = new List<BookDat>();
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(resulthtml);
            HtmlNode table = html.DocumentNode.SelectNodes("//table[@class='loan']").First();
            HtmlNodeCollection tds = table.SelectNodes(".//td");
            int i = 0;
            string txttemp = "";
            foreach (var node in tds)
            {
               
                if (node.HasAttributes == true)
                {
                    tempbook = new BookDat(); 
                    i = 0;
                    string[] temp = node.InnerText.Split('/');
                    txttemp = temp[0];
                }
                if (i == 2)
                {
                    tempbook.Title = txttemp;
                    tempbook.Returndate = node.InnerText;
                    templist.Add(tempbook);
                }
                i++;
            }
            return templist;
        }
        private void SaveBookListtoXML(List<BookDat> bookList)
        {
            string Datafilepath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "booksData.xml");
            XmlDocument xd = new XmlDocument();
            XmlNode root = xd.CreateElement("loanbooks");
            xd.AppendChild(root);
            foreach (var book in bookList)
            {
                XmlNode booknode = xd.CreateElement("book");
                root.AppendChild(booknode);
                XmlNode booktitle = xd.CreateElement("title");
                booktitle.InnerText = book.Title;
                booknode.AppendChild(booktitle);
                XmlNode bookreturndate = xd.CreateElement("returndate");
                bookreturndate.InnerText = book.Returndate;
                booknode.AppendChild(bookreturndate);
            }
            xd.Save(Datafilepath);
        }
    }

}