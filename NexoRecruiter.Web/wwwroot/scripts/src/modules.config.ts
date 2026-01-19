// modules.config.js
// import { GridModule } from './modules/grid.js';
// import { TabsModule } from './modules/tabs.js';
// import { CarouselModule } from './modules/carousel.js';

import { AuthModule } from "./modules/auth-module.ts";

/**
 * Configuración de módulos de la aplicación
 * Punto central para registrar todos los módulos disponibles
 */
export const APP_MODULES = {
    // 'grid': GridModule,
    // 'tabs': TabsModule,
    // 'carousel': CarouselModule
    'auth': AuthModule
};