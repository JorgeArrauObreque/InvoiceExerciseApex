# Ejercicio Técnico — Sistema de Gestión de Facturas (Full-Stack) | .NET 8 + SQLite + React

Sistema web para gestionar facturas, importarlas desde un archivo JSON en tiempo de ejecución, almacenarlas en SQLite y exponer una API REST documentada con Swagger para administración, búsqueda y reportes. 

---

## Objetivo

Implementar un sistema web para gestionar facturas integrando datos desde un archivo JSON, persistiendo en base de datos y permitiendo su administración mediante una interfaz intuitiva. 

---

## Stack tecnológico

### Backend
- .NET 8 (ASP.NET Core Web API). [cite:4][cite:10]
- Entity Framework Core (Code First) + SQLite. [cite:3][cite:23]
- Swagger (Swashbuckle) para documentación de endpoints. [cite:10][web:24]

### Frontend
- React (UI + validaciones en formularios). [cite:6]

---

## Requisitos implementados

### 1) Integración desde JSON
- Carga de facturas desde `bd_exam.json` en tiempo de ejecución. [cite:1][cite:10]
- `invoice_number` único (no se permiten duplicados). [cite:1]
- Validación de consistencia: si la suma de subtotales de productos no coincide con `total_amount`, la factura se marca como **inconsistente** y se excluye del sistema activo, pero se mantiene en base para reporte. [cite:10]
- Cálculo automático de estado de factura:
  - **Issued**: sin notas de crédito.
  - **Cancelled**: suma de montos de NC igual al total de la factura.
  - **Partial**: suma de montos de NC menor al total de la factura. [cite:10]
- Cálculo automático de estado de pago:
  - **Pending**: pago pendiente dentro del plazo.
  - **Overdue**: vencida según `payment_due_date`.
  - **Paid**: pago registrado. [cite:18]

### 2) Búsqueda
Búsqueda de facturas por:
- Número de factura.
- Estado de factura.
- Estado de pago. [cite:10]

### 3) Gestión de notas de crédito (NC)
- Creación de NC asociada a una factura, con fecha de creación automática. [cite:10]
- Validación de negocio: el monto de NC no puede superar el saldo pendiente de la factura. [cite:10]

### 4) Reportes
- Facturas consistentes con más de 30 días vencidas **sin pago** y **sin nota de crédito** (ej. endpoint `overdue-30`). [cite:14]
- Resumen total y porcentaje por estado de pago: **Paid / Pending / Overdue**. [cite:15][cite:18]
- Facturas inconsistentes (cuando `total_amount` no coincide con la suma de productos). [cite:14]

---

## Decisiones de diseño / buenas prácticas

### Arquitectura
Separación por capas tipo Clean Architecture / modular monolith (Domain / Application / Infrastructure / Web) para mantener el dominio y casos de uso aislados de infraestructura y presentación. [cite:7][cite:5]

### Contratos de API (DTOs)
Para endpoints de reportes se usan DTOs/`record` (por ejemplo `PayStatusSummaryItem`) en lugar de tuplas, para evitar respuestas JSON inestables como `[{},{},{}]` y mantener un contrato claro. [cite:15][cite:17]

### Reporte “Pay status” completo
El resumen de estados de pago devuelve siempre **Paid, Pending, Overdue**, rellenando con 0 cuando algún estado no existe (comportamiento normal de `GroupBy` en LINQ/EF). [cite:18]

### Manejo de errores
Se recomienda evitar `try/catch` repetido en controllers y centralizar el manejo de excepciones, devolviendo códigos HTTP coherentes (400 validación, 404 no encontrado, 500 para errores no esperados, 5xx errores de infraestructura/BD). [cite:13][cite:21]

---

## Swagger (documentación)

Swagger UI expone la documentación de la API y los códigos de respuesta. [cite:10][web:24]

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
- .NET SDK 8 instalado (backend). [web:24]
- Node.js + npm instalados (frontend). [web:108]

---

### 1) Levantar el Backend (API .NET 8)

1. Abre una terminal en la raíz del repo.
2. Ejecuta el proyecto Web/API:

```bash
dotnet restore
dotnet run --project <ruta-a-tu-proyecto-web-api>
```

Cuando el backend levante, la consola mostrará las URLs donde quedó escuchando (HTTP/HTTPS). [web:24]

Swagger:
- Abre `https://localhost:<puerto>/swagger` (o `http://localhost:<puerto>/swagger`). [web:24]

> Si Swagger no aparece, revisa que `UseSwagger()` y `UseSwaggerUI()` estén habilitados (normalmente solo en `Development`). [web:24]

---

### 2) Levantar el Frontend (React)

1. En otra terminal, entra a la carpeta del frontend:

```bash
cd <ruta-a-tu-frontend>
```

2. Instala dependencias y corre el servidor de desarrollo:

```bash
npm install
npm run dev
```

Por defecto, Vite suele levantar en `http://localhost:5173`. [web:108]

---

### 3) Conectar Frontend ↔ Backend (URL de la API)

Asegúrate de que el frontend apunte a la URL del backend (por ejemplo con una variable de entorno).  
Si estás usando Vite, normalmente se define como `VITE_API_URL` (ej. `VITE_API_URL=https://localhost:<puerto>`). [web:108]

---

### 4) Problema típico: CORS

Si el frontend (ej. `localhost:5173`) llama al backend en otro puerto, el navegador puede bloquear por CORS si no lo permites en la API. [web:119]

Si te pasa, habilita CORS en el backend para el origen del frontend (ej. `http://localhost:5173`). [web:119]
