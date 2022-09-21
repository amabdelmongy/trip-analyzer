# Service Agent

## Background

Implement the specified REST Endpoint
Protect the API with BasicAuth
Use Docker to run your application
Use one of the following languages: Go, Java, Python, C++
Automate the infrastructure rollout
Use an external service to determine the city name for depature and destination
Please also consider quality checks for your application
Upload your solution to a private GitHub repository and grant access for user 'ris-onboarding'
Provide a link to the secured hosted instance of your solution together with a username and a password
Provide the following files together with your code in the github repository:
• Dockerfile
• Build-Script
• Deployment-Script
• Kubernetes deployment YAML (only if Kubernetes is used)
• Infrastructure automation scripts
• README.md with documentation how to deploy the infrastructure and the application
please keep in mind that we will use your code to evaluate your skills. This programming challenge is not the best place for 'quick and dirty' solutions.


## Requirements

## Deliverables

Built an API service that allows the client to
  • Execute service call for Post.
  
This API Service is able to work with Json formatted input and it supports HTTP/HTTPS as messaging protocol.


##  Description

A simple web api. It consists of a backend services that allow clients to execute service calls.

## Solution Description

## Target Frameworks
Target frameworks: .NET core 6

## Technology
 - C#
 - .Net core
 - Swagger
 - NUnit
 - Moq
 - Docker to build the project

## Architecture
### Onion architecture
The Onion Architecture is an Architectural Pattern that enables maintainable and evolutionary enterprise systems.

### Unit tests
 It validates if that code results in the expected state (state testing) or executes the expected sequence of events (behavior testing).
 It covers a lot of code areas.

### Integration tests
individual software modules are combined and tested as a group

### Swagger documentation
  - Swagger generate file for last version of api under this link ```/swagger/v1/swagger.json```

##  How to run the code
To start the internal service and its dependencies locally, execute:
the file docker-compose.yml will be under api folder direct.
```
    docker-compose up --build
```
    run from visual studio 2019

## ToDo
1. Add more unit tests.
