# Ejercicio Técnico — Sistema de Gestión de Facturas (Full-Stack) | .NET 8 + SQLite + React

Sistema web para gestionar facturas, importarlas desde un archivo JSON en tiempo de ejecución, almacenarlas en SQLite y exponer una API REST documentada con Swagger para administración, búsqueda y reportes. 

---

## Objetivo

Implementar un sistema web para gestionar facturas integrando datos desde un archivo JSON, persistiendo en base de datos y permitiendo su administración mediante una interfaz intuitiva. 


---
##  ----------- Notas Importante --------------------------

para generar  un usuario, en swagger existe un **endpoint(/api/Auth/register)** donde puede crear el usuario con los parametros que se necesite. 
de igual manera, **se deja una base de datos sqllite** en la base del repositorio, llamado invoices.db y este lo coloque en la raiz de la api invoiceExercise junto con program, con eso no es necesario realizar migracion(update-database) las credenciales de este son:


------
user: test
password: test
------

## Stack tecnológico

### Backend
- .NET 8 (ASP.NET Core Web API).
- Entity Framework Core (Code First) + SQLite.
- Swagger (Swashbuckle) para documentación de endpoints.

### Frontend
- React (UI + validaciones en formularios).

---

## Requisitos implementados

### 1) Integración desde JSON
- Carga de facturas desde `bd_exam.json` en tiempo de ejecución.
- `invoice_number` único (no se permiten duplicados).
- Validación de consistencia: si la suma de subtotales de productos no coincide con `total_amount`, la factura se marca como **inconsistente** y se excluye del sistema activo, pero se mantiene en base para reporte.
- Cálculo automático de estado de factura:
  - **Issued**: sin notas de crédito.
  - **Cancelled**: suma de montos de NC igual al total de la factura.
  - **Partial**: suma de montos de NC menor al total de la factura.
- Cálculo automático de estado de pago:
  - **Pending**: pago pendiente dentro del plazo.
  - **Overdue**: vencida según `payment_due_date`.
  - **Paid**: pago registrado.

### 2) Búsqueda
Búsqueda de facturas por:
- Número de factura.
- Estado de factura.
- Estado de pago.

### 3) Gestión de notas de crédito (NC)
- Creación de NC asociada a una factura, con fecha de creación automática.
- Validación de negocio: el monto de NC no puede superar el saldo pendiente de la factura.

### 4) Reportes
- Facturas consistentes con más de 30 días vencidas **sin pago** y **sin nota de crédito** (ej. endpoint `overdue-30`).
- Resumen total y porcentaje por estado de pago: **Paid / Pending / Overdue**.
- Facturas inconsistentes (cuando `total_amount` no coincide con la suma de productos).

---

## Decisiones de diseño / buenas prácticas

### Arquitectura
Separación por capas tipo Clean Architecture / modular monolith (Domain / Application / Infrastructure / Web) para mantener el dominio y casos de uso aislados de infraestructura y presentación(API).

### Contratos de API (DTOs)
Para endpoints de reportes se usan DTOs/`record` (por ejemplo `PayStatusSummaryItem`) en lugar de tuplas, para evitar respuestas JSON inestables como `[{},{},{}]` y mantener un contrato claro.

### Reporte “Pay status” completo
El resumen de estados de pago devuelve siempre **Paid, Pending, Overdue**, rellenando con 0 cuando algún estado no existe (comportamiento normal de `GroupBy` en LINQ/EF).

### Manejo de errores
Se recomienda evitar `try/catch` repetido en controllers y centralizar el manejo de excepciones, devolviendo códigos HTTP coherentes (400 validación, 404 no encontrado, 500 para errores no esperados, 5xx errores de infraestructura/BD).

---

## Swagger (documentación)

Swagger UI expone la documentación de la API y los códigos de respuesta.

### Mostrar `<summary>` y `<remarks>` en Swagger (XML Comments)

1) En el `.csproj` del proyecto Web (donde están los Controllers), habilita la generación del XML:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

2) Configurar Swagger para leer el XML (`IncludeXmlComments`)

En `Program.cs` (proyecto Web/API), dentro de `AddSwaggerGen`, agrega exactamente esto:

```csharp
using System.Reflection;

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
```


## Cómo ejecutar (Frontend + Backend)

### Prerrequisitos
- .NET SDK 8 instalado (backend).
- Node.js + npm instalados (frontend).

---

### 1) Levantar el Backend (API .NET 8)

1. Abre una terminal en la raíz del repo.

2. **Configurar `appsettings.json`**:
   Crea un archivo `appsettings.json` en la carpeta del proyecto WebAPI (llamado InvoiceExercise) (donde está el `.csproj`) con el siguiente contenido:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=invoices.db"
     },
    "Jwt": {
      "Key": "iBBbjzliMAhbMNtk6152wJc/9BYROV2CdPQBBlaGJhVZ734MwBmwf8VfsTJE8yQkV89fgsIVcQzjkGGQOD7NRg==",
      "Issuer": "InvoiceExercise",
      "Audience": "InvoiceExerciseUsers"
    },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning",
         "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
       }
     },
     "AllowedHosts": "*"
   }
   ```

3. **Crear Base de Datos**:
   Aplica las migraciones para generar el archivo `invoices.db`. Elige una opción:

   **Opción A (.NET CLI):**
   asegurate estar en la ruta **InvoiceExerciseApex\backend\InvoiceExercise**
   ```bash
    dotnet tool install --global dotnet-ef
    dotnet ef database update --project Infrastructure --startup-project InvoiceExercise   
   ```
   **Opción B (Package Manager Console - Visual Studio):**
   ```powershell
    Update-Database -Project Infrastructure -StartupProject InvoiceExercise
   ```

4. Ejecuta el proyecto WebAPI:

  al igual que los anteriores comandos, asegurate estar en la ruta **InvoiceExerciseApex\backend\InvoiceExercise**

```bash
dotnet restore
dotnet run --project InvoiceExercise
```

Cuando el backend levante, la consola mostrará las URLs donde quedó escuchando (HTTP/HTTPS).

Swagger:
- Abre `https://localhost:<puerto>/swagger` (o `http://localhost:<puerto>/swagger`).

> **Nota:** La interfaz de Swagger está habilitada por defecto solo en el entorno de desarrollo (`Development`). No estará disponible en un entorno de producción.

> Si Swagger no aparece, es porque `UseSwagger()` y `UseSwaggerUI()` están habilitados solo en desarrollo, pero pueden ser modificados en IsProduction() (normalmente solo en `Development`).

---

### 2) Levantar el Frontend (React)

1. En otra terminal, entra a la carpeta del frontend:

```bash
cd frontend\InvoiceExerciseFront
```
esto desde la raíz del repo, la raiz del proyecto React esta en InvoiceExerciseFront.

2. Instala dependencias y corre el servidor de desarrollo:

```bash
npm install
npm run dev
```

Por defecto, Vite suele levantar en `http://localhost:5173`.

---

### 3) Conectar Frontend ↔ Backend (URL de la API)

Asegúrate de que el frontend apunte a la URL del backend (por ejemplo con una variable de entorno).  
Si estás usando Vite, normalmente se define como `VITE_API_URL` (ej. `VITE_API_URL=https://localhost:<puerto>`).

---

### 4) Problema típico: CORS

Si el frontend (ej. `localhost:5173`) llama al backend en otro puerto, el navegador puede bloquear por CORS si no lo permites en la API.

Si te pasa, habilita CORS en el backend para el origen del frontend (ej. `http://localhost:5173`), en caso que se cambie el puerto, con .WithOrigins("http://localhost:5173") con el puerto correspondiente en program.cs (WEBAPI).


---
##  Notas de crédito

para crear notas de crédito para alguna factura, realice la busqueda por cualquiera de los filtros y en la fila le aparecera la opcion de generar nota de crédito
------