using UnityEngine;
using System.Collections;
public class ToolComponent : MonoBehaviour
{

	public float strength; // [0..1] or 0% to 100%

	void Start ()
	{
		strength = 1; // full strength
	}
	public void use(float damage) {
		strength -= damage;
	}

	public bool destroyed() {
		return strength <= 0f;
	}
}

