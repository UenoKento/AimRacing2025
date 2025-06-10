using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateManagerBase : MonoBehaviour
{
	[SerializeField]
	protected GameState m_gameState = 0;


	public abstract void Initialize();

	public abstract void StateUpdate();


	#region
	public GameState state => m_gameState;
	#endregion
}
