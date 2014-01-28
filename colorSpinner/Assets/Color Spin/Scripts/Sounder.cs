using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Sounder : MonoBehaviour
{
	public enum Mode { Hits, Misses }

	private AudioSource source;
	public AudioClip[] clips;

	public Mode watch;

	private int lastObservedFigure = 0;

	// Use this for initialization
	void Start ()
	{
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		int currentFigure = watch == Mode.Hits ? GameStateController.Hits : GameStateController.Misses;

		if(clips.Length > 0 && lastObservedFigure < currentFigure)
		{
			if(source.isPlaying) source.Stop();
			AudioClip priorClip = source.clip;
			do {
				source.clip = clips[ Random.Range(0, clips.Length) ];
			} while(clips.Length > 1 && priorClip == source.clip);
			source.Play();
		}

		lastObservedFigure = currentFigure;
	}
}
