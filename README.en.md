## Components and Resources

| Component                | Description                                           | Documentation                                                                 |
|--------------------------|-------------------------------------------------------|-------------------------------------------------------------------------------|
| Hot Chocolate            | GraphQL server for .NET                               | [Hot Chocolate Documentation](https://chillicream.com/docs/hotchocolate/v13)  |
| .NET Core                | Development platform for modern applications          | [.NET Core Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview) |
| Postgres                 | Relational database management system                 | [Documentación Postgres](https://www.postgresql.org/)                         |

# TrackHub Management API

The **TrackHub Management API** provides a robust, modular service for managing core tracking and resource data in TrackHub's ecosystem. Built on **Clean Architecture** principles, this **GraphQL**-based API is designed for flexibility, maintainability, and scalability, ensuring seamless integration and easy adaptability as your business evolves.

## Key Features

This API offers:
- Streamlined account and resource management, providing precise access control and role management.
- Detailed access control with role-based and user-based policies for optimal security and compliance.
- Personalizable settings for both accounts and individual users to enhance user experience.

---

## Entities

### Account and Configuration Entities

- **Account**: Serves as the main organizational unit, linking users, groups, and operators for each client’s TrackHub operations.
- **AccountSettings**: Stores settings unique to an account, including map configurations and operational intervals for smoother client experiences.
- **Credential**: Manages secure access to external services through encrypted authentication details and tokens, ensuring safe, compliant integrations.

### User and Role Entities

- **User**: A person accessing the system, possibly with multiple group affiliations and a direct link to an account. Each user can have customized access roles.
- **UserGroup**: Defines associations between users and groups to streamline access control and permission assignments.
- **UserSettings**: Stores user preferences, such as theme, language, and notification settings, personalizing the user experience.
- **Operator**: Represents a third-party administrator managing devices, with the credentials to access related external services and monitor assets.

### Tracking and Grouping Entities

- **Transporter**: Represents an entity (e.g., vehicle, person, or pet) equipped with a GPS device, linked to groups and devices, and trackable in real-time.
- **TransporterGroup**: Establishes a relationship between transporters and groups, enabling structured management and permission control.
- **Device**: A tracking device within the system associated with both a transporter and an operator, supplying real-time location and status data.
- **TransporterPosition**: Records and timestamps the location of a transporter for route history and monitoring.
- **Group**: A collection of users and transporters within the system, simplifying permission management and enhancing security protocols.

---

### Why GraphQL?

The use of **GraphQL** enables efficient, customizable queries, letting clients request only the data they need to minimize bandwidth and enhance app performance. With GraphQL, applications can retrieve specific details about users, transporters, or devices, optimizing both operational efficiency and user experience.

## License

This project is licensed under the Apache 2.0 License. See the [LICENSE file](https://www.apache.org/licenses/LICENSE-2.0) for more information.