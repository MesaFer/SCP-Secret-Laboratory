using Sandbox;
using Sandbox.UI;
using SCP;

namespace SCP
{
	public partial class ScpHUD : RootPanel
	{
		public ScpHUD()
		{
			AddChild<Vitals>();

			AddChild<NameTags>();
			AddChild<CrosshairCanvas>();
			AddChild<ChatBox>();
			AddChild<VoiceList>();
			AddChild<KillFeed>();

			AddChild<Scoreboard<ScoreboardEntry>>();
		}
	}
}

