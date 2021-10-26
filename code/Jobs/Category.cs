using Sandbox;
using System.Collections.Generic;

namespace SCP
{

	[Library( "category" ), AutoGenerate]
	public partial class Category : Asset
	{

		[Property]
		public string ID { get; set; }
		[Property]
		public string DisplayName { get; set; }
		[Property]
		public string Color { get; set; }



		public static IReadOnlyList<Category> All => _all;
		internal static List<Category> _all = new();

		protected override void PostLoad()
		{
			base.PostLoad();
			if ( !_all.Contains( this ) )
				_all.Add( this );
		}

		public static Category GetByClassName( string className )
		{

			foreach ( Category category in Category.All )
			{
				if ( category.ID == className )
				{
					return category;
				}
			}
			return null;
		}

	}
}



