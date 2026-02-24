using System.Collections.Generic;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionDisplayOverObjects : MenuOptionCycling<bool> {
		protected override List<bool> AvailableOptions => new() {
			true,
			false
		};

		protected override bool CurrentOption {
			get => Options.Instance.DisplayOverObjects;
			set => Options.Instance.DisplayOverObjects = value;
		}
		
		protected override void UpdateText() {
			valueText.Render(CurrentOption ? "on" : "off");
		}
	}
}