using System.Collections.Generic;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionShowMinionDuration : MenuOptionCycling<bool> {
		protected override List<bool> AvailableOptions => new() {
			true,
			false
		};

		protected override bool CurrentOption {
			get => Options.Instance.ShowMinionDuration;
			set => Options.Instance.ShowMinionDuration = value;
		}
		
		protected override void UpdateText() {
			valueText.Render(CurrentOption ? "on" : "off");
		}
	}
}