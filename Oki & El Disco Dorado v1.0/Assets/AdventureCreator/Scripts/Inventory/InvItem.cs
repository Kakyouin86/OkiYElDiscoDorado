/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2021
 *	
 *	"InvItem.cs"
 * 
 *	This script is a container class for individual inventory items.
 * 
 */

using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	/**
	 * A data class for an Inventory item.
	 * Items are stored in InventoryManager, and referenced in InvInstance classes when added to the Player's inventory at runtime.
	 */
	[System.Serializable]
	public class InvItem : ITranslatable
	{

		#region Variables

		/** The item's Editor name */
		public string label;
		/** A unique identifier */
		public int id;
		/** The item's in-game name, if not label */
		public string altLabel;

		/** If True, the Player carries the item when the game begins */
		public bool carryOnStart;
		/** If True, then a Player prefab that is not the default carries the item when the game begins (if playerSwitching = PlayerSwitching.Allow in SettingsManager) */
		public bool carryOnStartNotDefault;
		/** The ID numbers of the Player prefabs that carry the item when the game begins, if carryOnStartNotDefault = True */
		public List<int> carryOnStartIDs = new List<int>();

		/** The item's properties */
		public List<InvVar> vars = new List<InvVar>();

		/** If True, then multiple instances of the item can be carried at once */
		public bool canCarryMultiple;
		/** The number of instances being carried, if canCarryMultiple = True */
		public int count;

		/** If True, the item's label will be lower-cased when placed in the middle of a Hotspot label if it is not at the start. */
		public bool canBeLowerCase = false;
		/** The maximum amount that can be held in a single slot */
		public int maxCount = 999;

		/** How to select items when multiple are in a given slot */
		public ItemStackingMode itemStackingMode = ItemStackingMode.All;

		/** The item's main graphic */
		public Texture tex;
		/** The item's 'highlighted' graphic */
		public Texture activeTex;
		/** The item's 'selected' graphic (if SettingsManager's selectInventoryDisplay = SelectInventoryDisplay.ShowSelectedGraphic) */
		public Texture selectedTex;
		/** A GameObject that can be associated with the item, for the creation of e.g. 3D inventory items (through scripting only) */
		public GameObject linkedPrefab;
		/** A CursorIcon instance that, if assigned, will be used in place of the 'tex' Texture when the item is selected on the cursor */
		public CursorIcon cursorIcon = new CursorIcon ();
		/** The translation ID number of the item's name, as generated by SpeechManager */
		public int lineID = -1;
		/** The ID number of the CursorIcon (in CursorManager's cursorIcons List) to show when hovering over the item if appropriate */
		public int useIconID = 0;
		/** The ID number of the item's InvBin category, as defined in InventoryManager */
		public int binID;
		
		/** If True, then the item has its own "Use X on Y" syntax when selected */
		public bool overrideUseSyntax = false;
		/** The "Use" in "Use X on Y", if overrideUseSyntax = True */
		public HotspotPrefix hotspotPrefix1 = new HotspotPrefix ("Use");
		/** The "on" on "Use X on Y", if overrideUseSyntax = True */
		public HotspotPrefix hotspotPrefix2 = new HotspotPrefix ("on");

		/** The ActionListAsset to run when the item is used, if multiple interactions are disallowed */
		public ActionListAsset useActionList;
		/** The ActionListAsset to run when the item is examined, if multiple interactions are disallowed */
		public ActionListAsset lookActionList;
		/** A List of all "Use" interactions associated with the item */
		public List<InvInteraction> interactions = new List<InvInteraction>();
		/** A list of all "Combine" interactions associated witht the item */
		public List<InvCombineInteraction> combineInteractions = new List<InvCombineInteraction>();
		[SerializeField] private List<ActionListAsset> combineActionList = new List<ActionListAsset>();
		[SerializeField] private List<int> combineID = new List<int>();
		/** The ActionListAsset to run when using the item on a Hotspot is unhandled */
		public ActionListAsset unhandledActionList;
		/** The ActionListAsset to run when using the item on another InvItem is unhandled */
		public ActionListAsset unhandledCombineActionList;

		// Deprecated
		[SerializeField] private int carryOnStartID;
		[SerializeField] private bool useSeparateSlots;
		[SerializeField] bool selectSingle;

		#if UNITY_EDITOR
		public bool showInFilter;
		private int sideInteraction = -1;
		private int sideCombineInteraction = -1;
		#endif

		#endregion


		#region Constructors

		/**
		 * <summary>The default Constructor.</summary>
		 * <param name = "idArray">An array of already-used ID numbers, so that a unique ID number can be assigned</param>
		 */
		public InvItem (int[] idArray)
		{
			count = 0;
			tex = null;
			activeTex = null;
			selectedTex = null;
			cursorIcon = new CursorIcon ();
			id = 0;
			binID = -1;
			maxCount = 999;
			carryOnStartNotDefault = false;
			vars = new List<InvVar>();
			linkedPrefab = null;
			canBeLowerCase = false;
			itemStackingMode = ItemStackingMode.All;

			interactions = new List<InvInteraction>();
			combineInteractions = new List<InvCombineInteraction>();

			overrideUseSyntax = false;
			hotspotPrefix1 = new HotspotPrefix ("Use");
			hotspotPrefix2 = new HotspotPrefix ("on");

			// Update id based on array
			foreach (int _id in idArray)
			{
				if (id == _id)
					id ++;
			}

			label = "Inventory item " + (id + 1).ToString ();
			altLabel = "";
		}


		/**
		 * <summary>A Constructor in which the id is explicitly set.</summary>
		 * <param name = "_id">The ID number to assign</param>
		 */
		public InvItem (int _id)
		{
			count = 0;
			tex = null;
			activeTex = null;
			selectedTex = null;
			cursorIcon = new CursorIcon ();
			id = _id;
			binID = -1;
			carryOnStartNotDefault = false;
			vars = new List<InvVar>();
			linkedPrefab = null;
			maxCount = 999;
			canBeLowerCase = false;

			itemStackingMode = ItemStackingMode.All;
			interactions = new List<InvInteraction>();
			combineInteractions = new List<InvCombineInteraction>();

			overrideUseSyntax = false;
			hotspotPrefix1 = new HotspotPrefix ("Use");
			hotspotPrefix2 = new HotspotPrefix ("on");

			label = "Inventory item " + (id + 1).ToString ();
			altLabel = "";
		}
		

		/**
		 * <summary>A Constructor that sets all its values by copying another InvItem.</summary>
		 * <param name = "assetItem">The InvItem to copy</param>
		 */
		public InvItem (InvItem assetItem)
		{
			count = assetItem.count;
			tex = assetItem.tex;
			activeTex = assetItem.activeTex;
			selectedTex = assetItem.selectedTex;

			//cursorIcon = assetItem.cursorIcon;
			cursorIcon = new CursorIcon ();
			cursorIcon.Copy (assetItem.cursorIcon, true);

			carryOnStart = assetItem.carryOnStart;
			carryOnStartNotDefault = assetItem.carryOnStartNotDefault;
			carryOnStartID = assetItem.carryOnStartID;
			carryOnStartIDs.Clear ();
			foreach (int _id in assetItem.carryOnStartIDs)
			{
				carryOnStartIDs.Add (_id);
			}
			Upgrade ();
			canCarryMultiple = assetItem.canCarryMultiple;
			label = assetItem.label;
			altLabel = assetItem.altLabel;
			id = assetItem.id;
			lineID = assetItem.lineID;
			useIconID = assetItem.useIconID;

			canBeLowerCase = assetItem.canBeLowerCase;

			binID = assetItem.binID;
			if (binID == -1 && KickStarter.inventoryManager && KickStarter.inventoryManager.bins != null && KickStarter.inventoryManager.bins.Count > 0)
			{
				// Place item in first available cateogry if undefined
				binID = KickStarter.inventoryManager.bins[0].id;
			}

			maxCount = assetItem.maxCount;
			itemStackingMode = assetItem.itemStackingMode;

			overrideUseSyntax = assetItem.overrideUseSyntax;
			hotspotPrefix1 = assetItem.hotspotPrefix1;
			hotspotPrefix2 = assetItem.hotspotPrefix2;
			
			useActionList = assetItem.useActionList;
			lookActionList = assetItem.lookActionList;
			interactions = assetItem.interactions;
			combineInteractions = assetItem.combineInteractions;
			unhandledActionList = assetItem.unhandledActionList;
			unhandledCombineActionList = assetItem.unhandledCombineActionList;
			linkedPrefab = assetItem.linkedPrefab;

			vars.Clear ();
			foreach (InvVar invVar in assetItem.vars)
			{
				vars.Add (new InvVar (invVar));
			}
			
			if (Application.isPlaying)
			{
				for (int i=0; i<vars.Count; i++)
				{
					if (vars[i].type == VariableType.PopUp)
					{
						// Transfer ID of PopUps translation from global property to per-item
						vars[i].popUpsLineID = KickStarter.inventoryManager.invVars[i].popUpsLineID;
					}
				}
			}
		}

		#endregion


		#if UNITY_EDITOR

		public void ShowGUI (string apiPrefix, List<string> binList)
		{
			Upgrade ();

			label = CustomGUILayout.TextField ("Name:", label, apiPrefix + ".label", "The item's Editor name");
			altLabel = CustomGUILayout.TextField ("Label (if not name):", altLabel, apiPrefix + ".altLabel", "The item's in-game name, if not label");

			bool isPronoun = !canBeLowerCase;
			isPronoun = CustomGUILayout.Toggle ("Name is pronoun?", isPronoun, "!" + apiPrefix + ".canBeLowerCase", "If False, the name will be lower-cased when inside sentences.");
			canBeLowerCase = !isPronoun;

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField (new GUIContent ("Category:", "The category that the item belongs to"), GUILayout.Width (146f));
			if (KickStarter.inventoryManager.bins.Count > 0)
			{
				int binNumber = KickStarter.inventoryManager.GetBinSlot (binID);
				binNumber = CustomGUILayout.Popup (binNumber, binList.ToArray (), apiPrefix + ".binID");
				binID = KickStarter.inventoryManager.bins[binNumber].id;
			}
			else
			{
				binID = -1;
				EditorGUILayout.LabelField ("No categories defined!", EditorStyles.miniLabel, GUILayout.Width (146f));
			}
			EditorGUILayout.EndHorizontal ();

			carryOnStart = CustomGUILayout.Toggle ("Carry on start?", carryOnStart, apiPrefix + ".carryOnStart", "If True, the Player carries the item when the game begins");
			if (carryOnStart && AdvGame.GetReferences ().settingsManager && AdvGame.GetReferences ().settingsManager.playerSwitching == PlayerSwitching.Allow && !AdvGame.GetReferences ().settingsManager.shareInventory)
			{
				carryOnStartNotDefault = CustomGUILayout.Toggle ("Non-default Player(s)?", carryOnStartNotDefault, apiPrefix + ".carryOnStartNotDefault", "If True, then a Player prefab that is not the default carries the item when the game begins");
				if (carryOnStartNotDefault)
				{
					carryOnStartIDs = ChoosePlayerGUI (carryOnStartIDs, apiPrefix + ".carryOnStartID");
				}
			}

			canCarryMultiple = CustomGUILayout.Toggle ("Can carry multiple?", canCarryMultiple, apiPrefix + ".canCarryMultiple", "If True, then multiple instances of the item can be carried at once");

			if (carryOnStart && canCarryMultiple)
			{
				count = CustomGUILayout.IntField ("Quantity on start:", count, apiPrefix + ".count", "The number of instances that the player is carrying when the game begins");
			}
			else
			{
				count = 1;
			}

			if (canCarryMultiple)
			{
				if (maxCount == 0)
				{
					maxCount = 999;
				}
				maxCount = CustomGUILayout.IntField ("Slot capacity:", maxCount, apiPrefix + ".maxCount", "The number of instances of the item that can occupy a single inventory slot");
				if (maxCount < 1)
				{
					maxCount = 1;
				}

				if (maxCount > 1)
				{
					itemStackingMode = (ItemStackingMode) CustomGUILayout.EnumPopup ("Selection mode:", itemStackingMode, apiPrefix + ".itemStackingMode", "How to select items when multiple are in a given slot");
				}
			}

			overrideUseSyntax = CustomGUILayout.Toggle ("Override 'Use' syntax?", overrideUseSyntax, apiPrefix + ".overrideUseSyntax", "If True, then the item has its own 'Use X on Y' syntax when selected");
			if (overrideUseSyntax)
			{
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Use syntax:", GUILayout.Width (100f));
				hotspotPrefix1.label = EditorGUILayout.TextField (hotspotPrefix1.label, GUILayout.MaxWidth (80f));
				EditorGUILayout.LabelField ("(item)", GUILayout.MaxWidth (40f));
				hotspotPrefix2.label = EditorGUILayout.TextField (hotspotPrefix2.label, GUILayout.MaxWidth (80f));
				EditorGUILayout.LabelField ("(hotspot)", GUILayout.MaxWidth (55f));
				EditorGUILayout.EndHorizontal ();
			}

			linkedPrefab = (GameObject) CustomGUILayout.ObjectField<GameObject> ("Linked prefab:", linkedPrefab, false, apiPrefix + ".linkedPrefab", "A GameObject that can be associated with the item, for the creation of e.g. 3D inventory items (through scripting only)");
			if (linkedPrefab != null)
			{
				EditorGUILayout.HelpBox ("This reference is accessible through scripting, or via Inventory parameter in the 'Object: Add or remove' Action.", MessageType.Info);
			}

			CustomGUILayout.DrawUILine ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField (new GUIContent ("Main graphic:", "The item's main graphic"), GUILayout.Width (145));
			tex = (Texture) CustomGUILayout.ObjectField<Texture> (tex, false, GUILayout.Width (70), GUILayout.Height (70), apiPrefix + ".tex");
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField (new GUIContent ("Active graphic:", "The item's 'highlighted' graphic"), GUILayout.Width (145));
			activeTex = (Texture) CustomGUILayout.ObjectField<Texture> (activeTex, false, GUILayout.Width (70), GUILayout.Height (70), apiPrefix + ".activeTex");
			EditorGUILayout.EndHorizontal ();

			if (AdvGame.GetReferences ().settingsManager != null && AdvGame.GetReferences ().settingsManager.selectInventoryDisplay == SelectInventoryDisplay.ShowSelectedGraphic)
			{
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField (new GUIContent ("Selected graphic:", "The item's 'selected' graphic"), GUILayout.Width (145));
				selectedTex = (Texture) CustomGUILayout.ObjectField<Texture> (selectedTex, false, GUILayout.Width (70), GUILayout.Height (70), apiPrefix + ".selectedTex");
				EditorGUILayout.EndHorizontal ();
			}
			if (AdvGame.GetReferences ().cursorManager != null)
			{
				CursorManager cursorManager = AdvGame.GetReferences ().cursorManager;
				if (cursorManager.inventoryHandling == InventoryHandling.ChangeCursor || cursorManager.inventoryHandling == InventoryHandling.ChangeCursorAndHotspotLabel)
				{
					cursorIcon.ShowGUI (true, true, "Cursor (optional):", cursorManager.cursorRendering, apiPrefix + ".cursorIcon", "A Cursor that, if assigned, will be used in place of the 'tex' Texture when the item is selected on the cursor");
					CustomGUILayout.DrawUILine ();
				}
			}

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Standard interactions", CustomStyles.subHeader);
			if (KickStarter.settingsManager && KickStarter.settingsManager.interactionMethod != AC_InteractionMethod.ContextSensitive && KickStarter.settingsManager.inventoryInteractions == InventoryInteractions.Multiple && KickStarter.cursorManager)
			{
				List<string> iconList = new List<string> ();
				foreach (CursorIcon icon in KickStarter.cursorManager.cursorIcons)
				{
					iconList.Add (icon.id.ToString () + ": " + icon.label);
				}

				if (KickStarter.cursorManager.cursorIcons.Count > 0)
				{
					foreach (InvInteraction interaction in interactions)
					{
						int i = interactions.IndexOf (interaction);
						EditorGUILayout.BeginHorizontal ();

						EditorGUILayout.LabelField (interactions[i].ID.ToString () + ":", GUILayout.Width (15f));

						bool enabledOnStart = !interaction.disabledOnStart;
						enabledOnStart = CustomGUILayout.Toggle (enabledOnStart, GUILayout.Width (15f), apiPrefix + ".interactions[" + i + "].disabledOnStart");
						interaction.disabledOnStart = !enabledOnStart;

						int invNumber = GetIconSlot (interaction.icon.id);
						invNumber = EditorGUILayout.Popup (invNumber, iconList.ToArray ());
						interaction.icon = KickStarter.cursorManager.cursorIcons[invNumber];

						string autoName = label + "_" + interaction.icon.label;
						interaction.actionList = ActionListAssetMenu.AssetGUI (string.Empty, interaction.actionList, autoName, apiPrefix + ".interactions[" + i + "].actionList", "The ActionList to run when the interaction is triggered");

						if (GUILayout.Button (string.Empty, CustomStyles.IconCog))
						{
							SideInteractionMenu (interactions.IndexOf (interaction));
						}

						EditorGUILayout.EndHorizontal ();
					}
				}
				else
				{
					EditorGUILayout.HelpBox ("No interaction icons defined - please use the Cursor Manager", MessageType.Warning);
				}
				if (GUILayout.Button ("Add interaction"))
				{
					Undo.RecordObject (KickStarter.inventoryManager, "Add new interaction");
					interactions.Add (new InvInteraction (KickStarter.cursorManager.cursorIcons[0], interactions));
				}
			}
			else
			{
				string autoName = label + "_Use";
				useActionList = ActionListAssetMenu.AssetGUI ("Use:", useActionList, autoName, apiPrefix + ".useActionList", "The ActionList asset to run when using the item is used");
				if (KickStarter.cursorManager && KickStarter.cursorManager.allowInteractionCursorForInventory && KickStarter.cursorManager.cursorIcons.Count > 0)
				{
					int useCursor_int = KickStarter.cursorManager.GetIntFromID (useIconID) + 1;
					if (useIconID == -1) useCursor_int = 0;
					useCursor_int = CustomGUILayout.Popup ("Use cursor icon:", useCursor_int, KickStarter.cursorManager.GetLabelsArray (true), apiPrefix + ".useIconID", "The Cursor to show when hovering over the item");

					if (useCursor_int == 0)
					{
						useIconID = -1;
					}
					else if (KickStarter.cursorManager.cursorIcons.Count > (useCursor_int - 1))
					{
						useIconID = KickStarter.cursorManager.cursorIcons[useCursor_int - 1].id;
					}
				}
				else
				{
					useIconID = 0;
				}
				autoName = label + "_Examine";
				lookActionList = ActionListAssetMenu.AssetGUI ("Examine:", lookActionList, autoName, apiPrefix + ".lookActionList", "The ActionListAsset to run when the item is examined");
			}

			if (KickStarter.settingsManager && KickStarter.settingsManager.CanSelectItems (false))
			{
				EditorGUILayout.Space ();
				EditorGUILayout.LabelField ("Unhandled interactions", CustomStyles.subHeader);
				string autoName = label + "_Unhandled_Hotspot";
				unhandledActionList = ActionListAssetMenu.AssetGUI ("Use on Hotspot:", unhandledActionList, autoName, apiPrefix + ".unhandledActionList", "The ActionList asset to run when using the item on a Hotspot is unhandled");
				autoName = label + "_Unhandled_Combine";
				unhandledCombineActionList = ActionListAssetMenu.AssetGUI ("Combine:", unhandledCombineActionList, autoName, apiPrefix + ".unhandledCombineActionList", "The ActionListAsset to run when using the item on another InvItem is unhandled");
			}

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Combine interactions", CustomStyles.subHeader);
			for (int i = 0; i < combineInteractions.Count; i++)
			{
				EditorGUILayout.BeginHorizontal ();

				EditorGUILayout.LabelField (combineInteractions[i].ID.ToString () + ":", GUILayout.Width (15f));

				bool enabledOnStart = !combineInteractions[i].disabledOnStart;
				enabledOnStart = CustomGUILayout.Toggle (enabledOnStart, GUILayout.Width (15f), apiPrefix + ".combineInteractions[" + i + "].disabledOnStart");
				combineInteractions[i].disabledOnStart = !enabledOnStart;

				int invNumber = KickStarter.inventoryManager.GetArraySlot (combineInteractions[i].combineID);
				invNumber = EditorGUILayout.Popup (invNumber, KickStarter.inventoryManager.GetLabelList ());
				combineInteractions[i].combineID = KickStarter.inventoryManager.items[invNumber].id;

				string autoName = label + "_Combine_" + KickStarter.inventoryManager.GetLabelList ()[invNumber];
				combineInteractions[i].actionList = ActionListAssetMenu.AssetGUI (string.Empty, combineInteractions[i].actionList, autoName, apiPrefix + ".combineInteractions[" + i + "].actionList", "A List of all 'Combine' InvInteraction objects associated with the item");

				if (GUILayout.Button (string.Empty, CustomStyles.IconCog))
				{
					SideCombineInteractionMenu (i);
				}

				EditorGUILayout.EndHorizontal ();
			}
			if (GUILayout.Button ("Add combine event"))
			{
				Undo.RecordObject (KickStarter.inventoryManager, "Add new combine event");
				combineInteractions.Add (new InvCombineInteraction (0, null, combineInteractions));
			}

			// List all "reverse" inventory combinations
			string reverseCombinations = string.Empty;
			foreach (InvItem otherItem in KickStarter.inventoryManager.items)
			{
				if (otherItem != this)
				{
					bool contains = false;
					foreach (InvCombineInteraction otherItemCombineInteraction in otherItem.combineInteractions)
					{
						if (otherItemCombineInteraction.combineID == id)
						{
							contains = true;
							break;
						}
					}

					if (contains)
					{
						reverseCombinations += "- " + otherItem.label + "\n";
						continue;
					}
				}
			}
			if (reverseCombinations.Length > 0)
			{
				EditorGUILayout.Space ();
				EditorGUILayout.HelpBox ("The following inventory items have combine interactions that reference this item:\n" + reverseCombinations, MessageType.Info);
			}

			if (KickStarter.inventoryManager.invVars.Count > 0)
			{
				EditorGUILayout.Space ();
				EditorGUILayout.LabelField ("Properties", CustomStyles.subHeader);

				RebuildProperties ();

				// UI for setting property values
				if (vars.Count > 0)
				{
					foreach (InvVar invVar in vars)
					{
						invVar.ShowGUI (apiPrefix + ".GetProperty (" + invVar.id + ")");
					}
				}
				else
				{
					EditorGUILayout.HelpBox ("No properties have been defined that this inventory item can use.", MessageType.Info);
				}
			}
		}


		private void SideInteractionMenu (int index)
		{
			GenericMenu menu = new GenericMenu ();
			sideInteraction = index;
			
			menu.AddItem (new GUIContent ("Insert after"), false, InteractionCallback, "Insert after");
			menu.AddItem (new GUIContent ("Delete"), false, InteractionCallback, "Delete");
			
			if (sideInteraction > 0 || sideInteraction < interactions.Count - 1)
			{
				menu.AddSeparator (string.Empty);
			}
			if (sideInteraction > 0)
			{
				menu.AddItem (new GUIContent ("Re-arrange/Move to top"), false, InteractionCallback, "Move to top");
				menu.AddItem (new GUIContent ("Re-arrange/Move up"), false, InteractionCallback, "Move up");
			}
			if (sideInteraction < interactions.Count - 1)
			{
				menu.AddItem (new GUIContent ("Re-arrange/Move down"), false, InteractionCallback, "Move down");
				menu.AddItem (new GUIContent ("Re-arrange/Move to bottom"), false, InteractionCallback, "Move to bottom");
			}

			menu.ShowAsContext ();
		}


		private void InteractionCallback (object obj)
		{
			if (sideInteraction >= 0 && sideInteraction < interactions.Count)
			{
				InvInteraction tempInteraction = interactions[sideInteraction];

				switch (obj.ToString ())
				{
					case "Insert after":
						Undo.RecordObject (KickStarter.inventoryManager, "Insert interaction");
						InvInteraction newInteraction = new InvInteraction (KickStarter.cursorManager.cursorIcons[0], interactions);
						interactions.Insert (sideInteraction + 1, newInteraction);
						break;

					case "Delete":
						Undo.RecordObject (KickStarter.inventoryManager, "Delete interaction");
						interactions.RemoveAt (sideInteraction);
						break;

					case "Move up":
						Undo.RecordObject (KickStarter.inventoryManager, "Move interaction up");
						interactions.RemoveAt (sideInteraction);
						interactions.Insert (sideInteraction - 1, tempInteraction);
						break;

					case "Move down":
						Undo.RecordObject (KickStarter.inventoryManager, "Move interaction down");
						interactions.RemoveAt (sideInteraction);
						interactions.Insert (sideInteraction + 1, tempInteraction);
						break;

					case "Move to top":
						Undo.RecordObject (KickStarter.inventoryManager, "Move interaction to top");
						interactions.RemoveAt (sideInteraction);
						interactions.Insert (0, tempInteraction);
						break;

					case "Move to bottom":
						Undo.RecordObject (KickStarter.inventoryManager, "Move interaction to bottom");
						interactions.Add (tempInteraction);
						interactions.RemoveAt (sideInteraction);
						break;

					default:
						break;
				}
			}

			EditorUtility.SetDirty (KickStarter.inventoryManager);
			AssetDatabase.SaveAssets ();

			sideInteraction = -1;
		}


		private void SideCombineInteractionMenu (int index)
		{
			GenericMenu menu = new GenericMenu ();
			sideCombineInteraction = index;

			menu.AddItem (new GUIContent ("Insert after"), false, CombineInteractionCallback, "Insert after");
			menu.AddItem (new GUIContent ("Delete"), false, CombineInteractionCallback, "Delete");
			
			if (sideCombineInteraction > 0 || sideCombineInteraction < combineInteractions.Count - 1)
			{
				menu.AddSeparator (string.Empty);
			}
			if (sideCombineInteraction > 0)
			{
				menu.AddItem (new GUIContent ("Re-arrange/Move to top"), false, CombineInteractionCallback, "Move to top");
				menu.AddItem (new GUIContent ("Re-arrange/Move up"), false, CombineInteractionCallback, "Move up");
			}
			if (sideInteraction < interactions.Count - 1)
			{
				menu.AddItem (new GUIContent ("Re-arrange/Move down"), false, CombineInteractionCallback, "Move down");
				menu.AddItem (new GUIContent ("Re-arrange/Move to bottom"), false, CombineInteractionCallback, "Move to bottom");
			}

			menu.ShowAsContext ();
		}


		private void CombineInteractionCallback (object obj)
		{
			if (sideCombineInteraction >= 0 && sideCombineInteraction < combineInteractions.Count)
			{
				InvCombineInteraction tempInteraction = combineInteractions[sideCombineInteraction];

				switch (obj.ToString ())
				{
					case "Insert after":
						Undo.RecordObject (KickStarter.inventoryManager, "Insert interaction");
						InvCombineInteraction newCombineInteraction = new InvCombineInteraction (0, null, combineInteractions);
						combineInteractions.Insert (sideCombineInteraction + 1, newCombineInteraction);
						break;

					case "Delete":
						Undo.RecordObject (KickStarter.inventoryManager, "Delete interaction");
						combineInteractions.RemoveAt (sideCombineInteraction);
						break;

					case "Move up":
						Undo.RecordObject (KickStarter.inventoryManager, "Move interaction up");
						combineInteractions.RemoveAt (sideCombineInteraction);
						combineInteractions.Insert (sideCombineInteraction - 1, tempInteraction);
						break;

					case "Move down":
						Undo.RecordObject (KickStarter.inventoryManager, "Move interaction down");
						combineInteractions.RemoveAt (sideCombineInteraction);
						combineInteractions.Insert (sideCombineInteraction + 1, tempInteraction);
						break;

					case "Move to top":
						Undo.RecordObject (KickStarter.inventoryManager, "Move interaction to top");
						combineInteractions.RemoveAt (sideCombineInteraction);
						combineInteractions.Insert (0, tempInteraction);
						break;

					case "Move to bottom":
						Undo.RecordObject (KickStarter.inventoryManager, "Move interaction to bottom");
						combineInteractions.Add (tempInteraction);
						combineInteractions.RemoveAt (sideCombineInteraction);
						break;

					default:
						break;
				}
			}

			EditorUtility.SetDirty (KickStarter.inventoryManager);
			AssetDatabase.SaveAssets ();

			sideInteraction = -1;
		}


		private List<int> ChoosePlayerGUI (List<int> playerIDs, string api)
		{
			CustomGUILayout.LabelField ("Item is carried by:", api);

			foreach (PlayerPrefab playerPrefab in KickStarter.settingsManager.players)
			{
				string playerName = "    " + playerPrefab.ID + ": " + ((playerPrefab.playerOb != null) ? playerPrefab.playerOb.GetName () : "(Unnamed)");
				bool isActive = false;
				foreach (int playerID in playerIDs)
				{
					if (playerID == playerPrefab.ID) isActive = true;
				}

				bool wasActive = isActive;
				isActive = EditorGUILayout.Toggle (playerName, isActive);
				if (isActive != wasActive)
				{
					if (isActive)
					{
						playerIDs.Add (playerPrefab.ID);
					}
					else
					{
						playerIDs.Remove (playerPrefab.ID);
					}
				}
			}
			return playerIDs;
		}


		public void RebuildProperties ()
		{
			// Which properties are available?
			List<int> availableVarIDs = new List<int> ();
			foreach (InvVar invVar in KickStarter.inventoryManager.invVars)
			{
				if (!invVar.limitToCategories || KickStarter.inventoryManager.bins.Count == 0 || invVar.categoryIDs.Contains (binID))
				{
					availableVarIDs.Add (invVar.id);
				}
			}

			// Create new properties / transfer existing values
			List<InvVar> newInvVars = new List<InvVar> ();
			foreach (InvVar invVar in KickStarter.inventoryManager.invVars)
			{
				if (availableVarIDs.Contains (invVar.id))
				{
					InvVar newInvVar = new InvVar (invVar);
					InvVar oldInvVar = GetProperty (invVar.id);
					if (oldInvVar != null)
					{
						newInvVar.TransferValues (oldInvVar);
					}
					newInvVar.popUpID = invVar.popUpID;
					newInvVars.Add (newInvVar);
				}
			}

			vars = newInvVars;
		}


		private int GetIconSlot (int iconID)
		{
			int i = 0;
			foreach (CursorIcon icon in AdvGame.GetReferences ().cursorManager.cursorIcons)
			{
				if (icon.id == iconID)
				{
					return i;
				}
				i++;
			}
			return 0;
		}


		public bool ReferencesAsset (ActionListAsset actionListAsset)
		{
			if (KickStarter.settingsManager && KickStarter.settingsManager.InventoryInteractions == InventoryInteractions.Multiple && AdvGame.GetReferences ().cursorManager)
			{
				foreach (InvInteraction interaction in interactions)
				{
					if (interaction.actionList == actionListAsset) return true;
				}
			}
			else
			{
				if (useActionList == actionListAsset) return true;
				if (lookActionList == actionListAsset) return true;
			}

			if (KickStarter.settingsManager && KickStarter.settingsManager.CanSelectItems (false))
			{
				if (unhandledActionList == actionListAsset) return true;
				if (unhandledCombineActionList == actionListAsset) return true;
			}

			foreach (InvCombineInteraction combineInteraction in combineInteractions)
			{
				if (combineInteraction.actionList == actionListAsset)
				{
					return true;
				}
			}

			return false;
		}


		public string EditorLabel
		{
			get
			{
				return id.ToString () + " (" + GetLabel (0) + ")";
			}
		}

		#endif


		#region PublicFunctions

		/** Upgrades the item from a previous version of AC */
		public void Upgrade ()
		{
			if (maxCount < 1)
			{
				maxCount = 999;
			}
			
			if (carryOnStartID >= 0 && carryOnStartIDs.Count == 0)
			{
				carryOnStartIDs.Add (carryOnStartID);
				carryOnStartID = -1;
			}

			if (canCarryMultiple && useSeparateSlots)
			{
				useSeparateSlots = false;
				maxCount = 1;
			}

			if (selectSingle)
			{
				itemStackingMode = ItemStackingMode.Single;
				selectSingle = false;
			}

			if (combineID != null && combineID.Count > 0)
			{
				combineInteractions = new List<InvCombineInteraction>();
				for (int i=0; i<combineID.Count; i++)
				{
					if (i < combineActionList.Count)
					{
						combineInteractions.Add (new InvCombineInteraction (combineID[i], combineActionList[i], combineInteractions));
					}
					else
					{
						combineInteractions.Add (new InvCombineInteraction (combineID[i], null, combineInteractions));
					}
				}

				combineID.Clear ();
				combineActionList.Clear ();
			}

			#if UNITY_EDITOR

			foreach (InvInteraction invInteraction in interactions)
			{
				invInteraction.Upgrade (interactions);
			}

			foreach (InvCombineInteraction combineInteraction in combineInteractions)
			{
				combineInteraction.Upgrade (combineInteractions);
			}

			#endif
		}


		/**
		 * <summary>Checks if the item has an InvInteraction combine interaction for a specific InvItem.</summary>
		 * <param name = "invItem">The InvITem to check for</param>
		 * <returns>True if the item has an InvInteraction combine interaction for the InvItem.</returns>
		 */
		public bool DoesHaveInventoryInteraction (InvItem invItem)
		{
			if (invItem != null)
			{
				foreach (InvCombineInteraction combineInteraction in combineInteractions)
				{
					if (combineInteraction.combineID == invItem.id)
					{
						return true;
					}
				}
			}
			
			return false;
		}


		/**
		 * <summary>Gets the item's display name.</summary>
		 * <param name = "languageNumber">The index of the current language, as set in SpeechManager</param>
		 * <returns>The item's display name</returns>
		 */
		public string GetLabel (int languageNumber)
		{
			if (languageNumber > 0)
			{
				if (!string.IsNullOrEmpty (altLabel))
				{
					return AdvGame.ConvertTokens (KickStarter.runtimeLanguages.GetTranslation (altLabel, lineID, languageNumber, GetTranslationType (0)));
				}
				return AdvGame.ConvertTokens (KickStarter.runtimeLanguages.GetTranslation (label, lineID, languageNumber, GetTranslationType (0)));
			}
			else
			{
				if (!string.IsNullOrEmpty (altLabel))
				{
					return AdvGame.ConvertTokens (altLabel);
				}
			}
			return AdvGame.ConvertTokens (label);
		}


		/**
		 * <summary>Runs one of the item's 'Use' interactions.</summary>
		 * <param name = "iconID">The ID number of the CursorIcon associated with the use interaction. If no number is supplied, the default use interaction will be run.</param>
		 */
		public void RunUseInteraction (int iconID = -1)
		{
			InvInstance newInstance = new InvInstance (this);
			if (iconID < 0)
			{
				newInstance.Use ();
			}
			else
			{
				newInstance.Use (iconID);
			}
		}


		/**
		 * <summary>Runs the item's 'Examine' interaction, if one is defined.</summary>
		 */
		public void RunExamineInteraction ()
		{
			InvInstance newInstance = new InvInstance (this);
			newInstance.Examine ();
		}


		/**
		 * <summary>Combines the item with itself. This is normally only available when combining items via Hotspot-based InventoryBox elements, but this allows it to be enforced.</summary>
		 */
		public void CombineWithSelf ()
		{
			InvInstance newInstance = new InvInstance (this);
			newInstance.Combine (newInstance, true);
		}


		/**
		 * <summary>Combines the item with another.</summary>
		 * <param name = "otherItemID">The ID number of the inventory item to combine with</param>
		 */
		public void CombineWithItem (int otherItemID)
		{
			InvInstance newInstance = new InvInstance (this);
			newInstance.Combine (new InvInstance (otherItemID));
		}


		/**
		 * <summary>Combines the item with another.</summary>
		 * <param name = "otherItem">The inventory item to combine with</param>
		 */
		public void CombineWithItem (InvItem otherItem)
		{
			InvInstance newInstance = new InvInstance (this);
			newInstance.Combine (new InvInstance (otherItem));
		}


		/** Selects the item. */
		public void Select ()
		{
			KickStarter.runtimeInventory.SelectItem (new InvInstance (this));
		}


		/**
		 * <summary>Shows any Menus with appearType = AppearType.OnInteraction, connected to a the InvItem.</summary>
		 */
		public void ShowInteractionMenus ()
		{
			if (KickStarter.playerMenus)
			{
				KickStarter.playerMenus.EnableInteractionMenus (this);
			}
		}


		/** 
		 * <summary>Turns on a specific Menu, linking any Interaction icons in it to the Inventory item. The Menu doesn't need to have an appearType of AppearType.OnInteraction.</summary>
		 * <param name = "menu">The Menu to turn on</param>
		 * <param name = "includeInventoryItems">If True, and supported, then other inventory items associcated with the item's interactions will be included as well</param>
		 */
		public void ShowInteractionMenu (Menu menu, bool includeInventoryItems)
		{
			menu.MatchInteractions (new InvInstance (this), includeInventoryItems);
			menu.TurnOn ();
		}


		/**
		 * <summary>If True, the inventory item has a graphic that can be used by the cursor when selected.</summary>
		 * <returns>True if the inventory item has a graphic that can be used by the cursor when selected.</returns>
		 */
		public bool HasCursorIcon ()
		{
			if (tex || (cursorIcon != null && cursorIcon.texture))
			{
				return true;
			}
			return false;
		}


		/**
		 * <summary>Gets a property of the inventory item.</summary>
		 * <param name = "ID">The ID number of the property to get</param>
		 * <param name = "multiplyByItemCount">If True, then the property's integer/float value will be multipled by the item's count</param>
		 * <returns>The property of the inventory item</returns>
		 */
		public InvVar GetProperty (int ID)
		{
			if (vars.Count > 0 && ID >= 0)
			{
				foreach (InvVar var in vars)
				{
					if (var.id == ID)
					{
						return var;
					}
				}
			}
			return null;
		}

		#endregion


		#region ITranslatable

		public string GetTranslatableString (int index)
		{
			if (index == 0)
			{
				if (!string.IsNullOrEmpty (altLabel))
				{
					return altLabel;
				}
				return label;
			}
			else
			{
				return vars[index-1].TextValue;
			}
		}


		public int GetTranslationID (int index)
		{
			if (index == 0)
			{
				return lineID;
			}
			else
			{
				return vars[index-1].textValLineID;
			}
		}


		public AC_TextType GetTranslationType (int index)
		{
			if (index == 0)
			{
				return AC_TextType.InventoryItem;
			}
			else
			{
				return AC_TextType.InventoryItemProperty;
			}
		}


		#if UNITY_EDITOR

		public void UpdateTranslatableString (int index, string updatedText)
		{
			if (index == 0)
			{
				altLabel = updatedText;
			}
			else if ((index-1) < vars.Count)
			{
				vars[index-1].TextValue = updatedText;
			}
		}


		public int GetNumTranslatables ()
		{
			if (vars != null)
			{
				return vars.Count + 1;
			}
			return 1;
		}


		public bool HasExistingTranslation (int index)
		{
			if (index == 0)
			{
				return lineID > -1;
			}
			else
			{
				return vars[index-1].textValLineID > -1;
			}
		}


		public void SetTranslationID (int index, int _lineID)
		{
			if (index == 0)
			{
				lineID = _lineID;
			}
			else
			{
				vars[index-1].textValLineID = _lineID;
			}
		}


		public string GetOwner (int index)
		{
			return string.Empty;
		}


		public bool OwnerIsPlayer (int index)
		{
			return false;
		}


		public bool CanTranslate (int index)
		{
			if (index == 0)
			{
				if (!string.IsNullOrEmpty (label) || !string.IsNullOrEmpty (altLabel))
				{
					return true;
				}
			}
			else
			{
				if (vars[index-1].type == VariableType.String && !string.IsNullOrEmpty (vars[index-1].TextValue))
				{
					return true;
				}
			}
			return false;
		}

		#endif

		#endregion

	}

}