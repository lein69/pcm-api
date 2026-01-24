<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useDataStore } from '../stores/data'

const dataStore = useDataStore()

interface Member {
  id: number
  fullName: string
  email: string
  joinDate?: string
  rankLevel?: number
}

onMounted(() => {
  dataStore.fetchMembers()
})
</script>

<template>
  <div class="members">
    <div class="page-header">
      <h2>👥 Danh sách Hội viên</h2>
      <p>Quản lý thông tin và xếp hạng thành viên CLB</p>
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
            </tr>
          </tbody>
        </table>
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
</style>