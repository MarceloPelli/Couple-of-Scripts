using UnityEngine;
using System.Collections;

public class QTE : MonoBehaviour
{
	[HideInInspector]
	public enum SequenceType
	{
		Single,
		Sequence,
		Mash
	}
	private SequenceType _sequenceType;

	public  KeyCode[] _keySequence;
	private KeyCode	  _keyToPress;

	[HideInInspector] public bool finished = false;
	[HideInInspector] public bool success = false;

	#region GUI Variables
	public	GameObject upArrow;
	public	GameObject downArrow;
	public	GameObject leftArrow;
	public	GameObject rightArrow;
	public	GameObject spacebar;
	private GameObject mySprite;
	#endregion

	private void Awake ()
	{
		upArrow	= (GameObject)Resources.Load("Prefabs/UI/QTEUpArrow");
		downArrow = (GameObject)Resources.Load("Prefabs/UI/QTEDownArrow");
		leftArrow = (GameObject)Resources.Load("Prefabs/UI/QTELeftArrow");
		rightArrow = (GameObject)Resources.Load("Prefabs/UI/QTERightArrow");
		spacebar = (GameObject)Resources.Load("Prefabs/UI/QTESpacebar");
	}

	#region StartQTEs
	public void StartQTE (KeyCode[] keySequence)
	{
		_keySequence = keySequence;

		StartCoroutine(QTESequence());
	}

	public void StartQTE (SequenceType sequenceType, KeyCode keyToPress)
	{
		_sequenceType = sequenceType;
		_keyToPress = keyToPress;

		switch(_keyToPress)
		{
			case KeyCode.W: mySprite = upArrow; break;
			case KeyCode.A: mySprite = leftArrow; break;
			case KeyCode.S: mySprite = downArrow; break;
			case KeyCode.D: mySprite = rightArrow; break;
			case KeyCode.Space: mySprite = spacebar; break;
		}
		mySprite = GameObject.Instantiate(mySprite, transform.position + Vector3.up * 3, transform.rotation) as GameObject;
		mySprite.transform.parent = transform;

		switch(_sequenceType)
		{
			case SequenceType.Single:
				StartCoroutine(QTESingle());
				break;
			case SequenceType.Mash:
				StartCoroutine(QTEMash()); 
				break;
		}
	}
	#endregion

	private IEnumerator QTESingle ()
	{
		for(float timer = 3; timer >= 0; timer -= Time.deltaTime)
		{
			// SUCCESS =)
			if(Input.GetKeyDown(_keyToPress))
			{
				success = finished = true;
				GameObject.Destroy(mySprite);
				yield break;
			}
			// FAIL by wrong input
			else if(Input.anyKeyDown)
			{
				success = false;
				finished = true;
				GameObject.Destroy(mySprite);
				yield break;
			}
			else yield return null;
		}

		// FAIL by time out!
		success = false;
		finished = true;
		GameObject.Destroy(mySprite);
	}

	private IEnumerator QTEMash ()
	{
		int keyPressCounter = 0;
		int missCounter = 0;

		for(float timer = 5; timer >= 0; timer -= Time.deltaTime)
		{
			if(Input.GetKeyDown(_keyToPress)) // Correct input =)
			{
				keyPressCounter++;

				if(keyPressCounter == 20)
				{
					success = finished = true;
					GameObject.Destroy(mySprite);
					yield break;
				}
			}
			else if(Input.anyKey) // Wrong input =(
			{
				// TODO Miss animation
				missCounter++;
				if(missCounter == 5)
				{
					success = false;
					finished = true;
					GameObject.Destroy(mySprite);
					yield break;
				}
			}
			yield return null;
		}

		// FAIL by time out!
		success = false;
		finished = true;
		GameObject.Destroy(mySprite);
	}

	private IEnumerator QTESequence ()
	{
		int keyIndex = 0;
		_keyToPress = _keySequence[keyIndex];

		for(float timer = 3; timer >= 0; timer -= Time.deltaTime)
		{
			// SUCCESS =)
			if(Input.GetKeyDown(_keyToPress))
			{
				if(keyIndex == _keySequence.Length - 1)
				{
					success = true;
					finished = true;
					GameObject.Destroy(mySprite);
					yield break;
				}
				else
				{

				}
				yield break;
			}
			// FAIL by wrong input
			else if(Input.anyKeyDown)
			{
				success = false;
				finished = true;
				GameObject.Destroy(mySprite);
				yield break;
			}
			else yield return null;
		}
		
		// FAIL by time out!
		success = false;
		finished = true;
		GameObject.Destroy(mySprite);
	}
}
