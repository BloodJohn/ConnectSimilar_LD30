using UnityEngine;
using System.Collections;

public class ItemManagerBehaviour : MonoBehaviour {
	public GameObject[] itemPrefabList;
	public Vector3 velocity = Vector3.right;
	public int maxColor = 1;

	public const float respawnTime = 3f;
	public bool isScrolling = true;
	public ItemBehaviour selectItem = null;
	private int selectIndex = -1;


	private const int totalColor = 9;
	private ScoreBehaviour score;
	private const int maxLength = 5;
	private int currentIndex = 0;
	private GameObject[] itemList =  new GameObject[maxLength];
	

	void Awake ()
	{
		score = GameObject.Find ("score").GetComponent<ScoreBehaviour> ();
	}

	// Use this for initialization
	void Start () {
		isScrolling = true;
		StartCoroutine(SpawnItems());
		
	}

	void Update () {

		if (isScrolling && Input.GetMouseButtonDown (0)) {
			
			Vector2 mousePosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
			
			if (hitCollider != null)
			{
				int selID = hitCollider.gameObject.GetInstanceID();
				int hitIndex = -1;

				for(int i=0; i<maxLength && hitIndex<0; i++)
					if (itemList[i] != null && itemList[i].GetInstanceID()==selID) hitIndex=i;

				if (hitIndex>=0)
				{
					selectIndex = hitIndex;
					for(int i=0; i<maxLength; i++)
						if (itemList[i] != null) 
					{
						ItemBehaviour itemBehaviour = itemList[i].GetComponent<ItemBehaviour>();
						if (itemBehaviour != null) 
						{
							itemBehaviour.ChangeSelect(hitIndex==i);
							if (hitIndex==i) selectItem = itemBehaviour;
						}
					}

					score.CheckSimilarSelect();
				}

			}
		}
	}

	public void Deselect ()
	{
		if (selectItem != null) 
		{
			selectItem.ChangeSelect (false);
			selectItem.Remove();
			selectItem = null;
		}

		if (selectIndex >= 0) 
		{
			itemList [selectIndex] = null;
			selectIndex = -1;
		}
	}

	public void Restart ()
	{
		maxColor++;
		selectIndex = -1;
		selectItem = null;

		for(int i=0; i<maxLength; i++)
			if (itemList[i] != null) 
		{
			ItemBehaviour item = itemList[i].GetComponent<ItemBehaviour>();
			
			if (item != null) item.Remove(); else Destroy(itemList[i]);
			itemList[i] = null;
		}
	}

	public void GameOver (int startColor)
	{
		maxColor = startColor;
		
		for(int i=0; i<maxLength; i++)
			if (itemList[i] != null) 
		{
			ItemBehaviour item = itemList[i].GetComponent<ItemBehaviour>();

			if (item != null) item.Remove(); else Destroy(itemList[i]);
			itemList[i] = null;
		}

		isScrolling = false;
	}
	
	private IEnumerator SpawnItems ()
	{
		while (isScrolling) {
			GameObject newItem = itemList [currentIndex];
			
			if (null == newItem) {
				int index = Mathf.Clamp( Random.Range(0,maxColor*2/totalColor),0,3);
				newItem = (GameObject)Instantiate (itemPrefabList [index], transform.position, Quaternion.identity);
				itemList [currentIndex] = newItem;
			}
			else 
			{
				newItem.transform.position = transform.position;
			}

			ItemBehaviour nextItem = newItem.GetComponent<ItemBehaviour>();
			if (nextItem != null) 
			{
				if (nextItem.isSelected) 
				{
					selectItem = null;
					selectIndex=-1;
				}
				nextItem.ChangeSelect(false);
				nextItem.velocity = velocity * (1 + maxColor/5);
				nextItem.SetColor(maxColor);
			}
			
			currentIndex++;
			if (currentIndex >= maxLength) currentIndex = 0;
			yield return new WaitForSeconds (respawnTime*7/ (5 + maxColor));
		}
	}
}
