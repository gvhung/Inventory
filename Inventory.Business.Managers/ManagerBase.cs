using System;
using System.ServiceModel;
using Inventory.Data.Contracts.DTOs;

namespace Inventory.Business.Managers
{
	public class ManagerBase
	{
		protected T ExecuteFaultHandledOperation<T>(Func<T> codeToExecute)
		{
			try
			{
				return codeToExecute.Invoke();
			}
			catch (FaultException ex)
			{
				throw ex;
			}
			catch (Exception ex)
			{
				throw new FaultException(ex.Message);
			}
		}

		protected void ExecuteFaultHandledOperation(Action codeToExecute)
		{
			try
			{
				codeToExecute.Invoke();
			}
			catch (FaultException ex)
			{
				throw ex;
			}
			catch (Exception ex)
			{
				throw new FaultException(ex.Message);
			}
		}
	}
}