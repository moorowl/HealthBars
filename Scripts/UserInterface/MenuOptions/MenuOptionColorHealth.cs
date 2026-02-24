using HealthBars.Utilities;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionColorHealth : MenuOptionColorSlider {
		protected override HsvColor CurrentColor {
			get => Options.Instance.ColorHealth;
			set => Options.Instance.ColorHealth = value;
		}
	}
}