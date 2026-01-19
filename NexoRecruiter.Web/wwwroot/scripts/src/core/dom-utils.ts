/**
 * Utilidades para manipulación del DOM
 */
export class DomUtils {

    /**
     * Selecciona un elemento del DOM
     * @param {string} selector - Selector CSS
     * @param {Document | HTMLElement} [context=document] - Contexto de búsqueda
     * @returns {HTMLElement | null} Elemento encontrado o null
     */
    select(selector: string, context: Document | HTMLElement = document) {
        return context.querySelector(selector);
    }

    /**
     * Selecciona todos los elementos que coincidan con el selector
     * @param {string} selector - Selector CSS
     * @param {Document | HTMLElement} [context=document] - Contexto de búsqueda
     * @returns {NodeListOf<HTMLElement>} Lista de elementos encontrados
     */
    selectAll(selector: string, context: Document | HTMLElement = document) {
        return context.querySelectorAll(selector);
    }

    /**
     * Ejecuta un callback cuando el DOM esté listo
     * @param {Function} callback - Función a ejecutar
     */
    ready(callback: () => void) {
        if (document.readyState !== 'loading') {
            callback();
        } else {
            document.addEventListener('DOMContentLoaded', callback);
        }
    }

    /**
     * Añade un event listener a un elemento
     * @param {HTMLElement} element - Elemento del DOM
     * @param {keyof HTMLElementEventMap} event - Nombre del evento
     * @param {EventListener} handler - Función callback
     */
    on(element: HTMLElement | null, event: keyof HTMLElementEventMap, handler: EventListener) {
        if (element == null) {
            console.warn(`DomUtils.on: Element is null for event '${event}'`);
            return;
        }
        element.addEventListener(event, handler);
    }

    /**
     * Event delegation - escucha eventos en elementos hijos actuales y futuros
     * @param {HTMLElement} parent - Elemento padre contenedor
     * @param {string} selector - Selector CSS de los elementos hijos
     * @param {keyof HTMLElementEventMap} event - Nombre del evento
     * @param {EventListener} handler - Función callback (this será el elemento clickeado)
     */
    delegate(parent: HTMLElement, selector: string, event: keyof HTMLElementEventMap, handler: EventListener) {
        parent.addEventListener(event, (e) => {
            const target = (e.target as HTMLElement).closest(selector);  

            if (target) {  
                handler.call(target, e);  
            }
        });
    }
};