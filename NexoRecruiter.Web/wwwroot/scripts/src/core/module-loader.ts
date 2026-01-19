/**
 * Cargador automático de módulos mediante atributos HTML
 * Permite inicializar módulos usando data-module sin código manual
 */
export class ModuleLoader {
    private modules: Map<string, any>;
    private instances: WeakMap<HTMLElement, any>;

    constructor() {
        this.modules = new Map();
        this.instances = new WeakMap();
    }

    /**
     * Registra un módulo para inicialización automática
     * @param {string} name - Nombre del módulo (debe coincidir con data-module)
     * @param {Function} ModuleClass - Clase del módulo a instanciar
     * @returns {ModuleLoader} Instancia actual para encadenar llamadas
     * @example
     * moduleLoader.register('grid', GridModule)
     *             .register('tabs', TabsModule);
     */
    register(name: string, ModuleClass: any) {
        if (!name || typeof name !== 'string') {
            console.error('Nombre de módulo inválido');
            return;
        }

        if (typeof ModuleClass !== 'function') {
            console.error(`"${name}" no es una clase válida`);
            return;
        }

        if (this.modules.has(name)) {
            console.warn(`Módulo "${name}" ya registrado, sobrescribiendo...`);
        }
        this.modules.set(name, ModuleClass);

        return this;
    }

    /**
     * Inicializa todos los módulos encontrados en el contexto
     * Busca elementos con [data-module] y crea instancias automáticamente
     * @param {Document | HTMLElement} [context=document] - Contexto de búsqueda
     * @returns {number} Número de módulos inicializados exitosamente
     * @example
     * // Inicializar todos los módulos del documento
     * moduleLoader.init();
     * 
     * // Inicializar solo en un contenedor específico
     * const container = document.querySelector('.dynamic-content');
     * moduleLoader.init(container);
     */
    init(context: Document | HTMLElement = document) {
        const elements: NodeListOf<HTMLElement> = context.querySelectorAll('[data-module]');
        let initialized = 0;

        elements.forEach(element => {
            if (this.instances.has(element)) {
                console.warn('Elemento ya inicializado, saltando...');
                return;
            }

            const moduleName = element.dataset.module;

            if (!this.modules.has(moduleName)) {
                console.warn(`Módulo "${moduleName}" no registrado`);
                return;
            }

            const ModuleClass = this.modules.get(moduleName);

            try {
                const newInstance = new ModuleClass(element);
                this.instances.set(element, newInstance);
                initialized++;  // ✅ Contar éxitos
            } catch (error) {
                console.error(`Error al inicializar "${moduleName}":`, error);
            }
        });

        console.log(`✅ ${initialized} módulos inicializados`);
        return initialized;
    }

    /**
     * Destruye la instancia de módulo asociada a un elemento
     * Llama al método destroy() de la instancia si existe y limpia referencias
     * @param {HTMLElement} element - Elemento cuya instancia se debe destruir
     * @example
     * const element = document.querySelector('[data-module="grid"]');
     * moduleLoader.destroy(element);
     */
    destroy(element: HTMLElement) {
        if (!this.instances.has(element)) {
            return;
        }
        const instance = this.instances.get(element);
        if (typeof instance.destroy === 'function') {
            try {
                instance.destroy();
            } catch (error) {
                console.error('Error al destruir módulo:', error);
            }
        }
        this.instances.delete(element);
    }
}