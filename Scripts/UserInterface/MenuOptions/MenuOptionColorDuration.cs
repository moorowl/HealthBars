using HealthBars.Utilities;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionColorDuration : MenuOptionColorSlider {
		protected override HsvColor CurrentColor {
			get => Options.Instance.ColorDuration;
			set => Options.Instance.ColorDuration = value;
		}
	}
}