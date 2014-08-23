using UnityEngine;
using System.Collections;

public class ScoreBehaviour : MonoBehaviour {

	public AudioClip[] deathClips;

	private int score = 0;					// The player's score.
	private int mistake = 0;
	private int maxMistake = 2;
	private ItemManagerBehaviour firstSide;
	private ItemManagerBehaviour secondSide;
	private int statusScore = 1;			// The score in the previous frame.
	
	
	void Awake ()
	{
		// Setting up the reference.
		firstSide = GameObject.Find("firstItemEmitter").GetComponent<ItemManagerBehaviour>();
		secondSide = GameObject.Find("secondItemEmitter").GetComponent<ItemManagerBehaviour>();
	}

	// Update is called once per frame
	void Update () {

		if (statusScore == 0) return;
		if (statusScore == 5) 
		{
			if (Input.GetMouseButtonDown (0)) Application.LoadLevel("GameLevel");
			return;
		}


		switch (statusScore) 
		{
			case 1: guiText.text = "Select Similar items"; break;
			case 2: guiText.text = string.Format("Not similar! Mistake {0}/{1}", mistake, maxMistake); break;
			case 3: guiText.text = string.Format("Score: {0}",score); break;
			case 4: guiText.text = string.Format("Final Score: {0}. Play Again?", score); StartCoroutine(GameOver()); break;
			default: guiText.text = string.Format("Score: {0}",score); break;
		}

		statusScore = 0;

	}

	public void CheckSimilarSelect()
	{
		if (null==firstSide.selectItem) return;
		if (null==secondSide.selectItem) return;
		if (firstSide.selectItem.color == secondSide.selectItem.color && firstSide.selectItem.prefabIndex == secondSide.selectItem.prefabIndex) 
		{
			score++;
			statusScore = 3;
			firstSide.Restart ();
			secondSide.Restart ();

			// Play a random audioclip from the deathClips array.
			int i = Random.Range(0, 2);
			AudioSource.PlayClipAtPoint(deathClips[i], transform.position);

			if (maxMistake<7) maxMistake++;
		} 
		else 
		{
			mistake++;
			statusScore = 2;

			firstSide.Deselect ();
			secondSide.Deselect ();

			AudioSource.PlayClipAtPoint(deathClips[2], transform.position);

			//Debug.Log(string.Format("mistake {0}/{1}", mistake, maxMistake));

			if (mistake >= maxMistake)
			{
				statusScore = 4;
				firstSide.GameOver (1);
				secondSide.GameOver (2);
			}
		}
	}

	private IEnumerator GameOver ()
	{
		yield return new WaitForSeconds (2f);
		
		statusScore = 5;
	}
}
