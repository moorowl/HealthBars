using System.Collections.Generic;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionShowCreatureHealth : MenuOptionCycling<bool> {
		protected override List<bool> AvailableOptions => new() {
			true,
			false
		};

		protected override bool CurrentOption {
			get => Options.Instance.ShowCreatureHealth;
			set => Options.Instance.ShowCreatureHealth = value;
		}
		
		protected override void UpdateText() {
			valueText.Render(CurrentOption ? "on" : "off");
		}
	}
}