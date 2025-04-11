package md568b7acc70a3923f7c540faa4319a94ae;


public class CookieObject
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.io.Serializable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ManageBook.CookieObject, ManageBook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CookieObject.class, __md_methods);
	}


	public CookieObject ()
	{
		super ();
		if (getClass () == CookieObject.class)
			mono.android.TypeManager.Activate ("ManageBook.CookieObject, ManageBook, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
