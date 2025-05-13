# AppMexaba

Bienvenido al proyecto **AppMexaba**, una plataforma modular desarrollada con ASP.NET Core MVC y SQL Server, diseñada para gestionar catálogos de artículos y compradores, con futuras extensiones para otros módulos del negocio.

## 🔧 Tecnologías utilizadas

- ASP.NET Core MVC (.NET 7+)
- Entity Framework Core
- SQL Server
- Bootstrap 5 + Select2 (interfaz frontend)

---

## 📦 Módulo actual: Catálogo de Artículos y Compradores

Este módulo permite:

- Crear y editar relaciones entre artículos y compradores.
- Buscar artículos por código o descripción.
- Buscar compradores por nombre.
- Mostrar resultados en paginación.

### 🗃️ Tablas utilizadas

- `invart` → Clave del artículo (`art`) y proveedor (`cve_pro`)
- `inviar` → Descripción del artículo (`des1`)
- `cprprv` → Información del proveedor (`nom`)
- `cprcom` → Información del comprador (`cve`, `nom`)
- `tcausr` → Usuario asociado al comprador (`nombre`)

---

## 📁 Estructura de carpetas

```
AppMexaba/
├── Controllers/          // Controladores MVC
├── Models/               // Clases de entidad y DTOs
├── Services/             // Lógica de negocio (CatalogoService)
├── Views/                // Vistas Razor
├── wwwroot/              // Archivos estáticos
└── README.md             // Este archivo ✨
```

---

## 🚀 Cómo ejecutar

1. Clona el repositorio
2. Configura tu cadena de conexión a SQL Server en `appsettings.json`
3. Ejecuta el proyecto con Visual Studio o `dotnet run`

---

## 🔜 Próximos pasos

- Agregar módulo de usuarios
- Integrar sistema de autenticación
- Implementar dashboard con Power BI
- Optimización de servicios

---

## 🧑‍💻 Desarrollado por

**Software 85** – Soluciones empresariales a medida


# Catálogo de Artículos - Módulo AppMexaba

Este módulo forma parte del sistema **AppMexaba**, y gestiona la relación entre artículos y compradores para procesos de abastecimiento y compras. Está construido con **ASP.NET Core MVC**, utilizando **EF Core** para el acceso a datos.

---

## Estructura del Módulo

### 1. Controlador: `CatalogoController`
Responsable de exponer las vistas y endpoints para:
- Listar artículos (`Index`)
- Crear un nuevo artículo-comprador (`Crear`)
- Editar relaciones (`Editar`)
- Consultas asincrónicas para Select2:
  - `/api/catalogo/buscar-articulos`
  - `/api/catalogo/buscar-compradores`

### 2. Servicio: `CatalogoService`
Contiene la lógica de negocio y acceso a datos. Aislado mediante la interfaz `ICatalogoService`. Expone:
- `ObtenerArticulosAsync()`
- `ObtenerArticuloPorIdAsync()`
- `CrearArticuloAsync()`
- `EditarArticuloAsync()`
- `BuscarArticulosAsync()`
- `BuscarCompradoresAsync()`

### 3. Modelo de datos: `ArticuloComprador`
Corresponde a la tabla `cpr_art`, relacionada con:
- `invart`: Clave del artículo
- `inviar`: Descripción del artículo
- `cprprv`: Datos del proveedor
- `cprcom`: Clave del comprador
- `tcausr`: Usuario asignado

### 4. Vista (Razor)
Vista fuertemente tipada que:
- Usa Select2 para artículos y compradores
- Carga datos relacionados (cve, usuario, proveedor)
- Valida selección antes del submit

---

## Flujo CRUD
1. **Crear/Editar**:
   - Selección de artículo y comprador desde listas asincrónicas.
   - Se guardan en la tabla `cpr_art` los campos:
     - `Art`, `Des1`, `NumProveedor`, `Proveedor`
     - `Comprador`, `Cve`, `Usuario`, `NumComprador`

2. **Listar**:
   - La vista `Index` muestra hasta 50 registros paginados, con filtro por título, artículo o comprador.

3. **Buscar**:
   - `/api/catalogo/buscar-articulos`: Consulta con joins entre `invart`, `inviar` y `cprprv`.
   - `/api/catalogo/buscar-compradores`: Consulta a `cprcom` cruzada con un diccionario estático de compradores clave.

---

## Recomendaciones de extensión
- Convertir el diccionario de compradores en tabla parametrizable
- Agregar validaciones de duplicados
- Agregar filtros por proveedor o usuario
- Exportar a Excel los resultados

---

## Ubicación de Archivos
- `Controllers/CatalogoController.cs`
- `Services/CatalogoService.cs`
- `Services/ICatalogoService.cs`
- `Models/ArticuloComprador.cs`
- `Views/Catalogo/*.cshtml`

---

## Dependencias
- Microsoft.EntityFrameworkCore.SqlServer
- Select2 v4+ (para la experiencia en combos)

---

## Autor


---

> Este módulo fue diseñado con enfoque **modular, escalable y desacoplado**, alineado con buenas prácticas de arquitectura en .NET.

