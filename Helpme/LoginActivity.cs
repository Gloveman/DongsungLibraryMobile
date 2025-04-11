using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Android.Content;
using System.Xml;
using Android.Content.PM;

namespace DongsungLibraryMobile
{
    [Activity(Label = "로그인", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
           
                base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            string filepath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "prefs.xml");
            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            SetContentView(Resource.Layout.Login);
            Button btnLogin = FindViewById<Button>(Resource.Id.button1);
            btnLogin.Click += BtnLogin_Click;
            if(File.Exists(filepath))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(filepath);

                XmlNode root = xd.SelectSingleNode("useraccount");
                FindViewById<EditText>(Resource.Id.txtID).Text = root.SelectSingleNode("id").InnerText;
                FindViewById<EditText>(Resource.Id.txtPW).Text = root.SelectSingleNode("pw").InnerText;
            }
        }
        
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                    string id = FindViewById<EditText>(Resource.Id.txtID).Text;
                    string pw = FindViewById<EditText>(Resource.Id.txtPW).Text;
                string filepath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "prefs.xml");
                if (id != "" && pw != "")
                {
                    new AlertDialog.Builder(this)
                    .SetTitle("로그인")
                    .SetMessage("아이디와 비밀번호가 정확합니까?")
                    .SetNegativeButton("취소", (senderAlert, args) => { })
                    .SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        new Thread(new ThreadStart(() => {
                            XmlDocument xd = new XmlDocument();
                            XmlNode root = xd.CreateElement("useraccount");
                            xd.AppendChild(root);
                            XmlNode _ID = xd.CreateElement("id");
                            _ID.InnerText = id;
                            root.AppendChild(_ID);
                            XmlNode _PW = xd.CreateElement("pw");
                            _PW.InnerText = pw;
                            root.AppendChild(_PW);
                            xd.Save(filepath);
                        })).Start();
                        var intent = new Intent(this, typeof(CheckBookActivity));
                        StartActivity(intent);
                    })
                    .Show();
                }
                else
                {
                    new AlertDialog.Builder(this)
                    .SetMessage("아이디와 비밀번호를 모두 입력하세요!")
                    .SetPositiveButton("OK", (senderAlert, args) => { })
                    .Show();
                }
                

            }
            catch(Exception E)
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
              .SetMessage("종료 하시겠습니까?")
              .SetPositiveButton("종료", (senderAlert, args) => { FinishAffinity(); })
              .SetNegativeButton("취소", (senderAlert, args) => { })
              .Show();
            ;
        }


    }
}