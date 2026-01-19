export class AuthModule {
    private context: HTMLElement;

    constructor(context: HTMLElement){
        this.context = context
        this.init();
    }

    submitForm() {
        const form = this.context.querySelector('form');
        form.action = '/Auth/Login';
        form.method = 'POST';

        console.log('Submitting form via AuthModule...', form);
        document.body.appendChild(form);
        form.submit();
    }

    addToWindownScope(): void {
        if (!(window as any).nextAuth) {
            (window as any).nextAuth = {};
        }

        (window as any).nextAuth.submitForm = this.submitForm.bind(this);
    }

    init() {
        this.addToWindownScope();
    }
}