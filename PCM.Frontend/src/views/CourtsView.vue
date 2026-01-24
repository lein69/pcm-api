<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useDataStore } from '../stores/data'
import axios from 'axios'

const dataStore = useDataStore()
const newCourt = ref({ name: '', description: '' })

interface Court {
  id: number
  name: string
  description: string
}

onMounted(() => {
  dataStore.fetchCourts()
})

const createCourt = async () => {
  try {
    await axios.post('http://localhost:5000/api/courts', newCourt.value)
    dataStore.fetchCourts()
    newCourt.value = { name: '', description: '' }
  } catch (error) {
    console.error('Failed to create court')
  }
}
</script>

<template>
  <div class="courts">
    <div class="page-header">
      <h2>🏓 Quản lý Sân bóng</h2>
      <p>Danh sách và trạng thái các sân Pickleball</p>
    </div>

    <div class="create-section">
      <form @submit.prevent="createCourt" class="inline-form">
        <div class="form-group">
          <input v-model="newCourt.name" placeholder="Tên sân (VD: Sân 1)" required class="form-input" />
        </div>
        <div class="form-group">
          <input v-model="newCourt.description" placeholder="Mô tả (VD: Sân ngoài trời)" class="form-input" />
        </div>
        <button type="submit" class="btn-add">
          <span>+</span> Thêm sân
        </button>
      </form>
    </div>

    <div class="courts-grid">
      <div v-for="court in (dataStore.courts as Court[])" :key="court.id" class="court-card">
        <div class="court-icon">🏟️</div>
        <div class="court-info">
          <h3>{{ court.name }}</h3>
          <p>{{ court.description || 'Chưa có mô tả' }}</p>
          <div class="court-status active">
            Đang hoạt động
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.courts {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.page-header {
  text-align: center;
  margin-bottom: 30px;
}

.create-section {
  background: white;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  margin-bottom: 30px;
}

.inline-form {
  display: flex;
  gap: 15px;
  align-items: center;
  flex-wrap: wrap;
}

.form-input {
  padding: 10px 15px;
  border: 1px solid #ddd;
  border-radius: 8px;
  min-width: 250px;
  flex: 1;
}

.btn-add {
  background-color: #42b883;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 8px;
  cursor: pointer;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 5px;
}

.courts-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 20px;
}

.court-card {
  background: white;
  border-radius: 12px;
  padding: 25px;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
  display: flex;
  align-items: center;
  gap: 20px;
  transition: transform 0.2s;
}

.court-card:hover {
  transform: translateY(-5px);
}

.court-icon {
  font-size: 40px;
  background: #f0f9ff;
  width: 70px;
  height: 70px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
}

.court-info h3 {
  margin: 0 0 5px 0;
  color: #2c3e50;
}

.court-info p {
  margin: 0 0 10px 0;
  color: #666;
  font-size: 14px;
}

.court-status.active {
  color: #059669;
  font-size: 12px;
  font-weight: bold;
  background: #ecfdf5;
  padding: 2px 8px;
  border-radius: 10px;
  display: inline-block;
}
</style>