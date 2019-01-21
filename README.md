# Cosmos DB real-time advanced analytics

Woodgrove Bank, who provides payment processing services for commerce, is looking to design and implement a proof-of-concept (PoC) of an innovative fraud detection solution. They want to provide new services to their merchant customers, helping them save costs by applying machine learning and advanced analytics to detect fraudulent transactions. Their customers are around the world, and the right solutions for them would minimize any latencies experienced using their service by distributing as much of the solution as possible, as closely as possible, to the regions in which their customers use the service.

## Target audience

- Application developer
- AI developer
- Data scientist

## Abstract

### Workshop

In this workshop you will learn to design a data pipeline solution that leverages Cosmos DB for both the scalable ingest of streaming data, and the globally distributed serving of both pre-scored data and machine learning models. The solution leverages the Cosmos DB change data feed in concert with the Azure Databricks Delta to enable a modern data warehouse solution that can be used to create risk reduction solutions for scoring transactions for fraud in an offline, batch approach and in a near real-time, request/response approach.

At the end of this workshop, you will be better able to design and implement solutions that leverage the strengths of Cosmos DB in support of advanced analytics solutions that require high throughput ingest, low latency serving and global scale in combination with scalable machine learning, big data and real-time processing capabilities.

#### Outline: Architecture Overview

The workshop explores the components in the following architecture:

![Outline Architecture](./Media/outline-architecture.png 'High-level view of the proposed architecture')

### Whiteboard design session _(this will go in the readme and in the WDS document)_

Woodgrove Bank, who provides payment processing services for commerce, is looking to design and implement a PoC of an innovative fraud detection solution. They want to provide new services to their merchant customers, helping them save costs by applying machine learning and advanced analytics to detect fraudulent transactions. Their customers are around the world, and the right solutions for them would minimize any latencies experienced using their service by distributing as much of the solution as possible, as closely as possible, to the regions in which their customers use the service.

In this whiteboard design session, you will work in a group to design the data pipeline PoC that could support the needs of Woodgrove Bank.

At the end of this workshop, you will be better able to design solutions that leverage the strengths of Cosmos DB in support of advanced analytics solutions that require high throughput ingest, low latency serving and global scale in combination with scalable machine learning, big data and real-time processing capabilities.

#### Outline: Key Concerns for Customer situation

- Globally distributed solution dealing with financial data (purchase transacations)
- Minimizing access latency to globally distributed .data.
- Supporting both real-time and batch fraud scoring
- Providing a unified platform that can support their near term data pipeline needs and provides a long term to standard for their data science, data engineering and developement needs.
- Understanding the components of a data pipeline, when to choose which Azure services for ingest, processing, storage and serving.
- Understanding how to implement a modern data analytics solution using data collected from Event Hubs and Cosmos DB.
- Understanding how to properly leverage Cosmos DB in data pipelines for ingest, supporting analytics, and serving processed data.
- Leveraging Cosmos DB change feed with Event Hubs
- Following the Microsoft Team Data Science process for producing and deploying a model.
- Utilizing Azure Machine Learning to support model experimentation and deployment, and reduce training time by using AutoML features.
- Enabling reporting by business users, analysts and data scientists against data in Azure Databricks and Cosmos DB.

### Hands-on lab _(this will go in the readme and in the HOL document)_

Woodgrove Bank, who provides payment processing services for commerce, is looking to design and implement a PoC of an innovative fraud detection solution. They want to provide new services to their merchant customers, helping them save costs by applying machine learning and advanced analytics to detect fraudulent transactions. Their customers are around the world, and the right solutions for them would minimize any latencies experienced using their service by distributing as much of the solution as possible, as closely as possible, to the regions in which their customers use the service.

In this hands-on lab session, you will implement a PoC of the data pipeline that could support the needs of Woodgrove Bank.

At the end of this workshop, you will be better able to implement solutions that leverage the strengths of Cosmos DB in support of advanced analytics solutions that require high throughput ingest, low latency serving and global scale in combination with scalable machine learning, big data and real-time processing capabilities.

#### Outline: Hands-on lab exercises

- Exercise 0: Before the hands-on lab
  - Task 1: Setting up the lab environment
- Exercise 1: Collecting streaming transaction data
  - Task 1: Configuring Event Hubs and the transaction generator
  - Task 2: Ingesting streaming data to Cosmos DB
  - Task 3: Choosing between Cosmos DB and Event Hubs for ingestion
- Exercise 2: Understanding and preparing the transaction data at scale
  - Task 1: Querying streaming transactions with Azure Databricks and Spark Structured Streaming
  - Task 2: Querying transactions directly from Cosmos DB with Azure Databricks and Spark
  - Task 3: Responding to transactions using the Cosmos DB Change Feed and Azure Databricks Delta
- Exercise 3: Creating and evaluating fraud models
  - Task 1: Creating a parsimonious model for fraud detection using Azure Databricks
  - Task 2: Experimenting, evaluating and selecting a best performing model using Azure Databricks and Azure Machine Learning
  - Task 3: Deploying the best model as a web service
  - Task 4: Utilizing the best model for batch scoring
- Exercise 4: Scaling globally
  - Task 1: Distributing batch scored data globally using Cosmos DB
  - Task 2: Distributing models globally
  - Task 3: Scheduling Azure Databricks jobs to batch score transactions on a schedule
- Exercise 5: Reporting
  - Task 1: Utilizing Power BI to summarize and visualize global fraud trends
  - Task 2: Creating dashboards in Azure Databricks

## Azure services and related products

- Azure Cosmos DB
- Azure Databricks
- Azure Data Lake Store
- Azure Event Hubs
- Azure Kubernetes Service
- Azure Machine Learning
- Power BI

## Azure solutions

_This is an internal reference and will be updated by project PM._

## Related references

_This should be a list of and links to prereqs, architectural diagrams, supporting docs, or briefing decks related to the material._

- [MCW](https://github.com/Microsoft/MCW)
