using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
	public Node node1;
	public Node node2;
	public string owner = "";
	LineRenderer line;
	[SerializeField] LineRenderer rootRenderer;
	[SerializeField] Transform rootTip;

	public Node GetOtherNode(Node node)
	{
		if (node == node1)
		{
			return node2;
		} else
		{
			return node1;
		}
	}

	private void Start()
	{
		line = GetComponent<LineRenderer>();
		rootRenderer.enabled = false;
		Vector3 offset = new Vector3(0, 0, 0.1f);
		line.SetPosition(0, node1.transform.position + offset);
		line.SetPosition(1, node2.transform.position + offset);
	}

	public void GrowRoot(Node from)
	{
		Node to = GetOtherNode(from);
		StartCoroutine(GrowRoutine(from.transform.position, to.transform.position));
	}

	IEnumerator GrowRoutine(Vector3 from, Vector3 to)
	{
		rootTip.gameObject.SetActive(true);
		//rootTip.rotation = Quaternion.Euler(new(0, 0, Vector3.Angle(from, to)));
		rootTip.LookAt(to);
		rootRenderer.enabled = true;
		rootRenderer.SetPosition(1, from);
		float growTime = 0.3f;
		for (float t = 0; t < 0.3f; t += Time.deltaTime)
		{
			Vector3 end = Vector3.Lerp(from, to, t/growTime);
			rootRenderer.SetPosition(0, end);
			rootTip.position = end;
			yield return null;
		}
		rootRenderer.SetPosition(0, to);
		rootTip.gameObject.SetActive(false);
		line.enabled = false;
	}
}
