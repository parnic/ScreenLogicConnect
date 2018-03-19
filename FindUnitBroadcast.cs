namespace ScreenLogicConnect
{
    public class FindUnitBroadcast
    {
        public static readonly sbyte[] data = new sbyte[]
        {
            ByteHelper.getLowWordLowByte(1),
            ByteHelper.getLowWordHighByte(1),
            ByteHelper.getHighWordLowByte(1),
            ByteHelper.getHighWordHighByte(1),
            0,
            0,
            0,
            0,
        };

        public FindUnitBroadcast()
        {
        }
    }
}
