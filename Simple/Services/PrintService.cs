namespace Simple.Services
{
	public interface IPrintService
	{
		string Print();
	}
	public class PrintService : IPrintService
	{
		public string Print() => "Hello from SinjulMSBH .. !!!!";
	}
}
