using System.Collections.Generic;
using UnityEngine;

namespace HealthBars.UserInterface.MenuOptions {
	public class MenuOptionResetToDefaults : RadicalMenuOption {
		public override void OnActivated() {
			base.OnActivated();
			
			Manager.menu.centerPopUpText.StartNewDisplaySequence(
				"HealthBars-Options/ResetToDefaultsDesc",
				null,
				menuInputCooldown: true,
				fadeTime: 0f,
				staticTime: 1.5f,
				useUnscaledTime: true,
				yPosition: 0f,
				textBackgroundAlpha: 1f,
				localize: true,
				TextManager.FontFace.boldMedium,
				response => {
					if (response.IsCancel)
						return;

					Options.Instance.SetDefaults();

					MenuOptionColorSlider.TemporarilyPreventInteraction();
				},
				options: new List<string> { "cancelDialogue", "yes" },
				minWidth: 10f,
				backgroundAlpha: 0.9f,
				pauseGame: false
			);
		}
	}
}