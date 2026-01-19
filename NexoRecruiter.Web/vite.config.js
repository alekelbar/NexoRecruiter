import { defineConfig } from 'vite';
import { resolve } from 'path';

export default defineConfig({
  // Directorio raíz desde donde Vite lee archivos
  root: './wwwroot',
  
  // URL base para assets
  base: '/',
  
  build: {
    // Directorio de salida (relativo a root)
    outDir: 'scripts/dist',
    
    // Limpiar dist antes de cada build
    emptyOutDir: true,
    
    // Sourcemaps para debugging (puedes cambiar a false en producción)
    sourcemap: true,
    
    // Configuración de Rollup (bundler interno de Vite)
    rollupOptions: {
      // Entry point principal
      input: {
        app: resolve(__dirname, 'wwwroot/scripts/src/app.ts')
      },
      
      output: {
        // Formato IIFE (bundle tradicional, todo en un archivo)
        format: 'iife',
        
        // Nombre global de tu app en window
        name: 'NexoApp',
        
        // Nombre del archivo generado
        entryFileNames: 'app.bundle.js',
        
        // No generar chunks separados (todo en un bundle)
        manualChunks: undefined
      }
    },
    
    // Minificación (esbuild es más rápido, terser comprime mejor)
    minify: 'esbuild',
    
    // Target para navegadores modernos
    target: 'es2022'
  },
  
  // Configuración del servidor de desarrollo (no lo usarás probablemente)
  server: {
    port: 5173,
    open: false
  }
});
