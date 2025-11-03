using Azure.Messaging.ServiceBus;

namespace Microsservice_AzureServiceBus.HostedServices
{
    public class ProductTopicConsumer1 : IHostedService
    {
        private readonly IConfiguration _config;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        private const string TOPIC_NAME = "stores";
        private const string SUBSCRIPTION_NAME = "stores-sub1";

        // Construtor recebe a configuração via injeção de dependência
        public ProductTopicConsumer1(IConfiguration config)
        {
            _config = config;
        }

        // Método chamado quando o serviço é iniciado
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Obtém a connection string da seção ConnectionStrings            
            string? connectionString = _config.GetConnectionString("AzureServiceBus");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A connection string do Azure Service Bus não foi encontrada.");
            }

            // Configurações do cliente Service Bus - usa WebSockets para compatibilidade com firewalls
            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets // Usa WebSockets para comunicação
            };

            // Cria o cliente do Service Bus
            _client = new ServiceBusClient(connectionString, clientOptions);

            // Configurações do processador de mensagens - define concorrência e controle de confirmação
            var processorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 5, // Processa até 5 mensagens simultaneamente
                AutoCompleteMessages = false // Controla manualmente a confirmação das mensagens
            };

            // Cria o processador para uma ASSINATURA de um TÓPICO
            _processor = _client.CreateProcessor(TOPIC_NAME, SUBSCRIPTION_NAME, processorOptions);

            // Define os handlers para processamento de mensagens e erros
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            // Inicia o processamento das mensagens
            await _processor.StartProcessingAsync(cancellationToken);
        }

        // Método chamado quando o serviço é parado
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_processor != null)
            {
                await _processor.StopProcessingAsync(cancellationToken);
                await _processor.DisposeAsync();
            }

            if (_client != null)
            {
                await _client.DisposeAsync();
            }
        }

        // Handler chamado para cada mensagem recebida da fila
        private static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                string body = args.Message.Body.ToString();
                Console.WriteLine($"Mensagem recebida: {body}");

                // Se tudo deu certo, completa a mensagem e a remove da fila
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");

                // AÇÃO CORRETA: Envia a mensagem para a Dead-Letter Queue
                await args.DeadLetterMessageAsync(args.Message, ex.Message, "Erro inesperado durante o processamento");
            }
        }

        // Handler chamado em caso de erro no processamento
        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Erro: {args.Exception.Message}");
            Console.WriteLine($"Entidade: {args.EntityPath}");
            Console.WriteLine($"Origem do erro: {args.ErrorSource}");
            return Task.CompletedTask;
        }
    }
}
