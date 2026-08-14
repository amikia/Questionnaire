const API_BASE_URL = "http://localhost:5068/api";

export async function apiClient<T>(
    endpoint: string,
    options: RequestInit = {}
): Promise<T> {

    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
        ...options,
        credentials: "include",
        headers: {
            "Content-Type": "application/json",
            ...options.headers
        }
    });

    if (!response.ok) {
        throw new Error(`API request failed: ${response.status}`);
    }

    return response.json();
}