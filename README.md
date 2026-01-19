# NexoRecruiter App

Sistema de reclutamiento y gestión de candidatos construido con Blazor Server y ASP.NET Core 9.0.

## 📋 Tabla de Contenidos

- [Pre-requisitos](#pre-requisitos)
- [Instalación](#instalación)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Desarrollo](#desarrollo)
  - [Flujo de Trabajo CSS/SCSS](#flujo-de-trabajo-cssscss)
  - [Flujo de Trabajo JavaScript](#flujo-de-trabajo-javascript)
- [Arquitectura CSS](#arquitectura-css)
- [Comandos Útiles](#comandos-útiles)
- [Tecnologías](#tecnologías)

---

## 🔧 Pre-requisitos

- **.NET 9.0 SDK** - [Descargar](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Node.js 18+** (para compilar SCSS y TypeScript) - [Descargar](https://nodejs.org/)
- **pnpm** (gestor de paquetes) - `npm install -g pnpm`
- **SQL Server** (LocalDB o instancia completa)
- **Visual Studio 2022** o **VS Code** (recomendado)

---

## 📦 Instalación

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd NexoRecruiterApp
```

### 2. Restaurar dependencias .NET

```bash
dotnet restore
```

### 3. Instalar dependencias Node.js

```bash
cd NexoRecruiter.Web
pnpm install
```

### 4. Configurar base de datos

Actualiza la cadena de conexión en `NexoRecruiter.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NexoRecruiterDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 5. Ejecutar migraciones

```bash
# Desde la raíz del proyecto
dotnet ef database update --project NexoRecruiter.Infrastructure --startup-project NexoRecruiter.Web
```

### 6. Compilar assets (SCSS y TypeScript)

```bash
cd NexoRecruiter.Web

# Compilar SCSS
npx sass wwwroot/styles/main.scss wwwroot/styles/main.css

# Compilar TypeScript
pnpm run build
```

### 7. Ejecutar la aplicación

```bash
dotnet run
# O con hot-reload
dotnet watch
```

La aplicación estará disponible en:
- HTTPS: `https://localhost:7176`
- HTTP: `https://localhost:5110`

---

## 📁 Estructura del Proyecto

```
NexoRecruiterApp/
├── NexoRecruiter.Domain/          # Entidades, repositorios, enums
│   ├── Entities/
│   ├── Enums/
│   └── repositories/
├── NexoRecruiter.Application/     # Lógica de negocio, DTOs, servicios
│   ├── DTOs/
│   ├── Services/
│   └── Validators/
├── NexoRecruiter.Infrastructure/  # Persistencia, DbContext, migrations
│   ├── Persistence/
│   ├── Repositories/
│   └── Migrations/
└── NexoRecruiter.Web/             # Blazor Server UI
    ├── Features/                  # Componentes por feature
    │   ├── Auth/
    │   ├── Dashboard/
    │   ├── Jobs/
    │   └── Shared/
    ├── wwwroot/
    │   ├── scripts/
    │   │   ├── src/               # TypeScript source
    │   │   │   ├── core/          # Módulos core (loader, event-bus, utils)
    │   │   │   ├── modules/       # Módulos de features
    │   │   │   ├── app.ts         # Entry point principal
    │   │   │   └── modules.config.ts
    │   │   └── dist/              # JavaScript compilado (generado por Vite)
    │   │       └── app.bundle.js
    │   ├── styles/                # SCSS organizado con ITCSS
    │   │   ├── 00-settings/       # Variables, configuración
    │   │   ├── 01-base/           # Resets, tipografía base
    │   │   ├── 02-layouts/        # Contenedores, grid
    │   │   ├── 03-components/     # Botones, cards reutilizables
    │   │   ├── 04-features/       # Estilos por página/feature
    │   │   ├── 05-utilities/      # Clases helper
    │   │   ├── main.scss          # Punto de entrada SCSS
    │   │   └── main.css           # CSS compilado (generado)
    │   └── images/
    ├── package.json               # Dependencias Node.js
    ├── pnpm-lock.yaml
    ├── tsconfig.json              # Configuración TypeScript
    └── vite.config.js             # Configuración Vite bundler
```

---

## 🛠️ Desarrollo

### Flujo de Trabajo CSS/SCSS

Este proyecto usa **SCSS** (Sass) con la metodología **ITCSS** (Inverted Triangle CSS) y **BEM** para nombres de clases.

#### Arquitectura ITCSS

Los estilos están organizados por especificidad creciente:

```
00-settings/   → Variables ($color-primary, $spacing-unit, mixins)
01-base/       → Normalize, reset, tipografía base
02-layouts/    → Estructura (containers, grid)
03-components/ → Componentes reutilizables (botones, cards)
04-features/   → Estilos específicos de páginas (login, dashboard)
05-utilities/  → Clases helper (.mt-2, .p-3, .text-center)
```

#### Convención BEM

Usa **Block__Element--Modifier** para nombres de clases:

```scss
// ✅ Correcto
.login-card { }                  // Block
.login-card__header { }          // Element
.login-card__header--active { }  // Modifier

// ❌ Evitar nombres genéricos
.card { }
.header { }
```

#### Compilación Automática

El proyecto incluye **DartSassBuilder** que compila SCSS automáticamente:

```bash
# Con dotnet watch (recomendado)
dotnet watch

# DartSassBuilder detecta cambios en *.scss y recompila
```

#### Compilación Manual

Si necesitas compilar manualmente:

```bash
# Compilar una vez
npx sass wwwroot/styles/main.scss wwwroot/styles/main.css

# Modo watch (recompila al detectar cambios)
npx sass --watch wwwroot/styles/main.scss wwwroot/styles/main.css

# Con sourcemaps (para debugging)
npx sass --watch --source-map wwwroot/styles/main.scss wwwroot/styles/main.css
```

#### Crear Nuevos Estilos

1. **Determina la capa ITCSS correcta:**
   - ¿Es una variable? → `00-settings/`
   - ¿Es un componente reutilizable? → `03-components/`
   - ¿Es específico de una página? → `04-features/`
   - ¿Es una clase helper? → `05-utilities/`

2. **Crea un partial (archivo con `_`):**
   ```scss
   // 03-components/_modal.scss
   .modal {
     position: fixed;
     z-index: 1000;
     
     &__header {
       padding: $spacing-lg;
     }
     
     &--large {
       max-width: 800px;
     }
   }
   ```

3. **Importa en `main.scss`:**
   ```scss
   // main.scss
   @import '03-components/modal';
   ```

4. **Usa en Razor:**
   ```razor
   <div class="modal modal--large">
     <div class="modal__header">
       <h2>Título</h2>
     </div>
   </div>
   ```

#### Variables SCSS Disponibles

```scss
// Colores
$color-primary: #594AE2;
$color-secondary: #2D3748;
$color-success: #48BB78;
$color-danger: #E53E3E;

// Spacing (sistema 8px)
$spacing-unit: 8px;
$spacing-xs: 4px;
$spacing-sm: 8px;
$spacing-md: 16px;
$spacing-lg: 24px;
$spacing-xl: 32px;
$spacing-2xl: 48px;

// Tipografía
$font-family-base: 'Inter', sans-serif;
$font-size-sm: 0.875rem;
$font-size-base: 1rem;
$font-size-lg: 1.125rem;

// Breakpoints
$breakpoints: (
  sm: 640px,
  md: 768px,
  lg: 1024px,
  xl: 1280px,
  2xl: 1536px
);
```

#### Usar Mixins Responsivos

```scss
.login-page__left-side {
  padding: $spacing-md;
  
  @include media(lg) {
    padding: $spacing-2xl;
  }
  
  @include media(2xl) {
    max-width: 1200px;
  }
}
```

---

### Flujo de Trabajo TypeScript + Vite

Este proyecto usa **TypeScript** compilado con **Vite** para bundling y optimización.

#### Arquitectura TypeScript

**Patrón de Module Loader:**
- Sistema de carga dinámica mediante atributos `data-module` en HTML
- Event Bus para comunicación desacoplada entre módulos
- Registro centralizado en `modules.config.ts`

**Estructura:**
```
wwwroot/scripts/src/
├── core/
│   ├── module-loader.ts    # Cargador automático de módulos
│   ├── event-bus.ts        # Sistema pub/sub
│   └── dom-utils.ts        # Helpers DOM
├── modules/
│   └── auth-module.ts      # Módulos específicos por feature
├── app.ts                  # Entry point principal
└── modules.config.ts       # Registro de módulos
```

#### Compilación Automática

El proyecto está configurado para compilar TypeScript automáticamente:

**En desarrollo (manual):**
```bash
# Terminal 1: TypeScript en watch mode
pnpm run dev

# Terminal 2: .NET en watch mode
cd NexoRecruiter.Web
dotnet watch
```

**En producción (automático):**
```bash
# Build de Release compila TS automáticamente
dotnet build -c Release

# Publish también
dotnet publish -c Release
```

#### Compilación Manual

Si necesitas compilar TypeScript manualmente:

```bash
# Compilar una vez
pnpm run build

# Watch mode (recompila al detectar cambios)
pnpm run dev

# Validar tipos sin compilar
pnpm run type-check
```

Esto genera: `wwwroot/scripts/dist/app.bundle.js`

#### Configuración Vite

El bundler está configurado para:
- **Formato:** IIFE (bundle único autoejecutable)
- **Output:** `wwwroot/scripts/dist/app.bundle.js`
- **Minificación:** esbuild (rápida)
- **Sourcemaps:** Habilitados en desarrollo
- **Target:** ES2022 (navegadores modernos)

#### Crear Nuevos Módulos TypeScript

1. **Define la interfaz del módulo:**
   ```ts
   // src/modules/my-module.ts
   import { Module } from '../core/module-loader.ts';
   
   export class MyModule implements Module {
     context: HTMLElement;
     
     constructor(context: HTMLElement) {
       this.context = context;
       this.init();
     }
     
     init(): void {
       // Inicialización del módulo
     }
   }
   ```

2. **Regístralo en `modules.config.ts`:**
   ```ts
   import { MyModule } from "./modules/my-module.ts";
   
   export const APP_MODULES: Record<string, ModuleClass> = {
     'auth': AuthModule,
     'mymodule': MyModule  // ← Agregar aquí
   };
   ```

3. **Usa en HTML con `data-module`:**
   ```razor
   <div data-module="mymodule">
     <!-- El módulo se inicializa automáticamente -->
   </div>
   ```

#### Interoperabilidad con Blazor

Usa `IJSRuntime` para llamar funciones JavaScript desde C#:

```razor
@inject IJSRuntime JSRuntime

@code {
    private async Task EjecutarJS()
    {
        // Llamar a función global en window
        await JSRuntime.InvokeVoidAsync("console.log", "Hola desde Blazor");
        
        // Llamar a función del bundle (expuesta en window.NexoApp)
        var resultado = await JSRuntime.InvokeAsync<string>("NexoApp.miMetodo", "parametro");
    }
}
```

**Ejemplo real con Module Loader:**

En TypeScript, los módulos exponen funciones a `window`:
```ts
// auth-module.ts
export class AuthModule implements Module {
  addToWindowScope(): void {
    if (!(window as any).nextAuth) {
      (window as any).nextAuth = {};
    }
    (window as any).nextAuth.submitForm = this.submitForm.bind(this);
  }
}
```

Desde Blazor:
```razor
@inject IJSRuntime JSRuntime

private async Task LoginAsync()
{
    // Llama a la función expuesta por el módulo
    await JSRuntime.InvokeVoidAsync("nextAuth.submitForm");
}
```

#### Debugging TypeScript

1. Abre DevTools (F12)
2. Ve a la pestaña **Sources**
3. Si los sourcemaps están habilitados, verás archivos `.ts` originales
4. Pon breakpoints en TypeScript directamente
5. Inspecciona `window.NexoApp` en la consola para ver tu aplicación

**Verificar bundle cargado:**
```javascript
// En la consola del navegador
console.log(window.NexoApp);  // Debe existir
console.log(window.app);       // Instancia de la aplicación
```

---

## 🎨 Arquitectura CSS

### Principios de Diseño

1. **Mobile-first:** Estilos base para móvil, media queries para pantallas grandes
2. **BEM naming:** Evita colisiones de nombres
3. **ITCSS layers:** Organización por especificidad
4. **DRY:** Variables y mixins para evitar repetición
5. **Modularidad:** Un archivo por componente/feature

### Guías de Estilo

#### ✅ Hacer

- Usa variables para colores, spacing, fuentes
- Anida máximo 3 niveles
- Sigue BEM para nombres (`.block__element--modifier`)
- Organiza en partials por responsabilidad
- Prefiere clases sobre IDs para estilos
- Usa mixins para patrones repetitivos

#### ❌ Evitar

- Anidar más de 4 niveles (código complejo)
- Nombres genéricos (`.card`, `.button`) sin namespace
- `!important` (indica mala especificidad)
- Estilos inline en Razor (usa clases)
- Duplicar valores (usa variables)

### Utilities Auto-generadas

El sistema genera clases de spacing automáticamente:

```scss
// Generadas por _spacing.scss
.mt-1 { margin-top: 8px; }
.mt-2 { margin-top: 16px; }
.mb-3 { margin-bottom: 24px; }
.p-4 { padding: 32px; }
// ... etc
```

Usa en componentes:

```razor
<div class="mt-3 p-2">
    <h1 class="mb-2">Título</h1>
</div>
```

---

## 🚀 Comandos Útiles

### .NET

```bash
# Ejecutar con hot-reload (desde NexoRecruiter.Web/)
cd NexoRecruiter.Web
dotnet watch

# Compilar solución en Debug (desde raíz)
dotnet build

# Compilar en Release (compila TypeScript automáticamente)
dotnet build -c Release

# Publish para producción
dotnet publish -c Release -o ./publish

# Ejecutar tests (desde raíz)
dotnet test

# Crear migración (desde raíz)
dotnet ef migrations add NombreMigracion --project NexoRecruiter.Infrastructure --startup-project NexoRecruiter.Web

# Actualizar base de datos (desde raíz)
dotnet ef database update --project NexoRecruiter.Infrastructure --startup-project NexoRecruiter.Web

# Revertir última migración (desde raíz)
dotnet ef database update MigracionAnterior --project NexoRecruiter.Infrastructure --startup-project NexoRecruiter.Web

# Eliminar última migración (desde raíz)
dotnet ef migrations remove --project NexoRecruiter.Infrastructure --startup-project NexoRecruiter.Web

# Limpiar build (también borra dist/ de TypeScript)
dotnet clean
```

### TypeScript + Vite

```bash
# Compilar TypeScript (una vez)
pnpm run build

# Watch mode (recompila automáticamente)
pnpm run dev

# Validar tipos sin compilar
pnpm run type-check

# Instalar dependencias
pnpm install
```

### SCSS

```bash
# Compilar SCSS (una vez)
npx sass wwwroot/styles/main.scss wwwroot/styles/main.css

# Watch mode (recompila automáticamente)
npx sass --watch wwwroot/styles/main.scss wwwroot/styles/main.css

# Compilar minificado (producción)
npx sass --style=compressed wwwroot/styles/main.scss wwwroot/styles/main.css

# Con sourcemaps (debugging)
npx sass --source-map wwwroot/styles/main.scss wwwroot/styles/main.css
```

### Git

```bash
# Estado
git status

# Commit
git add .
git commit -m "feat: descripción del cambio"

# Push
git push origin main
```

---

## 🔨 Tecnologías

### Backend

- **.NET 9.0** - Framework principal
- **Blazor Server** - UI framework con renderizado en servidor
- **Entity Framework Core 9.0** - ORM
- **ASP.NET Core Identity** - Autenticación y autorización
- **SQL Server** - Base de datos

### Frontend

- **MudBlazor 8.15** - Componentes UI Material Design
- **TypeScript 5.9** - Lenguaje tipado para JavaScript
- **Vite 7.3** - Bundler y build tool ultrarrápido
- **SCSS (Dart Sass)** - Preprocesador CSS
- **DartSassBuilder** - Compilación automática de SCSS
- **ITCSS** - Arquitectura CSS
- **BEM** - Convención de nombres CSS

### Arquitectura Frontend

- **Module Loader Pattern** - Carga dinámica de módulos TypeScript
- **Event Bus** - Comunicación desacoplada (pub/sub)
- **ES6 Modules** - Sistema de módulos moderno
- **IIFE Bundle** - Bundle único autoejecutable

### Herramientas de Desarrollo

- **Visual Studio 2022 / VS Code**
- **dotnet watch** - Hot reload para .NET
- **Vite** - HMR (Hot Module Replacement) para TypeScript
- **pnpm** - Gestor de paquetes rápido
- **Git** - Control de versiones

---

## 📝 Notas de Desarrollo

### Flujo de Trabajo Recomendado

**Desarrollo local:**
```bash
# Terminal 1: TypeScript en watch mode
pnpm run dev

# Terminal 2: .NET con hot reload
dotnet watch
```

Ambos procesos se recargan automáticamente al detectar cambios.

**Build de producción:**
```bash
# Compila todo automáticamente (SCSS + TypeScript + .NET)
dotnet build -c Release
```

**Publish para deploy:**
```bash
# Genera carpeta lista para producción
dotnet publish -c Release -o ./publish
```

El sistema validará que existan:
- `wwwroot/scripts/dist/app.bundle.js`
- `wwwroot/styles/main.css`

### Integración MSBuild

El `.csproj` está configurado para:

1. **Excluir archivos source del publish:**
   - No copia `scripts/src/` (TypeScript)
   - No copia archivos `.scss`
   - No copia `node_modules/`, `package.json`, configs

2. **Instalar dependencias automáticamente:**
   - Si no existe `node_modules/`, ejecuta `pnpm install`

3. **Compilar TypeScript en Release:**
   - `dotnet build -c Release` ejecuta `pnpm run build`
   - Genera bundle minificado

4. **Validar assets antes de publish:**
   - Verifica que existan bundles compilados
   - Falla con mensaje claro si faltan

5. **Limpiar archivos generados:**
   - `dotnet clean` también borra `dist/`

### Migraciones Pendientes de SCSS

El proyecto usa `@import` que será deprecado en Sass 3.0. Migración futura:

```scss
// Actual (deprecado)
@import '00-settings/colors';

// Futuro (recomendado)
@use '00-settings/colors';
```

### TypeScript: Strictness Gradual

El proyecto inicia con `strict: false` en `tsconfig.json` para facilitar la migración. Puedes ir activando opciones progresivamente:

```json
{
  "compilerOptions": {
    "strict": true,              // Activa todas las comprobaciones
    "noUnusedLocals": true,      // Error en variables no usadas
    "noUnusedParameters": true,  // Error en parámetros no usados
    "noImplicitReturns": true    // Require return en todas las ramas
  }
}
```

### Convenciones de Commits

Usa [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: Nueva funcionalidad
fix: Corrección de bug
docs: Cambios en documentación
style: Cambios de formato (CSS, código)
refactor: Refactorización de código
test: Añadir o modificar tests
chore: Tareas de mantenimiento
```

---

## 🤝 Contribuir

1. Crea un branch desde `main`
2. Realiza tus cambios
3. Asegúrate que compile sin errores
4. Commit con mensaje descriptivo
5. Crea Pull Request

---

## 📄 Licencia

[Incluir licencia del proyecto]

---

## 👥 Autores

- **[@alekelbar](https://github.com/alekelbar)** - Desarrollo principal
