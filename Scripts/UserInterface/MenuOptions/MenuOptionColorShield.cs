using HealthBars.Utilities;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionColorShield : MenuOptionColorSlider {
		protected override HsvColor CurrentColor {
			get => Options.Instance.ColorShield;
			set => Options.Instance.ColorShield = value;
		}
	}
}