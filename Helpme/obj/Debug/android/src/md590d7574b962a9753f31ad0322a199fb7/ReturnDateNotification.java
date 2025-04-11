package md590d7574b962a9753f31ad0322a199fb7;


public class ReturnDateNotification
	extends android.app.Notification
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DongsungLibraryMobile.ReturnDateNotification, DongsungLibraryMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ReturnDateNotification.class, __md_methods);
	}


	public ReturnDateNotification ()
	{
		super ();
		if (getClass () == ReturnDateNotification.class)
			mono.android.TypeManager.Activate ("DongsungLibraryMobile.ReturnDateNotification, DongsungLibraryMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public ReturnDateNotification (android.os.Parcel p0)
	{
		super (p0);
		if (getClass () == ReturnDateNotification.class)
			mono.android.TypeManager.Activate ("DongsungLibraryMobile.ReturnDateNotification, DongsungLibraryMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.OS.Parcel, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public ReturnDateNotification (int p0, java.lang.CharSequence p1, long p2)
	{
		super (p0, p1, p2);
		if (getClass () == ReturnDateNotification.class)
			mono.android.TypeManager.Activate ("DongsungLibraryMobile.ReturnDateNotification, DongsungLibraryMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:Java.Lang.ICharSequence, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int64, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
