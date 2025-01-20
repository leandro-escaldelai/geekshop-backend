using Microsoft.Extensions.Logging;
using OrderJob.Services;
using MessageBus;

namespace OrderJob.Queues
{

	public class CreatedOrderQueue(
		IOrderService service,
		ILogger logger) : IQueue<int>
	{

		public const string Name = "created_order";

		public async Task Process(int data)
		{
			logger.LogInformation($"Processing created order, id {data}");

			await Task.CompletedTask;
		}

	}

}
