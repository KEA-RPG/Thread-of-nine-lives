import axios, { AxiosError, AxiosInstance } from 'axios';
import { useEffect, useState } from 'react';

export interface Response<T> {
  data: T | undefined;
  error: AxiosError | null;
}

class ApiClient {
  public apiClient: AxiosInstance | undefined;
  
  private getClient(): AxiosInstance {
    let baseUrl = "https://localhost:7195/";
    const envBaseUrl = import.meta.env.VITE_BASE_URL;

    if (envBaseUrl) {
      baseUrl = envBaseUrl;
    }

    if (!this.apiClient) {
      this.apiClient = axios.create({
        baseURL: baseUrl,
        timeout: 10000,



        withCredentials: true,

        headers: {
          'Content-Type': 'application/json',
        }
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

    // Attach Bearer JWT if present
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }


    if (antiForgeryToken) {
      headers["X-CSRF-TOKEN"] = antiForgeryToken;
    }

    return headers;
  }


  get<T>(url: string): Response<T> {
    const [data, setData] = useState<T>();
    const [error, setError] = useState(null);

    useEffect(() => {
      this.getClient()
        .get<T>(url, { headers: this.getHeaders() })
        .then((response) => setData(response.data))
        .catch((err) => setError(err));
      // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return { data, error };
  }

  async post<TBody, TReturn>(url: string, body: TBody): Promise<Response<TReturn>> {
    try {
      const response = await this.getClient().post<TReturn>(url, body, {
        headers: this.getHeaders(),
      });
      return { data: response.data, error: null };
    } catch (error: any) {
      console.error("Error making POST request:", error);
      return { data: undefined, error };
    }
  }

  // PUT
  async put<TBody, TReturn>(url: string, body: TBody): Promise<Response<TReturn>> {
    try {
      const response = await this.getClient().put<TReturn>(url, body, {
        headers: this.getHeaders()
      });
      return { data: response.data, error: null };
    } catch (error: any) {
      return { data: undefined, error: error };
    }
  }

  // DELETE
  async delete<T>(url: string): Promise<Response<T>> {
    try {
      const response = await this.getClient().delete<T>(url, {
        headers: this.getHeaders()
      });
      return { data: response.data, error: null };
    } catch (error: any) {
      return { data: undefined, error: error };
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
