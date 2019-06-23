# ![RealWorld Example App](logo.png)

> ### .NET Core codebase containing real world examples (CRUD, auth, advanced patterns, etc) that adheres to the [RealWorld](https://github.com/gothinkster/realworld) spec and API.


### [Demo](https://github.com/gothinkster/realworld)&nbsp;&nbsp;&nbsp;&nbsp;[RealWorld](https://github.com/gothinkster/realworld)


This codebase was created to demonstrate a fully fledged fullstack application built with .NET Core including CRUD operations, authentication, routing, pagination, and more.

We've gone to great lengths to adhere to the .NET Core community styleguides & best practices.

For more information on how to this works with other frontends/backends, head over to the [RealWorld](https://github.com/gothinkster/realworld) repo.


# How it works

This project was inspired by the current ASP.NET Core implementations of Conduit.
Inspired by clean architecture, the solution is broken up into nine separate projects, utilizing the DIP (dependency inversion principal) for applicable projects.

Core projects:
 - Conduit.Api - The API spec and gateway for the application, built using thin controllers and ASP.NET Core
 - Conduit.Core - The central business logic project for the applications, leverages CQRS and MediatR to perform operations
 - Conduit.Domain - The API model project housing all domain objects leveraged in the projects
 - Conduit.Persistence - The data access layer of the application, uses Entity Framework Core and Fluent API for entity configuration
 - Conduit.Infrastructure - Miscellaneous application functionality, including common services and security
 - Conduit.Shared - Common resources and extensions/helper methods used within each project
 
Test projects:
 - Conduit.Core.Tests - xUnit tests for the Conduit.Core project
 - Conduit.Shared.Tests - xUnit tests from the Conduit.Shared project
 - Conduit.Integration.Tests - xUnit e2e tests for the API spec
 
My aim for this project is to achieve as **close** to 100% test coverage as possible, demonstrating TDD and full spec suite unit testing methodologies.

# Getting started

> npm install, npm start, etc.

