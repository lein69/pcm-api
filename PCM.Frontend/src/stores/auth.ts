import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import apiClient from '@/api'

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('token') || '')
  const user = ref(null)

  const isAuthenticated = computed(() => !!token.value)

  const login = async (email: string, password: string) => {
    try {
      const response = await apiClient.post('/auth/login', {
        email,
        password
      })
      token.value = response.data.token
      localStorage.setItem('token', token.value)
      return true
    } catch (error) {
      console.error('Login failed:', error)
      return false
    }
  }

  const logout = () => {
    token.value = ''
    user.value = null
    localStorage.removeItem('token')
  }

  const checkAuth = () => {
    // The apiClient interceptor automatically handles attaching the token
    // from localStorage on every request. This function is kept for
    // compatibility with its usage in main.ts but its body is no longer needed.
  }

  return {
    token,
    user,
    isAuthenticated,
    login,
    logout,
    checkAuth
  }
})