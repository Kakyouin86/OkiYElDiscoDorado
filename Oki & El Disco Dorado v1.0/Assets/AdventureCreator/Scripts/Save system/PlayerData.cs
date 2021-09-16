﻿/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2021
 *	
 *	"PlayerData.cs"
 * 
 *	A data container for saving the state of a Player. Each Player in a game has its own instance of this class stored in SaveData by SaveSystem.
 * 
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	/**
	 * A data container for saving the state of a Player.
	 * Each Player in a game has its own instance of this class stored in SaveData by SaveSystem.
	 */
	[System.Serializable]
	public class PlayerData
	{

		#region Variables

		/** The ID number of the Player that this data references */
		public int playerID = 0;
		/** The current scene number */
		public int currentScene = -1;
		/** The current scene name */
		public string currentSceneName = "";
		/** The last-visited scene number */
		public int previousScene = -1;
		/** The last-visited scene name */
		public string previousSceneName = "";
		/** The details any sub-scenes that are also open (as build indices) */
		public string openSubScenes = "";
		/** The details any sub-scenes that are also open (as names) */
		public string openSubSceneNames = "";

		/** The Player's X position */
		public float playerLocX = 0f;
		/** The Player's Y position */
		public float playerLocY = 0f;
		/** The Player's Z position */
		public float playerLocZ = 0f;
		/** The Player'sY rotation */
		public float playerRotY = 0f;

		/** The walk speed */
		public float playerWalkSpeed = 0f;
		/** The run speed */
		public float playerRunSpeed = 0f;

		/** The idle animation */
		public string playerIdleAnim = "";
		/** The walk animation */
		public string playerWalkAnim = "";
		/** The talk animation */
		public string playerTalkAnim = "";
		/** The run animation */
		public string playerRunAnim = "";

		/** A unique identifier for the walk sound AudioClip */
		public string playerWalkSound = "";
		/** A unique identifier for the run sound AudioClip */
		public string playerRunSound = "";
		/** A unique identified for the portrait graphic */
		public string playerPortraitGraphic = "";
		/** The Player's display name */
		public string playerSpeechLabel = "";
		/** The ID number that references the Player's name, as generated by the Speech Manager */
		public int playerDisplayLineID = 0;

		/** The target node number of the current Path */
		public int playerTargetNode = 0;
		/** The previous node number of the current Path */
		public int playerPrevNode = 0;
		/** The positions of each node in a pathfinding-generated Path */
		public string playerPathData = "";
		/** True if the Player is currently running */
		public bool playerIsRunning = false;
		/** True if the Player is locked along a Path */
		public bool playerLockedPath = false;
		/** True if the Player is locked along a Path, and going backwards */
		public bool playerLockedPathReversing = false;
		/** The type of Path the Player is locked along, if playerLockedPathReversing = true */
		public int playerLockedPathType;
		/** The Constant ID number of the Player's current Path */
		public int playerActivePath = 0;
		/** True if the Player's current Path affects the Y position */
		public bool playerPathAffectY = false;

		/** The target node number of the Player's last-used Path */
		public int lastPlayerTargetNode = 0;
		/** The previous node number of the Player's last-used Path */
		public int lastPlayerPrevNode = 0;
		/** The Constant ID number of the Player's last-used Path */
		public int lastPlayerActivePath = 0;

		/** True if the Player cannot move up */
		public bool playerUpLock = false;
		/** True if the Player cannot move down */
		public bool playerDownLock = false;
		/** True if the Player cannot move left */
		public bool playerLeftlock = false;
		/** True if the Player cannot move right */
		public bool playerRightLock = false;
		/** True if the Player cannot run */
		public int playerRunLock = 0;
		/** True if free-aiming is prevented */
		public bool playerFreeAimLock = false;
		/** True if the Player's Rigidbody is unaffected by gravity */
		public bool playerIgnoreGravity = false;

		/** True if a sprite-based Player is locked to face a particular direction */
		public bool playerLockDirection = false;
		/** The direction that a sprite-based Player is currently facing */
		public string playerSpriteDirection = "";
		/** True if a sprite-based Player has its scale locked */
		public bool playerLockScale = false;
		/** The scale of a sprite-based Player */
		public float playerSpriteScale = 0f;
		/** True if a sprite-based Player has its sorting locked */
		public bool playerLockSorting = false;
		/** The sorting order of a sprite-based Player */
		public int playerSortingOrder = 0;
		/** The order in layer of a sprite-based Player */
		public string playerSortingLayer = "";

		/** What Inventory Items (see: InvItem) the player is currently carrying */
		public string inventoryData = "";

		/** If True, the Player is playing a custom animation */
		public bool inCustomCharState = false;
		/** True if the Player's head is facing a Hotspot */
		public bool playerLockHotspotHeadTurning = false;
		/** True if the Player's head is facing a particular object */
		public bool isHeadTurning = false;
		/** The ConstantID number of the head target Transform */
		public int headTargetID = 0;
		/** The Player's head target's X position (offset) */
		public float headTargetX = 0f;
		/** The Player's head target's Y position (offset) */
		public float headTargetY = 0f;
		/** The Player's head target's Z position (offset) */
		public float headTargetZ = 0f;

		/** The Constant ID number of the active _Camera */
		public int gameCamera = 0;
		/** The Constant ID number of the last active _Camera during gameplay */
		public int lastNavCamera = 0;
		/** The Constant ID number of the last active-but-one _Camera during gameplay */
		public int lastNavCamera2 = 0;

		/** The MainCamera's X position */
		public float mainCameraLocX = 0f;
		/** The MainCamera's Y position */
		public float mainCameraLocY = 0f;
		/** The MainCamera's Z position */
		public float mainCameraLocZ = 0f;
		/** The MainCamera's X rotation */
		public float mainCameraRotX = 0f;
		/** The MainCamera's Y rotation */
		public float mainCameraRotY = 0f;
		/** The MainCamera's Z rotation */
		public float mainCameraRotZ = 0f;

		/** True if split-screen is currently active */
		public bool isSplitScreen = false;
		/** True if the gameplay is performed in the top (or left) side during split-screen */
		public bool isTopLeftSplit = false;
		/** True if split-screen is arranged vertically  */
		public bool splitIsVertical = false;
		/** The Constant ID number of the split-screen camera that gameplay is not performed in */
		public int splitCameraID = 0;
		/** During split-screen, the proportion of the screen that the gameplay camera takes up */
		public float splitAmountMain = 0f;
		/** During split-screen, the proportion of the screen that the non-gameplay camera take up */
		public float splitAmountOther = 0f;
		/** The intensity of the current camera shake */
		public float shakeIntensity = 0f;
		/** The duration, in seconds, of the current camera shake */
		public float shakeDuration = 0f;
		/** The int-converted value of CamersShakeEffect */
		public int shakeEffect = 0;
		/** During box-overlay, the size and position of the overlay effect */
		public float overlayRectX, overlayRectY, overlayRectWidth, overlayRectHeight = 0f;

		/** True if the NPC has a FollowSortingMap component that follows the scene's default SortingMap */
		public bool followSortingMap = false;
		/** The ConstantID number of the SortingMap that the NPC's FollowSortingMap follows, if not the scene's default */
		public int customSortingMapID = 0;

		/** The active Document being read */
		public int activeDocumentID = -1;
		/** A record of the Documents collected */
		public string collectedDocumentData = "";
		/** A record of the last-opened page for each viewed Document */
		public string lastOpenDocumentPagesData = "";
		/** A record of the player's current objectives */
		public string playerObjectivesData = "";

		/** The Constant ID number of the non-active-Players's follow target */
		public int followTargetID = 0;
		/** True if the non-active-Players is following the player */
		public bool followTargetIsPlayer = false;
		/** The frequency with which the non-active-Players follows its target */
		public float followFrequency = 0f;
		/** The distance that the non-active-Players keeps with when following its target */
		public float followDistance = 0f;
		/** The maximum distance that the non-active-Players keeps when following its target */
		public float followDistanceMax = 0f;
		/** If True, the non-active-Players will face their follow target when idle */
		public bool followFaceWhenIdle = false;
		/** If True, the non-active-Players will stand a random direction from their target */
		public bool followRandomDirection = false;
		/** If True, and the character is an inactive Player, they can follow the active Player across scenes */
		public bool followAcrossScenes = false;

		/** Data related to the character's left hand Ik state */
		public string leftHandIKState;
		/** Data related to the character's right hand Ik state */
		public string rightHandIKState;
		/** Data related to the character's available sprite directions */
		public string spriteDirectionData;

		/** Save data for any Remember components attached to the Player */
		public List<ScriptData> playerScriptData = new List<ScriptData>();
		/** The Constant ID number of the PlayerStart to appear at when that PlayerStart's scene is next opened */
		public int tempPlayerStart = 0;

		public TeleportPlayerStartMethod tempTeleportPlayerStartMethod;

		#endregion


		#region PublicFunctions

		/**
		 * <summary>Updates the record of the Player's current position</summary>
		 * <param name = "newSceneIndex">The scene in which to place the Player in</param>
		 * <param name = "teleportPlayerStartMethod">How to select which PlayerStart to appear at (SceneDefault, BasedOnPrevious, EnteredHere)</param>
		 * <param name = "playerStartID">The Constant ID value of the PlayerStart for the Player to appear at</param>
		 */
		public void UpdatePosition (int newSceneIndex, TeleportPlayerStartMethod teleportPlayerStartMethod, int playerStartID)
		{
			UpdateCurrentAndShiftPrevious (newSceneIndex);
			OnUpdatePosition (newSceneIndex == SceneChanger.CurrentSceneIndex, teleportPlayerStartMethod, playerStartID);
		}


		/**
		 * <summary>Updates the record of the Player's current position</summary>
		 * <param name = "newSceneName">The scene in which to place the Player in</param>
		 * <param name = "teleportPlayerStartMethod">How to select which PlayerStart to appear at (SceneDefault, BasedOnPrevious, EnteredHere)</param>
		 * <param name = "playerStartID">The Constant ID value of the PlayerStart for the Player to appear at</param>
		 */
		public void UpdatePosition (string newSceneName, TeleportPlayerStartMethod teleportPlayerStartMethod, int playerStartID)
		{
			UpdateCurrentAndShiftPrevious (newSceneName);
			OnUpdatePosition (newSceneName == SceneChanger.CurrentSceneName, teleportPlayerStartMethod, playerStartID);
		}


		/**
		 * <summary>Updates the record of the Player's current position to the current scene</summary>
		 * <param name = "teleportPlayerStartMethod">How to select which PlayerStart to appear at (SceneDefault, BasedOnPrevious, EnteredHere)</param>
		 * <param name = "playerStart">The PlayerStart for the Player to appear at</param>
		 */
		public void UpdatePosition (TeleportPlayerStartMethod teleportPlayerStartMethod, PlayerStart playerStart)
		{
			UpdateCurrentAndShiftPrevious (SceneChanger.CurrentSceneIndex);
			UpdateCurrentAndShiftPrevious (SceneChanger.CurrentSceneName);

			tempPlayerStart = 0;

			if (teleportPlayerStartMethod == TeleportPlayerStartMethod.SceneDefault)
			{
				playerStart = KickStarter.sceneSettings.defaultPlayerStart;
			}
			else if (teleportPlayerStartMethod == TeleportPlayerStartMethod.BasedOnPrevious)
			{
				playerStart = KickStarter.sceneSettings.GetPlayerStart (playerID);
			}

			UpdatePositionFromPlayerStart (playerStart);
		}


		/**
		 * <summary>Copies the record of the Player's current position from another PlayerData instance</summary>
		 * <param name = "playerData">The PlayerData class instance to copy from</param>
		 */
		public void CopyPosition (PlayerData playerData)
		{
			UpdateCurrentAndShiftPrevious (playerData.currentScene);
			UpdateCurrentAndShiftPrevious (playerData.currentSceneName);
			
			tempPlayerStart = 0;

			playerLocX = playerData.playerLocX;
			playerLocY = playerData.playerLocY;
			playerLocZ = playerData.playerLocZ;
			playerRotY = playerData.playerRotY;

			gameCamera = 0;
		}


		/**
		 * <summary>Updates the record of the Player's current position, based on data made when their scene was not active</summary>
		 * <param name = "sceneInstance">The scene instance of the Player to affect</param>
		 */
		public void UpdateFromTempPosition (Player sceneInstance)
		{
			if (tempPlayerStart == 0)
			{
				return;
			}

			PlayerStart playerStart = null;

			switch (tempTeleportPlayerStartMethod)
			{
				case TeleportPlayerStartMethod.SceneDefault:
					playerStart = KickStarter.sceneSettings.defaultPlayerStart;
					break;

				case TeleportPlayerStartMethod.BasedOnPrevious:
					playerStart = KickStarter.sceneSettings.GetPlayerStart (playerID);
					break;

				case TeleportPlayerStartMethod.EnteredHere:
					// Search the scene the Player is in, in case the character is in a sub-scene
					Scene sceneToSearch = (sceneInstance.gameObject.IsPersistent ())
										  ? SceneChanger.CurrentScene
										  : sceneInstance.gameObject.scene;
					playerStart = ConstantID.GetComponent <PlayerStart> (tempPlayerStart, sceneToSearch);
					break;
			}

			UpdatePositionFromPlayerStart (playerStart);
		}


		/** Updates the Player's presence in the scene. According to the data set in this class, they will be added to or removed from the scene. */
		public void UpdatePresenceInScene ()
		{
			PlayerPrefab playerPrefab = KickStarter.settingsManager.GetPlayerPrefab (playerID);
			if (playerPrefab != null)
			{
				if (KickStarter.saveSystem.CurrentPlayerID == playerID)
				{
					playerPrefab.SpawnInScene (false);
				}
				else if ((KickStarter.settingsManager.referenceScenesInSave == ChooseSceneBy.Name && SceneChanger.CurrentSceneName == currentSceneName) ||
						 (KickStarter.settingsManager.referenceScenesInSave == ChooseSceneBy.Number && SceneChanger.CurrentSceneIndex == currentScene))
				{
					playerPrefab.SpawnInScene (false);
				}
				else
				{
					SubScene subScene = null;
					
					switch (KickStarter.settingsManager.referenceScenesInSave)
					{
						case ChooseSceneBy.Name:
							subScene = KickStarter.sceneChanger.GetSubScene (currentSceneName);
							break;

						case ChooseSceneBy.Number:
						default:
							subScene = KickStarter.sceneChanger.GetSubScene (currentScene);
							break;
					}

					if (subScene != null)
					{
						playerPrefab.SpawnInScene (subScene.gameObject.scene);
					}
					else
					{
						playerPrefab.RemoveFromScene ();
					}
				}
			}
		}


		public void SpawnIfFollowingActive ()
		{
			if (KickStarter.saveSystem.CurrentPlayerID != playerID &&
				followTargetIsPlayer &&
				followAcrossScenes)
			{
				switch (KickStarter.settingsManager.referenceScenesInSave)
				{
					case ChooseSceneBy.Name:
						if (currentSceneName == SceneChanger.CurrentSceneName)
						{
							return;
						}
						break;

					case ChooseSceneBy.Number:
					default:
						if (currentScene == SceneChanger.CurrentSceneIndex)
						{
							return;
						}
						break;
				}

				ClearPathData ();
				UpdatePosition (SceneChanger.CurrentSceneIndex, TeleportPlayerStartMethod.BasedOnPrevious, 0);
				UpdatePresenceInScene ();
			}
		}


		/** Clears all data related to current pathfinding and movement */
		public void ClearPathData ()
		{
			playerPathData = string.Empty;
			playerActivePath = 0;
			lastPlayerActivePath = 0;
		}


		/**
		 * <summary>Updates the internal record of the player's current scene</summary>
		 * <param name = "newSceneIndex">The index of the new scene</param>
		 */
		public void UpdateCurrentAndShiftPrevious (int newSceneIndex)
		{
			if (currentScene != newSceneIndex)
			{
				previousScene = currentScene;
				currentScene = newSceneIndex;
			}
		}


		/**
		 * <summary>Updates the internal record of the player's current scene</summary>
		 * <param name = "newSceneIndex">The index of the new scene</param>
		 */
		public void UpdateCurrentAndShiftPrevious (string newSceneName)
		{
			if (currentSceneName != newSceneName)
			{
				previousSceneName = currentSceneName;
				currentSceneName = newSceneName;
			}
		}

		#endregion


		#region PrivateFunctions

		private void OnUpdatePosition (bool inScene, TeleportPlayerStartMethod teleportPlayerStartMethod, int playerStartID)
		{
			tempPlayerStart = 0;
			if (inScene)
			{
				// Updating position to the current scene
				PlayerStart playerStart = null;

				switch (teleportPlayerStartMethod)
				{
					case TeleportPlayerStartMethod.BasedOnPrevious:
						playerStart = KickStarter.sceneSettings.GetPlayerStart (playerID);
						break;

					case TeleportPlayerStartMethod.EnteredHere:
						if (playerStartID != 0)
						{
							playerStart = ConstantID.GetComponent<PlayerStart> (playerStartID);
						}
						break;

					case TeleportPlayerStartMethod.SceneDefault:
						playerStart = KickStarter.sceneSettings.defaultPlayerStart;
						break;

					default:
						break;
				}

				if (playerStart)
				{
					UpdatePositionFromPlayerStart (playerStart);
				}
				else if (teleportPlayerStartMethod == TeleportPlayerStartMethod.EnteredHere && playerStartID != 0)
				{
					ACDebug.LogWarning ("Cannot find PlayerStart with Constant ID = " + playerStartID + " for Player ID = " + playerID + " in the current scene.");
				}
				else
				{
					ACDebug.LogWarning ("Cannot find suitable PlayerStart for Player ID = " + playerID + " in the current scene");
				}
			}
			else
			{
				// Position is being set in another scene, so keep a record of it
				tempTeleportPlayerStartMethod = teleportPlayerStartMethod;
				tempPlayerStart = (teleportPlayerStartMethod == TeleportPlayerStartMethod.EnteredHere) ? playerStartID : -1;
			}
		}


		private void UpdatePositionFromPlayerStart (PlayerStart playerStart)
		{
			if (playerStart)
			{
				tempPlayerStart = 0;

				playerLocX = playerStart.Position.x;
				playerLocY = playerStart.Position.y;
				playerLocZ = playerStart.Position.z;
				playerRotY = playerStart.ForwardAngle;

				gameCamera = 0;

				if (playerStart.cameraOnStart)
				{
					ConstantID cameraID = playerStart.cameraOnStart.GetComponent<ConstantID> ();
					if (cameraID)
					{
						gameCamera = cameraID.constantID;
					}
					else
					{
						ACDebug.LogWarning ("Cannot set Player ID = " + playerID + "'s active camera because " + playerStart.cameraOnStart + " has no ConstantID component.", playerStart.cameraOnStart);
					}
				}
				else if (KickStarter.sceneSettings.defaultPlayerStart && KickStarter.sceneSettings.defaultPlayerStart.cameraOnStart)
				{
					ConstantID cameraID = KickStarter.sceneSettings.defaultPlayerStart.cameraOnStart.GetComponent<ConstantID> ();
					if (cameraID)
					{
						gameCamera = cameraID.constantID;
					}
					else
					{
						ACDebug.LogWarning ("Cannot set Player ID = " + playerID + "'s active camera because " + playerStart.cameraOnStart + " has no ConstantID component.", playerStart.cameraOnStart);
					}
				}
				else
				{
					ACDebug.LogWarning ("Cannot set Player ID = " + playerID + "'s active camera because PlayerStart " + playerStart.name + " has no Camera On Start with a ConstantID component.", playerStart);
				}
			}
		}

		#endregion


		#region GetSet

		/** The Player's position data */
		public Vector3 PlayerPosition
		{
			get
			{
				return new Vector3 (playerLocX, playerLocY, playerLocZ);
			}
		}


		/** The Player's rotation data */
		public Quaternion PlayerRotation
		{
			get
			{
				return Quaternion.AngleAxis (playerRotY, Vector3.up);
			}
		}

		#endregion


		#if UNITY_EDITOR

		public void ShowGUI ()
		{
			try
			{
				CustomGUILayout.MultiLineLabelGUI ("Player ID:", playerID.ToString ());

				EditorGUILayout.LabelField ("Scene info:");
				if (KickStarter.settingsManager && KickStarter.settingsManager.referenceScenesInSave == ChooseSceneBy.Name)
				{
					if (!string.IsNullOrEmpty (currentSceneName)) CustomGUILayout.MultiLineLabelGUI ("   Current:", currentSceneName.ToString ());
					if (!string.IsNullOrEmpty (previousSceneName)) CustomGUILayout.MultiLineLabelGUI ("   Previous:", previousSceneName.ToString ());
					if (!string.IsNullOrEmpty (openSubSceneNames)) CustomGUILayout.MultiLineLabelGUI ("   Sub-scenes:", openSubSceneNames.ToString ());
				}
				else
				{
					CustomGUILayout.MultiLineLabelGUI ("   Current:", currentScene.ToString ());
					CustomGUILayout.MultiLineLabelGUI ("   Previous:", previousScene.ToString ());
					CustomGUILayout.MultiLineLabelGUI ("   Sub-scenes:", openSubScenes);
				}
				if (tempPlayerStart != 0)
				{
					CustomGUILayout.MultiLineLabelGUI ("   PlayerStart ID:", tempPlayerStart.ToString ());
					CustomGUILayout.MultiLineLabelGUI ("   PlayerStart method:", tempTeleportPlayerStartMethod.ToString ());
				}
				
				EditorGUILayout.LabelField ("Movement:");
				CustomGUILayout.MultiLineLabelGUI ("   Position:", "(" + playerLocX.ToString () + ", " + playerLocY.ToString () + ", " + playerLocZ.ToString () + ")");
				CustomGUILayout.MultiLineLabelGUI ("   Rotation:", playerRotY.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Walk speed:", playerWalkSpeed.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Run speed:", playerRunSpeed.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Is running?", playerIsRunning.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Up locked?", playerUpLock.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Down locked?", playerDownLock.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Left locked?", playerLeftlock.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Right locked?", playerRightLock.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Run locked?", playerRunLock.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Free-aim locked?", playerFreeAimLock.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Ignore gravity?", playerIgnoreGravity.ToString ());

				EditorGUILayout.LabelField ("Animation:");
				CustomGUILayout.MultiLineLabelGUI ("   Idle animation:", playerIdleAnim);
				CustomGUILayout.MultiLineLabelGUI ("   Walk animation:", playerWalkAnim);
				CustomGUILayout.MultiLineLabelGUI ("   Talk animation:", playerTalkAnim);
				CustomGUILayout.MultiLineLabelGUI ("   Run animation:", playerRunAnim);
				CustomGUILayout.MultiLineLabelGUI ("   In custom state?", inCustomCharState.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Left hand IK:", leftHandIKState);
				CustomGUILayout.MultiLineLabelGUI ("   Right hand IK:", rightHandIKState);

				EditorGUILayout.LabelField ("Sound:");
				CustomGUILayout.MultiLineLabelGUI ("   Walk sound:", playerWalkSound);
				CustomGUILayout.MultiLineLabelGUI ("   Run sound:", playerRunSound);

				EditorGUILayout.LabelField ("Speech:");
				CustomGUILayout.MultiLineLabelGUI ("   Portrait graphic:", playerPortraitGraphic);
				CustomGUILayout.MultiLineLabelGUI ("   Speech label:", playerSpeechLabel);
				CustomGUILayout.MultiLineLabelGUI ("   Speech label ID:", playerDisplayLineID.ToString ());

				EditorGUILayout.LabelField ("Pathfinding:");
				CustomGUILayout.MultiLineLabelGUI ("   Target node:", playerTargetNode.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Previous node:", playerPrevNode.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Path data:", playerPathData);
				CustomGUILayout.MultiLineLabelGUI ("   Locked to path?", playerLockedPath.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Active path:", playerActivePath.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Path affects Y?", playerPathAffectY.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Last target node:", lastPlayerTargetNode.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Last previous node:", lastPlayerPrevNode.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Last active path:", lastPlayerActivePath.ToString ());

				EditorGUILayout.LabelField ("Sprites:");
				CustomGUILayout.MultiLineLabelGUI ("   Lock direction?", playerLockDirection.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Sprite direction:", playerSpriteDirection);
				CustomGUILayout.MultiLineLabelGUI ("   Scale locked?", playerLockScale.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Sprite scale:", playerSpriteScale.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Lock sorting?", playerLockSorting.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Sorting order:", playerSortingOrder.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Sorting layer:", playerSortingLayer);
				CustomGUILayout.MultiLineLabelGUI ("   Follow default Sorting Map?", followSortingMap.ToString ());
				if (!followSortingMap)
				{
					CustomGUILayout.MultiLineLabelGUI ("   Sorting map?", customSortingMapID.ToString ());
				}

				EditorGUILayout.LabelField ("Inventory:");
				CustomGUILayout.MultiLineLabelGUI ("   Items:", inventoryData);
				CustomGUILayout.MultiLineLabelGUI ("   Active Document:", activeDocumentID.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Collected Documents:", collectedDocumentData.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Last-open Document pages", lastOpenDocumentPagesData.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Objectives:", playerObjectivesData.ToString ());

				EditorGUILayout.LabelField ("Head-turning:");
				CustomGUILayout.MultiLineLabelGUI ("   Head facing Hotspot?", playerLockHotspotHeadTurning.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Head turning?", isHeadTurning.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Head target:", headTargetID.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Head target position:", "(" + headTargetX + ", " + headTargetY + ", " + headTargetZ + ")");

				EditorGUILayout.LabelField ("Camera:");
				CustomGUILayout.MultiLineLabelGUI ("   Camera:", gameCamera.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Last nav cam:", lastNavCamera.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Last nav cam 2:", lastNavCamera2.ToString ());
				CustomGUILayout.MultiLineLabelGUI ("   Camera position:", "(" + mainCameraLocX + ", " + mainCameraLocY + ", " + mainCameraLocZ + ")");
				CustomGUILayout.MultiLineLabelGUI ("   Camera rotation:", "(" + mainCameraRotX + ", " + mainCameraRotY + ", " + mainCameraRotZ + ")");
				CustomGUILayout.MultiLineLabelGUI ("   Split-screen?", isSplitScreen.ToString ());
				if (isSplitScreen)
				{
					CustomGUILayout.MultiLineLabelGUI ("   Top-left split?", isTopLeftSplit.ToString ());
					CustomGUILayout.MultiLineLabelGUI ("   Vertical split?", splitIsVertical.ToString ());
					CustomGUILayout.MultiLineLabelGUI ("   Split camera:", splitCameraID.ToString ());
					CustomGUILayout.MultiLineLabelGUI ("   Split amount main:", splitAmountMain.ToString ());
					CustomGUILayout.MultiLineLabelGUI ("   Split amount other:", splitAmountOther.ToString ());
					CustomGUILayout.MultiLineLabelGUI ("   Overlay rect:", "(" + overlayRectX + ", " + overlayRectY + ", " + overlayRectWidth + ", " + overlayRectHeight + ")");
				}
				CustomGUILayout.MultiLineLabelGUI ("   Shake intensity:", shakeIntensity.ToString ());
				if (shakeIntensity > 0f)
				{
					CustomGUILayout.MultiLineLabelGUI ("   Shake duration", shakeDuration.ToString ());
					CustomGUILayout.MultiLineLabelGUI ("   Shake effect:", ((CameraShakeEffect)shakeEffect).ToString ());
				}

				if (playerScriptData != null && playerScriptData.Count > 0)
				{
					EditorGUILayout.LabelField ("Remember data:");
					foreach (ScriptData scriptData in playerScriptData)
					{
						RememberData rememberData = SaveSystem.FileFormatHandler.DeserializeObject<RememberData> (scriptData.data);
						if (rememberData != null)
						{
							CustomGUILayout.MultiLineLabelGUI ("   " + rememberData.GetType ().ToString () + ":", EditorJsonUtility.ToJson (rememberData, true));
						}
					}
				}
			}
			catch (Exception e)
			{
				ACDebug.LogWarning ("Error displaying player data. Exception: " + e);
			}
		}

		#endif

	}

}