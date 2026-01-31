<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useDataStore } from '../stores/data'
import api from "@/api";


const dataStore = useDataStore()

interface Challenge {
  id: number
  title: string
  type: string
  status: string
  prizePool?: number
  gameMode?: string
  entryFee?: number
  config_TargetWins?: number
}

onMounted(() => {
  dataStore.fetchChallenges()
})

// --- Admin Logic ---
const showModal = ref(false)
const isEditing = ref(false)
const isSubmitting = ref(false)

const formData = reactive({
  id: 0,
  title: '',
  type: 'MiniGame',
  gameMode: 'TeamBattle',
  status: 'Open',
  entryFee: 50000,
  prizePool: 0,
  config_TargetWins: 5
})

const resetForm = () => {
  formData.id = 0
  formData.title = ''
  formData.type = 'MiniGame'
  formData.gameMode = 'TeamBattle'
  formData.status = 'Open'
  formData.entryFee = 50000
  formData.prizePool = 0
  formData.config_TargetWins = 5
}

const openAddModal = () => {
  isEditing.value = false
  resetForm()
  showModal.value = true
}

const openEditModal = (challenge: Challenge) => {
  isEditing.value = true
  formData.id = challenge.id
  formData.title = challenge.title
  formData.type = challenge.type
  formData.status = challenge.status
  // Map các trường khác nếu có trong API response
  formData.entryFee = (challenge as any).entryFee || 0
  formData.prizePool = challenge.prizePool || 0
  formData.gameMode = challenge.gameMode || 'None'
  showModal.value = true
}

const closeModal = () => {
  showModal.value = false
  resetForm()
}

const handleSubmit = async () => {
  if (isSubmitting.value) return
  isSubmitting.value = true
  try {
    if (isEditing.value) {
      await api.put(`/challenges/${formData.id}`, formData)
      alert('Cập nhật giải đấu thành công!')
    } else {
      await api.post('/challenges', formData)
      alert('Tạo giải đấu thành công!')
    }
    closeModal()
    dataStore.fetchChallenges()
  } catch (e) {
    console.error(e)
    alert('Có lỗi xảy ra.')
  } finally {
    isSubmitting.value = false
  }
}

const handleDelete = async (id: number) => {
  if(!confirm('Bạn có chắc muốn xóa giải đấu này?')) return
  try {
    await api.delete(`/challenges/${id}`)
    dataStore.fetchChallenges()
  } catch(e) {
    console.error(e)
    alert('Không thể xóa.')
  }
}
</script>

<template>
  <div class="challenges">
    <div class="page-header">
      <h2>🏆 Sàn đấu & Giải đấu</h2>
      <p>Tham gia tranh tài và nhận thưởng</p>
      <button class="btn-add" @click="openAddModal">+ Tạo giải đấu</button>
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
        
        <div class="admin-actions">
          <button class="btn-icon" @click="openEditModal(challenge)">✏️</button>
          <button class="btn-icon delete" @click="handleDelete(challenge.id)">🗑️</button>
        </div>
      </div>
    </div>

    <!-- Modal -->
    <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal-content">
        <div class="modal-header">
          <h3>{{ isEditing ? 'Sửa giải đấu' : 'Tạo giải đấu mới' }}</h3>
          <button class="btn-close" @click="closeModal">×</button>
        </div>
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label>Tên giải:</label>
            <input v-model="formData.title" class="form-control" required />
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Loại:</label>
              <select v-model="formData.type" class="form-control">
                <option value="Duel">Duel (Thách đấu)</option>
                <option value="MiniGame">Mini Game</option>
              </select>
            </div>
            <div class="form-group">
              <label>Chế độ:</label>
              <select v-model="formData.gameMode" class="form-control">
                <option value="None">None</option>
                <option value="TeamBattle">Team Battle</option>
                <option value="RoundRobin">Round Robin</option>
              </select>
            </div>
          </div>
          <div class="form-row">
             <div class="form-group">
              <label>Phí tham gia:</label>
              <input v-model.number="formData.entryFee" type="number" class="form-control" />
            </div>
            <div class="form-group">
              <label>Trạng thái:</label>
              <select v-model="formData.status" class="form-control">
                <option value="Open">Mở đăng ký</option>
                <option value="Ongoing">Đang diễn ra</option>
                <option value="Finished">Kết thúc</option>
              </select>
            </div>
          </div>
          <button type="submit" class="btn-primary full-width">Lưu</button>
        </form>
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
  position: relative;
}

.btn-add {
  position: absolute;
  right: 0;
  top: 0;
  background: #42b883;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 600;
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

.admin-actions {
  margin-top: 10px;
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  border-top: 1px solid #eee;
  padding-top: 10px;
}

.btn-icon {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 16px;
}
.btn-icon.delete { color: red; }

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0; left: 0; width: 100%; height: 100%;
  background: rgba(0,0,0,0.5);
  display: flex; justify-content: center; align-items: center;
  z-index: 1000;
}
.modal-content {
  background: white; padding: 25px; border-radius: 12px;
  width: 90%; max-width: 500px;
}
.modal-header {
  display: flex; justify-content: space-between; margin-bottom: 20px;
}
.btn-close {
  background: none; border: none; font-size: 24px; cursor: pointer;
}
.form-group { margin-bottom: 15px; }
.form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 15px; }
.form-control {
  width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 6px;
  box-sizing: border-box;
}
.btn-primary {
  background: #42b883; color: white; border: none; padding: 12px;
  border-radius: 6px; cursor: pointer; font-weight: bold;
}
.full-width { width: 100%; }
</style>