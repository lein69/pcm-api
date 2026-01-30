<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useAuthStore } from './stores/auth'
import { onMounted } from 'vue'

const authStore = useAuthStore()

const logout = () => {
  authStore.logout()
  window.location.href = '/login'
}

onMounted(() => {
  if (!authStore.isAuthenticated && window.location.pathname !== '/login') {
    window.location.href = '/login'
  }
})
</script>

<template>
  <div id="app">
    <header v-if="authStore.isAuthenticated" class="navbar">
      <div class="container">
        <div class="brand">
          <span class="logo-icon">🏓</span>
          <span class="logo-text">PCM Club</span>
        </div>

        <nav class="nav-links">
          <RouterLink to="/" class="nav-item">Dashboard</RouterLink>
          <RouterLink to="/members" class="nav-item">Hội viên</RouterLink>
          <RouterLink to="/courts" class="nav-item">Sân bãi</RouterLink>
          <RouterLink to="/bookings" class="nav-item">Đặt sân</RouterLink>
          <RouterLink to="/challenges" class="nav-item">Giải đấu</RouterLink>
          <RouterLink to="/matches" class="nav-item">Trận đấu</RouterLink>
          <RouterLink to="/transactions" class="nav-item">Tài chính</RouterLink>
          <RouterLink to="/news" class="nav-item">Tin tức</RouterLink>
        </nav>

        <div class="user-actions">
          <button @click="logout" class="btn-logout">
            <span class="icon">🚪</span> Đăng xuất
          </button>
        </div>
      </div>
    </header>
    <main class="main-content">
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
#app {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  background-color: #f8f9fa;
  min-height: 100vh;
}

.navbar {
  background: white;
  box-shadow: 0 2px 10px rgba(0,0,0,0.05);
  position: sticky;
  top: 0;
  z-index: 1000;
  padding: 0 20px;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 70px;
}

.brand {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 24px;
  font-weight: 800;
  color: #42b883;
  user-select: none;
}

.logo-icon {
  font-size: 28px;
}

.logo-text {
  background: linear-gradient(135deg, #42b883 0%, #35495e 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.nav-links {
  display: flex;
  gap: 5px;
}

.nav-item {
  text-decoration: none;
  color: #555;
  font-weight: 600;
  padding: 8px 12px;
  border-radius: 8px;
  transition: all 0.2s ease;
  font-size: 15px;
  white-space: nowrap;
}

.nav-item:hover {
  background-color: #f0f9ff;
  color: #42b883;
}

.nav-item.router-link-active {
  background-color: #e6fffa;
  color: #059669;
}

.btn-logout {
  background: white;
  border: 1px solid #fee2e2;
  color: #ef4444;
  padding: 8px 16px;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 6px;
  transition: all 0.2s;
  font-size: 14px;
}

.btn-logout:hover {
  background: #fee2e2;
  border-color: #ef4444;
}

.main-content {
  padding: 20px;
  min-height: calc(100vh - 70px);
}

@media (max-width: 1024px) {
  .nav-item {
    padding: 8px 8px;
    font-size: 14px;
  }
}

@media (max-width: 900px) {
  .container {
    flex-direction: column;
    height: auto;
    padding: 15px 0;
    gap: 15px;
  }
  
  .nav-links {
    flex-wrap: wrap;
    justify-content: center;
  }
  
  .navbar {
    position: relative;
  }
}
</style>