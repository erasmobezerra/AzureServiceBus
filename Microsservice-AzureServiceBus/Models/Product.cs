namespace Microsservice_AzureServiceBus.Models
{
    // Modelo que representa um produto enviado/recebido via Service Bus
    public class Product
    {
        // Identificador único do produto
        public Guid id { get; set; }
        // Nome do produto
        public string Name { get; set; }
        // Preço do produto
        public double Price { get; set; }
    }
}
