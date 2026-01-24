<script setup lang="ts">
import { useAuthStore } from '../stores/auth'
import { useDataStore } from '../stores/data'
import { onMounted } from 'vue'

const authStore = useAuthStore()
const dataStore = useDataStore()

onMounted(() => {
  // Load some basic data for dashboard
  dataStore.fetchMembers()
  dataStore.fetchCourts()
  dataStore.fetchBookings()
  dataStore.fetchChallenges()
  dataStore.fetchMatches()
})
</script>

<template>
  <div class="home">
    <div class="welcome-header">
      <div class="welcome-content">
        <h1>🎾 Chào mừng đến PCM</h1>
        <h2>Pickleball Club Management System</h2>
        <p class="welcome-text">
          Hệ thống quản lý câu lạc bộ Pickleball toàn diện
        </p>
        <div class="user-info">
          <span class="user-greeting">
            Xin chào, <strong>{{ authStore.user?.email || 'User' }}</strong>!
          </span>
        </div>
      </div>
      <div class="welcome-image">
        <div class="pickleball-icon">🎾</div>
      </div>
    </div>

    <div class="dashboard-grid">
      <div class="stat-card">
        <div class="stat-icon">👥</div>
        <div class="stat-content">
          <h3>{{ dataStore.members.length }}</h3>
          <p>Thành viên</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon">🏓</div>
        <div class="stat-content">
          <h3>{{ dataStore.courts.length }}</h3>
          <p>Sân bóng</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon">📅</div>
        <div class="stat-content">
          <h3>{{ dataStore.bookings.length }}</h3>
          <p>Đặt sân</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon">🏆</div>
        <div class="stat-content">
          <h3>{{ dataStore.challenges.length }}</h3>
          <p>Giải đấu</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon">🎯</div>
        <div class="stat-content">
          <h3>{{ dataStore.matches.length }}</h3>
          <p>Trận đấu</p>
        </div>
      </div>
    </div>

    <div class="quick-actions">
      <h2>🚀 Truy cập nhanh</h2>
      <div class="actions-grid">
        <router-link to="/members" class="action-card">
          <div class="action-icon">👥</div>
          <div class="action-content">
            <h3>Quản lý thành viên</h3>
            <p>Xem và quản lý danh sách thành viên</p>
          </div>
          <div class="action-arrow">→</div>
        </router-link>

        <router-link to="/courts" class="action-card">
          <div class="action-icon">🏓</div>
          <div class="action-content">
            <h3>Quản lý sân bóng</h3>
            <p>Thêm, sửa, xóa thông tin sân</p>
          </div>
          <div class="action-arrow">→</div>
        </router-link>

        <router-link to="/bookings" class="action-card">
          <div class="action-icon">📅</div>
          <div class="action-content">
            <h3>Đặt sân</h3>
            <p>Quản lý lịch đặt sân</p>
          </div>
          <div class="action-arrow">→</div>
        </router-link>

        <router-link to="/challenges" class="action-card">
          <div class="action-icon">🏆</div>
          <div class="action-content">
            <h3>Giải đấu</h3>
            <p>Tổ chức và quản lý giải đấu</p>
          </div>
          <div class="action-arrow">→</div>
        </router-link>

        <router-link to="/matches" class="action-card">
          <div class="action-icon">🎯</div>
          <div class="action-content">
            <h3>Trận đấu</h3>
            <p>Theo dõi kết quả trận đấu</p>
          </div>
          <div class="action-arrow">→</div>
        </router-link>

        <router-link to="/transactions" class="action-card">
          <div class="action-icon">💰</div>
          <div class="action-content">
            <h3>Giao dịch</h3>
            <p>Quản lý thu chi</p>
          </div>
          <div class="action-arrow">→</div>
        </router-link>
      </div>
    </div>

    <div class="info-section">
      <div class="info-card">
        <h3>📚 Thông tin hệ thống</h3>
        <ul>
          <li><strong>Backend:</strong> ASP.NET Core 9.0 Web API</li>
          <li><strong>Frontend:</strong> Vue.js 3 + TypeScript</li>
          <li><strong>Database:</strong> SQL Server với EF Core</li>
          <li><strong>Authentication:</strong> JWT Bearer Token</li>
          <li><strong>Features:</strong> CRUD, Business Logic, Validation</li>
        </ul>
      </div>

      <div class="info-card">
        <h3>🎯 Tính năng chính</h3>
        <ul>
          <li>✅ Quản lý thành viên và xếp hạng</li>
          <li>✅ Đặt sân với validation</li>
          <li>✅ Tổ chức giải đấu và trận đấu</li>
          <li>✅ Quản lý tài chính</li>
          <li>✅ Phân quyền Admin/Member</li>
        </ul>
      </div>
    </div>
  </div>
</template>

<style scoped>
.home {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.welcome-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 15px;
  padding: 40px;
  margin-bottom: 30px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  color: white;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
}

.welcome-content {
  flex: 1;
}

.welcome-content h1 {
  margin: 0 0 10px 0;
  font-size: 36px;
  font-weight: 700;
}

.welcome-content h2 {
  margin: 0 0 20px 0;
  font-size: 24px;
  font-weight: 500;
  opacity: 0.9;
}

.welcome-text {
  font-size: 18px;
  margin-bottom: 20px;
  opacity: 0.9;
}

.user-info {
  margin-top: 15px;
}

.user-greeting {
  background: rgba(255, 255, 255, 0.2);
  padding: 10px 15px;
  border-radius: 25px;
  display: inline-block;
  font-size: 16px;
}

.welcome-image {
  flex-shrink: 0;
  margin-left: 40px;
}

.pickleball-icon {
  font-size: 120px;
  animation: bounce 2s infinite;
}

@keyframes bounce {
  0%, 20%, 50%, 80%, 100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-10px);
  }
  60% {
    transform: translateY(-5px);
  }
}

.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 20px;
  margin-bottom: 40px;
}

.stat-card {
  background: white;
  border-radius: 12px;
  padding: 25px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  display: flex;
  align-items: center;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
}

.stat-icon {
  font-size: 40px;
  margin-right: 20px;
}

.stat-content h3 {
  margin: 0;
  font-size: 32px;
  font-weight: 700;
  color: #333;
}

.stat-content p {
  margin: 5px 0 0 0;
  color: #666;
  font-size: 14px;
}

.quick-actions {
  margin-bottom: 40px;
}

.quick-actions h2 {
  text-align: center;
  color: #333;
  margin-bottom: 30px;
  font-size: 28px;
  font-weight: 600;
}

.actions-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 20px;
}

.action-card {
  background: white;
  border-radius: 12px;
  padding: 25px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  display: flex;
  align-items: center;
  text-decoration: none;
  color: inherit;
  transition: all 0.3s ease;
}

.action-card:hover {
  transform: translateY(-3px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
  border-left: 5px solid #667eea;
}

.action-icon {
  font-size: 32px;
  margin-right: 20px;
  width: 50px;
  text-align: center;
}

.action-content {
  flex: 1;
}

.action-content h3 {
  margin: 0 0 8px 0;
  color: #333;
  font-size: 18px;
  font-weight: 600;
}

.action-content p {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.action-arrow {
  font-size: 20px;
  color: #667eea;
  transition: transform 0.3s ease;
}

.action-card:hover .action-arrow {
  transform: translateX(5px);
}

.info-section {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 30px;
}

.info-card {
  background: white;
  border-radius: 12px;
  padding: 30px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
}

.info-card h3 {
  color: #333;
  margin-bottom: 20px;
  font-size: 20px;
  font-weight: 600;
}

.info-card ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.info-card li {
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
  color: #555;
}

.info-card li:last-child {
  border-bottom: none;
}

.info-card li strong {
  color: #333;
}

@media (max-width: 768px) {
  .welcome-header {
    flex-direction: column;
    text-align: center;
    padding: 30px 20px;
  }

  .welcome-image {
    margin-left: 0;
    margin-top: 20px;
  }

  .pickleball-icon {
    font-size: 80px;
  }

  .dashboard-grid {
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  }

  .actions-grid {
    grid-template-columns: 1fr;
  }

  .info-section {
    grid-template-columns: 1fr;
  }
}
</style>