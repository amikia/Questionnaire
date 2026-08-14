export async function router(): Promise<void> {
    const path = window.location.pathname;

    switch (path) {
        case "/":
            navigate("/login");
            break;

        case "/login":
            await import("../pages/login/login");
            break;

        default:
            navigate("/login");
            break;
    }
}

function navigate(path: string): void {
    window.history.replaceState({}, "", path);
    router();
}