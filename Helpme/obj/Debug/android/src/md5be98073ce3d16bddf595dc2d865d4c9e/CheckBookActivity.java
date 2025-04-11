package md5be98073ce3d16bddf595dc2d865d4c9e;


public class CheckBookActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Helpme.CheckBookActivity, Helpme, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CheckBookActivity.class, __md_methods);
	}


	public CheckBookActivity ()
	{
		super ();
		if (getClass () == CheckBookActivity.class)
			mono.android.TypeManager.Activate ("Helpme.CheckBookActivity, Helpme, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
