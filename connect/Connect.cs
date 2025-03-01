namespace ConnectLib
{
	public class Connect
	{
		private readonly int _originId;
		private readonly int _destinationId;

		public Connect(int originId, int destinationId)
		{
			_originId = originId;
			_destinationId = destinationId;
		}
		
		public int GetOrigin() => _originId;
		public int GetDestination() => _destinationId;
	}
}
