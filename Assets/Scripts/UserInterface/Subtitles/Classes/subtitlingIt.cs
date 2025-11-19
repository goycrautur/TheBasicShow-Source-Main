using System;
using UnityEngine;

[Serializable]
public class subtitlingItftTimings
{
    // hi yuri if u reading this hi x2
	public bool enabled = true;
	public string headText = "Placeholder";
	public Color textColor = new Color(1f, 1f, 1f, 1f);
	[Min(0f)]
	public float duration;
	public bool utStyle = true;
	public float TextAppearSpeed = 1f;
	public bool shakey;
	public float shakeyspeed = 1f;
    public float shakeyradius = 0.3f;
    public int fonts;
}
[Serializable]
public class subtitlingIt
{
    // hi yuri if u reading this hi
	public bool enabled = true;
	public string headText = "Placeholder";
	public Color textColor = new Color(1f, 1f, 1f, 1f);
	[Min(0f)]
	public float duration;
	public bool utStyle = true;
	public float TextAppearSpeed = 1f;
	public bool shakey;
	public float shakeyspeed = 1f;
    public float shakeyradius = 0.3f;
    public int fonts;
	public bool timedSub = false;
	public subtitlingItftTimings[] subtitleTimings;
}

