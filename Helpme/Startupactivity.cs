using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using System.Threading.Tasks;

namespace DongsungLibraryMobile
{
    [Activity(Label = "Dongsung Library Mobile",MainLauncher =true, Theme = "@style/startup",NoHistory =true,ScreenOrientation = ScreenOrientation.Portrait)]
    public class startupactivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)

        {
            base.OnCreate(savedInstanceState);
        }
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => {Startup(); });
            startupWork.Start();

        }
        async void Startup()
        {
            string filepath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "prefs.xml");
            Intent intent;
            await Task.Delay(2000);
            if (File.Exists(filepath))
            {
               

                intent = new Intent(this, typeof(CheckBookActivity));
                StartActivity(intent);
            }
            else
            {
                RunOnUiThread(() => {
                    new AlertDialog.Builder(this)
                    .SetMessage("로그인이 필요합니다.")
                    .SetCancelable(false)
                    .SetPositiveButton("OK", (senderAlert, args) => {
                         intent = new Intent(this, typeof(LoginActivity));
                          StartActivity(intent);
                         })
                        .Show();

                });
  
            }
        }
            

        
    }
}