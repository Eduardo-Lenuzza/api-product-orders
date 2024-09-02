# Desafio NEO - API de Ordens de Produção em C#

## Objetivo

O objetivo deste projeto é desenvolver uma API em C# para gerenciar ordens de produção e seus apontamentos. A API deve permitir o carregamento de dados de ordens de produção e apontamentos de um arquivo Excel, além de fornecer funcionalidades para manipulação e consulta desses dados.

## Funcionalidades da API

A API implementada oferece as seguintes funcionalidades:

1. **Criar um Objeto de Ordem de Produção**  
   Criação de objetos de ordens de produção com campos especificados no arquivo Excel fornecido.

2. **Carregar Dados de Ordens**  
   - Carregar dados de ordens de produção a partir de um arquivo Excel.
   - Atribuir um ID incremental (`OrderId`) para cada combinação única de `OrderNumber` e `OperationNumber`.
   - Retornar o número de objetos importados.

3. **Carregar Dados de Apontamentos**  
   - Carregar dados de apontamentos de produção a partir de um arquivo Excel.
   - Retornar o número de registros importados e o número de `OrderId`s únicas importadas.

4. **Buscar uma Ordem de Produção**  
   Permitir a pesquisa de uma `OrderId` específica e retornar todos os dados associados a ela.

5. **Limpeza de Dados**  
   Remover todos os dados carregados na memória.

## Regras de Negócio

1. **Exclusão de Ordens**  
   Ordens com quantidade apontada maior ou igual à quantidade original devem ser deletadas.  
   A API retorna o número de operações deletadas e quantas ordens restam.

2. **Atualização de Ordens**  
   Ordens com soma de quantidade apontada inferior à quantidade total devem ter suas quantidades atualizadas.  
   A API retorna o número de ordens que foram atualizadas.

3. **Falha de Apontamento**  
   Ordens presentes no Excel de apontamentos, mas ausentes no Excel de ordens, devem ser listadas como "Falha de Apontamento".

## Pré-requisitos

- **Visual Studio**: Desenvolva e teste o projeto utilizando o Visual Studio.
- **.NET Core**: Certifique-se de ter o SDK do .NET Core instalado para compilar e executar a API.

## Como Executar o Projeto

1. Clone o repositório para o seu ambiente local.

   ```bash
   git clone https://github.com/seu-usuario/nome-do-repositorio.git
