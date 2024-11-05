// apiCaller.ts
import axios, { AxiosInstance } from 'axios';
export interface Response<T> {
  data: T | undefined;
  error: string | null;
  isLoading: boolean;
}

class ApiCaller {
  public apiClient: AxiosInstance | undefined;

  public getClient() : AxiosInstance {
    if (!this.apiClient) {
      this.apiClient = axios.create({
        baseURL: "https://localhost:7195/",
        timeout: 10000,
        headers: {
          ContentType: 'application/json',
        }
      });
    }
    console.log(this.getToken());
    return this.apiClient;
  }

  private getHeaders() {
    const headers: Record<string, string> = {
      'Content-Type': 'application/json',
    };
    if (this.getToken()) {
      headers['Authorization'] = `Bearer ${this.getToken()}`;
    }
    return headers;
  }

  async get<T>(url: string): Promise<Response<T>> {
    let data: T | undefined;
    let error: string | null = null;
    let isLoading = true;

    await this.getClient().get<T>(`${url}`, {
      headers: this.getHeaders()
    })
      .then((response) => data = response.data)
      .catch((response) => error = response.message)
      .finally(() => isLoading = false);

    return { data, error, isLoading };
  }

  async post<TBody,TReturn>(url: string, body: TBody ): Promise<Response<TReturn>> {
    let data: TReturn | undefined;
    let error: string | null = null;
    let isLoading = true;

    await this.getClient().post<TReturn>(`${url}`,body, {
      headers: this.getHeaders()
    })
      .then((response) => data = response.data)
      .catch((response) => error = response.message)
      .finally(() => isLoading = false);

    return { data, error, isLoading };
  }

  async put<TBody,TReturn>(url: string, body: TBody ): Promise<Response<TReturn>> {
    let data: TReturn | undefined;
    let error: string | null = null;
    let isLoading = true;

    await this.getClient().put<TReturn>(`${url}`,body, {
      headers: this.getHeaders()
    })
      .then((response) => data = response.data)
      .catch((response) => error = response.message)
      .finally(() => isLoading = false);

    return { data, error, isLoading };
  }

  async delete<T>(url: string): Promise<Response<T>> {
    let data: T | undefined;
    let error: string | null = null;
    let isLoading = true;

    this.getClient().delete<T>(`${url}`, {
      headers: this.getHeaders()
    })
      .then((response) => data = response.data)
      .catch((response) => error = response.message)
      .finally(() => isLoading = false);

    return { data, error, isLoading };
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

export default ApiCaller;
