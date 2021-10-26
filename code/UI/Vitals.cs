using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace SCP
{
	public partial class Vitals : Panel
	{
		private readonly Label Health;
		private readonly Panel HealthBar;

		private readonly Label Armor;
		private readonly Panel ArmorBar;

		private readonly Label Stam;
		private readonly Panel StamBar;

		public Vitals()
		{


			StyleSheet.Load( "/ui/Vitals.scss" );
			Panel vitalsBack = Add.Panel( "vitalsBack" );

			// Health
			Panel healthBarBack = vitalsBack.Add.Panel( "healthBarBack" );
			HealthBar = healthBarBack.Add.Panel( "healthBar" );

			healthBarBack.Add.Label( "favorite", "healthIcon" );

			Health = healthBarBack.Add.Label( "0", "healthText" );

			// Armor
			Panel armorBarBack = vitalsBack.Add.Panel( "armorBarBack" );
			ArmorBar = armorBarBack.Add.Panel( "armorBar" );

			armorBarBack.Add.Label( "shield", "armorIcon" );

			Armor = armorBarBack.Add.Label( "0", "armorText" );
			// Stamina
			Panel stamBarBack = vitalsBack.Add.Panel( "stamBarBack" );
			StamBar = stamBarBack.Add.Panel( "stamBar" );


			stamBarBack.Add.Label( "directions_run", "stamIcon" );


			var player = Local.Pawn as ScpPlayer;
			if ( player == null ) return;
		}

		public override void Tick()
		{
			base.Tick();

			var player = Local.Pawn as ScpPlayer;
			if ( player == null ) return;


			Health.Text = $"{player.Health.CeilToInt()}";

			HealthBar.Style.Dirty();
			HealthBar.Style.Width = Length.Percent( player.Health );

			Armor.Text = $"{player.Armor.CeilToInt()}";

			ArmorBar.Style.Dirty();
			ArmorBar.Style.Width = Length.Percent( player.Armor );

			StamBar.Style.Dirty();
			StamBar.Style.Width = Length.Percent( player.Stamina );
		}
	}
}

