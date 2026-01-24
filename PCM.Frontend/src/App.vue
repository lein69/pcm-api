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
    <header v-if="authStore.isAuthenticated">
      <nav>
        <RouterLink to="/">Home</RouterLink> |
        <RouterLink to="/members">Members</RouterLink> |
        <RouterLink to="/courts">Courts</RouterLink> |
        <RouterLink to="/bookings">Bookings</RouterLink> |
        <RouterLink to="/challenges">Challenges</RouterLink> |
        <RouterLink to="/matches">Matches</RouterLink> |
        <RouterLink to="/transactions">Transactions</RouterLink> |
        <RouterLink to="/news">News</RouterLink> |
        <button @click="logout">Logout</button>
      </nav>
    </header>
    <main>
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b883;
}
</style>