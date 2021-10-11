# RentItWebAPI

## About the project

A simple application for managing booking process for rental-oriented businesses.

This Web API provides a business manager an ability to publish detailed information about offered items and handle a workflow of information between a customer and an organization. In current implementation it is possible to maintain rentals of items for which daily charges apply.
Due to an occurence of heavy dependency on seasons or other external factors in some kinds of business, there are two available routes of making a valid booking. For the sake of better understanding those routes are named:

1. Book-As-You-Go - a booking can be made as long as there are no other colliding reservations;
2. Book On Request - above condition must be met + the customer must first make a request that can be accepted or rejected by the business manager.

The second option is perfect for some businesses that strongly rely on seasonal rentals, such as: hotels, guest houses or companies that rent out out boats or other sort of water equipment in leisure areas. In such cases managers need to plan their rentals wisely, since there is a short period of time when their resources are viable.

## Features

In current stage this application includes:

* authentication via JWT, authorization by role;
* interaction with items in CRUD manner;
* making reservations and requests regarding offered items (Book-as-you-go/Book On Request);
* sending automatic notification e-mails to customers and business;
* creating invoices with an external, free Web API and saving them on Azure Blobs;
* basic error handling.

## Technologies

This project has been created with:

* ASP.NET Core (Web API) ver 5.0,
* Entity Framework Core ver. 5.0.9
* Azure Storage

Moreover, the application uses content from:
* invoice-generator.com - a free Web API used to generate .pdf Invoices,
* playground.dyspatch.io - free HTML templates used as e-mail body.

## Installation 

To run this application, simply change credentials in appsettings.json file. This includes following sections: `Azure Blob Storage Connection String`, `Mail`, `Gmail`, `Authentication`.
You can deploy it on any server or run it locally by changing the database connection string in appsettings.Development.json.
To make it work with Azure Storage, go to BlobService.cs and in method `GetBlobContainerClient` pass a parameter with the name of your Blob Container. (in project files, this variable is set to `"invoice"`).

