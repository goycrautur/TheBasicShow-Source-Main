using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class NanaPeelScript : MonoBehaviour
{
	// Token: 0x0600092A RID: 2346 RVA: 0x00020C21 File Offset: 0x0001F021
	private void Start()
	{
		player = FindObjectOfType<PlayerScript>();
		this.rb = base.GetComponent<Rigidbody>(); //Get the RigidBody
		if (throwedByPlayer)
		{
			this.audioDevice.PlayOneShot(this.aud_Woo);
			this.rb.velocity = base.transform.forward * 20; //Move forward
			StartCoroutine(Wooo());
		}
		else
		{
			stopped = true;
		}
	}

    IEnumerator Wooo()
    {
    yield return new WaitForSeconds(1);
	this.rb.velocity = base.transform.forward * 0;
	stopped = true;
    }
    public void Wee()
    {
		this.rb.velocity = 20 * base.transform.forward;
        this.slipping = true;
        this.audioDevice.clip = this.aud_Slip;
		this.audioDevice.loop = true;
		this.audioDevice.Play();
    }

	// Token: 0x0600092B RID: 2347 RVA: 0x00020C5C File Offset: 0x0001F05C
	private void Update()
	{
        if (this.slipping)
        {
		this.rb.velocity = base.transform.forward * 20;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 0 || other.gameObject.layer == 8) && other.gameObject.tag != "Player" & !other.isTrigger && this.slipping)
        {
        this.audioDevice.Stop();
		this.audioDevice.loop = false;
		this.audioDevice.PlayOneShot(this.aud_Woo);
		this.rb.velocity = base.transform.forward * 0;
        Disappear();
        }
        if ((other.gameObject.layer == 0 || other.gameObject.layer == 8) && other.gameObject.tag != "Player" & !other.isTrigger && !this.slipping)
        {
        this.audioDevice.PlayOneShot(this.aud_Bum);
		this.rb.velocity = base.transform.forward * 0;
    	}
        if (other.tag == "NPC" && !this.slipping)
        {
		this.nanaTransform.rotation = other.transform.rotation;
        this.Wee();
		npcSlide = true;
        }
        if (other.tag == "Player" && !this.slipping && stopped)
        {
		this.nanaTransform.rotation = other.transform.rotation;
    	this.Wee();
		playerSlide = true;
        }
    }

	private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && this.slipping && stopped && playerSlide & !npcSlide)
    	{
			other.transform.position = transform.position;
			other.GetComponent<CharacterController>().Move(transform.forward * 0.25f);
        }
    }


    public void Disappear()
    {
		this.audioDevice.PlayOneShot(this.aud_Woo);
		slipping = false;
		stopped = false;
		UnityEngine.Object.Destroy(base.gameObject, 1f);
    }

	// Token: 0x040005AE RID: 1454
	private float lifeSpan;

	public Transform nanaTransform;

	public AudioClip aud_Woo;

	public AudioClip aud_Bum;


	public AudioClip aud_Slip;

	// Token: 0x040005AF RID: 1455
	private Rigidbody rb;

    public AudioSource audioDevice;

    private bool slipping;

    private bool stopped;

    private bool npcSlide;

    private bool playerSlide;

	public PlayerScript player;

	public bool throwedByPlayer = true;
}
