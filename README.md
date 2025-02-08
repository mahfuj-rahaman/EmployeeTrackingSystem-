for running the code please follow the instruction
<br>
//docker compose

<br>
<code> docker compose up -d --build </code>
<br>


# <h1>Employee Tracking System</h1>
<hr>

<h2>High-Level Design Overview</h2>

<h3> 1. Architecture Overview</h3>

<b>Microservices Architecture:</b> The system will be designed using a microservices architecture to ensure modularity, scalability, and ease of maintenance.

<b>CQRS and Vertical Slicing:</b> The system will be designed following the CQRS (Command Query Responsibility Segregation) pattern to separate the read and write operations. Vertical slicing will be used to split the application into smaller, more manageable units.

<b>API Versioning:</b> API versioning will be implemented to ensure backward compatibility and smooth transitions between different versions of the APIs.

<h3> 2. Technology Stack</h3>

<b>Backend Framework:</b> ASP.NET Core 8.0

<b>Mediator Pattern:</b> MediatR for handling commands and queries

<b>Message Broker:</b> MassTransit for distributed messaging

<b>Database (Command):</b> MSSQL for command operations

<b>Database (Query):</b> Redis for query operations

<h3> 3. Microservices</h3>

<h4>Employee Service:</h4>

<b>Responsibilities:</b> Manage employee data including personal details, department, and designation.

<b>Endpoints:</b> Create, Read, Update, and Delete employee information.

<h4>Department Service:</h4>

<b>Responsibilities:</b> Manage department information.

<b>Endpoints:</b> Create, Read, Update, and Delete departments.

<h4>Designation Service:</h4>

<b>Responsibilities:</b> Manage designation information.

<b>Endpoints:</b> Create, Read, Update, and Delete designations.

<h4>Authentication Service:</h4>

<b>Responsibilities:</b> Centralized login system for authentication and authorization.

<b>Endpoints:</b> Login, Register, Token management.

<h3> 4. CQRS Implementation</h3>

<h4>Commands:</h4>

Handled by MSSQL using MediatR.

Commands will be used for operations that modify the state (e.g., CreateEmployeeCommand, UpdateEmployeeCommand).

<h4>Queries:</h4>

Handled by Redis using MediatR.

Queries will be used for read-only operations (e.g., GetEmployeeQuery, GetDepartmentQuery).

<h3> 5. API Versioning</h3>

<h4>Versioning Strategy:</h4>

<b>URL Versioning:</b> APIs will be versioned using URLs (e.g., /api/v1/employees, /api/v2/employees).

<b>Header Versioning:</b> APIs can also support versioning through custom headers.

<h3> 6. MassTransit Integration</h3>

<b>Message Bus:</b> MassTransit will be used for event-driven communication between microservices.

<b>Event Handling:</b> Each microservice will publish and subscribe to events using MassTransit.

<h3> 7. Authentication and Authorization</h3>

<b>Centralized Login:</b> The Authentication Service will handle user login and token management.

<b>JWT Tokens:</b> JSON Web Tokens (JWT) will be used for secure communication between clients and services.

<h4>Overall Flow</h4>

<b>User Authentication:</b> Users will authenticate using the Authentication Service. A JWT token will be issued upon successful authentication.

<b>Commands:</b> When a user performs an action that modifies data, a command will be sent to the respective microservice (Employee Service, Department Service, etc.).

<b>Events:</b> The microservice will process the command and publish events using MassTransit.

<b>Queries:</b> For read operations, the system will query Redis to fetch the required data.


