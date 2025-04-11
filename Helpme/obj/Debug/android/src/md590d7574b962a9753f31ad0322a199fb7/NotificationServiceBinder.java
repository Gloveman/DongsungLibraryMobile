package md590d7574b962a9753f31ad0322a199fb7;


public class NotificationServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DongsungLibraryMobile.NotificationServiceBinder, DongsungLibraryMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NotificationServiceBinder.class, __md_methods);
	}


	public NotificationServiceBinder ()
	{
		super ();
		if (getClass () == NotificationServiceBinder.class)
			mono.android.TypeManager.Activate ("DongsungLibraryMobile.NotificationServiceBinder, DongsungLibraryMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public NotificationServiceBinder (md590d7574b962a9753f31ad0322a199fb7.NotificationService p0)
	{
		super ();
		if (getClass () == NotificationServiceBinder.class)
			mono.android.TypeManager.Activate ("DongsungLibraryMobile.NotificationServiceBinder, DongsungLibraryMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "DongsungLibraryMobile.NotificationService, DongsungLibraryMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
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
