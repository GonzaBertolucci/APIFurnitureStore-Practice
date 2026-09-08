API Furniture Store 🪑
API RESTful desarrollada para la gestión del backend de un E-commerce de muebles. El proyecto demuestra la implementación práctica de una arquitectura limpia, manejo de transacciones comerciales y seguridad mediante tokens.

🚀 Tecnologías y Características
Core: Desarrollado con C# y .NET 10.

Seguridad: Implementación de JWT, Refresh Tokens y Secret Manager.

Base de Datos: Mapeo relacional y acceso a datos utilizando Entity Framework Core.

Gestión de Usuarios: Endpoints protegidos, Register, Login y validación por email.

Lógica de Negocio: CRUD completo para Productos, Categorías, Clientes y Órdenes.

🛠️ Configuración Local
Clonar el repositorio en tu equipo local mediante la terminal.

Abrir la solución del proyecto utilizando Visual Studio 2026.

Hacer clic derecho en el proyecto y usar la opción "Administrar secretos del usuario" para configurar tu clave privada de JWT y tu cadena de conexión a la base de datos.

Ejecutar el comando Update-Database en la Consola del Administrador de Paquetes para aplicar las migraciones correspondientes.

Compilar y ejecutar la solución.

📚 Uso y Pruebas
El proyecto tiene Swagger integrado. Al ejecutar la API en el entorno de desarrollo local, se abrirá automáticamente una interfaz gráfica en el navegador web. Desde allí se puede acceder a la documentación interactiva de todos los endpoints, autorizarse ingresando el token JWT y realizar pruebas HTTP directamente desde la página.
