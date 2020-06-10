using PdfSharp.Xamarin.Forms.Delegates;
using PdfSharpCore.Drawing;
using Xamarin.Forms;

namespace _3Guards_app
{
    class TimingListRendererDelegate : PdfListViewRendererDelegate
	{
		public override void DrawCell(ListView listView, int section, int row, XGraphics page, XRect bounds, double scaleFactor)
		{
			base.DrawCell(listView, section, row, page, bounds, scaleFactor);
		}

		public override void DrawFooter(ListView listView, int section, XGraphics page, XRect bounds, double scaleFactor)
		{
			base.DrawFooter(listView, section, page, bounds, scaleFactor);
		}

		public override double GetFooterHeight(ListView listView, int section)
		{
			return base.GetFooterHeight(listView, section);
		}
	}
}
