import { apiClient } from "./client";
import type { LoginRequest, LoginResponse } from "../models/auth";

export async function login(
    request: LoginRequest
): Promise<LoginResponse> {

    return apiClient<LoginResponse>("/Authorization/AuthorizeWithPassword", {
        method: "POST",
        body: JSON.stringify(request)
    });
}