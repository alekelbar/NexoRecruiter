// app.js
import { ModuleLoader } from './core/module-loader.ts';
import { EventBus } from './core/event-bus.ts';
import { DomUtils } from './core/dom-utils.ts';
import { APP_MODULES } from './modules.config.ts';

class App {
    private moduleLoader: ModuleLoader;
    private eventBus: EventBus;
    private dom: DomUtils;

    constructor() {
        this.moduleLoader = new ModuleLoader();
        this.eventBus = new EventBus();
        this.dom = new DomUtils();
        
        // Registrar módulos desde configuración
        this.registerModules(APP_MODULES);
    }
    
    /**
     * Registra módulos desde un objeto de configuración
     */
    registerModules(modules) {
        Object.entries(modules).forEach(([name, ModuleClass]) => {
            this.moduleLoader.register(name, ModuleClass);
        });
    }
    
    start() {
        this.dom.ready(() => {
            this.moduleLoader.init();
        });
        return this;
    }
}

const app = new App();
app.start();

(window as any).app = app;
export default app;