using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Drawing;

namespace RLC.RemoteDesktop
{
	/// <summary>
	/// Screen capturing service.
	/// </summary>
	[ServiceContract(SessionMode=SessionMode.Required)]
	public interface IRemoteDesktop
	{
		/// <summary>
		/// Capture the screen data.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		byte[] UpdateScreenImage();

		/// <summary>
		/// Capture the cursor data.
		/// </summary>
		/// <returns></returns>
		[OperationContract]
		byte[] UpdateCursorImage();
	}
}
