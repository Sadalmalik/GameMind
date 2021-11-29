using UnityEngine;

public class Cell : MonoBehaviour
{
	public Vector2Int position;
    
	public Border left;
	public Border forward;
	public Border right;
	public Border back;
    
	public GameObject view;
}