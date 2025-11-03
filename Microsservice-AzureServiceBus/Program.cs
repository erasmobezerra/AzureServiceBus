using Azure.Messaging.ServiceBus;
using Microsservice_AzureServiceBus.HostedServices;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AzureServiceBus");

// Registra o ServiceBusClient como Singleton para ser reutilizado pela aplicação
builder.Services.AddSingleton<ServiceBusClient>(sp =>
    new ServiceBusClient(connectionString));

// Registra os serviços hospedados que consomme mensagens do Service Bus
builder.Services.AddHostedService<ProductQueueConsumer>();
builder.Services.AddHostedService<ProductTopicConsumer1>();
builder.Services.AddHostedService<ProductTopicConsumer2>();
builder.Services.AddHostedService<ProductTopicConsumer3>();

// Adiciona os serviços ao contêiner de dependências
builder.Services.AddControllers();

// Adiciona suporte para documentação da API com Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Mapeia os controllers para as rotas
app.MapControllers();

// Inicia a aplicação
await app.RunAsync();