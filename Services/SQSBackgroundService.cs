
using System.Net;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace EventTwo.Services;

public class SQSBackgroundService(
    IConfiguration configuration,
    IAmazonSQS sqsClient,
    ILogger<SQSBackgroundService> logger
) : BackgroundService
{

    private readonly string QueueUrl = configuration.GetValue<string>("AWS:QueueUrl") ?? string.Empty;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = QueueUrl,
                MaxNumberOfMessages = 10,
            };

            var response = await sqsClient.ReceiveMessageAsync(request, stoppingToken);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                foreach (var message in response.Messages)
                {
                    logger.LogInformation(message.Body);
                }
            }
        }
    }
}