﻿## Componentes y Recursos Utilizados

| Componente                | Descripción                                             | Documentación                                                                 |
|---------------------------|---------------------------------------------------------|-------------------------------------------------------------------------------|
| Hot Chocolate             | Servidor GraphQL para .Net        | [Documentación Hot Chocolate](https://chillicream.com/docs/hotchocolate/v13)                           |
| .NET Core                 | Plataforma de desarrollo para aplicaciones modernas     | [Documentación .NET Core](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview) |
| Postgres                  | Sistema de gestión de bases de datos relacional         | [Documentación Postgres](https://www.postgresql.org/)                         |

# API de Administración de TrackHub

La **API de Administración de TrackHub** proporciona un servicio modular y robusto para gestionar los datos de seguimiento y recursos principales en el ecosistema de TrackHub. Construida sobre principios de **Arquitectura Limpia**, esta API basada en **GraphQL** está diseñada para ofrecer flexibilidad, mantenibilidad y escalabilidad, asegurando una integración sin complicaciones y una fácil adaptabilidad a medida que evoluciona su negocio.

## Principales Características

Esta API ofrece:
- Gestión simplificada de cuentas y recursos, con un control de acceso detallado y administración de roles.
- Control de acceso específico mediante políticas basadas en roles y usuarios, para seguridad y cumplimiento óptimos.
- Configuraciones personalizables para cuentas y usuarios individuales, mejorando la experiencia del usuario.

---

## Entidades

### Entidades de Cuenta y Configuración

- **Account** (Cuenta): Actúa como la unidad organizativa principal, vinculando usuarios, grupos y operadores para cada operación del cliente en TrackHub.
- **AccountSettings** (Configuración de Cuenta): Almacena configuraciones específicas de una cuenta, incluyendo configuraciones de mapas e intervalos operacionales para mejorar la experiencia del cliente.
- **Credential** (Credencial): Gestiona el acceso seguro a servicios externos mediante detalles de autenticación y tokens encriptados, asegurando integraciones seguras y conformes.

### Entidades de Usuario y Roles

- **User** (Usuario): Una persona que accede al sistema, que puede estar afiliada a múltiples grupos y vinculada directamente a una cuenta. Cada usuario puede tener roles de acceso personalizados.
- **UserGroup** (Grupo de Usuario): Define asociaciones entre usuarios y grupos, facilitando el control de acceso y la asignación de permisos.
- **UserSettings** (Configuración de Usuario): Almacena las preferencias del usuario, como el tema, el idioma y la configuración de notificaciones, personalizando la experiencia de cada usuario.
- **Operator** (Operador): Representa un administrador externo que gestiona dispositivos, con las credenciales necesarias para acceder a servicios externos relacionados y monitorear activos.

### Entidades de Seguimiento y Agrupamiento

- **Transporter** (Transportador): Representa una entidad (como un vehículo, persona o mascota) equipada con un dispositivo GPS, vinculada a grupos y dispositivos, y rastreable en tiempo real.
- **TransporterGroup** (Grupo de Transportador): Establece una relación entre transportadores y grupos, permitiendo una gestión estructurada y el control de permisos.
- **Device** (Dispositivo): Un dispositivo de seguimiento en el sistema asociado tanto a un transportador como a un operador, proporcionando datos de ubicación y estado en tiempo real.
- **TransporterPosition** (Posición de Transportador): Registra y marca temporalmente la ubicación de un transportador para el historial de rutas y monitoreo.
- **Group** (Grupo): Una colección de usuarios y transportadores dentro del sistema, simplificando la gestión de permisos y mejorando los protocolos de seguridad.
- **Report** (Informe): Representa la lista de informes disponibles para la cuenta del sistema, incluyendo tipos de informes, formatos y detalles de generación.

---

### ¿Por qué GraphQL?

El uso de **GraphQL** permite consultas eficientes y personalizables, permitiendo a los clientes solicitar solo los datos que necesitan para minimizar el ancho de banda y mejorar el rendimiento de la aplicación. Con GraphQL, las aplicaciones pueden recuperar detalles específicos sobre usuarios, transportadores o dispositivos, optimizando tanto la eficiencia operativa como la experiencia del usuario.

## Licencia

Este proyecto está bajo la Licencia Apache 2.0. Consulta el archivo [LICENSE](https://www.apache.org/licenses/LICENSE-2.0) para más información.