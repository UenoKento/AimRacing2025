using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class SearchObject : EditorWindow
{
	[MenuItem("Tools/Material vs Submesh Checker (Scene + Prefabs)")]
	public static void ShowWindow()
	{
		GetWindow<SearchObject>("Material vs Submesh Checker");
	}

	private Vector2 scrollPos;
	private List<ResultEntry> results = new List<ResultEntry>();

	private class ResultEntry
	{
		public string message;
		public Object obj;  // 選択対象（GameObjectかPrefabAsset）
	}

	void OnGUI()
	{
		if (GUILayout.Button("Check Materials vs Submeshes in Scene and Prefabs"))
		{
			CheckAll();
		}

		EditorGUILayout.LabelField($"Results: {results.Count}");
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		foreach (var entry in results)
		{
			if (GUILayout.Button(entry.message, GUILayout.Height(20)))
			{
				Selection.activeObject = entry.obj;
				EditorGUIUtility.PingObject(entry.obj);
			}
		}

		EditorGUILayout.EndScrollView();
	}

	void CheckAll()
	{
		results.Clear();

		// １．シーン内チェック
		var sceneRenderers = GameObject.FindObjectsOfType<Renderer>();
		foreach (var rend in sceneRenderers)
		{
			CheckRenderer(rend.gameObject, rend, false);
		}

		// ２．プロジェクト内Prefabチェック
		string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
		foreach (var guid in prefabGuids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
			if (prefab == null) continue;

			// Prefab内のRendererをすべて取得
			Renderer[] prefabRenderers = prefab.GetComponentsInChildren<Renderer>(true);
			foreach (var rend in prefabRenderers)
			{
				CheckRenderer(prefab, rend, true, path);
			}
		}

		if (results.Count == 0)
		{
			results.Add(new ResultEntry { message = "All checked renderers have matching material and submesh counts.", obj = null });
		}
	}

	// isPrefab = trueならPrefabAsset、falseならシーン内GameObject
	void CheckRenderer(GameObject root, Renderer rend, bool isPrefab, string prefabPath = null)
	{
		Material[] mats = rend.sharedMaterials;

		Mesh mesh = null;
		if (rend is MeshRenderer)
		{
			var mf = rend.GetComponent<MeshFilter>();
			if (mf) mesh = mf.sharedMesh;
		}
		else if (rend is SkinnedMeshRenderer smr)
		{
			mesh = smr.sharedMesh;
		}

		if (mesh == null) return;

		int subMeshCount = mesh.subMeshCount;
		int materialCount = mats.Length;

		if (subMeshCount != materialCount)
		{
			string name = isPrefab ? $"{root.name} (Prefab)" : rend.gameObject.name;
			string pathOrScene = isPrefab ? prefabPath : "Scene Object";

			results.Add(new ResultEntry
			{
				message = $"[Mismatch] {name} - Mesh '{mesh.name}' subMeshCount = {subMeshCount}, Material count = {materialCount} ({pathOrScene})",
				obj = isPrefab ? (Object)root : rend.gameObject
			});
		}
	}
}
