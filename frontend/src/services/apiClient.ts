// apiCaller.ts
import axios, { AxiosError, AxiosInstance } from 'axios';
import { useEffect, useState } from 'react';
export interface Response<T> {
  data: T | undefined;
  error: AxiosError | null;
}

class ApiClient {
  public apiClient: AxiosInstance | undefined;

  private getClient(): AxiosInstance {
    if (!this.apiClient) {
      this.apiClient = axios.create({
        baseURL: "https://localhost:7195/",
        timeout: 10000,
        headers: {
          ContentType: 'application/json',
        },
        withCredentials: true,   
        
      });
    }
    return this.apiClient;
  }

  private getHeaders() {
    const headers: Record<string, string> = {
        "Content-Type": "application/json",
    };
    const token = this.getToken();
    const antiForgeryToken = localStorage.getItem("antiForgeryToken");

    if (token) {
        headers["Authorization"] = `Bearer ${token}`;
    }

    if (antiForgeryToken) {
        headers["X-CSRF-TOKEN"] = antiForgeryToken; // Include CSRF token
        headers["RequestVerificationToken"] = antiForgeryToken; // Include CSRF token
    }

    return headers;
}

  

  get<T>(url: string): Response<T> {
    const [data, setData] = useState<T>();
    const [error, setError] = useState(null);

    useEffect(() => {
      this.getClient().get<T>(`${url}`, {
        headers: this.getHeaders()
      }).then((response) => setData(response.data))
        .catch((response) => {
          setError(response)
        })
    }, []);

    return { data, error };
  }

  async post<TBody, TReturn>(url: string, body: TBody): Promise<Response<TReturn>> {
    try {
        console.log("Posting with URL:", url);
        console.log("With credentials?", this.getClient().defaults.withCredentials);

        const response = await this.getClient().post<TReturn>(`${url}`, body, {
            headers: this.getHeaders(),
            withCredentials: true // Ensure cookies are included in all requests
        });
        return { data: response.data, error: null };
    } catch (error: any) {
        console.error("Error making POST request:", error);
        return { data: undefined, error: error };
    }
}


  // put method
  async put<TBody, TReturn>(url: string, body: TBody): Promise<Response<TReturn>> {
    try {
      const response = await this.getClient().put<TReturn>(`${url}`, body, {
        headers: this.getHeaders()
      });
      return { data: response.data, error: null };
    } catch (error: any) {
      return { data: undefined, error: error.message };
    }
  }

  async delete<T>(url: string): Promise<Response<T>> {
    try {
      const response = await this.getClient().delete(`${url}`, {
        headers: this.getHeaders()
      });
      return { data: response.data, error: null };
    } catch (error: any) {
      return { data: undefined, error: error.message };
    }
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
    if (this.apiClient) {
      this.apiClient.defaults.headers['Authorization'] = token ? `Bearer ${token}` : '';
    }
  }

  getToken() {
    return localStorage.getItem('token');
  }
}

export default ApiClient;
