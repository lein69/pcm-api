<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useDataStore } from '../stores/data'
import axios from 'axios'

const dataStore = useDataStore()

interface Challenge {
  id: number
  title: string
  type: string
  status: string
  prizePool?: number
  gameMode?: string
}

onMounted(() => {
  dataStore.fetchChallenges()
})
</script>

<template>
  <div class="challenges">
    <div class="page-header">
      <h2>🏆 Sàn đấu & Giải đấu</h2>
      <p>Tham gia tranh tài và nhận thưởng</p>
    </div>

    <div class="challenges-grid">
      <div v-for="challenge in (dataStore.challenges as Challenge[])" :key="challenge.id" class="challenge-card">
        <div class="card-header">
          <span class="badge-type">{{ challenge.type }}</span>
          <span class="badge-status" :class="challenge.status?.toLowerCase()">{{ challenge.status }}</span>
        </div>
        
        <h3>{{ challenge.title }}</h3>
        
        <div class="prize-section">
          <span class="label">Tổng giải thưởng</span>
          <span class="amount">{{ new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(challenge.prizePool || 0) }}</span>
        </div>

        <div class="info-row">
          <span>🎮 Mode: {{ challenge.gameMode || 'N/A' }}</span>
          <span>👥 Slots: --/--</span>
        </div>

        <button class="btn-join">Tham gia ngay</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.challenges {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.page-header {
  text-align: center;
  margin-bottom: 40px;
}

.challenges-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 25px;
}

.challenge-card {
  background: white;
  border-radius: 15px;
  padding: 25px;
  box-shadow: 0 10px 20px rgba(0,0,0,0.05);
  transition: transform 0.3s;
  border: 1px solid #eee;
  display: flex;
  flex-direction: column;
}

.challenge-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 15px 30px rgba(0,0,0,0.1);
}

.card-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 15px;
}

.badge-type {
  background: #f3f4f6;
  color: #4b5563;
  padding: 4px 8px;
  border-radius: 6px;
  font-size: 12px;
  font-weight: 600;
}

.badge-status {
  padding: 4px 8px;
  border-radius: 6px;
  font-size: 12px;
  font-weight: 600;
}

.badge-status.open { background: #dcfce7; color: #166534; }
.badge-status.ongoing { background: #dbeafe; color: #1e40af; }
.badge-status.finished { background: #f3f4f6; color: #6b7280; }

.challenge-card h3 {
  margin: 0 0 20px 0;
  font-size: 20px;
  color: #1f2937;
}

.prize-section {
  background: linear-gradient(135deg, #fffbeb 0%, #fef3c7 100%);
  padding: 15px;
  border-radius: 10px;
  text-align: center;
  margin-bottom: 20px;
}

.prize-section .label {
  display: block;
  font-size: 12px;
  color: #92400e;
  margin-bottom: 5px;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.prize-section .amount {
  display: block;
  font-size: 24px;
  font-weight: 800;
  color: #b45309;
}

.info-row {
  display: flex;
  justify-content: space-between;
  font-size: 14px;
  color: #6b7280;
  margin-bottom: 20px;
}

.btn-join {
  margin-top: auto;
  width: 100%;
  padding: 12px;
  background: #667eea;
  color: white;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-join:hover {
  background: #5a67d8;
}
</style>