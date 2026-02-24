using HealthBars.Utilities;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionColorMana : MenuOptionColorSlider {
		protected override HsvColor CurrentColor {
			get => Options.Instance.ColorMana;
			set => Options.Instance.ColorMana = value;
		}
	}
}