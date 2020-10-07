namespace ExtractDx11MESH.TXGHs
{
	public class TXGH03 : TXGH01
	{
		public TXGH03(byte[] fileData, int iPos)
			: base(fileData, iPos)
		{
		}

		protected override void ReadTextureMeta()
		{
			iPos += 16;
			iPos += 4;
			iPos += 4;
			iPos += 46;
		}
	}
}
