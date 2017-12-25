using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Brainiac.Design
{
	public sealed class Font
	{
		private readonly Typeface _typeface;
		private readonly double _size;

		public Font(string fontFamily, FontWeight fontWeight, FontStyle fontStyle,  double size)
		{
			_typeface= new Typeface( new FontFamily(fontFamily), fontStyle, fontWeight, FontStretches.Normal );
			_size= size;
		}

		public Font(string fontFamily, bool bold, double size)
		{
			_typeface= new Typeface( new FontFamily(fontFamily), FontStyles.Normal, bold ? FontWeights.Bold : FontWeights.Normal, FontStretches.Normal );
			_size= size;
		}

		public FormattedText FormatText(string text, Brush color)
		{
			return new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, _typeface, _size, color);
		}
	}
}
