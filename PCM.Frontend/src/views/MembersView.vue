<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useDataStore } from '../stores/data'
import api from "@/api"

const dataStore = useDataStore()

interface Member {
  id: number
  fullName: string
  email: string
  phoneNumber?: string
  joinDate?: string
  rankLevel?: number
}

onMounted(() => {
  dataStore.fetchMembers()
})

// --- Logic Quản lý Modal & Form ---
const showModal = ref(false)
const isEditing = ref(false)
const isSubmitting = ref(false)

const formData = reactive({
  id: 0,
  fullName: '',
  email: '',
  phoneNumber: '',
  rankLevel: 3.0
})

const resetForm = () => {
  formData.id = 0
  formData.fullName = ''
  formData.email = ''
  formData.phoneNumber = ''
  formData.rankLevel = 3.0
}

const openAddModal = () => {
  isEditing.value = false
  resetForm()
  showModal.value = true
}

const openEditModal = (member: Member) => {
  isEditing.value = true
  formData.id = member.id
  formData.fullName = member.fullName
  formData.email = member.email
  formData.phoneNumber = member.phoneNumber || ''
  formData.rankLevel = member.rankLevel || 3.0
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
      // Gọi API Sửa
      await api.put(`/members/${formData.id}`, formData)
      alert('Cập nhật thành công!')
    } else {
      // Gọi API Thêm mới
      // Lưu ý: Backend cần endpoint POST /members xử lý việc tạo Identity User + Member
      await api.post('/members', formData)
      alert('Thêm hội viên thành công!')
    }
    
    closeModal()
    dataStore.fetchMembers() // Load lại danh sách
  } catch (error) {
    console.error(error)
    alert('Có lỗi xảy ra! Vui lòng kiểm tra lại.')
  } finally {
    isSubmitting.value = false
  }
}

const handleDelete = async (id: number) => {
  if (!confirm('Bạn có chắc chắn muốn xóa hội viên này? Hành động này không thể hoàn tác.')) return

  try {
    await api.delete(`/members/${id}`)
    alert('Đã xóa hội viên.')
    dataStore.fetchMembers()
  } catch (error) {
    console.error(error)
    alert('Không thể xóa hội viên này.')
  }
}
</script>

<template>
  <div class="members">
    <div class="page-header">
      <h2>👥 Danh sách Hội viên</h2>
      <p>Quản lý thông tin và xếp hạng thành viên CLB</p>
      <button class="btn-add" @click="openAddModal">
        + Thêm hội viên
      </button>
    </div>
    
    <div class="card">
      <div class="table-responsive">
        <table class="table">
          <thead>
            <tr>
              <th>Họ và tên</th>
              <th>Email</th>
              <th>Ngày tham gia</th>
              <th>Rank DUPR</th>
              <th>Trạng thái</th>
              <th class="text-center">Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="member in (dataStore.members as Member[])" :key="member.id">
              <td>
                <div class="member-info">
                  <div class="avatar">{{ member.fullName.charAt(0) }}</div>
                  <span class="name">{{ member.fullName }}</span>
                </div>
              </td>
              <td>{{ member.email }}</td>
              <td>{{ new Date(member.joinDate || Date.now()).toLocaleDateString('vi-VN') }}</td>
              <td>
                <span class="rank-badge">{{ member.rankLevel?.toFixed(2) || 'N/A' }}</span>
              </td>
              <td>
                <span class="status-active">Hoạt động</span>
              </td>
              <td class="text-center">
                <button class="btn-icon btn-edit" @click="openEditModal(member)" title="Sửa">✏️</button>
                <button class="btn-icon btn-delete" @click="handleDelete(member.id)" title="Xóa">🗑️</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- MODAL FORM -->
    <div v-if="showModal" class="modal-overlay" @click.self="closeModal">
      <div class="modal-content">
        <div class="modal-header">
          <h3>{{ isEditing ? 'Cập nhật thông tin' : 'Thêm hội viên mới' }}</h3>
          <button class="btn-close" @click="closeModal">×</button>
        </div>
        
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label>Họ và tên:</label>
            <input v-model="formData.fullName" type="text" required class="form-control" placeholder="Nhập họ tên..." />
          </div>

          <div class="form-group">
            <label>Email:</label>
            <input v-model="formData.email" type="email" required class="form-control" :disabled="isEditing" placeholder="example@pcm.com" />
            <small v-if="isEditing" class="text-muted">Email không thể thay đổi</small>
          </div>

          <div class="form-group">
            <label>Số điện thoại:</label>
            <input v-model="formData.phoneNumber" type="tel" class="form-control" placeholder="09xxxx..." />
          </div>

          <div class="form-group">
            <label>Rank DUPR:</label>
            <input v-model.number="formData.rankLevel" type="number" step="0.01" min="1" max="7" class="form-control" />
          </div>

          <div class="modal-actions">
            <button type="button" class="btn-secondary" @click="closeModal">Hủy</button>
            <button type="submit" class="btn-primary" :disabled="isSubmitting">
              {{ isSubmitting ? 'Đang lưu...' : 'Lưu thông tin' }}
            </button>
          </div>
        </form>
      </div>
    </div>

  </div>
</template>

<style scoped>
.members {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.page-header {
  margin-bottom: 30px;
  text-align: center;
  position: relative;
}

.btn-add {
  background-color: #42b883;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
  position: absolute;
  right: 0;
  top: 0;
  transition: background 0.2s;
}

.btn-add:hover {
  background-color: #3aa876;
}

.card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  padding: 15px 20px;
  text-align: left;
  border-bottom: 1px solid #eee;
}

th {
  background-color: #f8f9fa;
  font-weight: 600;
  color: #2c3e50;
}

.text-center {
  text-align: center;
}

.member-info {
  display: flex;
  align-items: center;
  gap: 10px;
}

.avatar {
  width: 35px;
  height: 35px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
}

.rank-badge {
  background-color: #e0f2fe;
  color: #0369a1;
  padding: 4px 8px;
  border-radius: 12px;
  font-weight: bold;
  font-size: 14px;
}

.status-active {
  color: #059669;
  font-size: 14px;
  display: flex;
  align-items: center;
  gap: 5px;
}

.status-active::before {
  content: "";
  display: block;
  width: 8px;
  height: 8px;
  background-color: #059669;
  border-radius: 50%;
}

/* BUTTONS */
.btn-icon {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 18px;
  margin: 0 5px;
  transition: transform 0.2s;
}

.btn-icon:hover {
  transform: scale(1.2);
}

.btn-edit { color: #f59e0b; }
.btn-delete { color: #ef4444; }

/* MODAL STYLES */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 25px;
  border-radius: 12px;
  width: 100%;
  max-width: 500px;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.modal-header h3 {
  margin: 0;
  color: #1f2937;
}

.btn-close {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #6b7280;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 500;
  color: #374151;
}

.form-control {
  width: 100%;
  padding: 10px;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  box-sizing: border-box; /* Fix input width overflow */
}

.text-muted {
  font-size: 12px;
  color: #6b7280;
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 25px;
}

.btn-secondary {
  padding: 10px 20px;
  background: #f3f4f6;
  color: #374151;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
}

.btn-primary {
  padding: 10px 20px;
  background: #42b883;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
}

.btn-primary:disabled {
  background: #a7f3d0;
  cursor: not-allowed;
}
</style>