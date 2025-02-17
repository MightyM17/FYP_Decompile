using System.IO;
using UnityEngine;

public class utils
{
	public enum BlendMode
	{
		Opaque = 0,
		Cutout = 1,
		Fade = 2,
		Transparent = 3
	}

	public static Mesh CloneMesh(Mesh mesh)
	{
		return new Mesh
		{
			vertices = mesh.vertices,
			normals = mesh.normals,
			tangents = mesh.tangents,
			triangles = mesh.triangles,
			uv = mesh.uv,
			uv2 = mesh.uv2,
			uv3 = mesh.uv3,
			uv4 = mesh.uv4,
			uv5 = mesh.uv5,
			uv6 = mesh.uv6,
			uv7 = mesh.uv7,
			uv8 = mesh.uv8,
			bindposes = mesh.bindposes,
			boneWeights = mesh.boneWeights,
			bounds = mesh.bounds,
			colors = mesh.colors,
			name = mesh.name
		};
	}

	public static void SetMaterialRenderingMode(Material pMaterial, BlendMode pBlendMode)
	{
		switch (pBlendMode)
		{
		case BlendMode.Opaque:
			pMaterial.SetFloat("_Mode", 0f);
			pMaterial.SetInt("_SrcBlend", 1);
			pMaterial.SetInt("_DstBlend", 0);
			pMaterial.SetInt("_ZWrite", 1);
			pMaterial.DisableKeyword("_ALPHATEST_ON");
			pMaterial.DisableKeyword("_ALPHABLEND_ON");
			pMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			pMaterial.renderQueue = -1;
			break;
		case BlendMode.Cutout:
			pMaterial.SetFloat("_Mode", 1f);
			pMaterial.SetInt("_SrcBlend", 1);
			pMaterial.SetInt("_DstBlend", 0);
			pMaterial.SetInt("_ZWrite", 1);
			pMaterial.EnableKeyword("_ALPHATEST_ON");
			pMaterial.DisableKeyword("_ALPHABLEND_ON");
			pMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			pMaterial.renderQueue = 2450;
			break;
		case BlendMode.Fade:
			pMaterial.SetFloat("_Mode", 2f);
			pMaterial.SetInt("_SrcBlend", 5);
			pMaterial.SetInt("_DstBlend", 10);
			pMaterial.SetInt("_ZWrite", 0);
			pMaterial.DisableKeyword("_ALPHATEST_ON");
			pMaterial.EnableKeyword("_ALPHABLEND_ON");
			pMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			pMaterial.renderQueue = 3000;
			break;
		case BlendMode.Transparent:
			pMaterial.SetFloat("_Mode", 3f);
			pMaterial.SetInt("_SrcBlend", 1);
			pMaterial.SetInt("_DstBlend", 10);
			pMaterial.SetInt("_ZWrite", 0);
			pMaterial.DisableKeyword("_ALPHATEST_ON");
			pMaterial.DisableKeyword("_ALPHABLEND_ON");
			pMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			pMaterial.renderQueue = 3000;
			break;
		}
	}

	public static Texture2D LoadWaveformImage(string filePath)
	{
		Texture2D texture2D = null;
		if (File.Exists(filePath))
		{
			byte[] data = File.ReadAllBytes(filePath);
			texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			return RemoveColour(Color.black, texture2D);
		}
		return null;
	}

	public static Texture2D RemoveColour(Color colour, Texture2D texture)
	{
		Color[] pixels = texture.GetPixels(0, 0, texture.width, texture.height, 0);
		for (int i = 0; i < pixels.Length; i++)
		{
			if (pixels[i] == colour)
			{
				pixels[i] = new Color(0f, 0f, 0f, 0f);
			}
		}
		texture.SetPixels(0, 0, texture.width, texture.height, pixels, 0);
		texture.Apply();
		return texture;
	}
}
