import axios from 'axios';

const apiClient = axios.create({
  // Sử dụng biến môi trường hoặc fallback về localhost
  baseURL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:7183/api',
  headers: {
    'Content-Type': 'application/json'
  }
});

// Request interceptor: Tự động thêm token vào header
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor: Xử lý lỗi chung (ví dụ: 401 Unauthorized)
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    return Promise.reject(error);
  }
);

export default apiClient;