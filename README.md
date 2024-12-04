# FIAP_TC System

## Visão Geral

O **FIAP_TC System** é uma aplicação modular construída em .NET 8, projetada para gerenciar `Atendimentos` e `Contatos`. O sistema integra APIs, serviços em background e frameworks de testes, garantindo escalabilidade e manutenção. Ele utiliza ferramentas modernas como RabbitMQ para mensageria e Prometheus para métricas.
## Funcionalidades

- **Gestão de Atendimentos**: API para criar, atualizar, deletar e recuperar atendimentos.
- **Gestão de Contatos**: API para gerenciar contatos, incluindo validações personalizadas para números de telefone e e-mails.
- **Processamento em Background**: Gerencia tarefas assíncronas utilizando RabbitMQ.
- **Configuração de Banco de Dados**: Usa Entity Framework Core com SQL Server.
- **Pipeline CI**: Testes automatizados e build com GitHub Actions.
- **Suporte a Kubernetes**: Arquivos de configuração para deploy em clusters Kubernetes.
- **Monitoramento**: Integração com Prometheus para métricas de requisições HTTP e serviços.

## Estrutura de Diretórios

```plaintext
├── Consumer                   # Serviço de background que consome mensagens do RabbitMQ
├── FIAP_TC.Case.Api           # API de Atendimentos
├── FIAP_TC.Case.Core          # Entidades centrais e DTOs
├── FIAP_TC.Case.Infrastructure# Configuração de infraestrutura e banco de dados
├── FIAP_TC.Case.Tests         # Testes unitários para a API de Atendimentos
├── FIAP_TC.Contact.Api        # API de Contatos
├── FIAP_TC.Contact.Core       # Entidades e DTOs relacionadas a Contatos
├── FIAP_TC.Contact.Infrastructure # Configuração de banco e repositório de Contatos
├── FIAP_TC.Contact.Tests      # Testes unitários para a API de Contatos
├── k8s-configs                # Arquivos de deploy para Kubernetes
```
## Pré-requisitos

Antes de configurar e rodar o projeto, certifique-se de ter os seguintes softwares e ferramentas instalados no seu ambiente:

- [.NET 8 SDK](https://dotnet.microsoft.com/) - Para desenvolvimento e execução do projeto.
- [Docker](https://www.docker.com/) - Para containerização e execução de dependências.
- [RabbitMQ](https://www.rabbitmq.com/) - Para mensageria.
- [Prometheus](https://prometheus.io/) - Para monitoramento e coleta de métricas.
- [Kubernetes](https://kubernetes.io/) - Para orquestração e deploy de containers.
- [Minikube](https://minikube.sigs.k8s.io/docs/) - Para executar clusters Kubernetes localmente (opcional para testes locais).
- **Ferramentas de Teste**:
  - [Moq](https://github.com/moq/moq4) - Para criação de mocks em testes unitários.
  - [xUnit](https://xunit.net/) - Framework de testes unitários.
  - [Coverlet](https://github.com/coverlet-coverage/coverlet) - Para coleta de métricas de cobertura de código.


---


## Deploy e Testes Locais com Kubernetes

### Configuração do Kubernetes com Minikube

1. **Inicie o Minikube**:
   Certifique-se de que o Minikube está instalado e inicialize o cluster local:
   ```bash
   minikube start
   ```

2. **Suba as Imagens Locais com Docker Compose**:
   Execute o `docker-compose up` para construir as imagens Docker necessárias:
   ```bash
   docker-compose up --build
   ```

3. **Carregue as Imagens no Minikube**:
   Após criar as imagens com o Docker Compose, carregue-as para o ambiente do Minikube:
   ```bash
   minikube image load contact-api:latest
   minikube image load case-api:latest
   minikube image load consumer:latest
   ```

4. **Crie um Namespace**:
   Para organizar os recursos no cluster, crie um namespace:
   ```bash
   kubectl create namespace fiap-tc
   ```

5. **Aplique os Manifests do Kubernetes**:
   Navegue até o diretório `k8s-configs` e aplique os arquivos YAML para criar os deployments e serviços:
   ```bash
   kubectl apply -f k8s-configs/ -n fiap-tc
   ```

6. **Verifique os Pods e Serviços**:
   Certifique-se de que os pods estão rodando corretamente:
   ```bash
   kubectl get pods -n fiap-tc
   ```

   Verifique os serviços expostos:
   ```bash
   kubectl get services -n fiap-tc
   ```

7. **Acesse Todos os Serviços**:
   Use o seguinte comando para expor e acessar todos os serviços do namespace:
   ```bash
   minikube service --all -n fiap-tc
   ```

   Esse comando abrirá automaticamente os serviços disponíveis no navegador ou mostrará os endereços de acesso.

8. **Monitoramento e Logs**:
   - Verifique os logs de um pod específico:
     ```bash
     kubectl logs nome-do-pod -n fiap-tc
     ```

9. **Finalize o Cluster**:
   Quando terminar, você pode parar o Minikube para liberar recursos:
   ```bash
   minikube stop
   ```

