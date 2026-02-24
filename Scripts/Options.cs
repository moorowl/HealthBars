using System;
using System.Text;
using HealthBars.UserInterface;
using HealthBars.Utilities;
using Newtonsoft.Json;
using PugMod;
using UnityEngine;

namespace HealthBars {
	public class Options {
		public static Options Instance { get; private set; } = new();
		
		private const string FilePath = Main.InternalName + "/Options.json";
		private const int CurrentVersion = 0;
		private const float AutosaveInterval = 10f;

		public bool ShowCreatureHealth {
			get => _data.ShowCreatureHealth;
			set {
				_data.ShowCreatureHealth = value;
				_isDirty = true;
			}
		}
		
		public bool ShowOtherPlayerHealth {
			get => _data.ShowOtherPlayerHealth;
			set {
				_data.ShowOtherPlayerHealth = value;
				_isDirty = true;
			}
		}
		
		public bool ShowLocalPlayerHealth {
			get => _data.ShowLocalPlayerHealth;
			set {
				_data.ShowLocalPlayerHealth = value;
				_isDirty = true;
			}
		}
		
		public bool ShowMinionDuration {
			get => _data.ShowMinionDuration;
			set {
				_data.ShowMinionDuration = value;
				_isDirty = true;
			}
		}
		
		public bool ShowOreBoulderProgress {
			get => _data.ShowOreBoulderProgress;
			set {
				_data.ShowOreBoulderProgress = value;
				_isDirty = true;
			}
		}
		
		public bool DisplayOverObjects {
			get => _data.DisplayOverObjects;
			set {
				_data.DisplayOverObjects = value;
				_isDirty = true;
			}
		}
		
		public bool EmphasizeLargeHits {
			get => _data.EmphasizeLargeHits;
			set {
				_data.EmphasizeLargeHits = value;
				_isDirty = true;
			}
		}
		
		public HsvColor ColorHealth {
			get => _data.ColorHealth;
			set {
				if (!_data.ColorHealth.Equals(value))
					_isDirty = true;
				_data.ColorHealth = value;
			}
		}
		
		public HsvColor ColorMana {
			get => _data.ColorMana;
			set {
				if (!_data.ColorMana.Equals(value))
					_isDirty = true;
				_data.ColorMana = value;
			}
		}
		
		public HsvColor ColorShield {
			get => _data.ColorShield;
			set {
				if (!_data.ColorShield.Equals(value))
					_isDirty = true;
				_data.ColorShield = value;
			}
		}
		
		public HsvColor ColorDuration {
			get => _data.ColorDuration;
			set {
				if (!_data.ColorDuration.Equals(value))
					_isDirty = true;
				_data.ColorDuration = value;
			}
		}

		private OptionsData _data;
		private bool _hasInit;
		private bool _isDirty;
		private float _lastSavedTime;
		
		private void SetData(OptionsData data) {
			_data = data;
			_isDirty = false;
		}
		
		public void Init() {
			MenuAdder.OnInit += () => {
				var menu = MenuAdder.AddMenu((RadicalOptionsMenu) Manager.menu.uiOptionsMenu, 19901, "HealthBars-Options/Header");
				menu.AddOptionFromPath(Main.AssetBundle, $"Assets/{Main.InternalName}/Prefabs/MenuOptions.prefab");
			};
			
			Load();
			_hasInit = true;
		}

		public void Update() {
			if (!_hasInit)
				return;
			
			if (_isDirty && Time.unscaledTime > _lastSavedTime + AutosaveInterval)
				Save();
		}

		public void SetDefaults() {
			SetData(new OptionsData());
			Save();
		}
		
		public void Save() {
			if (!API.ConfigFilesystem.DirectoryExists(Main.InternalName))
				API.ConfigFilesystem.CreateDirectory(Main.InternalName);
			
			try {
				var serializedData = JsonConvert.SerializeObject(_data);
				API.ConfigFilesystem.Write(FilePath, Encoding.UTF8.GetBytes(serializedData));
			} catch (Exception ex) {
				Debug.Log($"[{Main.DisplayName}]: Error while saving options");
				Debug.LogException(ex);
			}
			
			_isDirty = false;
			_lastSavedTime = Time.unscaledTime;
		}
		
		private void Load() {
			if (!API.ConfigFilesystem.FileExists(FilePath)) {
				SetDefaults();
				return;
			}
			
			try {
				var deserializedData = JsonConvert.DeserializeObject<OptionsData>(Encoding.UTF8.GetString(API.ConfigFilesystem.Read(FilePath)));
				SetData(deserializedData);
			} catch (Exception ex) {
				Debug.Log($"[{Main.DisplayName}]: Error while loading options, using defaults");
				Debug.LogException(ex);
				SetDefaults();
			}
		}
		
		private class OptionsData {
			public int Version { get; set; } = CurrentVersion;
			public bool ShowCreatureHealth { get; set; } = true;
			public bool ShowOtherPlayerHealth { get; set; } = true;
			public bool ShowLocalPlayerHealth { get; set; }
			public bool ShowMinionDuration { get; set; }
			public bool ShowOreBoulderProgress { get; set; }
			public bool DisplayOverObjects { get; set; } = true;
			public bool EmphasizeLargeHits { get; set; }
			public HsvColor ColorHealth { get; set; } = new(0f, 0.77f, 1f);
			public HsvColor ColorMana { get; set; } = new(0.58f, 1f, 0.9f);
			public HsvColor ColorShield { get; set; } = new(0.15f, 1f, 0.9f);
			public HsvColor ColorDuration { get; set; } = new(0.58f, 1f, 0.9f);
		}
	}
}