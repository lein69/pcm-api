<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useDataStore } from '../stores/data'
import apiClient from '../api' // Giả sử bạn tạo file api.ts

const dataStore = useDataStore()

interface Court {
  id: number
  name: string
  description: string
}

interface Booking {
  id: number
  startTime: string
  endTime: string
  notes: string
  status: string
  court?: Court
  member?: { fullName: string }
}

const isSubmitting = ref(false)

const newBooking = ref({
  courtId: 0,
  memberId: 1, // Assume current user
  startTime: '',
  endTime: '',
  notes: ''
})

onMounted(() => {
  dataStore.fetchBookings()
  dataStore.fetchCourts()
})

const createBooking = async () => {
  if (isSubmitting.value) return

  isSubmitting.value = true
  try {
    await apiClient.post('/bookings', {
      ...newBooking.value,
      startTime: new Date(newBooking.value.startTime),
      endTime: new Date(newBooking.value.endTime)
    })
    dataStore.fetchBookings()
    // Reset form
    newBooking.value = {
      courtId: 0,
      memberId: 1,
      startTime: '',
      endTime: '',
      notes: ''
    }
  } catch (error) {
    console.error('Failed to create booking:', error)
    alert('Có lỗi xảy ra khi đặt sân. Vui lòng thử lại.')
  } finally {
    isSubmitting.value = false
  }
}

const formatDateTime = (dateString: string) => {
  return new Date(dateString).toLocaleString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const getStatusClass = (status: string) => {
  switch (status?.toLowerCase()) {
    case 'confirmed':
    case 'active':
      return 'status-confirmed'
    case 'pending':
      return 'status-pending'
    case 'cancelled':
      return 'status-cancelled'
    default:
      return 'status-default'
  }
}

const getStatusText = (status: string) => {
  switch (status?.toLowerCase()) {
    case 'confirmed':
    case 'active':
      return 'Đã xác nhận'
    case 'pending':
      return 'Chờ xác nhận'
    case 'cancelled':
      return 'Đã hủy'
    default:
      return status || 'Không xác định'
  }
}
</script>

<template>
  <div class="bookings-view">
    <div class="header">
      <h1>📅 Quản lý đặt sân</h1>
      <p>Đặt sân và quản lý lịch đặt sân của câu lạc bộ</p>
    </div>

    <div class="content-grid">
      <!-- Create Booking Form -->
      <div class="form-card">
        <h2>Đặt sân mới</h2>
        <form @submit.prevent="createBooking" class="booking-form">
          <div class="form-group">
            <label for="court">Chọn sân:</label>
            <select v-model="newBooking.courtId" id="court" required class="form-control">
              <option value="0" disabled>Chọn sân</option>
              <option v-for="court in (dataStore.courts as Court[])" :key="court.id" :value="court.id">
                {{ court.name }} - {{ court.description }}
              </option>
            </select>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="startTime">Thời gian bắt đầu:</label>
              <input v-model="newBooking.startTime" id="startTime" type="datetime-local" required class="form-control" />
            </div>
            <div class="form-group">
              <label for="endTime">Thời gian kết thúc:</label>
              <input v-model="newBooking.endTime" id="endTime" type="datetime-local" required class="form-control" />
            </div>
          </div>

          <div class="form-group">
            <label for="notes">Ghi chú:</label>
            <textarea v-model="newBooking.notes" id="notes" placeholder="Nhập ghi chú (tùy chọn)" class="form-control" rows="3"></textarea>
          </div>

          <button type="submit" class="btn-primary" :disabled="isSubmitting">
            <span v-if="isSubmitting">Đang xử lý...</span>
            <span v-else>Đặt sân</span>
          </button>
        </form>
      </div>

      <!-- Bookings List -->
      <div class="list-card">
        <h2>Danh sách đặt sân</h2>
        <div v-if="dataStore.bookings.length === 0" class="empty-state">
          <div class="empty-icon">📅</div>
          <p>Chưa có đặt sân nào</p>
        </div>
        <div v-else class="bookings-list">
          <div v-for="booking in (dataStore.bookings as Booking[])" :key="booking.id" class="booking-item">
            <div class="booking-header">
              <div class="court-info">
                <strong>{{ booking.court?.name }}</strong>
                <span class="court-desc">{{ booking.court?.description }}</span>
              </div>
              <div class="booking-status" :class="getStatusClass(booking.status)">
                {{ getStatusText(booking.status) }}
              </div>
            </div>
            <div class="booking-details">
              <div class="time-info">
                <span class="time-label">Thời gian:</span>
                <span>{{ formatDateTime(booking.startTime) }} - {{ formatDateTime(booking.endTime) }}</span>
              </div>
              <div class="member-info" v-if="booking.member">
                <span class="member-label">Người đặt:</span>
                <span>{{ booking.member.fullName }}</span>
              </div>
              <div class="notes" v-if="booking.notes">
                <span class="notes-label">Ghi chú:</span>
                <span>{{ booking.notes }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.bookings-view {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.header {
  margin-bottom: 30px;
  text-align: center;
}

.header h1 {
  color: #2c3e50;
  margin-bottom: 10px;
}

.content-grid {
  display: grid;
  grid-template-columns: 1fr 2fr;
  gap: 30px;
}

@media (max-width: 768px) {
  .content-grid {
    grid-template-columns: 1fr;
  }
}

.form-card, .list-card {
  background: white;
  padding: 25px;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  height: fit-content;
}

.form-card h2, .list-card h2 {
  margin-top: 0;
  margin-bottom: 20px;
  color: #2c3e50;
  border-bottom: 2px solid #f0f0f0;
  padding-bottom: 10px;
}

.form-group {
  margin-bottom: 15px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: 500;
  color: #666;
}

.form-control {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 14px;
  transition: border-color 0.3s;
  box-sizing: border-box;
}

.form-control:focus {
  border-color: #42b883;
  outline: none;
}

.btn-primary {
  width: 100%;
  padding: 12px;
  background-color: #42b883;
  color: white;
  border: none;
  border-radius: 6px;
  font-weight: bold;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-primary:hover:not(:disabled) {
  background-color: #3aa876;
}

.btn-primary:disabled {
  background-color: #a8d5c2;
  cursor: not-allowed;
}

.booking-item {
  border: 1px solid #eee;
  border-radius: 8px;
  padding: 15px;
  margin-bottom: 15px;
  transition: transform 0.2s;
}

.booking-item:hover {
  transform: translateY(-2px);
  box-shadow: 0 2px 5px rgba(0,0,0,0.05);
}

.booking-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 10px;
}

.court-info strong {
  display: block;
  font-size: 16px;
  color: #2c3e50;
}

.court-desc {
  font-size: 12px;
  color: #888;
}

.booking-status {
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: bold;
}

.status-confirmed { background-color: #e6fffa; color: #047857; }
.status-pending { background-color: #fffbeb; color: #b45309; }
.status-cancelled { background-color: #fef2f2; color: #b91c1c; }
.status-default { background-color: #f3f4f6; color: #4b5563; }

.booking-details {
  font-size: 14px;
  color: #555;
}

.booking-details > div {
  margin-bottom: 5px;
}

.empty-state {
  text-align: center;
  padding: 40px;
  color: #888;
}

.empty-icon {
  font-size: 48px;
  margin-bottom: 10px;
}
</style>