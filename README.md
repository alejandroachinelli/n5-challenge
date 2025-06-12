# N5 Challenge – Fullstack Application

Este repositorio contiene la solución al challenge técnico de N5 Company, implementado con una arquitectura robusta y moderna. El sistema permite gestionar permisos de empleados, integrando una API .NET 8, mensajería con Kafka, indexación en Elasticsearch y un frontend ReactJS.

---

## 📦 Estructura del Proyecto

```
/
├── src/                         # Código fuente backend (.NET 8)
├── test/                        # Tests unitarios
├── docs/                        # Documentación técnica
├── n5challenge-frontend/       # Aplicación ReactJS (Vite + MUI)
├── docker-compose.yml          # Servicios: API, SQL Server, Kafka, Elasticsearch, Frontend
├── .gitignore                  # Ignora archivos innecesarios
├── README.md                   # Este archivo
```

---

## 🧪 Tecnologías Utilizadas

### Backend (.NET 8)
- ASP.NET Core Web API
- Entity Framework Core + SQL Server
- MediatR + CQRS
- Apache Kafka
- Elasticsearch
- AutoMapper
- Docker / Docker Compose

### Frontend (React)
- ReactJS + Vite
- Axios
- React Router
- Material UI (MUI)
- TypeScript

---

## ⚙️ Instrucciones para ejecutar el proyecto

### ✅ Requisitos previos

- [Docker](https://www.docker.com/)
- [Node.js](https://nodejs.org/) (solo si querés ejecutar React sin Docker)

---

### 🐳 Ejecutar TODO con Docker

Desde la carpeta base donde se encuentra la solucion del proyecto backend, abrimos una consola para ejecutar el siguiente comando:

```bash
docker compose up -d --build
```

Esto levanta:
- API .NET en `http://localhost:8080`
- Frontend React en `http://localhost:3000`
- SQL Server, Kafka, Elasticsearch, Kibana (puertos configurados internamente)

---

### 🔧 Comandos Útiles

#### Crear migraciones (si hacés cambios en la DB):
```bash
cd src
dotnet ef migrations add NombreMigracion -s Services/N5Challenge.Services.WebAPI
```

#### Ejecutar tests:
```bash
dotnet test
```

---

## 🧪 Funcionalidades Implementadas

- ✅ Crear, editar, eliminar y listar permisos
- ✅ Relación con tipos de permisos
- ✅ Consumo de API desde React con Axios
- ✅ Manejo de errores y validaciones
- ✅ Publicación de eventos a Kafka al crear/modificar permisos
- ✅ Indexación automática en Elasticsearch
- ✅ Búsquedas desde Kibana
- ✅ Diseño responsive con Material UI
- ✅ Dockerizado completamente

---

## 👤 Autor

**Alejandro Martín Achinelli**  
[GitHub](https://github.com/alejandroachinelli) – [LinkedIn](https://linkedin.com/in/alejandroachinelli)

---

## 📝 Notas

> El proyecto está desarrollado con fines evaluativos.  
> Se priorizó el uso de buenas prácticas, separación por capas, y estándares modernos de desarrollo.