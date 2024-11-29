export interface AntiForgeryResponse {
    requestToken: string;
  }
  
  /**
   * Fetches and stores the anti-CSRF token directly using the Fetch API.
   * Should be called during initialization (e.g., after login).
   */
  const fetchAntiForgeryToken = async (): Promise<void> => {
    try {
        const response = await fetch("https://localhost:7195/auth/antiforgery-token", {
            method: "GET",
            credentials: "include", // Include cookies
            headers: {
                "Content-Type": "application/json",
            },
        });

        if (!response.ok) {
            throw new Error(`Failed to fetch anti-forgery token: ${response.status}`);
        }

        const data: AntiForgeryResponse = await response.json();
        if (data?.requestToken) {
            // Store the token in localStorage
            localStorage.setItem("antiForgeryToken", data.requestToken);
        } else {
            console.error("Anti-forgery token missing in response.");
        }
    } catch (error) {
        console.error("Error fetching anti-forgery token:", error);
    }
};

  
  /**
   * Retrieves the anti-CSRF token from localStorage.
   * Returns null if the token is not available.
   */
  const getAntiForgeryToken = (): string | null => {
    return localStorage.getItem("antiForgeryToken");
  };
  
  export { fetchAntiForgeryToken, getAntiForgeryToken };
  