namespace Microsservice_AzureServiceBus.Models
{
    public class ScheduledProductRequest
    {
        public Product Product { get; set; }
        public int MinutesToSchedule { get; set; } // Quantos minutos no futuro a mensagem deve ser entregue
    }
}
