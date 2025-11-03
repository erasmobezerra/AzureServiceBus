using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsservice_AzureServiceBus.Models;
using System.Text.Json;

namespace Microsservice_AzureServiceBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly IConfiguration _config;
        private const string QUEUE_NAME = "product";
        private const string TOPIC_NAME = "stores";

        public ProductController(ServiceBusClient serviceBusClient, IConfiguration config)
        {
            _serviceBusClient = serviceBusClient;
            _config = config;
        }

        // Endpoint para envio imediato da mensagem
        [HttpPost]
        public async Task<IActionResult> PostQueue([FromBody] Product product)
        {
            product.id = Guid.NewGuid();
            await SendMessageAsync(product, QUEUE_NAME);
            return Ok( new
            {
                Message = "Produto enviado com sucesso!",
                Product = product
            });
        }

        // ENDPOINT para agendar mensagens
        [HttpPost("schedule-queue")]
        public async Task<IActionResult> PostScheduledQueue([FromBody] ScheduledProductRequest scheduledProduct)
        {
            var product = scheduledProduct.Product;
            product.id = Guid.NewGuid();

            // Calcula a data e hora do agendamento em UTC
            var scheduledTime = DateTimeOffset.UtcNow.AddMinutes(scheduledProduct.MinutesToSchedule);

            // Envia a mensagem com a data de agendamento
            await SendMessageAsync(product, QUEUE_NAME, scheduledTime);

            // Retorna uma resposta indicando que o produto foi agendado
            return Accepted(new
            {
                Message = "Produto agendado com sucesso!",
                ScheduledTimeUtc = scheduledTime,
                Product = product
            });
        }

        // NOVO ENDPOINT para PUBLICAR mensagens no TÓPICO
        [HttpPost("topic")]
        public async Task<IActionResult> PostTopic([FromBody] Product product)
        {
            product.id = Guid.NewGuid();
            await SendMessageAsync(product, TOPIC_NAME); 
            return Accepted(new 
            { 
                Message = "Produto publicado com sucesso no tópico!", 
                Product = product 
            });
        }


        // Método para envio de mensagem que aceita um parâmetro opcional de agendamento
        private async Task SendMessageAsync(Product product, string queueOrTopicName, DateTimeOffset? scheduledTime = null)
        {
            ServiceBusSender sender = _serviceBusClient.CreateSender(queueOrTopicName);

            // Serializa o produto para JSON e cria a mensagem do Service Bus
            string messageBody = JsonSerializer.Serialize(product);
            ServiceBusMessage message = new ServiceBusMessage(messageBody);

            // Se uma data de agendamento foi fornecida, definimos a propriedade na mensagem
            if (scheduledTime.HasValue)
                message.ScheduledEnqueueTime = scheduledTime.Value;
                       
            await sender.SendMessageAsync(message); // Envia a mensagem para a fila
            await sender.DisposeAsync(); // Garante que o sender seja descartado após o uso
            
        }
    }
}