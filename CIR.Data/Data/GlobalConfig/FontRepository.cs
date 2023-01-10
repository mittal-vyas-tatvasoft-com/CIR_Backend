using CIR.Common.Data;
using CIR.Core.Interfaces.GlobalConfig;

namespace CIR.Data.Data.GlobalConfig
{
	public class FontRepository : IFontRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _CIRDBContext;
		#endregion

		#region CONSTRUCTOR
		public FontRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS

		//public async Task<Fonts> GetAllFonts()
		//{
		//	var fonts = await _CIRDBContext.Fonts
		//}
		#endregion


	}
}
