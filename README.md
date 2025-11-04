# 🧾 TeamPoints Deployment

Despliegue de la aplicación **TeamPoints (ASP.NET + PostgreSQL)** usando Docker Compose.

## 🧱 Servicios
- **Backend:** ASP.NET 8
- **Database:** PostgreSQL 16
- **Servidor:** Ubuntu 22.04 en DigitalOcean
- **Proxy:** NGINX con SSL Let's Encrypt
- **Dominio:** [https://points.pointsumg.xyz](https://points.pointsumg.xyz)

## 🚀 Comandos principales

```bash
# Clonar proyecto
git clone https://github.com/tuusuario/TeamPoints-Deploy.git
cd TeamPoints-Deploy

# Desplegar
docker compose -f docker-compose.deploy.yml up -d

# Verificar servicios
docker compose ps
