# 🚀 Microsserviço com Azure Service Bus

Este projeto foi desenvolvido no curso da DIO chamado "Introdução a Conceitos de Mensageria e Service Bus com Azure" e demonstra a implementação de um microsserviço em .NET 8 utilizando Azure Service Bus para mensageria, ilustrando dois padrões comuns de comunicação assíncrona: Filas (Queues) e Tópicos (Topics).

## ☁️ Sobre o Azure Service Bus

O Azure Service Bus é um serviço de mensageria empresarial totalmente gerenciado pela Microsoft Azure. Ele permite a comunicação assíncrona entre aplicações e serviços, oferecendo dois modelos principais:

### 📩 Filas (Queues)

- Comunicação ponto a ponto  
- Uma mensagem é enviada por um produtor e consumida por apenas um consumidor  
- Garante entrega FIFO (First-In-First-Out)  
- Ideal para:  
  - Balanceamento de carga  
  - Desacoplamento entre serviços  
  - Processamento assíncrono de tarefas  

### 📡 Tópicos (Topics) e Assinaturas

- Modelo de publicação/assinatura (pub/sub)  
- Uma mensagem é publicada em um tópico e pode ser consumida por múltiplos assinantes  
- Cada assinante recebe uma cópia da mensagem  
- Ideal para:  
  - Distribuição de eventos  
  - Cenários de broadcast  
  - Múltiplos consumidores interessados na mesma mensagem  

## 🧱 Estrutura do Projeto

O projeto contém:

- `ProductController`: API REST para envio de mensagens  
- `ProductQueueConsumer`: Consumidor de mensagens da fila  
- `ProductTopicConsumer1`, `ProductTopicConsumer2`, `ProductTopicConsumer3`: Consumidores de mensagens do tópico  
- `Models`: Classes de domínio e DTOs  

## ⚙️ Configuração e Execução

### 🧰 Pré-requisitos

- .NET 8.0 SDK  
- Uma conta Azure com um namespace do Service Bus configurado  
- Visual Studio 2022 ou VS Code  

### 🔧 Configuração do Azure Service Bus

1. Crie um namespace do Azure Service Bus no portal Azure  
2. Crie uma fila chamada "product"  
3. Crie um tópico chamado "stores" com três assinaturas: "stores-sub1", "stores-sub2", "stores-sub3"  
4. Copie a string de conexão do namespace  

### 💻 Configuração Local

1. Clone o repositório:  
```powershell
git clone https://github.com/erasmobezerra/Microsservice-AzureServiceBus.git
cd Microsservice-AzureServiceBus
```

2. Configure a string de conexão no `appsettings.json`  

3. Execute o projeto:  
```powershell
dotnet restore
dotnet run
```

## 🌐 Acessar Swagger UI

Abra o navegador e acesse:

```
http://localhost:7000/api/swagger/ui
```

Você verá uma interface gerada automaticamente com base nas definições OpenAPI onde poderá realizar os testes das requisições!

---

## 🔗 Endpoints da API

- `POST /api/product/queue`: Envia mensagem para uma fila  
- `POST /api/product/topic`: Envia mensagem para um tópico  
- `POST /api/product/schedule`: Agenda uma mensagem para envio futuro  

---

🙏 Agradeço profundamente à **Digital Innovation One** por proporcionar este aprendizado gratuito e de qualidade. Um reconhecimento especial ao professor **[Leonardo Buta](https://www.linkedin.com/in/leonardo-buta/)** pela excelente didática e orientação durante todo o processo.

<div align="center">
  <p>⭐ Se este projeto foi útil para você, considere dar uma estrela!</p>
</div>

