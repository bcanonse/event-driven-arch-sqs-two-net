using Amazon.Runtime;
using Amazon.SQS;
using EventTwo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var sqsClient = new AmazonSQSClient(
    new BasicAWSCredentials(
        builder.Configuration.GetSection("AWS:AccessKeyID").Value,
        builder.Configuration.GetSection("AWS:SecretAccessKey").Value
    ), Amazon.RegionEndpoint.USEast2
);

builder.Services.AddSingleton<IAmazonSQS>(sqsClient);

builder.Services.AddHostedService<SQSBackgroundService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
