using UnityEngine;

/// <summary>
/// LineRendererで円を描く
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class LineCircleDraw : MonoBehaviour
{
	public enum Axis { X, Y, Z };

	[SerializeField,Range(4, 1000),Tooltip("円の頂点数")]
	private int m_verticesCount = 60;
	[SerializeField, Tooltip("円の倍率")]
	private Vector2 m_radiusMagnification = Vector2.one;
	[SerializeField, Tooltip("半径")]
	private float m_radius = 1.0f;
	[SerializeField,Tooltip("線の幅")]
	private float m_lineWidth = 0.1f;
	[SerializeField, Tooltip("円の軸")]
	private Axis m_axis = Axis.Z;

	private LineRenderer m_line;

	/// <summary>
	/// 半径
	/// </summary>
	public float radius
	{
		get { return m_radius; }
		set 
		{
			m_radius = value; 
			CreateCircle();
		}
	}


	/// <summary>
	/// 開始処理
	/// </summary>
	void Start()
	{
		m_line = gameObject.GetComponent<LineRenderer>();
		m_line.useWorldSpace = false;
		m_line.loop = true;
		CreateCircle();
	}

	/// <summary>
	/// インスペクタの値を変えた時、円を再作成する
	/// </summary>
	private void OnValidate()
	{
		CreateCircle();
	}

	/// <summary>
	/// 円を作成する
	/// </summary>
	[ContextMenu("CreateCircle")]
	void CreateCircle()
	{
		if (m_line == null)
			m_line = GetComponent<LineRenderer>();

		m_line.startWidth = m_lineWidth;
		m_line.endWidth = m_lineWidth;
		m_line.positionCount = (m_verticesCount + 1);

		float x, y, z = 0.0f;
		float angle = 0.0f;

		for (int i = 0; i < (m_verticesCount + 1); i++)
		{
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius * m_radiusMagnification.x;
			y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius * m_radiusMagnification.y;

			switch (m_axis)
			{
				case Axis.X: m_line.SetPosition(i, new Vector3(z, y, x)); break;
				case Axis.Y: m_line.SetPosition(i, new Vector3(y, z, x)); break;
				case Axis.Z: m_line.SetPosition(i, new Vector3(x, y, z)); break;
				default: break;
			}

			angle += (360.0f / m_verticesCount);
		}
	}
}