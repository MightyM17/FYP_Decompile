using System.Collections.Generic;

public class VertexConnection
{
	private HashSet<int> connection;

	public HashSet<int> Connection => connection;

	public VertexConnection()
	{
		connection = new HashSet<int>();
	}

	public void Connect(int to)
	{
		connection.Add(to);
	}

	public static Dictionary<int, VertexConnection> BuildNetwork(int[] triangles)
	{
		Dictionary<int, VertexConnection> dictionary = new Dictionary<int, VertexConnection>();
		int i = 0;
		for (int num = triangles.Length; i < num; i += 3)
		{
			int num2 = triangles[i];
			int num3 = triangles[i + 1];
			int num4 = triangles[i + 2];
			if (!dictionary.ContainsKey(num2))
			{
				dictionary.Add(num2, new VertexConnection());
			}
			if (!dictionary.ContainsKey(num3))
			{
				dictionary.Add(num3, new VertexConnection());
			}
			if (!dictionary.ContainsKey(num4))
			{
				dictionary.Add(num4, new VertexConnection());
			}
			dictionary[num2].Connect(num3);
			dictionary[num2].Connect(num4);
			dictionary[num3].Connect(num2);
			dictionary[num3].Connect(num4);
			dictionary[num4].Connect(num2);
			dictionary[num4].Connect(num3);
		}
		return dictionary;
	}
}
