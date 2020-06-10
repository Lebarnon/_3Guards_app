using PdfSharpCore.Pdf;

namespace _3Guards_app
{
	public interface IPdfSave
	{
		void Save(PdfDocument doc, string fileName);
	}
}
