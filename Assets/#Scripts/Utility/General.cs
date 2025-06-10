using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Rendering.PostProcessing
{
    public class General
    {
		/// <summary>
		/// �X�v���C�g�����b�V���ɕϊ�����
		/// </summary>
		/// <param name="sprite">�p�ӂ���Sprite</param>
		/// <returns>Sprite��ϊ�����Mesh</returns>
		public Mesh SpriteToMesh(Sprite sprite)
		{
			var mesh = new Mesh();
			mesh.SetVertices(Array.ConvertAll(sprite.vertices, c => (Vector3)c).ToList());
			mesh.SetUVs(0, sprite.uv.ToList());
			mesh.SetTriangles(Array.ConvertAll(sprite.triangles, c => (int)c), 0);

			return mesh;
		}

		/// <summary>
		///�@PropertySheet�̍쐬
		/// </summary>
		/// <param name="shader">�p�ӂ���Shader</param>
		/// <returns>PropertySheet</returns>
		//public PropertySheet GetSheet(Shader shader)
		//{
		//	var shaderName = shader.name;
		//	var material = new Material(shader)
		//	{
		//		name = string.Format("PostProcess - {0}", shaderName.Substring(shaderName.LastIndexOf('/') + 1)),
		//		hideFlags = HideFlags.DontSave
		//	};
		//	var sheet = new PropertySheet(material);
		//	return sheet;
		//}
	}
}
