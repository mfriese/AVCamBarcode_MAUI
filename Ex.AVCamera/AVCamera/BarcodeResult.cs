namespace AVCamBarcode
{
    public class BarcodeResult
	{
		public BarcodeResult(string code)
		{
			Barcode = code;
		}

		public string Barcode { get; }
	}
}

