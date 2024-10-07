import axios from "axios"

const apiClient = axios.create({
  baseURL: "https://localhost:7195/",
  timeout: 1000,
});

export default apiClient