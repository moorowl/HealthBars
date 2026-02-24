using System.Collections.Generic;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionShowOreBoulderProgress : MenuOptionCycling<bool> {
		protected override List<bool> AvailableOptions => new() {
			true,
			false
		};

		protected override bool CurrentOption {
			get => Options.Instance.ShowOreBoulderProgress;
			set => Options.Instance.ShowOreBoulderProgress = value;
		}
		
		protected override void UpdateText() {
			valueText.Render(CurrentOption ? "on" : "off");
		}
	}
}