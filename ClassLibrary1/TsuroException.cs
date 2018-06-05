using System;
namespace tsuro
{
	// Exceptions related to Tsuro rules
	public class TsuroException : Exception
    {
		public TsuroException(string message) : base(message)
        {
        }
    }

	public class XMLTagOrderException : Exception
    {
		public XMLTagOrderException(string message) : base(message)
        {
        }
    }
}
