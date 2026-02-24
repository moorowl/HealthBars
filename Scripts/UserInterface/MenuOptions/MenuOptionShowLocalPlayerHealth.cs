using System.Collections.Generic;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionShowLocalPlayerHealth : MenuOptionCycling<bool> {
		protected override List<bool> AvailableOptions => new() {
			true,
			false
		};

		protected override bool CurrentOption {
			get => Options.Instance.ShowLocalPlayerHealth;
			set => Options.Instance.ShowLocalPlayerHealth = value;
		}
		
		protected override void UpdateText() {
			valueText.Render(CurrentOption ? "on" : "off");
		}
	}
}