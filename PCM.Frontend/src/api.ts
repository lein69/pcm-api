import axios, { type AxiosInstance } from "axios";

const api: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL as string,
  withCredentials: true,
});

// ================= INTERCEPTORS =================

// Request: Gắn token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);

// Response: Xử lý lỗi chung
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Có thể handle 401 ở đây sau
    return Promise.reject(error);
  }
);

// ================= EXPORT =================

export default api;
