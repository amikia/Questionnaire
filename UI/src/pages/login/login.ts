import "./login.css";

import { login } from "../../api/auth.api";
import type { LoginRequest } from "../../models/auth";


const form =
    document.querySelector<HTMLFormElement>("#loginForm");

const usernameInput =
    document.querySelector<HTMLInputElement>("#username");

const passwordInput =
    document.querySelector<HTMLInputElement>("#password");

const loginButton =
    document.querySelector<HTMLButtonElement>("#loginButton");

const errorMessage =
    document.querySelector<HTMLParagraphElement>("#errorMessage");


if (
    !form ||
    !usernameInput ||
    !passwordInput ||
    !loginButton ||
    !errorMessage
) {
    throw new Error("Login page elements were not found.");
}


form.addEventListener("submit", async (event) => {

    event.preventDefault();

    errorMessage.textContent = "";

    const username = usernameInput.value.trim();
    const password = passwordInput.value;


    if (!username || !password) {

        errorMessage.textContent =
            "Username and password are required.";

        return;
    }


    const request: LoginRequest = {
        username,
        password
    };


    loginButton.disabled = true;
    loginButton.textContent = "Logging in...";


    try {

        // Call the API
        const response = await login(request);

        console.log("Login successful:", response);

        // Show success notification
        showNotification(
            "Successfully logged in!",
            "success"
        );

        // Wait before navigating
        setTimeout(() => {
            window.location.href = "/dashboard";
        }, 1500);

    }
    catch (error) {

        console.error("Login failed:", error);

        showNotification(
            error instanceof Error
                ? error.message
                : "Login failed.",
            "error"
        );

        errorMessage.textContent =
            error instanceof Error
                ? error.message
                : "Login failed.";

    }
    finally {

        loginButton.disabled = false;
        loginButton.textContent = "Login";
    }
});


function showNotification(
    message: string,
    type: "success" | "error"
): void {

    const notification =
        document.createElement("div");

    notification.className =
        `notification ${type}`;

    notification.textContent = message;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.remove();
    }, 3000);
}