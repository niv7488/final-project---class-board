using System.ServiceModel;

namespace RLC.RemoteDesktop
{
	[ServiceContract]
	public interface IViewerService
	{
		[OperationContract]
		void PushScreenUpdate(byte[] data);

		[OperationContract]
		string PushCursorUpdate(byte[] data);

		[OperationContract]
		string Ping();
	}
}
