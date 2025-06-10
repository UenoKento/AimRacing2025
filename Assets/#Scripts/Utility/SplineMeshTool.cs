/**
 * @file    SplineMeshTool.cs
 * @brief   スプラインに沿ってメッシュを配置する
 * @author  22cu0219 鈴木友也
 * @date    2024/04/17  作成
 */

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;
using UnityEditor;

namespace SplineCustomTools
{
	[ExecuteAlways]
	[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
    public class SplineMeshTool : MonoBehaviour
    {
		[StructLayout(LayoutKind.Sequential)]
		struct VertexData
		{
			public Vector3 Position { get; set; }
		}


		[SerializeField]
        SplineContainer splineContainer;

        [SerializeField]
        int containerIndex = 0;

        [SerializeField]
        [Range(0, 100)]
        int resolution;

        [SerializeField]
        float width;

        float3 pos;

		List<Vector3> gizmoPos = new List<Vector3>();

        MeshFilter meshFilter;
        Mesh mesh;

        private void Reset()
        {
            TryGetComponent(out splineContainer);
            TryGetComponent(out meshFilter);
            mesh = new Mesh();
            meshFilter.sharedMesh = mesh;
        }

        private void OnEnable()
        {
			Spline.Changed += OnSplineChanged;
        }

        private void OnDisable()
        {
			Spline.Changed -= OnSplineChanged;
        }

		private void OnSplineChanged(Spline arg1, int arg2,SplineModification arg3)
        {
			Rebuild();
        }

        public void Rebuild()
        {
			Debug.Log("Rebuild Meshs");
			gizmoPos.Clear();

			mesh.Clear();
			var meshDataArray = Mesh.AllocateWritableMeshData(1);
			var meshData = meshDataArray[0];
			meshData.subMeshCount = 1;

			// 頂点数とインデックス数を計算する
			var vertexCount = 2 * (resolution + 1);
			var indexCount = 6 * resolution;

			// インデックスと頂点のフォーマットを指定する
			var indexFormat = IndexFormat.UInt32;
			meshData.SetIndexBufferParams(indexCount, indexFormat);
			meshData.SetVertexBufferParams(vertexCount, new VertexAttributeDescriptor[]
			{
			new(VertexAttribute.Position),
			});

			var vertices = meshData.GetVertexData<VertexData>();
			var indices = meshData.GetIndexData<UInt32>();

			for (int i = 0; i <= resolution; ++i)
			{
				// 頂点座標を計算する
				splineContainer?.Evaluate(containerIndex, (float)i / resolution, out pos, out var tangent, out  var upVec);
				gizmoPos.Add(pos);

				// ローカル座標に変換
				pos = transform.InverseTransformPoint(pos);
				Debug.Log("pos:" + pos);

                var p0 = vertices[2 * i];
				var p1 = vertices[2 * i + 1];
				p0.Position = pos;
				p1.Position = pos + new float3(0, width, 0);
				vertices[2 * i] = p0;
				vertices[2 * i + 1] = p1;

				Debug.Log("p0:" + p0.Position);
				Debug.Log("p1:" + p1.Position);
				
			}

			for (int i = 0; i < resolution; ++i)
			{
				indices[6 * i + 0] = (UInt32)(2 * i + 0);
				indices[6 * i + 1] = (UInt32)(2 * i + 1);
				indices[6 * i + 2] = (UInt32)(2 * i + 2);
				indices[6 * i + 3] = (UInt32)(2 * i + 1);
				indices[6 * i + 4] = (UInt32)(2 * i + 3);
				indices[6 * i + 5] = (UInt32)(2 * i + 2);
			}

			meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount));

			Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
			mesh.RecalculateBounds();
		}

		void OnRenderObject()
		{
			if (!Application.isPlaying)
			{
				EditorApplication.QueuePlayerLoopUpdate();
				SceneView.RepaintAll();
			}
		}


        private void OnDrawGizmosSelected()
        {
			if (gizmoPos.Count == 0)
				return;

			//Handles.matrix = transform.localToWorldMatrix;
			foreach(Vector3 pos in gizmoPos)
				Handles.SphereHandleCap(0, pos, Quaternion.identity, transform.lossyScale.magnitude * 0.1f, EventType.Repaint);
			
		}
	}
}
#endif
