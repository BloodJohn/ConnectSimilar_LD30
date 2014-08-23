using UnityEngine;
using System.Collections;

public class ItemBehaviour : MonoBehaviour {

	public Vector3 velocity = Vector3.right;
	public Color color = Color.white;
	public int prefabIndex = 1;

	public bool isSelected = false;

	private const int totalColor = 9;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += velocity * Time.deltaTime;
	}

	public void ChangeSelect(bool newSelected)
	{
		if (isSelected == newSelected) return;

		isSelected = newSelected;
		
		if (isSelected)
			transform.localScale = Vector3.one * 1.5f;
		else
			transform.localScale = Vector3.one * 1f;
	}


	public void SetColor(int maxColor)
	{
		int rndColor = Random.Range(0, maxColor<totalColor ? maxColor : totalColor);

		color = Color.white;

		switch (rndColor) 
		{
		case 1: color = Color.red; break;
		case 2: color = Color.green; break;
		case 3: color = Color.blue; break;
		case 4: color = Color.yellow; break;
		case 5: color = Color.cyan; break;
		case 6: color = Color.magenta; break;
		case 7: color = Color.grey; break;
		case 8: color = Color.gray; break;
		default: color = Color.white; break;
		}

		renderer.material.color = color;
	}

	public void Remove()
	{
		StartCoroutine(RemoveItem());
	}

	private IEnumerator RemoveItem ()
	{
		velocity = Quaternion.Euler(0, 0, isSelected ? -90 : 90) * velocity * (isSelected ? 4 : 7);

		yield return new WaitForSeconds (2f);

		Destroy (gameObject);
	}

}
