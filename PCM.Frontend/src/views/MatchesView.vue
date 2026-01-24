<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useDataStore } from '../stores/data'
import axios from 'axios'

const dataStore = useDataStore()

interface Match {
  id: number
  date: string
  format: number
  winningSide: number
  team1Score: number
  team2Score: number
}

onMounted(() => {
  dataStore.fetchMatches()
})
</script>

<template>
  <div class="matches">
    <div class="page-header">
      <h2>🎯 Kết quả Trận đấu</h2>
      <p>Lịch sử các trận tranh tài nảy lửa</p>
    </div>

    <div class="matches-list">
      <div v-for="match in (dataStore.matches as Match[])" :key="match.id" class="match-card">
        <div class="match-info">
          <span class="match-date">{{ new Date(match.date || Date.now()).toLocaleDateString('vi-VN') }}</span>
          <span class="match-format">{{ match.format === 1 ? 'Đơn (1vs1)' : 'Đôi (2vs2)' }}</span>
        </div>
        
        <div class="scoreboard">
          <div class="team team-1" :class="{ winner: match.winningSide === 1 }">
            <span class="team-name">Team 1</span>
            <span class="score">{{ match.team1Score || 0 }}</span>
          </div>
          <div class="vs">VS</div>
          <div class="team team-2" :class="{ winner: match.winningSide === 2 }">
            <span class="score">{{ match.team2Score || 0 }}</span>
            <span class="team-name">Team 2</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.matches {
  padding: 20px;
  max-width: 800px;
  margin: 0 auto;
}

.page-header {
  text-align: center;
  margin-bottom: 30px;
}

.match-card {
  background: white;
  border-radius: 12px;
  padding: 20px;
  margin-bottom: 15px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  border-left: 5px solid #ccc;
}

.match-info {
  display: flex;
  justify-content: space-between;
  color: #666;
  font-size: 14px;
  margin-bottom: 15px;
}

.scoreboard {
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-weight: bold;
}

.team {
  display: flex;
  align-items: center;
  gap: 15px;
  font-size: 18px;
}

.team.winner {
  color: #059669;
}

.team.winner .score {
  background: #059669;
  color: white;
}

.score {
  background: #f3f4f6;
  padding: 5px 15px;
  border-radius: 8px;
  min-width: 30px;
  text-align: center;
}

.vs {
  color: #9ca3af;
  font-size: 14px;
  font-style: italic;
}
</style>