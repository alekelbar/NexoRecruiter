/**
 * Sistema de eventos pub/sub para comunicación entre módulos
 */
export class EventBus {
    private events: Map<string, Function[]>;
    
    constructor() {
        this.events = new Map();
    }

    /**
     * Suscribe un callback a un evento
     * @param {string} eventName - Nombre del evento
     * @param {Function} callback - Función a ejecutar cuando se emita el evento
     * @returns {Function} Función para desuscribirse del evento
     */
    on(eventName: string, callback: Function) {
        if (!this.events.has(eventName)) {
            this.events.set(eventName, []);
        };

        /** @type {Function[]} */
        const callbacks = this.events.get(eventName);
        callbacks.push(callback);

        return () => {
            this.off(eventName, callback);
        }
    }

    /**
     * Desuscribe un callback de un evento
     * @param {string} eventName - Nombre del evento
     * @param {Function} callback - Función a eliminar
     */
    off(eventName: string, callback: Function) {
        if (!this.events.has(eventName)) return;

        const callbacks = this.events.get(eventName);
        const filteredCallbacks = callbacks.filter(cb => cb !== callback);

        this.events.set(eventName, filteredCallbacks);
    }

    /**
     * Emite un evento y ejecuta todos los callbacks suscritos
     * @param {string} eventName - Nombre del evento a emitir
     * @param {...any} args - Argumentos a pasar a los callbacks
     */
    emit(eventName: string, ...args: any[]) {
        if (!this.events.has(eventName)) return;

        const callbacks = this.events.get(eventName);
        callbacks.forEach(cb => {
            try {
                cb(...args);
            } catch (error) {
                console.warn('Error:', error);
            }
        })
    }

    /**
     * Suscribe un callback que se ejecutará solo una vez
     * @param {string} eventName - Nombre del evento
     * @param {Function} callback - Función a ejecutar (se auto-desuscribe después)
     */
    once(eventName: string, callback: Function) {
        if (!this.events.get(eventName)) {
            this.events.set(eventName, []);
        };

        const wrapper = (...args) => {
            callback(...args);
            this.off(eventName, wrapper);
        }

        this.on(eventName, wrapper);
    }
}