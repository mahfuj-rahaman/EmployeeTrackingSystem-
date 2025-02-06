# <h1>Employee Tracking System</h1>
<hr>

<h2>High-Level Design Overview</h2>

<h3> 1. Architecture Overview</h3>

Microservices Architecture: The system will be designed using a microservices architecture to ensure modularity, scalability, and ease of maintenance.

CQRS and Vertical Slicing: The system will be designed following the CQRS (Command Query Responsibility Segregation) pattern to separate the read and write operations. Vertical slicing will be used to split the application into smaller, more manageable units.

API Versioning: API versioning will be implemented to ensure backward compatibility and smooth transitions between different versions of the APIs.

<h3> 2. Technology Stack</h3>
Backend Framework: ASP.NET Core 8.0

Mediator Pattern: MediatR for handling commands and queries

Message Broker: MassTransit for distributed messaging

Database (Command): MSSQL for command operations

Database (Query): Redis for query operations

<h3> 3. Microservices</h3>

Employee Service:

Responsibilities: Manage employee data including personal details, department, and designation.

Endpoints: Create, Read, Update, and Delete employee information.

Department Service:

Responsibilities: Manage department information.

Endpoints: Create, Read, Update, and Delete departments.

Designation Service:

Responsibilities: Manage designation information.

Endpoints: Create, Read, Update, and Delete designations.

Authentication Service:

Responsibilities: Centralized login system for authentication and authorization.

Endpoints: Login, Register, Token management.

<h3> 4. CQRS Implementation</h3>

Commands:

Handled by MSSQL using MediatR.

Commands will be used for operations that modify the state (e.g., CreateEmployeeCommand, UpdateEmployeeCommand).

Queries:

Handled by Redis using MediatR.

Queries will be used for read-only operations (e.g., GetEmployeeQuery, GetDepartmentQuery).

<h3>5. API Versioning</h3>

Versioning Strategy:

URL Versioning: APIs will be versioned using URLs (e.g., /api/v1/employees, /api/v2/employees).

Header Versioning: APIs can also support versioning through custom headers.

<h3>6. MassTransit Integration</h3>

Message Bus: MassTransit will be used for event-driven communication between microservices.

Event Handling: Each microservice will publish and subscribe to events using MassTransit.

<h3> 7. Authentication and Authorization</h3>
Centralized Login: The Authentication Service will handle user login and token management.

JWT Tokens: JSON Web Tokens (JWT) will be used for secure communication between clients and services.

Overall Flow
User Authentication: Users will authenticate using the Authentication Service. A JWT token will be issued upon successful authentication.

Commands: When a user performs an action that modifies data, a command will be sent to the respective microservice (Employee Service, Department Service, etc.).

Events: The microservice will process the command and publish events using MassTransit.

Queries: For read operations, the system will query Redis to fetch the required data.
